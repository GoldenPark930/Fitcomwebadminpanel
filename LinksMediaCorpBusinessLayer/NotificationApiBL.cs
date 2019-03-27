
namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;

    public class NotificationApiBL
    {
        static object syncLock = new object();
        /// <summary>
        /// Save or Update User Notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SaveUpdateNotification(UserNotificationSettingVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                tblUserNotificationSetting objtblUserNotification = null;
                tblUserNotificationSetting objUserNotification = null;
                Credentials objCred = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SaveUpdateNotification()");
                    objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    string notificationType = string.Empty;
                    string deviceType = string.Empty;
                    notificationType = Convert.ToString(model.NotificationType);
                    deviceType = Convert.ToString(model.DeviceType);
                    objUserNotification = dataContext.UserNotificationSetting.FirstOrDefault(u => u.DeviceID == model.DeviceID
                        && u.NotificationType == notificationType && u.UserCredId == objCred.Id && u.DeviceType == deviceType);
                    if (objUserNotification != null)
                    {
                        objUserNotification.IsNotify = model.IsNotify;
                        objUserNotification.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        objtblUserNotification = new tblUserNotificationSetting();
                        objtblUserNotification.UserCredId = objCred.Id;
                        objtblUserNotification.NotificationType = notificationType;
                        objtblUserNotification.IsNotify = model.IsNotify;
                        objtblUserNotification.DeviceID = string.IsNullOrEmpty(model.DeviceID) ? "" : model.DeviceID;
                        objtblUserNotification.DeviceType = string.IsNullOrEmpty(deviceType) ? "" : deviceType;
                        objtblUserNotification.CreatedDate = DateTime.Now;
                        objtblUserNotification.ModifiedDate = DateTime.Now;
                        dataContext.UserNotificationSetting.Add(objtblUserNotification);
                    }
                    dataContext.SaveChanges();

                    return true;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : SaveUpdateNotification()  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objtblUserNotification = null;
                    objUserNotification = null;
                    objCred = null;
                }
            }
        }
        /// <summary>
        /// Reset the User Notification based on user credtails
        /// </summary>
        /// <param name="userCredID"></param>
        /// <returns></returns>
        public static bool ResetUserNotification(int userCredID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ResetUserNotification()");
                    IQueryable<tblUserNotificationSetting> objUserNotification = dataContext.UserNotificationSetting.Where(u => u.UserCredId == userCredID);
                    foreach (var nt in objUserNotification)
                    {
                        nt.IsNotify = false;
                        nt.ModifiedDate = DateTime.UtcNow;
                    }
                    dataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : ResetUserNotification()  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get UserNotificationList from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<UserNotificationsVM>> GetUserNotificationList(int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                string notificationMessage = string.Empty;
                Total<List<UserNotificationsVM>> totalNotificationList = null;
                List<UserNotificationsVM> userNotificationList = null;
                NotificationType objNotificationType;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserNotificationList()");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<UserNotificationsVM> userNotifications = (from un in dataContext.UserNotifications
                                                                   join ct in dataContext.Credentials on un.ReceiverCredID equals ct.Id
                                                                   where un.ReceiverCredID > 0 && un.SenderCredlID > 0 && un.ReceiverCredID == cred.Id
                                                                   orderby un.CreatedDate descending
                                                                   select new UserNotificationsVM
                                                                   {
                                                                       UserNotificationID = un.NotificationID,
                                                                       UserID = dataContext.Credentials.Where(uc => uc.Id == un.SenderCredlID).Select(y => y.UserId).FirstOrDefault(),
                                                                       UserType = dataContext.Credentials.Where(uc => uc.Id == un.SenderCredlID).Select(y => y.UserType).FirstOrDefault(),
                                                                       NotificationType = un.NotificationType,
                                                                       CreatedNotificationUtcDateTime = un.CreatedDate,
                                                                       Status = un.Status,
                                                                       IsRead = un.IsRead,
                                                                       TargetPostID = un.TargetID,
                                                                       ReceiverCredID = un.ReceiverCredID,
                                                                       SenderCredID = un.SenderCredlID,
                                                                       SenderUserName = un.SenderUserName,
                                                                       DevicesTokenID = un.TokenDevicesID,
                                                                       IsFriendChallenge = un.IsFriendChallenge,
                                                                       IsOnBoarding = un.IsOnBoarding
                                                                   }).ToList();

                    userNotifications = userNotifications.GroupBy(cr => new { cr.SenderCredID, cr.NotificationType, cr.TargetPostID, cr.CreatedNotificationUtcDateTime })
                                                         .Select(usr => usr.FirstOrDefault()).ToList();
                    if (userNotifications != null)
                    {
                        // fill the sender  full name and image url
                        userNotificationList = new List<UserNotificationsVM>();
                        for (int count = 0; count < userNotifications.Count; count++)
                        {
                            UserNotificationsVM userNt = userNotifications[count];
                            if (userNt != null && userNt.UserID > 0)
                            {
                                UserNotificationsVM objUserNotificationsVM = new UserNotificationsVM();
                                UserVM senderDetails = (!string.IsNullOrEmpty(userNt.UserType) && userNt.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)) ?
                                                          (from usr in dataContext.User
                                                           join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                           where userNt.UserID > 0 && crd.UserType == Message.UserTypeUser && usr.UserId == userNt.UserID
                                                           select new UserVM
                                                           {
                                                               CredUserId = crd.Id,
                                                               FullName = usr.FirstName + " " + usr.LastName,
                                                               ImageUrl = usr.UserImageUrl,
                                                               SenderEmailID = crd.EmailId
                                                           }).FirstOrDefault() :
                                                           (!string.IsNullOrEmpty(userNt.UserType) && userNt.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)) ?
                                                          (from trainer in dataContext.Trainer
                                                           join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                           where userNt.UserID > 0 && crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == userNt.UserID
                                                           select new UserVM
                                                           {
                                                               CredUserId = crd.Id,
                                                               FullName = trainer.FirstName + " " + trainer.LastName,
                                                               ImageUrl = trainer.TrainerImageUrl,
                                                               SenderEmailID = crd.EmailId
                                                           }).FirstOrDefault() : null;
                                if (senderDetails != null)
                                {
                                    objUserNotificationsVM.ImageURL = string.IsNullOrEmpty(senderDetails.ImageUrl) ? string.Empty :
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + senderDetails.ImageUrl;
                                    objUserNotificationsVM.SenderUserName = senderDetails.FullName;
                                    objUserNotificationsVM.SenderEmailID = senderDetails.SenderEmailID;
                                }
                                if (!Enum.TryParse(userNt.NotificationType, out objNotificationType))
                                {
                                    notificationMessage = string.Empty;
                                }
                                if (objNotificationType == NotificationType.TrainerJoinTeam)
                                {
                                    objUserNotificationsVM.TeamName = userNt.TargetPostID > 0 ? dataContext.Teams.Where(t => t.TeamId == userNt.TargetPostID).Select(tt => tt.TeamName).FirstOrDefault() : string.Empty;
                                    if (!string.IsNullOrEmpty(objUserNotificationsVM.TeamName))
                                    {
                                        objUserNotificationsVM.TeamName = objUserNotificationsVM.TeamName.Substring(1);

                                    }
                                }
                                objUserNotificationsVM.IsActive = IsActiveNotification(objNotificationType, userNt.TargetPostID);
                                objUserNotificationsVM.UserNotificationType = objNotificationType;
                                objUserNotificationsVM.NotificationMessage = notificationMessage;
                                objUserNotificationsVM.UserID = userNt.UserID;
                                objUserNotificationsVM.UserType = userNt.UserType;
                                objUserNotificationsVM.CreatedNotificationUtcDateTime = userNt.CreatedNotificationUtcDateTime;
                                objUserNotificationsVM.NotificationType = userNt.NotificationType;
                                objUserNotificationsVM.UserNotificationID = userNt.UserNotificationID;
                                objUserNotificationsVM.TargetPostID = userNt.TargetPostID;
                                objUserNotificationsVM.Status = false;
                                objUserNotificationsVM.IsRead = userNt.IsRead;
                                objUserNotificationsVM.SenderCredID = userNt.SenderCredID;
                                objUserNotificationsVM.ReceiverCredID = userNt.ReceiverCredID;
                                objUserNotificationsVM.IsFriendChallenge = userNt.IsFriendChallenge;
                                objUserNotificationsVM.IsOnBoarding = userNt.IsOnBoarding;
                                userNotificationList.Add(objUserNotificationsVM);
                            }
                        }
                        // Remove of deleted notification post.
                        if (userNotificationList != null)
                        {
                            userNotificationList = userNotificationList.Where(nt => nt.IsActive && !string.IsNullOrEmpty(nt.SenderUserName)).ToList();
                        }
                        // Update the count the other people have same notification
                        for (int icount = 0; icount < userNotificationList.Count; icount++)
                        {
                            UserNotificationsVM usrNt = userNotificationList[icount];
                            if (!Enum.TryParse(usrNt.NotificationType, out objNotificationType))
                            {
                                objNotificationType = NotificationType.None;
                            }
                            switch (objNotificationType)
                            {
                                case NotificationType.NewsFeedBoomed:
                                case NotificationType.NewsFeedCommented:
                                case NotificationType.ResultFeedBoomed:
                                case NotificationType.ResultCommented:
                                case NotificationType.PostCommentedReplyMsg:
                                case NotificationType.PostResultReplyMsg:
                                    GetALLlNotificationListByResultID(usrNt, ref userNotificationList);
                                    break;

                            }
                        }
                        // Update the status false when it called the Web API
                        IQueryable<tblUserNotifications> usernofication = (from un in dataContext.UserNotifications
                                                                           where un.ReceiverCredID == cred.Id && un.Status
                                                                           select un);
                        foreach (tblUserNotifications obj in usernofication)
                        {
                            obj.Status = false;
                        };
                        dataContext.SaveChanges();
                        //End the update Notification stutus
                        totalNotificationList = new Total<List<UserNotificationsVM>>();
                        totalNotificationList.TotalList = (from l in userNotificationList
                                                           orderby l.CreatedNotificationUtcDateTime descending
                                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                        totalNotificationList.TotalCount = userNotificationList.Count;
                        totalNotificationList.UnReadNotificationCount = 0;
                        if ((totalNotificationList.TotalCount) > endIndex)
                        {
                            totalNotificationList.IsMoreAvailable = true;
                        }
                    }
                    return totalNotificationList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserNotificationList--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get ALL NotificationList By ResultID and Notification Type
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        public static void GetALLlNotificationListByResultID(UserNotificationsVM model, ref List<UserNotificationsVM> userNotifications)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTotalNotificationListByResultID()-targetId-" + model.TargetPostID + ",NotificationType-" + model.NotificationType);
                    if (model.TargetPostID > 0)
                    {
                        List<UserNotificationsVM> notidicationdetails = userNotifications.Where(nt => nt.TargetPostID == model.TargetPostID && nt.NotificationType == model.NotificationType)
                            .OrderByDescending(usr => usr.UserNotificationID).Distinct().ToList();
                        if (notidicationdetails != null)
                        {
                            model.TotalCount = notidicationdetails.Where(srd => srd.SenderCredID != srd.ReceiverCredID).GroupBy(n => n.SenderCredID).Select(nt => nt.FirstOrDefault()).ToList().Count;
                            model.SenderUserName = notidicationdetails.Select(user => user.SenderUserName).FirstOrDefault();
                            for (int count = 1; count < notidicationdetails.Count; count++)
                            {
                                userNotifications.Remove(notidicationdetails[count]);
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTotalNotificationListByResultID--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        //
        public static bool IsActiveNotification(NotificationType notificationType, int targetPostId)
        {
            StringBuilder traceLog = null;
            bool isActive = true;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: targetPostId-" + targetPostId);
                    switch (notificationType)
                    {
                        case NotificationType.TrainerChallege:
                        case NotificationType.FriendChallege:
                            isActive = dataContext.Challenge.Any(ch => ch.ChallengeId == targetPostId);
                            break;
                        case NotificationType.TrainerJoinTeam:
                            //  isActive = dataContext.User.Any(ch => ch.UserId == targetPostId);
                            break;
                        case NotificationType.ProfilesPostToUser:
                        case NotificationType.TrainerPostToUser:
                        case NotificationType.NewsFeedBoomed:
                        case NotificationType.NewsFeedCommented:
                        case NotificationType.PostCommentedReplyMsg:
                            isActive = dataContext.MessageStraems.Any(ch => ch.MessageStraemId == targetPostId);
                            break;
                        case NotificationType.ResultFeedBoomed:
                        case NotificationType.ResultCommented:
                        case NotificationType.PostResultReplyMsg:
                            isActive = dataContext.UserChallenge.Any(ch => ch.Id == targetPostId);
                            break;
                    }
                    return isActive;
                }
                catch
                {
                    return true;
                }
                finally
                {
                    traceLog.AppendLine("End: IsActiveNotification--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }

        /// <summary>
        /// Get ALLlNotificationListByUSerProfile
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <param name="userNotifications"></param>
        public static void GetALLlNotificationListByUSerProfile(UserNotificationsVM model, UserNotificationsVM result, ref List<UserNotificationsVM> userNotifications)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetALLlNotificationListByUSerProfile()-targetId-" + model.TargetPostID + ",NotificationType-" + model.NotificationType);
                    if (model.ReceiverCredID > 0)
                    {
                        List<UserNotificationsVM> notidicationdetails = userNotifications.Where(nt => nt.ReceiverCredID == model.ReceiverCredID
                            && nt.NotificationType == model.NotificationType).OrderByDescending(usr => usr.UserNotificationID).ToList();
                        if (notidicationdetails != null)
                        {
                            result.TotalCount = notidicationdetails.Count;
                            result.SenderUserName = notidicationdetails.Select(user => user.SenderUserName).FirstOrDefault();
                            for (int count = 1; count < notidicationdetails.Count; count++)
                            {
                                userNotifications.Remove(notidicationdetails[count]);
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetALLlNotificationListByUSerProfile--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get TotalActiveNotification based on user 
        /// </summary>
        /// <param name="credID"></param>
        /// <returns></returns>
        public static int GetTotalActiveNotification(int credID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                int totalActiveNotification = 0;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTotalActiveNotification()" + credID);
                    var activeNotifications = (from un in dataContext.UserNotifications
                                               where un.ReceiverCredID == credID && un.Status
                                               select un).ToList();
                    if (activeNotifications != null)
                    {
                        totalActiveNotification = activeNotifications.Count;
                    }
                    return totalActiveNotification;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTotalActiveNotification--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Send to User Notification in case trainer post message or other user post message
        /// </summary>
        /// <param name="currentUserType"></param>
        /// <param name="currentUserId"></param>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="messageStreamID"></param>
        public static void SendProfileNotificationToUser(string currentUserType, int currentUserId, int userId, string userType, int messageStreamID)
        {
            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = false;
                                StringBuilder traceLog = null;
                                int receiverUserCredId = 0;
                                string notificationType = string.Empty;
                                traceLog = new StringBuilder();
                                //bool IsUSerInTeam = false;
                                traceLog.AppendLine("Start: SendNotificationToUser()- userID-" + userId + " ,userType-" + userType);
                                using (LinksMediaContext dataContext = new LinksMediaContext())
                                {
                                    int personalTrainerCredId = 0;
                                    try
                                    {
                                        Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                                        string senderName = string.Empty;
                                        if (currentUserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                            && userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                                        {

                                            personalTrainerCredId = (from c in dataContext.Credentials
                                                                     join u in dataContext.User
                                                                     on c.UserId equals u.UserId
                                                                     where c.UserId == userId && c.UserType == Message.UserTypeUser
                                                                     select u.PersonalTrainerCredId).FirstOrDefault();

                                            // My team trainers post on my profile
                                            if (personalTrainerCredId > 0 && personalTrainerCredId == currentUserId)
                                            {
                                                notificationType = NotificationType.TrainerPostToUser.ToString();
                                                //IsUSerInTeam = true;
                                            }
                                            else // Other team trainers post on my profile
                                            {
                                                notificationType = NotificationType.ProfilesPostToUser.ToString();
                                            }

                                        }
                                        else
                                        {
                                            notificationType = NotificationType.ProfilesPostToUser.ToString();
                                        }
                                        UserVM receiverrDetails = userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) ?
                                                                               (from usr in dataContext.User
                                                                                join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                                                where crd.UserType == Message.UserTypeUser && usr.UserId == userId
                                                                                select new UserVM
                                                                                {
                                                                                    CredUserId = crd.Id,
                                                                                    FullName = usr.FirstName + " " + usr.LastName
                                                                                }).FirstOrDefault() :
                                                                               (from trainer in dataContext.Trainer
                                                                                join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                                                where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == userId
                                                                                select new UserVM
                                                                                {
                                                                                    CredUserId = crd.Id,
                                                                                    FullName = trainer.FirstName + " " + trainer.LastName
                                                                                }).FirstOrDefault();

                                        UserVM senderDetails = cred.UserType.Equals(Message.UserTypeUser) ?
                                                                              (from usr in dataContext.User
                                                                               join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                                               where crd.UserType == Message.UserTypeUser && usr.UserId == cred.UserId
                                                                               select new UserVM
                                                                               {
                                                                                   CredUserId = crd.Id,
                                                                                   FullName = usr.FirstName + " " + usr.LastName
                                                                               }).FirstOrDefault() :
                                                                              (from trainer in dataContext.Trainer
                                                                               join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                                               where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == cred.UserId
                                                                               select new UserVM
                                                                               {
                                                                                   CredUserId = crd.Id,
                                                                                   FullName = trainer.FirstName + " " + trainer.LastName
                                                                               }).FirstOrDefault();
                                        if (receiverrDetails != null)
                                        {
                                            receiverUserCredId = receiverrDetails.CredUserId;
                                        }
                                        if (senderDetails != null)
                                        {
                                            senderName = senderDetails.FullName;
                                        }
                                        SendNotificationToUser(receiverUserCredId, senderName, cred.UserId,
                                            cred.UserType, notificationType, certificate, messageStreamID);
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        traceLog.AppendLine("End: JoinTeam  --- " + DateTime.Now.ToLongDateString());
                                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                                        traceLog = null;
                                    }
                                }
                            }).Start();
        }
        /// <summary>
        /// Send all team all memeber notification when trainer post the profile
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="messageStreamId"></param>
        public static void SendALLTeamNotificationByTrainer(int trainerID, int messageStreamId, int senderUserId, string senderUserType)
        {
            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                StringBuilder traceLog = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    int receiverUserCredId = 0;
                    int teamId = 0;
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SendNotificationToUser()- trainerCredID-" + trainerID);
                        // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);                   
                        int trainerteamID = (from t in dataContext.Trainer
                                             join team in dataContext.Teams
                                             on t.TeamId equals team.TeamId
                                             where t.TrainerId == trainerID
                                             select t.TeamId).FirstOrDefault();
                        if (trainerteamID > 0)
                        {
                            teamId = trainerteamID;
                        }
                        if (teamId > 0)
                        {
                            string senderName = string.Empty;
                            List<int> teamMemberIDS = (from usrtm in dataContext.TrainerTeamMembers
                                                       join crd in dataContext.Credentials on usrtm.UserId equals crd.Id
                                                       where usrtm.TeamId == teamId
                                                       select usrtm.UserId).Distinct().ToList();
                            // get list of receciver of notification
                            List<UserVM> receiverrDetails = (from usr in dataContext.User
                                                             join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                             where crd.UserType == Message.UserTypeUser && teamMemberIDS.Contains(crd.Id)
                                                             select new UserVM
                                                             {
                                                                 CredUserId = crd.Id,
                                                                 FullName = usr.FirstName + " " + usr.LastName
                                                             }).Distinct().ToList();
                            // get details from  sender
                            UserVM senderDetails = (from trainer in dataContext.Trainer
                                                    join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                    where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == senderUserId
                                                    select new UserVM
                                                    {
                                                        CredUserId = crd.Id,
                                                        FullName = trainer.FirstName + " " + trainer.LastName
                                                    }).FirstOrDefault();
                            //
                            if (senderDetails != null)
                            {
                                senderName = senderDetails.FullName;
                            }
                            //
                            if (receiverrDetails != null)
                            {
                                foreach (var receiver in receiverrDetails)
                                {
                                    receiverUserCredId = receiver.CredUserId;
                                    SendNotificationToUser(receiverUserCredId, senderName, senderUserId, senderUserType,
                                        NotificationType.TrainerPostToUser.ToString(), certificate, messageStreamId);
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendALLTeamNotificationByTrainer()  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Send all team all memeber notification when trainer post the profile
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="messageStreamId"></param>
        public static void SendTrainerTeamsUserNotificationByTrainer(int trainerID, int messageStreamId)
        {

            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                StringBuilder traceLog = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    int receiverUserCredId = 0;
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SendAllTeamNotificationByTrainer()- trainerID-" + trainerID);
                        Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        string senderName = string.Empty;
                        // Get trainer team list
                        List<int> trainerTeamIds = (from crd in dataContext.Credentials
                                                    join tms in dataContext.TrainerTeamMembers
                                                    on crd.Id equals tms.UserId
                                                    where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                                    select tms.TeamId).ToList();
                        // To get all user of traner team users
                        List<UserVM> receiverrDetails = (from usr in dataContext.User
                                                         join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                         where crd.UserType == Message.UserTypeUser && usr.TeamId > 0 && trainerTeamIds.Contains(usr.TeamId)
                                                         select new UserVM
                                                         {
                                                             CredUserId = crd.Id,
                                                             FullName = usr.FirstName + " " + usr.LastName
                                                         }).ToList();

                        // Remove the duplicate user from  team group
                        if (receiverrDetails != null && receiverrDetails.Count > 0)
                        {
                            receiverrDetails = receiverrDetails.GroupBy(ul => ul.CredUserId)
                                                             .Select(grp => grp.FirstOrDefault())
                                                             .ToList();
                        }
                        //
                        UserVM senderDetails = (from trainer in dataContext.Trainer
                                                join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == cred.UserId
                                                select new UserVM
                                                {
                                                    CredUserId = crd.Id,
                                                    FullName = trainer.FirstName + " " + trainer.LastName
                                                }).FirstOrDefault();
                        if (senderDetails != null)
                        {
                            senderName = senderDetails.FullName;
                        }
                        if (receiverrDetails != null)
                        {
                            foreach (UserVM receiver in receiverrDetails)
                            {
                                receiverUserCredId = receiver.CredUserId;
                                SendNotificationToUser(receiverUserCredId, senderName, cred.UserId, cred.UserType,
                                    NotificationType.TrainerPostToUser.ToString(), certificate, messageStreamId);
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendAllTeamNotificationByTrainer  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                    }
                }
            }).Start();
        }
        /// <summary>
        /// Send the Notification of all type of post/commented
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userType"></param>
        /// <param name="notificationType"></param>
        public static void SendNotificationToALLNType(int userID, string userType, string notificationType, Credentials cred, int targetID = 0)
        {
            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = false;
                                StringBuilder traceLog = null;
                                using (LinksMediaContext dataContext = new LinksMediaContext())
                                {
                                    int receiverUserCredId = 0;
                                    try
                                    {
                                        traceLog = new StringBuilder();
                                        traceLog.AppendLine("Start: SendNotificationToALLNType()- userID-" + userID + " ,userType-" + userType + " ,notificationType-" + notificationType);
                                        // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                                        string senderName = string.Empty;
                                        UserVM receiverrDetails = !string.IsNullOrEmpty(userType) && userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) ?
                                                                         (from usr in dataContext.User
                                                                          join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                                          where crd.UserType == Message.UserTypeUser && usr.UserId == userID
                                                                          select new UserVM
                                                                          {
                                                                              CredUserId = crd.Id,
                                                                              FullName = usr.FirstName + " " + usr.LastName
                                                                          }).FirstOrDefault() :
                                                                         (from trainer in dataContext.Trainer
                                                                          join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                                          where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == userID
                                                                          select new UserVM
                                                                          {
                                                                              CredUserId = crd.Id,
                                                                              FullName = trainer.FirstName + " " + trainer.LastName
                                                                          }).FirstOrDefault();

                                        UserVM senderDetails = !string.IsNullOrEmpty(cred.UserType) && cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) ?
                                                                              (from usr in dataContext.User
                                                                               join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                                               where crd.UserType == Message.UserTypeUser && usr.UserId == cred.UserId
                                                                               select new UserVM
                                                                               {
                                                                                   CredUserId = crd.Id,
                                                                                   FullName = usr.FirstName + " " + usr.LastName
                                                                               }).FirstOrDefault() :
                                                                              (from trainer in dataContext.Trainer
                                                                               join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                                               where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == cred.UserId
                                                                               select new UserVM
                                                                               {
                                                                                   CredUserId = crd.Id,
                                                                                   FullName = trainer.FirstName + " " + trainer.LastName
                                                                               }).FirstOrDefault();
                                        if (receiverrDetails != null)
                                        {
                                            receiverUserCredId = receiverrDetails.CredUserId;
                                        }
                                        if (senderDetails != null)
                                        {
                                            senderName = senderDetails.FullName;
                                        }
                                        SendNotificationToUser(receiverUserCredId, senderName, cred.UserId, cred.UserType, notificationType, certificate, targetID);
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        traceLog.AppendLine("End: SendNotificationToALLNType  --- " + DateTime.Now.ToLongDateString());
                                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                                        traceLog = null;
                                    }
                                }
                            }).Start();
        }
        /// <summary>
        ///  Send the user notification to user in case boomed/commented/following
        /// </summary>
        /// <param name="userID"></param>
        public static void SendNotificationToUser(int receiverUserCredId, string senderName, int senderUserId, string senderUserType, string notificationType, byte[] certificate, int targetID = 0)
        {
            lock (syncLock)
            {
                StringBuilder traceLog = null;
                List<UserNotification> allactiveDevices = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        bool isNTSent = false;
                        //string NotificationSentStatus = string.Empty;
                        traceLog.AppendLine("Start: SendNotificationToUser()");
                        if (receiverUserCredId > 0)
                        {
                            allactiveDevices = PushNotificationBL.GetLastUserDeviceID(receiverUserCredId);
                            bool isSendtNotification = false;
                            bool isSaveSuccessfully = false;
                            long notificationID = 0;
                            DateTime createdNotificationUtcDateTime = DateTime.UtcNow;
                            foreach (UserNotification objuserNotification in allactiveDevices)
                            {
                                // If Deives Token null then by pass to sending push notification
                                if (!string.IsNullOrEmpty(objuserNotification.DeviceID))
                                {
                                    if (!isSaveSuccessfully)
                                    {
                                        UserNotificationsVM objNT = new UserNotificationsVM();
                                        objNT.UserID = senderUserId;
                                        objNT.UserType = senderUserType;
                                        objNT.ReceiverCredID = receiverUserCredId;
                                        objNT.NotificationType = notificationType;
                                        objNT.SenderUserName = senderName;
                                        objNT.TokenDevicesID = objuserNotification.DeviceID;
                                        objNT.TargetPostID = targetID;
                                        objNT.CreatedNotificationUtcDateTime = createdNotificationUtcDateTime;
                                        notificationID = CommonWebApiBL.SaveDevicesNotification(objNT);
                                        isSaveSuccessfully = true;
                                    }
                                    isNTSent = SendNotificationToDevices(receiverUserCredId, senderName, notificationType, objuserNotification.DeviceID, objuserNotification.DeviceType, certificate, senderUserId, targetID);
                                }
                                if (isNTSent)
                                {
                                    if (!isSendtNotification)
                                    {
                                        isSendtNotification = true;
                                    }
                                    isSaveSuccessfully = false;
                                }
                            }
                            if (!isSendtNotification)
                            {
                                if (notificationID > 0)
                                {
                                    // NotificationApiBL.DeleteNotification(notificationID);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogManagerInstance.WriteErrorLog(ex);
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendNotificationToUser()  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                        allactiveDevices = null;
                    }
                }
            }
        }
        /// <summary>
        ///  Send the  user total Pending challenges by friend
        /// </summary>
        /// <param name="userID"></param>
        public static bool SendNotificationToDevices(int userID, string name, string notificationType, string deviceID, string deviceType, byte[] certificate, int challegeUSerID, int targetID = 0)
        {
            StringBuilder traceLog = null;
            bool isSendNotification = false;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SendNotificationToDevices()");
                var item = PushNotificationBL.GetNotificationStatus(userID, notificationType, deviceID, deviceType);
                bool success = item != null ? item.IsNotify : true;
                if (success)
                {
                    int userTotalNotification = PushNotificationBL.GetUserTotalFriendPendingChallenges(userID);
                    int totalbudget = CommonWebApiBL.GetTotalUSerNotification(userID, deviceID);
                    isSendNotification = PushNotificationBL.ValidateAndSendNotification(userID, deviceID, notificationType, name, deviceType, userTotalNotification, certificate, totalbudget, string.Empty, challegeUSerID, targetID);
                    return isSendNotification;
                }
                return isSendNotification;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                traceLog.AppendLine("End: SendNotificationToDevices()  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                // PendingChallengeList = null;
            }
        }
        /// <summary>
        /// Send the commented user when posted user comment.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="messageStreamId"></param>
        /// <param name="commentType"></param>
        public static void SendALLSenderNotificationByUser(int messageStreamId, string commentType, string replyNotificationType, Credentials cred)
        {
            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                StringBuilder traceLog = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    int receiverUserCredId = 0;
                    try
                    {
                        traceLog = new StringBuilder();
                        // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        traceLog.AppendLine("Start: SendALLSenderNotificationByUser()- TrainerCredId-" + cred.Id);
                        string senderName = string.Empty;
                        List<int> senderCredIDList = (from usrtm in dataContext.UserNotifications
                                                      where usrtm.TargetID == messageStreamId && usrtm.NotificationType == commentType &&
                                                      usrtm.SenderCredlID != cred.Id
                                                      select usrtm.SenderCredlID).Distinct().ToList();

                        UserVM senderDetails = cred.UserType.Equals(Message.UserTypeUser) ?
                                                              (from usr in dataContext.User
                                                               join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                                               where crd.UserType == Message.UserTypeUser && usr.UserId == cred.UserId
                                                               select new UserVM
                                                               {
                                                                   CredUserId = crd.Id,
                                                                   FullName = usr.FirstName + " " + usr.LastName
                                                               }).FirstOrDefault() :
                                                               cred.UserType.Equals(Message.UserTypeTrainer) ?
                                                              (from trainer in dataContext.Trainer
                                                               join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                               where crd.UserType == Message.UserTypeTrainer && trainer.TrainerId == cred.UserId
                                                               select new UserVM
                                                               {
                                                                   CredUserId = crd.Id,
                                                                   FullName = trainer.FirstName + " " + trainer.LastName
                                                               }).FirstOrDefault() : null;
                        if (senderDetails != null)
                        {
                            senderName = senderDetails.FullName;
                        }
                        if (senderCredIDList != null)
                        {
                            foreach (var receiver in senderCredIDList)
                            {
                                receiverUserCredId = receiver;
                                SendNotificationToUser(receiverUserCredId, senderName, cred.UserId, cred.UserType, replyNotificationType, certificate, messageStreamId);
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendALLSenderNotificationByUser  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Update Notification Read Status by Notification ID
        /// </summary>
        /// <param name="notificationID"></param>
        /// <returns></returns>
        public static bool UpdateNotificationReadStatus(LinksMediaContext dataContext, long notificationID)
        {
            StringBuilder traceLog = null;

            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamBL Get UpdateNotificationReadStatus-notificationID:-" + notificationID);
                if (notificationID > 0)
                {
                    var userNotification = dataContext.UserNotifications.FirstOrDefault(nt => nt.NotificationID == notificationID);
                    if (userNotification != null)
                    {
                        userNotification.Status = false;
                        userNotification.IsRead = true;
                        userNotification.ModifiedDate = DateTime.UtcNow;
                        dataContext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End : UpdateNotificationReadStatus  : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }

        }

        /// <summary>
        /// Get ALL NotificationList By ResultID and Notification Type
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        public static int GetALLlNotificationListByResultID(int targetPostID, string notificationType, int receiverCredID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetALLlNotificationListByResultID()-targetId-" + targetPostID + ",NotificationType-" + notificationType);
                    if (targetPostID > 0)
                    {
                        var notidicationdetails = dataContext.UserNotifications.Where(nt => nt.TargetID == targetPostID && nt.NotificationType == notificationType && nt.ReceiverCredID == receiverCredID).OrderByDescending(usr => usr.NotificationID).Distinct().ToList();
                        if (notidicationdetails != null)
                        {
                            int totalcount = notidicationdetails.Where(srd => srd.SenderCredlID != srd.ReceiverCredID).GroupBy(n => n.SenderCredlID).Select(nt => nt.FirstOrDefault()).ToList().Count;
                            int current = notidicationdetails.Where(srd => srd.SenderCredlID == receiverCredID).ToList().Count;
                            return (current > 0) ? totalcount - 1 : totalcount;
                        }
                    }
                    return 0;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetALLlNotificationListByResultID--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get ALLlNotificationListByUSerProfile
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <param name="userNotifications"></param>
        public static int GetALLlNotificationListByUSerProfile(string notificationType, int receiverCredID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetALLlNotificationListByUSerProfile()-receiverCredID-" + receiverCredID + ",NotificationType-" + notificationType);
                    if (receiverCredID > 0)
                    {
                        var notidicationdetails = dataContext.UserNotifications.Where(nt => nt.ReceiverCredID == receiverCredID && nt.SenderCredlID != nt.ReceiverCredID && nt.NotificationType == notificationType).OrderByDescending(usr => usr.NotificationID).ToList();
                        if (notidicationdetails != null)
                        {
                            int totalcount = notidicationdetails.Count;
                            int current = notidicationdetails.Where(srd => srd.SenderCredlID == receiverCredID).ToList().Count;
                            return (current > 0) ? totalcount - 1 : totalcount;
                        }
                    }
                    return 0;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetALLlNotificationListByUSerProfile--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Delete Notification based on notification ID
        /// </summary>
        /// <param name="notificationID"></param>
        /// <returns></returns>
        public static bool DeleteNotification(long notificationID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: DeleteNotification()-:notificationID-" + notificationID);
                    var objUserNotification = dataContext.UserNotifications.FirstOrDefault(u => u.NotificationID == notificationID);
                    if (objUserNotification != null)
                    {
                        dataContext.UserNotifications.Remove(objUserNotification);
                        dataContext.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : DeleteNotification()  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Send the team trainer Notification
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="teamJoinUserID"></param>
        /// <param name="joinedUserName"></param>
        public static void SendJoinedTeamNotificationToTrainers(int teamId, string joinedUserName, string NtType, int userId, string userType)
        {
            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                StringBuilder traceLog = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SendJoinedTeamNotificationToTrainers()- TeamId-" + teamId);
                        if (teamId > 0)
                        {
                            string teamName = dataContext.Teams.Where(t => t.TeamId == teamId).Select(tt => tt.TeamName).FirstOrDefault();
                            string senderName = string.Empty;
                            if (!string.IsNullOrEmpty(teamName))
                            {
                                teamName = teamName.Substring(1);
                            }
                            List<int> teamtrainerIDS = (from tr in dataContext.TrainerTeamMembers
                                                        join crd in dataContext.Credentials
                                                        on tr.UserId equals crd.Id
                                                        where crd.UserType == Message.UserTypeTrainer && tr.TeamId == teamId
                                                        select crd.Id).Distinct().ToList();
                            senderName = joinedUserName;
                            if (teamtrainerIDS != null && teamtrainerIDS.Count > 0)
                            {
                                foreach (int receiver in teamtrainerIDS)
                                {
                                    SendNotificationToTeamTrainers(receiver, senderName, userId, userType, NtType, teamName, certificate, teamId);
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendJoinedTeamNotificationToTrainers  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                    }
                }
            }).Start();
        }
        /// <summary>
        /// Send team trainer notification
        /// </summary>
        /// <param name="receiverUserCredId"></param>
        /// <param name="senderName"></param>
        /// <param name="senderUserId"></param>
        /// <param name="senderUserType"></param>
        /// <param name="notificationType"></param>
        /// <param name="certificate"></param>
        private static void SendNotificationToTeamTrainers(int receiverUserCredId, string senderName, int senderUserId, string senderUserType, string notificationType, string teamName, byte[] certificate, int teamId = 0)
        {
            lock (syncLock)
            {

                StringBuilder traceLog = null;
                List<UserNotification> allactiveDevices = null;
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SendNotificationToTeamTrainers()");
                        if (receiverUserCredId > 0)
                        {
                            allactiveDevices = PushNotificationBL.GetLastUserDeviceID(receiverUserCredId);
                            long userNotificationID = 0;
                            bool isNotificationSent = false;
                            bool isSentNotification = false;
                            bool isLoginedUser = false;
                            DateTime createdNotificationUtcDateTime = DateTime.UtcNow;
                            foreach (UserNotification objuserNotification in allactiveDevices)
                            {
                                //If devices token null then bypass sending push notification to devices
                                if (!string.IsNullOrEmpty(objuserNotification.DeviceID))
                                {
                                    var item = PushNotificationBL.GetNotificationStatus(receiverUserCredId, notificationType, objuserNotification.DeviceID, objuserNotification.DeviceType);
                                    bool success = item != null ? item.IsNotify : true;
                                    if (success)
                                    {
                                        if (!isLoginedUser)
                                        {
                                            UserNotificationsVM objNT = new UserNotificationsVM();
                                            objNT.UserID = senderUserId;
                                            objNT.UserType = senderUserType;
                                            objNT.ReceiverCredID = receiverUserCredId;
                                            objNT.NotificationType = notificationType;
                                            objNT.SenderUserName = senderName;
                                            objNT.TokenDevicesID = objuserNotification.DeviceID;
                                            objNT.TargetPostID = teamId;
                                            objNT.CreatedNotificationUtcDateTime = createdNotificationUtcDateTime;
                                            userNotificationID = CommonWebApiBL.SaveDevicesNotification(objNT);
                                            isLoginedUser = true;
                                        }
                                        int userTotalNotification = PushNotificationBL.GetUserTotalFriendPendingChallenges(receiverUserCredId);
                                        isNotificationSent = TeamBL.SendJoinNotification(senderUserId, receiverUserCredId, objuserNotification.DeviceID, NotificationType.TrainerJoinTeam.ToString(), senderName, objuserNotification.DeviceType, userTotalNotification, teamName, certificate);
                                        if (isNotificationSent)
                                        {
                                            if (!isSentNotification)
                                            {
                                                isSentNotification = true;
                                            }
                                            isLoginedUser = false;
                                        }
                                    }
                                }
                            }
                            if (!isSentNotification)
                            {
                                if (userNotificationID > 0)
                                {
                                    // NotificationApiBL.DeleteNotification(userNotificationID);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogManagerInstance.WriteErrorLog(ex);
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendNotificationToTeamTrainers()  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                        allactiveDevices = null;
                    }
                }
            }
        }

        /// <summary>
        /// Send  Notification To Trainer when user Select Primary Trainer
        /// </summary>
        /// <param name="userCredId"></param>
        /// <param name="senderName"></param>
        public static void SendSelectPrimaryTrainerNotificationToTrainer(int userCredId, string senderName, int userId, string userType)
        {

            byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
            string NtType = NotificationType.SelectPrimaryTrainer.ToString();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SendSelectPrimaryTrainerNotificationToTrainer()- trainerCredId-" + userCredId);
                    //Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (userCredId > 0)
                    {
                        SendNotificationToUser(userCredId, senderName, userId, userType, NtType, certificate);

                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: SendSelectPrimaryTrainerNotificationToTrainer  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }).Start();
        }


    }
}


