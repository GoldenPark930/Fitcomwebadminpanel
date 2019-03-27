namespace LinksMediaCorp.Controllers.API
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// User Registration Controller class is used for register the user in IOS and Androids apps
    /// </summary>
    public class UserRegistrationController : ApiController
    {
        /// <summary>
        /// Action for User Registration
        /// </summary>
        /// <returns>ServiceResponse<Token></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        [HttpPost]
        public async Task<IHttpActionResult> UserRegistration(UserInfo model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserRegistraionVM> objToken = null;
            UserRegistraionVM token = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UserRegistration Request Data:-FirstLastName-" + model.FirstName + model.LastName + ",Gender-" + model.Gender + ",EmailId-" + model.EmailId + ",DeviceID-" + model.DeviceID + ",DeviceType-" + model.DeviceType + ",ZipCode-" + model.ZipCode);
                objToken = new ServiceResponse<UserRegistraionVM>();
                bool isEmailExist = isEmailExist = await UseresBL.IsEmailExist(model.EmailId);
                if (isEmailExist)
                {
                    objToken.IsResultTrue = false;
                    objToken.ErrorMessage = Message.EmailAlreadyExist;
                }
                else
                {
                    Token usertoken = await UseresBL.Register(model);
                    token = new UserRegistraionVM() { TokenId = usertoken.TokenId, UserId = usertoken.UserId, UserType = usertoken.UserType };
                    token.UserDetail = ProfileApiBL.GetHomeProfileDetails(usertoken.UserId, usertoken.UserType);
                    objToken.jsonData = token;
                    objToken.IsResultTrue = true;
                }
                return Ok(objToken);
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
                traceLog.AppendLine("End:UserRegistration Rssponse Result Ststus:-" + objToken.IsResultTrue + ",ErrorMessage-" + objToken.ErrorMessage + ",Registered DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                token = null;
            }
        }

        /// <summary>
        /// Post UTC Message
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        public IHttpActionResult SaveUTCMessage(UTCMessageVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> UTCMEssageResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChangePassword");
                bool isPosted = CommonWebApiBL.UTCPostMessage(model);
                UTCMEssageResponse = new ServiceResponse<bool>();
                UTCMEssageResponse.jsonData = isPosted;
                UTCMEssageResponse.IsResultTrue = true;
                return Ok(UTCMEssageResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                UTCMEssageResponse.IsResultTrue = false;
                UTCMEssageResponse.ErrorMessage = Message.ServerError;
                return Ok(UTCMEssageResponse);
            }
            finally
            {
                traceLog.AppendLine("End :GetMainSearchBarList --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Post List
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        public IHttpActionResult GetUTCPostMessage()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<UTCMessageVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUTCPostMessage");
                objResponse = new ServiceResponse<List<UTCMessageVM>>();
                objResponse.jsonData = CommonWebApiBL.GetUTCPostMessage();
                objResponse.IsResultTrue = true;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetUTCPostMessage --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objResponse = null;
            }
        }



    }
}
