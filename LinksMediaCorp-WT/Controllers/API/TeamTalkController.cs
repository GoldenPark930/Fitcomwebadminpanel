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
    public class TeamTalkController : ApiController
    {

        /// <summary>
        /// Action to post Share message text on my team section
        /// </summary>
        /// <returns>ServiceResponse<ViewPostVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/26/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostMyTeamTextMessage(ProfilePostVM<TextMessageStream> postModel)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostMyTeamTextMessage() Request Data:-UserId-" + postModel.UserId + ",UserType-" + postModel.UserType + ",IsImageVideo-" + postModel.IsImageVideo);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = TeamBL.PostShare(postModel);
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
                traceLog.AppendLine("End:PostMyTeamTextMessage() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get post list on my Team tab ui
        /// </summary>
        /// <returns>ServiceResponse<Total<List<ViewPostVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetMyTeamPostList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetMyTeamPostList() Request Data:-UserId-" + model.Param.UserId + ",UserTyp-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<ViewPostVM>>>();
                objResponse.jsonData = TeamApiBL.GetMyTeamPostList(model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetMyTeamPostList() Response Data:-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }


        /// <summary>
        /// Post following text message
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostFollowingTextMessage(ProfilePostVM<TextMessageStream> postModel)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostFollowingTextMessage() Request Data:-UserId-" + postModel.UserId + ",UserType-" + postModel.UserType + ",Stream-" + postModel.Stream + ",IsImageVideo-" + postModel.IsImageVideo);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = TeamBL.PostFollowingShare(postModel);
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
                traceLog.AppendLine("End:PostFollowingTextMessage() Response Result Ststus-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }


        /// <summary>
        /// Action to get post list on following tab UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<ViewPostVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFollowingPostList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFollowingPostList() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<ViewPostVM>>>();
                objResponse.jsonData = TeamApiBL.GetFollowingPostList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetFollowingPostList() Response Result Status:-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
