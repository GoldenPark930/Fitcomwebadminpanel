namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Text;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System.Linq;
    using System.Web.Security.AntiXss;
    using System.Data.Entity;
    using LinksMediaCorpUtility.Resources;
    public class Login
    {
        #region ValidateUser
        /// <summary>
        /// Function to authenticate User
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>True/False</returns>
        public static bool ValidateUser(string username, string password, ref Credentials credential)
        {
            bool success = false;
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: Login, ValidateUser Method with Param UserName: " + username);
                    var item = _dbContext.Credentials.Where(m => m.EmailId.Equals(username.Trim()) && m.Password.Equals(password.Trim()) && m.UserType != Message.UserTypeUser).SingleOrDefault();
                    if (item != null)
                    {
                        success = true;
                        credential.UserId = item.UserId;
                        credential.UserType = item.UserType;
                        credential.Id = item.Id;
                        credential.LastLogin = item.LastLogin == null ? DateTime.Now : item.LastLogin;
                        switch (item.UserType)
                        {
                            case ConstantHelper.constUserTypeUser:
                                {
                                    credential.UserName = _dbContext.User.Where(usr => usr.UserId == item.UserId).Select(un => un.FirstName).FirstOrDefault();
                                }
                                break;
                            case ConstantHelper.constUserTypeTrainer:
                                {
                                    credential.UserName = _dbContext.Trainer.Where(usr => usr.TrainerId == item.UserId).Select(un => un.FirstName).FirstOrDefault();
                                }
                                break;
                            case ConstantHelper.constUserTypeWebAdmin:
                                {
                                    credential.UserName = ConstantHelper.constUserTypeWebAdminName;
                                }
                                break;
                            case ConstantHelper.constUserTypeWebTeam:
                                {
                                    credential.UserName = credential.UserName = _dbContext.Teams.Where(usr => usr.TeamId == item.UserId).Select(un => un.TeamName).FirstOrDefault();
                                    credential.UserName = string.IsNullOrEmpty(credential.UserName)?string.Empty:credential.UserName.Substring(1);
                                }
                                break;
                        }

                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("Index  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return success;
        }
        public static bool IsSessionExpire(string userName)
        {
            bool success = true;
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: check user session: " + userName);
                if (!string.IsNullOrEmpty(userName))
                {
                    success = false;
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
            }
        }


        #endregion

        public static bool UpdatePassword(ChangePassword model)
        {
            bool success = false;
            StringBuilder traceLog = new StringBuilder();
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    objEncrypt = new EncryptDecrypt();
                    traceLog.AppendLine("Start: Login, ValidateUser Method with Param UserName: ");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);

                    string password = objEncrypt.EncryptString(AntiXssEncoder.HtmlEncode(model.OldPassword.Trim(), false));
                    string newpassword = objEncrypt.EncryptString(AntiXssEncoder.HtmlEncode(model.NewPassword.Trim(), false));
                    tblCredentials credential = _dbContext.Credentials.FirstOrDefault(m => m.Id == credentialId && m.Password == password);
                    if (credential == null)
                    {
                        return success;
                    }
                    else
                    {
                        credential.Password = newpassword;
                        _dbContext.Entry(credential).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        success = true;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    objEncrypt.Dispose();
                    traceLog.AppendLine("Index  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return success;
        }
        public static void UpdateLastLogin()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateLastLogin");
                    if (System.Web.HttpContext.Current.Session != null)
                    {
                        int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                        tblCredentials credential = _dbContext.Credentials.Where(m => m.Id == credentialId).FirstOrDefault();
                        credential.LastLogin = DateTime.Now;
                        _dbContext.SaveChanges();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("UpdateLastLogin end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastLoginTime"></param>
        /// <returns></returns>
        public static String LastLogIn(DateTime? lastLoginTime)
        {
            string lastLogin = lastLoginTime.ToString();
            int index = lastLogin.IndexOf(' ');
            string time = lastLogin.Substring(index + 1);
            string dataDate = ((DateTime)lastLoginTime).Month + "/" + ((DateTime)lastLoginTime).Day + "/" + ((DateTime)lastLoginTime).Year;
            string currentDate = (DateTime.Now).Month + "/" + (DateTime.Now).Day + "/" + (DateTime.Now).Year;
            string yesterDay = (DateTime.Now).Month + "/" + (DateTime.Now.AddDays(-1)).Day + "/" + (DateTime.Now).Year;
            if (dataDate == currentDate)
            {
                lastLogin = "Today " + time;
            }
            else if (dataDate == yesterDay)
            {
                lastLogin = "Yesteday " + time;
            }
            int indexColen = lastLogin.LastIndexOf(':');
            int indexSpace = lastLogin.LastIndexOf(' ');
            string lastLoginDateTime = lastLogin.Substring(0, indexColen) + " " + lastLogin.Substring(indexSpace + 1);
            return lastLoginDateTime;
        }
        /// <summary>
        /// Convert to utc date to local date time
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ConvertToLocalTime(DateTime utcTime, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return utcTime;
            }
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Local);
            //   return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, timeZoneId);
        }
    }
}