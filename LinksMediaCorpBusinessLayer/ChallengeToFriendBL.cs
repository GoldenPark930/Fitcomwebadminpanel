namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpEntity;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Drawing;
    using System.Globalization;
    using System.Web;
    using System.Text.RegularExpressions;

    public class ChallengeToFriendBL
    {
        private static object syncLock = new object();

        /// <summary>
        /// Function to filter friend list
        /// </summary>
        /// <returns>Total<List<FriendVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/04/2015
        /// </devdoc>        
        public static Total<List<FriendVM>> GetFilterFriendList(NumberOfRecord<FriendFilterParam> model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<FriendVM> filterFriendList = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFilterFriendList");
                    if (!string.IsNullOrEmpty(model.Param.Name))
                    {
                        model.Param.Name = model.Param.Name.Trim();
                    }
                    Total<List<FriendVM>> objAllFriendList = GetAllFriendList(dataContext, new NumberOfRecord<string>() { StartIndex = 0, EndIndex = 1000 });
                    filterFriendList = (from list in objAllFriendList.TotalList
                                        orderby list.UserName
                                        where (model.Param.Name == null || list.UserName.ToUpper(CultureInfo.InvariantCulture).Contains(model.Param.Name.ToUpper(CultureInfo.InvariantCulture)))
                                        select list).ToList();
                    objAllFriendList.TotalList = filterFriendList;
                    return objAllFriendList;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFilterFriendList  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    filterFriendList = null;
                }
            }
        }
        /// <summary>
        /// Get User All FriendList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Total<List<FriendVM>> GetUserAllFriendList(NumberOfRecord<string> model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog.AppendLine("Start :GetUserAllFriendList() method");
                try
                {
                    return GetAllFriendList(dataContext, model);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserAllFriendList  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Function to get all friend list
        /// </summary>
        /// <returns>Total<List<FriendVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/30/2015
        /// </devdoc>
        /// 
        public static Total<List<FriendVM>> GetAllFriendList(LinksMediaContext dataContext, NumberOfRecord<string> model)
        {
            StringBuilder traceLog = null;
            List<FriendVM> objFriendList = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllFriendList");
                Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                List<int> teamIdlist = new List<int>();
                if (objCredential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                {
                    teamIdlist = (from crd in dataContext.Credentials
                                  join tms in dataContext.TrainerTeamMembers
                                  on crd.Id equals tms.UserId
                                  where crd.Id == objCredential.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                  select tms.TeamId).ToList();
                }
                else if (objCredential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                {
                    teamIdlist = (from crd in dataContext.Credentials
                                  join usr in dataContext.User on crd.UserId equals usr.UserId
                                  where crd.UserType == Message.UserTypeUser && crd.UserId == objCredential.UserId
                                  select usr.TeamId).ToList();
                }

                objFriendList = ((from cred in dataContext.Credentials
                                  join user in dataContext.User on cred.UserId equals user.UserId
                                  join ttm in dataContext.TrainerTeamMembers on cred.Id equals ttm.UserId
                                  where ttm.TeamId > 0 && teamIdlist.Contains(ttm.TeamId) && cred.UserType == Message.UserTypeUser
                                  select new FriendVM
                                  {
                                      CredUserId = cred.Id,
                                      UserId = cred.UserId,
                                      UserType = cred.UserType,
                                      UserName = user.FirstName + " " + user.LastName,
                                      State = user.State,
                                      CityName = user.City,
                                      ImageUrl = user.UserImageUrl,
                                      Location = user.City + ", " + user.State,
                                      IsSelect = true
                                  }).Union
                             (from f in dataContext.Followings
                              join cred in dataContext.Credentials on f.FollowUserId equals cred.Id
                              join user in dataContext.User on cred.UserId equals user.UserId
                              where f.UserId == objCredential.Id && cred.UserType == Message.UserTypeUser
                              select new FriendVM
                              {
                                  CredUserId = cred.Id,
                                  UserId = cred.UserId,
                                  UserType = cred.UserType,
                                  UserName = user.FirstName + " " + user.LastName,
                                  State = user.State,
                                  CityName = user.City,
                                  ImageUrl = user.UserImageUrl,
                                  Location = user.City + ", " + user.State,
                                  IsSelect = false
                              })).ToList();
                objFriendList.Remove(objFriendList.FirstOrDefault(f => f.CredUserId == objCredential.Id));
                objFriendList = objFriendList.OrderByDescending(frd => frd.IsSelect).ToList();
                // Remove the duplicate the friend list
                if (objFriendList != null)
                {
                    objFriendList = objFriendList.GroupBy(cr => cr.CredUserId).Select(usr => usr.OrderByDescending(item => item.IsSelect).FirstOrDefault()).ToList();
                }
                objFriendList.ForEach(fl =>
                {
                    fl.ImageUrl = string.IsNullOrEmpty(fl.ImageUrl) ? null : CommonUtility.VirtualPath + Message.ProfilePicDirectory + fl.ImageUrl;
                });
                Total<List<FriendVM>> objresult = new Total<List<FriendVM>>();
                objresult.TotalList = (from fl in objFriendList
                                       orderby fl.UserName
                                       select fl).Skip(model.StartIndex).Take(model.EndIndex - model.StartIndex).ToList();
                objresult.TotalCount = objFriendList.Count();
                if (objresult.TotalCount > model.EndIndex)
                {
                    objresult.IsMoreAvailable = true;
                }
                return objresult;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: GetAllFriendList  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objFriendList = null;
            }

        }

        /// <summary>
        /// Function to post challenge to friends
        /// </summary>
        /// <returns>List<CompletedChallengeVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/04/2015
        /// </devdoc>
        /// 
        public static CompletedChallengesWithPendingVM PostChallengeToFriend(ChallengeFriendVM model)
        {
            StringBuilder traceLog = null;
            Credentials objCredential = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostChallengeToFriend");
                    objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    ChallengeUserDetails challengeBy = objCredential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) ?
                                  (from usr in dataContext.User
                                   join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                   join chlng in dataContext.UserChallenge on crd.Id equals chlng.UserId into temp
                                   from usertemp in temp.DefaultIfEmpty()
                                   where usr.UserId == objCredential.UserId
                                   && crd.UserType == Message.UserTypeUser
                                   && usertemp.ChallengeId == model.ChallengeId
                                   orderby usertemp.Id descending
                                   select new ChallengeUserDetails
                                   {
                                       ChallengeByUserName = usr.FirstName + " " + usr.LastName,
                                       Result = usertemp == null ? string.Empty : usertemp.Result,
                                       Fraction = usertemp == null ? string.Empty : usertemp.Fraction,
                                       ResultUnitSuffix = usertemp == null ? string.Empty : usertemp.ResultUnit,
                                       UserChallengeId = usertemp == null ? 0 : usertemp.Id
                                   }).FirstOrDefault() :
                                  objCredential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase) ?
                                  (from trainer in dataContext.Trainer
                                   join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                   join chlng in dataContext.UserChallenge on crd.Id equals chlng.UserId into temp
                                   from usertemp in temp.DefaultIfEmpty()
                                   where trainer.TrainerId == objCredential.UserId
                                   && crd.UserType == Message.UserTypeTrainer
                                   && usertemp.ChallengeId == model.ChallengeId
                                   orderby usertemp.Id descending
                                   select new ChallengeUserDetails
                                   {
                                       ChallengeByUserName = trainer.FirstName + " " + trainer.LastName,
                                       Result = usertemp == null ? string.Empty : usertemp.Result,
                                       Fraction = usertemp == null ? string.Empty : usertemp.Fraction,
                                       ResultUnitSuffix = usertemp == null ? string.Empty : usertemp.ResultUnit,
                                       UserChallengeId = usertemp == null ? 0 : usertemp.Id
                                   }).FirstOrDefault()
                                 : null;

                    string challenegUserName = string.Empty;
                    if (challengeBy == null)
                    {
                        challenegUserName = objCredential.UserType.Equals(Message.UserTypeUser) ?
                                    (from usr in dataContext.User
                                     where usr.UserId == objCredential.UserId
                                     select usr.FirstName + " " + usr.LastName
                                     ).FirstOrDefault() :
                                     objCredential.UserType.Equals(Message.UserTypeTrainer) ?
                                     (from trainer in dataContext.Trainer
                                      where trainer.TrainerId == objCredential.UserId
                                      select trainer.FirstName + " " + trainer.LastName
                                     ).FirstOrDefault() : null;
                    }
                    // If User is selected AllTeam Member checkbox 
                    if (model.IsSelecAllTMembers)
                    {
                        List<int> teamIds = new List<int>();
                        switch (objCredential.UserType)
                        {
                            case ConstantHelper.constuser:
                                teamIds = (from usr in dataContext.User
                                           join crd in dataContext.Credentials
                                           on usr.UserId equals crd.UserId
                                           where crd.Id == objCredential.Id && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                           select usr.TeamId).ToList();
                                break;
                            case ConstantHelper.consttrainer:
                                teamIds = (from crd in dataContext.Credentials
                                           join tms in dataContext.TrainerTeamMembers
                                           on crd.Id equals tms.UserId
                                           where crd.Id == objCredential.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                           select tms.TeamId).ToList();
                                break;
                        }
                        List<FriendVM> objUserList = new List<FriendVM>();
                        if (teamIds.Count > 0)
                        {
                            objUserList = (from cred in dataContext.Credentials
                                           join usr in dataContext.User on cred.UserId equals usr.UserId
                                           where usr.TeamId > 0 && teamIds.Contains(usr.TeamId) && cred.UserType == Message.UserTypeUser
                                           select new FriendVM
                                           {
                                               CredUserId = cred.Id
                                           }).ToList();

                            // Remove the duplicate user from  team group
                            if (objUserList != null)
                            {
                                objUserList = objUserList.GroupBy(ul => ul.CredUserId)
                                                                 .Select(grp => grp.FirstOrDefault())
                                                                 .ToList();
                            }
                        }
                        var remainingTM = (model.TeamUnselectedList != null && model.TeamUnselectedList.Count > 0) ? objUserList.Except(model.TeamUnselectedList).ToList() : objUserList;
                        if (model.FriendList != null && model.FriendList.Count > 0)
                        {
                            var allSelectedFriends = remainingTM != null ? remainingTM.Union(model.FriendList): model.FriendList;
                            model.FriendList = allSelectedFriends.ToList();
                        }
                        else
                        {
                            model.FriendList = remainingTM;
                        }
                    }
                    if (model.FriendList != null)
                    {
                        model.FriendList = model.FriendList.GroupBy(ul => ul.CredUserId)
                                                                 .Select(grp => grp.FirstOrDefault())
                                                                 .ToList();
                    }

                    model.FriendList.ForEach(friend =>
                        {
                            if (objCredential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                            {
                                var objtblChallengeToFriend = new tblChallengeToFriend();
                                objtblChallengeToFriend.ChallengeId = model.ChallengeId;
                                objtblChallengeToFriend.SubjectId = objCredential.Id;
                                objtblChallengeToFriend.TargetId = friend.CredUserId;
                                objtblChallengeToFriend.IsPending = true;
                                objtblChallengeToFriend.ChallengeDate = DateTime.Now;
                                objtblChallengeToFriend.ChallengeByUserName = challengeBy != null ? challengeBy.ChallengeByUserName : challenegUserName;
                                objtblChallengeToFriend.Result = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : string.IsNullOrEmpty(challengeBy.Result) ? string.Empty : challengeBy.Result.Replace(",", string.Empty);
                                objtblChallengeToFriend.Fraction = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : challengeBy.Fraction;
                                objtblChallengeToFriend.ResultUnitSuffix = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : challengeBy.ResultUnitSuffix;
                                objtblChallengeToFriend.IsProgram = model.IsProgram;
                                objtblChallengeToFriend.PersonalMessage = model.PersonalMessage;
                                objtblChallengeToFriend.IsActive = true;
                                objtblChallengeToFriend.UserChallengeId = (model.IsAcceptChallenge || challengeBy == null) ? 0 : challengeBy.UserChallengeId;
                                dataContext.ChallengeToFriends.Add(objtblChallengeToFriend);
                                dataContext.SaveChanges();
                                friend.ChallengeToFriendId = objtblChallengeToFriend.ChallengeToFriendId;
                                friend.IsFriendChallenge = true;
                            }
                            // If loged in is traienr and challenge to personal user and add in user assignment.
                            else if (objCredential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                            {

                                var objtblUserAssignmentByTrainer = new tblUserAssignmentByTrainer();
                                objtblUserAssignmentByTrainer.ChallengeId = model.ChallengeId;
                                objtblUserAssignmentByTrainer.SubjectId = objCredential.Id;
                                objtblUserAssignmentByTrainer.TargetId = friend.CredUserId;
                                objtblUserAssignmentByTrainer.IsCompleted = false;
                                objtblUserAssignmentByTrainer.ChallengeDate = DateTime.Now;
                                objtblUserAssignmentByTrainer.Result = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : string.IsNullOrEmpty(challengeBy.Result) ? string.Empty : challengeBy.Result.Replace(",", string.Empty);
                                objtblUserAssignmentByTrainer.Fraction = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : challengeBy.Fraction;
                                objtblUserAssignmentByTrainer.ResultUnitSuffix = (model.IsAcceptChallenge || challengeBy == null) ? string.Empty : challengeBy.ResultUnitSuffix;
                                objtblUserAssignmentByTrainer.ChallengeByUserName = challengeBy != null ? challengeBy.ChallengeByUserName : challenegUserName;
                                objtblUserAssignmentByTrainer.IsProgram = model.IsProgram;
                                objtblUserAssignmentByTrainer.IsRead = false;
                                objtblUserAssignmentByTrainer.IsActive = true;
                                objtblUserAssignmentByTrainer.PersonalMessage = model.PersonalMessage;
                                objtblUserAssignmentByTrainer.UserChallengeId = (model.IsAcceptChallenge || challengeBy == null) ? 0 : challengeBy.UserChallengeId;
                                friend.IsFriendChallenge = false;
                                objtblUserAssignmentByTrainer.IsOnBoarding = false;
                                dataContext.UserAssignments.Add(objtblUserAssignmentByTrainer);
                                dataContext.SaveChanges();
                                friend.ChallengeToFriendId = objtblUserAssignmentByTrainer.UserAssignmentId;
                            }
                        });

                    byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
                    string ChallengeByUserName, objCredentialUserType = string.Empty;
                    ChallengeByUserName = challengeBy != null ? challengeBy.ChallengeByUserName : challenegUserName;
                    int friendUserId = objCredential.UserId;
                    int targetchallenegID = model.ChallengeId;
                    objCredentialUserType = objCredential.UserType;
                    new Thread(() =>
                             {
                                 Thread.CurrentThread.IsBackground = false;
                                 foreach (FriendVM frd in model.FriendList)
                                 {
                                     // Send the Notification for trainer Challeneg or Friend Challege                                                               
                                     SendChallegesNotificationToUser(frd.CredUserId, ChallengeByUserName, friendUserId, objCredentialUserType, certificate, targetchallenegID, frd.ChallengeToFriendId, false, frd.IsFriendChallenge);
                                 }
                             }).Start();

                    return ChallengeApiBL.GetCompletedChallengeList(objCredential, dataContext);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: PostChallengeToFriend  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objCredential = null;
                }
            }
        }
        /// <summary>
        ///  Send the  user total Pending challenges by friend
        /// </summary>
        /// <param name="userID"></param>
        public static void SendChallegesNotificationToUser(int credUserId, string name, int userID, string userType, byte[] certificate, int targetchallenegID = 0, int challengeToFriendId = 0, bool isOnBoarding = false, bool IsFriendChallenge = false)
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
                        string notificationType = string.Empty;
                        bool isNTSent = false;
                        //  string NotificationSentStatus = string.Empty;
                        traceLog.AppendLine("Start: SendChallegesNotificationToUser()");
                        allactiveDevices = PushNotificationBL.GetLastUserDeviceID(credUserId);
                        bool isSendtNotification = false;
                        bool isSaveSuccessfully = false;
                        bool isSendNT = false;
                        DateTime createdNotificationUtcDateTime = DateTime.UtcNow;
                        foreach (UserNotification objuserNotification in allactiveDevices)
                        {
                            // If Deives Token null then by pass to sending push notification
                            if (!string.IsNullOrEmpty(objuserNotification.DeviceID))
                            {
                                if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                                {
                                    notificationType = NotificationType.TrainerChallege.ToString();
                                    isSendNT = true;
                                }
                                else
                                {
                                    notificationType = NotificationType.FriendChallege.ToString();
                                    isSendNT = true;
                                }
                                if (isSendNT)
                                {
                                    if (!isSaveSuccessfully)
                                    {
                                        UserNotificationsVM objNT = new UserNotificationsVM();
                                        objNT.UserID = userID;
                                        objNT.UserType = userType;
                                        objNT.ReceiverCredID = credUserId;
                                        objNT.NotificationType = notificationType;
                                        objNT.SenderUserName = name;
                                        objNT.TokenDevicesID = objuserNotification.DeviceID;
                                        objNT.TargetPostID = targetchallenegID;
                                        objNT.ChallengeToFriendId = challengeToFriendId;
                                        objNT.CreatedNotificationUtcDateTime = createdNotificationUtcDateTime;
                                        objNT.IsFriendChallenge = IsFriendChallenge;
                                        objNT.IsOnBoarding = isOnBoarding;
                                        CommonWebApiBL.SaveDevicesNotification(objNT);
                                        isSaveSuccessfully = true;
                                    }
                                    isNTSent = SendNotificationToUser(credUserId, name, notificationType, objuserNotification.DeviceID, objuserNotification.DeviceType, certificate, userID);
                                    if (isNTSent)
                                    {
                                        if (!isSendtNotification)
                                        {
                                            isSendtNotification = true;
                                        }
                                        isSaveSuccessfully = false;
                                    }

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
                        traceLog.AppendLine("End: SendChallegesNotificationToUser()  --- " + DateTime.Now.ToLongDateString());
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
        public static bool SendNotificationToUser(int userID, string name, string notificationType, string deviceID, string deviceType, byte[] certificate, int challegeUSerID)
        {
            StringBuilder traceLog = null;
            bool isSendNotification = false;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SendNotificationToUser()");
                var item = PushNotificationBL.GetNotificationStatus(userID, notificationType, deviceID, deviceType);
                bool success = item != null ? item.IsNotify : true;
                if (success)
                {
                    int userTotalNotification = PushNotificationBL.GetUserTotalFriendPendingChallenges(userID);
                    int totalbudget = CommonWebApiBL.GetTotalUSerNotification(userID, deviceID);
                    isSendNotification = PushNotificationBL.ValidateAndSendNotification(userID, deviceID, notificationType, name, deviceType, userTotalNotification, certificate, totalbudget, string.Empty, challegeUSerID);
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
                traceLog.AppendLine("End: SendNotificationToUser()  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                // PendingChallengeList = null;
            }

        }

        /// <summary>
        /// Function to get pending challenge list
        /// </summary>
        /// <returns>Total<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/07/2015
        /// </devdoc>
        public static List<PendingChallengeVM> GetPendingChallengeList(long notificationID = 0)
        {
            StringBuilder traceLog = new StringBuilder();
            List<PendingChallengeVM> objPendingList = null;
            List<PendingChallengeVM> objTempPendingList = null;
            List<ChallengeByUser> userAndResult = null;
            PendingChallengeVM challengeresult = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetPendingChallengeList---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<PendingChallengeVM> listPendingChallenge = (from c in dataContext.Challenge
                                                                     join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                                     join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                     join crd in dataContext.Credentials on ctf.SubjectId equals crd.Id
                                                                     where crd.UserType == Message.UserTypeUser && c.IsActive && ctf.TargetId == cred.Id
                                                                     && ctf.IsPending && ctf.IsActive
                                                                     orderby ctf.ChallengeDate descending
                                                                     select new PendingChallengeVM
                                                                     {
                                                                         ChallengeId = c.ChallengeId,
                                                                         ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                         ChallengeName = c.ChallengeName,
                                                                         DifficultyLevel = c.DifficultyLevel,
                                                                         ChallengeType = ct.ChallengeType,
                                                                         PersonalMessage = ctf.PersonalMessage,
                                                                         Duration = c.FFChallengeDuration,
                                                                         IsSubscription = c.IsSubscription,
                                                                         ThumbnailUrl = (from cexe in dataContext.CEAssociation
                                                                                         join ex in dataContext.Exercise
                                                                                         on cexe.ExerciseId equals ex.ExerciseId
                                                                                         where cexe.ChallengeId == c.ChallengeId
                                                                                         orderby cexe.RocordId
                                                                                         select ex.ThumnailUrl
                                                                                                     ).FirstOrDefault(),
                                                                         TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                                           join bp in dataContext.Equipments
                                                                                           on trzone.EquipmentId equals bp.EquipmentId
                                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                                           select bp.Equipment).Distinct().ToList<string>(),
                                                                         TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                                           join bp in dataContext.BodyPart
                                                                                           on trzone.PartId equals bp.PartId
                                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                                           select bp.PartName).Distinct().ToList<string>(),
                                                                         Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                         ResultUnit = ct.ResultUnit,
                                                                         ChallengeByUserName = ctf.ChallengeByUserName,  // For Only First Name only
                                                                         Result = ctf.Result,
                                                                         Fraction = ctf.Fraction,
                                                                         SubjectId = ctf.SubjectId,
                                                                         ResultUnitSuffix = ctf.ResultUnitSuffix,
                                                                         ChallengeToFriendId = ctf.ChallengeToFriendId,
                                                                         DbPostedDate = ctf.ChallengeDate,
                                                                         Description = c.Description,
                                                                         ProgramImageUrl = c.ProgramImageUrl,
                                                                         ChallengeByUserType = dataContext.Credentials.Where(cr => cr.Id == ctf.SubjectId).Select(uu => uu.UserType).FirstOrDefault()
                                                                     }).ToList();
                    objPendingList = new List<PendingChallengeVM>();
                    listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                          .Select(grp => grp.FirstOrDefault())
                                                                          .ToList();
                    float challengeBestTempResult = 0;
                    objPendingList = new List<PendingChallengeVM>();
                    foreach (PendingChallengeVM chalengeDetails in listPendingChallenge)
                    {
                        objTempPendingList = listPendingChallenge.Where(list => list.ChallengeId == chalengeDetails.ChallengeId && list.SubjectId == chalengeDetails.SubjectId).ToList();
                        string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + chalengeDetails.ProgramImageUrl;
                        chalengeDetails.ProgramImageUrl = (string.IsNullOrEmpty(chalengeDetails.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + chalengeDetails.ProgramImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + chalengeDetails.ProgramImageUrl : string.Empty;
                        if (!string.IsNullOrEmpty(chalengeDetails.ProgramImageUrl) && System.IO.File.Exists(filePath))
                        {
                            using (Bitmap objBitmap = new Bitmap(filePath))
                            {
                                double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                chalengeDetails.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                chalengeDetails.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                            }
                        }
                        else
                        {
                            chalengeDetails.Height = string.Empty;
                            chalengeDetails.Width = string.Empty;
                        }

                        //Add challenged user name First Name and Last Name and image url
                        if (chalengeDetails != null)
                        {
                            if (chalengeDetails.ChallengeByUserType == Message.UserTypeUser)
                            {
                                var userdetails = (from crd in dataContext.Credentials
                                                   join usr in dataContext.User
                                                   on crd.UserId equals usr.UserId
                                                   where crd.UserType == Message.UserTypeUser && crd.Id == chalengeDetails.SubjectId
                                                   select usr).FirstOrDefault();
                                if (userdetails != null)
                                {
                                    chalengeDetails.ChallengedUserImageUrl = string.IsNullOrEmpty(userdetails.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + userdetails.UserImageUrl;
                                    chalengeDetails.ChallengeByUserName = userdetails.FirstName + " " + userdetails.LastName;
                                    chalengeDetails.ChallengeByUserId = userdetails.UserId;

                                }
                            }
                            else if (chalengeDetails.ChallengeByUserType == Message.UserTypeTrainer)
                            {
                                var trainerdetails = (from crd in dataContext.Credentials
                                                      join usr in dataContext.Trainer
                                                      on crd.UserId equals usr.TrainerId
                                                      where crd.UserType == Message.UserTypeTrainer && crd.Id == chalengeDetails.SubjectId
                                                      select usr).FirstOrDefault();
                                if (trainerdetails != null)
                                {
                                    chalengeDetails.ChallengedUserImageUrl = string.IsNullOrEmpty(trainerdetails.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerdetails.TrainerImageUrl;
                                    chalengeDetails.ChallengeByUserName = trainerdetails.FirstName + " " + trainerdetails.LastName;
                                    chalengeDetails.ChallengeByUserId = trainerdetails.TrainerId;
                                }
                            }
                        }
                        PersonalChallengeVM challengeBestresult = TeamBL.GetGlobalPersonalBestResult(chalengeDetails.ChallengeId, chalengeDetails.SubjectId, dataContext);
                        if (chalengeDetails != null && !string.IsNullOrEmpty(chalengeDetails.ResultUnit))
                        {
                            switch (chalengeDetails.ResultUnit)
                            {
                                case ConstantHelper.constTime:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                            //Code for HH:MM:SS And MM:SS format
                                            string tempResult = r.Result;
                                            char[] splitChar = { ':' };
                                            string[] spliResult = tempResult.Split(splitChar);
                                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                            }
                                            else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                            }
                                        }
                                    });
                                    objTempPendingList = chalengeDetails.ChallengeSubTypeId == 6 ?
                                                 objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                                 : objTempPendingList.OrderBy(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     orderby usrResult.TempOrderIntValue ascending
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName,
                                                         Result = usrResult.Result
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        r.UserName = string.IsNullOrEmpty(r.UserName) ? string.Empty : r.UserName.Split(' ')[0];
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (chalengeDetails.ChallengeSubTypeId == 6)
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue >= challengeBestTempResult) ? true : false;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue > challengeBestTempResult) ? false : true;
                                    }
                                    objPendingList.Add(challengeresult);
                                    break;
                                case ConstantHelper.constReps:
                                case ConstantHelper.constWeight:
                                case ConstantHelper.constDistance:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : (r.Result.Replace(",", string.Empty));
                                            r.TempOrderIntValue = (float)Convert.ToDouble(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Replace(",", string.Empty).Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    objPendingList.Add(challengeresult);

                                    break;
                                case ConstantHelper.conRounds:
                                case ConstantHelper.constInterval:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Fraction))
                                        {
                                            string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                            r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                            r.Result = r.Result + " " + r.Fraction;
                                        }
                                        else
                                        {
                                            r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToInt16(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();

                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.UserName))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    objPendingList.Add(challengeresult);
                                    break;
                            }
                        }
                        else
                        {
                            if (chalengeDetails != null && (chalengeDetails.ChallengeType == ConstantHelper.FreeFormChallengeType || chalengeDetails.ChallengeType == ConstantHelper.ProgramChallengeType))
                            {
                                objPendingList.Add(chalengeDetails);
                            }
                        }
                    }
                    listPendingChallenge.ForEach(r =>
                    {
                        if (!string.IsNullOrEmpty(r.ChallengeType))
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        }
                        if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                        {
                            r.Equipment = string.Join(",", r.TempEquipments);
                        }
                        r.TempEquipments = null;
                        if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                        {
                            r.TargetZone = string.Join(",", r.TempTargetZone);
                        }
                        r.TempTargetZone = null;
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        // For Check Pending Program is active or based on user credital Id
                        if (r.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                        {
                            r.IsActiveProgram = dataContext.UserActivePrograms.Any(ap => ap.ProgramId == r.ChallengeId && ap.UserCredId == cred.Id && ap.IsCompleted == false);
                        }
                    });
                    // Update the Notification IsRead ststus
                    if (notificationID > 0)
                    {
                        NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    }

                    return objPendingList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPendingChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objTempPendingList = null;
                    userAndResult = null;
                }
            }
        }
        /// <summary>
        /// Get Notification Pending Challenge Description
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PendingChallengeVM GetNotificationPendingChallengeDescription(NotificationSenderVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            PendingChallengeVM chalengeDetails = null;
            List<PendingChallengeVM> objTempPendingList = null;
            List<ChallengeByUser> userAndResult = null;
            PendingChallengeVM challengeresult = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetNotificationPendingChallengeDescription---- " + DateTime.Now.ToLongDateString());
                    //  int challegeId, int senderUserID, string senderUserType, int senderCredID, long notificationID = 0;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<PendingChallengeVM> listPendingChallenge = null;

                    IQueryable<PendingChallengeVM> lstPendingChallenge = CommonReportingUtility.GetFriendNotificationChallengeDescription(dataContext, cred.Id, model);
                    if (model.ChallegeId > 0)
                    {
                        listPendingChallenge = lstPendingChallenge.Where(ch => ch.ChallengeId == model.ChallegeId).ToList();
                    }
                    else
                    {
                        listPendingChallenge = lstPendingChallenge.ToList();
                    }

                    listPendingChallenge.ForEach(r =>
                    {
                        if (!string.IsNullOrEmpty(r.Description))
                        {
                            r.Description = r.Description.Replace("<br />", "||br||");
                            const string HTML_TAG_PATTERN = "<.*?>";
                            r.Description = Regex.Replace(r.Description, HTML_TAG_PATTERN, string.Empty).Replace("&nbsp;", string.Empty);
                        }

                        // Provide height width of progarm
                        if (!string.IsNullOrEmpty(r.ProgramImageUrl))
                        {
                            string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + r.ProgramImageUrl;
                            r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + r.ProgramImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;
                            if (System.IO.File.Exists(filePath))
                            {
                                using (Bitmap objBitmap = new Bitmap(filePath))
                                {
                                    double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                    double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                    r.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                    r.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                                }
                            }
                            else
                            {
                                r.Height = string.Empty;
                                r.Width = string.Empty;
                            }
                        }
                        else
                        {
                            r.Height = string.Empty;
                            r.Width = string.Empty;
                        }
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                        {
                            r.TargetZone = string.Join(", ", r.TempTargetZone);
                        }
                        r.TempTargetZone = null;
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;

                    });
                    chalengeDetails = new PendingChallengeVM();
                    float challengeBestTempResult = 0;
                    if (listPendingChallenge != null && listPendingChallenge.Count() > 0)
                    {
                        objTempPendingList = listPendingChallenge.Where(list => list.ChallengeId == model.ChallegeId).ToList();
                        int chlngeId = 0;
                        int challengeByusercredId = 0;
                        if (chalengeDetails != null)
                        {
                            chalengeDetails = listPendingChallenge.FirstOrDefault();
                            chlngeId = chalengeDetails.ChallengeId;
                            challengeByusercredId = chalengeDetails.SubjectId;
                            var userdetails = ProfileApiBL.GetProfileDetailsByCredId(model.SenderCredID, model.SenderUserType);
                            if (userdetails != null)
                            {
                                chalengeDetails.ChallengedUserImageUrl = userdetails.ProfileImageUrl;
                                chalengeDetails.ChallengeByUserName = userdetails.UserName;
                                chalengeDetails.ChallengeByUserId = userdetails.UserId;

                            }
                        }
                        PersonalChallengeVM challengeBestresult = TeamBL.GetGlobalPersonalBestResult(chlngeId, challengeByusercredId, dataContext);
                        if (chalengeDetails != null && !string.IsNullOrEmpty(chalengeDetails.ResultUnit))
                        {
                            switch (objTempPendingList.FirstOrDefault().ResultUnit)
                            {
                                case ConstantHelper.constTime:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                            //Code for HH:MM:SS And MM:SS format
                                            string tempResult = r.Result;
                                            char[] splitChar = { ':' };
                                            string[] spliResult = tempResult.Split(splitChar);
                                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                            }
                                            else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                            }
                                        }
                                    });
                                    objTempPendingList = chalengeDetails.ChallengeSubTypeId == 6 ?
                                                 objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                                 : objTempPendingList.OrderBy(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     orderby usrResult.TempOrderIntValue ascending
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName,
                                                         Result = usrResult.Result
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        r.UserName = string.IsNullOrEmpty(r.UserName) ? string.Empty : r.UserName.Split(' ')[0];
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserName = chalengeDetails.ChallengeByUserName;
                                    challengeresult.ChallengeType = chalengeDetails.ChallengeType;
                                    //
                                    challengeresult.Duration = chalengeDetails.Duration;
                                    challengeresult.DifficultyLevel = chalengeDetails.DifficultyLevel;
                                    challengeresult.TargetZone = chalengeDetails.TargetZone;
                                    challengeresult.ThumbNailHeight = chalengeDetails.ThumbNailHeight;
                                    challengeresult.ThumbNailWidth = chalengeDetails.ThumbNailWidth;
                                    challengeresult.ThumbnailUrl = chalengeDetails.ThumbnailUrl;
                                    //

                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    if (chalengeDetails.ChallengeSubTypeId == 6)
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue >= challengeBestTempResult) ? true : false;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue > challengeBestTempResult) ? false : true;
                                    }
                                    chalengeDetails = challengeresult;
                                    break;
                                case ConstantHelper.constReps:
                                case ConstantHelper.constWeight:
                                case ConstantHelper.constDistance:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : (r.Result.Replace(",", string.Empty));
                                            r.TempOrderIntValue = (float)Convert.ToDouble(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });

                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserName = chalengeDetails.ChallengeByUserName;
                                    challengeresult.ChallengeType = chalengeDetails.ChallengeType;
                                    challengeresult.Duration = chalengeDetails.Duration;
                                    challengeresult.DifficultyLevel = chalengeDetails.DifficultyLevel;
                                    challengeresult.TargetZone = chalengeDetails.TargetZone;
                                    challengeresult.ThumbNailHeight = chalengeDetails.ThumbNailHeight;
                                    challengeresult.ThumbNailWidth = chalengeDetails.ThumbNailWidth;
                                    challengeresult.ThumbnailUrl = chalengeDetails.ThumbnailUrl;

                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Replace(",", string.Empty).Trim();
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    chalengeDetails = challengeresult;
                                    break;
                                case ConstantHelper.conRounds:
                                case ConstantHelper.constInterval:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Fraction))
                                        {
                                            string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                            r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                            r.Result = r.Result + " " + r.Fraction;
                                        }
                                        else
                                        {
                                            r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToInt16(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.UserName))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserName = chalengeDetails.ChallengeByUserName;
                                    challengeresult.ChallengeType = chalengeDetails.ChallengeType;
                                    challengeresult.Duration = chalengeDetails.Duration;
                                    challengeresult.DifficultyLevel = chalengeDetails.DifficultyLevel;
                                    challengeresult.TargetZone = chalengeDetails.TargetZone;
                                    challengeresult.ThumbNailHeight = chalengeDetails.ThumbNailHeight;
                                    challengeresult.ThumbNailWidth = chalengeDetails.ThumbNailWidth;
                                    challengeresult.ThumbnailUrl = chalengeDetails.ThumbnailUrl;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    chalengeDetails = challengeresult;
                                    break;
                            }
                        }
                    }
                    if (model.NotificationID > 0)
                    {
                        NotificationApiBL.UpdateNotificationReadStatus(dataContext, model.NotificationID);
                    }
                    if (chalengeDetails != null && chalengeDetails.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType)
                    {
                        chalengeDetails.IsWellness = true;
                    }
                    return chalengeDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetNotificationPendingChallengeDescription : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objTempPendingList = null;
                    userAndResult = null;
                }
            }
        }
        /// <summary>
        /// Delete PendingChallenge Result based on challengeToFriend ID
        /// </summary>
        /// <param name="challengeToFriendId"></param>
        /// <returns></returns>
        public static List<PendingChallengeVM> DeletePendingChallengeResult(int challengeToFriendId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeletePendingChallengeResult in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var userPendingChallengesresult = (from ufc in dataContext.ChallengeToFriends
                                                       where ufc.ChallengeToFriendId == challengeToFriendId && ufc.IsPending && ufc.TargetId == cred.Id
                                                       select ufc).FirstOrDefault();
                    if (userPendingChallengesresult != null)
                    {
                        dataContext.ChallengeToFriends.Remove(userPendingChallengesresult);
                        dataContext.SaveChanges();
                    }
                    return GetPendingChallengeList();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  DeletePendingChallengeResult in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Frend Challenge List
        /// </summary>
        /// <param name="notificationID"></param>
        /// <returns></returns>
        public static List<PendingChallengeVM> GetFreindChallengeList(long notificationID = 0)
        {
            StringBuilder traceLog = new StringBuilder();
            List<PendingChallengeVM> objPendingList = null;
            List<PendingChallengeVM> objTempPendingList = null;
            List<ChallengeByUser> userAndResult = null;
            PendingChallengeVM challengeresult = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetPendingChallengeList---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<PendingChallengeVM> listPendingChallenge = (from c in dataContext.Challenge
                                                                     join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                                     join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                     join ucrd in dataContext.Credentials
                                                                     on ctf.SubjectId equals ucrd.Id
                                                                     where c.IsActive
                                                                     && ctf.IsActive
                                                                     && ctf.TargetId == cred.Id
                                                                     && ucrd.UserType == Message.UserTypeUser
                                                                     && ctf.IsPending == true
                                                                     orderby ctf.ChallengeDate descending
                                                                     select new PendingChallengeVM
                                                                     {
                                                                         ChallengeId = c.ChallengeId,
                                                                         ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                         ChallengeName = c.ChallengeName,
                                                                         DifficultyLevel = c.DifficultyLevel,
                                                                         ChallengeType = ct.ChallengeType,
                                                                         PersonalMessage = ctf.PersonalMessage,
                                                                         Duration = c.FFChallengeDuration,
                                                                         IsSubscription = c.IsSubscription,
                                                                         ThumbnailUrl = (from cexe in dataContext.CEAssociation
                                                                                         join ex in dataContext.Exercise
                                                                                         on cexe.ExerciseId equals ex.ExerciseId
                                                                                         where cexe.ChallengeId == c.ChallengeId
                                                                                         orderby cexe.RocordId
                                                                                         select ex.ThumnailUrl
                                                                                                     ).FirstOrDefault(),
                                                                         TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                                           join bp in dataContext.Equipments
                                                                                           on trzone.EquipmentId equals bp.EquipmentId
                                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                                           select bp.Equipment).Distinct().ToList<string>(),
                                                                         TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                                           join bp in dataContext.BodyPart
                                                                                           on trzone.PartId equals bp.PartId
                                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                                           select bp.PartName).Distinct().ToList<string>(),
                                                                         Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                         ResultUnit = ct.ResultUnit,
                                                                         ChallengeByUserName = ctf.ChallengeByUserName,  // For Only First Name only
                                                                         Result = ctf.Result,
                                                                         Fraction = ctf.Fraction,
                                                                         SubjectId = ctf.SubjectId,
                                                                         ResultUnitSuffix = ctf.ResultUnitSuffix,
                                                                         ChallengeToFriendId = ctf.ChallengeToFriendId,
                                                                         DbPostedDate = ctf.ChallengeDate,
                                                                         Description = c.Description,
                                                                         ProgramImageUrl = c.ProgramImageUrl,
                                                                         ChallengeByUserType = dataContext.Credentials.Where(crd => crd.Id == ctf.SubjectId).Select(uu => uu.UserType).FirstOrDefault()
                                                                     }).ToList();
                    objPendingList = new List<PendingChallengeVM>();
                    listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                          .Select(grp => grp.FirstOrDefault())
                                                                          .ToList();
                    float challengeBestTempResult = 0;
                    objPendingList = new List<PendingChallengeVM>();
                    foreach (PendingChallengeVM chalengeDetails in listPendingChallenge)
                    {
                        objTempPendingList = listPendingChallenge.Where(list => list.ChallengeId == chalengeDetails.ChallengeId && list.SubjectId == chalengeDetails.SubjectId).ToList();
                        string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + chalengeDetails.ProgramImageUrl;
                        chalengeDetails.ProgramImageUrl = (string.IsNullOrEmpty(chalengeDetails.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + chalengeDetails.ProgramImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + chalengeDetails.ProgramImageUrl : string.Empty;
                        if (!string.IsNullOrEmpty(chalengeDetails.ProgramImageUrl) && System.IO.File.Exists(filePath))
                        {
                            using (Bitmap objBitmap = new Bitmap(filePath))
                            {
                                double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                chalengeDetails.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                chalengeDetails.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                            }
                        }
                        else
                        {
                            chalengeDetails.Height = string.Empty;
                            chalengeDetails.Width = string.Empty;
                        }
                        //Add challenged user name First Name and Last Name and image url
                        if (chalengeDetails != null)
                        {
                            if (chalengeDetails.ChallengeByUserType == Message.UserTypeUser)
                            {
                                var userdetails = (from crd in dataContext.Credentials
                                                   join usr in dataContext.User
                                                   on crd.UserId equals usr.UserId
                                                   where crd.UserType == Message.UserTypeUser && crd.Id == chalengeDetails.SubjectId
                                                   select usr).FirstOrDefault();
                                if (userdetails != null)
                                {
                                    chalengeDetails.ChallengedUserImageUrl = string.IsNullOrEmpty(userdetails.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + userdetails.UserImageUrl;
                                    chalengeDetails.ChallengeByUserName = userdetails.FirstName + " " + userdetails.LastName;
                                    chalengeDetails.ChallengeByUserId = userdetails.UserId;

                                }
                            }
                        }
                        PersonalChallengeVM challengeBestresult = TeamBL.GetGlobalPersonalBestResult(chalengeDetails.ChallengeId, chalengeDetails.SubjectId, dataContext);
                        if (chalengeDetails != null && !string.IsNullOrEmpty(chalengeDetails.ResultUnit))
                        {
                            switch (chalengeDetails.ResultUnit)
                            {
                                case ConstantHelper.constTime:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                            //Code for HH:MM:SS And MM:SS format
                                            string tempResult = r.Result;
                                            char[] splitChar = { ':' };
                                            string[] spliResult = tempResult.Split(splitChar);
                                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                            }
                                            else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                            }
                                        }
                                    });
                                    objTempPendingList = chalengeDetails.ChallengeSubTypeId == 6 ?
                                                 objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                                 : objTempPendingList.OrderBy(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     orderby usrResult.TempOrderIntValue ascending
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName,
                                                         Result = usrResult.Result
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        r.UserName = string.IsNullOrEmpty(r.UserName) ? string.Empty : r.UserName.Split(' ')[0];
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (chalengeDetails.ChallengeSubTypeId == 6)
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue >= challengeBestTempResult) ? true : false;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = (challengeresult.TempOrderIntValue > challengeBestTempResult) ? false : true;
                                    }
                                    objPendingList.Add(challengeresult);
                                    break;
                                case ConstantHelper.constReps:
                                case ConstantHelper.constWeight:
                                case ConstantHelper.constDistance:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : (r.Result.Replace(",", string.Empty));
                                            r.TempOrderIntValue = (float)Convert.ToDouble(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });

                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Replace(",", string.Empty).Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    objPendingList.Add(challengeresult);

                                    break;
                                case ConstantHelper.conRounds:
                                case ConstantHelper.constInterval:
                                    objTempPendingList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Fraction))
                                        {
                                            string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                            r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                            r.Result = r.Result + " " + r.Fraction;
                                        }
                                        else
                                        {
                                            r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToInt16(r.Result);
                                        }
                                    });
                                    objTempPendingList = objTempPendingList.OrderByDescending(k => k.TempOrderIntValue).ToList();

                                    //Get all username and its result in temp object
                                    userAndResult = (from usrResult in objTempPendingList
                                                     select new ChallengeByUser
                                                     {
                                                         UserName = usrResult.ChallengeByUserName ?? string.Empty,
                                                         Result = usrResult.Result ?? string.Empty
                                                     }).ToList();
                                    if (userAndResult != null)
                                        userAndResult = userAndResult.GroupBy(result => new { result.UserName }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    userAndResult.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.UserName))
                                        {
                                            r.UserName = r.UserName.Split(' ')[0];
                                        }
                                    });
                                    //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                    objTempPendingList = objTempPendingList.GroupBy(result => new { result.ChallengeId, result.SubjectId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                    objTempPendingList[0].ChallengeBy = userAndResult;
                                    challengeresult = objTempPendingList[0];
                                    challengeresult.ChallengedUserImageUrl = chalengeDetails.ChallengedUserImageUrl;
                                    challengeresult.ChallengeByUserId = chalengeDetails.ChallengeByUserId;
                                    challengeresult.ChallengeByUserType = chalengeDetails.ChallengeByUserType;
                                    challengeresult.PersonalMessage = chalengeDetails.PersonalMessage;
                                    if (challengeBestresult != null && !string.IsNullOrEmpty(challengeBestresult.Result))
                                    {
                                        challengeBestTempResult = challengeBestresult.TempOrderIntValue;
                                        challengeresult.PersonalBestResult = challengeBestresult.Result.Trim();
                                    }
                                    else
                                    {
                                        challengeresult.PersonalBestResult = string.Empty;
                                    }
                                    if (challengeresult.TempOrderIntValue >= challengeBestTempResult)
                                    {
                                        challengeresult.IsRecentChallengUserBest = true;
                                    }
                                    else
                                    {
                                        challengeresult.IsRecentChallengUserBest = false;
                                    }
                                    objPendingList.Add(challengeresult);
                                    break;
                            }
                        }
                        else
                        {
                            if (chalengeDetails != null && (chalengeDetails.ChallengeType == ConstantHelper.FreeFormChallengeType || chalengeDetails.ChallengeType == ConstantHelper.ProgramChallengeType))
                            {
                                objPendingList.Add(chalengeDetails);
                            }
                        }
                    }
                    listPendingChallenge.ForEach(r =>
                    {
                        if (!string.IsNullOrEmpty(r.ChallengeType))
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        }
                        if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                        {
                            r.Equipment = string.Join(",", r.TempEquipments);
                        }
                        r.TempEquipments = null;
                        if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                        {
                            r.TargetZone = string.Join(",", r.TempTargetZone);
                        }
                        r.TempTargetZone = null;
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        // For Check Pending Program is active or based on user credital Id
                        if (r.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                        {
                            r.IsActiveProgram = dataContext.UserActivePrograms.Any(ap => ap.ProgramId == r.ChallengeId && ap.UserCredId == cred.Id && ap.IsCompleted == false);
                        }
                    });

                    if (notificationID > 0)
                    {
                        NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    }

                    return objPendingList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPendingChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objTempPendingList = null;
                    userAndResult = null;
                }
            }
        }
    }
}