using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpEntity.WebApiVM;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
namespace LinksMediaCorpBusinessLayer
{
    public class ChatHistoryApiBL
    {
        private static object syncLock = new object();
        /// <summary>
        ///  Get ChatHistory based on Sender and Receiver Id
        /// </summary>
        /// <param name="chatRequest"></param>
        /// <returns></returns>
        public static Total<List<SocketSentChatVM>> GetChatHistory(ChatHistoryRequest chatRequest, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            Total<List<SocketSentChatVM>> chatHistoryList = new Total<List<SocketSentChatVM>>();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: GetChatHistory() for receiver Id");
                    if (chatRequest != null && chatRequest.SenderCredId > 0)
                    {
                        Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        int currentUserCrdId = cred.Id;
                        int senderCredId = chatRequest.SenderCredId;
                        List<SocketSentChatResponse> chatlist = (from ch in dataContext.ChatHistory
                                                                 where (ch.ReceiverCredId == currentUserCrdId && ch.SenderCredId == senderCredId) || (ch.ReceiverCredId == senderCredId && ch.SenderCredId == currentUserCrdId)
                                                                 orderby ch.ChatHistoryId descending
                                                                 select new SocketSentChatResponse
                                                                 {
                                                                     ChatHistoryId = ch.ChatHistoryId,
                                                                     Message = ch.Message,
                                                                     ReceiverEmailId = dataContext.Credentials.Where(crd => crd.Id == ch.ReceiverCredId).Select(s => s.EmailId).FirstOrDefault(),
                                                                     SenderEmailId = dataContext.Credentials.Where(crd => crd.Id == ch.SenderCredId).Select(s => s.EmailId).FirstOrDefault(),
                                                                     TrasactionDateTime = ch.TrasactionDateTime
                                                                 }).ToList();

                        if (chatlist != null)
                        {
                            var chathistorylist = chatlist.Skip(startIndex).Take(endIndex - startIndex).ToList().OrderBy(ch => ch.ChatHistoryId).ToList();
                            chatHistoryList.TotalList = (from m in chathistorylist
                                                         orderby m.ChatHistoryId
                                                         select new SocketSentChatVM
                                                         {
                                                             ChatHistoryId = m.ChatHistoryId,
                                                             Message = m.Message,
                                                             ReceiverEmailId = m.ReceiverEmailId,
                                                             SenderEmailId = m.SenderEmailId,
                                                             TrasactionDateTime = m.TrasactionDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff")
                                                         }).ToList();


                            chatHistoryList.TotalCount = chatlist.Count();
                            if ((chatHistoryList.TotalCount) > endIndex)
                            {
                                chatHistoryList.IsMoreAvailable = true;
                            }
                        }
                        // Update IsRead when user
                        if (chatRequest.IsRead || currentUserCrdId > 0)
                        {
                            UpdateChatHistory(currentUserCrdId, senderCredId);
                            UpdateChatNotification(currentUserCrdId, senderCredId);
                        }

                    }
                    return chatHistoryList;
                }
                finally
                {
                    traceLog.AppendLine("GetChatHistory  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update Chat History
        /// </summary>
        /// <param name="currentUserCrdId"></param>
        /// <param name="senderCredId"></param>
        private static void UpdateChatHistory(int currentUserCrdId, int senderCredId)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                lock (syncLock)
                {
                    StringBuilder traceLog = null;
                    using (LinksMediaContext dataContext = new LinksMediaContext())
                    {
                        try
                        {
                            traceLog = new StringBuilder();
                            traceLog.AppendLine("Start: UpdateChatHistory()");
                            List<tblChatHistory> chatlist = (from ch in dataContext.ChatHistory
                                                             where ch.ReceiverCredId == currentUserCrdId && ch.SenderCredId == senderCredId && ch.IsRead == false
                                                             orderby ch.TrasactionDateTime descending
                                                             select ch
                                                                 ).ToList();
                            chatlist.ForEach(ch =>
                            {
                                ch.IsRead = true;
                            });
                            dataContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogManagerInstance.WriteErrorLog(ex);
                        }
                        finally
                        {
                            traceLog.AppendLine("End: UpdateChatHistory()  --- " + DateTime.Now.ToLongDateString());
                            LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        }
                    }
                }
            }).Start();

        }

