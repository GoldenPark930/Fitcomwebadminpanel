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
    public class TeamController : ApiController
    {

        /// <summary>
        /// Action to Get All team List
        /// </summary>
        /// <returns>ServiceResponse<List<TeamTrainerVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/23/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetAllTeams()
        {
            StringBuilder traceLog = null;
            ServiceResponse<TeamsDetails> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllTeams()");
                objResponse = new ServiceResponse<TeamsDetails>();
                objResponse.jsonData = TeamApiBL.GetAllTeams();
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
                traceLog.AppendLine("End:GetAllTeams() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        [HttpPost]
        public IHttpActionResult GetFilterTeams(MainSearchBarParam serachTeam)
        {
            StringBuilder traceLog = null;
            ServiceResponse<TeamsDetails> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterTeams()");
                objResponse = new ServiceResponse<TeamsDetails>();
                objResponse.jsonData = TeamApiBL.GetFilterTeams(serachTeam);
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
                traceLog.AppendLine("End:GetFilterTeams() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to Get All Challenge List
        /// </summary>
        /// <returns>ServiceResponse<List<TeamTrainerVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetTeamTrainers(NumberOfRecord model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<TeamTrainerVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetAllTrainers()");
                objResponse = new ServiceResponse<Total<List<TeamTrainerVM>>>();
                objResponse.jsonData = TeamApiBL.GetTeamMobileCoachTrainer(model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetAllTrainers() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to Get Filter Team
        /// </summary>
        /// <returns>ServiceResponse<List<TeamTrainerVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/23/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetFilterTrainers(TrainerFilterParam model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<TeamTrainerVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterTrainers() Request Data:-City-" + model.City + ",Specialization-" + model.Specialization + ",State-" + model.State);
                objResponse = new ServiceResponse<List<TeamTrainerVM>>();
                objResponse.jsonData = TeamApiBL.GetFilterTrainer(model);
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
                traceLog.AppendLine("End:GetFilterTrainers() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to leave team under trainer
        /// </summary>
        /// <returns>ServiceResponse<bool></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult LeaveTeam()
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: LeaveTeam()");
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TeamBL.LeaveTeam();
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
                traceLog.AppendLine("End:LeaveTeam() Response Result Ststus-" + objResponse.IsResultTrue + ",Modified Datetime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to join team under trainer
        /// </summary>
        /// <returns>ServiceResponse<bool></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult JoinTeam(TeamVM objTeamInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: JoinTeam() Request Data:-TrainerId-" + objTeamInfo.TeamId);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TeamBL.JoinTeam(objTeamInfo);
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
                traceLog.AppendLine("End:JoinTeam() Response Data-" + objResponse.IsResultTrue + ",Joined Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to join team under trainer
        /// </summary>
        /// <returns>ServiceResponse<bool></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult TeamNutritions()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<NutritionVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamNutritions() Request Data:");
                objResponse = new ServiceResponse<List<NutritionVM>>();
                objResponse.jsonData = TeamBL.GetTeamNutritionList();
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
                traceLog.AppendLine("End:TeamNutritions() Response Data-" + objResponse.IsResultTrue + ",Joined Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to Follow or Unfollow user
        /// </summary>
        /// <returns>ServiceResponse<bool></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult FollowUnfollow(FollowUserVM modelFollow)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: FollowUnfollow() Request Data-:UserId-" + modelFollow.UserId + ",UserType-" + modelFollow.UserType + ",IsFollow-" + modelFollow.IsFollow);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TeamBL.FollowUnfollow(modelFollow);
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
                traceLog.AppendLine("End:FollowUnfollow() Response Result Status-" + objResponse.IsResultTrue + ",FollowUnfollow Datetime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to Follow or Unfollow Team by user
        /// </summary>
        /// <param name="modelFollow"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult FollowUnfollowTeam(FollowUserVM modelFollow)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: FollowUnfollowTeam() Request Data-:UserId-" + modelFollow.UserId + ",UserType-" + modelFollow.UserType + ",IsFollow-" + modelFollow.IsFollow);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TeamBL.FollowUnfollowTeam(modelFollow);
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
                traceLog.AppendLine("End:FollowUnfollowTeam() Response Result Status-" + objResponse.IsResultTrue + ",FollowUnfollow Datetime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to get specialization list
        /// </summary>
        /// <returns>ServiceResponse<List<Specialization>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>
        /// 
        [HttpGet]
        public IHttpActionResult GetSpecializationList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<Specialization>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetSpecializationList()");
                objResponse = new ServiceResponse<List<Specialization>>();
                objResponse.jsonData = TeamBL.GetSpecializationList();
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
                traceLog.AppendLine("End:GetSpecializationList() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Team UserMemebers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetTeamUserMemebers(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FollowerFollwingUserVM>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerTeam() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<FollowerFollwingUserVM>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = TeamApiBL.GetTeamMemberList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetTrainerTeam() Response Result Status-" + objResponce.IsResultTrue + ",Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Team MTActive User Memebers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTeamMTActiveUserMemebers(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<FollowerFollwingUserVM>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerTeam() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<FollowerFollwingUserVM>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = TeamApiBL.GetTeamUserMemberList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetTrainerTeam() Response Result Status-" + objResponce.IsResultTrue + ",Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get MyTeam RequestData
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetMyTeamRequestData(NumberOfRecord model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<MyTeamHomeVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetMyTeamRequestData()");
                objResponse = new ServiceResponse<MyTeamHomeVM>();
                objResponse.jsonData = TeamBL.GeMyTeamRequestData(model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetMyTeamRequestData() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Posted Result detailed based on post ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPostedDetails(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ViewPostVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPostedDetails() Request Data:-PostId-" + model.PostId);
                objResponse = new ServiceResponse<ViewPostVM>();
                objResponse.jsonData = TeamApiBL.GetPostDetailsByPostId(model.PostId, model.NotificationID);
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
                traceLog.AppendLine("End:GetPostedDetails() Response Data-" + objResponse.IsResultTrue + ", Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Completed ResultDetails based on post Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/4/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetCompletedResultDetails(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<RecentResultVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCompletedResultDetails() Request Data:-TrainerId-" + model.PostId);
                objResponse = new ServiceResponse<RecentResultVM>();
                objResponse.jsonData = TeamApiBL.GetPostedResultDetails(model.PostId, model.NotificationID);
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
                traceLog.AppendLine("End:GetCompletedResultDetails() Response Data-" + objResponse.IsResultTrue + ", Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }



    }
}
