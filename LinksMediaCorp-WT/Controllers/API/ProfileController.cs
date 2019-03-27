namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;

    /// <summary>
    /// Controller to manage User Profile Web Api Service
    /// </summary>
    /// <devdoc>
    /// Developer Name - Arvind Kumar
    /// Date - 06/16/2015
    /// </devdoc>
    [TokenValidationFilter]
    public class ProfileController : ApiController
    {
        /// <summary>
        /// Action to get User Profile details
        /// </summary>
        /// <returns>ServiceResponse<UserProfileVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/17/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult ProfileDetails(ProfileDetailVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserProfileVM> objUserDetailResponse = null;
            ServiceResponse<TrainerViewVM> objTrainerDetailResponse = null;
            bool isJonedTeam = false;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ProfileDetails() Request Data:-UserId-" + model.UserId + ",UserType-" + model.UserType);
                if (model.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                {
                    objUserDetailResponse = new ServiceResponse<UserProfileVM>();
                    objUserDetailResponse.jsonData = ProfileBL.GetUserProfileDetails(model.UserId, ref isJonedTeam, model.UserNotificationID);
                    objUserDetailResponse.IsResultTrue = true;
                    traceLog.AppendLine("Response Status Result Ststus-" + objUserDetailResponse.IsResultTrue);
                    return Ok(objUserDetailResponse);
                }
                else if (model.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                {
                    objTrainerDetailResponse = new ServiceResponse<TrainerViewVM>();
                    objTrainerDetailResponse.jsonData = ProfileBL.GetTrainerProfileDetails(model.UserId, ref isJonedTeam, model.UserNotificationID);
                    objTrainerDetailResponse.IsResultTrue = true;
                    traceLog.AppendLine("Response Status Result Ststus-" + objTrainerDetailResponse.IsResultTrue);
                    return Ok(objTrainerDetailResponse);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:ProfileDetails() Updated DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to update user profile
        /// </summary>
        /// <returns>ServiceResponse<UserProfileVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/23/2015
        /// </devdoc>
        [HttpPost]
        public async Task<IHttpActionResult> UpdateProfile(UserProfileVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserProfileVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateProfile() Request Data:-FirstName-" + model.FirstName + ",LastName-" + model.LastName + ",EmailId-" + model.EmailId + ",UserType-" + model.UserType + ",UserId-" + model.UserId + ",PicBase64String-" + model.PicBase64String + ",PicExtension-" + model.PicExtension + ",PicName-" + model.PicName);
                objResponse = new ServiceResponse<UserProfileVM>();
                Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                // Update Profile Pic
                if (!string.IsNullOrWhiteSpace(model.PicBase64String))
                {
                    string root = HttpContext.Current.Server.MapPath("~/images/profilepic");
                    var bytes = Convert.FromBase64String(model.PicBase64String);
                    string picName = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + model.PicName + "." + model.PicExtension;
                    using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        if (imageFile != null)
                        {
                            imageFile.Dispose();
                        }
                    }
                    model.PicName = picName;
                    ProfileBL.UpdateProfilePic(model.UserId, model.UserType, picName);
                }
                bool IsEmailExist = await UseresBL.IsEmailExist(model.EmailId, objCred.UserType, objCred.UserId);
                if (IsEmailExist)
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.EmailAlreadyExist;
                }
                else
                {
                    objResponse.jsonData = await ProfileBL.UpdateUserProfile(model, objCred.Id);
                    if (objResponse.jsonData.ZipcodeNotExist)
                    {
                        objResponse.IsResultTrue = false;
                        objResponse.ErrorMessage = Message.ZipCodeNotExist;
                    }
                    else
                    {
                        objResponse.IsResultTrue = true;
                    }
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
                traceLog.AppendLine("End:UpdateProfile() Response Result Ststus-" + objResponse.IsResultTrue + ",ErrorMessage-" + objResponse.ErrorMessage + "Updated DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get user pics
        /// </summary>
        /// <returns>ServiceResponse<List<PicsVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult GetPics(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<PicsVM>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPics() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<PicsVM>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = ProfileBL.GetPics(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetPics() Response Status-" + objResponce.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get user videos
        /// </summary>
        /// <returns>ServiceResponse<List<VideoInfo>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult GetVideos(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<VideoInfo>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetVideosRequest Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<VideoInfo>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = ProfileBL.GetVideos(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetVideos() Response Status-" + objResponce.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get user videos
        /// </summary>
        /// <returns>ServiceResponse<List<VideoAndPicVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult GetVideosAndPics(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<VideoAndPicVM>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetVideosAndPicsRequest Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<VideoAndPicVM>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = ProfileBL.GetVideosAndPics(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetVideosAndPics() Response Status-" + objResponce.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to update profile pic
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UpdateProfilePic(ProfilePicStreamVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateProfilePic() Request Data:-PicBase64String-" + model.PicBase64String + ",PicName-" + model.PicName + ",PicExtension-" + model.PicExtension);
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
                bool flag = ProfileBL.UpdateProfilePic(model.UserId, model.UserType, picName);
                if (flag)
                {
                    objResponse = new ServiceResponse<MessageVM>() { IsResultTrue = true, jsonData = new MessageVM() { Message = Message.PhotoUploadedSuccess } };
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
                traceLog.AppendLine("End:UpdateProfilePic() Response Result Status-" + objResponse.IsResultTrue + ",ErrorMessage-" + objResponse.ErrorMessage + ",Updated DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post Share message text on user profile
        /// </summary>
        /// <returns>ServiceResponse<ViewPostVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostTextMessage(ProfilePostVM<TextMessageStream> postModel)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostTextMessage() Request Data:-UserId-" + postModel.UserId + ",UserType-" + postModel.UserType + ",Stream-" + postModel.Stream + ",IsImageVideo-" + postModel.IsImageVideo);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = ProfileBL.PostShare(postModel);
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
                traceLog.AppendLine("End:PostTextMessage Response REsult Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get post list on on user profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<ViewPostVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPostList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPostList() Request Data-UserId-" + model.Param.UserId + ",UserType=" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<ViewPostVM>>>();
                objResponse.jsonData = ProfileApiBL.GetPostList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetPostList() Response Result Status-" + objResponse.IsResultTrue + "Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post boom on user profile UI
        /// </summary>
        /// </summary>
        /// <returns>ServiceResponse<CountVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CountVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostBoom() Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<CountVM>();
                objResponse.jsonData = new CountVM();
                objResponse.jsonData.Count = ProfileApiBL.PostBoom(model);
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
                traceLog.AppendLine("End:PostBoom() Response Data:-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to post comment on user profile UI
        /// </summary>
        /// <returns>ServiceResponse<CommentVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<CommentVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PostComment() Request Data:-PostId-" + model.PostId + ",UserName" + model.UserName + ",UserId-" + model.UserId + ",Message-" + model.Message);
                objResponse = new ServiceResponse<CommentVM>();
                objResponse.jsonData = ProfileBL.PostComment(model);
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
                traceLog.AppendLine("End:PostComment() Response Result status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get boom list of a comment on user profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<BoomVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
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
                objResponse.jsonData = ProfileApiBL.GetBoomList(model.Param.PostId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetBoomList() Response Result Status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get comment list of a shared message on user profile UI
        /// </summary>
        /// <returns>ServiceResponse<Total<List<CommentVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
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
                traceLog.AppendLine("End:GetCommentList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to delete post on the basis of MessageStreamId/PostId
        /// </summary>
        /// <returns>ServiceResponse<Total<List<CommentVM>>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        [HttpPost]
        public IHttpActionResult DeletePost(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: DeletePost() request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = ProfileApiBL.DeletePost(model.PostId);
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
                traceLog.AppendLine("End:DeletePost() Response Result Status-" + objResponse.IsResultTrue + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;

            }
        }
        /// <summary>
        /// Action to get User/Trainer completed challenges challenge list
        /// </summary>
        /// <returns>ServiceResponse<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/07/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserChallengeList(UserIdAndUserType model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PendingChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserChallengeList() Request Data:-UserId" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<List<PendingChallengeVM>>();
                objResponse.jsonData = ProfileApiBL.GetUserChallengeList(model.UserId, model.UserType);
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
                traceLog.AppendLine("End:GetUserChallengeList() Request Data:-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get user User LatestActivity post in application
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 10/03/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserLatestActivity(UserIdAndUserType model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserLatestActivity() Request Data:-UserId" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = ProfileBL.GetUserTrainerLatestActivity(model.UserId, model.UserType);
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
                traceLog.AppendLine("End:GetUserLatestActivity() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get UserFollowings List for user or Trainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/03/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserFollowings(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FollowerFollwingUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserFollowings() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<FollowerFollwingUserVM>>>();
                objResponse.jsonData = ProfileApiBL.GetUserFollowingList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserFollowings() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        ///  Get UserFollowers list for user/trainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 09/03/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserFollowers(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FollowerFollwingUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserFollowers() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<FollowerFollwingUserVM>>>();
                objResponse.jsonData = ProfileApiBL.GetUserFollowerList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserFollowers Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Update User Onborading Info for current user
        /// </summary>
        /// <param name="onboradingUserInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserOnboradingInfo(UserOnBoradingInfo onboradingUserInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> userInfo = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:  Updating AddUpdateUserInfo User Information-FitnessLevel-" + onboradingUserInfo.FitnessLevel);
                userInfo = new ServiceResponse<bool>();
                bool IsUpdated = UseresBL.UpdateUserOnboardingInfo(onboradingUserInfo);
                userInfo.jsonData = IsUpdated;
                userInfo.IsResultTrue = true;
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                userInfo.IsResultTrue = false;
                userInfo.ErrorMessage = Message.ServerError;
                return Ok(userInfo);
            }
            finally
            {
                traceLog.AppendLine("End:AddUpdateUserInfo Rssponse Status:-" + userInfo.IsResultTrue + "Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get User Onborading Info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserOnboradingInfo()
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserOnBoradingInfo> userInfo = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:  Updating GetUserOnboradingInfo-");
                userInfo = new ServiceResponse<UserOnBoradingInfo>();
                userInfo.jsonData = UseresBL.GetUserOnboardingInfo();
                userInfo.IsResultTrue = true;
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                userInfo.IsResultTrue = false;
                userInfo.ErrorMessage = Message.ServerError;
                return Ok(userInfo);
            }
            finally
            {
                traceLog.AppendLine("End:AddUpdateUserInfo Rssponse Status:-" + userInfo.IsResultTrue + "Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}