        /// <summary>
        /// Update Chat Notification based on sender and receiver Id
        /// </summary>
        /// <param name="currentUserCrdId"></param>
        /// <param name="senderCredId"></param>
        private static void UpdateChatNotification(int currentUserCrdId, int senderCredId)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;
                lock (syncLock)
                {
                    StringBuilder traceLog = null;
                    using (LinksMediaContext dataContext = new LinksMediaContext())
                    {
                        try
                        {
                            traceLog = new StringBuilder();
                            //  string notificationType = string.Empty;
                            traceLog.AppendLine("Start: UpdateChatNotification()");
                            string chatNotificationtype = NotificationType.ChatNotification.ToString();
                            List<tblUserNotifications> chatlist = (from ch in dataContext.UserNotifications
                                                                   where ch.NotificationType == chatNotificationtype && ch.ReceiverCredID == currentUserCrdId && ch.SenderCredlID == senderCredId && ch.IsRead == false
                                                                   select ch
                                                                   ).ToList();
                            chatlist.ForEach(ch =>
                            {
                                ch.IsRead = true;
                            });
                            dataContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogManagerInstance.WriteErrorLog(ex);
                        }
                        finally
                        {
                            traceLog.AppendLine("End: UpdateChatNotification()  --- " + DateTime.Now.ToLongDateString());
                            LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        }
                    }
                }
            }).Start();

        }
        /// <summary>
        /// Save Chat History
        /// </summary>
        /// <param name="chatHistory"></param>
        /// <returns></returns>
        public static void SaveChatHistory(ChatHistoryVM chatHistory)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: SaveChatHistory() for receiver Id");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (chatHistory != null && chatHistory.ReceiverCredId > 0 && chatHistory.SenderCredId > 0 && !string.IsNullOrEmpty(chatHistory.Message))
                    {
                        tblChatHistory objChatHistory = new tblChatHistory()
                        {
                            ReceiverCredId = chatHistory.ReceiverCredId,
                            SenderCredId = cred.Id,
                            Message = chatHistory.Message,
                            IsRead = false,
                            TrasactionDateTime = DateTime.UtcNow,
                        };
                        dataContext.ChatHistory.Add(objChatHistory);
                        dataContext.SaveChanges();
                    }
                    return;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("SaveChatHistory  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Save ChatHistory FromSocket
        /// </summary>
        /// <param name="chatHistory"></param>
        public static void SaveChatHistoryFromSocket(SocketSentChatVM chatHistory)
        {
            try
            {
                SendChatNotification(chatHistory);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credUserId"></param>
        /// <param name="message"></param>
        private static void SendChatNotification(SocketSentChatVM chatHistory)
        {
            byte[] certificate = File.ReadAllBytes(chatHistory.CeritifcatePath);
            new Thread(() =>
            {

                Thread.CurrentThread.IsBackground = false;
                lock (syncLock)
                {
                    StringBuilder traceLog = null;
                    List<UserNotification> allactiveDevices = null;
                    using (LinksMediaContext dataContext = new LinksMediaContext())
                    {
                        try
                        {
                            traceLog = new StringBuilder();
                            traceLog.Append("Start:SendChatNotification()");
                            string notificationType = string.Empty;
                            traceLog.AppendLine("Start: SendChallegesNotificationToUser()");

                            List<tblCredentials> chatsenderReceiverDetails = dataContext.Credentials.Where(em => string.Compare(em.EmailId, chatHistory.ReceiverEmailId, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(em.EmailId, chatHistory.SenderEmailId, StringComparison.OrdinalIgnoreCase) == 0).ToList();
                            int receiverCredId = 0;
                            int senderCredId = 0;
                            string senderUserType = string.Empty;
                            if (chatsenderReceiverDetails != null)
                            {
                                receiverCredId = chatsenderReceiverDetails.Where(em => em.EmailId.Equals(chatHistory.ReceiverEmailId, StringComparison.OrdinalIgnoreCase)).Select(cr => cr.Id).FirstOrDefault();
                                senderCredId = chatsenderReceiverDetails.Where(em => em.EmailId.Equals(chatHistory.SenderEmailId, StringComparison.OrdinalIgnoreCase)).Select(cr => cr.Id).FirstOrDefault();
                                senderUserType = chatsenderReceiverDetails.Where(em => em.EmailId.Equals(chatHistory.SenderEmailId, StringComparison.OrdinalIgnoreCase)).Select(cr => cr.UserType).FirstOrDefault();
                            }

                            if (chatHistory != null && receiverCredId > 0 && senderCredId > 0 && !string.IsNullOrEmpty(chatHistory.Message))
                            {
                                bool isRead = false;
                                if (!chatHistory.IsOffine)
                                {
                                    isRead = true;
                                }
                                tblChatHistory objChatHistory = new tblChatHistory()
                                {
                                    ReceiverCredId = receiverCredId,
                                    SenderCredId = senderCredId,
                                    Message = chatHistory.Message,
                                    IsRead = isRead,
                                    TrasactionDateTime = Convert.ToDateTime(chatHistory.TrasactionDateTime),
                                };
                                dataContext.ChatHistory.Add(objChatHistory);
                                dataContext.SaveChanges();
                                if (chatHistory.IsOffine)
                                {
                                    string message = string.Empty;
                                    allactiveDevices = PushNotificationBL.GetLastUserDeviceID(receiverCredId);
                                    string senderFirstName = string.Empty;
                                    if (senderUserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                                    {
                                        senderFirstName = (from usr in dataContext.User
                                                           join crd in dataContext.Credentials
                                                           on usr.UserId equals crd.UserId
                                                           where crd.Id == senderCredId && crd.UserType == Message.UserTypeUser
                                                           select usr.FirstName + " " + usr.LastName).FirstOrDefault();
                                    }
                                    else if (senderUserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                                    {
                                        senderFirstName = (from usr in dataContext.Trainer
                                                           join crd in dataContext.Credentials
                                                           on usr.TrainerId equals crd.UserId
                                                           where crd.Id == senderCredId && crd.UserType == Message.UserTypeTrainer
                                                           select usr.FirstName + " " + usr.LastName).FirstOrDefault();
                                    }
                                    message = senderFirstName + " sent you a message!";
                                    bool isSaveSuccessfully = false;
                                    foreach (UserNotification objuserNotification in allactiveDevices)
                                    {
                                        // If Deives Token null then by pass to sending push notification
                                        if (!string.IsNullOrEmpty(objuserNotification.DeviceID))
                                        {
                                            if (!isSaveSuccessfully)
                                            {
                                                notificationType = ConstantHelper.constChatNotification;
                                                tblUserNotifications objdeviceNft = new tblUserNotifications
                                                {
                                                    SenderCredlID = senderCredId,
                                                    ReceiverCredID = receiverCredId,
                                                    NotificationType = notificationType,
                                                    SenderUserName = senderFirstName,
                                                    Status = true,
                                                    IsRead = false,
                                                    TargetID = 0,
                                                    CreatedDate = DateTime.Parse(chatHistory.TrasactionDateTime),
                                                    TokenDevicesID = objuserNotification.DeviceID
                                                };
                                                dataContext.UserNotifications.Add(objdeviceNft);
                                                dataContext.SaveChanges();
                                                isSaveSuccessfully = true;
                                            }
                                            PushNotificationiOSAndriod objPushNotificationiOSAndriod = new PushNotificationiOSAndriod();
                                            int totalNotificationcount = dataContext.ChatHistory.Count(ch => ch.ReceiverCredId == receiverCredId && ch.IsRead == false);
                                            int totalbudget = CommonWebApiBL.GetTotalUSerNotification(receiverCredId, objuserNotification.DeviceID);
                                            if (objuserNotification.DeviceType.Equals(DeviceType.IOS.ToString(), StringComparison.OrdinalIgnoreCase))
                                            {
                                                objPushNotificationiOSAndriod.SendPushNotificationForiOS(objuserNotification.DeviceID, message, notificationType, totalNotificationcount, certificate, totalbudget, 0);
                                            }
                                            else if (objuserNotification.DeviceType.Equals(DeviceType.Android.ToString(), StringComparison.OrdinalIgnoreCase))
                                            {
                                                objPushNotificationiOSAndriod.SendPushNotificationForAndriod(objuserNotification.DeviceID, message, notificationType, totalNotificationcount, totalbudget, 0);

                                            }
                                        }
                                    }
                                    message = string.Empty;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogManagerInstance.WriteErrorLog(ex);
                        }
                        finally
                        {
                            traceLog.AppendLine("End: SendChatNotification()  --- " + DateTime.Now.ToLongDateString());
                            LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                            traceLog = null;
                            allactiveDevices = null;
                        }
                    }
                }
            }).Start();

        }

    }
}
