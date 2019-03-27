
namespace LinksMediaCorp.Controllers.API
{
    using System;
    using System.Text;
    using System.Web.Http;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using System.Collections.Generic;
    using LinksMediaCorp.Filters;
    using LinksMediaCorpUtility;
    using System.Threading;
    [TokenValidationFilter]
    public class ProgramController : ApiController
    {
        /// <summary>
        /// Get TrainerLibrary Menu List for all trainers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetTrainerLibraryMenuList()
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<TrainerLibraryMenuVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibraryMenuList():-Request Data-");
                objResponse = new ServiceResponse<List<TrainerLibraryMenuVM>>();
                objResponse.jsonData = ProgramApiBL.GetTrainerLibraryMenuList();
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
                traceLog.AppendLine("End:GetTrainerLibraryMenuList() with Response Result Status-" + objResponse.IsResultTrue + " FetchDateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get TrainerLibrary category List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerLibrarySubMenuList(TrainerLibraryCategoryRequest model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<ChallengeCategory>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibrarySubMenuList() Request Data-UserId-" + model.UserId + ",UserType-" + model.UserType);
                objResponse = new ServiceResponse<List<ChallengeCategory>>();
                objResponse.jsonData = ProgramApiBL.GetTrainerLibrarySubCategoryList(model.UserId, model.UserType, model.ChallegeTypeId);
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
                traceLog.AppendLine("End:GetTrainerLibrarySubMenuList Response Data:- Result status-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get TrainerLibrary Filter ChallengeList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerLibraryFilterChallengeList(TrainerLibraryChallengeFilterParam model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibraryFilterChallengeList with Request data-Type" + model.Type + ",BodyZone" + model.BodyZone + ",Difficulty" + model.Difficulty + ",Equipment" + model.Equipment + ",ExerciseType" + model.ExerciseType);
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ProgramApiBL.GetTrainerLibraryFilterChallengeList(model);
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
                traceLog.AppendLine("End:GetTrainerLibraryFilterChallengeList() Response with Result Status-" + objResponse.IsResultTrue + ",FetchDatatime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objResponse = null;
            }
        }
        /// <summary>
        ///  Get TrainerLibrary All FitnnesTest List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetTrainerLibraryAllFitnessTestList(UserIdAndUserType model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<MainChallengeVM>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibraryAllFitnnesTestList():-Request Data-UserId-" + model.UserId);
                objResponse = new ServiceResponse<List<MainChallengeVM>>();
                objResponse.jsonData = ProgramApiBL.GetTrainerLibraryAllFitnnesTestList(model.UserId, model.UserType);
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
                traceLog.AppendLine("End:GetTrainerLibraryAllFitnnesTestList() with Response Result Status-" + objResponse.IsResultTrue + " FetchDateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objResponse = null;
            }
        }
        /// <summary>
        /// Get Freeform TrainerLibrary Challenges BySubCategory based on trainer ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFreeFormTrainerLibraryChallengesBySubCategory(TrainerLibraryWorkoutListByCategory model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<ChallengeTabVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFreeFormTrainerLibraryChallengesBySubCategory() with request data-WorkoutSubCategoryID-" + model.WorkoutSubCategoryID + ",UserId-" + model.UserId);
                objResponse = new ServiceResponse<ChallengeTabVM>();
                objResponse.jsonData = ProgramApiBL.GetFreeFormTrainerLibraryChallengesBySubCategory(model);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetFreeFormTrainerLibraryChallengesBySubCategory() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetFreeFormTrainerLibraryChallengesBySubCategory()   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Trainer Libraray FitcomTest BodyPart List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTrainerLibrarayFitcomTestBodyPartList(UserIdAndUserType model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<FittnessTestChallenge> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerLibrarayFitcomTestBodyPartList with no request data");
                objResponse = new ServiceResponse<FittnessTestChallenge>();
                objResponse.jsonData = ProgramApiBL.GetTrainerLibraryFitcomTestBodyPartList(model.UserId, model.UserType);
                objResponse.IsResultTrue = true;
                traceLog.AppendLine("End: GetTrainerLibrarayFitcomTestBodyPartList() with Response data with status-" + objResponse.IsResultTrue);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetTrainerLibrarayFitcomTestBodyPartList()   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}