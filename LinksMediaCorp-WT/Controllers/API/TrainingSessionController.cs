using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using LinksMediaCorp.Filters;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;

namespace LinksMediaCorp.Controllers.API
{
    [TokenValidationFilter]
    public class TrainingSessionController : ApiController
    {
        /// <summary>
        /// Get the User Trainers Session User List
        /// </summary>
        /// <param name="model">UserI and UserType of trainer provided</param>
        /// <returns>List of Team usres</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetTrainerSessionUsersList(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<SessionTeamUserVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerSessionUsersList() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType);
                objResponse = new ServiceResponse<Total<List<SessionTeamUserVM>>>();
                objResponse.jsonData = TrainingSessionApiBL.GetTrainerSessionUserList(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetTrainerSessionUsersList() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Purchase SessionHistoryList To User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetPurchaseSessionHistoryListToUser(NumberOfRecord<UserCredential> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<TotalSession<List<PurchaseTraingSessionVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPurchaseSessionHistoryListToUser() Request Data:-UserCredId-" + model.Param.UserCredId);
                objResponse = new ServiceResponse<TotalSession<List<PurchaseTraingSessionVM>>>();
                objResponse.jsonData = TrainingSessionApiBL.GetPurchaseSessionHistoryListToUser(model.Param.UserCredId, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetPurchaseSessionHistoryListToUser() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get User UseSession List based on user crdentail ID
        /// </summary>
        /// <param name="model">User credtial with start and end index</param>
        /// <returns>List of user seesion details of user</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserUseSessionList(NumberOfRecord<UserCredential> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<TotalSession<List<UsedSessionsVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserUseSessionList() Request Data:-UserCredId-" + model.Param.UserCredId);
                objResponse = new ServiceResponse<TotalSession<List<UsedSessionsVM>>>();
                objResponse.jsonData = TrainingSessionApiBL.GetUsersUseSessionList(model.Param.UserCredId, model.Param.TrainingType, model.StartIndex, model.EndIndex);
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
                traceLog.AppendLine("End:GetUserUseSessionList() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Puchase the Training Session from Trainer By trainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult PurchaseSession(PurchaseTraingSessionVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PurchaseTraingSession() Request Data:-TrainerId-" + model.TrainerId + ",UserCredID-" + model.UserCredId + ",NumberOfSession-" + model.NumberOfSession + ",Amount-" + model.Amount);
                objResponse = new ServiceResponse<bool>();
                if (model.Amount <= 0)
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.InvalidTrainingSessionAmount;
                }
                else if (model.NumberOfSession <= 0)
                {
                    objResponse.IsResultTrue = false;
                    objResponse.ErrorMessage = Message.InvalidNumberOfTrainingSession;
                }
                else
                {
                    objResponse.jsonData = TrainingSessionApiBL.PurchaseSessionToUser(model);
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
                traceLog.AppendLine("End:PurchaseTraingSession() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get User SessionDetails WithTrainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult SubmitUseSession(UsedSessionsVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserSessionDetailVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SubmitUseSessionWithTrainer() Request Data:-UserId-" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<UserSessionDetailVM>();
                objResponse.jsonData = TrainingSessionApiBL.SaveUserTraingSession(model);
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
                traceLog.AppendLine("End:SubmitUseSessionWithTrainer() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Update the trace the used session notes base on Used session Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UpdateUseSessionTrack(UsedSessionsVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<long> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateUseSessionWithTrainer() Request Data:-UseSessionId-" + model.UseSessionId + ",IsTracNotes-" + model.IsTracNotes);
                objResponse = new ServiceResponse<long>();
                objResponse.jsonData = TrainingSessionApiBL.UpdateUseSessionWithTrainer(model);
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
                traceLog.AppendLine("End:UpdateUseSessionWithTrainer() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get the User sesssion details by user credtial Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/15/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserSessionDetails(UserSessionDetailVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<UserSessionDetailVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserSessionDetailsWithTrainer() Request Data:-UserCredId-" + model.UserCredId);
                objResponse = new ServiceResponse<UserSessionDetailVM>();
                objResponse.jsonData = TrainingSessionApiBL.GetUserSessionDetailsWithTrainer(model.UserCredId);
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
                traceLog.AppendLine("End:GetUserSessionDetailsWithTrainer() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get User SessionDetails By User credtial Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserSessionDetailsByUser(UserSessionDetailVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<SessionTeamUserVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserSessionDetailsByUser() Request Data:-UserCredId-" + model.UserCredId);
                objResponse = new ServiceResponse<SessionTeamUserVM>();
                objResponse.jsonData = TrainingSessionApiBL.GetUserSessionDetails(model.UserCredId);
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
                traceLog.AppendLine("End:GetUserSessionDetailsByUser() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Update UserSession Details wit Team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        public IHttpActionResult UpdateUserSessionDetailsWithTrainer(UserSessionDetailVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateUserSessionDetailsWithTrainer() Request Data:-UserId-" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TrainingSessionApiBL.UpdateUserSessionDetailsWithTrainer(model.UserCredId, model.RemaingNumberOfSession);
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
                traceLog.AppendLine("End:UpdateUserSessionDetailsWithTrainer() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
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
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        [HttpPost]
        public IHttpActionResult SubmitUsedSessionNotes(NotesUsedSessionVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SubmitUsedSessionNotes() Request Data:-UserId-" + model.UserId + ",UserType-" + model.UserType + ",UsedSessionNoteId-" + model.UsedSessionNoteId + ",Notes-" + model.Notes);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TrainingSessionApiBL.SubmitUsedSessionNotes(model);
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
                traceLog.AppendLine("End:SubmitUsedSessionNotes() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Update UseSession WithTrainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        [HttpPost]
        public IHttpActionResult GetUserSesionNotes(NotesUsedSessionVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<NotesUsedSessionVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserSesionNotes() Request Data:-NotesUsedSessionVM-" + model.UsedSessionNoteId);
                objResponse = new ServiceResponse<NotesUsedSessionVM>();
                objResponse.jsonData = TrainingSessionApiBL.GetUsedSessionNotes(model);
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
                traceLog.AppendLine("End:GetUserSesionNotes() Response Result Status-" + objResponse.IsResultTrue + ",Posted DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
