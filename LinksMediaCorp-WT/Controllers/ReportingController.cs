namespace LinksMediaCorp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using AutoMapper;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using System.Data.Entity.Validation;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Threading.Tasks;
    using OfficeOpenXml;
    using System.Data;
    using OfficeOpenXml.Style;
    public class ReportingController : Controller
    {
        #region Challenges
        /// <summary>
        /// Action to view reporting main page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult Main()
        {
            StringBuilder traceLog = null;
            MainModel model = null;
            List<ViewChallenes> objListChallenge = null;
            ChallengesData challenegdata = new ChallengesData();
            ChallengesData programdata = new ChallengesData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: reporting main page controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    programdata.CurrentPageIndex = challenegdata.CurrentPageIndex;
                }
                else
                {
                    challenegdata.CurrentPageIndex = 0;
                    programdata.CurrentPageIndex = 0;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    programdata.SortField = challenegdata.SortField;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                    programdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    programdata.SortDirection = challenegdata.SortDirection;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                    programdata.SortDirection = ConstantHelper.constASC;
                }
                HttpContext.Session[Message.PreviousUrl] = Message.MainUrlText;
                HttpContext.Session["SelectedTrainerId"] = 0;
                /*BInd view model*/
                model = new MainModel();
                objListChallenge = ChallengesBL.GetTrainerChallenges(-2, Message.UserTypeAdmin);
                List<ViewChallenes> objworkoutListChallenge = ChallengesBL.GetTrainerWorkoutChallenges(-2, Message.UserTypeAdmin);
                if (objListChallenge != null)
                {
                    objListChallenge.AddRange(objworkoutListChallenge);
                }
                else
                {
                    objListChallenge = new List<ViewChallenes>();
                    objListChallenge.AddRange(objworkoutListChallenge);
                }
                challenegdata.SetChallengesViewData(objListChallenge);
                model.ChallengesList = challenegdata;
                List<ViewChallenes> programList = ChallengesBL.GetTrainerPrograms(-2, Message.UserTypeAdmin);
                if ((programdata.CurrentPageIndex * 10) > programList.Count())
                {
                    programdata.CurrentPageIndex = 0;
                }
                programdata.SetChallengesViewData(programList);
                model.ProgramList = programdata;
                model.TrainerList = TrainersBL.GetTrainers();
                model.UserList = UseresBL.GetUsers();
                model.ActivityList = ActivityBL.GetActivities();
                return View(model);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("reporting main page controller end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Action to view channges view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Challenges()
        {
            StringBuilder traceLog = null;
            List<TrainerViewVM> objListTrainers = null;
            ChallengesData challenegdata = new ChallengesData();
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: Challenges controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                    pagenumber = 0;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                ///Get Challenges from the database
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                HttpContext.Session[Message.PreviousUrl] = Message.ChallengeUrl;
                int selectedTrainerId = Convert.ToInt32(HttpContext.Session[ConstantHelper.constSelectedTrainerId]);
                challenegdata.SelectedTrainerId = 0;
                int trainerCredentialId = 0;
                if (selectedTrainerId > 0)
                {
                    trainerCredentialId = ChallengesBL.GetCredentialId(selectedTrainerId);
                }
                else
                {
                    trainerCredentialId = selectedTrainerId;
                }
                objListChallenge = ChallengesBL.GetTrainerChallenges(trainerCredentialId, Message.UserTypeAdmin);
                challenegdata.SelectedTrainerId = selectedTrainerId;
                challenegdata.SetChallengesViewData(objListChallenge);
                return View(challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Challenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Workouts()
        {
            StringBuilder traceLog = null;
            List<TrainerViewVM> objListTrainers = null;
            ChallengesData challenegdata = new ChallengesData();
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: Challenges controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                    pagenumber = 0;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                ///Get Challenges from the database
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                HttpContext.Session[Message.PreviousUrl] = ConstantHelper.constWorkoutChallengeUrl;
                int selectedTrainerId = Convert.ToInt32(HttpContext.Session[ConstantHelper.constSelectedTrainerId]);
                challenegdata.SelectedTrainerId = 0;
                int trainerCredentialId = 0;
                if (selectedTrainerId > 0)
                {
                    trainerCredentialId = ChallengesBL.GetCredentialId(selectedTrainerId);
                }
                else
                {
                    trainerCredentialId = selectedTrainerId;
                }
                objListChallenge = ChallengesBL.GetTrainerWorkoutChallenges(trainerCredentialId, Message.UserTypeAdmin);
                challenegdata.SelectedTrainerId = selectedTrainerId;
                challenegdata.SetChallengesViewData(objListChallenge);
                return View(challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Challenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Create Free form challenges on admin site
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari       
        /// </devdoc>
        [HttpGet]
        public ActionResult FreeFormChallenges()
        {
            StringBuilder traceLog = null;
            List<TrainerViewVM> objListTrainers = null;
            FreeFormChallengeView objfreeformChaleeges = new FreeFormChallengeView();
            List<FreeFormChallengeVM> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constuser)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: FreeFormChallenges() on Reporting controller");
                ///Get Challenges from the database
                objListChallenge = FreeFormChallengeBL.GetFreeFormTrainerChallenges(-1, Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]));
                if (Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == Message.UserTypeAdmin)
                {
                    objfreeformChaleeges.isAdminUser = true;
                }
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                HttpContext.Session[Message.PreviousUrl] = ConstantHelper.constFreeFormChallenges;
                objfreeformChaleeges.FreeFormChallenges = objListChallenge;
                return View(objfreeformChaleeges);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("FreeFormChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to view trainer challenges page in trainer login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult TrainerChallenges()
        {
            StringBuilder traceLog = null;
            int credentialId = 0;
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TrainerChallenges controller");
                credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                objListChallenge = ChallengesBL.GetTrainerAdminChallenges(credentialId, Message.UserTypeTrainer, ConstantHelper.constFittnessCommonSubTypeId);
                return View(objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TrainerChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        [HttpGet]
        public ActionResult TrainerWorkouts()
        {
            StringBuilder traceLog = null;
            int credentialId = 0;
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TrainerChallenges controller");
                credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                objListChallenge = ChallengesBL.GetTrainerAdminChallenges(credentialId, Message.UserTypeTrainer, ConstantHelper.FreeformChallangeId);
                return View(objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TrainerChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        [HttpGet]
        public ActionResult TrainerPrograms()
        {
            StringBuilder traceLog = null;
            int credentialId = 0;
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TrainerPrograms controller");
                credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                objListChallenge = ChallengesBL.GetTrainerPrograms(credentialId, Message.UserTypeTrainer);
                HttpContext.Session[Message.PreviousUrl] = ConstantHelper.constTrainerProgramsUrl;
                return View(objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TrainerPrograms end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view create challenge page 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateChallenge(int? id)
        {
            StringBuilder traceLog = null;
            CreateChallengeVM objChallenge = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<SelectListItem> objSelectList = null;
            // List<ChallengeTypes> objListChallengeSubType = null;
            ChallengeTypes objChallengeType = null;
            UserResult userResult = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create challenge controller");
                objChallenge = new CreateChallengeVM();
                objListChallengeType = ChallengesBL.GetChallengeType();
                List<int> fittnessTypeId = ChallengesBL.GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                if (objListChallengeType != null)
                {
                    objListChallengeType = objListChallengeType.Where(ch => fittnessTypeId.Contains(ch.ChallengeSubTypeId)).ToList();
                }
                ViewBag.ChallengeTypeList = objListChallengeType;
                ViewBag.ChallengeSubTypeList = null;
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list*/
                if (id != null)
                {
                    #region DraftedRequest
                    int Id = (int)id;
                    objSelectList = new List<SelectListItem>();
                    string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                    objChallenge = ChallengesBL.GetChallangeById(Id, ref objSelectList, userType);
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                        objChallenge.ChallengeType = challengeSubTypeId;
                    }
                    /*Get Challenge types from challebge type master table*/
                    ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                    ///*Get challenge viedo and result*/
                    string videoUrl = ChallengesCommonBL.GetVideoByChallengeId(Id);
                    if (videoUrl != null)
                    {
                        objChallenge.HypeVideoLink = videoUrl;
                    }
                    /*Get Variavle Limit*/
                    objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                    objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId, objChallengeType.Unit);
                    /*get challenge type detail*/
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                    CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                    /*Get challenge result*/
                    userResult = ChallengesCommonBL.GetResultByChallengeId(Id);
                    if (userResult != null)
                    {
                        switch (objChallengeType.ResultUnit)
                        {
                            case ConstantHelper.constTime:
                                if (userResult.Result != null)
                                {
                                    CommonReportingUtility.ValidateChallengeResultTimeVariableValue(objChallenge, userResult.Result);
                                }
                                break;
                            case ConstantHelper.constWeight:
                            case ConstantHelper.constDistance:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultWeightorDestance = Convert.ToDecimal(userResult.Result.Replace(",", string.Empty));
                                }
                                break;
                            case ConstantHelper.constReps:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultRepsRound = Convert.ToInt32(userResult.Result.Replace(",", string.Empty));
                                }
                                break;
                            case ConstantHelper.conRounds:
                            case ConstantHelper.constInterval:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultRepsRound = Convert.ToInt32(userResult.Result.Replace(",", string.Empty));
                                }
                                objChallenge.ResultFrection = userResult.Fraction;
                                break;
                        }
                    }
                    else
                    {
                        CommonReportingUtility.ResetChallengeResultVariable(objChallenge);
                    }
                    objChallenge.IsNewAddedExercise1 = ConstantHelper.constSingleZero;
                    #endregion
                }
                else
                {
                    CommonReportingUtility.ResetChallengeTimeVariable(objChallenge);
                    objChallenge.IsNewAddedExercise1 = ConstantHelper.constSingleOne;
                    List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constWorkoutChallengeSubType);
                    objChallenge.AvailableChallengeCategory = challengeCategoryList;
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                objListChallengeType = null;
                objListChallengeType = null;
                objSelectList = null;
                // objListChallengeSubType = null;
                objChallengeType = null;
                userResult = null;
            }
        }
        /// <summary>
        /// Action to create challenge
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateChallenge(CreateChallengeVM objModels)
        {
            StringBuilder traceLog = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ChallengeTypes> objListChallengeSubType = null;
            ExerciseType exerciseType = null;
            List<ExerciseType> objAllExerciseTypes = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                // Validate the ModelState
                ValidateCreateChallenge(objModels);
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: create challenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    ChallengesBL.SubmitChallenge(objModels, credentialId, credentialId, objModels.FormSubmitType);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(ConstantHelper.constTrainerChallenges);
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetChallengeType();

                    ViewBag.ChallengeTypeList = objListChallengeType;
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);

                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    }
                    if (objModels.ChallengeType > 0)
                    {
                        objListChallengeSubType = ChallengesBL.GetChallengeSubType(objModels.ChallengeType);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    objModels.FFExeVideoLink1 = objModels.FFExeVideoUrl1;
                    objModels.FFExeVideoUrl1 = string.IsNullOrEmpty(objModels.FFExeVideoUrl1) ? string.Empty : objModels.FFExeVideoUrl1.Replace(" ", ConstantHelper.constExeciseFileNameExtraSpace);
                    /*get fraction list*/
                    if (objModels.ResultRepsRound != 0 && objModels.ResultRepsRound != null)
                    {
                        ViewBag.ResultRepsRound = objModels.ResultRepsRound;
                    }
                    if (objModels.ResultTime != ConstantHelper.constNotValidate)
                    {
                        ViewBag.ResultTime = objModels.ResultTime;
                    }
                    if (objModels.ResultWeightorDestance != 0 && objModels.ResultWeightorDestance == null)
                    {
                        ViewBag.ResultWeightorDestance = objModels.ResultWeightorDestance;
                    }

                    if (objModels.ResultFrection != null)
                    {
                        ViewBag.SelectedFraction = objModels.ResultFrection;
                    }
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                    /*Get all Exercise Types*/
                    exerciseType = new ExerciseType();
                    objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.AvailableExerciseTypes = objAllExerciseTypes;
                    objModels.SelectedExerciseTypes = new List<ExerciseType>();
                    if (objModels.PostedExerciseTypes != null)
                    {
                        if (objModels.PostedExerciseTypes != null)
                        {
                            if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                            {
                                foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                                {
                                    exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();

                                    objModels.SelectedExerciseTypes.Add(exerciseType);
                                }
                            }
                        }
                    }

                    // Get Available trending category while fail validation
                    objModels.AvailableTrendingCategory = ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }

                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }

                    }
                    ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    TempData["AlertMessage"] = string.Empty;
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListChallengeSubType = null;
                exerciseType = null;
                objAllExerciseTypes = null;
            }
        }


        /// <summary>
        /// Action to view create challenge page 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateTrainerWorkout(int? id)
        {
            StringBuilder traceLog = null;
            CreateChallengeVM objChallenge = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<SelectListItem> objSelectList = null;
            // List<ChallengeTypes> objListChallengeSubType = null;
            ChallengeTypes objChallengeType = null;
            UserResult userResult = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create challenge controller");
                objChallenge = new CreateChallengeVM();
                objChallenge.ChallengeType = ConstantHelper.FreeformChallangeId;
                objListChallengeType = ChallengesBL.GetWorkoutType();
                ViewBag.ChallengeTypeSubList = objListChallengeType;

                ViewBag.ChallengeSubTypeList = null;
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list*/
                if (id != null)
                {
                    #region DraftedRequest
                    int Id = (int)id;
                    objSelectList = new List<SelectListItem>();
                    string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                    objChallenge = ChallengesBL.GetChallangeById(Id, ref objSelectList, userType);
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                        objChallenge.ChallengeType = challengeSubTypeId;
                    }
                    /*Get Challenge types from challebge type master table*/
                    ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                    ///*Get challenge viedo and result*/
                    string videoUrl = ChallengesCommonBL.GetVideoByChallengeId(Id);
                    if (videoUrl != null)
                    {
                        objChallenge.HypeVideoLink = videoUrl;
                    }
                    /*Get Variavle Limit*/
                    objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                    objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId, objChallengeType.Unit);
                    /*get challenge type detail*/
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                    CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                    /*Get challenge result*/
                    userResult = ChallengesCommonBL.GetResultByChallengeId(Id);
                    if (userResult != null)
                    {
                        switch (objChallengeType.ResultUnit)
                        {
                            case ConstantHelper.constTime:
                                if (userResult.Result != null)
                                {
                                    CommonReportingUtility.ValidateChallengeResultTimeVariableValue(objChallenge, userResult.Result);
                                }
                                break;
                            case ConstantHelper.constWeight:
                            case ConstantHelper.constDistance:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultWeightorDestance = Convert.ToDecimal(userResult.Result.Replace(",", string.Empty));
                                }
                                break;
                            case ConstantHelper.constReps:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultRepsRound = Convert.ToInt32(userResult.Result.Replace(",", string.Empty));
                                }
                                break;
                            case ConstantHelper.conRounds:
                            case ConstantHelper.constInterval:
                                if (!string.IsNullOrEmpty(userResult.Result))
                                {
                                    objChallenge.ResultRepsRound = Convert.ToInt32(userResult.Result.Replace(",", string.Empty));
                                }
                                objChallenge.ResultFrection = userResult.Fraction;
                                break;
                        }
                    }
                    else
                    {
                        CommonReportingUtility.ResetChallengeResultVariable(objChallenge);
                    }
                    objChallenge.IsNewAddedExercise1 = ConstantHelper.constSingleZero;
                    #endregion
                }
                else
                {
                    CommonReportingUtility.ResetChallengeTimeVariable(objChallenge);
                    objChallenge.IsNewAddedExercise1 = ConstantHelper.constSingleOne;
                    List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constWorkoutChallengeSubType);
                    objChallenge.AvailableChallengeCategory = challengeCategoryList;
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                objListChallengeType = null;
                objListChallengeType = null;
                objSelectList = null;
                // objListChallengeSubType = null;
                objChallengeType = null;
                userResult = null;
            }
        }
        /// <summary>
        /// Action to create challenge
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateTrainerWorkout(CreateChallengeVM objModels)
        {
            StringBuilder traceLog = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ChallengeTypes> objListChallengeSubType = null;
            ExerciseType exerciseType = null;
            List<ExerciseType> objAllExerciseTypes = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                // Validate the ModelState
                ValidateCreateChallenge(objModels);
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: create challenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    ChallengesBL.SubmitChallenge(objModels, credentialId, credentialId, objModels.FormSubmitType);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(ConstantHelper.constTrainerWorkouts);
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetChallengeType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    }
                    if (objModels.ChallengeType > 0)
                    {
                        objListChallengeSubType = ChallengesBL.GetChallengeSubType(objModels.ChallengeType);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    objModels.FFExeVideoLink1 = objModels.FFExeVideoUrl1;
                    objModels.FFExeVideoUrl1 = string.IsNullOrEmpty(objModels.FFExeVideoUrl1) ? string.Empty : objModels.FFExeVideoUrl1.Replace(" ", ConstantHelper.constExeciseFileNameExtraSpace);
                    /*get fraction list*/
                    if (objModels.ResultRepsRound != 0 && objModels.ResultRepsRound != null)
                    {
                        ViewBag.ResultRepsRound = objModels.ResultRepsRound;
                    }
                    if (objModels.ResultTime != ConstantHelper.constNotValidate)
                    {
                        ViewBag.ResultTime = objModels.ResultTime;
                    }
                    if (objModels.ResultWeightorDestance != 0 && objModels.ResultWeightorDestance == null)
                    {
                        ViewBag.ResultWeightorDestance = objModels.ResultWeightorDestance;
                    }

                    if (objModels.ResultFrection != null)
                    {
                        ViewBag.SelectedFraction = objModels.ResultFrection;
                    }
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                    /*Get all Exercise Types*/
                    exerciseType = new ExerciseType();
                    objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.AvailableExerciseTypes = objAllExerciseTypes;
                    objModels.SelectedExerciseTypes = new List<ExerciseType>();
                    if (objModels.PostedExerciseTypes != null)
                    {
                        if (objModels.PostedExerciseTypes != null)
                        {
                            if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                            {
                                foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                                {
                                    exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();

                                    objModels.SelectedExerciseTypes.Add(exerciseType);
                                }
                            }
                        }
                    }

                    // Get Available trending category while fail validation
                    objModels.AvailableTrendingCategory = ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }

                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    TempData["AlertMessage"] = string.Empty;
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListChallengeSubType = null;
                exerciseType = null;
                objAllExerciseTypes = null;
            }
        }
        /// <summary>
        /// Validate ModelState while Create Challenge by Trainer
        /// </summary>
        /// <param name="objModels"></param>
        [NonAction]
        private void ValidateCreateChallenge(CreateChallengeVM objModels)
        {
            ModelState.Remove("CODStartDate");
            ModelState.Remove("CODEndDate");
            ModelState.Remove("EndUserNameId");
            ModelState.Remove("UserResultId");
            ModelState.Remove("UserVideoLink");
            ModelState.Remove("TCStartDate");
            ModelState.Remove("TCEndDate");
            ModelState.Remove("TrainerId");
            ModelState.Remove("ResultId");
            ModelState.Remove("TrainerVideoLink");
            ModelState.Remove("SponsorName");
            ModelState.Remove("ChallengeId");
            ModelState.Remove("CropImageRowData");

            // ModelState.Remove("ChallengeName");
            if (objModels.ChallengeType == ConstantHelper.FreeformChallangeId)
            {
                ModelState.Remove("VariableValue");
                ModelState.Remove("VariableHours");
                ModelState.Remove("VariableMinute");
                ModelState.Remove("VariableSecond");
                ModelState.Remove("VariableMS");
                ModelState.Remove("UserResultId");
                ModelState.Remove("ExeName1");
                ModelState.Remove("ExeName1");
                ModelState.Remove("ExeName2");
                // ModelState.Remove("ChallengeSubTypeId");
                ModelState.Remove("FFExeName1");
                if (objModels.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType)
                {
                    ModelState.Remove("FFExeName1");
                    ModelState.Remove("FFAExeName1");
                    ModelState.Remove("SelectedTargetZoneCheck");
                    ModelState.Remove("SelectedEquipmentCheck");
                    ModelState.Remove("ChallengeCategoryId");
                    ModelState.Remove("SelectedChallengeCategoryCheck");
                }
                else
                {
                    if (!objModels.IsFFAExeName1)
                    {
                        ModelState.Remove("FFAExeName1");
                    }
                    else
                    {
                        ModelState.Remove("FFExeName1");
                    }

                }
            }
            else
            {
                ModelState.Remove("Description");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("ChallengeCategory");
                ModelState.Remove("ChallengeCategoryId");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("FFAExeName1");
                ModelState.Remove("SelectedChallengeCategoryCheck");
                if (!string.IsNullOrEmpty(objModels.VariableLimit) && !string.IsNullOrEmpty(objModels.VariableValue))
                {
                    // Instantiate the regular expression object.   
                    if (objModels.VariableLimit.Contains(Message.VariableUnitmiles))
                    {
                        string pat = @"^[0-9]+(\.[0-9]{1,2})?$";
                        // Match the regular expression pattern against a text string.
                        if (Regex.IsMatch(Convert.ToString(objModels.VariableValue), pat))
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                }
                else if (objModels.VariableUnit == null)
                {
                    ModelState.Remove("VariableValue");
                }
                // Add Variables minutes in case VariableUnit is in Minutes
                if (objModels.VariableUnit != null && objModels.VariableUnit.Equals(Message.VariableUnitminutes, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(objModels.VariableHours) || !string.IsNullOrEmpty(objModels.VariableMinute) || !string.IsNullOrEmpty(objModels.VariableSecond) || !string.IsNullOrEmpty(objModels.VariableMS))
                    {
                        if (objModels.VariableHours != ConstantHelper.constTimeVariableUnit || objModels.VariableMinute != ConstantHelper.constTimeVariableUnit || objModels.VariableSecond != ConstantHelper.constTimeVariableUnit || objModels.VariableMS != ConstantHelper.constTimeVariableUnit)
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                    objModels.VariableValue = CommonReportingUtility.FormatTimeVariable(objModels.VariableHours, objModels.VariableMinute, objModels.VariableSecond, objModels.VariableMS);
                }
                else
                {
                    ModelState.Remove("VariableHours");
                    ModelState.Remove("VariableMinute");
                    ModelState.Remove("VariableSecond");
                    ModelState.Remove("VariableMS");
                }
                // Result time flage
                bool resulttime = false;
                if (!string.IsNullOrEmpty(objModels.ResultVariableHours) || !string.IsNullOrEmpty(objModels.ResultVariableMinute) || !string.IsNullOrEmpty(objModels.ResultVariableSecond) || !string.IsNullOrEmpty(objModels.ResultVariableMS))
                {
                    if (objModels.ResultVariableHours != ConstantHelper.constTimeVariableUnit || objModels.ResultVariableMinute != ConstantHelper.constTimeVariableUnit || objModels.ResultVariableSecond != ConstantHelper.constTimeVariableUnit || objModels.ResultVariableMS != ConstantHelper.constTimeVariableUnit)
                    {
                        ModelState.Remove("VariableValue");
                        resulttime = true;
                    }
                }
                if (resulttime)
                {
                    objModels.ResultTime = CommonReportingUtility.FormatTimeVariable(objModels.ResultVariableHours, objModels.ResultVariableMinute, objModels.ResultVariableSecond, objModels.ResultVariableMS);
                }
                else
                {
                    objModels.ResultTime = null;
                }
            }
        }
        /// <summary>
        /// Get all Execise types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllExerciseType()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = ChallengesCommonBL.GetExerciseTypes();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }
        /// <summary>
        /// Get All Equipment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllEquipment()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = ChallengesCommonBL.GetEquipments();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }
        /// <summary>
        /// Get All TrainingZone
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllTrainingZone()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = ChallengesCommonBL.GetBodyParts();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }
        /// <summary>
        /// Get All Difficulty Level
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllDifficultyLevel()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = ChallengesCommonBL.GetProgramDifficulties();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }
        /// <summary>
        /// Get All TrainingZone
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllTrainers()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = ChallengesBL.GetTrainers();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;
        }
        /// <summary>
        /// Get All Execise By ChallengeID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllExeciseVideoByChallengeID(int id)
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = FreeFormChallengeBL.GetFreeformChallangeExeciseById(id);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }
        /// <summary>
        /// Get All Week Workouts based on ProgramID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllWeeWorkoutsByProgramID(int id)
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = FreeFormChallengeBL.GetWeekWorkoutByProgramId(id);
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;
        }
        /// <summary>
        /// Get all team
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllTeams()
        {
            JsonResult result = new JsonResult();
            try
            {
                result.Data = TeamBL.GetAllTeamName();
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            return result;

        }

        /// <summary>
        /// Action to view create challenge page in admin login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateAdminChallenge()
        {
            StringBuilder traceLog = null;
            AdminChallenge objAdminChallenge = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            List<BodyPart> objAllTraingZones = null;
            List<ExerciseType> objAllExerciseTypes = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin challenge controller");
                objAdminChallenge = new AdminChallenge();
                objListChallengeType = ChallengesBL.GetChallengeType();
                List<int> fittnessTypeId = ChallengesBL.GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                if (objListChallengeType != null)
                {
                    objListChallengeType = objListChallengeType.Where(ch => fittnessTypeId.Contains(ch.ChallengeSubTypeId)).ToList();
                }
                ViewBag.ChallengeTypeList = objListChallengeType;
                objListTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objListTrainers;
                ViewBag.ChallengeSubTypeList = null;
                /*Get all Exercise Types*/
                objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                ViewBag.ExerciseTypeList = objAllExerciseTypes;
                objAdminChallenge.SetAvailableExerciseTypes(objAllExerciseTypes);
                //Ended By Arvind
                /*Code for radio button list*/
                objAllTraingZones = ChallengesCommonBL.GetBodyParts();
                ViewBag.ListBodyPartList = objAllTraingZones;
                objAdminChallenge.SetAvailableTargetZones(objAllTraingZones);
                /*Code for radio button list for equipment*/
                List<Equipments> equipmentlist = ChallengesCommonBL.GetEquipments();
                ViewBag.ListEquipment = equipmentlist;
                objAdminChallenge.SetAvailableEquipments(equipmentlist);
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                CommonReportingUtility.ResetChallengeVariable(objAdminChallenge);
                List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                objAdminChallenge.SelecetdTeams = objListTeams;
                objAdminChallenge.AvailableTeams = objListTeams;
                List<TrendingCategory> trendingCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);

                objAdminChallenge.SetAvailableTrendingCategory(trendingCategoryList);

                List<TrendingCategory> trendingFittnessCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constFittnessCommonSubTypeId);
                objAdminChallenge.SetAvailableTrendingCategory(trendingFittnessCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList());
                objAdminChallenge.AvailableSecondaryTrendingCategory = trendingFittnessCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constWorkoutChallengeSubType);
                objAdminChallenge.SetAvailableChallengeCategory(challengeCategoryList);

                return View(objAdminChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create admin challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListTrainers = null;
                objAllExerciseTypes = null;
            }
        }
        /// <summary>
        /// Action to create challenge in admin login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateAdminChallenge(AdminChallenge objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                ValidateCreateAdminChallenge(objModels);
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<AdminChallenge, CreateChallengeVM>();
                    CreateChallengeVM objChalange = Mapper.Map<AdminChallenge, CreateChallengeVM>(objModels);
                    traceLog.AppendLine("Start: create admin challenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    int trainerId = 0;
                    if (objModels.TrainerId != null)
                    {
                        int trainerUserId = (int)objModels.TrainerId;
                        trainerId = (int)TrainersBL.GetTrainerId(trainerUserId);
                    }
                    ChallengesBL.SubmitChallenge(objChalange, credentialId, trainerId, string.Empty);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = 1, sort = ConstantHelper.constModifiedDate, sortdir = ConstantHelper.constDESC });
                }
                else
                {
                    ViewBag.ChallengeTypeList = ChallengesBL.GetChallengeType(); ;
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    }
                    else if (objModels.ChallengeType == ConstantHelper.FreeformChallangeId)
                    {
                        objModels.SelectedChallengeTypeId = ConstantHelper.FreeformChallangeId;
                    }
                    if (objModels.ChallengeType > 0)
                    {
                        List<ChallengeTypes> objListChallengeSubType = ChallengesBL.GetChallengeSubType(objModels.ChallengeType);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    // List<ViewTrainers> objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = ChallengesBL.GetTrainers(); ;
                    /*Get all Exercise Types*/
                    ExerciseType exerciseType = new ExerciseType();
                    List<ExerciseType> objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.SetAvailableExerciseTypes(objAllExerciseTypes);
                    objModels.FFExeVideoLink1 = objModels.FFExeVideoUrl1;
                    objModels.FFExeVideoUrl1 = string.IsNullOrEmpty(objModels.FFExeVideoUrl1) ? string.Empty : objModels.FFExeVideoUrl1.Replace(" ", ConstantHelper.constExeciseFileNameExtraSpace);
                    if (objModels.PostedExerciseTypes != null)
                    {
                        List<ExerciseType> selectedExerciseTypes = new List<ExerciseType>();
                        if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                        {
                            foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                            {
                                exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedExerciseTypes.Add(exerciseType);
                            }
                        }
                        objModels.SetSelectedExerciseTypes(selectedExerciseTypes);
                    }
                    BodyPart objBodyPart = new BodyPart();
                    List<BodyPart> objAllTrainingZones = ChallengesCommonBL.GetBodyParts();
                    ViewBag.ListBodyPartList = objAllTrainingZones;
                    objModels.SetAvailableTargetZones(objAllTrainingZones);
                    if (objModels.PostedTargetZones != null)
                    {
                        List<BodyPart> selectedTargetZones = new List<BodyPart>();
                        if (objModels.PostedTargetZones.SelectedTargetZoneIDs != null)
                        {
                            foreach (var item in objModels.PostedTargetZones.SelectedTargetZoneIDs)
                            {
                                objBodyPart = objAllTrainingZones.Where(m => m.PartId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedTargetZones.Add(objBodyPart);
                            }
                        }
                        objModels.SetSelectedTargetZones(selectedTargetZones);
                    }
                    Equipments objEquipments = new Equipments();
                    List<Equipments> objAllEquipments = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = objAllEquipments;
                    objModels.SetAvailableEquipments(objAllEquipments);
                    if (objModels.PostedEquipments != null)
                    {
                        List<Equipments> selectedEquipments = new List<Equipments>();
                        if (objModels.PostedEquipments.SelectedEquipmentIDs != null)
                        {
                            foreach (var item in objModels.PostedEquipments.SelectedEquipmentIDs)
                            {
                                objEquipments = objAllEquipments.Where(m => m.EquipmentId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedEquipments.Add(objEquipments);
                            }
                        }
                        objModels.SetSelectedEquipments(selectedEquipments);
                    }

                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                    ViewBag.ChallengeSubTypeList = null;
                    ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    if(objModels.ChallengeSubTypeId == 0)
                    {
                        objModels.ChallengeSubTypeId = ConstantHelper.constFittnessCommonSubTypeId;
                    }
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }
                    // Get Available trending category while fail validation
                    List<TrendingCategory> allSelectedTrendingCatList = ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    objModels.SetAvailableTrendingCategory(allSelectedTrendingCatList.Where(cat=>cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList());
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            List<TrendingCategory> selecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    selecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                            objModels.SetSelecetdTrendingCategory(selecetdTrendingCategory);
                        }
                    }
                    // Get Available  Fittness trending category while fail validation
                    objModels.AvailableSecondaryTrendingCategory= allSelectedTrendingCatList.Where(cat => cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    // Get Available challenge category while fail validation
                    objModels.SetAvailableChallengeCategory(ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId));
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            List<ChallengeCategory> SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SetSelecetdChallengeCategory(SelecetdChallengeCategory);
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateAdminChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }


        /// <summary>
        /// Action to view create challenge page in admin login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateWorkoutChallenge()
        {
            StringBuilder traceLog = null;
            AdminChallenge objAdminChallenge = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            List<BodyPart> objAllTraingZones = null;
            List<ExerciseType> objAllExerciseTypes = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin challenge controller");
                objAdminChallenge = new AdminChallenge();
                objListChallengeType = ChallengesBL.GetWorkoutType();

                ViewBag.ChallengeTypeSubList = objListChallengeType;
                objAdminChallenge.ChallengeType = ConstantHelper.FreeformChallangeId;
                objListTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objListTrainers;
                /*Get all Exercise Types*/
                objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                ViewBag.ExerciseTypeList = objAllExerciseTypes;
                objAdminChallenge.SetAvailableExerciseTypes(objAllExerciseTypes);
                //Ended By Arvind
                /*Code for radio button list*/
                objAllTraingZones = ChallengesCommonBL.GetBodyParts();
                ViewBag.ListBodyPartList = objAllTraingZones;
                objAdminChallenge.SetAvailableTargetZones(objAllTraingZones);
                /*Code for radio button list for equipment*/
                List<Equipments> equipmentlist = ChallengesCommonBL.GetEquipments();
                ViewBag.ListEquipment = equipmentlist;
                objAdminChallenge.SetAvailableEquipments(equipmentlist);
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                CommonReportingUtility.ResetChallengeVariable(objAdminChallenge);
                List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                objAdminChallenge.SelecetdTeams = objListTeams;
                objAdminChallenge.AvailableTeams = objListTeams;
                List<TrendingCategory> trendingCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);
                //objAdminChallenge.SetAvailableTrendingCategory(trendingCategoryList);
               // List<TrendingCategory> trendingFittnessCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constFittnessCommonSubTypeId);
                objAdminChallenge.SetAvailableTrendingCategory(trendingCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList());
                objAdminChallenge.AvailableSecondaryTrendingCategory= trendingCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constWorkoutChallengeSubType);
                objAdminChallenge.SetAvailableChallengeCategory(challengeCategoryList);

                return View(objAdminChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create admin challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListTrainers = null;
                objAllExerciseTypes = null;
            }
        }
        /// <summary>
        /// Action to create challenge in admin login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateWorkoutChallenge(AdminChallenge objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                objModels.ChallengeType = ConstantHelper.FreeformChallangeId;
                ValidateCreateAdminChallenge(objModels);
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<AdminChallenge, CreateChallengeVM>();
                    CreateChallengeVM objChalange = Mapper.Map<AdminChallenge, CreateChallengeVM>(objModels);
                    traceLog.AppendLine("Start: create admin challenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    int trainerId = 0;
                    if (objModels.TrainerId != null)
                    {
                        int trainerUserId = (int)objModels.TrainerId;
                        trainerId = (int)TrainersBL.GetTrainerId(trainerUserId);
                    }
                    FreeFormChallengeBL.SubmitAdminFreeFormChallenge(objChalange, credentialId, trainerId, string.Empty);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = 1, sort = ConstantHelper.constModifiedDate, sortdir = ConstantHelper.constDESC });
                }
                else
                {
                    ViewBag.ChallengeTypeList = ChallengesBL.GetChallengeType(); ;
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    if (challengeSubTypeId > 0)
                    {
                        objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    }
                    else if (objModels.ChallengeType == ConstantHelper.FreeformChallangeId)
                    {
                        objModels.SelectedChallengeTypeId = ConstantHelper.FreeformChallangeId;
                    }
                    if (objModels.ChallengeType > 0)
                    {
                        List<ChallengeTypes> objListChallengeSubType = ChallengesBL.GetChallengeSubType(objModels.ChallengeType);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    // List<ViewTrainers> objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = ChallengesBL.GetTrainers(); ;
                    /*Get all Exercise Types*/
                    ExerciseType exerciseType = new ExerciseType();
                    List<ExerciseType> objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.SetAvailableExerciseTypes(objAllExerciseTypes);
                    objModels.FFExeVideoLink1 = objModels.FFExeVideoUrl1;
                    objModels.FFExeVideoUrl1 = string.IsNullOrEmpty(objModels.FFExeVideoUrl1) ? string.Empty : objModels.FFExeVideoUrl1.Replace(" ", ConstantHelper.constExeciseFileNameExtraSpace);
                    if (objModels.PostedExerciseTypes != null)
                    {
                        List<ExerciseType> selectedExerciseTypes = new List<ExerciseType>();
                        if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                        {
                            foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                            {
                                exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedExerciseTypes.Add(exerciseType);
                            }
                        }
                        objModels.SetSelectedExerciseTypes(selectedExerciseTypes);
                    }
                    BodyPart objBodyPart = new BodyPart();
                    List<BodyPart> objAllTrainingZones = ChallengesCommonBL.GetBodyParts();
                    ViewBag.ListBodyPartList = objAllTrainingZones;
                    objModels.SetAvailableTargetZones(objAllTrainingZones);
                    if (objModels.PostedTargetZones != null)
                    {
                        List<BodyPart> selectedTargetZones = new List<BodyPart>();
                        if (objModels.PostedTargetZones.SelectedTargetZoneIDs != null)
                        {
                            foreach (var item in objModels.PostedTargetZones.SelectedTargetZoneIDs)
                            {
                                objBodyPart = objAllTrainingZones.Where(m => m.PartId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedTargetZones.Add(objBodyPart);
                            }
                        }
                        objModels.SetSelectedTargetZones(selectedTargetZones);
                    }
                    Equipments objEquipments = new Equipments();
                    List<Equipments> objAllEquipments = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = objAllEquipments;
                    objModels.SetAvailableEquipments(objAllEquipments);
                    if (objModels.PostedEquipments != null)
                    {
                        List<Equipments> selectedEquipments = new List<Equipments>();
                        if (objModels.PostedEquipments.SelectedEquipmentIDs != null)
                        {
                            foreach (var item in objModels.PostedEquipments.SelectedEquipmentIDs)
                            {
                                objEquipments = objAllEquipments.Where(m => m.EquipmentId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedEquipments.Add(objEquipments);
                            }
                        }
                        objModels.SetSelectedEquipments(selectedEquipments);
                    }

                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                    ViewBag.ChallengeSubTypeList = null;
                    ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    if(objModels.ChallengeSubTypeId == 0)
                    {
                        objModels.ChallengeSubTypeId = ConstantHelper.constWorkoutChallengeSubType;
                    }                    
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }
                    // Get Available trending category while fail validation
                    List<TrendingCategory> list= ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    objModels.SetAvailableTrendingCategory(list.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList());
                    objModels.AvailableSecondaryTrendingCategory=list.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            List<TrendingCategory> selecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    selecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                            objModels.SetSelecetdTrendingCategory(selecetdTrendingCategory);
                        }
                    }              
                    // Selected Team Id in case of validation
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    // Get Available challenge category while fail validation
                    objModels.SetAvailableChallengeCategory(ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId));
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            List<ChallengeCategory> SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SetSelecetdChallengeCategory(SelecetdChallengeCategory);
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateAdminChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }
        /// <summary>
        /// Validate Create AdminChallenge
        /// </summary>
        /// <param name="objModels"></param>
        [NonAction]
        private void ValidateCreateAdminChallenge(AdminChallenge objModels)
        {
            ModelState.Remove("CropImageRowData");
            if (objModels.ChallengeType == ConstantHelper.FreeformChallangeId)
            {
                ModelState.Remove("VariableValue");
                ModelState.Remove("VariableHours");
                ModelState.Remove("VariableMinute");
                ModelState.Remove("VariableSecond");
                ModelState.Remove("VariableMS");
                ModelState.Remove("GlobalResultFilterHours");
                // ModelState.Remove("ChallengeSubTypeId");
                ModelState.Remove("ExeName1");
                ModelState.Remove("ExeName2");
                if (objModels.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType)
                {
                    ModelState.Remove("FFExeName1");
                    ModelState.Remove("FFAExeName1");
                    ModelState.Remove("SelectedTargetZoneCheck");
                    ModelState.Remove("SelectedEquipmentCheck");
                    ModelState.Remove("ChallengeCategoryId");
                    ModelState.Remove("SelectedChallengeCategoryCheck");
                }
                else
                {
                    if (!objModels.IsFFAExeName1)
                    {
                        ModelState.Remove("FFAExeName1");
                    }
                    else
                    {
                        ModelState.Remove("FFExeName1");
                    }
                }
            }
            else
            {
                ModelState.Remove("Description");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("FFAExeName1");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("ChallengeCategoryId");
                ModelState.Remove("SelectedChallengeCategoryCheck");
                if (!string.IsNullOrEmpty(objModels.VariableLimit) && !string.IsNullOrEmpty(objModels.VariableValue))
                {
                    // Instantiate the regular expression object.   
                    if (objModels.VariableLimit.Contains(Message.VariableUnitmiles) || objModels.VariableLimit.Contains("Amount of time"))
                    {
                        string pat = @"^[0-9]+(\.[0-9]{1,2})?$";
                        // Match the regular expression pattern against a text string.
                        if (Regex.IsMatch(Convert.ToString(objModels.VariableValue), pat))
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                }
                else if (objModels.VariableUnit == null)
                {
                    ModelState.Remove("VariableValue");
                }
                // Add Variables minutes in case VariableUnit is in Minutes
                if (objModels.VariableUnit != null && objModels.VariableUnit.Equals(Message.VariableUnitminutes, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(objModels.VariableHours) || !string.IsNullOrEmpty(objModels.VariableMinute) || !string.IsNullOrEmpty(objModels.VariableSecond) || !string.IsNullOrEmpty(objModels.VariableMS))
                    {
                        if (objModels.VariableHours != ConstantHelper.constTimeVariableUnit || objModels.VariableMinute != ConstantHelper.constTimeVariableUnit || objModels.VariableSecond != ConstantHelper.constTimeVariableUnit || objModels.VariableMS != ConstantHelper.constTimeVariableUnit)
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                    objModels.VariableValue = CommonReportingUtility.FormatTimeVariable(objModels.VariableHours, objModels.VariableMinute, objModels.VariableSecond, objModels.VariableMS);
                }
                else
                {
                    ModelState.Remove("VariableHours");
                    ModelState.Remove("VariableMinute");
                    ModelState.Remove("VariableSecond");
                    ModelState.Remove("VariableMS");
                }
                // Add Global Result Filter in HHMMSS format
                if (string.IsNullOrEmpty(objModels.GlobalResultFilterValue))
                {
                    if (!string.IsNullOrEmpty(objModels.GlobalResultFilterHours) || !string.IsNullOrEmpty(objModels.GlobalResultFilterMinute) || !string.IsNullOrEmpty(objModels.GlobalResultFilterSecond) || !string.IsNullOrEmpty(objModels.GlobalResultFilterMS))
                    {
                        if (objModels.GlobalResultFilterHours != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterMinute != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterSecond != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterMS != ConstantHelper.constTimeVariableUnit)
                        {
                            ModelState.Remove("GlobalResultFilterHours");
                        }
                    }
                    objModels.GlobalResultFilterValue = CommonReportingUtility.FormatTimeVariable(objModels.GlobalResultFilterHours, objModels.GlobalResultFilterMinute, objModels.GlobalResultFilterSecond, objModels.GlobalResultFilterMS);
                }
                else
                {
                    ModelState.Remove("GlobalResultFilteHours");
                    ModelState.Remove("GlobalResultFilteMinute");
                    ModelState.Remove("GlobalResultFilteSecond");
                    ModelState.Remove("GlobalResultFilteMS");

                }
            }
        }
        [NonAction]
        private CreateChallengeVM GetViewchallengeData(int id)
        {

            List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
            List<SelectListItem> objSelectList = new List<SelectListItem>();
            string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
            CreateChallengeVM objChallenge = ChallengesBL.GetChallangeById(id, ref objSelectList, userType);
            if (objChallenge != null)
            {
                if (objChallenge.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType || objChallenge.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType)
                {
                    objChallenge.AvailableExerciseVideoList = FreeFormChallengeBL.GetAllFreeformChallangeExeciseById(id);
                }
                int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                if (challengeSubTypeId > 0)
                {
                    objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                    objChallenge.ChallengeType = challengeSubTypeId;
                }
                if (!string.IsNullOrEmpty(objChallenge.VariableValue))
                {
                    string[] variableValueWithMS = objChallenge.VariableValue.Split(new char[1] { '.' });
                    if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                    {
                        if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                        {
                            //Code for HH:MM:SS And MM:SS format
                            string tempValue = string.Empty;
                            char[] splitChar = { ':' };
                            string[] spliResult = variableValueWithMS[0].Split(splitChar);
                            tempValue = spliResult[0].Equals(ConstantHelper.constTimeVariableUnit) ? string.Empty : spliResult[0] + ConstantHelper.constHourdisplay;
                            if (!string.IsNullOrEmpty(tempValue))
                            {
                                tempValue += spliResult[1].Equals(ConstantHelper.constTimeVariableUnit) ? string.Empty : ", " + spliResult[1] + ConstantHelper.constMinutedisplay;
                            }
                            else
                            {
                                tempValue += spliResult[1].Equals(ConstantHelper.constTimeVariableUnit) ? string.Empty : spliResult[1] + ConstantHelper.constMinutedisplay;
                            }
                            if (!string.IsNullOrEmpty(tempValue))
                            {
                                tempValue += spliResult[2].Equals(ConstantHelper.constTimeVariableUnit) ? string.Empty : ", " + spliResult[2] + ConstantHelper.constSeconddisplay;
                            }
                            else
                            {
                                tempValue += spliResult[2].Equals(ConstantHelper.constTimeVariableUnit) ? string.Empty : spliResult[2] + ConstantHelper.constSeconddisplay;
                            }

                            objChallenge.VariableValue = tempValue;
                        }
                    }
                }
                /*Challenge types and sub-types*/
                if (objChallenge.ChallengeSubTypeId > 0)
                {
                    challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                    objChallenge.ChallengeSubType_Description = objListChallengeType[objChallenge.ChallengeSubTypeId - 1]
                        .ChallengeSubType.Replace("____", Convert.ToString(objChallenge.VariableValue)).Replace(ConstantHelper.constAmountOfTimewithQuestionMark, ConstantHelper.constQuestionMark).Replace(ConstantHelper.constSecondwithQuestionMark, ConstantHelper.constQuestionMark);
                    char[] split = { ' ' };
                    string[] tempData = objListChallengeType[challengeSubTypeId - 1].ChallengeType.Split(split);
                    objChallenge.ChallengeType_Name = tempData[0];
                    if (objChallenge.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType || objChallenge.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType)
                    {
                        objChallenge.ChallengeType_Name = objChallenge.ChallengeSubType_Description;
                        objChallenge.ChallengeSubType_Description = string.Empty;
                        objChallenge.ExeDesc1 = string.Empty;
                    }
                    /*Get Variavle Limit*/
                    ChallengeTypes objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                    objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId,
                                                               objChallengeType.Unit);
                    /*get challenge type detail*/
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    /*assign result unit to the model*/
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                }
            }

            return objChallenge;
        }
        /// <summary>
        /// Action to view challenge 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateTrainerChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeTrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: UpdateTrainerChallenge  controller");
                CreateChallengeVM objChallenge = GetViewchallengeData(id);

                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainerChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        [HttpGet]
        public ActionResult ViewTrainerChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeTrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: UpdateTrainerChallenge  controller");
                CreateChallengeVM objChallenge = GetViewchallengeData(id);
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainerChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to view update challenge view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: UpdateChallenge  controller");
                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                List<int> fittnessTypeId = ChallengesBL.GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                if (objListChallengeType != null)
                {
                    objListChallengeType = objListChallengeType.Where(ch => fittnessTypeId.Contains(ch.ChallengeSubTypeId)).ToList();
                }
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                CreateChallengeVM objChallenge = ChallengesBL.GetChallangeById(id, ref objSelectList, userType);
                int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                if (challengeSubTypeId > 0)
                {
                    objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                    objChallenge.ChallengeType = challengeSubTypeId;
                }
                if (challengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType == ConstantHelper.FreeFormChallengeType && !t.IsActive).ToList();
                }
                else
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType != ConstantHelper.FreeFormChallengeType).ToList();
                }
                ViewBag.ChallengeTypeList = objListChallengeType;
                ViewBag.ChallengeSubTypeList = null;
                if (objChallenge.ChallengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objChallenge.SelectedChallengeTypeId = objChallenge.ChallengeSubTypeId;
                }
                if (objChallenge.IsFeatured && !string.IsNullOrEmpty(objChallenge.FeaturedImageUrl))
                {
                    ViewBag.FeaturedPhoto = ConstantHelper.constfeaturedpicDir + objChallenge.FeaturedImageUrl;
                }
                /*Get Challenge types from challebge type master table*/
                ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                ViewBag.Trainers = ChallengesCommonBL.GetTrainersByChallengeId(id);
                /*Get Users of that chalenge who have completed the challenge from the database*/
                List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(id);
                ViewBag.Users = objListUsers;
                if (objChallenge.TrainerId != null && objChallenge.TrainerId != 0)
                {
                    int trainerCred = ChallengesBL.GetCredentialId((int)objChallenge.TrainerId);
                    ViewBag.TrainerResult = TrainersBL.GetTrainerResult(trainerCred, id);
                }
                /*Get all trainers*/
                ViewBag.TrainerList = ChallengesBL.GetTrainers();
                /*Get all Exercise Types*/
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                objChallenge.SelectedTargetZoneCheck = Message.NotAvailable;
                ViewBag.UserResult = TrainersBL.GetTrainerResult(objChallenge.EndUserCredntialId, id);
                objChallenge.SelectedEquipmentCheck = Message.NotAvailable;
                /*Get Variavle Limit*/
                ChallengeTypes objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                if (objChallengeType != null)
                {
                    if (objChallengeType.MaxLimit != 0)
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId,
                                                                   objChallengeType.Unit);
                    }
                    else
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} :", objChallengeType.ChallengeSubTypeId);
                    }
                }
                if (objChallenge != null)
                {
                    switch (objChallenge.ChallengeSubTypeId)
                    {
                        case 3:
                        case 4:
                        case 10:
                        case 12:
                            objChallenge.VariableLimit = string.Format("Type {0} - Amount of time:", objChallengeType.ChallengeSubTypeId);
                            break;
                    }
                }
                /*get challenge type detail*/
                if (objChallengeType != null)
                {
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                }
                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for equipment*/
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                /*assign result unit to the model*/
                CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                CommonReportingUtility.ResetChallengeGlobalVariable(objChallenge);
                CommonReportingUtility.ValidateChallengeGlobalVariableValue(objChallenge);
                objChallenge.Page = Request.QueryString[ConstantHelper.constpage] != null ? int.Parse(Request.QueryString[ConstantHelper.constpage]) : 1;
                objChallenge.SortField = Request.QueryString[ConstantHelper.constsort] != null ? Request.QueryString[ConstantHelper.constsort] : ConstantHelper.constChallengeName;
                objChallenge.Sortdir = Request.QueryString[ConstantHelper.constsortdir] != null ? Request.QueryString[ConstantHelper.constsortdir] : ConstantHelper.constASC;
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view update challenge view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateWorkoutChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: UpdateWorkoutChallenge  controller");


                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                CreateChallengeVM objChallenge = ChallengesBL.GetChallangeById(id, ref objSelectList, userType);
                int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                if (challengeSubTypeId > 0)
                {
                    objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                    objChallenge.ChallengeType = challengeSubTypeId;
                }
                if (challengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType == ConstantHelper.FreeFormChallengeType && !t.IsActive).ToList();
                }
                else
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType != ConstantHelper.FreeFormChallengeType).ToList();
                }
                ViewBag.ChallengeTypeList = objListChallengeType;
                ViewBag.ChallengeSubTypeList = null;
                if (objChallenge.ChallengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objChallenge.SelectedChallengeTypeId = objChallenge.ChallengeSubTypeId;
                }
                if (objChallenge.IsFeatured && !string.IsNullOrEmpty(objChallenge.FeaturedImageUrl))
                {
                    ViewBag.FeaturedPhoto = ConstantHelper.constfeaturedpicDir + objChallenge.FeaturedImageUrl;
                }
                /*Get Challenge types from challebge type master table*/
                // List<ChallengeTypes> objListChallengeSubType = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                ViewBag.Trainers = ChallengesCommonBL.GetTrainersByChallengeId(id);
                /*Get Users of that chalenge who have completed the challenge from the database*/
                List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(id);
                //ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objChallenge.ChallengeSubTypeId);
                ViewBag.Users = objListUsers;
                if (objChallenge.TrainerId != null && objChallenge.TrainerId != 0)
                {
                    int trainerCred = ChallengesBL.GetCredentialId((int)objChallenge.TrainerId);
                    ViewBag.TrainerResult = TrainersBL.GetTrainerResult(trainerCred, id);
                }
                /*Get all trainers*/
                ViewBag.TrainerList = ChallengesBL.GetTrainers();
                /*Get all Exercise Types*/
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                objChallenge.SelectedTargetZoneCheck = Message.NotAvailable;
                ViewBag.UserResult = TrainersBL.GetTrainerResult(objChallenge.EndUserCredntialId, id);
                objChallenge.SelectedEquipmentCheck = Message.NotAvailable;
                // objChallenge.SelectedChallengeCategoryCheck = Message.NotAvailable;
                /*Get Variavle Limit*/
                ChallengeTypes objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                if (objChallengeType != null)
                {
                    if (objChallengeType.MaxLimit != 0)
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId,
                                                                   objChallengeType.Unit);
                    }
                    else
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} :", objChallengeType.ChallengeSubTypeId);
                    }
                }
                if (objChallenge != null)
                {
                    switch (objChallenge.ChallengeSubTypeId)
                    {
                        case 3:
                        case 4:
                        case 10:
                        case 12:
                            objChallenge.VariableLimit = string.Format("Type {0} - Amount of time:", objChallengeType.ChallengeSubTypeId);
                            break;
                    }
                }
                /*get challenge type detail*/
                if (objChallengeType != null)
                {
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                }
                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for equipment*/
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                /*assign result unit to the model*/
                CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                CommonReportingUtility.ResetChallengeGlobalVariable(objChallenge);
                CommonReportingUtility.ValidateChallengeGlobalVariableValue(objChallenge);
                objChallenge.Page = Request.QueryString[ConstantHelper.constpage] != null ? int.Parse(Request.QueryString[ConstantHelper.constpage]) : 1;
                objChallenge.SortField = Request.QueryString[ConstantHelper.constsort] != null ? Request.QueryString[ConstantHelper.constsort] : ConstantHelper.constChallengeName;
                objChallenge.Sortdir = Request.QueryString[ConstantHelper.constsortdir] != null ? Request.QueryString[ConstantHelper.constsortdir] : ConstantHelper.constASC;
                //   HttpContext.Session["SelectedTrainerId"]
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateWorkoutChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Copy Free Form Challenges only based on saved workout.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CopyChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: CopyChallenge  controller");
                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                CreateChallengeVM objChallenge = ChallengesBL.GetCopyChallangeById(id, ref objSelectList, userType, credentialId);
                int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                if (challengeSubTypeId > 0)
                {
                    objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                    objChallenge.ChallengeType = challengeSubTypeId;
                }
                if (challengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType == ConstantHelper.FreeFormChallengeType && !t.IsActive).ToList();
                }
                else
                {
                    objListChallengeType = objListChallengeType.Where(t => t.ChallengeType != ConstantHelper.FreeFormChallengeType).ToList();
                }
                ViewBag.ChallengeTypeList = objListChallengeType;
                ViewBag.ChallengeSubTypeList = null;
                if (objChallenge.ChallengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objChallenge.SelectedChallengeTypeId = objChallenge.ChallengeSubTypeId;
                }
                if (objChallenge.IsFeatured && !string.IsNullOrEmpty(objChallenge.FeaturedImageUrl))
                {
                    ViewBag.FeaturedPhoto = ConstantHelper.constfeaturedpicDir + objChallenge.FeaturedImageUrl;
                }
                /*Get Challenge types from challebge type master table*/
                ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                ViewBag.Trainers = ChallengesCommonBL.GetTrainersByChallengeId(id);
                /*Get Users of that chalenge who have completed the challenge from the database*/
                List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(id);
                ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objChallenge.ChallengeSubTypeId);
                ViewBag.Users = objListUsers;
                if (objChallenge.TrainerId != null && objChallenge.TrainerId != 0)
                {
                    int trainerCred = ChallengesBL.GetCredentialId((int)objChallenge.TrainerId);
                    ViewBag.TrainerResult = TrainersBL.GetTrainerResult(trainerCred, id);
                }
                /*Get all trainers*/
                ViewBag.TrainerList = ChallengesBL.GetTrainers();
                /*Get all Exercise Types*/
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                objChallenge.SelectedTargetZoneCheck = Message.NotAvailable;
                ViewBag.UserResult = TrainersBL.GetTrainerResult(objChallenge.EndUserCredntialId, id);
                objChallenge.SelectedChallengeCategoryCheck = Message.NotAvailable;
                objChallenge.SelectedEquipmentCheck = Message.NotAvailable;
                /*Get Variavle Limit*/
                ChallengeTypes objChallengeType = ChallengesCommonBL.GetChallengeVal(objChallenge.ChallengeSubTypeId);
                if (objChallengeType != null)
                {
                    if (objChallengeType.MaxLimit != 0)
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId,
                                                                   objChallengeType.Unit);
                    }
                    else
                    {
                        objChallenge.VariableLimit = string.Format("Type {0} :", objChallengeType.ChallengeSubTypeId);
                    }
                }
                if (objChallenge != null)
                {
                    switch (objChallenge.ChallengeSubTypeId)
                    {
                        case 3:
                        case 4:
                        case 10:
                        case 12:
                            objChallenge.VariableLimit = string.Format("Type {0} - Amount of time:", objChallengeType.ChallengeSubTypeId);
                            break;
                    }
                }
                /*get challenge type detail*/
                if (objChallengeType != null)
                {
                    objChallenge.IsMoreThenOne = objChallengeType.IsExerciseMoreThanOne;
                    objChallenge.VariableUnit = objChallengeType.Unit;
                    objChallenge.ResultUnitType = objChallengeType.ResultUnit;
                }
                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for equipment*/
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                /*assign result unit to the model*/
                CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                CommonReportingUtility.ResetChallengeGlobalVariable(objChallenge);
                CommonReportingUtility.ValidateChallengeGlobalVariableValue(objChallenge);
                ViewBag.TestList = null; /*We have to change. It is use for all empty dropdown*/
                objChallenge.Page = int.Parse(Request.QueryString[ConstantHelper.constpage]);
                objChallenge.SortField = Request.QueryString[ConstantHelper.constsort];
                objChallenge.Sortdir = Request.QueryString[ConstantHelper.constsortdir];
                return View(ConstantHelper.constUpdateChallenge, objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        [HttpGet]
        public ActionResult CopyWorkoutChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: CopyWorkoutChallenge  controller");
                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetWorkoutType();
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                CreateChallengeVM objChallenge = ChallengesBL.GetCopyChallangeById(id, ref objSelectList, userType, credentialId);
                int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objChallenge.ChallengeSubTypeId);
                if (challengeSubTypeId > 0)
                {
                    objChallenge.SelectedChallengeTypeId = challengeSubTypeId;
                    objChallenge.ChallengeType = challengeSubTypeId;
                }
                ViewBag.ChallengeSubTypeList = objListChallengeType;
                if (objChallenge.ChallengeSubTypeId == ConstantHelper.FreeformChallangeId)
                {
                    objChallenge.SelectedChallengeTypeId = objChallenge.ChallengeSubTypeId;
                }
                if (objChallenge.IsFeatured && !string.IsNullOrEmpty(objChallenge.FeaturedImageUrl))
                {
                    ViewBag.FeaturedPhoto = ConstantHelper.constfeaturedpicDir + objChallenge.FeaturedImageUrl;
                }
                /*Get Challenge types from challebge type master table*/
                ViewBag.ChallengeTypeSubList = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                ViewBag.Trainers = ChallengesCommonBL.GetTrainersByChallengeId(id);
                /*Get Users of that chalenge who have completed the challenge from the database*/
                List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(id);
                ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objChallenge.ChallengeSubTypeId);
                ViewBag.Users = objListUsers;
                if (objChallenge.TrainerId != null && objChallenge.TrainerId != 0)
                {
                    int trainerCred = ChallengesBL.GetCredentialId((int)objChallenge.TrainerId);
                    ViewBag.TrainerResult = TrainersBL.GetTrainerResult(trainerCred, id);
                }
                /*Get all trainers*/
                ViewBag.TrainerList = ChallengesBL.GetTrainers();
                /*Get all Exercise Types*/
                ViewBag.ExerciseTypeList = ChallengesCommonBL.GetExerciseTypes();
                objChallenge.SelectedTargetZoneCheck = Message.NotAvailable;
                ViewBag.UserResult = TrainersBL.GetTrainerResult(objChallenge.EndUserCredntialId, id);
                objChallenge.SelectedChallengeCategoryCheck = Message.NotAvailable;
                objChallenge.SelectedEquipmentCheck = Message.NotAvailable;
                /*Get Variavle Limit*/

                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for equipment*/
                ViewBag.ListEquipment = ChallengesCommonBL.GetEquipments();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetDifficulties();
                /*assign result unit to the model*/
                CommonReportingUtility.ValidateChallengeTimeVariableValue(objChallenge);
                CommonReportingUtility.ResetChallengeGlobalVariable(objChallenge);
                CommonReportingUtility.ValidateChallengeGlobalVariableValue(objChallenge);
                ViewBag.TestList = null; /*We have to change. It is use for all empty dropdown*/
                objChallenge.Page = int.Parse(Request.QueryString[ConstantHelper.constpage]);
                objChallenge.SortField = Request.QueryString[ConstantHelper.constsort];
                objChallenge.Sortdir = Request.QueryString[ConstantHelper.constsortdir];
                return View(ConstantHelper.constUpdateWorkoutChallenge, objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("CopyWorkoutChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to update challenge 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult UpdateChallenge(CreateChallengeVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                ValidateUpdateChallenge(objModels);
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateChallenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    //if (objModels.ProfileUrl != null)
                    if (objModels.IsFeatured && !String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constFeaturedImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            objModels.FeaturedImageUrl = picName;
                        }
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.FeaturedImageUrl = newpicName;
                        }
                    }
                    else
                    {
                        objModels.FeaturedImageUrl = string.Empty;
                    }
                    ChallengesBL.UpdateChallenges(objModels, credentialId);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    ViewBag.FreeFormExeciseList = null;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = objModels.Page, sort = objModels.SortField, sortdir = objModels.Sortdir });

                }
                else
                {
                    List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    if (challengeSubTypeId != 0)
                    {
                        List<ChallengeTypes> objListChallengeSubType = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    /*Get Trainers of that chalenge who have completed the challenge from the database*/
                    List<TrainerViewVM> objListTrainers = ChallengesCommonBL.GetTrainersByChallengeId(objModels.ChallengeId);
                    ViewBag.Trainers = objListTrainers;
                    if (objModels.FeaturedImageUrl != null)
                    {
                        ViewBag.FeaturedPhoto = ConstantHelper.constprofilepicDir + objModels.FeaturedImageUrl;
                    }
                    /*Get Users of that chalenge who have completed the challenge from the database*/
                    List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(objModels.ChallengeId);
                    ViewBag.Users = objListUsers;
                    if (objModels.TrainerId != null && objModels.TrainerId != 0)
                    {
                        int trainerCred = ChallengesBL.GetCredentialId((int)objModels.TrainerId);
                        List<UserResult> objTrainerResult = TrainersBL.GetTrainerResult(trainerCred, objModels.ChallengeId);
                        ViewBag.TrainerResult = objTrainerResult;
                    }

                    if (objModels.EndUserNameId != null && objModels.EndUserNameId != 0)
                    {
                        int userCred = ProfileBL.GetUserCredentialId((int)objModels.EndUserNameId);
                        List<UserResult> objUserResult = TrainersBL.GetTrainerResult(userCred, objModels.ChallengeId);
                        ViewBag.UserResult = objUserResult;
                    }

                    /*Get all trainers*/
                    List<ViewTrainers> objAllTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objAllTrainers;
                    BodyPart objBodyPart = new BodyPart();
                    List<BodyPart> objAllTrainingZones = ChallengesCommonBL.GetBodyParts();
                    ViewBag.ListBodyPartList = objAllTrainingZones;
                    objModels.AvailableTargetZones = objAllTrainingZones;
                    objModels.SelectedTargetZones = new List<BodyPart>();
                    if (objModels.PostedTargetZones != null)
                    {
                        if (objModels.PostedTargetZones.SelectedTargetZoneIDs != null)
                        {
                            foreach (var item in objModels.PostedTargetZones.SelectedTargetZoneIDs)
                            {
                                objBodyPart = objAllTrainingZones.Where(m => m.PartId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelectedTargetZones.Add(objBodyPart);
                            }
                        }
                    }

                    Equipments objEquipments = new Equipments();
                    List<Equipments> objAllEquipments = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = objAllEquipments;
                    objModels.AvailableEquipments = objAllEquipments;
                    objModels.SelectedEquipments = new List<Equipments>();
                    if (objModels.PostedEquipments != null)
                    {
                        if (objModels.PostedEquipments.SelectedEquipmentIDs != null)
                        {
                            foreach (var item in objModels.PostedEquipments.SelectedEquipmentIDs)
                            {
                                objEquipments = objAllEquipments.Where(m => m.EquipmentId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelectedEquipments.Add(objEquipments);
                            }
                        }
                    }

                    /*Code for radio button list for difficulty*/
                    List<Difficulties> objListDifficulty = ChallengesCommonBL.GetDifficulties();
                    ViewBag.ListDifficulty = objListDifficulty;
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);
                    /*Get all Exercise Types*/
                    ExerciseType exerciseType = new ExerciseType();
                    List<ExerciseType> objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.AvailableExerciseTypes = objAllExerciseTypes;
                    objModels.SelectedExerciseTypes = new List<ExerciseType>();
                    if (objModels.PostedExerciseTypes != null)
                    {
                        if (objModels.PostedExerciseTypes != null)
                        {
                            if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                            {
                                foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                                {
                                    exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();

                                    objModels.SelectedExerciseTypes.Add(exerciseType);
                                }
                            }
                        }
                    }

                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }

                    // Get Available trending category while fail validation
                    var allSelectedTrendingCatList= ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    objModels.AvailableTrendingCategory = allSelectedTrendingCatList.Where(cat => cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    objModels.AvailableSecondaryTrendingCategory = allSelectedTrendingCatList.Where(cat => cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }

                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    TempData["AlertMessage"] = string.Empty;
                    // ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    return View(objModels);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ModelState, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to update challenge 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult UpdateWorkoutChallenge(CreateChallengeVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                objModels.ChallengeType = ConstantHelper.FreeformChallangeId;
                ValidateUpdateChallenge(objModels);
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateWorkoutChallenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    //if (objModels.ProfileUrl != null)
                    if (objModels.IsFeatured && !String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constFeaturedImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            objModels.FeaturedImageUrl = picName;
                        }
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.FeaturedImageUrl = newpicName;
                        }
                    }
                    else
                    {
                        objModels.FeaturedImageUrl = string.Empty;
                    }
                    FreeFormChallengeBL.UpdateAdminFreeFormChallenges(objModels, credentialId);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    ViewBag.FreeFormExeciseList = null;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = objModels.Page, sort = objModels.SortField, sortdir = objModels.Sortdir });

                }
                else
                {
                    List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    int challengeSubTypeId = ChallengesCommonBL.GetChallengeSubTypeId(objModels.ChallengeSubTypeId);
                    objModels.SelectedChallengeTypeId = challengeSubTypeId;
                    if (challengeSubTypeId != 0)
                    {
                        List<ChallengeTypes> objListChallengeSubType = ChallengesBL.GetChallengeSubType(challengeSubTypeId);
                        ViewBag.ChallengeTypeSubList = objListChallengeSubType;
                    }
                    /*Get Trainers of that chalenge who have completed the challenge from the database*/
                    List<TrainerViewVM> objListTrainers = ChallengesCommonBL.GetTrainersByChallengeId(objModels.ChallengeId);
                    ViewBag.Trainers = objListTrainers;
                    if (objModels.FeaturedImageUrl != null)
                    {
                        ViewBag.FeaturedPhoto = ConstantHelper.constprofilepicDir + objModels.FeaturedImageUrl;
                    }
                    /*Get Users of that chalenge who have completed the challenge from the database*/
                    List<CreateUserVM> objListUsers = ChallengesCommonBL.GetUserByChallengeId(objModels.ChallengeId);
                    ViewBag.Users = objListUsers;
                    if (objModels.TrainerId != null && objModels.TrainerId != 0)
                    {
                        int trainerCred = ChallengesBL.GetCredentialId((int)objModels.TrainerId);
                        List<UserResult> objTrainerResult = TrainersBL.GetTrainerResult(trainerCred, objModels.ChallengeId);
                        ViewBag.TrainerResult = objTrainerResult;
                    }

                    if (objModels.EndUserNameId != null && objModels.EndUserNameId != 0)
                    {
                        int userCred = ProfileBL.GetUserCredentialId((int)objModels.EndUserNameId);
                        List<UserResult> objUserResult = TrainersBL.GetTrainerResult(userCred, objModels.ChallengeId);
                        ViewBag.UserResult = objUserResult;
                    }

                    /*Get all trainers*/
                    List<ViewTrainers> objAllTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objAllTrainers;
                    BodyPart objBodyPart = new BodyPart();
                    List<BodyPart> objAllTrainingZones = ChallengesCommonBL.GetBodyParts();
                    ViewBag.ListBodyPartList = objAllTrainingZones;
                    objModels.AvailableTargetZones = objAllTrainingZones;
                    objModels.SelectedTargetZones = new List<BodyPart>();
                    if (objModels.PostedTargetZones != null)
                    {
                        if (objModels.PostedTargetZones.SelectedTargetZoneIDs != null)
                        {
                            foreach (var item in objModels.PostedTargetZones.SelectedTargetZoneIDs)
                            {
                                objBodyPart = objAllTrainingZones.Where(m => m.PartId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelectedTargetZones.Add(objBodyPart);
                            }
                        }
                    }

                    Equipments objEquipments = new Equipments();
                    List<Equipments> objAllEquipments = ChallengesCommonBL.GetEquipments();
                    /*Code for radio button list for equipment*/
                    ViewBag.ListEquipment = objAllEquipments;
                    objModels.AvailableEquipments = objAllEquipments;
                    objModels.SelectedEquipments = new List<Equipments>();
                    if (objModels.PostedEquipments != null)
                    {
                        if (objModels.PostedEquipments.SelectedEquipmentIDs != null)
                        {
                            foreach (var item in objModels.PostedEquipments.SelectedEquipmentIDs)
                            {
                                objEquipments = objAllEquipments.Where(m => m.EquipmentId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelectedEquipments.Add(objEquipments);
                            }
                        }
                    }

                    /*Code for radio button list for difficulty*/
                    List<Difficulties> objListDifficulty = ChallengesCommonBL.GetDifficulties();
                    ViewBag.ListDifficulty = objListDifficulty;
                    ViewBag.ChallengeCategoryList = ChallengesCommonBL.GetChallengeCategoryList(objModels.ChallengeSubTypeId);
                    /*Get all Exercise Types*/
                    ExerciseType exerciseType = new ExerciseType();
                    List<ExerciseType> objAllExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                    ViewBag.ExerciseTypeList = objAllExerciseTypes;
                    objModels.AvailableExerciseTypes = objAllExerciseTypes;
                    objModels.SelectedExerciseTypes = new List<ExerciseType>();
                    if (objModels.PostedExerciseTypes != null)
                    {
                        if (objModels.PostedExerciseTypes != null)
                        {
                            if (objModels.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                            {
                                foreach (var item in objModels.PostedExerciseTypes.SelectedExerciseTypeIDs)
                                {
                                    exerciseType = objAllExerciseTypes.Where(m => m.ExerciseTypeId == Convert.ToInt32(item)).FirstOrDefault();

                                    objModels.SelectedExerciseTypes.Add(exerciseType);
                                }
                            }
                        }
                    }

                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }

                    // Get Available trending category while fail validation
                    var allSelectedTrendingCatList= ChallengesCommonBL.GetTrendingCategory(objModels.ChallengeSubTypeId);
                    objModels.AvailableTrendingCategory = allSelectedTrendingCatList.Where(cat => cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }

                    objModels.AvailableSecondaryTrendingCategory = allSelectedTrendingCatList.Where(cat => cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }

                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ChallengeSubTypeId);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    TempData["AlertMessage"] = string.Empty;
                    // ViewBag.FreeFormExeciseList = objModels.FreeFormExerciseNameDescriptionList;
                    return View(objModels);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ModelState, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateWorkoutChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Validate Update Challenge
        /// </summary>
        /// <param name="objModels"></param>
        [NonAction]
        private void ValidateUpdateChallenge(CreateChallengeVM objModels)
        {
            if (!objModels.IsSetToCOD)
            {
                ModelState.Remove("CODStartDate");
                ModelState.Remove("CODEndDate");
                ModelState.Remove("EndUserNameId");
                ModelState.Remove("UserResultId");
                ModelState.Remove("UserVideoLink");
            }
            if (!objModels.IsSetToSponsorChallenge)
            {
                ModelState.Remove("TCStartDate");
                ModelState.Remove("TCEndDate");
                ModelState.Remove("TrainerId");
                ModelState.Remove("ResultId");
                ModelState.Remove("TrainerVideoLink");
                ModelState.Remove("SponsorName");
            }
            if (objModels.ChallengeType == ConstantHelper.FreeformChallangeId)
            {
                // Remove the None Free Form challenge Required Field
                ModelState.Remove("VariableValue");
                ModelState.Remove("VariableHours");
                ModelState.Remove("VariableMinute");
                ModelState.Remove("VariableSecond");
                ModelState.Remove("VariableMS");
                ModelState.Remove("GlobalResultFilterHours");
                //  ModelState.Remove("ChallengeSubTypeId");
                ModelState.Remove("ExeName1");
                ModelState.Remove("ExeName2");
                ModelState.Remove("GlobalResultFilteHours");
                ModelState.Remove("GlobalResultFilteMinute");
                ModelState.Remove("GlobalResultFilteSecond");
                ModelState.Remove("GlobalResultFilteMS");
                ModelState.Remove("ResultId");
                ModelState.Remove("UserResultId");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("VariableValue");
                ModelState.Remove("VariableHours");
                ModelState.Remove("VariableMinute");
                ModelState.Remove("VariableSecond");
                ModelState.Remove("VariableMS");
                ModelState.Remove("GlobalResultFilterHours");
                // ModelState.Remove("ChallengeSubTypeId");
                ModelState.Remove("ExeName1");
                ModelState.Remove("ExeName2");
                if (objModels.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType)
                {
                    ModelState.Remove("FFAExeName1");
                    ModelState.Remove("FFExeName1");
                    ModelState.Remove("SelectedTargetZoneCheck");
                    ModelState.Remove("SelectedEquipmentCheck");
                    ModelState.Remove("ChallengeCategoryId");
                    ModelState.Remove("CropImageRowData");
                    ModelState.Remove("SelectedChallengeCategoryCheck");
                }
                else
                {
                    if (!objModels.IsFeatured || !string.IsNullOrEmpty(objModels.FeaturedImageUrl))
                    {
                        ModelState.Remove("CropImageRowData");
                    }
                    if (!objModels.IsFFAExeName1)
                    {
                        ModelState.Remove("FFAExeName1");
                    }
                    else
                    {
                        ModelState.Remove("FFExeName1");
                    }

                }
            }
            else
            {
                ModelState.Remove("Description");
                ModelState.Remove("ChallengeCategory");
                ModelState.Remove("ChallengeCategoryId");
                ModelState.Remove("FFAExeName1");
                ModelState.Remove("FFExeName1");
                ModelState.Remove("SelectedChallengeCategoryCheck");
                if (!objModels.IsFeatured || !string.IsNullOrEmpty(objModels.FeaturedImageUrl))
                {
                    ModelState.Remove("CropImageRowData");
                }
                if (!string.IsNullOrEmpty(objModels.VariableLimit) && !string.IsNullOrEmpty(objModels.VariableValue))
                {
                    // Instantiate the regular expression object.   
                    if (objModels.VariableLimit.Contains(Message.VariableUnitmiles))
                    {
                        string pat = @"^[0-9]+(\.[0-9]{1,2})?$";
                        // Match the regular expression pattern against a text string.
                        if (Regex.IsMatch(Convert.ToString(objModels.VariableValue), pat))
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                }
                else if (objModels.VariableUnit == null)
                {
                    ModelState.Remove("VariableValue");
                }
                if (objModels.VariableUnit != null && objModels.VariableUnit.Equals(Message.VariableUnitminutes, StringComparison.OrdinalIgnoreCase))
                {
                    //ModelState["VariableValue"].Errors.Clear();
                    if (!string.IsNullOrEmpty(objModels.VariableHours) || !string.IsNullOrEmpty(objModels.VariableMinute) || !string.IsNullOrEmpty(objModels.VariableSecond) || !string.IsNullOrEmpty(objModels.VariableMS))
                    {
                        if (objModels.VariableHours != ConstantHelper.constTimeVariableUnit || objModels.VariableMinute != ConstantHelper.constTimeVariableUnit || objModels.VariableSecond != ConstantHelper.constTimeVariableUnit || objModels.VariableMS != ConstantHelper.constTimeVariableUnit)
                        {
                            ModelState.Remove("VariableValue");
                        }
                    }
                    objModels.VariableValue = CommonReportingUtility.FormatTimeVariable(objModels.VariableHours, objModels.VariableMinute, objModels.VariableSecond, objModels.VariableMS);
                }
                else
                {
                    ModelState.Remove("VariableHours");
                    ModelState.Remove("VariableMinute");
                    ModelState.Remove("VariableSecond");
                    ModelState.Remove("VariableSecond");
                    ModelState.Remove("VariableMS");
                }
                // Add Global Result Filter in HHMMSS format
                if (string.IsNullOrEmpty(objModels.GlobalResultFilterValue))
                {
                    if (!string.IsNullOrEmpty(objModels.GlobalResultFilterHours) || !string.IsNullOrEmpty(objModels.GlobalResultFilterMinute) || !string.IsNullOrEmpty(objModels.GlobalResultFilterSecond) || !string.IsNullOrEmpty(objModels.GlobalResultFilterMS))
                    {
                        if (objModels.GlobalResultFilterHours != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterMinute != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterSecond != ConstantHelper.constTimeVariableUnit || objModels.GlobalResultFilterMS != ConstantHelper.constTimeVariableUnit)
                        {
                            ModelState.Remove("GlobalResultFilterValue");
                        }
                    }
                    objModels.GlobalResultFilterValue = CommonReportingUtility.FormatTimeVariable(objModels.GlobalResultFilterHours, objModels.GlobalResultFilterMinute, objModels.GlobalResultFilterSecond, objModels.GlobalResultFilterMS);
                }
                else
                {
                    ModelState.Remove("GlobalResultFilteHours");
                    ModelState.Remove("GlobalResultFilteMinute");
                    ModelState.Remove("GlobalResultFilteSecond");
                    ModelState.Remove("GlobalResultFilteMS");
                }
            }
        }
        /// <summary>
        /// Action to delete challenge
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult DeleteData(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: DeleteData  controller");
                ChallengesBL.DeleteChallenge(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                int page = int.Parse(Request.QueryString[ConstantHelper.constpage]);
                string sortField = Request.QueryString[ConstantHelper.constsort];
                string sortdir = Request.QueryString[ConstantHelper.constsortdir];
                return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = page, sort = sortField, sortdir = sortdir });
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteData end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// View Challenge based on challenge Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewChallenge()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: ViewChallenge  controller");
                int challengeId = 0;
                challengeId = Convert.ToInt32(Request.QueryString["id"]);
                ViewWorkoutDetailVM objChallenge = new ViewWorkoutDetailVM();
                if (challengeId > 0)
                {
                    objChallenge = ProgramBL.GetProgramWorkoutById(challengeId);
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("ViewChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Auto Saving Execise Set into database
        /// </summary>
        /// <param name="CEARecordId"></param>
        /// <param name="ExeciseSetDataString"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AutoSaveExeciseSet(int CEARecordId, string ExeciseSetDataString)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: AutoSaveExeciseSet.CEARecordId-" + CEARecordId + ",ExeciseSetDataString-" + ExeciseSetDataString);
                bool isSaved = false;
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                isSaved = CommonBL.SaveExeciseSet(CEARecordId, ExeciseSetDataString, credentialId);
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("AutoSaveExeciseSet end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Auto Save Execise on challenge
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="execiseDescription"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AutoSaveExecise(int challengeId, string execiseDescription)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: AutoSaveExeciseSet-challengeId-" + challengeId + ",execiseDescription-" + execiseDescription);
                bool isSaved = false;
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                isSaved = CommonBL.AutoSaveExecise(challengeId, execiseDescription, credentialId);
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("AutoSaveExeciseSet end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// AutoSave All ExeciseWithSets based on program or challenge Id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="execiseDescription"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AutoSaveAllExeciseWithSets(int challengeId, string execiseDescription)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: AutoSaveAllExeciseWithSets-challengeId-" + challengeId + ",execiseDescription-" + execiseDescription);
                bool isSaved = false;
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                isSaved = CommonBL.AutoSaveAllExeciseWithSets(challengeId, execiseDescription, credentialId);
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("AutoSaveAllExeciseWithSets end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Validate Onborading ExeciseId for restricate the user to selecet duploicate on team
        /// </summary>
        /// <param name="searchExeise"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidateOnboradingExeciseId(SearchOnBoradingRequest searchExeise)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: ValidateOnborading SelectedExeciseID-" + searchExeise.ExeciseID);
                bool isAvailable = true;
                if (ChallengesCommonBL.ValidateOnTeamboardingExeciseID(searchExeise.ExeciseID, searchExeise.TeamID))
                {
                    isAvailable = false;
                }
                else
                {
                    isAvailable = true;
                }

                return Json(isAvailable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("ValidateOnboradingExeciseId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Trending CategoryByType based on program/challenge/FittnessTest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTrendingCategoryByType(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTrendingCategoryByType");
                List<TrendingCategory> objListChallengeType = ChallengesCommonBL.GetTrendingCategory(id);
                return Json(new SelectList(objListChallengeType.ToArray(), "TrendingCategoryId", "TrendingCategoryName"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetTrendingCategoryByType end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        #endregion
        #region Trainers

        /// <summary>
        /// check Trainer Challenge Name is available or not
        /// </summary>
        /// <param name="challengeName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTrainerChallengeName(string challengeName)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTrainerChallengeName");
                string result = string.Empty;
                int trainerCredentialId = 0;
                trainerCredentialId = (int)HttpContext.Session["CredentialId"];
                if (ChallengesCommonBL.GetTrainersByChallengeName(trainerCredentialId, challengeName))
                {
                    result = "Available";
                }
                else
                {
                    result = "Not Available";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetTrainerChallengeName end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to view trainers view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Trainers()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: Trainers controller");
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = 0;
                /*Get Challenges from the database*/
                List<ViewTrainers> objListChallenge = TrainersBL.GetTrainers();
                HttpContext.Session[Message.PreviousUrl] = Message.TrainerUrl;
                return View(objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Trainers end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        // Get All teams information created by admin
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Teams()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: Teams controller");
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = 0;
                /*Get Challenges from the database*/
                List<ViewTeams> objListChallenge = TeamBL.GetALLTeams();
                HttpContext.Session[Message.PreviousUrl] = Message.TeamUrl;
                return View(objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Teams end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view create trainer page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateTrainer()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: create Trainer controller");
                CreateTrainerVM objtrainer = new CreateTrainerVM();
                List<Specialization> objListSpecialization = TrainersBL.GetSpecializations();
                List<State> objListStates = TrainersBL.GetState();
                ViewBag.lstStates = objListStates;
                ViewBag.lstSpecializationList = objListSpecialization;
                List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                ViewBag.lstTeams = objListTeams;
                objtrainer.SetAvailableTeams(objListTeams);
                objtrainer.SetAvailableSpecializations(objListSpecialization);
                return View(objtrainer);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create Trainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to create trainer
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (!string.IsNullOrEmpty(objModels.PhoneNumber))
                {
                    var isPhoneAllzero = objModels.PhoneNumber.All(c => c == '0');
                    if (isPhoneAllzero)
                    {
                        ModelState.AddModelError("PhoneNumber", "Entered phone number is not valid.");
                    }
                    else
                    {
                        ModelState.Remove("PhoneNumber");
                    }
                }
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: CreateTrainer controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.CreatedBy = credentialId;

                    //if (objModels.ProfileUrl != null)
                    if (!String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constProfileImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        var bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        objModels.TrainerImageUrl = picName;
                    }
                    TrainersBL.SubmitTrainer(objModels);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    Specialization specialization = new Specialization();
                    List<Specialization> objListSpecialization = TrainersBL.GetSpecializations();
                    objModels.SetAvailableSpecializations(objListSpecialization);

                    DDTeams teams = new DDTeams();
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.SetAvailableTeams(objListTeams);

                    List<State> objListStates = TrainersBL.GetState();
                    ViewBag.lstStates = objListStates;
                    ViewBag.lstTeams = objListTeams;
                    List<City> objListCitye = TrainersBL.GetCities(objModels.State);
                    ViewBag.lstCities = objListCitye;

                    if (objModels.PostedSpecializations != null)
                    {
                        List<Specialization> selectedSpecializations = null;
                        if (objModels.PostedSpecializations.PrimarySpecializationIDs != null)
                        {
                            selectedSpecializations = new List<Specialization>();
                            foreach (var item in objModels.PostedSpecializations.PrimarySpecializationIDs)
                            {
                                specialization = objListSpecialization.Where(m => m.SpecializationId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedSpecializations.Add(specialization);
                            }
                            objModels.SetSelectedTopthreeSpecializations(selectedSpecializations);
                        }
                        if (objModels.PostedSpecializations.SecondarySpecializationIDs != null)
                        {
                            selectedSpecializations = new List<Specialization>();
                            foreach (var item in objModels.PostedSpecializations.SecondarySpecializationIDs)
                            {
                                specialization = objListSpecialization.Where(m => m.SpecializationId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedSpecializations.Add(specialization);
                            }
                            objModels.SetSelectedSecondarySpecializations(selectedSpecializations);
                        }
                    }
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            List<DDTeams> selecetdTeams = new List<DDTeams>();
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                selecetdTeams.Add(teams);
                            }
                            objModels.SetSelecetdTeams(selecetdTeams);
                        }
                    }
                    ViewBag.TrainerProfilePhoto = objModels.CropImageRowData;
                    return View(objModels);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// Action to view update trainer
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateTrainer(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: UpdateTrainer  controller");
                UpdateTrainerVM objTrainer = TrainersBL.GetTrainerById(id);
                List<State> objListStates = TrainersBL.GetState();
                ViewBag.lstStates = objListStates;
                List<City> objListCitye = TrainersBL.GetCities(objTrainer.State);
                ViewBag.lstCities = objListCitye;
                ViewBag.lstTeams = TeamBL.GetAllTeamName();
                String output = Server.HtmlDecode(objTrainer.AboutMe);
                objTrainer.AboutMe = output;
                objTrainer.PrimarySpecializationCheck = Message.NotAvailable;
                if (objTrainer.TrainerImageUrl != null)
                {
                    ViewBag.TrainerProfilePhoto = ConstantHelper.constprofilepicDir + objTrainer.TrainerImageUrl;
                }
                return View(objTrainer);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to update trainer
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult UpdateTrainer(UpdateTrainerVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                if (!objModels.IsChangePassword)
                {
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (!string.IsNullOrEmpty(objModels.PhoneNumber))
                {
                    var isPhoneAllzero = objModels.PhoneNumber.All(c => c == '0');
                    if (isPhoneAllzero)
                    {
                        ModelState.AddModelError("PhoneNumber", "Entered phone number is not valid.");
                    }
                    else
                    {
                        ModelState.Remove("PhoneNumber");
                    }
                }
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateTrainer controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.ModifyBy = credentialId;
                    if (!string.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        string root = Server.MapPath("~/images/profilepic");
                        string base64data = string.Empty;
                        string extension = ConstantHelper.constImageExtension;
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        var bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.const_Resize + extension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        objModels.TrainerImageUrl = picName;
                    }
                    TrainersBL.UpdateTrainer(objModels);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    UpdateTrainerVM objTrainer = TrainersBL.GetTrainerById(objModels.TrainerId);
                    Specialization specialization = new Specialization();
                    List<City> objListCitye = TrainersBL.GetCities(objTrainer.State);
                    ViewBag.lstCities = objListCitye;
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    ViewBag.lstTeams = objListTeams;
                    objModels.SetAvailableTeams(objListTeams);
                    List<Specialization> selectedSpecializations = null;
                    if (objModels.PostedSpecializations != null)
                    {
                        selectedSpecializations = new List<Specialization>();
                        if (objModels.PostedSpecializations.PrimarySpecializationIDs != null)
                        {
                            foreach (var item in objModels.PostedSpecializations.PrimarySpecializationIDs)
                            {
                                specialization = objTrainer.AvailableSpecializations.Where(m => m.SpecializationId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedSpecializations.Add(specialization);
                            }
                        }
                        objModels.SetSelectedTopthreeSpecializations(selectedSpecializations);
                    }
                    if (objModels.PostedSpecializations != null)
                    {
                        selectedSpecializations = new List<Specialization>();
                        if (objModels.PostedSpecializations.SecondarySpecializationIDs != null)
                        {
                            foreach (var item in objModels.PostedSpecializations.SecondarySpecializationIDs)
                            {
                                specialization = objTrainer.AvailableSpecializations.Where(m => m.SpecializationId == Convert.ToInt32(item)).FirstOrDefault();
                                selectedSpecializations.Add(specialization);
                            }
                        }
                        objModels.SetSelectedSecondarySpecializations(selectedSpecializations);
                    }
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        List<DDTeams> selecetdTeams = new List<DDTeams>();
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                DDTeams teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                selecetdTeams.Add(teams);
                            }
                        }
                        objModels.SetSelecetdTeams(selecetdTeams);
                    }
                    List<State> objListStates = TrainersBL.GetState();
                    ViewBag.lstStates = objListStates;
                    objModels.SetAvailableSpecializations(objTrainer.AvailableSpecializations as List<Specialization>);
                    /*Retrive Profilepic, TrainerPics and trainerVideo with path*/
                    if (objTrainer.TrainerImageUrl != null)
                    {
                        ViewBag.TrainerProfilePhoto = ConstantHelper.constprofilepicDir + objTrainer.TrainerImageUrl;
                    }

                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete trainer
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult DeleteTrainer(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: DeleteTrainer  controller");
                TrainersBL.DeleteTrainer(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return RedirectToAction(GetUrl(Request.UrlReferrer.AbsoluteUri));
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Delete Level Team from team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteLevelTeam(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: DeleteLevelTeam on reporting controller");
                var isdeleted = TeamTalkBL.DeleteLevelTeams(id);
                return Json(isdeleted, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteLevelTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        #endregion
        #region Users
        /// <summary>
        /// Action to view users view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Users()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: Users controller");
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = 0;
                /*Get Trainers from the database*/
                List<CreateUserVM> objListUsers = UseresBL.GetUsers();
                var objListTeams= TeamBL.GetALLTeams();
                if (objListTeams != null)
                {
                    objListTeams = objListTeams.OrderBy(tm => tm.TeamName).ToList();
                }
                ViewBag.Teams = objListTeams;
                HttpContext.Session[Message.PreviousUrl] = Message.UserUrl;
                return View(objListUsers);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Users end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Write  data on response for execl downloading 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="output"></param>
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            foreach (System.ComponentModel.PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (System.ComponentModel.PropertyDescriptor prop in props)
                {
                    output.Write(prop.GetValue(item));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }
        /// <summary>
        /// WriteHtmlTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="output"></param>

        public void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start : WriteHtmlTable()");
                using (StringWriter sw = new StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                    {

                        //  Create a form to contain the List
                        using (System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table())
                        {
                            System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
                            using (System.Web.UI.WebControls.TableRow row = new System.Web.UI.WebControls.TableRow())
                            {
                                foreach (System.ComponentModel.PropertyDescriptor prop in props)
                                {
                                    using (System.Web.UI.WebControls.TableHeaderCell hcell = new System.Web.UI.WebControls.TableHeaderCell())
                                    {
                                        hcell.Text = prop.Name;
                                        hcell.BackColor = System.Drawing.Color.Yellow;
                                        row.Cells.Add(hcell);
                                    }
                                }
                                table.Rows.Add(row);
                            }
                            //  add each of the data item to the table
                            foreach (T item in data)
                            {
                                using (var datarow = new System.Web.UI.WebControls.TableRow())
                                {
                                    foreach (System.ComponentModel.PropertyDescriptor prop in props)
                                    {
                                        using (System.Web.UI.WebControls.TableCell cell = new System.Web.UI.WebControls.TableCell())
                                        {

                                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                                            datarow.Cells.Add(cell);
                                        }
                                    }
                                    table.Rows.Add(datarow);
                                }
                            }
                            //  render the table into the htmlwriter
                            table.RenderControl(htw);
                        }
                        //  render the htmlwriter into the response
                        output.Write(sw.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);               
            }
            finally
            {
                traceLog.AppendLine("WriteHtmlTable end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }

        }

        /// <summary>
        /// Export User inata in Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportUserExcelData()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:ExportUserExcelData()");
                List<CreateUserVM> objListUsers = UseresBL.GetUsers();
                var userList = (from usr in objListUsers
                                select new
                                {
                                    UserName = usr.FullName,
                                    MTActive = usr.MTActiveStatus,
                                    PremiumMember = usr.PremiumMemberStatus,
                                    DOB = String.Format("{0:MM/dd/yyyy}", usr.DateOfBirth),
                                    Gender = usr.Gender,
                                    Zip = usr.ZipCode,
                                    EmailId = usr.EmailId,
                                    TeamName = usr.TeamName,
                                    TeamId = usr.UniqueTeamId,
                                    SignUpDate = String.Format("{0:G}", usr.CreatedDate)                                    
                                }).ToList();
                string[] columns = { "UserName", "MTActive", "PremiumMember", "DOB", "Gender", "Zip", "EmailId", "TeamName", "TeamId", "SignUpDate" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(userList, "", true, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "UserDetails.xlsx");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return new HttpNotFoundResult();
            }
            finally
            {
                traceLog.AppendLine("ExportUserExcelData end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Export Users CSV Data
        /// </summary>
        public void ExportUserCSVData()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: ExportCSVData user Data");
                IEnumerable<CreateUserVM> objListUsers = UseresBL.GetUsers();
                var userList = (from usr in objListUsers
                                select new
                                {
                                    UserName = usr.FullName,
                                    MTActive = usr.MTActiveStatus,
                                    PremiumMember = usr.PremiumMemberStatus,
                                    DOB = String.Format("{0:MM/dd/yyyy}", usr.DateOfBirth),
                                    Gender = usr.Gender,
                                    Zip = usr.ZipCode,
                                    EmailId = usr.EmailId,
                                    TeamName = usr.TeamName,
                                    TeamId = usr.UniqueTeamId,
                                    SignUpDate = String.Format("{0:G}", usr.CreatedDate)

                                }).ToList();
                using (StringWriter sw = new StringWriter())
                {
                    sw.WriteLine("\"User Name\",\"MTActive\",\"Premium Member\",\"DOB\",\"Gender\",\"Zip\",\"Email Id\",\"Team Name\",\"TeamId\",\"Sign-Up-Date\"");
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=Exported_Users.csv");
                    Response.ContentType = "text/csv";
                    foreach (var line in userList)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                                                   line.UserName,
                                                   line.MTActive,
                                                   line.PremiumMember,
                                                   line.DOB,
                                                   line.Gender,
                                                   line.Zip,
                                                   line.EmailId,
                                                   line.TeamName,
                                                   line.TeamId,
                                                   String.Format("{0:G}", line.SignUpDate)
                                                   ));
                    }

                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);               
            }
            finally
            {
                traceLog.AppendLine("ExportCSVData end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Export Trainer Data in Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportTrainerExcelData()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("start ExportTrainerExcelData() method");
                List<ViewTrainers> objListTrainers = TrainersBL.GetTrainers();
                var trainerdata = (from tr in objListTrainers
                                   select new
                                   {
                                       Name = tr.TrainerName,
                                       Specialization = tr.TopThreeSpecialization,
                                       TrainerType = tr.TrainerType,
                                       TeamName = tr.TeamName,
                                       TeamId = tr.UniqueTeamId,
                                       Teamcount = tr.TeamCount,
                                       TrainerId = tr.EnteredTrainerID,
                                       Location = tr.Address,
                                       Email=tr.Email,
                                       PhoneNumber=tr.PhoneNumber,
                                       PremiumMember = tr.PremiumMemberStatus 
                                   }).ToList();
                string[] columns = { "Name", "Specialization", "TrainerType", "TeamName", "TeamId", "Teamcount", "TrainerId", "Location", "Email", "PhoneNumber", "PremiumMember" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(trainerdata, "", true, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "TrainerDetails.xlsx");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return new HttpNotFoundResult();
            }
            finally
            {
                traceLog.AppendLine("ExportTrainerExcelData end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Export Trainers CSV Data
        /// </summary>
        public void ExportTrainerCSVData()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: ExportCSVData user Data");
                List<ViewTrainers> objListTrainer = TrainersBL.GetTrainers();
                using (StringWriter trainerExprtsw = new StringWriter())
                {
                    trainerExprtsw.WriteLine("\"Name\",\"Specialization\",\"Trainer Type\",\"Team Name\",\"TeamId\",\"Team Count\",\"TrainerID\",\"Address\",\"Email\",\"PhoneNumber\",\"PremiumMember\"");
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=Exported_Trainers.csv");
                    Response.ContentType = "text/csv";
                    foreach (var line in objListTrainer)
                    {
                        trainerExprtsw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                                                   line.TrainerName,
                                                   line.TopThreeSpecialization,
                                                   line.TrainerType,
                                                   line.TeamName,
                                                   line.UniqueTeamId,
                                                   line.TeamCount,
                                                   line.EnteredTrainerID,
                                                   line.Address,
                                                   line.Email,
                                                   line.PhoneNumber,
                                                   line.PremiumMemberStatus
                                                   ));
                    }
                    Response.Write(trainerExprtsw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
            }
            finally
            {
                traceLog.AppendLine("ExportCSVData end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to view create user page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateUser()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: CreateUser controller");
                return View();
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateUser end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to create user
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: CreateUser controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.CreatedBy = credentialId;
                    await UseresBL.SubmitUser(objModels);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateUser end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view update user page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateUser(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: UpdateUser  controller");
                UpdateUserVM objUser = UseresBL.GetUserById(id);
                return View(objUser);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateUser end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to update user
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public async Task<ActionResult> UpdateUser(UpdateUserVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                if (!objModels.IsUserChangePassword)
                {
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateUser controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.ModifiedBy = credentialId;
                    await UseresBL.UpdateUser(objModels);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateUser end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete user
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        public ActionResult DeleteUser(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: DeleteUser  controller");
                UseresBL.DeleteUser(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return RedirectToAction(GetUrl(Request.UrlReferrer.AbsoluteUri));
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteUser end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        #endregion
        #region Activities
        /// <summary>
        /// Action to view activities view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Activities()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: Activities controller");
                /*Get Trainers from the database*/
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = 0;
                List<ViewActivitiesVM> objListActivities = ActivityBL.GetActivities();
                HttpContext.Session[Message.PreviousUrl] = Message.ActivityUrl;
                return View(objListActivities);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Activities end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view create activity page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult CreateActivity()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: CreateActivity controller");
                GetActivityRequiredLists();
                return View();
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to update activity
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult CreateActivity(ActivityVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: CreateActivity controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                    objModels.ModifiedBy = credentialId;
                    // ActivityBL activityBL = new ActivityBL();
                    if (objModels.PicUrl != null)
                    {
                        objModels.Pic = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + objModels.PicUrl.FileName;
                        /*Code for save pic*/
                        string path = Server.MapPath("~") + "images\\profilepic\\" + objModels.Pic;
                        objModels.PicUrl.SaveAs(path);
                    }

                    if (objModels.VideoUrl != null)
                    {
                        objModels.Video = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + objModels.VideoUrl.FileName;
                        /*Code for save video*/
                        string path = Server.MapPath("~") + "videos\\" + objModels.Video;
                        objModels.VideoUrl.SaveAs(path);
                    }

                    ActivityBL.SubmitActivity(objModels);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    GetActivityRequiredLists();
                    List<City> objListCitye = TrainersBL.GetCities(objModels.State);
                    ViewBag.lstCities = objListCitye;
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view update activity page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateActivity(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: UpdateActivity controller");
                GetActivityRequiredLists();
                ActivityVM objActivityList = ActivityBL.GetActivityById(id);
                objActivityList.DateOfEvent = DateTime.SpecifyKind(objActivityList.DateOfEvent, DateTimeKind.Local);
                List<City> objListCitye = TrainersBL.GetCities(objActivityList.State);
                ViewBag.lstCities = objListCitye;
                return View(objActivityList);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to update activity
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult UpdateActivity(ActivityVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateActivity controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.ModifiedBy = credentialId;
                    if (objModels.PicUrl != null)
                    {
                        objModels.Pic = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + objModels.PicUrl.FileName;
                        /*Code for save pic*/
                        string path = Server.MapPath("~") + ConstantHelper.constProfileImage + objModels.Pic;
                        objModels.PicUrl.SaveAs(path);
                    }
                    if (objModels.VideoUrl != null)
                    {
                        objModels.Video = Guid.NewGuid().ToString() + ConstantHelper.constUnderscore + objModels.VideoUrl.FileName;
                        /*Code for save video*/
                        string path = Server.MapPath("~") + ConstantHelper.const_videos + objModels.Video;
                        objModels.VideoUrl.SaveAs(path);
                    }
                    ActivityBL.UpdateActivity(objModels);
                    TempData[ConstantHelper.constAlertMessage] = Message.UpdateMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    GetActivityRequiredLists();
                    List<City> objListCitye = TrainersBL.GetCities(objModels.State);
                    ViewBag.lstCities = objListCitye;
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to delete activity
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        public ActionResult DeleteActivity(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: DeleteActivity  controller");
                ActivityBL.DeleteActivity(id);
                TempData[ConstantHelper.constAlertMessage] = Message.DeleteMessage;
                return RedirectToAction(GetUrl(Request.UrlReferrer.AbsoluteUri));
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteActivity end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        #endregion
        #region Jquery actions
        /// <summary>
        /// Action to get challenge sub-type by challenge type
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetSubTypeByType(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetSubTypeByType");
                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeSubType(id);
                return Json(new SelectList(objListChallengeType.ToArray(), "ChallengeSubTypeId", "ChallengeSubType"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetSubTypeByType end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// Action to get challenge sub-type by challenge type
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetChallengeCategory(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetChallengeCategory");
                List<ChallengeCategory> objListChallengeType = ChallengesCommonBL.GetChallengeCategoryList(id);
                return Json(new SelectList(objListChallengeType.ToArray(), "ChallengeCategoryId", "ChallengeCategoryName"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetChallengeCategory end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to get trainer result for setting any challenge in to sponsor challenge queue
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetResults(string id /* drop down value */)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetResults");
                int trainerId = 0;
                int challengeId = 0;
                if (id.Contains(','))
                {
                    char[] split = { ',' };
                    string[] Ids = id.Split(split);
                    trainerId = Convert.ToInt32(Ids[0]);
                    challengeId = Convert.ToInt32(Ids[1]);
                }
                int credentialId = ChallengesBL.GetCredentialId(trainerId);
                List<UserResult> objListResults = TrainersBL.GetTrainerResult(credentialId, challengeId);
                return Json(new SelectList(objListResults.ToArray(), "Id", "Result"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetResults end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to get user result for setting any challenge in to cod queue
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetUserResults(string id /* drop down value */)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetUserResults");
                int trainerId = 0;
                int challengeId = 0;
                if (id.Contains(','))
                {
                    char[] split = { ',' };
                    string[] Ids = id.Split(split);
                    trainerId = Convert.ToInt32(Ids[0]);
                    challengeId = Convert.ToInt32(Ids[1]);
                }
                int credentialId = ProfileBL.GetUserCredentialId(trainerId);
                List<UserResult> objListResults = TrainersBL.GetTrainerResult(credentialId, challengeId);
                return Json(new SelectList(objListResults.ToArray(), "Id", "Result"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetUserResults end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to get cities by state code
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetCities(string id /* drop down value */)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetCities");
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                   || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    List<City> objListCitye = TrainersBL.GetCities(id);
                    return Json(new SelectList(objListCitye.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
                }
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetCities end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to get challenge type related value by challenge type id
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public JsonResult GetValueypeBySubType(int id)
        {
            StringBuilder traceLog = new StringBuilder();

            try
            {
                traceLog.AppendLine("Start: GetValueypeBySubType");
                ChallengeTypes objChallengeType = ChallengesCommonBL.GetChallengeVal(id);

                //string strValue = string.Format("Type {0} - # of {1} ({2}):", objChallengeType.ChallengeSubTypeId, objChallengeType.Unit, objChallengeType.MaxLimit); Modified By Irshad
                string strValue = string.Format("Type {0} - # of {1} :", objChallengeType.ChallengeSubTypeId, objChallengeType.Unit);

                if (objChallengeType != null)
                {
                    switch (objChallengeType.ChallengeSubTypeId)
                    {
                        case 3:
                        case 4:
                        case 10:
                        case 12:
                            strValue = string.Format("Type {0} - Amount of time:", objChallengeType.ChallengeSubTypeId);
                            break;
                        case 5:
                        case 6:
                            strValue = string.Format("Type {0} - ", objChallengeType.ChallengeSubTypeId);
                            break;
                    }
                }
                var result = new { Result = strValue, isMoreThanOne = objChallengeType.IsExerciseMoreThanOne, resultUnit = objChallengeType.ResultUnit, variableUnit = objChallengeType.Unit, MaxLimit = objChallengeType.MaxLimit };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetValueypeBySubType end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// Action to get exercise for autocomplete
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetExercise(string term)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetExercise");
                List<Exercise> lstExercise = new List<Exercise>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                     || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    lstExercise = ChallengesBL.GetExerciseIndex(term.Trim());
                    return Json(lstExercise, JsonRequestBehavior.AllowGet);
                }
                return Json(lstExercise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetExercise end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to get exercise for autocomplete
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetAllExercise()
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetAllExercise");
                List<Exercise> lstExercise = new List<Exercise>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                     || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    lstExercise = ChallengesCommonBL.GetAllExerciseIndex();
                }
                return Json(lstExercise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetAllExercise end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to get exercise for autocomplete
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetSerachExercise(string term, string bodyPart, string equipment, string exerciseType)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetExercise");
                List<Exercise> lstExercise = new List<Exercise>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                      || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                       || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    lstExercise = ChallengesCommonBL.GetSearchExerciseIndex(term, bodyPart, equipment, exerciseType);
                    return Json(lstExercise, JsonRequestBehavior.AllowGet);
                }
                return Json(lstExercise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetExercise end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to check user email from database
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetEmailRecord(string input)
        {
            StringBuilder traceLog = new StringBuilder();
            string result = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetEmailRecord");
                string email = string.Empty;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                       || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                        || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    email = UseresBL.GetEmail(input.Trim());
                    if (!string.IsNullOrEmpty(email))
                    {
                        result = ConstantHelper.constAvailable;
                    }
                    else
                    {
                        result = ConstantHelper.constNotAvailable;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(ConstantHelper.constNotAvailable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("GetEmailRecord end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to check trainer email from database
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetTrainerEmail(string input)
        {
            StringBuilder traceLog = new StringBuilder();
            string result = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetTrainerEmail" + input);
                string email = string.Empty;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                        || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                         || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    email = UseresBL.GetTrainerEmail(input.Trim());
                    if (!string.IsNullOrEmpty(email))
                    {
                        result = ConstantHelper.constAvailable;
                    }
                    else
                    {
                        result = ConstantHelper.constNotAvailable;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(ConstantHelper.constNotAvailable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("GetTrainerEmail end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Validate Team Email Address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTeamEmail(string input)
        {
            StringBuilder traceLog = new StringBuilder();
            string result = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetTeamEmail");
                string email = string.Empty;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                         || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    email = TeamBL.GetTeamEmail(input.Trim());
                    if (!string.IsNullOrEmpty(email))
                    {
                        result = ConstantHelper.constAvailable;
                    }
                    else
                    {
                        result = ConstantHelper.constNotAvailable;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(ConstantHelper.constNotAvailable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("GetTeamEmail end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to check user name from database
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetUserRecord(string input)
        {
            StringBuilder traceLog = new StringBuilder();
            string result = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetUserRecord-Request" + input);
                string email = string.Empty;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                           || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    email = UseresBL.GetUser(input.Trim());
                    if (!string.IsNullOrEmpty(email))
                    {
                        result = ConstantHelper.constAvailable;
                    }
                    else
                    {
                        result = ConstantHelper.constNotAvailable;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("GetUserRecord end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to check team name from database
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetTeamRecord(string input)
        {
            StringBuilder traceLog = new StringBuilder();
            string result = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetTeamRecord-Request" + input);
                string email = string.Empty;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                           || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    email = UseresBL.GetTeam(input.Trim());
                    if (!string.IsNullOrEmpty(email))
                    {
                        result = ConstantHelper.constAvailable;
                    }
                    else
                    {
                        result = ConstantHelper.constNotAvailable;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("GetTeamRecord end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        #endregion
        #region AjaxCall
        /// <summary>
        /// Action to get users
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetUsers(string term)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetUsers-Request" + term);
                List<string> lstUsers = new List<string>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    lstUsers = ProfileApiBL.GetUsers(term.Trim());
                    return Json(lstUsers, JsonRequestBehavior.AllowGet);
                }
                return Json(lstUsers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetUsers end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to get fraction
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public JsonResult GetFraction(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetFraction action");
                /*ViewBag for Fraction detai dropdown*/
                List<SelectListItem> objSelectList;
                if (id.Equals(1))
                {
                    objSelectList = new List<SelectListItem>()
                    {
                        new SelectListItem()
                        {
                            Text = Message.OneByTwo, Value = Message.OneByTwo
                        },
                    };
                }
                else if (id.Equals(2))
                {
                    objSelectList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = Message.OneByThree, Value = Message.OneByThree
                    },
                    new SelectListItem()
                    {
                        Text = Message.TwoByThree, Value = Message.TwoByThree
                    }
                };
                }
                else
                {
                    objSelectList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = Message.OneByFour, Value = Message.OneByFour
                    },
                    new SelectListItem()
                    {
                        Text = Message.TwoByFour, Value = Message.TwoByFour
                    },
                    new SelectListItem()
                    {
                        Text = Message.ThreeByFour, Value = Message.ThreeByFour
                    }
                };
                }

                return Json(new SelectList(objSelectList.ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetFraction end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to get challenge by trainer id
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ChallengesByTrainerId(int id)
        {
            StringBuilder traceLog = null;
            List<ViewChallenes> objListChallenge = new List<ViewChallenes>();
            ChallengesData challenegdata = new ChallengesData();
            List<TrainerViewVM> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengesByTrainerId controller");
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = id;
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }

                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }

                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                if (id > 0)
                {
                    int trainerCredentialId = ChallengesBL.GetCredentialId(id);
                    id = trainerCredentialId;
                }
                HttpContext.Session[Message.PreviousUrl] = Message.ChallengeUrl;
                objListChallenge = ChallengesBL.GetTrainerChallenges(id, Message.UserTypeAdmin);
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                challenegdata.SetChallengesViewData(objListChallenge);
                return PartialView("_Challenges", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("ChallengesByTrainerId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        ///  Workouts By TrainerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult WorkoutsByTrainerId(int id)
        {
            StringBuilder traceLog = null;
            List<ViewChallenes> objListChallenge = new List<ViewChallenes>();
            ChallengesData challenegdata = new ChallengesData();
            List<TrainerViewVM> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ChallengesByTrainerId controller");
                HttpContext.Session[ConstantHelper.constSelectedTrainerId] = id;
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }

                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }

                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                if (id > 0)
                {

                    int trainerCredentialId = ChallengesBL.GetCredentialId(id);
                    id = trainerCredentialId;
                }
                HttpContext.Session[Message.PreviousUrl] = Message.ChallengeUrl;
                objListChallenge = ChallengesBL.GetTrainerWorkoutChallenges(id, Message.UserTypeAdmin);
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                challenegdata.SetChallengesViewData(objListChallenge);
                return PartialView("_WorkoutChallenges", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("ChallengesByTrainerId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Users By TeamId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult UsersByTeamId(int id)
        {
            StringBuilder traceLog = null;
            List<CreateUserVM> objListUsers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UsersByTeamId controller:-TeamId-" + id);
                HttpContext.Session[ConstantHelper.constSelectedTeamId] = id;
                if (id > 0)
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.UserUrl;
                }
                else
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.UserUrl;
                }
                objListUsers = UseresBL.GetUsers(id);
                ViewBag.Teams = TeamBL.GetAllTeamName();
                return PartialView("_Users", objListUsers);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("UsersByTeamId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Search Challenges based on seach cateria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchChallenges(string id)
        {
            StringBuilder traceLog = null;
            int trainersId = -1;
            string serach = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] seachdata = id.Split('~');
                trainersId = Convert.ToInt32(seachdata[0]);
                serach = seachdata[1];
            }
            List<ViewChallenes> objListChallenge = new List<ViewChallenes>();
            ChallengesData challenegdata = new ChallengesData();
            List<TrainerViewVM> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchChallenges controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                if (trainersId > 0)
                {
                    int trainerCredentialId = ChallengesBL.GetCredentialId(trainersId);
                    trainersId = trainerCredentialId;
                }

                HttpContext.Session[Message.PreviousUrl] = Message.ChallengeUrl;
                objListChallenge = ChallengesBL.GetSeachChallenges(trainersId, Message.UserTypeAdmin, serach);
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                challenegdata.SetChallengesViewData(objListChallenge);
                return PartialView("_Challenges", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Search Workout Challenges
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchWorkoutChallenges(string id)
        {
            StringBuilder traceLog = null;
            int trainersId = -1;
            string serach = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] seachdata = id.Split('~');
                trainersId = Convert.ToInt32(seachdata[0]);
                serach = seachdata[1];
            }
            List<ViewChallenes> objListChallenge = new List<ViewChallenes>();
            ChallengesData challenegdata = new ChallengesData();
            List<TrainerViewVM> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchChallenges controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }
                if (trainersId > 0)
                {
                    int trainerCredentialId = ChallengesBL.GetCredentialId(trainersId);
                    trainersId = trainerCredentialId;
                }
                HttpContext.Session[Message.PreviousUrl] = ConstantHelper.constWorkouts;
                objListChallenge = ChallengesBL.GetSeachWorkoutChallenges(trainersId, Message.UserTypeAdmin, serach);
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = ConstantHelper.constNoTrainer });
                ViewBag.Trainers = objListTrainers;
                challenegdata.SetChallengesViewData(objListChallenge);
                return PartialView("_WorkoutChallenges", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Search Program by Program Name and difficult level
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchProgram(string id)
        {
            StringBuilder traceLog = null;
            int trainersId = -1;
            string serach = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] seachdata = id.Split('~');
                trainersId = Convert.ToInt32(seachdata[0]);
                serach = seachdata[1];
            }
            ChallengesData challenegdata = new ChallengesData();
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchProgram controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }

                if (trainersId > 0)
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.ProgramUrl;
                    int trainerCredentialId = ChallengesBL.GetCredentialId(trainersId);
                    trainersId = trainerCredentialId;
                }
                else
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.ProgramUrl;
                }

                // HttpContext.Session["SelectedTrainerId"] = 0;
                ///Get Challenges from the database
                objListChallenge = ChallengesBL.GetTrainerPrograms(trainersId, Message.UserTypeAdmin, serach);
                challenegdata.SetChallengesViewData(objListChallenge);
                return PartialView("_Programs", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Search User by User Name and Team Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchUser(string id)
        {
            StringBuilder traceLog = null;
            int teamId = 0;
            string serach = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                string[] seachdata = id.Split('~');
                teamId = Convert.ToInt32(seachdata[0]);
                serach = seachdata[1];
            }
            List<CreateUserVM> objListUsers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchProgram controller");
                if (teamId > 0)
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.UserUrl;
                }
                else
                {
                    HttpContext.Session[Message.PreviousUrl] = Message.UserUrl;
                }
                ///Get Challenges from the database
                objListUsers = UseresBL.GetUsers(teamId, serach);
                return PartialView("_Users", objListUsers);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchTeam(string id)
        {
            StringBuilder traceLog = null;

            List<ViewTeams> objListTeams = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchTeam controller");
                objListTeams = TeamBL.GetALLTeams(id);
               
                HttpContext.Session[Message.PreviousUrl] = Message.TeamUrl;
                return PartialView("_Teams", objListTeams);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchTrainer(string id)
        {
            StringBuilder traceLog = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SearchTrainer controller");
                List<ViewTrainers> objListChallenge = TrainersBL.GetTrainers(id);
                HttpContext.Session[Message.PreviousUrl] = Message.TrainerUrl;
                return PartialView("_Trainers", objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }



        /// <summary>
        /// Get Programs By TrainerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProgramsByTrainerId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start :ProgramsByTrainerId() with request-" + id);
                ChallengesData challenegdata = new ChallengesData();
                List<ViewChallenes> objListChallenge = new List<ViewChallenes>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    HttpContext.Session[ConstantHelper.constSelectedTrainerId] = id;
                    if (Request.QueryString[ConstantHelper.constpage] != null)
                    {
                        int pagenumber = 0;
                        challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                        TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                    }
                    else if (TempData[ConstantHelper.constpage] != null)
                    {
                        var savedpage = TempData[ConstantHelper.constpage];
                        if (savedpage != null && savedpage.GetType() == typeof(int))
                            challenegdata.CurrentPageIndex = (int)savedpage;
                    }

                    if (Request.QueryString[ConstantHelper.constsort] != null)
                    {
                        challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                        TempData[ConstantHelper.constsort] = challenegdata.SortField;
                    }
                    else if (TempData[ConstantHelper.constsort] != null)
                    {
                        challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                    }
                    else
                    {
                        challenegdata.SortField = ConstantHelper.constChallengeName;
                    }

                    if (Request.QueryString[ConstantHelper.constsortdir] != null)
                    {
                        challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                        TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                    }
                    else if (TempData[ConstantHelper.constsortdir] != null)
                    {
                        challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                    }
                    else
                    {
                        challenegdata.SortDirection = ConstantHelper.constASC;
                    }
                    if (id > 0)
                    {
                        int trainerCredentialId = ChallengesBL.GetCredentialId(id);
                        id = trainerCredentialId;
                    }
                    objListChallenge = ChallengesBL.GetTrainerPrograms(id, Message.UserTypeAdmin);

                    challenegdata.SetChallengesViewData(objListChallenge);
                    return PartialView("_Programs", challenegdata);
                }
                return PartialView("_Programs", challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Error();
            }
            finally
            {
                traceLog.AppendLine("ProgramsByTrainerId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Free form Challenges By TrainerId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FreeformChallengesByTrainerId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:FreeformChallengesByTrainerId() with request-" + id);
                List<FreeFormChallengeVM> objListChallenge = new List<FreeFormChallengeVM>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    if (id > 0)
                    {
                        int trainerCredentialId = ChallengesBL.GetCredentialId(id);
                        id = trainerCredentialId;
                    }
                    objListChallenge = FreeFormChallengeBL.GetFreeFormTrainerChallenges(id, Message.UserTypeAdmin);
                    return PartialView("FreeFormChallenges", objListChallenge);
                }
                return PartialView("FreeFormChallenges", objListChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Error();
            }
            finally
            {
                traceLog.AppendLine("ProgramsByTrainerId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to view error page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        public ActionResult Error()
        {
            ViewBag.Title = "Error Page";
            return View();
        }

        /// <summary>
        /// method to get activity list
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        private void GetActivityRequiredLists()
        {
            List<TrainerViewVM> objListTrainers = TrainersBL.GetAllTrainers();
            ViewBag.Trainers = objListTrainers;
            List<State> objListStates = TrainersBL.GetState();
            ViewBag.lstStates = objListStates;
        }

        /// <summary>
        /// method to url for previous page
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        private string GetUrl()
        {
            string strUrl = string.Empty;
            string url = string.Empty;
            strUrl = Convert.ToString(HttpContext.Session[Message.PreviousUrl]);
            if (strUrl.Contains('/'))
            {
                int lastIndex = strUrl.LastIndexOf('/');
                url = strUrl.Substring(lastIndex + 1);
            }

            return url;
        }

        /// <summary>
        /// method to url for previous page
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        private string GetUrl(string strUrl)
        {
            string url = string.Empty;
            if (strUrl.Contains('/'))
            {
                int lastIndex = strUrl.LastIndexOf('/');
                url = strUrl.Substring(lastIndex + 1);
            }

            return url;
        }
        /// <summary>
        /// Create FreeForm Challenge
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        [HttpGet]
        public ActionResult CreateFreeFormChallenge(int? id)
        {
            StringBuilder traceLog = null;
            FreeFormChallengeVM objAdminChallenge = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constuser)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin Free Form Challenge controller");
                objAdminChallenge = new FreeFormChallengeVM();
                if (Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == Message.UserTypeAdmin)
                {
                    objAdminChallenge.IsAdminUser = true;
                }
                if (id != null)
                {
                    int Id = (int)id;
                    List<SelectListItem> objSelectList = new List<SelectListItem>();
                    objAdminChallenge = FreeFormChallengeBL.GetFreeFormChallangeById(Id, ref objSelectList);
                }
                objListTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objListTrainers;
                return View(objAdminChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View(objAdminChallenge);
            }
            finally
            {
                traceLog.AppendLine("create admin Free Form Challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Submit free form challenges
        /// </summary>
        /// <param name="freeformModel"></param>
        /// <param name="submit"></param>
        /// <returns></returns
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        [HttpPost]
        public ActionResult CreateFreeFormChallenge(FreeFormChallengeVM freeformModel, string submit)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constuser)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == Message.UserTypeAdmin)
                {
                    freeformModel.IsAdminUser = true;
                }
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: create admin Free Form Challenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                    int trainerId = 0;
                    if (freeformModel.TrainerId != null)
                    {
                        int trainerUserId = (int)freeformModel.TrainerId;
                        trainerId = (int)TrainersBL.GetTrainerId(trainerUserId);
                    }
                    FreeFormChallengeBL.SubmitFreeFormChallenge(freeformModel, credentialId, trainerId, submit);
                    TempData[ConstantHelper.constAlertMessage] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    List<ViewTrainers> objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;
                    return View(freeformModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("Create Free Form Challenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Action to view update challenge view page
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpGet]
        public ActionResult UpdateFreeFormChallenge(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constuser)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: UpdateChallenge  controller");
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                FreeFormChallengeVM objChallenge = FreeFormChallengeBL.GetFreeFormChallangeById(id, ref objSelectList);
                if (Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == Message.UserTypeAdmin)
                {
                    objChallenge.IsAdminUser = true;
                }
                /*Get all trainers*/
                List<ViewTrainers> objAllTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objAllTrainers;
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Action to update challenge 
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh       
        /// </devdoc>
        [HttpPost]
        public ActionResult UpdateFreeFormChallenge(FreeFormChallengeVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constuser)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateChallenge controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    FreeFormChallengeBL.UpdateFreeFormChallenges(objModels, credentialId);
                    TempData[ConstantHelper.constAlertMessage] = Message.UpdateMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {   /*Get Trainers of that chalenge who have completed the challenge from the database*/
                    List<TrainerViewVM> objListTrainers = ChallengesCommonBL.GetTrainersByChallengeId(objModels.ChallengeId);
                    ViewBag.Trainers = objListTrainers;
                    /*Get all trainers*/
                    List<ViewTrainers> objAllTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objAllTrainers;
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// create team
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 05/16/2016
        /// </devdoc>[HttpPost]
        [HttpGet]
        public ActionResult CreateTeam()
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: create Tream controller");
                CreateTeamVM objtrainer = new CreateTeamVM();
                List<State> objListStates = TrainersBL.GetState();
                objtrainer.GuidRecordId = Guid.NewGuid().ToString();
                List<TrendingCategory> objWorkoutTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);

                objtrainer.AvailableWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt=>wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                objtrainer.AvailableSecondaryWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                List<TrendingCategory> objProgramTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                objtrainer.AvailableProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                objtrainer.AvailableSecondaryProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                List<TrendingCategory> objFitnessTestTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constEnduranceChallengeType1);
                objtrainer.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                objtrainer.AvailableSecondaryFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                ViewBag.lstStates = objListStates;
                objtrainer.PrimaryCommissionRate = 0.0M;
                objtrainer.Level1CommissionRate = 0.0M;
                objtrainer.Level2CommissionRate = 0.0M;
                return View(objtrainer);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create Teams end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// submit team into databae
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 03/18/2016
        /// </devdoc>[HttpPost]
        [HttpPost]
        public ActionResult CreateTeam(CreateTeamVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            if (!string.IsNullOrEmpty(objModels.PhoneNumber))
            {
                var isPhoneAllzero = objModels.PhoneNumber.All(c => c == '0');
                if (isPhoneAllzero)
                {
                    ModelState.AddModelError("PhoneNumber", "Entered phone number is not valid.");
                }
                else
                {
                    ModelState.Remove("PhoneNumber");
                }
            }
            if (!string.IsNullOrEmpty(objModels.CropNutrition1ImageRowData))
            {
                ModelState.Remove("Nutrition1ImageUrl");
            }
            if (!string.IsNullOrEmpty(objModels.CropNutrition2ImageRowData))
            {
                ModelState.Remove("Nutrition2ImageUrl");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: CreateTeam controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                    objModels.CreatedBy = credentialId;

                    if (!string.IsNullOrEmpty(objModels.CropProfileImageRowData))
                    {
                        objModels.ProfileImageUrl = SaveUploadImage(objModels.CropProfileImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropPremiumImageRowData))
                    {
                        objModels.PremiumImageUrl = SaveUploadImage(objModels.CropPremiumImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropNutrition1ImageRowData))
                    {
                        objModels.Nutrition1ImageUrl = SaveUploadImage(objModels.CropNutrition1ImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropNutrition2ImageRowData))
                    {
                        objModels.Nutrition2ImageUrl = SaveUploadImage(objModels.CropNutrition2ImageRowData);
                    }

                    TeamBL.SubmitTeam(objModels);
                    TempData[ConstantHelper.constAlertMessage] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    List<State> objListStates = TrainersBL.GetState();
                    ViewBag.lstStates = objListStates;
                    List<City> objListCitye = TrainersBL.GetCities(objModels.State);
                    ViewBag.lstCities = objListCitye;
                    ViewBag.TeamProfileImageUrl = objModels.CropProfileImageRowData;
                    ViewBag.TeamPremiumImageUrl = objModels.CropPremiumImageRowData;
                    ViewBag.Nutrition1ImageUrl = objModels.CropNutrition1ImageRowData;
                    ViewBag.Nutrition2ImageUrl = objModels.CropNutrition2ImageRowData;
                    List<TrendingCategory> objWorkoutTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);
                   // objModels.AvailableWorkoutTrendingCategory = objWorkoutTrendingList;
                    objModels.AvailableWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    List<TrendingCategory> objProgramTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                    // objModels.AvailableProgramTrendingCategory = objProgramTrendingList;
                    objModels.AvailableProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    List<TrendingCategory> objFitnessTestTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constEnduranceChallengeType1);
                   // objModels.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList;
                    objModels.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    if (objModels.PostedFitnessTestTrendingCategory != null)
                    {
                        if (objModels.PostedFitnessTestTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdFitnessTestTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedFitnessTestTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingFitTest = objFitnessTestTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdFitnessTestTrendingCategory.Add(objtrendingFitTest);
                            }
                        }
                    }
                    if (objModels.PostedProgramTrendingCategory != null)
                    {
                        if (objModels.PostedProgramTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdProgramTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedProgramTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingProgram = objProgramTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdProgramTrendingCategory.Add(objtrendingProgram);
                            }
                        }
                    }
                    if (objModels.PostedWorkoutTrendingCategory != null)
                    {
                        if (objModels.PostedWorkoutTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdWorkoutTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedWorkoutTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingworkout = objWorkoutTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdWorkoutTrendingCategory.Add(objtrendingworkout);
                            }
                        }
                    }

                    if (objModels.PostedSecondaryFitnessTestTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryFitnessTestTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingFitTest = objFitnessTestTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryFitnessTestTrendingCategory.Add(objtrendingFitTest);
                            }
                        }
                    }
                    if (objModels.PostedSecondaryProgramTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryProgramTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryProgramTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedProgramTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingProgram = objProgramTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryProgramTrendingCategory.Add(objtrendingProgram);
                            }
                        }
                    }
                    if (objModels.PostedSecondaryWorkoutTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryWorkoutTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingworkout = objWorkoutTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryWorkoutTrendingCategory.Add(objtrendingworkout);
                            }
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        ///  Get the team details based on teamId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 03/18/2016
        /// </devdoc>[HttpPost]
        [HttpGet]
        public ActionResult UpdateTeam(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: UpdateTeam  controller-TeamId-" + id);
                CreateTeamVM objTeam = TeamBL.GetTeamById(id);
                List<State> objListStates = TrainersBL.GetState();
                ViewBag.lstStates = objListStates;
                List<City> objListCitye = TrainersBL.GetCities(objTeam.State);
                ViewBag.lstCities = objListCitye;
                if (objTeam.ProfileImageUrl != null)
                {
                    ViewBag.TeamProfileImageUrl = ConstantHelper.constprofilepicDir + objTeam.ProfileImageUrl;
                }
                if (objTeam.PremiumImageUrl != null)
                {
                    ViewBag.TeamPremiumImageUrl = ConstantHelper.constprofilepicDir + objTeam.PremiumImageUrl;
                }
                if (objTeam.Nutrition1ImageUrl != null)
                {
                    ViewBag.TeamNutrition1ImageUrl = ConstantHelper.constprofilepicDir + objTeam.Nutrition1ImageUrl;
                }
                if (objTeam.Nutrition2ImageUrl != null)
                {
                    ViewBag.TeamNutrition2ImageUrl = ConstantHelper.constprofilepicDir + objTeam.Nutrition2ImageUrl;
                }

                return View(objTeam);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("UpdateTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Update team detailse based on team Id
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        [HttpPost]
        public ActionResult UpdateTeam(CreateTeamVM objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (!objModels.IsChangePassword)
                {
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (!string.IsNullOrEmpty(objModels.CropNutrition1ImageRowData))
                {
                    ModelState.Remove("Nutrition1ImageUrl");
                }
                if (!string.IsNullOrEmpty(objModels.CropNutrition2ImageRowData))
                {
                    ModelState.Remove("Nutrition2ImageUrl");
                }
                if (!string.IsNullOrEmpty(objModels.PhoneNumber))
                {
                    var isPhoneAllzero = objModels.PhoneNumber.All(c => c == '0');
                    if (isPhoneAllzero)
                    {
                        ModelState.AddModelError("PhoneNumber", "Entered phone number is not valid.");
                    }
                    else
                    {
                        ModelState.Remove("PhoneNumber");
                    }
                }
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateTrainer controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                    objModels.ModifiedBy = credentialId;
                    if (!string.IsNullOrEmpty(objModels.CropProfileImageRowData))
                    {
                        objModels.ProfileImageUrl = SaveUploadImage(objModels.CropProfileImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropPremiumImageRowData))
                    {
                        objModels.PremiumImageUrl = SaveUploadImage(objModels.CropPremiumImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropNutrition1ImageRowData))
                    {
                        objModels.Nutrition1ImageUrl = SaveUploadImage(objModels.CropNutrition1ImageRowData);
                    }
                    if (!string.IsNullOrEmpty(objModels.CropNutrition2ImageRowData))
                    {
                        objModels.Nutrition2ImageUrl = SaveUploadImage(objModels.CropNutrition2ImageRowData);
                    }
                    TeamBL.UpdateTeam(objModels);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    CreateTeamVM objTrainer = TeamBL.GetTeamById(objModels.TeamId);
                    List<City> objListCitye = TrainersBL.GetCities(objTrainer.State);
                    ViewBag.lstCities = objListCitye;
                    List<State> objListStates = TrainersBL.GetState();
                    ViewBag.lstStates = objListStates;
                    /*Retrive Profilepic, TrainerPics and trainerVideo with path*/
                    if (objTrainer.ProfileImageUrl != null)
                    {
                        ViewBag.TeamProfileImageUrl = "../../images/profilepic/" + objTrainer.ProfileImageUrl;
                    }
                    if (objTrainer.PremiumImageUrl != null)
                    {
                        ViewBag.TeamPremiumImageUrl = "../../images/profilepic/" + objTrainer.PremiumImageUrl;
                    }
                    if (objTrainer.Nutrition1ImageUrl != null)
                    {
                        ViewBag.Nutrition1ImageUrl = "../../images/profilepic/" + objTrainer.Nutrition1ImageUrl;
                    }
                    if (objTrainer.Nutrition2ImageUrl != null)
                    {
                        ViewBag.Nutrition2ImageUrl = "../../images/profilepic/" + objTrainer.Nutrition2ImageUrl;
                    }
                    List<TrendingCategory> objWorkoutTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);
                    // objModels.AvailableWorkoutTrendingCategory = objWorkoutTrendingList;
                    objModels.AvailableWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    List<TrendingCategory> objProgramTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                    // objModels.AvailableProgramTrendingCategory = objProgramTrendingList;
                    objModels.AvailableProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    List<TrendingCategory> objFitnessTestTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constEnduranceChallengeType1);
                    // objModels.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList;
                    objModels.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();

                    if (objModels.PostedFitnessTestTrendingCategory != null)
                    {
                        if (objModels.PostedFitnessTestTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdFitnessTestTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedFitnessTestTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingFitTest = objFitnessTestTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdFitnessTestTrendingCategory.Add(objtrendingFitTest);
                            }
                        }
                    }
                    if (objModels.PostedProgramTrendingCategory != null)
                    {
                        if (objModels.PostedProgramTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdProgramTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedProgramTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingProgram = objProgramTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdProgramTrendingCategory.Add(objtrendingProgram);
                            }
                        }
                    }
                    if (objModels.PostedWorkoutTrendingCategory != null)
                    {
                        if (objModels.PostedWorkoutTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdWorkoutTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedWorkoutTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingworkout = objWorkoutTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdWorkoutTrendingCategory.Add(objtrendingworkout);
                            }
                        }
                    }

                    if (objModels.PostedSecondaryFitnessTestTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryFitnessTestTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingFitTest = objFitnessTestTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryFitnessTestTrendingCategory.Add(objtrendingFitTest);
                            }
                        }
                    }
                    if (objModels.PostedSecondaryProgramTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryProgramTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryProgramTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedProgramTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingProgram = objProgramTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryProgramTrendingCategory.Add(objtrendingProgram);
                            }
                        }
                    }
                    if (objModels.PostedSecondaryWorkoutTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryWorkoutTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID)
                            {
                                var objtrendingworkout = objWorkoutTrendingList.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                objModels.SelecetdSecondaryWorkoutTrendingCategory.Add(objtrendingworkout);
                            }
                        }
                    }



                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Save Uploaded Image on local server folder
        /// </summary>
        /// <param name="cropImageRowData"></param>
        /// <returns></returns>
        [NonAction]
        private string SaveUploadImage(string cropImageRowData)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: SaveUploadImage() for save uploaded image in server image profile folder");
                string root = Server.MapPath(ConstantHelper.constProfileImagePath);
                string base64data = string.Empty;
                string savedpicName = string.Empty;
                string extension = ".jpg";
                char[] split = { ',' };
                if (!string.IsNullOrEmpty(cropImageRowData))
                {
                    string[] imagextension = cropImageRowData.Split(split);
                    if (imagextension.Length > 1)
                    {
                        base64data = imagextension[1];
                    }
                    var bytes = Convert.FromBase64String(base64data);

                    if (bytes.Length > 0)
                    {
                        savedpicName = Guid.NewGuid().ToString() + "_Resize" + extension;
                    }
                    using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + savedpicName, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    traceLog.AppendLine("Start: save uploaded image in database" + savedpicName);
                }
                return savedpicName;
            }
            finally
            {
                traceLog.AppendLine("SaveUploadImage end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Delate team based on team and reset user /trainer to default team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc> 
        [HttpGet]
        public ActionResult DeleteTeam(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog.AppendLine("Start: DeleteTeam  controller");
                TeamBL.DeleteTeam(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return RedirectToAction(GetUrl(Request.UrlReferrer.AbsoluteUri));
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Add FFExecise based on execise ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddFFExecise(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    traceLog.AppendLine("Start: AddFFExecise() for Add Free form Execise  controller");
                    AddFFExeciseVM objAddFFExeciseVM = new AddFFExeciseVM()
                    {
                        ExeciseCountID = id,
                        IsNewAddedExercise = ConstantHelper.constSingleOne
                    };
                    return PartialView("_AddFFExecise", objAddFFExeciseVM);
                }
                return PartialView("_AddFFExecise");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("AddFFExecise end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Add Free form Execise Set
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFFExeciseSet(AddExeciseSetVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
            {
                try
                {
                    traceLog.AppendLine("Start: AddFFExeciseSet method() for Add Free form Execise set in Reporting  controller");
                    return PartialView("_AddExeciseSet", model);
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("AddFFExeciseSet end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_AddExeciseSet");
        }
        /// <summary>
        /// Get FF Execise Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFFExeciseDetail(GetFFExeciseVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
            {
                try
                {
                    traceLog.AppendLine("Start: GetFFExeciseDetail method() for Fet Free form Execise set in Reporting  controller");
                    return PartialView("_GetFFExeciseDetail", model);

                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("GetFFExeciseDetail end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_GetFFExeciseDetail");
        }
        /// <summary>
        /// Get FF ExeciseSet Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFFExeciseSetDetail(GetExeciseSetVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetFFExeciseSetDetail method() for get Free form Execise set in Reporting  controller");
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    return PartialView("_GetFFExeciseSetDetail", model);
                }
                return PartialView("_GetFFExeciseSetDetail");
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetFFExeciseSetDetail end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected Exercises based on indexes
        /// </summary>
        /// <param name="searchexeciseIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSelectedExercises(SearchExeciseVM searchexeciseIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetSelectedExercise");
                List<Exercise> lstExercise = new List<Exercise>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.consttrainer)
                {
                    lstExercise = ChallengesBL.GetSelecetdIndexExercises(searchexeciseIndex);
                    return Json(lstExercise, JsonRequestBehavior.AllowGet);
                }
                return Json(lstExercise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedExercise end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected Onborading Videos Exercises
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSelectedOnboradingVideExercises(string searchTerm)
        {
            StringBuilder traceLog = new StringBuilder();
            List<Exercise> lstExercise = null;
            try
            {
                traceLog.AppendLine("Start: GetSelectedOnboradingVideExercises");
                lstExercise = new List<Exercise>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
              || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    lstExercise = ChallengesBL.GetSelecetdIndexExercises(searchTerm);
                    return Json(lstExercise, JsonRequestBehavior.AllowGet);
                }
                return Json(lstExercise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedOnboradingVideExercises end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        #endregion
        #region Program
        /// <summary>
        /// Get all Programs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Programs()
        {
            StringBuilder traceLog = null;
            ChallengesData challenegdata = new ChallengesData();
            List<TrainerViewVM> objListTrainers = null;
            List<ViewChallenes> objListChallenge = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: Challenges controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    challenegdata.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = challenegdata.CurrentPageIndex;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        challenegdata.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = challenegdata.SortField;
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    challenegdata.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    challenegdata.SortField = ConstantHelper.constChallengeName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = challenegdata.SortDirection;
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    challenegdata.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    challenegdata.SortDirection = ConstantHelper.constASC;
                }

                // HttpContext.Session["SelectedTrainerId"] = 0;
                int selectedTrainerId = Convert.ToInt32(HttpContext.Session[ConstantHelper.constSelectedTrainerId]);
                challenegdata.SelectedTrainerId = 0;
                int trainerCredentialId = 0;
                if (selectedTrainerId > 0)
                {
                    trainerCredentialId = ChallengesBL.GetCredentialId(selectedTrainerId);
                }
                else
                {
                    trainerCredentialId = selectedTrainerId;
                }
                ///Get Challenges from the database
                objListChallenge = ChallengesBL.GetTrainerPrograms(trainerCredentialId, Message.UserTypeAdmin);
                objListTrainers = TrainersBL.GetAllTrainers();
                objListTrainers.Insert(0, new TrainerViewVM() { TrainerId = 0, TrainerName = "No Trainer" });
                ViewBag.Trainers = objListTrainers;
                HttpContext.Session[Message.PreviousUrl] = Message.ProgramUrl;
                challenegdata.SelectedTrainerId = selectedTrainerId;
                challenegdata.SetChallengesViewData(objListChallenge);
                return View(challenegdata);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("Challenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Create Admin Program by admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAdminProgram()
        {
            StringBuilder traceLog = null;
            CreateAdminProgram objAdminProgram = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin Program controller");
                objAdminProgram = new CreateAdminProgram();
                objListChallengeType = ChallengesBL.GetProgramType();
                ViewBag.ChallengeTypeList = objListChallengeType;
                objListTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objListTrainers;
                ViewBag.ChallengeSubTypeList = null;

                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                objAdminProgram.SelecetdTeams = objListTeams;
                objAdminProgram.AvailableTeams = objListTeams;
                List<TrendingCategory> trendingCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                objAdminProgram.AvailableTrendingCategory = trendingCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                objAdminProgram.AvailableSecondaryTrendingCategory = trendingCategoryList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constProgramChallengeSubType);
                objAdminProgram.AvailableChallengeCategory = challengeCategoryList;

                return View(objAdminProgram);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create admin Prpgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListTrainers = null;
            }
        }
        /// <summary>
        /// Post Program and save in database
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAdminProgram(CreateAdminProgram objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                ModelState.Remove("FeaturedImageRowData");
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: create admin program controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                    int trainerId = 0;
                    if (objModels.TrainerId != null)
                    {
                        int trainerUserId = (int)objModels.TrainerId;
                        trainerId = (int)TrainersBL.GetTrainerId(trainerUserId);
                    }
                    //if (objModels.ProfileUrl != null)
                    if (!String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constProfileImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();

                        }
                        objModels.ProgramImageUrl = picName;
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.ProgramImageUrl = newpicName;
                        }
                    }
                    ChallengesBL.SubmitProgram(objModels, credentialId, trainerId, string.Empty);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = 1, sort = ConstantHelper.constModifiedDate, sortdir = ConstantHelper.constDESC });
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetProgramType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                    ViewBag.TrainerProfilePhoto = objModels.CropImageRowData;
                    ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }
                    // Get Available trending category while fail validation
                    List<TrendingCategory> allTrendindlist= ChallengesCommonBL.GetTrendingCategory(objModels.ProgramType);
                    objModels.AvailableTrendingCategory = allTrendindlist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    objModels.AvailableSecondaryTrendingCategory = allTrendindlist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }                   
                    // Selected Team Id in case of validation
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ProgramType);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateAdminChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }


        /// <summary>
        /// Create Trainer Program by trainer admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateTrainerProgram(int? id)
        {
            StringBuilder traceLog = null;
            CreateAdminProgram objAdminProgram = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin Program controller");
                objAdminProgram = new CreateAdminProgram();
                if (id != null)
                {
                    int programId = (int)id;
                    objAdminProgram = ChallengesBL.GetProgramById(programId);
                    objListChallengeType = ChallengesBL.GetProgramType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                    ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                    if (objAdminProgram.ProgramImageUrl != null)
                    {
                        ViewBag.TrainerProfilePhoto = "../../images/profilepic/" + objAdminProgram.ProgramImageUrl;
                    }
                    if (objAdminProgram.IsFeatured && objAdminProgram.FeaturedImageUrl != null)
                    {
                        ViewBag.FeaturedPhoto = "../../images/featuredpic/" + objAdminProgram.FeaturedImageUrl;
                    }
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetProgramType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;

                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objAdminProgram.SelecetdTeams = objListTeams;
                    objAdminProgram.AvailableTeams = objListTeams;
                    List<TrendingCategory> trendingCategoryList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                    objAdminProgram.AvailableTrendingCategory = trendingCategoryList;
                    ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                    List<ChallengeCategory> challengeCategoryList = ChallengesCommonBL.GetChallengeCategory(ConstantHelper.constProgramChallengeSubType);
                    objAdminProgram.AvailableChallengeCategory = challengeCategoryList;
                }

                return View(objAdminProgram);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create admin Prpgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListTrainers = null;
            }
        }
        /// <summary>
        /// Submit Program by trainer
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTrainerProgram(CreateAdminProgram objModels)
        {
            StringBuilder traceLog = new StringBuilder();
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.consttrainer)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                ModelState.Remove("FeaturedImageRowData");
                ModelState.Remove("SelectedChallengeCategoryCheck");
                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: create admin program controller");
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);

                    //if (objModels.ProfileUrl != null)
                    if (!String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constProfileImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();

                        }
                        objModels.ProgramImageUrl = picName;
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.ProgramImageUrl = newpicName;
                        }
                    }
                    ChallengesBL.SubmitProgram(objModels, credentialId, credentialId, objModels.FormSubmitType);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetProgramType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                    ViewBag.TrainerProfilePhoto = objModels.CropImageRowData;
                    ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }
                    // Get Available trending category while fail validation
                    objModels.AvailableTrendingCategory = ChallengesCommonBL.GetTrendingCategory(objModels.ProgramType);
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ProgramType);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateAdminChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }
        /// <summary>
        /// Copy of Admin Program
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CopyAdminProgram(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeAdmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }

            try
            {
                traceLog.AppendLine("Start: CopyAdminProgram  controller");
                List<ChallengeTypes> objListChallengeType = ChallengesBL.GetChallengeType();
                //  List<SelectListItem> objSelectList = new List<SelectListItem>();
                string userType = Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]);
                int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session[ConstantHelper.constCredentialId]);
                int programId = ProgramBL.CreateCopyProgramById(id, userType, credentialId);
                CreateAdminProgram objAdminProgram = ChallengesBL.GetProgramById(programId);
                objListChallengeType = ChallengesBL.GetProgramType();
                ViewBag.ChallengeTypeList = objListChallengeType;
                List<ViewTrainers> objListTrainers = ChallengesBL.GetTrainers();
                ViewBag.TrainerList = objListTrainers;
                ViewBag.ChallengeSubTypeList = null;
                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                if (objAdminProgram.ProgramImageUrl != null)
                {
                    ViewBag.TrainerProfilePhoto = "../../images/profilepic/" + objAdminProgram.ProgramImageUrl;
                }
                if (objAdminProgram.IsFeatured && objAdminProgram.FeaturedImageUrl != null)
                {
                    ViewBag.FeaturedPhoto = "../../images/featuredpic/" + objAdminProgram.FeaturedImageUrl;
                }
                return View("UpdateProgram", objAdminProgram);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("CopyAdminProgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Saved Program based on  program Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateProgram(int id)
        {
            StringBuilder traceLog = null;
            CreateAdminProgram objAdminProgram = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateProgram() on Reporting controller");

                objAdminProgram = ChallengesBL.GetProgramById(id);
                objListChallengeType = ChallengesBL.GetProgramType();
                ViewBag.ChallengeTypeList = objListChallengeType;
                objListTrainers = ChallengesBL.GetTrainers();

                ViewBag.TrainerList = objListTrainers;
                ViewBag.ChallengeSubTypeList = null;
                /*Code for radio button list*/
                ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                /*Code for radio button list for difficulty*/
                ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                if (objAdminProgram.ProgramImageUrl != null)
                {
                    ViewBag.TrainerProfilePhoto = "../../images/profilepic/" + objAdminProgram.ProgramImageUrl;
                }
                if (objAdminProgram.IsFeatured && objAdminProgram.FeaturedImageUrl != null)
                {
                    ViewBag.FeaturedPhoto = "../../images/featuredpic/" + objAdminProgram.FeaturedImageUrl;
                }
                return View(objAdminProgram);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return View();
            }
            finally
            {
                traceLog.AppendLine("create admin Prpgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
                objListChallengeType = null;
                objListTrainers = null;
            }
        }
        /// <summary>
        /// Update Program based on Program Id
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateProgram(CreateAdminProgram objModels)
        {
            StringBuilder traceLog = null;
            List<ChallengeTypes> objListChallengeType = null;
            List<ViewTrainers> objListTrainers = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: create admin program controller");
                if (!objModels.IsFeatured || !string.IsNullOrEmpty(objModels.FeaturedImageUrl))
                {
                    ModelState.Remove("FeaturedImageRowData");
                }
                if (ModelState.IsValid)
                {
                    int credentialId = Convert.ToInt32(System.Web.HttpContext.Current.Session["CredentialId"]);
                    int trainerId = 0;
                    if (objModels.TrainerCredId.HasValue)
                    {
                        int trainerUserId = (int)objModels.TrainerCredId;
                        trainerId = (int)TrainersBL.GetTrainerId(trainerUserId);
                    }
                    //if (objModels.ProfileUrl != null)
                    if (!String.IsNullOrEmpty(objModels.CropImageRowData))
                    {
                        // string guidForImage = Guid.NewGuid().ToString();
                        string root = Server.MapPath(ConstantHelper.constProfileImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.CropImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            objModels.ProgramImageUrl = picName;
                        }
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.ProgramImageUrl = newpicName;
                        }
                    }
                    if (objModels.IsFeatured && !String.IsNullOrEmpty(objModels.FeaturedImageRowData))
                    {
                        string root = Server.MapPath(ConstantHelper.constFeaturedImagePath);
                        string base64data = string.Empty;
                        // string extension = ".jpg";
                        char[] split = { ',' };
                        string[] imagextension = objModels.FeaturedImageRowData.Split(split);
                        if (imagextension.Length > 1)
                        {
                            base64data = imagextension[1];
                        }
                        byte[] bytes = Convert.FromBase64String(base64data);
                        string picName = string.Empty;
                        string newpicName = string.Empty;
                        if (bytes.Length > 0)
                        {
                            picName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                            newpicName = Guid.NewGuid().ToString() + ConstantHelper.constProfileImageResize + ConstantHelper.constImageExtension;
                        }
                        using (var imageFile = new FileStream(root + ConstantHelper.constDoubleBackSlash + picName, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            objModels.FeaturedImageUrl = picName;
                        }
                        FileInfo fileInfo = new FileInfo(root + ConstantHelper.constDoubleBackSlash + picName);
                        double fileSizeKB = fileInfo.Length / 1024;
                        if (fileSizeKB > 300)
                        {
                            ImageHelper.VaryQualityLevel(root + ConstantHelper.constDoubleBackSlash + picName, root + ConstantHelper.constDoubleBackSlash + newpicName);
                            objModels.FeaturedImageUrl = newpicName;
                        }
                    }
                    else
                    {
                        objModels.FeaturedImageUrl = string.Empty;
                    }
                    // FeaturedImageRowData
                    ChallengesBL.UpdateProgram(objModels, credentialId, trainerId);
                    TempData["AlertMessage"] = Message.SubmitMessage;
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString(), new { page = objModels.Page, sort = objModels.SortField, sortdir = objModels.Sortdir });
                }
                else
                {
                    objListChallengeType = ChallengesBL.GetProgramType();
                    ViewBag.ChallengeTypeList = objListChallengeType;
                    objListTrainers = ChallengesBL.GetTrainers();
                    ViewBag.TrainerList = objListTrainers;
                    ViewBag.ChallengeSubTypeList = null;
                    /*Code for radio button list*/
                    ViewBag.ListBodyPartList = ChallengesCommonBL.GetBodyParts();
                    /*Code for radio button list for difficulty*/
                    ViewBag.ListDifficulty = ChallengesCommonBL.GetProgramDifficulties();
                    ViewBag.SelectedChallengeTypeId = ConstantHelper.constProgramChallengeSubType;
                    if (objModels.ProgramImageUrl != null)
                    {
                        ViewBag.TrainerProfilePhoto = "../../images/profilepic/" + objModels.ProgramImageUrl;
                    }
                    if (objModels.FeaturedImageUrl != null)
                    {
                        ViewBag.FeaturedPhoto = "../../images/profilepic/" + objModels.FeaturedImageUrl;
                    }
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    objModels.AvailableTeams = objListTeams;
                    objModels.SelecetdTeams = new List<DDTeams>();
                    // Selected Team Id in case of validation
                    if (objModels.PostedTeams != null)
                    {
                        if (objModels.PostedTeams.TeamsID != null)
                        {
                            foreach (var item in objModels.PostedTeams.TeamsID)
                            {
                                var teams = objListTeams.Where(m => m.TeamId == Convert.ToInt32(item)).FirstOrDefault();
                                if (teams != null)
                                {
                                    objModels.SelecetdTeams.Add(teams);
                                }
                            }
                        }
                    }
                    var allTrendingCatList= ChallengesCommonBL.GetTrendingCategory(objModels.ProgramType);
                    if (allTrendingCatList != null)
                    {
                        objModels.AvailableTrendingCategory = allTrendingCatList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                        objModels.AvailableSecondaryTrendingCategory = allTrendingCatList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    }
                    // Selected Team Id in case of validation
                    if (objModels.PostedTrendingCategory != null)
                    {
                        if (objModels.PostedTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    if (objModels.PostedSecondaryTrendingCategory != null)
                    {
                        if (objModels.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                        {
                            objModels.SelecetdSecondaryTrendingCategory = new List<TrendingCategory>();
                            foreach (var item in objModels.PostedSecondaryTrendingCategory.TrendingCategoryID)
                            {
                                var trendingCategory = objModels.AvailableSecondaryTrendingCategory.Where(m => m.TrendingCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (trendingCategory != null)
                                {
                                    objModels.SelecetdSecondaryTrendingCategory.Add(trendingCategory);
                                }
                            }
                        }
                    }
                    // Get Available challenge category while fail validation
                    objModels.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(objModels.ProgramType);
                    // Selected Team Id in case of validation
                    if (objModels.PostedChallengeCategory != null)
                    {
                        if (objModels.PostedChallengeCategory.ChallengeCategoryId != null && objModels.PostedChallengeCategory.ChallengeCategoryId.Count > 0)
                        {
                            objModels.SelecetdChallengeCategory = new List<ChallengeCategory>();
                            foreach (var item in objModels.PostedChallengeCategory.ChallengeCategoryId)
                            {
                                var challengeCategory = objModels.AvailableChallengeCategory.Where(m => m.ChallengeCategoryId == Convert.ToInt32(item)).FirstOrDefault();
                                if (challengeCategory != null)
                                {
                                    objModels.SelecetdChallengeCategory.Add(challengeCategory);
                                }
                            }
                            objModels.SelectedChallengeCategoryCheck = Message.NotAvailable;
                        }
                    }
                    return View(objModels);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("CreateAdminChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }
        /// <summary>
        /// Get All Filter workout based on difficult level and training zone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllFilterworkout(SearchWeekWorkoutVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetAllFilterworkout");
                List<WorkoutResponse> workoutList = new List<WorkoutResponse>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constuser)
                {
                    workoutList = ProgramBL.GetAllFilterWorkout(model);
                    return Json(workoutList, JsonRequestBehavior.AllowGet);
                }
                return Json(workoutList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetAllFilterworkout end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get All Filte rFitnessTest based on filter cateria
        /// </summary>
        /// <param name="serachItem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllFilterFitnessTest(string serachItem)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetAllFilterFitnessTest");
                List<TeamSearchResponse> workoutList = new List<TeamSearchResponse>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName])) || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    workoutList = CommonBL.GetAllFilterTeamSeachChallenge(serachItem, ConstantHelper.constFitnessTestName, 0);
                    return Json(workoutList, JsonRequestBehavior.AllowGet);
                }
                return Json(workoutList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetAllFilterFitnessTest end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get All Beginner Program based on filter caterias
        /// </summary>
        /// <param name="serachItem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllBeginnerProgram(string serachItem)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetAllFilterFitnessTest");
                List<TeamSearchResponse> workoutList = new List<TeamSearchResponse>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName])) || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    workoutList = CommonBL.GetAllFilterTeamSeachChallenge(serachItem, ConstantHelper.constProgramChallenge, ConstantHelper.constTeamSearchBeginnerLevel);
                    return Json(workoutList, JsonRequestBehavior.AllowGet);
                }
                return Json(workoutList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetAllFilterFitnessTest end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get All AdvInt Program based on  searched cateria
        /// </summary>
        /// <param name="serachItem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllAdvIntProgram(string serachItem)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetAllFilterFitnessTest");
                List<TeamSearchResponse> workoutList = new List<TeamSearchResponse>();
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName])) || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    workoutList = CommonBL.GetAllFilterTeamSeachChallenge(serachItem, ConstantHelper.constProgramChallenge, ConstantHelper.constTeamSearchIntAdvLevel);
                    return Json(workoutList, JsonRequestBehavior.AllowGet);
                }
                return Json(workoutList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetAllFilterFitnessTest end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Add Program Week based on program Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProgramWeek(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constuser)
            {
                try
                {
                    traceLog.AppendLine("Start: AddProgramWeek() for Add Program week ");
                    AddProgramWeekVM objAddProgramWeekVM = new AddProgramWeekVM()
                    {
                        WeekCountID = id
                    };
                    return PartialView("_AddWeek", objAddProgramWeekVM);
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("AddProgramWeek end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_AddWeek");
        }
        /// <summary>
        /// Add Program Week Workout for program 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProgramWeekWorkout(AddProgramWeekVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constuser)
            {
                try
                {
                    traceLog.AppendLine("Start: AddProgramWeekWorkout method() for Add Week workout in Reporting  controller");
                    return PartialView("_AddWeekWorkout", model);
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("AddFFExeciseSet end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_AddWeekWorkout");

        }
        /// <summary>
        /// Get Program Week Detail based on program Id and show on partial view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProgramWeekDetail(GetProgramWeekVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constuser)
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramWeekDetail method() for Fet Free form Execise set in Reporting  controller");
                    return PartialView("_GetProgramWeekDetail", model);
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("GetProgramWeekDetail end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_GetProgramWeekDetail");
        }
        /// <summary>
        /// Get Program Week WorkoutDetail and show on partisl view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProgramWeekWorkoutDetail(GetProgramWeekWorkoutVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constuser)
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramWeekWorkoutDetail method() for get Free form Execise set in Reporting  controller");
                    return PartialView("_GetProgramWeekWorkouts", model);
                }
                catch (Exception ex)
                {
                    LogManager.LogManagerInstance.WriteErrorLog(ex);
                    return null;
                }
                finally
                {
                    traceLog.AppendLine("GetProgramWeekWorkoutDetail end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return PartialView("_GetProgramWeekWorkouts");
        }
        /// <summary>
        /// Delete Program based program Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteProgram(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                traceLog.AppendLine("Start: DeleteProgram() in  controller");
                ProgramBL.DeleteProgram(id);
                TempData["AlertMessage"] = Message.DeleteMessage;
                return RedirectToAction(GetUrl(Request.UrlReferrer.AbsoluteUri));
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("DeleteProgram end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Seach Level Teams
        /// </summary>
        /// <param name="serachItem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSeachLevel1Team(string serachItem, string guidRecord, int teamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetSeachLevel1Team");
                List<LevelTeamVM> seachLevelTeam = null;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    seachLevelTeam = TeamTalkBL.GetSeachLevelTeam(serachItem, guidRecord, teamId);
                    return Json(seachLevelTeam, JsonRequestBehavior.AllowGet);
                }
                return Json(seachLevelTeam, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetSeachLevel1Team end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Add Level Teams in Primary Team
        /// </summary>
        /// <param name="primaryTeamId"></param>
        /// <param name="GuidRecord"></param>
        /// <param name="teamId"></param>
        /// <param name="teamName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddLevelTeam(int primaryTeamId, string GuidRecord, int teamId, string teamName, int levelTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: AddLevelTeam-teamName-" + teamName);
                LevelTeamVM addedLevelTeam = null;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    addedLevelTeam = TeamTalkBL.AddLevelTeam(primaryTeamId, GuidRecord, teamId, teamName, levelTypeId);
                }
                return PartialView("_AddLevelTeam", addedLevelTeam);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("AddLevelTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }

        }

        /// <summary>
        /// Get Level Teams
        /// </summary>
        /// <param name="primaryTeamId"></param>
        /// <param name="GuidRecord"></param>
        /// <param name="IsLevel1Team"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLevelTeams(int primaryTeamId, string GuidRecord, int levelTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetLevelTeams-primaryTeamId-" + primaryTeamId);
                List<LevelTeamVM> addedLevelTeams = null;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                    || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin)
                {
                    addedLevelTeams = TeamTalkBL.GetLevelTeams(primaryTeamId, GuidRecord, levelTypeId);
                }
                return PartialView("_GetLevelTeam", addedLevelTeams);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("GetLevelTeams end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }

        }

        /// <summary>
        /// Action to view trainer challenges page in trainer login
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari       
        /// </devdoc>
        [HttpGet]
        public ActionResult TeamAdminView()
        {
            StringBuilder traceLog = null;
            int userId = 0;
            TeamViewData objTeamViewData = new TeamViewData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constUserTypeWebTeam)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamAdminView controller");
                userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);               
                ViewBag.PrimaryTeams = TeamBL.GetAllTeamName();
                if (userId > 0)
                {
                    DateTime now = DateTime.Now;                    
                    TeamCommisionRequest objTeamCommisionRequest = new TeamCommisionRequest
                    {
                        TeamId = userId,
                        Month = now.Month - 1,
                        Year = now.Year
                    };
                    ViewBag.CommissionYear = TeamTalkBL.GetTeamCommissionYear(objTeamCommisionRequest);                  
                    objTeamViewData = TeamTalkBL.GetTeamCommisionDetails(userId, now.Month-1, now.Year);
                    objTeamViewData.SelectedYear = now.Year;
                    objTeamViewData.SelectedMonth = now.Month-1;
                    objTeamViewData.PrimaryTeam = userId;
                    if (objTeamViewData.PrimaryTeamDetail == null)
                    {
                        objTeamViewData.NoReportMessage = "No records found!";
                    }
                    else
                    {
                        objTeamViewData.NoReportMessage = string.Empty;
                    }

                }
                return View(objTeamViewData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TeamAdminView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Team AdminView
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TeamAdminView(FormCollection fc)
        {
            StringBuilder traceLog = null;
            // int credentialId = 0;
            TeamViewData objTeamViewData = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constUserTypeWebTeam)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                int userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                bool isShownCurrentMonth = true;
                int selectedMonth = 0;
                int selectedYear = 0;
                if (!Int32.TryParse(fc["SearchMonth"], out selectedMonth))
                {
                    selectedMonth = 0;
                }
                else
                {
                    isShownCurrentMonth = false;
                }
                if (!Int32.TryParse(fc["AdminTeamSearchYear"], out selectedYear))
                {
                    selectedYear = 0;
                }
                traceLog.AppendLine("Start: TeamsView controller");
                ViewBag.PrimaryTeams = TeamBL.GetAllTeamName();
                objTeamViewData = TeamTalkBL.GetTeamCommisionDetails(userId, selectedMonth, selectedYear);
                objTeamViewData.IsShowCurrentMonth = isShownCurrentMonth;
                objTeamViewData.SelectedMonth = selectedMonth;
                objTeamViewData.SelectedYear = selectedYear;
                objTeamViewData.PrimaryTeam = userId;
                TeamCommisionRequest objTeamCommisionRequest = new TeamCommisionRequest
                {
                    TeamId = userId,
                    Year = selectedYear,
                    Month = selectedMonth
                };               
                ViewBag.CommissionYear =  TeamTalkBL.GetTeamCommissionYear(objTeamCommisionRequest);
               // ViewBag.CommisionMonth = GetCommissionMonths();
                ViewBag.CommisionMonth = TeamTalkBL.GetTeamCommissionMonth(objTeamCommisionRequest);
                if (selectedMonth > 0)
                {
                    objTeamViewData.MonthLevel = GetMonthName(selectedMonth);
                }               
                if (objTeamViewData.PrimaryTeamDetail == null)
                {
                    objTeamViewData.NoReportMessage = "No records found!";
                }
                else
                {
                    objTeamViewData.NoReportMessage = string.Empty;
                }
                return View(objTeamViewData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TeamsView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// GetCommissionMonths
        /// </summary>
        /// <returns></returns>
        private List<CommisionMonth> GetCommissionMonths() 
        {
            List<CommisionMonth> commissionMonth = new List<CommisionMonth>()
                {
                    new CommisionMonth() {Month =1,Name="January"},
                    new CommisionMonth() {Month =2,Name="Febuary"},
                    new CommisionMonth() {Month =3,Name="March"},
                    new CommisionMonth() {Month =4,Name="April"},
                    new CommisionMonth() {Month =5,Name="May"},
                    new CommisionMonth() {Month =6,Name="June"},
                    new CommisionMonth() {Month =7,Name="July"},
                    new CommisionMonth() {Month =8,Name="August"},
                    new CommisionMonth() {Month =9,Name="September"},
                    new CommisionMonth() {Month =10,Name="October"},
                    new CommisionMonth() {Month =11,Name="November"},
                    new CommisionMonth() {Month =12,Name="December"},
                };
           return commissionMonth;
        }
        /// <summary>
        /// Get MonthName base month number
        /// </summary>
        /// <param name="monthNum"></param>
        /// <returns></returns>
        private string GetMonthName(int monthNum)
        {
            string monthName = "January";
            switch (monthNum)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "Febuary";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;

            }
            return monthName;
        }
        /// <summary>
        /// Send Mail to Team Admin for monthly commision report
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendCommisionTeam(TeamCommisionRequest teamRequest)
        {
            StringBuilder traceLog = null;
            // int credentialId = 0;
            TeamViewData objTeamViewData = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                int teamId = teamRequest.TeamId;
                traceLog.AppendLine("Start: TeamsView controller");

                DateTime startDate = new DateTime(teamRequest.Year, teamRequest.Month, 1);
               // DateTime endDate = new DateTime(teamRequest.Year, teamRequest.Month, DateTime.DaysInMonth(teamRequest.Year, teamRequest.Month));
                objTeamViewData = TeamTalkBL.GetTeamCommisionDetails(teamId, DateTime.Now.Month, DateTime.Now.Year);
                objTeamViewData.ReportDate = startDate;                //----------------------
                EmailService objEmailService = new EmailService();
                Controller currentcontr = this;
                string messagebody = RenderCommisionViewToString(currentcontr, "CommissionViewMailTemplate", objTeamViewData, "_CommissionViewLayout");
                objEmailService.SendMail(objTeamViewData.PrimaryTeamDetail.EmailId, messagebody, "Fitcom Commissions Details");
                //-----------------------
                return Json(new { Message = "Team commision report has sent to team administrator" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TeamsView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }



        /// <summary>
        /// Teams view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TeamsView()
        {
            StringBuilder traceLog = null;
            // int credentialId = 0;
            TeamViewData objTeamViewData = new TeamViewData();
            objTeamViewData.Level1TeamsCommision = 0.0M;
            objTeamViewData.Level2TeamsCommision = 0.0M;
            objTeamViewData.PrimaryTeamCommision = 0.0M;
            objTeamViewData.TotalTeamCommision = 0.0M;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamsView controller");
                ViewBag.PrimaryTeams = TeamBL.GetAllTeamName();
                //List<CommisionYear> commisionYear = new List<CommisionYear>();
                //for (int year = DateTime.Now.Year; year >= 2000; year--)
                //{
                //    commisionYear.Add(new CommisionYear() { Year = year });
                //}
                //List<CommisionMonth> commissionMonth = new List<CommisionMonth>()
                //{
                //    new CommisionMonth() {Month =1,Name="January"},
                //    new CommisionMonth() {Month =2,Name="Febuary"},
                //    new CommisionMonth() {Month =3,Name="March"},
                //    new CommisionMonth() {Month =4,Name="April"},
                //    new CommisionMonth() {Month =5,Name="May"},
                //    new CommisionMonth() {Month =6,Name="June"},
                //    new CommisionMonth() {Month =7,Name="July"},
                //    new CommisionMonth() {Month =8,Name="August"},
                //    new CommisionMonth() {Month =9,Name="September"},
                //    new CommisionMonth() {Month =10,Name="October"},
                //    new CommisionMonth() {Month =11,Name="November"},
                //    new CommisionMonth() {Month =12,Name="December"},
                //};

                //ViewBag.CommissionYear = commisionYear;
                //ViewBag.CommisionMonth = commissionMonth;
                //objTeamViewData.SelectedYear = DateTime.Now.Year;
                //objTeamViewData.SelectedMonth = 1;
                return View(objTeamViewData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TeamsView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TeamsView(FormCollection fc)
        {
            StringBuilder traceLog = null;
            // int credentialId = 0;
            TeamViewData objTeamViewData = null;
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
          || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamsView controller");
                int primartTeamId = 0;
                if (!int.TryParse(fc["SearchPrimaryTeam"], out primartTeamId))
                {
                    primartTeamId = 0;
                }
                int selectedMonth = 0;
                int selectedYear = 0;
                if (!Int32.TryParse(fc["SearchMonth"], out selectedMonth))
                {
                    selectedMonth = 0;
                }
                if (!Int32.TryParse(fc["SearchYear"], out selectedYear))
                {
                    selectedYear = 0;
                }
                ViewBag.PrimaryTeams = TeamBL.GetAllTeamName();     
                List<CommisionYear> commisionYear = new List<CommisionYear>();              
                TeamCommisionRequest searchCommissionRequest = new TeamCommisionRequest
                {
                    TeamId= primartTeamId,
                    Year= selectedYear,
                    Month= selectedMonth
                };
                commisionYear = TeamTalkBL.GetTeamCommissionYear(searchCommissionRequest);
                List<CommisionMonth> commissionMonth = TeamTalkBL.GetTeamCommissionMonth(searchCommissionRequest);
                ViewBag.CommissionYear = commisionYear;
                ViewBag.CommisionMonth = commissionMonth;
                objTeamViewData = TeamTalkBL.GetTeamCommisionDetails(primartTeamId, selectedMonth, selectedYear);
                objTeamViewData.SelectedMonth = selectedMonth;
                objTeamViewData.SearchedTeamId = primartTeamId;
                objTeamViewData.SelectedYear = selectedYear;
                if (selectedMonth > 0)
                {
                    objTeamViewData.MonthLevel = GetMonthName(selectedMonth);
                }
                if (objTeamViewData.PrimaryTeamDetail == null)
                {
                    objTeamViewData.NoReportMessage = "No records found!";
                }
                else
                {
                    objTeamViewData.NoReportMessage = string.Empty;
                }
                return View(objTeamViewData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("TeamsView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Trainer Program View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TrainerProgramView(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[Message.LoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != Message.UserTypeTrainer)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog.AppendLine("Start: TrainerProgramView controller");
                ViewProgramDetail objChallenge = new ViewProgramDetail();
                if (id > 0)
                {
                    objChallenge = ProgramBL.GetProgramById(id);
                }
                return View(objChallenge);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine("TrainerProgramView end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        #endregion
        /// <summary>
        /// Render Commission ViewToString
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="masterview"></param>
        /// <returns></returns>
        public static string RenderCommisionViewToString(Controller controller, string viewName, object model, string masterview)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, masterview);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public JsonResult GetTeamCommissionYear(TeamCommisionRequest searchCommissionRequest)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamCommissionYear()");
                List<CommisionYear> lstCommisionYear = null;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
             || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constUserTypeWebTeam)
                {
                    lstCommisionYear = TeamTalkBL.GetTeamCommissionYear(searchCommissionRequest);
                    return Json(lstCommisionYear, JsonRequestBehavior.AllowGet);
                }
                return Json(lstCommisionYear, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine(" End of GetTeamCommissionYear() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Team Generated Commission Month
        /// </summary>
        /// <param name="searchCommissionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTeamCommissionMonth(TeamCommisionRequest searchCommissionRequest)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamCommissionMonth()");
                List<CommisionMonth> lstCommisionMonth = null;
                if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                   || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constLoginadmin
                   || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) == ConstantHelper.constUserTypeWebTeam)
                {
                    lstCommisionMonth = TeamTalkBL.GetTeamCommissionMonth(searchCommissionRequest);
                    return Json(lstCommisionMonth, JsonRequestBehavior.AllowGet);
                }
                return Json(lstCommisionMonth, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                traceLog.AppendLine(" End of GetTeamCommissionMonth() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Export TeamView Commission information in Excel
        /// </summary>
        /// <param name="primartTeamId"></param>
        /// <param name="selectedMonth"></param>
        /// <param name="selectedYear"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportTeamViewCommissionExcelData(int id,int year,int month)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                TeamViewData teamViewData = TeamTalkBL.GetTeamCommisionDetails(id, month, year);
                if (teamViewData != null)
                {
                    string[] columns = { "TeamName", "EmailId", "PhoneNumber", "Users", "Premium" };
                    DataSet ds = new DataSet();
                    List<LevelTeamVM> primaryTeam = new List<LevelTeamVM>();
                    primaryTeam.Add(teamViewData.PrimaryTeamDetail);
                    ds.Tables.Add(ExcelExportHelper.ListToDataTable<LevelTeamVM>(primaryTeam));
                    ds.Tables.Add(ExcelExportHelper.ListToDataTable<LevelTeamVM>(teamViewData.Level1TeamDetail));
                    ds.Tables.Add(ExcelExportHelper.ListToDataTable<LevelTeamVM>(teamViewData.Level2TeamDetail));
                    List<TeamViewCommission> objTeamViewCommission = new List<TeamViewCommission>();
                    objTeamViewCommission.Add(new TeamViewCommission() { CommissionLevel = "*projected", CommissionTotal = Convert.ToString(GetMonthName(month) + " " + year)});
                    objTeamViewCommission.Add(new TeamViewCommission() { CommissionLevel = "Primary Team", CommissionTotal = Convert.ToString(teamViewData.PrimaryTeamCommision) });
                    objTeamViewCommission.Add(new TeamViewCommission() { CommissionLevel = "Level 1 Teams", CommissionTotal = Convert.ToString(teamViewData.Level1TeamsCommision) });
                    objTeamViewCommission.Add(new TeamViewCommission() { CommissionLevel = "Level 2 Teams", CommissionTotal = Convert.ToString(teamViewData.Level2TeamsCommision) });
                    objTeamViewCommission.Add(new TeamViewCommission() { CommissionLevel = "Total", CommissionTotal = Convert.ToString(teamViewData.TotalTeamCommision) });
                    ds.Tables.Add(ExcelExportHelper.ListToDataTable<TeamViewCommission>(objTeamViewCommission));                  
                    byte[] filecontent = ExportTeamViewExcel(ds, "", true, columns);
                    return File(filecontent, ExcelExportHelper.ExcelContentType, "TeamViewDetails.xlsx");
                }
                return new HttpNotFoundResult();
            }
            catch(Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return new HttpNotFoundResult();
            }
            finally
            {
                traceLog.AppendLine(" End of GetTeamCommissionMonth() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        
        /// <summary>
        /// Export TeamView Commisssion int Excel based on primary TeamId
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="heading"></param>
        /// <param name="showSrNo"></param>
        /// <param name="columnsToTake"></param>
        /// <returns></returns>
        public static byte[] ExportTeamViewExcel(DataSet dataset, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {
            StringBuilder traceLog = new StringBuilder();
            byte[] result = null;
            try
            {
                traceLog.AppendLine("Start ExportExcel() on Excel Helper");
                using (ExcelPackage package = new ExcelPackage())
                {
                    DataTable dataTable = dataset.Tables[0];
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} UserDetails", heading));
                    int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;
                    int index = dataTable.Rows.Count;                   
                    // add the content into the Excel file
                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);
                    DataTable dtLevel1Team = dataset.Tables[1];
                    int levle1TeamIndex = 1;
                    int levle2TeamIndex = 1;                   
                    if (dtLevel1Team != null)
                    {
                        levle1TeamIndex = index + dtLevel1Team.Rows.Count + 2;
                        workSheet.Cells["A" + levle1TeamIndex].LoadFromText("Level1 Teams");
                        ++levle1TeamIndex;
                        workSheet.Cells["A" + levle1TeamIndex].LoadFromDataTable(dtLevel1Team, true);
                        using (ExcelRange r = workSheet.Cells[levle1TeamIndex, 1, levle1TeamIndex, dataTable.Columns.Count])
                        {
                            r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            r.Style.Font.Bold = true;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                        }
                    }
                    DataTable dtLevel2Team = dataset.Tables[2];
                    if (dtLevel2Team != null)
                    {
                        levle2TeamIndex = levle1TeamIndex + dtLevel2Team.Rows.Count + 2;                        
                        workSheet.Cells["A" + levle2TeamIndex].LoadFromText("Level2 Teams");
                        ++levle2TeamIndex;
                        workSheet.Cells["A" + levle2TeamIndex].LoadFromDataTable(dtLevel2Team, true);
                        using (ExcelRange r = workSheet.Cells[levle2TeamIndex, 1, levle2TeamIndex, dataTable.Columns.Count])
                        {
                            r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            r.Style.Font.Bold = true;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                        }
                    }                 
                    DataTable dtTotalcommission = dataset.Tables[3];
                    if (dtTotalcommission != null)
                    { 
                    levle2TeamIndex = levle2TeamIndex+dtTotalcommission.Rows.Count;
                        workSheet.Cells["A" + levle2TeamIndex].LoadFromText("Commissions");
                        levle2TeamIndex++;
                        foreach (DataRow dr in dtTotalcommission.Rows)
                        {    workSheet.Cells["A" + levle2TeamIndex].LoadFromText(Convert.ToString(dr["CommissionLevel"])+":      "+ Convert.ToString(dr["CommissionTotal"]));
                             levle2TeamIndex++;
                        }
                    }
                    // autofit width of cells with small content
                    int columnIndex = 1;
                    for (int count = 0; dataTable.Columns.Count >= count; count++)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                        columnIndex++;
                    }
                    // format header - bold, yellow on black
                    using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                    }
                    // format cells - add borders
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                    // removed ignored columns
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                    {
                        if (i == 0 && showSrNo)
                        {
                            continue;
                        }
                        if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                        {
                            workSheet.DeleteColumn(i + 1);
                        }
                    }
                    if (!String.IsNullOrEmpty(heading))
                    {
                        workSheet.Cells["A1"].Value = heading;
                        workSheet.Cells["A1"].Style.Font.Size = 20;

                        workSheet.InsertColumn(1, 1);
                        workSheet.InsertRow(1, 1);
                        workSheet.Column(1).Width = 5;
                    }
                    result = package.GetAsByteArray();
                }
                return result;
            }
            finally
            {
               
                traceLog.AppendLine("End  ExportExcel() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

    }
}

