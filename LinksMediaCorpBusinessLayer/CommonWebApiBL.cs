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
    using System.Web;
    using System.Configuration;
    using LinksMediaCorpUtility.Resources;
    public class CommonWebApiBL
    {

        /// <summary>
        /// Function to authenticate User base on user credetials and get authencation token
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>True/False</returns>
        public static bool ValidateUser(Credentials usercred, ref Credentials credential, ref string token, ref string userType, ref int userId)
        {
            bool success = false;
            StringBuilder traceLog = null;
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                tblUserToken objTokenInfo = null;
                tblCredentials item = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: Login, ValidateUser Method with Param UserName: " + usercred.UserName + usercred.DeviceID + usercred.DeviceType);
                    objEncrypt = new EncryptDecrypt();
                    string encPwd = objEncrypt.EncryptString(usercred.Password);
                    item = _dbContext.Credentials.FirstOrDefault(m => m.EmailId == usercred.UserName
                                                                      && m.Password == encPwd
                                                                       && (m.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                                                       || m.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)));
                    if (item != null)
                    {
                        traceLog.AppendLine("Start  to create user token");
                        success = true;
                        credential.UserId = item.UserId;
                        credential.UserType = item.UserType;
                        //Code for Last Login of User
                        item.LastLogin = DateTime.Now;
                        /*delete old user token on same devices except current user*/
                        if (usercred != null && string.IsNullOrEmpty(usercred.DeviceID))
                        {
                            DeleteOtherUserTokensOnDevices(usercred.DeviceID, usercred.DeviceType);
                        }
                        objTokenInfo = new tblUserToken();
                        objTokenInfo.UserId = item.Id;
                        objTokenInfo.Token = CommonUtility.GetToken();
                        objTokenInfo.ClientIPAddress = ConstantKey.DeviceID;
                        objTokenInfo.TokenDevicesID = string.IsNullOrEmpty(usercred.DeviceID) ? string.Empty : usercred.DeviceID;
                        objTokenInfo.DeviceType = string.IsNullOrEmpty(usercred.DeviceType) ? string.Empty : usercred.DeviceType;
                        objTokenInfo.IsExpired = false;
                        objTokenInfo.ExpiredOn = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ExpireDuration"]));
                        objTokenInfo.IsRememberMe = usercred.RememberMe;
                        objTokenInfo.DeviceUID = usercred.DeviceUID;
                        _dbContext.UserToken.Add(objTokenInfo);
                        _dbContext.SaveChanges();
                        if (string.IsNullOrEmpty(usercred.DeviceID))
                        {
                            NotificationApiBL.ResetUserNotification(item.Id);
                        }
                        token = objTokenInfo.Token;
                        userType = item.UserType;
                        userId = item.UserId;
                        traceLog.AppendLine("Token are created" + token);
                    }
                    return success;
                }
                finally
                {
                    objEncrypt.Dispose();
                    traceLog.AppendLine("Login  Start end() : --- " + DateTime.Now.ToLongDateString() + success.ToString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    item = null;
                    traceLog = null;
                    objTokenInfo = null;

                }
            }
        }
        /// <summary>
        /// Delete the old the authenication on same devices
        /// </summary>
        /// <param name="devicesId"></param>
        /// <param name="deviceUID"></param>
        /// <param name="devicesType"></param>
        /// <returns></returns>
        public static bool DeleteOtherUserTokensOnDevices(string devicesId, string devicesType)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                bool success = false;
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: DeleteOtherUserTokensOnDevices(), delete old existing token records: " + devicesId);
                    if (!string.IsNullOrWhiteSpace(devicesType) && devicesType == DiviceTypeType.Android.ToString())
                    {
                        string andriodDevicesType = DiviceTypeType.Android.ToString();
                        List<tblUserToken> userTokenlist = _dbContext.UserToken.Where(ut => ut.TokenDevicesID == devicesId && ut.DeviceType == andriodDevicesType).ToList();
                        /*delete accepted change by the user*/
                        if (userTokenlist != null)
                        {
                            _dbContext.UserToken.RemoveRange(userTokenlist);
                            _dbContext.SaveChanges();
                            success = true;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(devicesType) && devicesType == DiviceTypeType.IOS.ToString())
                    {
                        if (!string.IsNullOrWhiteSpace(devicesId))
                        {
                            List<tblUserToken> userTokenlist = _dbContext.UserToken.Where(ut => ut.TokenDevicesID == devicesId).ToList();
                            /*delete accepted change by the user*/
                            if (userTokenlist != null)
                            {
                                _dbContext.UserToken.RemoveRange(userTokenlist);
                                _dbContext.SaveChanges();
                                success = true;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return false;
                }
                finally
                {
                    traceLog.AppendLine(" DeleteOtherUserTokensOnDevices end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
                return success;
            }
        }
        /// <summary>
        /// Function to authenticate user Token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>True/False</returns>
        public static bool ValidateToken(string token, string clientIPAddress)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                bool success = false;
                StringBuilder traceLog = null;
                tblUserToken userToken = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: Login, ValidateUserToken Method with Param token: " + token);
                    userToken = _dbContext.UserToken.FirstOrDefault(t => t.Token == token && t.ClientIPAddress == clientIPAddress);
                    if (userToken != null)
                    {
                        if (userToken.IsRememberMe)
                        {
                            success = true;
                        }
                        else
                        {
                            if (userToken.ExpiredOn > DateTime.Now)
                            {
                                if (!userToken.IsExpired)
                                {
                                    success = true;
                                    userToken.ExpiredOn = DateTime.Now.AddMinutes(Convert.ToByte(ConfigurationManager.AppSettings["ExpireDuration"]));
                                }
                            }
                            else
                            {
                                userToken.IsExpired = true;
                            }
                        }

                        _dbContext.SaveChanges();
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
                    traceLog.AppendLine("Index  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    userToken = null;
                }
            }
        }

        /// <summary>
        /// Function to get Credentials object of Current User on the basis of Token Id
        /// </summary>
        /// <param name="token">string</param>
        /// <returns>Credentials</returns>
        public static Credentials GetUserId(string token)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                Credentials objCredential = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserId");
                    objCredential = (from c in _dbContext.Credentials
                                     join ut in _dbContext.UserToken on c.Id equals ut.UserId
                                     where ut.Token == token
                                     select new Credentials
                                     {
                                         Id = c.Id,
                                         UserId = c.UserId,
                                         UserType = c.UserType,
                                         DeviceID = ut.TokenDevicesID,
                                         DeviceType = ut.DeviceType
                                     }).FirstOrDefault();
                    return objCredential;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserId --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objCredential = null;
                }
            }
        }
        /// <summary>
        /// Function to get Date time format like IOS App
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>String</returns>
        public static string GetDateTimeFormat(DateTime date)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetDateTimeFormat");
                    DateTime postedDate = date;
                    // string dateStr = postedDate.Date.Equals(DateTime.Now.Date) ? "Today" : (postedDate.AddDays(1).Date.Equals(DateTime.Now.Date) ? "Yesterday" : "Other");
                    string dateStr = postedDate.Date.Equals(DateTime.Now.Date) ? Message.msgToday.ToString() :
                        (postedDate.AddDays(1).Date.Equals(DateTime.Now.Date) ? Message.msgYesterday.ToString() : Message.msgOther.ToString());
                    string result = string.Empty;
                    switch (dateStr)
                    {
                        case ConstantHelper.constDateFormatToday:
                            result = dateStr + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                        case ConstantHelper.constDateFormatYesterday:
                            result = dateStr + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                        default:
                            result = date.ToString(ConstantKey.constMMddyyyy) + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                    }
                    return result;
                }
                finally
                {
                    traceLog.AppendLine("End: GetDateTimeFormat --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Function to get Date time format like IOS App
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>String</returns>
        public static string GetDateTimeFormatForWeb(DateTime date)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetDateTimeFormat");
                    DateTime localdate = ConvertToLocalTime(date, TimeZoneInfo.Local.Id);
                    string result = string.Empty;
                    DateTime currentlocaldate = ConvertToLocalTime(DateTime.Now, TimeZoneInfo.Local.Id);
                    string dateStr = localdate.Date.Equals(currentlocaldate) ? Message.msgToday.ToString() :
                        (localdate.AddDays(1).Date.Equals(currentlocaldate) ? Message.msgYesterday.ToString() : Message.msgOther.ToString());
                    switch (dateStr)
                    {
                        case ConstantHelper.constDateFormatToday:
                            result = dateStr + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                        case ConstantHelper.constDateFormatYesterday:
                            result = dateStr + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                        default:
                            result = date.ToString(ConstantKey.constMMddyyyy) + " " + Message.msgat.ToString() + " " + date.ToString(ConstantKey.consthhmmtt);
                            break;
                    }
                    return result;
                }
                finally
                {
                    traceLog.AppendLine("End: GetDateTimeFormat --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Convert utc time to local time based on timeZoneId
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        private static DateTime ConvertToLocalTime(DateTime utcTime, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return utcTime;
            }
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Local);
        }
        /// <summary>
        /// Function to delete current user token
        /// </summary>
        /// <param></param>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/19/2015
        /// </devdoc>
        public static bool DeleteCurrentUserToken()
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                tblUserToken userToken = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: DeleteCurrentUserToken");
                    string token = Thread.CurrentPrincipal.Identity.Name;
                    bool flag = false;
                    userToken = _dbContext.UserToken.Remove(_dbContext.UserToken.FirstOrDefault(u => u.Token.Equals(token)));
                    _dbContext.SaveChanges();
                    if (userToken != null)
                    {
                        flag = true;
                    }
                    return flag;
                }
                finally
                {
                    traceLog.AppendLine("End: DeleteCurrentUserToken --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    userToken = null;
                }
            }
        }
        /// <summary>
        /// Function to change password
        /// </summary>
        /// <param></param>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/24/2015
        /// </devdoc>
        public static bool ChangePassword(EditPasswordVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                EncryptDecrypt objEncrypt = null;
                Credentials cred = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChangePassword");
                    bool flag = false;
                    cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (cred != null)
                    {
                        if (IsOldPasswordCorrect(model.OldPassword, cred.Id))
                        {
                            objEncrypt = new EncryptDecrypt();
                            tblCredentials objCredential = _dbContext.Credentials.Where(crd => crd.Id == cred.Id).FirstOrDefault();
                            model.NewPassword = objEncrypt.EncryptString(model.NewPassword);
                            if (objCredential != null)
                            {
                                objCredential.Password = model.NewPassword;
                            }
                            _dbContext.SaveChanges();
                            flag = true;
                        }
                    }
                    return flag;
                }
                finally
                {
                    if (objEncrypt != null)
                    {
                        objEncrypt.Dispose();
                        objEncrypt = null;
                    }
                    traceLog.AppendLine("End: ChangePassword --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to  check old password is correct or not
        /// </summary>
        /// <param></param>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/24/2015
        /// </devdoc>
        private static bool IsOldPasswordCorrect(string oldPwd, int credId)
        {
            StringBuilder traceLog = null;
            bool flag = false;
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                tblCredentials objCredential = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: IsOldPasswordCorrect");
                    objEncrypt = new EncryptDecrypt();
                    string encPwd = objEncrypt.EncryptString(oldPwd);
                    objCredential = _dbContext.Credentials.Where(crd => crd.Id == credId && crd.Password == encPwd).FirstOrDefault();
                    if (objCredential != null)
                    {
                        flag = true;
                    }

                    return flag;
                }
                finally
                {
                    objEncrypt.Dispose();
                    traceLog.AppendLine("End: IsOldPasswordCorrect --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    objCredential = null;
                }
            }
        }
        /// <summary>
        /// SaveFile on existing path
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        public static void SaveFile(byte[] content, string path)
        {
            //Save file
            using (FileStream str = File.Create(path))
            {
                str.Write(content, 0, content.Length);
            }
        }
        /// <summary>
        ///  Get exercise thumnail if avaialble otherwise create aand get path
        /// </summary>
        /// <param name="exerciseVideoPath"></param>
        /// <param name="thumnailName"></param>
        /// <returns></returns>
        public static string GetSaveExerciseThumbnail(string exercisePath, string thumnailName)
        {
            StringBuilder traceLog = null;
            string exercisethumnailNamePath = string.Empty;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetSaveExerciseThumbnail");
                if (!string.IsNullOrEmpty(exercisePath) && !string.IsNullOrEmpty(thumnailName))
                {
                    string root = HttpContext.Current.Server.MapPath("~/videos/exerciseThumbnails");
                    thumnailName = thumnailName.Replace(" ", string.Empty);
                    string thumnailNamePath = thumnailName + Message.JpgImageExtension;
                    if (!string.IsNullOrEmpty(thumnailNamePath) && !File.Exists(root + ConstantHelper.constDoubleBackSlash + thumnailNamePath))
                    {
                        FFMPEG OBJfFMPEG = new FFMPEG();
                        OBJfFMPEG.SaveThumbnail(exercisePath, root + ConstantHelper.constDoubleBackSlash + thumnailNamePath);
                        exercisethumnailNamePath = CommonUtility.VirtualPath + Message.ExerciseThumbnailDirectory + thumnailNamePath;
                    }
                    else
                    {
                        exercisethumnailNamePath = CommonUtility.VirtualPath + Message.ExerciseThumbnailDirectory + thumnailNamePath;
                    }
                }
                return exercisethumnailNamePath;
            }
            catch
            {
                return exercisethumnailNamePath;
            }
            finally
            {
                traceLog.AppendLine("End: GetSaveExerciseThumbnail --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }


        }
        /// <summary>
        /// Upadte or Save DevicesNotification count for user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static long SaveDevicesNotification(UserNotificationsVM model)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetSaveDevicesNotification");
                    int userCredetials = 0;
                    var objCredentials = _dbContext.Credentials.Where(user => user.UserId == model.UserID && user.UserType.Equals(model.UserType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (objCredentials != null)
                    {
                        userCredetials = objCredentials.Id;
                    }
                    if (userCredetials != model.ReceiverCredID && !_dbContext.UserNotifications.Any(f => f.SenderCredlID == userCredetials
                        && f.SenderCredlID == model.ReceiverCredID && f.ReceiverCredID == model.ReceiverCredID && f.NotificationType == model.NotificationType
                        && f.TargetID == model.TargetPostID && f.TokenDevicesID == model.TokenDevicesID))
                    {
                        tblUserNotifications objdeviceNft = new tblUserNotifications
                        {
                            SenderCredlID = userCredetials,
                            ReceiverCredID = model.ReceiverCredID,
                            NotificationType = model.NotificationType,
                            SenderUserName = model.SenderUserName,
                            Status = true,
                            IsRead = false,
                            TargetID = model.TargetPostID,
                            CreatedDate = model.CreatedNotificationUtcDateTime,
                            TokenDevicesID = model.TokenDevicesID,
                            ChallengeToFriendId = model.ChallengeToFriendId,
                            IsFriendChallenge = model.IsFriendChallenge,
                            IsOnBoarding = model.IsOnBoarding
                        };
                        _dbContext.UserNotifications.Add(objdeviceNft);
                        _dbContext.SaveChanges();
                        return objdeviceNft.NotificationID;
                    }
                    return 0;
                }
                finally
                {
                    traceLog.AppendLine("End: GetSaveDevicesNotification --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// Count Total USer Notification for user
        /// </summary>
        /// <param name="userCredID"></param>
        /// <returns></returns>
        public static int GetTotalUSerNotification(int userCredID, string deviceId)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetSaveDevicesNotification");
                    int totalcountNotification = _dbContext.UserNotifications.Count(user => user.ReceiverCredID == userCredID && user.Status && user.TokenDevicesID == deviceId);
                    return totalcountNotification;
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    traceLog.AppendLine("End: GetSaveDevicesNotification --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// Remove notification incash fail to send notification
        /// </summary>
        /// <param name="userNotificationID"></param>
        /// <returns></returns>
        public static bool RemoveUserNotification(long userNotificationID)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetSaveDevicesNotification");
                    var objdeviceNft = _dbContext.UserNotifications.Where(nt => nt.NotificationID == userNotificationID).FirstOrDefault();
                    _dbContext.UserNotifications.Remove(objdeviceNft);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    traceLog.AppendLine("End: GetSaveDevicesNotification --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// Update Device Token And Type based on AuthToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateDeviceTokenAndType(DeviceTokenVM model)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: UpdateDeviceTokenAndType()");
                    if (!string.IsNullOrEmpty(model.AuthToken))
                    {
                        var userToken = _dbContext.UserToken.Where(dn => dn.Token.Equals(model.AuthToken.Trim())).FirstOrDefault();
                        if (userToken != null)
                        {
                            userToken.TokenDevicesID = model.DeviceToken;
                            userToken.DeviceType = model.DeviceType.ToString();
                            _dbContext.SaveChanges();
                            return true;
                        }
                    }
                    return false;
                }
                finally
                {
                    traceLog.AppendLine("End: UpdateDeviceTokenAndType --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UTCPostMessage(UTCMessageVM model)
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: UTCPostMessage");
                    tblUTCPostMessage objUTCPostMessage = new tblUTCPostMessage
                    {
                        PostMessage = model.Message,
                        PostDateTime = model.PostedDate
                    };
                    _dbContext.UTCPostMessage.Add(objUTCPostMessage);
                    _dbContext.SaveChanges();
                    return true;
                }
                finally
                {
                    traceLog.AppendLine("End: UTCPostMessage --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// Get UTCPostMessage
        /// </summary>
        /// <returns></returns>
        public static List<UTCMessageVM> GetUTCPostMessage()
        {
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    List<UTCMessageVM> UTCMessageList = (from utcm in _dbContext.UTCPostMessage
                                                         select new UTCMessageVM
                                                         {
                                                             Message = utcm.PostMessage,
                                                             PostedDate = utcm.PostDateTime
                                                         }).ToList();

                    return UTCMessageList;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUTCPostMessage --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }

        }
        /// <summary>
        /// Get Thumnail Image from  posted video 
        /// </summary>
        /// <param name="thumnailpath"></param>
        /// <param name="thumnailHeight"></param>
        /// <param name="thumnailWidth"></param>
        public static void GetThumNailDemension(string thumnailFileName, out string thumnailHeight, out string thumnailWidth)
        {
            try
            {
                string root = HttpContext.Current.Server.MapPath("~/videos/result");
                if (!string.IsNullOrEmpty(thumnailFileName) && File.Exists(root + ConstantHelper.constDoubleBackSlash + thumnailFileName))
                {
                    using (System.Drawing.Image objImage = System.Drawing.Image.FromFile(root + ConstantHelper.constDoubleBackSlash + thumnailFileName))
                    {
                        thumnailHeight = Convert.ToString(objImage.Height);
                        thumnailWidth = Convert.ToString(objImage.Width);
                    }
                    return;
                }
                else
                {
                    thumnailHeight = string.Empty;
                    thumnailWidth = string.Empty;
                }
            }
            catch
            {
                thumnailHeight = string.Empty;
                thumnailWidth = string.Empty;
            }
        }

       
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firststring"></param>
        /// <param name="secondString"></param>
        /// <returns></returns>
        public static string GetConcateString(string firststring, string secondString)
        {
            return firststring + ", " + secondString;
        }


    }
}