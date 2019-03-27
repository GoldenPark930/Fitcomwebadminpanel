namespace LinksMediaCorp.Controllers.API
{
    using System;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Text;
    using LinksMediaCorpEntity;
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpUtility;
    [TokenValidationFilter]
    public class PostAndCommentController : ApiController
    {
        /// <summary>
        /// Action to post Share message text on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<ViewPostVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostTextMessage(PostVM<TextMessageStream> postModel)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostAndComment PostTextMessage() Request Data:-TargetId-" + postModel.TargetId + ",Stream-" + postModel.Stream + ",IsImageVideo-" + postModel.IsImageVideo);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = PostAndCommentBL.PostShare(postModel);
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
                traceLog.AppendLine("End: PostAndComment PostTextMessage() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get post list on on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<ViewPostVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPostList(NumberOfRecord<TrainerInfo> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:PostAndComment GetPostList() Request Data:-TrainerId-" + model.Param.TrainerId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<ViewPostVM>>>();
                objResponse.jsonData = PostAndCommentBL.GetPostList(model.Param.TrainerId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:PostAndComment GetPostList()-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to post boom on trainer profile UI
        /// </summary>
        /// </summary>
        /// <returns>ServiceResponse<CountVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CountVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostAndComment PostBoom()Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<CountVM>();
                objResponse.jsonData = new CountVM();
                objResponse.jsonData.Count = PostAndCommentBL.PostBoom(model);
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
                traceLog.AppendLine("End:PostAndComment PostBoom()- Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post comment on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<CommentVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/16/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CommentVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:PostAndComment PostComment() Resquest Data:-UserId-" + model.UserId + ",UserName-" + model.UserName + ",UserType-" + model.UserType + ",PostId-" + model.PostId + ",Message-" + model.Message);
                objResponse = new ServiceResponse<CommentVM>();
                objResponse.jsonData = PostAndCommentBL.PostComment(model);
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
                traceLog.AppendLine("End: PostAndComment PostComment() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get boom list of a comment on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<BoomVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetBoomList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<BoomVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetBoomList() Request Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<BoomVM>>>();
                objResponse.jsonData = PostAndCommentBL.GetBoomList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetBoomList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get comment list of a shared message on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<CommentVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/19/2015
        [HttpPost]
        public IHttpActionResult GetCommentList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<CommentVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCommentList() Request Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<CommentVM>>>();
                objResponse.jsonData = PostAndCommentBL.GetCommentList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetCommentList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Delete the Message Post of user, trainer and profile and also news feed
        /// </summary>
        /// <param name="messageStraemId"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/03/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeleteMessagePost(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteMessagePost() Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = PostAndCommentBL.DeleteMessagePost(model.PostId);
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
                traceLog.AppendLine("End:DeleteMessagePost() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Delete the Message Post of user, trainer and profile and also news feeed
        /// </summary>
        /// <param name="messageStraemId"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/03/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeleteComments(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteMessagePost() Request Data:-commentID-" + model.PostId);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = PostAndCommentBL.DeleteMessagePost(model.PostId);
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
                traceLog.AppendLine("End:DeleteMessagePost() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post boom on trainer result feed list
        /// </summary>
        /// </summary>
        /// <returns>ServiceResponse<CountVM></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 02/10/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostResultBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CountVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostAndComment PostResultBoom()Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<CountVM>();
                objResponse.jsonData = new CountVM();
                objResponse.jsonData.Count = PostAndCommentBL.PostResultBoom(model);
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
                traceLog.AppendLine("End:PostAndComment PostBoom()- Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post comment on My team result feed
        /// </summary>
        /// <returns>ServiceResponse<CommentVM></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 02/10/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostResultComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CommentVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:PostAndComment PostResultComment() Resquest Data:-UserId-" + model.UserId + ",UserName-" + model.UserName + ",UserType-" + model.UserType + ",PostId-" + model.PostId + ",Message-" + model.Message);
                objResponse = new ServiceResponse<CommentVM>();
                objResponse.jsonData = PostAndCommentBL.PostResultCommentData(model);
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
                traceLog.AppendLine("End: PostAndComment PostResultComment() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get boom list of a comment on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<BoomVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetResultBoomList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<BoomVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetBoomList() Request Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<BoomVM>>>();
                objResponse.jsonData = PostAndCommentBL.GetResultBoomList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetBoomList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get comment list of a shared message on trainer profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<CommentVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/19/2015
        [HttpPost]
        public IHttpActionResult GetResultCommentList(NumberOfRecord<ViewPostVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<CommentVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCommentList() Request Data:-PostId-" + model.Param.PostId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<CommentVM>>>();
                objResponse.jsonData = PostAndCommentBL.GetResultCommentList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetCommentList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched Result Status-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
