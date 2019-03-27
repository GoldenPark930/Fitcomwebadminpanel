namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using System;
    using System.Text;
    using System.Web.Http;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    public class LoginController : ApiController
    {
        /// <summary>
        /// Action to Authenticate users
        /// </summary>
        /// <returns>ServiceResponse<Token></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult Login(Credentials credInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Token> objToken = null;
            string token = string.Empty;
            string userType = string.Empty;
            int userID = 0;
            bool success = false;
            Credentials objOut = null;
            try
            {
                traceLog = new StringBuilder();
                objToken = new ServiceResponse<Token>();
                objOut = new Credentials();
                traceLog.AppendLine("Start: Login, LoginUser Method" + credInfo.UserName + "-" + credInfo.Password + "DeviceID-" + credInfo.DeviceID + "DeviceType-" + credInfo.DeviceType);
                success = CommonWebApiBL.ValidateUser(credInfo, ref objOut, ref token, ref userType, ref userID);
                if (success)
                {
                    traceLog.AppendLine("Success ValidateUser() method with token" + token);
                    objToken.jsonData = new Token() { TokenId = token, UserId = userID, UserType = userType };
                    objToken.IsResultTrue = true;
                    return Ok(objToken);
                }
                else
                {
                    traceLog.AppendLine("Not found user and password of app users");
                    objToken.ErrorMessage = Message.WrongUserPassword;
                    objToken.IsResultTrue = false;
                    return Ok(objToken);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                objToken.IsResultTrue = false;
                objToken.ErrorMessage = Message.ServerError;
                return Ok(objToken);
            }
            finally
            {
                traceLog.AppendLine("Index  Start end() : --- with Repsponse Status" + objToken.IsResultTrue + " ErrorMessage" + objToken.ErrorMessage + "-" + DateTime.Now.ToLongDateString() + success.ToString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objOut = null;
            }
        }

        /// <summary>
        /// Action to Change Password
        /// </summary>
        /// <returns>ServiceResponse<Token></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        /// 
        [HttpPost]
        [TokenValidationFilter]
        public IHttpActionResult ChangePassword(EditPasswordVM model)
        {
            StringBuilder traceLog = null;
            //  string token = string.Empty;
            ServiceResponse<bool> changpasswordstatus = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChangePassword Request Data:-NewPassword-" + model.NewPassword + ",OldPassword-" + model.OldPassword);
                bool flag = CommonWebApiBL.ChangePassword(model);
                changpasswordstatus = new ServiceResponse<bool>();
                changpasswordstatus.jsonData = flag;
                changpasswordstatus.IsResultTrue = flag;
                if (!flag)
                {
                    changpasswordstatus.ErrorMessage = Message.IncorrectPassword;
                }
                return Ok(changpasswordstatus);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                changpasswordstatus.ErrorMessage = Message.ServerError;
                changpasswordstatus.IsResultTrue = false;
                return Ok(changpasswordstatus);
            }
            finally
            {
                traceLog.AppendLine("End: ChangePassword Response Result Status-" + changpasswordstatus.jsonData + ",Modified DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}