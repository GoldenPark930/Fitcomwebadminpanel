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
    public class ChallengeMessageFeedController : ApiController
    {
        /// <summary>
        /// Action to post Share message text on challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<ViewPostVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostTextMessage(PostVM<TextMessageStream> postModel)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed PostTextMessage() with Request Data:-Stream-" + postModel.Stream + ",IsImageVideo-" + postModel.IsImageVideo + ",TargetId-" + postModel.TargetId);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = ChallengeMessageFeedBL.PostShare(postModel);
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
                traceLog.AppendLine("End: ChallengeMessageFeed PostTextMessage Reponse Data:-PostId-" + objResponse.IsResultTrue + ",Posted Datetime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get post list on challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<Total<List<ChallengeViewPostVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/08/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPostList(NumberOfRecord<ChallengeIdVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<ChallengeViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed GetPostList() Request Data:-ChallengeId-" + model.Param.ChallengeId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<ChallengeViewPostVM>>>();
                objResponse.jsonData = ChallengeMessageFeedBL.GetPostListForChallenge(model.Param.ChallengeId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End: ChallengeMessageFeed GetPostList() Response Data:-Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to post boom on challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<CountVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CountVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed PostBoom Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<CountVM>();
                objResponse.jsonData = new CountVM();
                objResponse.jsonData.Count = ChallengeMessageFeedBL.PostBoom(model);
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
                traceLog.AppendLine("End: ChallengeMessageFeed PostBoom() Response Data:-Result status-" + objResponse.IsResultTrue + ",Posted Datetime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to post comment on challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<CommentVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CommentVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed PostComment():-Message-" + model.Message + ",CommentBy-" + model.CommentBy);
                objResponse = new ServiceResponse<CommentVM>();
                objResponse.jsonData = ChallengeMessageFeedBL.PostComment(model);
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
                traceLog.AppendLine("End: ChallengeMessageFeed PostComment() Response Data:-Result Status-" + objResponse.IsResultTrue + ",CommentId-" + objResponse.jsonData != null ? objResponse.jsonData.CommentId.ToString() : "" + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get boom list of a challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<Total<List<BoomVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetBoomList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<BoomVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed GetBoomList() Rquest Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<BoomVM>>>();
                objResponse.jsonData = ChallengeMessageFeedBL.GetBoomList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End: ChallengeMessageFeed GetBoomList Response Result Ststus-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get comment list of a challenge completed(Comment Section)
        /// </summary>
        /// <returns>ServiceResponse<Total<List<CommentVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/06/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetCommentList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<CommentVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengeMessageFeed GetCommentList() Request Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<CommentVM>>>();
                objResponse.jsonData = ChallengeMessageFeedBL.GetCommentList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End: ChallengeMessageFeed GetCommentList() Response Result Status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
