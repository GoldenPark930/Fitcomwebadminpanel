namespace LinksMediaCorp.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using Newtonsoft.Json;
    using System.Globalization;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Threading;
    /// <summary>
    /// Challenge Controller class is used for handle user request for Challenge
    /// </summary>
    [TokenValidationFilter]
    public class ChallengeController : ApiController
    {
        /// <summary>
        /// Action to Get All Challenge List
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/27/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetAllChallengeList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllChallengeList():-Request Data-");
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetAllChallengeList();
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
                traceLog.AppendLine("End:GetAllChallengeList() with Response Result Status-" + objResponse.IsResultTrue + " FetchDateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objResponse = null;
            }
        }
        /// <summary>
        /// Get All Challenge Type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllChallengeType()
        {
            StringBuilder traceLog = null;
            ServiceResponse<ChallegeTypeDetail> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllChallengeType():-Request Data-");
                objResponse = new ServiceResponse<ChallegeTypeDetail>();
                objResponse.jsonData = ChallengeApiBL.GetAllChallengeType();
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
                traceLog.AppendLine("End:GetAllChallengeType() with Response Result Status-" + objResponse.IsResultTrue + " FetchDateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }


        /// <summary>
        /// Action to get filtered challenge list
        /// </summary>
        /// <returns>Total<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/14/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFilterChallengeList(ChallengeFilterParam model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterChallengeList with Request data-Type" + model.Type + ",BodyZone" + model.BodyZone + ",Difficulty" + model.Difficulty + ",Equipment" + model.Equipment + ",ExerciseType" + model.ExerciseType);
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetFittenestTestFilterChallengeList(model);
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
                traceLog.AppendLine("End:GetFilterChallengeList() Response with Result Status-" + objResponse.IsResultTrue + ",FetchDatatime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objResponse = null;
            }
        }
        /// <summary>
        /// Get Filter Free FitcomChallenge List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFilterFreeFitcomChallengeList(ChallengeFilterParam model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterFreeFitcomChallengeList with Request data-Type" + model.Type + ",BodyZone"
                    + model.BodyZone + ",Difficulty" + model.Difficulty + ",Equipment" + model.Equipment + ",ExerciseType" + model.ExerciseType);
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetFilterFreeFitcomChallengeList(model);
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
                traceLog.AppendLine("End:GetFilterFreeFitcomChallengeList() Response with Result Status-" + objResponse.IsResultTrue +
                    ",FetchDatatime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to Get Challenge Details on ChallenegId
        /// </summary>
        /// <returns>ServiceResponse<ChallengeDetailVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/27/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetChallengeDetails(MainChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ChallengeDetailVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetChallengeDetails() with Request Data:-ChallengeID-" + model.ChallengeId);
                objResponse = new ServiceResponse<ChallengeDetailVM>();
                objResponse.jsonData = ChallengeApiBL.GetChallengeDetail(model.ChallengeId);
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
                traceLog.AppendLine("End:GetChallengeDetails() with Resonse Data:-Challange Result Status-" + objResponse.IsResultTrue
                    + "FetchDataTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to Get Team Challenge List
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/27/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetTeamChallengeList()
        {
            StringBuilder traceLog = null;
            bool isTeamJoined = false;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTeamChallengeList() Request Data-");
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetTeamChallengeList(ref isTeamJoined);
                objResponse.IsJoinedTeam = isTeamJoined;
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
                traceLog.AppendLine("End:GetTeamChallengeList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-"
                    + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to Submit Result with Message and Result only
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult SubmitResult(SubmitResultVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageStreamIdVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SubmitResult() Request Data:-UserID-" + model.UserId + ",FractionValue" + model.FractionValue + ",IsGlobal"
                    + model.IsGlobal + ",IsImageVideo" + model.IsImageVideo + ",IsTextImageVideo" + model.IsTextImageVideo + ",MessageStreamId" + model.MessageStreamId + ",MessageText" + model.MessageText + ",ResultValue" + model.ResultValue);
                objResponse = new ServiceResponse<MessageStreamIdVM>();
                Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                int userChallengeId = ChallengeApiBL.AcceptChallenge(model.ChallengeId, cred);
                MessageStreamIdVM objMessageStreamVM = new MessageStreamIdVM();
                objMessageStreamVM = ChallengeApiBL.SubmitChallengeResult(model, userChallengeId, cred);
                if (model.IsProgram)
                {
                    objMessageStreamVM.IsAllWeekWorkoutcompleted = ChallengeApiBL.UpdateProgramWorkout(model.ProgramWeekWorkoutId, cred, userChallengeId);
                }
                objResponse.jsonData = objMessageStreamVM;
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
                traceLog.AppendLine("End:SubmitResult() Response Data:-" + "IsResultTrue" + objResponse.IsResultTrue + "SubmittedDatatime"
                    + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Decline Challenge user/trainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeclineChallengeOrProgram(ChallengeIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeclineChallengeOrProgram() Request Data:- ChallengeId-" + model.ChallengeId);
                objResponse = new ServiceResponse<bool>();
                Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                bool isSuccess = ChallengeApiBL.DeclineChallengeOrProgram(model.ChallengeId, cred, model.IsProgram);
                objResponse.jsonData = isSuccess;
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
                traceLog.AppendLine("End:DeclineChallengeOrProgram() Response Data:SubmittedDatatime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Submit Result Message
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SubmitResultMessage(ShareSubmitResultMessageVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SubmitResultMessage() Request Data:-UserChallengesId-" + model.UserChallengesId);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = ChallengeApiBL.ShareMessageToTeam(model.UserChallengesId, model.CompletedTextMessage);
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
                traceLog.AppendLine("End:SubmitResultMessage() Response Data:-" + "IsResultTrue" + objResponse.IsResultTrue + "SubmittedDatatime"
                    + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }


        /// <summary>
        /// Action to upload Result Photo as well as upload photo with shared message on challenge complete section
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UploadResultPhoto(MessageStreamPicVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadResultPhoto with Request Data:-MessageStreamId-" + model.MessageStreamId + ",PicBase64String-"
                    + model.PicBase64String + ",PicExtension-" + model.PicExtension + ",PicName-" + model.PicName + ",ImageMode-"
                    + model.ImageMode + ",Width-" + model.Width + ",Height-" + model.Height);
                objResponse = new ServiceResponse<MessageVM>();
                string root = HttpContext.Current.Server.MapPath("~/images/result");
                var bytes = Convert.FromBase64String(model.PicBase64String);
                string picName = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + model.PicName + "." + model.PicExtension;
                using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                model.PicName = picName;
                string filePath = root + ConstantHelper.constDoubleBackSlash + picName;
                double sourceWidth = 0;
                double sourceHeight = 0;
                if (File.Exists(filePath))
                {
                    using (Bitmap objBitmap = new Bitmap(filePath))
                    {
                        sourceWidth = Convert.ToDouble(objBitmap.Size.Width);
                        sourceHeight = Convert.ToDouble(objBitmap.Size.Height);
                    }
                }
                if (string.IsNullOrEmpty(model.Height) && string.IsNullOrEmpty(model.Width))
                {
                    model.Height = Convert.ToString(sourceHeight);
                    model.Width = Convert.ToString(sourceWidth);
                }
                else
                {
                    decimal imageheight = 0;
                    decimal imagewight = 0;
                    if (!(decimal.TryParse(model.Height, out imageheight)))
                    {
                        model.Height = Convert.ToString(sourceHeight);
                    }
                    if (!(decimal.TryParse(model.Width, out imagewight)))
                    {
                        model.Width = Convert.ToString(sourceWidth);
                    }
                }
                bool flag = ChallengeApiBL.UploadResultPic(model);
                if (flag)
                {
                    objResponse = new ServiceResponse<MessageVM>()
                    {
                        IsResultTrue = true,
                        jsonData = new MessageVM() { Message = Message.PhotoSuccessfullyUploaded }
                    };
                }
                else
                {
                    objResponse = new ServiceResponse<MessageVM>() { IsResultTrue = false, ErrorMessage = Message.ServerError };
                }
                return Ok(objResponse);
            }
            catch (System.Exception e)
            {
                LogManager.LogManagerInstance.WriteErrorLog(e);
                return InternalServerError();
            }
            finally
            {
                traceLog.AppendLine("End:UploadResultPhotoResponse:-IsResultTrue-" + objResponse.IsResultTrue + ",ErrorMessage-"
                    + objResponse.ErrorMessage + ",UploadedDatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Upload  Result Phot by Andriod Apps
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 08/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UploadResultPhotoByAndriod()
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadResultPhotoByAndriod");
                objResponse = new ServiceResponse<MessageVM>();
                var httpRequest = HttpContext.Current.Request;
                bool requestfileExist = false;
                string picName = string.Empty;
                MessageStreamPicVM model = JsonConvert.DeserializeObject<MessageStreamPicVM>(httpRequest.Params["MessageStreamId"]);
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        string root = HttpContext.Current.Server.MapPath("~/images/result");
                        picName = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + model.PicName + "." + model.PicExtension;
                        string filePath = root + ConstantHelper.constDoubleBackSlash + picName;
                        postedFile.SaveAs(filePath);
                        requestfileExist = true;
                        if (File.Exists(filePath))
                        {
                            using (Bitmap objBitmap = new Bitmap(filePath))
                            {
                                double sourceWidth = Convert.ToDouble(objBitmap.Size.Width);
                                double sourceHeight = Convert.ToDouble(objBitmap.Size.Height);
                                model.Height = Convert.ToString(sourceHeight, CultureInfo.CurrentCulture);
                                model.Width = Convert.ToString(sourceWidth, CultureInfo.CurrentCulture);
                            }
                        }
                        traceLog.Append("Upload Result Photo ByAndriod on web server sucessfully");
                        break;
                    }
                }
                model.PicName = picName;
                bool flag = ChallengeApiBL.UploadResultPic(model);
                if (flag && requestfileExist)
                {
                    objResponse = new ServiceResponse<MessageVM>()
                    {
                        IsResultTrue = true,
                        jsonData = new MessageVM() { Message = Message.PhotoSuccessfullyUploaded }
                    };
                }
                else
                {
                    objResponse = new ServiceResponse<MessageVM>() { IsResultTrue = false, ErrorMessage = Message.ServerError };
                }
                return Ok(objResponse);
            }
            catch (System.Exception e)
            {
                LogManager.LogManagerInstance.WriteErrorLog(e);
                return InternalServerError();
            }
            finally
            {
                traceLog.AppendLine("End:UploadResultPhotoByAndriod  Resonse Data:-" + objResponse.IsResultTrue + ",ErrorMessage-" + objResponse.ErrorMessage + ",UploadedDatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;

            }
        }
        /// <summary>
        /// Action to upload Result video as well as upload video with shared message on challenge complete section
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UploadResultVideo(MessageStreamVideoVM model)
        {
            StringBuilder traceLog = null;
            string videoName = string.Empty;
            ServiceResponse<MsgStremVideoIdVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadResultVideo with Request Data:-MessageStreamId-" + model.MessageStreamId);
                if (model != null)
                {
                    string root = HttpContext.Current.Server.MapPath("~/videos/result");
                    if (!model.IsChunkedData)
                    {
                        model.VideoExtension = string.IsNullOrEmpty(model.VideoExtension) ? ConstantHelper.constMP4Exesion : model.VideoExtension;
                        var bytes = model.VideoByteArray;
                        videoName = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + model.VideoName + "." + model.VideoExtension;
                        using (var videoFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + videoName, FileMode.Create))
                        {
                            videoFile.Write(bytes, 0, bytes.Length);
                            videoFile.Flush();
                        }
                        model.VideoName = videoName;
                        if (ChallengeApiBL.UploadResultVideo(model))
                        {
                            ChallengeApiBL.UpdateIsTextImageVideoFlag(videoName, false);
                        }
                        objResponse = new ServiceResponse<MsgStremVideoIdVM>();
                        objResponse.IsResultTrue = true;
                        objResponse.jsonData = new MsgStremVideoIdVM() { MessageStreamVideoId = videoName };
                    }
                    else
                    {
                        string path = root + ConstantHelper.constDoubleBackSlash + model.VideoName;
                        if (File.Exists(path))
                        {
                            var bytes = model.VideoByteArray;
                            using (var videoFile = new FileStream(path, FileMode.Append))
                            {
                                videoFile.Write(bytes, 0, bytes.Length);
                                videoFile.Flush();
                            }
                        }
                        objResponse = new ServiceResponse<MsgStremVideoIdVM>();
                        objResponse.IsResultTrue = true;
                        objResponse.jsonData = new MsgStremVideoIdVM() { MessageStreamVideoId = model.VideoName };
                    }
                    if (model.IsLastChunk)
                    {
                        string thumnailName = model.VideoName.Split('.')[0] + Message.JpgImageExtension;
                        FFMPEG oBJfFMPEG = new FFMPEG();
                        oBJfFMPEG.SaveThumbnail(root + ConstantHelper.constDoubleBackSlash + (string.IsNullOrEmpty(model.VideoName) ? videoName : model.VideoName), root + ConstantHelper.constDoubleBackSlash + thumnailName);
                        ChallengeApiBL.UpdateIsTextImageVideoFlag((string.IsNullOrEmpty(model.VideoName) ? videoName : model.VideoName), true);
                        traceLog.AppendLine("Start: UploadResultVideo withIsLastChunk-VideoName" + model.VideoName);
                    }
                    return Ok(objResponse);
                }
                return BadRequest();
            }
            catch (System.Exception e)
            {
                LogManager.LogManagerInstance.WriteErrorLog(e);
                ChallengeApiBL.UpdateIsTextImageVideoFlag((string.IsNullOrEmpty(model.VideoName) ? videoName : model.VideoName), false);
                return InternalServerError();
            }
            finally
            {
                traceLog.AppendLine("End:UploadResultVideo   --- " + objResponse.IsResultTrue + ",ErrorMessage-" + objResponse.ErrorMessage + ",UploadedDatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to upload video by Android Device
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/22/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UploadResultVideoByAndroid()
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageVM> objResponse = null;
            MessageStreamVideoVM objMessageStreamVideo = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadResultVideoByAndroid");
                string root = HttpContext.Current.Server.MapPath("~/videos/result");
                objMessageStreamVideo = new MessageStreamVideoVM();
                objMessageStreamVideo.VideoName = Guid.NewGuid().ToString() + "." + "mp4";
                var httpRequest = HttpContext.Current.Request;
                objMessageStreamVideo.MessageStreamId = Convert.ToInt32(httpRequest.Params["MessageStreamId"]);
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = root + ConstantHelper.constDoubleBackSlash + objMessageStreamVideo.VideoName;
                        postedFile.SaveAs(filePath);
                        string thumnailName = string.IsNullOrEmpty(objMessageStreamVideo.VideoName) ? string.Empty : objMessageStreamVideo.VideoName.Split('.')[0] + Message.JpgImageExtension;
                        if (!string.IsNullOrEmpty(thumnailName))
                        {
                            FFMPEG oBJfFMPEG = new FFMPEG();
                            oBJfFMPEG.SaveThumbnail(root + ConstantHelper.constDoubleBackSlash + objMessageStreamVideo.VideoName, root + ConstantHelper.constDoubleBackSlash + thumnailName);
                        }
                        ChallengeApiBL.UploadResultVideo(objMessageStreamVideo);
                        // ChallengeApiBL.UpdateIsTextImageVideoFlag(objMessageStreamVideo.VideoName, true);
                        bool isSave = ChallengeApiBL.UpdateIsTextImageVideoFlag(objMessageStreamVideo.VideoName, true);
                        traceLog.AppendLine("Upload Result Video ByAndroid with status-" + isSave);
                    }
                    objResponse = new ServiceResponse<MessageVM>();
                    objResponse.IsResultTrue = true;
                    objResponse.jsonData = new MessageVM() { Message = Message.VideoSuccessfullyUploaded };
                }
                else
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.ServerError;
                }
                return Ok(objResponse);
            }
            catch (System.Exception e)
            {
                LogManager.LogManagerInstance.WriteErrorLog(e);
                objResponse.IsResultTrue = false;
                objResponse.ErrorMessage = Message.ServerError;
                return InternalServerError();
            }
            finally
            {
                traceLog.AppendLine("End:UploadResultVideoByAndroid Response Data:-Result Status-" + objResponse.IsResultTrue + ",ErrorMessage-" + objResponse.ErrorMessage + ",UploadedDatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objMessageStreamVideo = null;
            }
        }

        /// <summary>
        /// Action to get completed challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<CompletedChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetCompletedChallengeList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<CompletedChallengesWithPendingVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCompletedChallengeList()");
                objResponse = new ServiceResponse<CompletedChallengesWithPendingVM>();
                Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                objResponse.jsonData = ChallengeApiBL.GetUserCompletedChallengeList(cred);
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
                traceLog.AppendLine("End:GetCompletedChallengeList Resonse Data:-Result Status" + objResponse.IsResultTrue + ",Fetch DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get completed challenge list by userId and User Type
        /// </summary>
        /// <returns>ServiceResponse<List<CompletedChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 08/07/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetCompletedChallengeListByUserIdAndUserType(UserIdAndUserType model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<CompletedChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCompletedChallengeListByUserIdAndUserType() with Request Data:-UserId-" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<List<CompletedChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetCompletedChallengeList(model);
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
                traceLog.AppendLine("End:GetCompletedChallengeListByUserIdAndUserType() Response Data:-Result Status-" + objResponse.IsResultTrue + ",Fetch DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get personal challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<PersonalChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPersonalChallengeList(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PersonalChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPersonalChallengeList() Request Data:-ChallengeId-" + model.ChallengeId);
                objResponse = new ServiceResponse<List<PersonalChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetPersonalCompletedChallengeList(model.ChallengeId);
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
                traceLog.AppendLine("End:GetPersonalChallengeList() Response Data:- Result Status-" + objResponse.IsResultTrue + ",Fetch DataTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get global challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<GlobalChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetGlobalChallengeList(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<GlobalChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetGlobalChallengeList() Request Data:-ChallengeId-" + model.ChallengeId);
                objResponse = new ServiceResponse<List<GlobalChallengeVM>>();
                Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                objResponse.jsonData = ChallengeApiBL.GetUserGlobalCompletedChallengeList(model.ChallengeId, cred);
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
                traceLog.AppendLine("End:GetGlobalChallengeList Response Data:-Result Status-" + objResponse.IsResultTrue + ",Fetch DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get all filter challenges parameters
        /// </summary>
        /// <returns>ServiceResponse<AllChallengeFilterParameterList></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/14/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetChallengeFilterParameter()
        {
            StringBuilder traceLog = null;
            ServiceResponse<AllChallengeFilterParameterList> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCompletedChallengeList()");
                objResponse = new ServiceResponse<AllChallengeFilterParameterList>();
                objResponse.jsonData = ChallengeApiBL.GetChallengeFilterParameter();
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
                traceLog.AppendLine("End:GetCompletedChallengeList() Response Data:- Result Status-" + objResponse.IsResultTrue + ",Fetch DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        ///  Get GetChallenegeAcceptors users based on challage ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 08/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetChallenegeAcceptors(NumberOfRecord<ChallengeIdVM> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FollowerFollwingUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetChallenegeAcceptors Request Data:-ChallengeId-" + model.Param.ChallengeId + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<FollowerFollwingUserVM>>>();
                objResponse.jsonData = ChallengeApiBL.GetChallenegeAcceptorList(model.Param.ChallengeId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetChallenegeAcceptors  Response Data:- Result Status" + objResponse.IsResultTrue + ",Fetch DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        ///  Delete the Global user challanes list based on resultID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///  /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/13/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeletePersonalResult(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PersonalChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeletePersonalResult() Request Data:-ChallengeId-" + model.ChallengeId + ",ResultId-" + model.ResultId);
                objResponse = new ServiceResponse<List<PersonalChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.DeletePersonalResult(model.ChallengeId, model.ResultId);
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
                traceLog.AppendLine("End:DeletePersonalResult() Respnse Data:-Result Status-" + objResponse.IsResultTrue + "Deleted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        ///  Delete the  Global User challenges result based in Result ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 07/26/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeleteGlobalResult(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<GlobalChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteGlobalResult() Response Data:-ChallengeId" + model.ChallengeId + ",ResultId-" + model.ResultId);
                objResponse = new ServiceResponse<List<GlobalChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.DeleteGlobalResult(model.ChallengeId, model.ResultId);
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
                traceLog.AppendLine("End:DeleteGlobalResult() Response Data:- Result Status-" + objResponse.IsResultTrue + "Delete DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Delete the completed result based userChanges ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 02/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult DeleteCompletedResult(CompletedChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CompletedChallengesWithPendingVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeleteCompletedResult() Request Data:-ChallengeId-" + model.ChallengeId);
                objResponse = new ServiceResponse<CompletedChallengesWithPendingVM>();
                objResponse.jsonData = ChallengeApiBL.DeleteCompletedResult(model.ChallengeId);
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
                traceLog.AppendLine("End:DeleteCompletedResult() Response Data:-" + objResponse.IsResultTrue + "Deleted DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get global challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<GlobalChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFilterGlobalChallengeList(FilterGlobalResultVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<GlobalChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterGlobalChallengeList() Request Data:-ChallengeID-" + model.ChallengeID + ",SearchBy-" + model.SearchBy);
                objResponse = new ServiceResponse<List<GlobalChallengeVM>>();
                objResponse.jsonData = ChallengeApiBL.GetFilterGlobalCompletedChallengeList(model.ChallengeID, model.SearchBy);
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
                traceLog.AppendLine("End:GetFilterGlobalChallengeList() Response Data:-Result Status-" + objResponse.IsResultTrue + "Fetch DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to Get All Challenge List
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/27/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetFittnessCategoryList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<CategoryResponse> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetMostPopularChallengeList with no request data");
                objResponse = new ServiceResponse<CategoryResponse>();
                objResponse.jsonData = ChallengeApiBL.GetFittnessCategoryList();
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetMostPopularChallengeList() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetMostPopularChallengeList   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Create the Free Form challenge
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/23/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult SubmitFreeFormChallenge(FreeFormChallenges model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SubmitFreeFormChallenge() Request Data:-Description-" + model.Description + ",ChallengeName-" + model.ChallengeName);
                objResponse = new ServiceResponse<bool>();
                if (string.IsNullOrEmpty(model.Description))
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.ChallengeDescriptionMsg;
                }
                else if (string.IsNullOrEmpty(model.ChallengeName))
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.ChallengeNameMsg;
                }
                else
                {
                    objResponse.jsonData = FreeFormChallengeBL.CreateFreeFormChallenge(model);
                    objResponse.IsResultTrue = true;
                }
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:SubmitFreeFormChallenge() Response Data:-Result Status-" + objResponse.IsResultTrue + "Fetch DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get FreeForm Challenge By challenge Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 03/23/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetFreeFormChallengeByID(FreeFormChallenges model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<FreeFormChallenges> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeFormChallengeByID with no request data");
                objResponse = new ServiceResponse<FreeFormChallenges>();
                objResponse.jsonData = FreeFormChallengeBL.GetFreeFormChallengeById(model.ChallengeId);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeFormChallengeByID() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeFormChallengeByID   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Update FreeForm Challenge By challenge Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/23/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult UpdateFreeFormChallengeByID(FreeFormChallenges model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<FreeFormChallenges> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateFreeFormChallengeByID with no request data");
                objResponse = new ServiceResponse<FreeFormChallenges>();
                objResponse.jsonData = FreeFormChallengeBL.UpdateFreeFormChallengeByID(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: UpdateFreeFormChallengeByID() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeFormChallengeByID   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get UnAssigned FreeForm Challenges
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/23/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetFreeWorkoutChallenges()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<FreeFormChallenges>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeWorkoutChallenges with no request data");
                objResponse = new ServiceResponse<List<FreeFormChallenges>>();
                objResponse.jsonData = FreeFormChallengeBL.GetUnAssignedFreeFormChallenges();
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeWorkoutChallenges() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeWorkoutChallenges   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Free Workout FreeForm Challenges by Category Id,Sub category Id and is It in Premium types or not
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 07/08/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFreeWorkoutChallengesBySubCategory(WorkoutChallengeRequest model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<FreeFormChallenges>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeWorkoutChallengesBySubCategory with request data" + model.WorkoutSubCategoryID);
                objResponse = new ServiceResponse<List<FreeFormChallenges>>();
                objResponse.jsonData = FreeFormChallengeBL.GetWorkoutChallengesByCategoryId(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeWorkoutChallengesBySubCategory() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeWorkoutChallengesBySubCategory   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        [HttpPost]
        public IHttpActionResult GetFreeUnassignedWorkoutChallengesBySubCategory(WorkoutChallengeRequest model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<FreeFormChallenges>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeWorkoutChallengesBySubCategory with request data" + model.WorkoutSubCategoryID);
                objResponse = new ServiceResponse<List<FreeFormChallenges>>();
                objResponse.jsonData = FreeFormChallengeBL.GetFreeformUnassignedWorkoutChallengesByCategoryId(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeWorkoutChallengesBySubCategory() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeWorkoutChallengesBySubCategory   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        [HttpPost]
        public IHttpActionResult GetFreeUnassignedProgramChallengesBySubCategory(ProgramChallengeRequest model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ProgramVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeUnassignedProgramChallengesBySubCategory with request data");
                objResponse = new ServiceResponse<List<ProgramVM>>();
                objResponse.jsonData = FreeFormChallengeBL.GetFreeformUnassignedProgramChallengesByCategoryId(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeUnassignedProgramChallengesBySubCategory() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeUnassignedProgramChallengesBySubCategory   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get Trainer assined active Free FormChallenges
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/12/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetTrainerFreeFormChallenges()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<FreeFormChallenges>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerFreeFormChallenges with no request data");
                objResponse = new ServiceResponse<List<FreeFormChallenges>>();
                objResponse.jsonData = FreeFormChallengeBL.GetFreeFormChallengesByTrainer();
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetTrainerFreeFormChallenges() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetTrainerFreeFormChallenges   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get TrainerLibrary ChallengeList
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 03/12/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetTeamTrainerLibraryChallengeList()
        {
            StringBuilder traceLog = null;
            bool isTeamJoined = false;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibraryChallengeList() Request Data-");
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = TrainerApiBL.GetTrainerLibraryChallengeListWithFreeForm(ref isTeamJoined);
                objResponse.IsJoinedTeam = isTeamJoined;
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
                traceLog.AppendLine("End:GetTrainerLibraryChallengeList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get FreePremium ChallengeList
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/12/2016
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetFreePremiumChallengeList()
        {
            StringBuilder traceLog = null;
            bool isTeamJoined = false;
            ServiceResponse<PremimumChallengeVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreePremiumChallengeList() Request Data-");
                objResponse = new ServiceResponse<PremimumChallengeVM>();
                objResponse.jsonData = TrainerApiBL.GetFreePremiumChallengeList(ref isTeamJoined);
                objResponse.IsJoinedTeam = isTeamJoined;
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
                traceLog.AppendLine("End:GetFreePremiumChallengeList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get filter premium list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFreePremiumWorkoutsList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<CategoryResponse> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreePremiumChallengeList() Request Data-");
                objResponse = new ServiceResponse<CategoryResponse>();
                objResponse.jsonData = ChallengesCommonBL.GetapiPreminumChallengeCategoryList(ConstantHelper.constWorkoutChallengeSubType, null);
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
                traceLog.AppendLine("End:GetFreePremiumChallengeList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get filter premium list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFreePremiumProgramCategortyList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<CategoryResponse> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreePremiumProgramCategortyList() Request Data-");
                objResponse = new ServiceResponse<CategoryResponse>();
                objResponse.jsonData = ChallengesCommonBL.GetapiPreminumChallengeCategoryList(ConstantHelper.constProgramChallengeSubType, null);
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
                traceLog.AppendLine("End:GetFreePremiumProgramCategortyList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get the Challenge List of trainer 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/12/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetTrainerLibraryChallengeList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ChallengeTabVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibraryChallengeList() Request Data-");
                objResponse = new ServiceResponse<ChallengeTabVM>();
                objResponse.jsonData = TrainerApiBL.GetTrainerLibraryChallengeList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetTrainerLibraryChallengeList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Premium Program By SubCategory filter data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPremiumProgramBySubCategory(ProgramChallengeRequest model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ProgramVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeWorkoutChallengesBySubCategory with request data" + model.ProgramCategoryID);
                objResponse = new ServiceResponse<List<ProgramVM>>();
                objResponse.jsonData = FreeFormChallengeBL.GetPrimumProgramByCategoryId(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeWorkoutChallengesBySubCategory() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeWorkoutChallengesBySubCategory   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Program Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProgramDetails(ProgramVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ProgramDetailVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetProgramDetails() with Request Data:-ChallengeID-" + model.ProgramId);
                objResponse = new ServiceResponse<ProgramDetailVM>();
                objResponse.jsonData = ChallengeApiBL.GetProgramDetail(model.ProgramId);
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
                traceLog.AppendLine("End:GetProgramDetails() with Resonse Data:-Challange Result Status-" + objResponse.IsResultTrue + "FetchDataTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Start Program by program Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult StartProgram(ProgramVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ProgramWeekWorkoutVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: StartProgram() with Request Data:-ChallengeID-" + model.ProgramId);
                objResponse = new ServiceResponse<List<ProgramWeekWorkoutVM>>();
                objResponse.jsonData = ChallengeApiBL.StartProgramById(model.ProgramId);
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
                traceLog.AppendLine("End:StartProgram() with Resonse Data:-Challange Result Status-" + objResponse.IsResultTrue + "FetchDataTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Program Workouts By Program Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProgramWorkoutsByProgramId(ProgramVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ProgramWeekWorkoutVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetProgramWorkoutsByProgramId() with Request Data:-ChallengeID-" + model.ProgramId);
                objResponse = new ServiceResponse<List<ProgramWeekWorkoutVM>>();
                objResponse.jsonData = ChallengeApiBL.GetProgramWorkoutsByProgramId(model.ProgramId);
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
                traceLog.AppendLine("End:GetProgramWorkoutsByProgramId() with Resonse Data:-Challange Result Status-" + objResponse.IsResultTrue + "FetchDataTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get User Active Programs for  currently login user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserActivePrograms()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ProgramDetailVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserActivePrograms() with Request Data:");
                objResponse = new ServiceResponse<List<ProgramDetailVM>>();
                objResponse.jsonData = ChallengeApiBL.GetActiveProgramDetail();
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
                traceLog.AppendLine("End:GetUserActivePrograms()." + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Trending Challenges By Trending Category Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrendingChallengesByTrendingCategoryId(TrendingCategory model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<TrendingChallengeListResponse>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrendingChallengesByTrendingCategoryId() with Request Data:-TrendingCategoryId-" + model.TrendingCategoryId);
                objResponse = new ServiceResponse<List<TrendingChallengeListResponse>>();
                objResponse.jsonData = TeamApiBL.GetTeamTrendingList(model.challengeSubTypeId, model.TrendingCategoryId);
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
                traceLog.AppendLine("End:GetTrendingChallengesByTrendingCategoryId() with Resonse Data:-Challange Result Status-FetchDataTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}
