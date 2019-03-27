using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using LinksMediaCorp.Filters;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
namespace LinksMediaCorp.Controllers.API
{
    [TokenValidationFilter]
    public class TrainerController : ApiController
    {
        /// <summary>
        /// Select User Personal Trainer
        /// </summary>
        /// <param name="objUserPersonalTrainer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SelectUserPersonalTrainer(UserPersonalTrainerVM objUserPersonalTrainer)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SelectUserPersonalTrainer() Request Data:-TrainerCredID-" + objUserPersonalTrainer.TrainerCredID);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TrainerApiBL.SelectUserPersonalTrainer(objUserPersonalTrainer);
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
                traceLog.AppendLine("End:SelectUserPersonalTrainer() Response Data-" + objResponse.IsResultTrue + ", Datetime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Trainer All Users on mamange tab
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerAllUsers(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<TeamUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerAllUsers() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<TeamUserVM>>>();
                objResponse.jsonData = TrainerApiBL.GetTrainerTeamUsers(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex, false);
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
                traceLog.AppendLine("End:GetTrainerAllUsers() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        ///  Get Trainer MTActiveUsers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerMTActiveUsers(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<TeamUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerMTActiveUsers() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<TeamUserVM>>>();
                objResponse.jsonData = TrainerApiBL.GetTrainerTeamUsers(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex, true);
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
                traceLog.AppendLine("End:GetTrainerMTActiveUsers() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get User Assignments
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUserAssignments(NumberOfRecord model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<UserAssignmentByTrainerVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserAssignments() Request Data:-StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<UserAssignmentByTrainerVM>>>();
                objResponse.jsonData = UseresBL.GetUserAssignmentsList(model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserAssignments() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        ///  Get User Trainer Assignment List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUserTrainerAssignmentList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<UserAssignmentByTrainerVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserTrainerAssignmentList() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<UserAssignmentByTrainerVM>>>();
                objResponse.jsonData = TrainersBL.GetUserTrainerAssignmentsList(model.Param.UserId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserTrainerAssignmentList() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get My Trainer Assignment List based on pagination start and end index
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMyTrainerAssignmentList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<UserAssignmentByTrainerVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetMyTrainerAssignmentList() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<UserAssignmentByTrainerVM>>>();
                objResponse.jsonData = TrainersBL.GetMyTrainerAssignmentsList(model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetMyTrainerAssignmentList() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Trainer User Activity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerUserActivity(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<RecentResultVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserTrainerAssignmentList() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<RecentResultVM>>>();
                objResponse.jsonData = UseresBL.GetUserActivity(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserTrainerAssignmentList() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}