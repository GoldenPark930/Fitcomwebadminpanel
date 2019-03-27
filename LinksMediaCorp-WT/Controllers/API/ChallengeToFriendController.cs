namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Http;

    [TokenValidationFilter]
    public class ChallengeToFriendController : ApiController
    {
        /// <summary>
        /// Action to get all friend list
        /// </summary>
        /// <returns>ServiceResponse<Total<List<FriendVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/30/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetAllFriendList(NumberOfRecord<string> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FriendVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllFriendList() with Request Data:-Param-" + model.Param + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<FriendVM>>>();
                objResponse.jsonData = ChallengeToFriendBL.GetUserAllFriendList(model);
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
                traceLog.AppendLine("End:GetAllFriendList() resonse Result Ststus-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get filetr friend list
        /// </summary>
        /// <returns>ServiceResponse<Total<List<FriendVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/30/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFilterFriendList(NumberOfRecord<FriendFilterParam> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FriendVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterFriendList" + model.Param + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<FriendVM>>>();
                objResponse.jsonData = ChallengeToFriendBL.GetFilterFriendList(model);
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
                traceLog.AppendLine("End:GetFilterFriendList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get pending challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/07/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetPendingChallengeList(long? UserNotificationID = 0)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PendingChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPendingChallengeList");
                objResponse = new ServiceResponse<List<PendingChallengeVM>>();
                long userNotifyID = 0;
                if (UserNotificationID == null)
                    userNotifyID = 0;
                else
                    userNotifyID = UserNotificationID ?? 0;
                objResponse.jsonData = ChallengeToFriendBL.GetPendingChallengeList(userNotifyID);
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
                traceLog.AppendLine("End:GetPendingChallengeList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserNotificationID"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFreindChallengeList(long? UserNotificationID = 0)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PendingChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreindChallengeList");
                objResponse = new ServiceResponse<List<PendingChallengeVM>>();
                long userNotifyID = 0;
                if (UserNotificationID == null)
                    userNotifyID = 0;
                else
                    userNotifyID = UserNotificationID ?? 0;
                objResponse.jsonData = ChallengeToFriendBL.GetFreindChallengeList(userNotifyID);
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
                traceLog.AppendLine("End:GetFreindChallengeList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/12/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetChallengedSenderDetails(NotificationSenderVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<PendingChallengeVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetChallengedSenderDetails");
                objResponse = new ServiceResponse<PendingChallengeVM>();
                objResponse.jsonData = ChallengeToFriendBL.GetNotificationPendingChallengeDescription(model);
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
                traceLog.AppendLine("End:GetChallengedSenderDetails() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post challenge to friends
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/07/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostChallengeToFriend(ChallengeFriendVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CompletedChallengesWithPendingVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostChallengeToFriend() Request Data:-ChallengeId-" + model.ChallengeId + ",IsSelecAllTMembers-" + model.IsSelecAllTMembers);
                objResponse = new ServiceResponse<CompletedChallengesWithPendingVM>();
                objResponse.jsonData = ChallengeToFriendBL.PostChallengeToFriend(model);
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
                traceLog.AppendLine("End:PostChallengeToFriend() Response Result status-" + objResponse.IsResultTrue + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Delete  Pending Challenge Result 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 11/12/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeletePendingChallengeResult(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PendingChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeletePendingChallengeResult() Request Result ID-" + model.ResultId);
                objResponse = new ServiceResponse<List<PendingChallengeVM>>();
                objResponse.jsonData = ChallengeToFriendBL.DeletePendingChallengeResult(model.ResultId);
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
                traceLog.AppendLine("End:DeletePendingChallengeResult() Resopnse Result Ststus-" + objResponse.IsResultTrue + ",Deleted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
