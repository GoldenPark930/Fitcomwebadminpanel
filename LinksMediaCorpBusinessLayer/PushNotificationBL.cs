namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpUtility.Resources;
    public class PushNotificationBL
    {
        /// <summary>
        /// validate devices user notification setting and send the notification to iOS and andriod app
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceToken"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        public static bool ValidateAndSendNotification(int userId, string deviceToken, string notificationType, string fullName, string deviceType,
            int totalpendingNotification, byte[] certificate, int totalbudget, string teamName, int teamjoinUserID = 0, int targetID = 0)
        {

            bool success = false;
            StringBuilder traceLog = new StringBuilder();
            string notificationMessage = string.Empty;
            NotificationType objNotificationType;
            PushNotificationiOSAndriod objPushNotificationiOSAndriod = null;
            try
            {
                traceLog.AppendLine("Start:ValidateNotificationExits() method");
                if (!Enum.TryParse(notificationType, out objNotificationType))
                {
                    objNotificationType = NotificationType.None;
                }
                int totalNt = 0;
                switch (objNotificationType)
                {
                    case NotificationType.TrainerJoinTeam:
                        notificationMessage = string.Format(Message.JoinTeamNotificationMsg, fullName, teamName);
                        break;
                    case NotificationType.TrainerChallege:
                        notificationMessage = string.Format(Message.TrainerNotificationMsg, fullName);
                        break;
                    case NotificationType.FriendChallege:
                        notificationMessage = string.Format(Message.FriendNotificationMsg, fullName);
                        break;
                    case NotificationType.UserSentToReceiver:
                        notificationMessage = string.Format(Message.UserSentToReceiverMsg, fullName);
                        break;

                    case NotificationType.NewsFeedBoomed:
                        {
                            totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                            --totalNt;
                            if (totalNt > 0)
                            {
                                notificationMessage = totalNt > 1 ? string.Format(Message.MoreNewsFeedBoomedMsg, fullName, totalNt, Message.NtPeople) :
                                    string.Format(Message.MoreNewsFeedBoomedMsg, fullName, totalNt, Message.NtPerson);
                            }
                            else
                            {
                                notificationMessage = string.Format(Message.NewsFeedBoomedMsg, fullName);
                            }
                            break;
                        }

                    case NotificationType.NewsFeedCommented:
                        {
                            totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                            --totalNt;
                            if (totalNt > 0)
                            {
                                notificationMessage = totalNt > 1 ? string.Format(Message.MoreNewsFeedCommentedMsg, fullName, totalNt, Message.NtPeople) : string.Format(Message.MoreNewsFeedCommentedMsg, fullName, totalNt, Message.NtPerson);
                            }
                            else
                            {
                                notificationMessage = string.Format(Message.NewsFeedCommentedMsg, fullName);
                            }

                        }
                        break;
                    case NotificationType.ResultFeedBoomed:
                        {
                            totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                            --totalNt;
                            if (totalNt > 0)
                            {
                                notificationMessage = totalNt > 1 ? string.Format(Message.MoreResultBoomedMsg, fullName, totalNt, Message.NtPeople) :
                                    string.Format(Message.MoreResultBoomedMsg, fullName, totalNt, Message.NtPerson);
                            }
                            else
                            {
                                notificationMessage = string.Format(Message.ResultBoomedMsg, fullName);
                            }
                            break;
                        }
                    case NotificationType.ResultCommented:
                        {
                            totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                            --totalNt;
                            if (totalNt > 0)
                            {
                                notificationMessage = totalNt > 1 ? string.Format(Message.MoreResultCommentedMsg, fullName, totalNt, Message.NtPeople) :
                                    string.Format(Message.MoreResultCommentedMsg, fullName, totalNt, Message.NtPerson);
                            }
                            else
                            {
                                notificationMessage = string.Format(Message.ResultCommentedMsg, fullName);
                            }
                            break;
                        }
                    case NotificationType.TrainerPostToUser:
                        notificationMessage = string.Format(Message.PersonalTrainerPostToUser, fullName);
                        break;
                    case NotificationType.PostCommentedReplyMsg:
                        totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                        --totalNt;
                        if (totalNt > 0)
                        {
                            notificationMessage = totalNt > 1 ? string.Format(Message.MorePostCommentReplyMsg, fullName, totalNt, Message.NtPeople) :
                                string.Format(Message.MorePostCommentReplyMsg, fullName, totalNt, Message.NtPerson);
                        }
                        else
                        {
                            notificationMessage = string.Format(Message.PostCommentReplyMsg, fullName);
                        }
                        break;
                    case NotificationType.PostResultReplyMsg:
                        totalNt = NotificationApiBL.GetALLlNotificationListByResultID(targetID, notificationType, userId);
                        --totalNt;
                        if (totalNt > 0)
                        {
                            notificationMessage = totalNt > 1 ? string.Format(Message.MoreResultCommentReplyMsg, fullName, totalNt, Message.NtPeople) :
                                string.Format(Message.MoreResultCommentReplyMsg, fullName, totalNt, Message.NtPerson);
                        }
                        else
                        {
                            notificationMessage = string.Format(Message.ResultCommentReplyMsg, fullName);
                        }
                        break;
                    case NotificationType.ProfilesPostToUser:
                        notificationMessage = string.Format(Message.ProfilePostNTMsg, fullName);
                        break;
                    case NotificationType.Following:
                        notificationMessage = string.Format(Message.FollowingNTMsg, fullName);
                        break;
                    case NotificationType.SelectPrimaryTrainer:
                        notificationMessage = string.Format(Message.UserSelectedPrimaryTrainer, fullName);
                        break;

                }
                objPushNotificationiOSAndriod = new PushNotificationiOSAndriod();
                if (deviceType.Equals(DeviceType.Android.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    success = objPushNotificationiOSAndriod.SendPushNotificationForAndriod(deviceToken, notificationMessage, notificationType, totalpendingNotification, totalbudget, teamjoinUserID);
                }
                else
                {
                    success = objPushNotificationiOSAndriod.SendPushNotificationForiOS(deviceToken, notificationMessage, notificationType, totalpendingNotification, certificate, totalbudget, teamjoinUserID);
                }
                return success;

            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                objPushNotificationiOSAndriod = null;
                traceLog.AppendLine("End ValidateNotificationExits() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }


        }

        /// <summary>
        /// Get user last login devices ID
        /// </summary>
        /// <param name="userCredID"></param>
        /// <returns></returns>
        public static List<UserNotification> GetLastUserDeviceID(int userCredID)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<UserNotification> userlastDevicesIDlist = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetLastUserDeviceID");
                    userlastDevicesIDlist = (from ut in dataContext.UserToken
                                             where ut.UserId == userCredID && (ut.IsExpired == false || ut.IsRememberMe == true)
                                             orderby ut.ExpiredOn descending
                                             select new UserNotification
                                             {
                                                 DeviceID = ut.TokenDevicesID,
                                                 UserCredID = ut.UserId,
                                                 DeviceType = ut.DeviceType,
                                                 TokenId = ut.Id
                                             }).Distinct().ToList();
                    if (userlastDevicesIDlist != null)
                    {
                        userlastDevicesIDlist = userlastDevicesIDlist.GroupBy(dt => new { dt.DeviceID }).Select(res => res.FirstOrDefault()).ToList();
                        userlastDevicesIDlist = userlastDevicesIDlist.OrderByDescending(nt => nt.TokenId).ToList();
                    }
                    return userlastDevicesIDlist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetLastUserDeviceID  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get User Total Friend PendingChallenges list count
        /// </summary>
        /// <param name="credID"></param>
        /// <returns></returns>
        public static int GetUserTotalFriendPendingChallenges(int credID)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                int pendingcount = 0;
                // List<PendingChallengeVM> listPendingChallenge = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserTotalFriendPendingChallenges-credID" + credID);
                    List<PendingChallengeVM> listPendingChallenge = (from c in dataContext.Challenge
                                                                     join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                                     join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                     join crd in dataContext.Credentials on ctf.SubjectId equals crd.Id
                                                                     where crd.UserType == Message.UserTypeUser && c.IsActive && ctf.TargetId == credID
                                                                     && ctf.IsPending && ctf.IsActive
                                                                     orderby ctf.ChallengeDate descending
                                                                     select new PendingChallengeVM
                                                                     {
                                                                         ChallengeId = c.ChallengeId,
                                                                         SubjectId = ctf.SubjectId
                                                                     }).ToList();
                    // Remove the duplicate the pending challenges
                    if (listPendingChallenge != null)
                    {
                        listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                              .Select(grp => grp.FirstOrDefault())
                                                                              .ToList();
                    }
                    // Total count of pending queue
                    if (listPendingChallenge != null)
                    {
                        pendingcount = listPendingChallenge.Count();
                    }

                    return pendingcount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetLastUserDeviceID  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credID"></param>
        /// <returns></returns>
        public static int GetMyFriendChallengeCount(int credID)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                int pendingcount = 0;
                // List<PendingChallengeVM> listPendingChallenge = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetMyFriendChallengeCount-credID" + credID);
                    List<PendingChallengeVM> listPendingChallenge = (from c in dataContext.Challenge
                                                                     join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                                     join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                     join crd in dataContext.Credentials on ctf.SubjectId equals crd.Id
                                                                     where c.IsActive && ctf.TargetId == credID && ctf.IsActive
                                                                     && ctf.IsPending
                                                                    && crd.UserType == Message.UserTypeUser
                                                                     orderby ctf.ChallengeDate descending
                                                                     select new PendingChallengeVM
                                                                     {
                                                                         ChallengeId = c.ChallengeId,
                                                                         SubjectId = ctf.SubjectId
                                                                     }).ToList();
                    // Remove the duplicate the pending challenges
                    if (listPendingChallenge != null)
                    {
                        listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                              .Select(grp => grp.FirstOrDefault())
                                                                              .ToList();
                    }
                    // Total count of pending queue
                    if (listPendingChallenge != null)
                    {
                        pendingcount = listPendingChallenge.Count();

                    }

                    return pendingcount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetMyFriendChallengeCount  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    //listPendingChallenge = null;
                }
            }


        }
        /// <summary>
        /// Get User Total Friend PendingChallenges list to user
        /// </summary>
        /// <param name="credID"></param>
        /// <returns></returns>
        public static List<PendingChallengeVM> GetTotalPendingChallengeToUser(int credID)
        {

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                //  string userlastDevicesID = string.Empty;
                List<PendingChallengeVM> listPendingChallenge = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTotalPendingChallengeToUser()");
                    if (credID > 0)
                    {
                        listPendingChallenge = (from c in dataContext.Challenge
                                                join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                join crd in dataContext.Credentials on ctf.SubjectId equals crd.Id
                                                where crd.UserType == Message.UserTypeUser && c.IsActive && ctf.TargetId == credID && ctf.IsActive
                                                && ctf.IsPending == true && ctf.IsActive
                                                orderby ctf.ChallengeDate descending
                                                select new PendingChallengeVM
                                                {
                                                    ChallengeId = c.ChallengeId,
                                                    ChallengeName = c.ChallengeName,
                                                    ChallengeByUserName = ctf.ChallengeByUserName
                                                }).ToList();
                        listPendingChallenge = new List<PendingChallengeVM>();
                        listPendingChallenge = listPendingChallenge.GroupBy(pl => pl.ChallengeId)
                                                                               .Select(grp => grp.FirstOrDefault())
                                                                               .ToList();
                    }

                    return listPendingChallenge;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTotalPendingChallengeToUser()  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;

                }
            }


        }
        /// <summary>
        /// Check user notification Status
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="devicesToken"></param>
        /// <param name="NotificationType"></param>
        /// <returns></returns>
        public static UserNotificationSettingVM GetNotificationStatus(int userID, string NotificationType, string deviceId, string devicesType)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                UserNotificationSettingVM objUserNotificationVM = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetNotificationSattus()");
                    objUserNotificationVM = (from us in dataContext.UserNotificationSetting
                                             where us.UserCredId == userID && us.NotificationType == NotificationType && us.DeviceID == deviceId && us.DeviceType == devicesType
                                             select new UserNotificationSettingVM
                                             {
                                                 DeviceID = us.DeviceID,
                                                 IsNotify = us.IsNotify
                                             }).FirstOrDefault();
                    return objUserNotificationVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetNotificationSattus  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;

                }

            }
        }
    }
}