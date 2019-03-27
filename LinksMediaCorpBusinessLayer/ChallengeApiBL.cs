namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using System.Text.RegularExpressions;
    using System.IO;
    using LinksMediaCorpUtility;
    using System.Web;
    using System.Drawing;
    using LinksMediaCorpUtility.Resources;
    public class ChallengeApiBL
    {

        private static object syncLock = new object();
        #region WebAPIBL
        /// <summary>
        /// Get Most Popular Challenge
        /// </summary>
        /// <returns></returns>
        public static CategoryResponse GetFittnessCategoryList()
        {
            StringBuilder traceLog = new StringBuilder();
            CategoryResponse objFittnessTestChallenge = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: MostPopularChallenge---- " + DateTime.Now.ToLongDateString());
                    List<int> teamIds = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int primaryTeamId = 0;
                    List<int> teamtrainers = new List<int>();
                    switch (cred.UserType)
                    {
                        case ConstantHelper.constuser:
                            teamIds = (from usr in dataContext.User
                                       join crd in dataContext.Credentials
                                       on usr.UserId equals crd.UserId
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                       select usr.TeamId).ToList();
                            primaryTeamId = teamIds.FirstOrDefault();
                            break;
                        case ConstantHelper.consttrainer:
                            teamIds = (from crd in dataContext.Credentials
                                       join tms in dataContext.TrainerTeamMembers
                                       on crd.Id equals tms.UserId
                                       orderby tms.RecordId ascending
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                       select tms.TeamId).ToList();
                            primaryTeamId = dataContext.Trainer.Where(tr => tr.TrainerId == cred.UserId).Select(t => t.TeamId).FirstOrDefault();
                            break;
                    }
                    objFittnessTestChallenge = new CategoryResponse();
                    if (teamIds != null && teamIds.Count > 0)
                    {
                        teamtrainers = (from crd in dataContext.Credentials
                                        join tms in dataContext.TrainerTeamMembers
                                        on crd.Id equals tms.UserId
                                        where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                        select tms.UserId).ToList();

                    }

                    //Challenge feed sorted by acceptors
                    objFittnessTestChallenge.ChallengeCategoryList = ChallengesCommonBL.GetExistingFittnestFittnessTestChallenge(dataContext, ConstantHelper.constFittnessCommonSubTypeId, teamtrainers, primaryTeamId);
                    objFittnessTestChallenge.FeaturedList = TeamApiBL.GetTeamFeaturedList(dataContext, ConstantHelper.constFittnessCommonSubTypeId, teamIds, primaryTeamId);
                    objFittnessTestChallenge.TrendingCategoryList = TeamApiBL.GetTeamTrendingCategoryList(dataContext, ConstantHelper.constFittnessCommonSubTypeId, teamIds, primaryTeamId);
                    return objFittnessTestChallenge;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  MostPopularChallenge : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get all active challenge list
        /// </summary>
        /// <returns>Total<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/23/2015
        /// </devdoc>
        public static List<MainChallengeVM> GetAllChallengeList()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllChallengeList---- " + DateTime.Now.ToLongDateString());
                    DateTime today = DateTime.Now.Date;
                    //Get All ChallengeId which is currently belong to Challenge of the day
                    List<int> exceptListCOD = (from cod in dataContext.ChallengeofTheDayQueue
                                               join c in dataContext.Challenge on cod.ChallengeId equals c.ChallengeId
                                               where c.IsActive && cod.StartDate <= today && cod.EndDate >= today
                                               select c.ChallengeId).ToList();

                    //Get All ChallengeId which is currently belong to Sponsor Challenge.
                    List<int> exceptListSponsor = (from tc in dataContext.TrainerChallenge
                                                   join c in dataContext.Challenge on tc.ChallengeId equals c.ChallengeId
                                                   where c.IsActive && tc.StartDate <= today && tc.EndDate >= today
                                                   select c.ChallengeId).ToList();
                    //Get All Challenge list exclude Challenge Of the Day(exceptListCOD) and Sponsor Challenge(exceptListSponsor)
                    List<MainChallengeVM> listMainVM = (from c in dataContext.Challenge
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        where !exceptListCOD.Contains(c.ChallengeId)
                                                        && !exceptListSponsor.Contains(c.ChallengeId)
                                                        && c.IsActive
                                                        orderby c.ChallengeName ascending
                                                        select new MainChallengeVM
                                                        {
                                                            ChallengeId = c.ChallengeId,
                                                            ChallengeName = c.ChallengeName,
                                                            DifficultyLevel = c.DifficultyLevel,
                                                            ChallengeType = ct.ChallengeType,
                                                            ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                            Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId)
                                                                                                .Select(y => y.UserId).Distinct().Count(),
                                                            ResultUnit = ct.ResultUnit
                                                        }).ToList();

                    listMainVM.ForEach(r =>
                    {
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;

                    });
                    return listMainVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetAllChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get All Challenge Type
        /// </summary>
        /// <returns></returns>
        public static ChallegeTypeDetail GetAllChallengeType()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllChallengeType()---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    ChallegeTypeDetail challengeTypesDetails = new ChallegeTypeDetail();
                    // Find the primary trainer ID
                    int teamId = 0;
                    if (objCred.UserType.Equals(Message.UserTypeTrainer))
                    {
                        teamId = dataContext.Trainer.Where(tm => tm.TrainerId == objCred.UserId).Select(tt => tt.TeamId).FirstOrDefault();
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeUser))
                    {
                        teamId = dataContext.User.Where(tm => tm.UserId == objCred.UserId).Select(tt => tt.TeamId).FirstOrDefault();
                    }
                    if (teamId == 0)
                    {
                        teamId = dataContext.Teams.Where(t => t.IsDefaultTeam).Select(tt => tt.TeamId).FirstOrDefault();
                    }
                    TeamsVM teamdetails = (from t in dataContext.Teams
                                           join cr in dataContext.Credentials
                                           on t.TeamId equals cr.UserId
                                           where t.TeamId == teamId && cr.UserType == Message.UserTypeTeam
                                           select new TeamsVM
                                           {
                                               TeamName = t.TeamName,
                                               TeamId = t.TeamId,
                                               ImagePicUrl = t.ProfileImageUrl,
                                               ImagePremiumUrl = t.PremiumImageUrl,
                                               HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty
                                           }).FirstOrDefault();
                    if (teamdetails != null)
                    {
                        teamdetails.TeamName = string.IsNullOrEmpty(teamdetails.TeamName) ? string.Empty : teamdetails.TeamName.Substring(1);
                        teamdetails.ImagePicUrl = string.IsNullOrEmpty(teamdetails.ImagePicUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ProfilePicDirectory + teamdetails.ImagePicUrl;
                        string picName = teamdetails.ImagePremiumUrl;
                        teamdetails.ImagePremiumUrl = string.IsNullOrEmpty(teamdetails.ImagePremiumUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ProfilePicDirectory + teamdetails.ImagePremiumUrl;
                        string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + picName;
                        if (System.IO.File.Exists(filePath))
                        {
                            using (Bitmap objBitmap = new Bitmap(filePath))
                            {
                                double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                teamdetails.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                teamdetails.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                            }
                        }
                        else
                        {
                            teamdetails.Height = string.Empty;
                            teamdetails.Width = string.Empty;
                        }
                    }
                    else
                    {
                        teamdetails = new TeamsVM();
                        teamdetails.ImagePremiumUrl = string.Empty;
                        teamdetails.ImagePicUrl = string.Empty;
                        teamdetails.TeamName = string.Empty;
                        teamdetails.Height = string.Empty;
                        teamdetails.Width = string.Empty;
                    }
                    challengeTypesDetails.FitcomChallengeName = ConstantHelper.constFitconchallenge;
                    challengeTypesDetails.WorkoutChallengeName = ConstantHelper.constWorkoutChallenge;
                    challengeTypesDetails.PremiumChallengeName = ConstantHelper.constPremiumChallenge;
                    challengeTypesDetails.ProgramChallengeName = ConstantHelper.constProgramChallenge;
                    challengeTypesDetails.ProgramTypeID = ConstantHelper.constProgramChallengeSubType;
                    challengeTypesDetails.PremiumChallengeDetail = teamdetails;
                    challengeTypesDetails.WorksoutList = ChallengesCommonBL.GetapiChallengeCategoryList(ConstantHelper.constWorkoutChallengeSubType);
                    challengeTypesDetails.ProgramList = ChallengesCommonBL.GetapiChallengeCategoryList(ConstantHelper.constProgramChallengeSubType);
                    challengeTypesDetails.PremimumTypeList = ChallengesCommonBL.GetPremiumChallengeCategoryList();
                    return challengeTypesDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetAllChallengeType : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Fittenest Test Filter Challenge List based body part zone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<MainChallengeVM> GetFittenestTestFilterChallengeList(ChallengeFilterParam model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFittenestTestFilterChallengeList---- " + DateTime.Now.ToLongDateString());
                    List<int> teamIds = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int primaryTeamId = 0;
                    List<int> teamtrainers = new List<int>();
                    switch (cred.UserType)
                    {
                        case ConstantHelper.constuser:
                            teamIds = (from usr in dataContext.User
                                       join crd in dataContext.Credentials
                                       on usr.UserId equals crd.UserId
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                       select usr.TeamId).ToList();
                            primaryTeamId = teamIds.FirstOrDefault();
                            break;
                        case ConstantHelper.consttrainer:
                            teamIds = (from crd in dataContext.Credentials
                                       join tms in dataContext.TrainerTeamMembers
                                       on crd.Id equals tms.UserId
                                       orderby tms.RecordId ascending
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                       select tms.TeamId).ToList();
                            primaryTeamId = dataContext.Trainer.Where(tr => tr.TrainerId == cred.UserId).Select(t => t.TeamId).FirstOrDefault();
                            break;
                    }
                    if (teamIds != null && teamIds.Count > 0)
                    {
                        teamtrainers = (from crd in dataContext.Credentials
                                        join tms in dataContext.TrainerTeamMembers
                                        on crd.Id equals tms.UserId
                                        where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                        select tms.UserId).ToList();

                    }

                    return GetFilterChallengeList(model, dataContext, teamtrainers, primaryTeamId, ConstantHelper.constFittnessCommonSubTypeId);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetFittenestTestFilterChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get filtered challenge list
        /// </summary>
        /// <returns>Total<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/14/2015
        /// </devdoc>
        public static List<MainChallengeVM> GetFilterChallengeList(ChallengeFilterParam model, LinksMediaContext dataContext, List<int> teamtrainers, int primaryTeamId, int challengeSubTypeId)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetFilterChallengeList---- " + DateTime.Now.ToLongDateString());
                model.BodyZone = !string.IsNullOrEmpty(model.BodyZone) ? model.BodyZone.ToUpper(CultureInfo.InvariantCulture) : null;
                // DateTime today = DateTime.Now.Date;
                //Get All ChallengeId which is currently belong to Challenge of the day
                var exceptListCOD = CommonApiBL.GetCODList(dataContext);
                //Get All ChallengeId which is currently belong to Sponsor Challenge.
                var exceptListSponsor = CommonApiBL.GetSponsorCODList(dataContext);
                //Get All Filter Challenge list exclude Challenge Of the Day(exceptListCOD) and Sponsor Challenge(exceptListSponsor)
                List<MainChallengeVM> listMainVM = new List<MainChallengeVM>();
                List<int> ChallengeSubTypeList = TeamApiBL.GetChallengeAllSubTypeId(challengeSubTypeId);               
                bool isDefaultTeam = false;
                 bool isShownNoTrainerFittnessTest = false;
                if (primaryTeamId > 0)
                {
                    var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                    if (primaryTeam != null)
                    {
                        isDefaultTeam = primaryTeam.IsDefaultTeam;
                        isShownNoTrainerFittnessTest = primaryTeam.IsShownNoTrainerFitnessTests;
                    }
                }
                listMainVM = (from c in dataContext.Challenge
                              join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                              orderby c.ChallengeName
                              where !exceptListCOD.Contains(c.ChallengeId)
                                   && !exceptListSponsor.Contains(c.ChallengeId)
                                   && c.IsActive
                                   && ((model.BodyZone != null && ConstantHelper.FreeFitnessTests.Equals(model.BodyZone, StringComparison.OrdinalIgnoreCase) && c.IsFreeFitnessTest) || (model.BodyZone != null  && (from trzone in dataContext.TrainingZoneCAssociations
                                                                                                                                                                                                                                            join bp in dataContext.BodyPart
                                                                                                                                                                                                                                                on trzone.PartId equals bp.PartId
                                                                                                                                                                                                                                            where trzone.ChallengeId == c.ChallengeId
                                                                                                                                                                                                                                            select bp.PartName.ToUpper()).ToList<string>().Contains(model.BodyZone)))

                                 && c.ChallengeSubTypeId > 0
                                 && ChallengeSubTypeList.Contains(c.ChallengeSubTypeId)
                                 && ((c.TrainerId > 0 && teamtrainers.Contains(c.TrainerId)) 
                                 || (c.TrainerId == 0 && isShownNoTrainerFittnessTest)
                                   )
                              orderby c.ChallengeName ascending
                              select new MainChallengeVM
                              {
                                  ChallengeId = c.ChallengeId,
                                  ChallengeName = c.ChallengeName,
                                  TrainerId = c.TrainerId,
                                  DifficultyLevel = c.DifficultyLevel,
                                  ChallengeType = ct.ChallengeType,
                                  ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                  IsSubscription = c.IsSubscription,
                                  TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                    join bp in dataContext.BodyPart
                                                    on trzone.PartId equals bp.PartId
                                                    where trzone.ChallengeId == c.ChallengeId
                                                    select bp.PartName).Distinct().ToList<string>(),
                                  Excercises = (from ce in dataContext.CEAssociation
                                                join exer in dataContext.Exercise on ce.ExerciseId equals exer.ExerciseId into execisegroup
                                                from alExe in execisegroup.DefaultIfEmpty()
                                                where ce.ChallengeId == c.ChallengeId
                                                orderby ce.RocordId ascending
                                                select new ExerciseVM
                                                {
                                                    ExerciseId = alExe.ExerciseId,
                                                    ExerciseName = (alExe.ExerciseName == null) ? string.Empty : alExe.ExerciseName,
                                                    ExcersiceDescription = (alExe.Description == null) ? string.Empty : alExe.Description,
                                                    ChallengeExcerciseDescription = ce.Description,
                                                    AlternateExcerciseDescription = ce.AlternateExeciseName,
                                                    IsAlternateExeciseName = ce.IsAlternateExeciseName,
                                                    VedioLink = (alExe.V720pUrl == null) ? string.Empty : alExe.V720pUrl,
                                                    Index = (alExe.Index == null) ? string.Empty : alExe.Index,
                                                    Reps = ce.Reps,
                                                    WeightForManDB = ce.WeightForMan,
                                                    WeightForWomanDB = ce.WeightForWoman,
                                                    ExerciseThumnail = (alExe != null && alExe.ThumnailUrl == null) ? string.Empty : alExe.ThumnailUrl,
                                                    ExeciseSetList = (from exsetce in dataContext.CESAssociations
                                                                      where exsetce.RecordId == ce.RocordId
                                                                      select new ExeciseSetVM
                                                                      {
                                                                          Description = (string.IsNullOrEmpty(exsetce.Description)
                                                                          || string.Compare(exsetce.Description, ConstantHelper.constExeciseSetSeperator) == 0) ? string.Empty : exsetce.Description,
                                                                          IsRestType = exsetce.IsRestType,
                                                                          RestTime = string.IsNullOrEmpty(exsetce.RestTime) ? string.Empty : exsetce.RestTime,
                                                                          SetReps = exsetce.SetReps,
                                                                          SetResult = string.IsNullOrEmpty(exsetce.SetResult) ? string.Empty : exsetce.SetResult,
                                                                          IsAutoCountDown = exsetce.AutoCountDown
                                                                      }).ToList()
                                                }).ToList(),
                                  Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                  ResultUnit = ct.ResultUnit
                              }).ToList();
                if (listMainVM != null && listMainVM.Count > 0)
                {
                    //if (isDefaultTeam)
                    //{
                    //    listMainVM = listMainVM.Where(ct => !(ct.TrainerId > 0)).ToList();
                    //}
                    //else
                    //{
                        listMainVM = listMainVM.Where(ct => ct.TrainerId > 0 || (ct.TrainerId == 0)).ToList();
                   // }
                }
                return listMainVM = FilterChallengeList(listMainVM);
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetFilterChallengeList : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }

        }
        /// <summary>
        /// Filter Challenge List
        /// </summary>
        /// <param name="listMainVM"></param>
        /// <returns></returns>
        private static List<MainChallengeVM> FilterChallengeList(List<MainChallengeVM> listMainVM)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("start : FilterChallengeList()");
                listMainVM.ForEach(r =>
                {
                    r.ChallengeType = r.ChallengeType.Split(' ')[0];
                    r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                    r.Excercises.ForEach(
                        exer =>
                        {
                            exer.WeightForMan = exer.WeightForManDB > 0 ? Convert.ToString(exer.WeightForManDB) + " " + ConstantHelper.constlbs : null;
                            exer.WeightForWoman = exer.WeightForWomanDB > 0 ? Convert.ToString(exer.WeightForWomanDB) + " " + ConstantHelper.constlbs : null;
                            exer.VedioLink = !string.IsNullOrEmpty(exer.VedioLink) ? exer.VedioLink : string.Empty;
                            if (string.IsNullOrEmpty(exer.ExerciseThumnail))
                            {
                                if (!string.IsNullOrEmpty(exer.ExerciseName))
                                {
                                    string thumnailName = exer.ExerciseName.Replace(" ", string.Empty);
                                    exer.ExerciseThumnail = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName +
                                        Message.JpgImageExtension;
                                }
                                else
                                {
                                    exer.ExerciseThumnail = string.Empty;
                                }
                            }
                            exer.ChallengeExcerciseDescription = (string.IsNullOrEmpty(exer.ChallengeExcerciseDescription)
                                || exer.ChallengeExcerciseDescription == ConstantHelper.constFFChallangeDescription) ? string.Empty : exer.ChallengeExcerciseDescription;
                            exer.ExerciseName = (exer.IsAlternateExeciseName) ? ((string.IsNullOrEmpty(exer.AlternateExcerciseDescription)
                                || exer.AlternateExcerciseDescription == ConstantHelper.constFFChallangeDescription) ? string.Empty : exer.AlternateExcerciseDescription) : exer.ExerciseName;
                            exer.ExeciseSetList.ForEach(exeset =>
                            {
                                exeset.RestTime = formatSetTimeFormat(exeset.RestTime);
                                exeset.SetResult = formatSetTimeFormat(exeset.SetResult);
                                exeset.Description = (string.IsNullOrEmpty(exeset.Description)
                                    || string.Compare(exer.ChallengeExcerciseDescription, ConstantHelper.constExeciseSetSeperator, StringComparison.OrdinalIgnoreCase) == 0) ?
                                    string.Empty : exeset.Description;
                            });
                        });

                    if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                    {
                        r.TargetZone = string.Join(", ", r.TempTargetZone);
                    }
                    r.TempTargetZone = null;
                });
                return listMainVM;
            }
            finally
            {
                traceLog.AppendLine("End  FilterChallengeList : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Filter FreeFitcomChallengeList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<MainChallengeVM> GetFilterFreeFitcomChallengeList(ChallengeFilterParam model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFilterChallengeList---- " + DateTime.Now.ToLongDateString());
                    // Manipulate value in model(ChallengeFilterParam) object according to request parameteres.
                    //We have modify the Type/Difficulty/Equipment/Bodyzone properties
                    model.Type = !string.IsNullOrEmpty(model.Type) ? (model.Type.Equals(Message.All) ? null : model.Type) : null;
                    model.Difficulty = !string.IsNullOrEmpty(model.Difficulty) ? (model.Difficulty.Equals(Message.All) ? null : model.Difficulty) : null;
                    model.Equipment = !string.IsNullOrEmpty(model.Equipment) ? (model.Equipment.Equals(Message.All) ? null : model.Equipment) : null;
                    model.ExerciseType = !string.IsNullOrEmpty(model.ExerciseType) ? (model.ExerciseType.Equals(Message.All) ? null : model.ExerciseType) : null;
                    model.BodyZone = !string.IsNullOrEmpty(model.BodyZone) ? (model.BodyZone.Equals(Message.All) ? null : model.BodyZone) : null;
                    DateTime today = DateTime.Now.Date;

                    //Get All ChallengeId which is currently belong to Challenge of the day
                    List<int> exceptListCOD = (from cod in dataContext.ChallengeofTheDayQueue
                                               join c in dataContext.Challenge on cod.ChallengeId equals c.ChallengeId
                                               where c.IsActive && cod.StartDate <= today && cod.EndDate >= today
                                               select c.ChallengeId).ToList();

                    //Get All ChallengeId which is currently belong to Sponsor Challenge.
                    List<int> exceptListSponsor = (from tc in dataContext.TrainerChallenge
                                                   join c in dataContext.Challenge on tc.ChallengeId equals c.ChallengeId
                                                   where c.IsActive && tc.StartDate <= today && tc.EndDate >= today
                                                   select c.ChallengeId).ToList();

                    //Get All Filter Challenge list exclude Challenge Of the Day(exceptListCOD) and Sponsor Challenge(exceptListSponsor)
                    List<MainChallengeVM> listMainVM = null;
                    string challengeType = ConstantHelper.FreeFormChallengeType;
                    if (!string.IsNullOrEmpty(model.ExerciseType))
                    {
                        listMainVM = (from c in dataContext.Challenge
                                      join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                      join etc in dataContext.ETCAssociations on c.ChallengeId equals etc.ChallengeId
                                      join etype in dataContext.ExerciseTypes on etc.ExerciseTypeId equals etype.ExerciseTypeId
                                      where !exceptListCOD.Contains(c.ChallengeId)
                                           && !exceptListSponsor.Contains(c.ChallengeId)
                                           && c.IsActive
                                           && (model.Difficulty == null || c.DifficultyLevel == model.Difficulty)
                                           && (model.BodyZone == null || (from trzone in dataContext.TrainingZoneCAssociations
                                                                          join bp in dataContext.BodyPart
                                                                          on trzone.PartId equals bp.PartId
                                                                          where trzone.ChallengeId == c.ChallengeId
                                                                          select bp.PartName).ToList<string>().Contains(model.BodyZone))
                                           && (model.Equipment == null || (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                           join bp in dataContext.Equipments
                                                                           on trzone.EquipmentId equals bp.EquipmentId
                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                           select bp.Equipment).ToList<string>().Contains(model.Equipment))
                                           && (model.Type == null || ct.ChallengeType.Contains(model.Type))
                                           && (model.ExerciseType == null || etype.ExerciseName.Contains(model.ExerciseType))
                                           && !(c.TrainerId > 0)
                                           && ct.ChallengeType != challengeType
                                           && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                      orderby c.ChallengeName ascending
                                      select new MainChallengeVM
                                      {
                                          ChallengeId = c.ChallengeId,
                                          ChallengeName = c.ChallengeName,
                                          DifficultyLevel = c.DifficultyLevel,
                                          ChallengeType = ct.ChallengeType,
                                          IsSubscription = c.IsSubscription,
                                          ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                          TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                            join bp in dataContext.Equipments
                                                            on trzone.EquipmentId equals bp.EquipmentId
                                                            where trzone.ChallengeId == c.ChallengeId
                                                            select bp.Equipment).Distinct().ToList<string>(),
                                          TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                            join bp in dataContext.BodyPart
                                                            on trzone.PartId equals bp.PartId
                                                            where trzone.ChallengeId == c.ChallengeId
                                                            select bp.PartName).Distinct().ToList<string>(),
                                          Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                          ResultUnit = ct.ResultUnit
                                      }).ToList();
                    }
                    else
                    {
                        listMainVM = (from c in dataContext.Challenge
                                      join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                      where !exceptListCOD.Contains(c.ChallengeId)
                                           && !exceptListSponsor.Contains(c.ChallengeId)
                                           && c.IsActive
                                           && (model.Difficulty == null || c.DifficultyLevel == model.Difficulty)
                                           && (model.BodyZone == null || (from trzone in dataContext.TrainingZoneCAssociations
                                                                          join bp in dataContext.BodyPart
                                                                          on trzone.PartId equals bp.PartId
                                                                          where trzone.ChallengeId == c.ChallengeId
                                                                          select bp.PartName).ToList<string>().Contains(model.BodyZone))
                                           && (model.Equipment == null || (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                           join bp in dataContext.Equipments
                                                                           on trzone.EquipmentId equals bp.EquipmentId
                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                           select bp.Equipment).ToList<string>().Contains(model.Equipment))
                                           && (model.Type == null || ct.ChallengeType.Contains(model.Type))
                                           && !(c.TrainerId > 0)
                                           && ct.ChallengeType != challengeType
                                           && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                      orderby c.ChallengeName ascending
                                      select new MainChallengeVM
                                      {
                                          ChallengeId = c.ChallengeId,
                                          ChallengeName = c.ChallengeName,
                                          DifficultyLevel = c.DifficultyLevel,
                                          ChallengeType = ct.ChallengeType,
                                          ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                          IsSubscription = c.IsSubscription,
                                          TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                            join bp in dataContext.Equipments
                                                            on trzone.EquipmentId equals bp.EquipmentId
                                                            where trzone.ChallengeId == c.ChallengeId
                                                            select bp.Equipment).Distinct().ToList<string>(),
                                          TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                            join bp in dataContext.BodyPart
                                                            on trzone.PartId equals bp.PartId
                                                            where trzone.ChallengeId == c.ChallengeId
                                                            select bp.PartName).Distinct().ToList<string>(),
                                          Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                          ResultUnit = ct.ResultUnit
                                      }).ToList();
                    }
                    listMainVM = listMainVM.OrderBy(item => item.ChallengeName).ToList();
                    listMainVM.ForEach(r =>
                    {
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];

                        if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                        {
                            r.TargetZone = string.Join(", ", r.TempTargetZone);
                        }
                        r.TempTargetZone = null;
                        if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                        {
                            r.Equipment = string.Join(", ", r.TempEquipments);
                        }
                        r.TempEquipments = null;
                        r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                    });

                    return listMainVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetFilterChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get a challenge details by challengeId
        /// </summary>
        /// <returns>ChallengeDetailVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/25/2015
        /// </devdoc>
        public static ChallengeDetailVM GetChallengeDetail(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeDetail---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    //Query to get Challenge Details from tblChallenge and tblChallengeType on the basis of ChallengeId
                    ChallengeDetailVM objChallengedetail = GetChallenge(dataContext, challengeId);
                    if (objChallengedetail != null)
                    {
                        objChallengedetail = CommonApiBL.ChallengeDetail(objChallengedetail, dataContext, challengeId, objCred);
                    }
                    return objChallengedetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetChallengeDetail  sucessfully: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Challenge detaile based challenge Id
        /// </summary>
        /// <param name="objCred"></param>
        /// <returns></returns>
        private static ChallengeDetailVM GetChallenge(LinksMediaContext dataContext, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetChallengeDetail---- " + DateTime.Now.ToLongDateString());
                ChallengeDetailVM objChallengedetail = (from c in dataContext.Challenge
                                                        join cred in dataContext.Credentials on c.TrainerId equals cred.Id into tranr
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        from trainer in tranr.DefaultIfEmpty()
                                                        where c.ChallengeId == challengeId
                                                        select new ChallengeDetailVM
                                                        {
                                                            ChallengeId = c.ChallengeId,
                                                            ChallengeName = c.ChallengeName,
                                                            DifficultyLevel = c.DifficultyLevel,
                                                            ChallengeType = ct.ChallengeType,
                                                            IsSubscription = c.IsSubscription,
                                                            TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                              join bp in dataContext.Equipments
                                                                              on trzone.EquipmentId equals bp.EquipmentId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.Equipment).Distinct().ToList<string>(),
                                                            TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                              join bp in dataContext.BodyPart
                                                                              on trzone.PartId equals bp.PartId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.PartName).Distinct().ToList<string>(),
                                                            Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().ToList().Count,
                                                            ResultUnit = ct.ResultUnit,
                                                            ChallengeDescription = ct.ChallengeSubType,
                                                            VariableValue = c.VariableValue,
                                                            ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                            VariableUnit = ct.Unit,
                                                            CreatedByUserType = trainer.UserType,
                                                            CreatedByTrainerId = trainer.UserId,
                                                            Description = c.Description,
                                                            ChallengeDuration = c.FFChallengeDuration,
                                                            ChallengeDetail = c.ChallengeDetail
                                                        }).FirstOrDefault();

                if (objChallengedetail != null)
                {
                    objChallengedetail.Excercises = CommonApiBL.ChallengeExeciseDeatils(dataContext, challengeId);
                }
                return objChallengedetail;

            }
            finally
            {
                traceLog.AppendLine("End  GetChallengeDetail : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setresult"></param>
        /// <returns></returns>
        private static string formatSetTimeFormat(string setresult)
        {
            StringBuilder traceLog = new StringBuilder();
            string setresultvalue = string.Empty;
            try
            {
                traceLog.AppendLine("Start: formatSetTimeFormat()---- " + setresult);
                if (!string.IsNullOrEmpty(setresult))
                {
                    string[] variableValueWithMS = setresult.Split(new char[1] { '.' });
                    if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                    {
                        if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                        {
                            char[] splitChar = { ':' };
                            string[] spliResult = variableValueWithMS[0].Split(splitChar);
                            setresultvalue = spliResult.Length > 2 ? spliResult[1] + ConstantHelper.constColon + spliResult[2] : ConstantHelper.constTimeMMSSFormat;
                        }
                        else
                        {
                            setresultvalue = ConstantHelper.constTimeMMSSFormat;
                        }
                    }
                }
                return setresultvalue;
            }
            finally
            {
                traceLog.AppendLine("End  formatSetTimeFormat : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Function to get team challenge list
        /// </summary>
        /// <returns>Total<List<MainChallengeVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/27/2015
        /// </devdoc>
        public static List<MainChallengeVM> GetTeamChallengeList(ref bool isTeamJoined)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeamChallengeList---- " + DateTime.Now.ToLongDateString());
                    int teamId = 0;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    teamId = CommonBL.GetTeamIdBasedOnUserCredId(cred.UserId, cred.UserType);
                    isTeamJoined = teamId > 0 ? true : false;
                    //Query to Get Team Challenge List on 08/18/2015
                    List<MainChallengeVM> resultList = (from uc in dataContext.UserChallenge
                                                        join ttm in dataContext.TrainerTeamMembers on uc.UserId equals ttm.UserId into userchallgegrp
                                                        join chlng in dataContext.Challenge on uc.ChallengeId equals chlng.ChallengeId
                                                        join ct in dataContext.ChallengeType on chlng.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        from usrchnge in userchallgegrp.DefaultIfEmpty()
                                                        where chlng.IsActive && (usrchnge.TeamId == teamId)
                                                        //orderby chlng.CreatedDate descending
                                                        orderby uc.AcceptedDate descending
                                                        select new MainChallengeVM
                                                        {
                                                            ChallengeId = chlng.ChallengeId,
                                                            ChallengeName = chlng.ChallengeName,
                                                            DifficultyLevel = chlng.DifficultyLevel,
                                                            ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                            ChallengeType = ct.ChallengeType,
                                                            IsSubscription = chlng.IsSubscription,
                                                            TempEquipments = (from eqip in dataContext.ChallengeEquipmentAssociations
                                                                              join bp in dataContext.Equipments
                                                                              on eqip.EquipmentId equals bp.EquipmentId
                                                                              where eqip.ChallengeId == chlng.ChallengeId
                                                                              select bp.Equipment).Distinct().ToList<string>(),
                                                            TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                              join bp in dataContext.BodyPart
                                                                              on trzone.PartId equals bp.PartId
                                                                              where trzone.ChallengeId == chlng.ChallengeId
                                                                              select bp.PartName).Distinct().ToList<string>(),
                                                            Strenght = dataContext.UserChallenge.Where(uchlng => uchlng.ChallengeId == chlng.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                            ResultUnit = ct.ResultUnit
                                                        }).ToList();
                    resultList.ForEach(r =>
                    {
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                        {
                            r.TargetZone = string.Join(", ", r.TempTargetZone);
                        }
                        r.TempTargetZone = null;
                        if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                        {
                            r.Equipment = string.Join(", ", r.TempEquipments);
                        }
                        r.TempEquipments = null;
                        r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                    });
                    if (resultList != null)
                    {
                        resultList = resultList.GroupBy(rl => rl.ChallengeId).Select(grp => grp.FirstOrDefault()).ToList();
                    }
                    return resultList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTeamChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get User Completed ChallengeList
        /// </summary>
        /// <param name="cred"></param>
        /// <returns></returns>
        public static CompletedChallengesWithPendingVM GetUserCompletedChallengeList(Credentials cred)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog.AppendLine("Start: GetUserCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                try
                {
                    return GetCompletedChallengeList(cred, dataContext);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetUserCompletedChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get completed challenge list
        /// </summary>
        /// <returns>List<MainChallengeVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/23/2015
        /// </devdoc>       
        public static CompletedChallengesWithPendingVM GetCompletedChallengeList(Credentials cred, LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            CompletedChallengesWithPendingVM objCompletedChallengesWithPendingVM = null;
            try
            {
                traceLog.AppendLine("Start: GetCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                objCompletedChallengesWithPendingVM = new CompletedChallengesWithPendingVM();
                //Query to get completed challenge list
                List<CompletedChallengeVM> resultQuery = (from uc in dataContext.UserChallenge
                                                          join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                          join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                          orderby uc.AcceptedDate descending
                                                          where uc.UserId == cred.Id && (uc.Result != null || uc.Fraction != null) && c.IsActive
                                                          select new CompletedChallengeVM
                                                          {
                                                              ChallengeId = uc.ChallengeId,
                                                              ChallengeName = c.ChallengeName,
                                                              ChallengeType = ct.ChallengeType,
                                                              IsActive = c.IsActive,
                                                              DifficultyLevel = c.DifficultyLevel,
                                                              ChallengeSubTypeId = ct.ChallengeSubTypeId
                                                          }).ToList<CompletedChallengeVM>();
                //Query to get completed challenge list order by ChallengeID, ChallengeName, ChallengeType and IsActive
                List<CompletedChallengeVM> resultList = (from q in resultQuery
                                                         group q by new
                                                         {
                                                             ChallangeID = q.ChallengeId,
                                                             ChallengeName = q.ChallengeName,
                                                             ChallengeType = q.ChallengeType,
                                                             IsActive = q.IsActive,
                                                             //AcceptedDate=uc.AcceptedDate
                                                             DifficultyLevel = q.DifficultyLevel,
                                                             ChallengeSubTypeId = q.ChallengeSubTypeId
                                                         } into userChlng
                                                         select new CompletedChallengeVM
                                                         {
                                                             ChallengeId = userChlng.Key.ChallangeID,
                                                             ChallengeName = userChlng.Key.ChallengeName,
                                                             ChallengeType = userChlng.Key.ChallengeType,
                                                             IsActive = userChlng.Key.IsActive,
                                                             DifficultyLevel = userChlng.Key.DifficultyLevel,
                                                             ChallengeSubTypeId = userChlng.Key.ChallengeSubTypeId
                                                         }).ToList<CompletedChallengeVM>();

                resultList.ForEach(r =>
                {
                    r.ChallengeType = string.IsNullOrEmpty(r.ChallengeType) ? string.Empty : r.ChallengeType.Split(' ')[0];
                    r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                    if (!string.IsNullOrEmpty(r.ChallengeType) && r.ChallengeType != ConstantHelper.constProgram)
                    {
                        PersonalChallengeVM personalbestresult = TeamBL.GetGlobalPersonalBestResult(r.ChallengeId, cred.Id, dataContext);
                        if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                        {
                            r.PersonalBestResult = personalbestresult.Result.Trim();
                        }
                        else
                        {
                            r.PersonalBestResult = string.Empty;
                        }
                    }
                    else
                    {
                        r.PersonalBestResult = string.Empty;
                    }
                });
                objCompletedChallengesWithPendingVM.CompletedChallengesList = resultList;
                objCompletedChallengesWithPendingVM.TotalPendingChallengeList = PushNotificationBL.GetUserTotalFriendPendingChallenges(cred.Id);
                return objCompletedChallengesWithPendingVM;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);

            }
        }
        /// <summary>
        /// Function to get completed challenge list by user id and user type
        /// </summary>
        /// <returns>List<MainChallengeVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 08/07/2015
        /// </devdoc>
        public static List<CompletedChallengeVM> GetCompletedChallengeList(UserIdAndUserType model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetCompletedChallengeList for specific userId and userType in BL---- " + DateTime.Now.ToLongDateString());

                    //Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblCredentials cred = dataContext.Credentials.FirstOrDefault(crd => crd.UserId == model.UserId && crd.UserType == model.UserType);
                    //Query to get completed challenge list
                    List<CompletedChallengeVM> resultQuery = (from uc in dataContext.UserChallenge
                                                              join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                              join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                              orderby uc.AcceptedDate descending
                                                              where uc.UserId == cred.Id && (uc.Result != null || uc.Fraction != null)
                                                              select new CompletedChallengeVM
                                                              {
                                                                  ChallengeId = uc.ChallengeId,
                                                                  ChallengeName = c.ChallengeName,
                                                                  ChallengeType = ct.ChallengeType,
                                                                  IsActive = c.IsActive,
                                                                  DifficultyLevel = c.DifficultyLevel,
                                                                  ChallengeSubTypeId = ct.ChallengeSubTypeId
                                                              }).ToList<CompletedChallengeVM>();
                    //Query to get completed challenge list order by ChallengeID, ChallengeName, ChallengeType and IsActive
                    List<CompletedChallengeVM> resultList = (from q in resultQuery
                                                             group q by new
                                                             {
                                                                 ChallangeID = q.ChallengeId,
                                                                 ChallengeName = q.ChallengeName,
                                                                 ChallengeType = q.ChallengeType,
                                                                 IsActive = q.IsActive,
                                                                 DifficultyLevel = q.DifficultyLevel,
                                                                 ChallengeSubTypeId = q.ChallengeSubTypeId
                                                                 //AcceptedDate=uc.AcceptedDate
                                                             } into userChlng
                                                             select new CompletedChallengeVM
                                                             {
                                                                 ChallengeId = userChlng.Key.ChallangeID,
                                                                 ChallengeName = userChlng.Key.ChallengeName,
                                                                 ChallengeType = userChlng.Key.ChallengeType,
                                                                 IsActive = userChlng.Key.IsActive,
                                                                 DifficultyLevel = userChlng.Key.DifficultyLevel,
                                                                 ChallengeSubTypeId = userChlng.Key.ChallengeSubTypeId
                                                                 //AcceptedDate=userChlng.Key.AcceptedDate
                                                             }).ToList<CompletedChallengeVM>();


                    resultList.ForEach(r =>
                    {
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType);
                        if (r.ChallengeType != ConstantHelper.constProgram)
                        {
                            PersonalChallengeVM personalbestresult = TeamBL.GetTeamBestResultByChallenge(r.ChallengeId, dataContext);
                            if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                            {
                                r.PersonalBestResult = personalbestresult.Result.Trim();
                            }
                        }
                        else
                        {
                            r.PersonalBestResult = string.Empty;
                        }

                    });
                    return resultList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetCompletedChallengeList for specific userId and userType in BL: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get TotalCompletedChallenegList based on profile
        /// </summary>
        /// <param name="credID"></param>
        /// <returns></returns>
        public static int GetTotalCompletedChallenegList(int credID)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                int totalCompletedChallengeList = 0;
                try
                {
                    traceLog.AppendLine("Start: GetTotalCompletedChallenegList in BL---- " + credID);
                    var completedChallengeList = (from uc in dataContext.UserChallenge
                                                  join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                  join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                  where uc.UserId == credID && (uc.Result != null || uc.Fraction != null)
                                                  select uc).ToList();
                    if (completedChallengeList != null)
                    {
                        totalCompletedChallengeList = completedChallengeList.Count;
                    }
                    return totalCompletedChallengeList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTotalCompletedChallenegList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get all filter challenges parameters
        /// </summary>
        /// <returns>AllChallengeFilterParameterList</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/14/2015
        /// </devdoc>
        public static AllChallengeFilterParameterList GetChallengeFilterParameter()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeFilterParameter in BL---- " + DateTime.Now.ToLongDateString());
                    AllChallengeFilterParameterList objCFP = new AllChallengeFilterParameterList();
                    objCFP.Types = dataContext.ChallengeType.Select(t => t.ChallengeType).Distinct().ToList();
                    List<string> objTypeList = new List<string>();
                    objCFP.Types.ForEach(t =>
                    {
                        objTypeList.Add(t.Split(' ')[0]);
                    });
                    objCFP.Types = objTypeList;
                    objCFP.Types.Insert(0, Message.All);
                    objCFP.Difficulties = dataContext.Difficulties.Select(d => d.Difficulty).ToList();
                    objCFP.Difficulties.Insert(0, Message.All);
                    objCFP.BodyZones = dataContext.BodyPart.Where(bp => bp.IsShownHoneyComb).Select(b => b.PartName).ToList();
                    objCFP.BodyZones.Insert(0, Message.All);
                    objCFP.Equipments = dataContext.Equipments.Select(e => e.Equipment).ToList();
                    objCFP.Equipments.Insert(0, Message.All);
                    objCFP.ExerciseTypes = dataContext.ExerciseTypes.Select(e => e.ExerciseName).ToList();
                    objCFP.ExerciseTypes.Insert(0, Message.All);
                    return objCFP;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetChallengeFilterParameter in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get personal completed challenge list
        /// </summary>
        /// <returns>List<PersonalChallengeVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/25/2015
        /// </devdoc>
        public static List<PersonalChallengeVM> GetPersonalCompletedChallengeList(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetPersonalCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    string challengeType = ConstantHelper.FreeFormChallengeType;
                    List<PersonalChallengeVM> resultList = new List<PersonalChallengeVM>();
                    resultList = (from uc in dataContext.UserChallenge
                                  join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                  join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                  where uc.ChallengeId == challengeId && uc.UserId == cred.Id && (uc.Result != null || uc.Fraction != null)
                                  && ct.ChallengeType != challengeType && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                  select new PersonalChallengeVM
                                  {
                                      UserChallengeId = uc.Id,
                                      Result = uc.Result,
                                      Fraction = uc.Fraction,
                                      CompletedDateDb = uc.AcceptedDate,
                                      ChallengeSubTypeid = ct.ChallengeSubTypeId,
                                      ResultUnit = ct.ResultUnit,
                                      ResultUnitSuffix = uc.ResultUnit,
                                      ChallengeType = ct.ChallengeType,
                                  }).ToList();
                    if (resultList != null && resultList.Count > 0)
                    {
                        resultList.ForEach(r =>
                        {
                            r.CompletedDate = r.CompletedDateDb.Day + " " +
                            CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(r.CompletedDateDb.Month) + " " +
                            r.CompletedDateDb.ToString("yy");
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                            r.IsWellness = (r.ChallengeSubTypeid == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                        });
                        //////////////////////////////////ORDER Implementation//////////////////////////////////////////////////////
                        var resultMethoddata = (from c in dataContext.Challenge
                                                join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                where c.ChallengeId == challengeId
                                                select new { ct.ResultUnit, ct.ChallengeSubTypeId }).FirstOrDefault();
                        string resultMethod = string.Empty;
                        if (resultMethod != null && !string.IsNullOrEmpty(resultMethoddata.ResultUnit))
                        {
                            resultMethod = resultMethoddata.ResultUnit.Trim();
                            switch (resultMethod)
                            {
                                case ConstantHelper.constTime:
                                    resultList.ForEach(r =>
                                    {

                                        r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                        //Code for HH:MM:SS And MM:SS format
                                        string tempResult = r.Result;
                                        char[] splitChar = { ':' };
                                        string[] spliResult = tempResult.Split(splitChar);
                                        if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                        {
                                            r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                        }
                                        else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                        {
                                            r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                        }

                                    });
                                    resultList = resultMethoddata.ChallengeSubTypeId == 6 ?
                                             resultList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                             : resultList.OrderBy(k => k.TempOrderIntValue).ToList();

                                    //resultList = resultList.OrderBy(k => k.TempOrderIntValue).ToList();
                                    PersonalChallengeVM topPC = resultList.FirstOrDefault();
                                    resultList = resultList.OrderByDescending(r => r.CompletedDateDb).Select(res => res).ToList();
                                    resultList.Insert(0, topPC);
                                    break;
                                case ConstantHelper.constReps:
                                case ConstantHelper.constWeight:
                                case ConstantHelper.constDistance:
                                    resultList.ForEach(r =>
                                    {
                                        r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : r.Result.Replace(",", string.Empty);
                                        r.TempOrderIntValue = string.IsNullOrEmpty(r.Result) ? 0 : (float)Convert.ToDouble(r.Result);
                                    });
                                    resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    topPC = resultList.FirstOrDefault();
                                    resultList = resultList.OrderByDescending(r => r.CompletedDateDb).Select(res => res).ToList();
                                    resultList.Insert(0, topPC);
                                    break;
                                case ConstantHelper.conRounds:
                                case ConstantHelper.constInterval:
                                    resultList.ForEach(r =>
                                    {
                                        if (!string.IsNullOrEmpty(r.Fraction))
                                        {
                                            string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                            r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                        }
                                        else
                                        {
                                            r.TempOrderIntValue = (float)Convert.ToInt16(r.Result);
                                        }
                                    });
                                    resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                    topPC = resultList.FirstOrDefault();
                                    resultList = resultList.OrderByDescending(r => r.CompletedDateDb).Select(res => res).ToList();
                                    resultList.Insert(0, topPC);
                                    break;
                            }
                        }
                    }
                    return resultList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPersonalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get User Global Completed ChallengeList
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="cred"></param>
        /// <returns></returns>
        public static List<GlobalChallengeVM> GetUserGlobalCompletedChallengeList(int challengeId, Credentials cred)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                traceLog.AppendLine("Start: GetUserGlobalCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                try
                {
                    return GetGlobalCompletedChallengeList(_dbContext, challengeId, cred);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetUserGlobalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get global completed challenge list
        /// </summary>
        /// <returns>List<GlobalChallengeVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/28/2015
        /// </devdoc>
        public static List<GlobalChallengeVM> GetGlobalCompletedChallengeList(LinksMediaContext _dbContext, int challengeId, Credentials cred)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetGlobalCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                string challengeType = ConstantHelper.FreeFormChallengeType;
                //Query to get all Global Completed Challenge Challenge List
                List<GlobalChallengeVM> resultList = (from uc in _dbContext.UserChallenge
                                                      join chlng in _dbContext.Challenge on uc.ChallengeId equals chlng.ChallengeId
                                                      join ct in _dbContext.ChallengeType on chlng.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                      join crd in _dbContext.Credentials on uc.UserId equals crd.Id
                                                      where uc.ChallengeId == challengeId && uc.IsGlobal && (uc.Result != null || uc.Fraction != null)
                                                      && ct.ChallengeType != challengeType && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                                      select new GlobalChallengeVM
                                                      {
                                                          ResultId = uc.Id,
                                                          ChallengeId = uc.ChallengeId,
                                                          ChallengeType = ct.ChallengeType,
                                                          Result = uc.Result,
                                                          Fraction = uc.Fraction,
                                                          UserCredId = uc.UserId,
                                                          UserId = crd.UserId,
                                                          UserType = crd.UserType,
                                                          ChallengeSubTypeId = chlng.ChallengeSubTypeId,
                                                          ResultUnit = ct.ResultUnit,
                                                          ResultUnitSuffix = uc.ResultUnit,
                                                          TempGlobalResultFilterIntValue = chlng.GlobalResultFilterValue,
                                                          BoomsCount = _dbContext.ResultBooms.Count(b => b.ResultId == uc.Id),
                                                          CommentsCount = _dbContext.ResultComments.Count(cmnt => cmnt.Id == uc.Id),
                                                          IsLoginUserBoom = _dbContext.ResultBooms.Where(bm => bm.ResultId == uc.Id).Any(b => b.BoomedBy == cred.Id),
                                                          IsLoginUserComment = _dbContext.ResultComments.Where(cm => cm.Id == uc.Id).Any(b => b.CommentedBy == cred.Id),
                                                      }).ToList<GlobalChallengeVM>();
                var resultMethod = (from c in _dbContext.Challenge
                                    join ct in _dbContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                    where c.ChallengeId == challengeId
                                    select new { ct.ResultUnit, ct.ChallengeSubTypeId }).FirstOrDefault();
                if (resultMethod != null && !string.IsNullOrEmpty(resultMethod.ResultUnit))
                {
                    //Ordering result cooresponding to ResultUnit
                    switch (resultMethod.ResultUnit.Trim())
                    {
                        case ConstantHelper.constTime:
                            foreach (var r in resultList.ToList())
                            {
                                bool isValidGlobalResult = true;
                                r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                float tempGlobalResultFilterIntValue = string.IsNullOrEmpty(r.TempGlobalResultFilterIntValue) ? 0 :
                                    Convert.ToInt32(r.TempGlobalResultFilterIntValue.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                if (resultMethod.ChallengeSubTypeId == 6)
                                {
                                    if (tempGlobalResultFilterIntValue > 0 && r.TempOrderIntValue > tempGlobalResultFilterIntValue)
                                    {
                                        isValidGlobalResult = false;
                                    }
                                }
                                else
                                {
                                    if (tempGlobalResultFilterIntValue > 0 && tempGlobalResultFilterIntValue > r.TempOrderIntValue)
                                    {
                                        isValidGlobalResult = false;
                                    }
                                }
                                if (isValidGlobalResult)
                                {
                                    //Code for HH:MM:SS And MM:SS format
                                    string tempResult = r.Result;
                                    char[] splitChar = { ':' };
                                    string[] spliResult = tempResult.Split(splitChar);
                                    if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                    {
                                        r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                    }
                                    else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                    {
                                        r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                    }
                                }
                                else
                                {
                                    var removedata = resultList.Where(item => item.ResultId == r.ResultId).FirstOrDefault();
                                    resultList.Remove(removedata);
                                }
                            }
                            resultList = resultMethod.ChallengeSubTypeId == 6 ?
                                         resultList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                         : resultList.OrderBy(k => k.TempOrderIntValue).ToList();
                            //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                            resultList = resultList.GroupBy(result => new { result.UserId, result.UserType }).Select(res => res.FirstOrDefault()).ToList();
                            break;
                        case ConstantHelper.constReps:
                        case ConstantHelper.constWeight:
                        case ConstantHelper.constDistance:
                            foreach (var r in resultList.ToList())
                            {
                                r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : r.Result.Replace(",", string.Empty);
                                r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToDouble(r.Result);
                                float tempGlobalResultFilterIntValue = string.IsNullOrEmpty(r.TempGlobalResultFilterIntValue) ? 0 :
                                    Convert.ToInt32(r.TempGlobalResultFilterIntValue.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                if (tempGlobalResultFilterIntValue > 0 && r.TempOrderIntValue > tempGlobalResultFilterIntValue)
                                {
                                    var removedata = resultList.Where(item => item.ResultId == r.ResultId).FirstOrDefault();
                                    resultList.Remove(removedata);
                                }
                            }
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            resultList = resultList.GroupBy(result => new { result.UserId, result.UserType }).Select(res => res.FirstOrDefault()).ToList();
                            break;
                        case ConstantHelper.conRounds:
                        case ConstantHelper.constInterval:
                            foreach (var r in resultList.ToList())
                            {
                                if (!string.IsNullOrEmpty(r.Fraction))
                                {
                                    string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                    r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                }
                                else
                                {
                                    r.TempOrderIntValue = (float)Convert.ToInt16(r.Result);
                                }
                                float tempGlobalResultFilterIntValue = string.IsNullOrEmpty(r.TempGlobalResultFilterIntValue) ? 0 :
                                    Convert.ToInt32(r.TempGlobalResultFilterIntValue.Replace(ConstantHelper.constColon, string.Empty).Replace(".", string.Empty));
                                if (tempGlobalResultFilterIntValue > 0 && r.TempOrderIntValue > tempGlobalResultFilterIntValue)
                                {
                                    var removedata = resultList.Where(item => item.ResultId == r.ResultId).FirstOrDefault();
                                    resultList.Remove(removedata);
                                }
                            }
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            resultList = resultList.GroupBy(result => new { result.UserId, result.UserType }).Select(res => res.FirstOrDefault()).ToList();
                            break;
                    }
                }
                resultList.ForEach(r =>
                {
                    r.ChallengeType = r.ChallengeType.Split(' ')[0];
                    r.IsWellness = (r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                    if (r.UserType.Equals(Message.UserTypeUser))
                    {
                        var user = _dbContext.User.FirstOrDefault(u => u.UserId == r.UserId);
                        if (user != null)
                        {
                            r.UserName = user.FirstName + " " + user.LastName;
                            r.ImageUrl = (!string.IsNullOrEmpty(user.UserImageUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.UserImageUrl))) ?
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl : string.Empty;
                            r.Address = user.City + ", " + user.State;
                        }
                    }
                    else if (r.UserType.Equals(Message.UserTypeTrainer))
                    {
                        var trainer = (from t in _dbContext.Trainer
                                       join c in _dbContext.Cities on t.City equals c.CityId
                                       where t.TrainerId == r.UserId
                                       select new
                                       {
                                           t.FirstName,
                                           t.LastName,
                                           t.TrainerImageUrl,
                                           t.State,
                                           c.CityName
                                       }).FirstOrDefault();
                        if (trainer != null)
                        {
                            r.UserName = trainer.FirstName + " " + trainer.LastName;
                            r.ImageUrl = (!string.IsNullOrEmpty(trainer.TrainerImageUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + trainer.TrainerImageUrl))) ?
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl : string.Empty;
                            r.Address = trainer.CityName + ", " + trainer.State;
                        }
                    }
                    r.Rank = resultList.IndexOf(r) + 1;
                    r.SuperScript = GetOrdinal(r.Rank);
                });
                //Insert Global Rank of the User at First position
                if (resultList.IndexOf(resultList.FirstOrDefault(rl => rl.UserId == cred.UserId && rl.UserType == cred.UserType)) > 0)
                {
                    resultList.Insert(0, resultList.FirstOrDefault(rl => rl.UserId == cred.UserId && rl.UserType == cred.UserType));
                }
                return resultList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetGlobalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Filter GlobalCompletedChallengeList based on search Cateria
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="trainerID"></param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
        public static List<GlobalChallengeVM> GetFilterGlobalCompletedChallengeList(int challengeId, string searchBy)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                int currentUserTeamID = 0;
                try
                {
                    traceLog.AppendLine("Start: GetFilterGlobalCompletedChallengeList in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var userGlobalCompletedChallengeList = GetGlobalCompletedChallengeList(_dbContext, challengeId, cred);
                    if (userGlobalCompletedChallengeList != null)
                    {
                        userGlobalCompletedChallengeList = (from gr in userGlobalCompletedChallengeList
                                                            select new GlobalChallengeVM
                                                            {
                                                                ResultId = gr.ResultId,
                                                                Result = gr.Result,
                                                                Fraction = gr.Fraction,
                                                                UserId = gr.UserId,
                                                                UserType = gr.UserType,
                                                                ChallengeSubTypeId = gr.ChallengeSubTypeId,
                                                                ResultUnit = gr.ResultUnit,
                                                                ResultUnitSuffix = gr.ResultUnit,
                                                                Rank = gr.Rank,
                                                                SuperScript = gr.SuperScript,
                                                                ImageUrl = gr.ImageUrl,
                                                                UserName = gr.UserName,
                                                                Address = gr.Address,
                                                                TempOrderIntValue = gr.TempOrderIntValue,
                                                                ResultTrainerId = gr.UserType.Equals(Message.UserTypeTrainer) ? _dbContext.Trainer.Where(u => u.TrainerId == gr.UserId).FirstOrDefault().TeamId :
                                                                gr.UserType.Equals(Message.UserTypeUser) ? _dbContext.User.Where(u => u.UserId == gr.UserId).FirstOrDefault().TeamId : 0
                                                            }).ToList();
                    }
                    if (!searchBy.Equals(Message.All, StringComparison.OrdinalIgnoreCase))
                    {
                        // Find the team trainer ID 
                        if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var trainerdetail = _dbContext.Trainer.FirstOrDefault(u => u.TrainerId == cred.UserId);
                            if (trainerdetail != null)
                            {
                                currentUserTeamID = trainerdetail.TeamId;
                            }
                        }
                        else if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var userdetails = _dbContext.User.FirstOrDefault(u => u.UserId == cred.UserId);
                            if (userdetails != null)
                            {
                                currentUserTeamID = userdetails.TeamId;
                            }
                        }
                        // If usec  have team then filter by team
                        if (currentUserTeamID > 0)
                        {
                            if (userGlobalCompletedChallengeList != null)
                            {
                                userGlobalCompletedChallengeList = userGlobalCompletedChallengeList.Where(ugl => ugl.ResultTrainerId == currentUserTeamID).ToList();
                            }
                        }
                        else
                        {
                            userGlobalCompletedChallengeList = null;
                        }
                    }
                    return userGlobalCompletedChallengeList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetFilterGlobalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get ordinal of a number
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/05/2015
        /// </devdoc>
        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    //return num + "th";
                    return "th";
            }

            switch (num % 10)
            {
                case 1:
                    //return num + "st";
                    return "st";
                case 2:
                    //return num + "nd";
                    return "nd";
                case 3:
                    //return num + "rd";
                    return "rd";
                default:
                    //return num + "th";
                    return "th";
            }
        }

        /// <summary>
        /// Function to Accept Challenge
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/21/2015
        /// </devdoc>
        public static int AcceptChallenge(int challengeId, Credentials cred)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: AcceptChallenge in BL---- " + DateTime.Now.ToLongDateString());                    
                    tblUserChallenges objUserChallenge = new tblUserChallenges();
                    objUserChallenge.AcceptedDate = DateTime.Now;
                    objUserChallenge.ChallengeId = challengeId;
                    objUserChallenge.CreatedBy = cred.Id;
                    objUserChallenge.CreatedDate = DateTime.Now;
                    objUserChallenge.ModifiedBy = cred.Id;
                    objUserChallenge.ModifiedDate = DateTime.Now;
                    objUserChallenge.UserId = cred.Id;
                    dataContext.UserChallenge.Add(objUserChallenge);                    
                    dataContext.SaveChanges();
                    //AppSubscriptionVM appSubscriptionInfo = new AppSubscriptionVM();
                    //bool isChallengeSubcribtion = dataContext.Challenge.Where(ch => ch.ChallengeId == challengeId).Select(c => c.IsSubscription).FirstOrDefault(); 
                    //if(isChallengeSubcribtion)
                    //{
                    //    appSubscriptionInfo.IsSubscriptionStatus = isChallengeSubcribtion;
                    //    appSubscriptionInfo.DeviceId = cred.DeviceID;
                    //    DeviceType deviceType;
                    //   if(!Enum.TryParse(cred.DeviceType,out deviceType))
                    //    {
                    //        deviceType = DeviceType.None;
                    //    }
                    //    appSubscriptionInfo.DeviceType = deviceType;                   
                    //    UpdateAppSubscription(appSubscriptionInfo, cred);
                    //}                   
                   
                    return objUserChallenge.Id;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  AcceptChallenge in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }       
        /// <summary>
        /// Decline Challenge 
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="cred"></param>
        /// <returns></returns>
        public static bool DeclineChallengeOrProgram(int challengeId, Credentials cred, bool isProgram)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeclineChallenge in BL---- " + DateTime.Now.ToLongDateString());
                    //Code For Remove Friend Challenges     
                    if (dataContext.ChallengeToFriends.Any(ctf => ctf.ChallengeId == challengeId && ctf.TargetId == cred.Id && ctf.IsProgram == isProgram))
                    {
                        List<tblChallengeToFriend> objCTFList = dataContext.ChallengeToFriends.Where(ctf => ctf.ChallengeId == challengeId && ctf.TargetId == cred.Id && ctf.IsProgram == isProgram).ToList();
                        objCTFList.ForEach(cf =>
                        {
                            cf.IsActive = false;
                        });
                        dataContext.SaveChanges();
                    }
                    //Code for Remove User Assignment list
                    if (dataContext.UserAssignments.Any(ctf => ctf.ChallengeId == challengeId && ctf.TargetId == cred.Id && ctf.IsProgram == isProgram))
                    {
                        List<tblUserAssignmentByTrainer> objUserAssignmentsList = dataContext.UserAssignments.Where(ctf => ctf.ChallengeId == challengeId && ctf.TargetId == cred.Id && ctf.IsProgram == isProgram).ToList();
                        objUserAssignmentsList.ForEach(cf =>
                        {
                            cf.IsActive = false;
                        });
                        dataContext.SaveChanges();
                    }

                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End:DeclineChallenge in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to submit Result
        /// </summary>
        /// <returns>int(MessageId)</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/21/2015
        /// </devdoc>
        public static MessageStreamIdVM SubmitChallengeResult(SubmitResultVM model, int userChallengeId, Credentials cred)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                MessageStreamIdVM objMessageStreamIdVM = null;
                try
                {
                    traceLog.AppendLine("Start: SubmitResult in BL---- " + DateTime.Now.ToLongDateString());
                    // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblMessageStream objMessageStream = new tblMessageStream();
                    objMessageStream.MessageType = Message.TextMessageType;
                    objMessageStream.PostedDate = DateTime.Now;
                    objMessageStream.SubjectId = cred.Id;
                    objMessageStream.TargetId = model.ChallengeId;
                    objMessageStream.TargetType = Message.ChallengeTargetType;
                    objMessageStream.Content = model.MessageText;
                    objMessageStream.IsTextImageVideo = model.IsTextImageVideo;
                    objMessageStream.IsImageVideo = model.IsImageVideo;
                    objMessageStream.IsNewsFeedHide = !model.IsNewsFeed;
                    dataContext.MessageStraems.Add(objMessageStream);
                    dataContext.SaveChanges();
                    tblUserChallenges objUserChallenge = dataContext.UserChallenge.FirstOrDefault(uc => uc.Id == userChallengeId);
                    if (objUserChallenge != null)
                    {
                        objUserChallenge.Result = string.IsNullOrEmpty(model.ResultValue) ? string.Empty : model.ResultValue.Replace(",", string.Empty);
                        objUserChallenge.Fraction = string.IsNullOrEmpty(model.FractionValue) ? null : model.FractionValue;
                        objUserChallenge.IsGlobal = model.IsGlobal;
                        objUserChallenge.IsNewsFeed = model.IsNewsFeed;
                        dataContext.SaveChanges();
                        //Code For Remove Pending Challenge                       
                        List<tblChallengeToFriend> objCTFList = dataContext.ChallengeToFriends.Where(ctf => ctf.ChallengeId == objUserChallenge.ChallengeId && ctf.TargetId == cred.Id && ctf.IsPending).ToList();
                        foreach (var item in objCTFList)
                        {
                            item.IsPending = false;
                            item.IsActive = true;
                        }
                        dataContext.SaveChanges();

                        //Code for Remove user assignment list
                        if (dataContext.UserAssignments.Any(ctf => ctf.ChallengeId == objUserChallenge.ChallengeId && ctf.TargetId == cred.Id))
                        {
                            List<tblUserAssignmentByTrainer> objUserAssignmentsList = dataContext.UserAssignments.Where(ctf => ctf.ChallengeId == objUserChallenge.ChallengeId && ctf.TargetId == cred.Id).ToList();
                            foreach (var item in objUserAssignmentsList)
                            {
                                item.IsCompleted = true;
                                item.IsActive = true;
                                item.IsRead = false;
                                item.ChallengeCompletedDate = DateTime.Now;
                            }
                            dataContext.SaveChanges();
                            if (objUserAssignmentsList != null)
                            {
                                objUserAssignmentsList = objUserAssignmentsList.OrderByDescending(asg => asg.UserAssignmentId).GroupBy(usr => new { usr.ChallengeId, usr.TargetId }).Select(uu => uu.FirstOrDefault()).ToList();
                            }
                            //when user completes a workout or fitness test
                            foreach (tblUserAssignmentByTrainer item in objUserAssignmentsList)
                            {
                                int challengeId = item.ChallengeId;
                                int receiverId = item.SubjectId;
                                string result = string.IsNullOrEmpty(item.Result) ? string.Empty : item.Result.Replace(",", string.Empty);
                                //item.Result ?? string.Empty;
                                string fraction = item.Fraction ?? string.Empty;
                                if (userChallengeId == 0)
                                {
                                    userChallengeId = dataContext.UserChallenge.Where(uc => uc.ChallengeId == challengeId && uc.UserId == cred.Id && uc.Result == result && (uc.Fraction == null
                                        || uc.Fraction == fraction)).OrderByDescending(uc => uc.AcceptedDate).Select(uc => uc.Id).FirstOrDefault();
                                }
                                byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
                                if (challengeId > 0)
                                {
                                    new Thread(() =>
                                    {
                                        Thread.CurrentThread.IsBackground = false;
                                        SendReceiverNotificationToUserOrTrainer(challengeId, receiverId, string.Empty, cred.UserId, cred.UserType, certificate, userChallengeId);
                                    }).Start();
                                }
                            }
                        }
                    }
                    objMessageStreamIdVM = new MessageStreamIdVM();
                    objMessageStreamIdVM.MessageStreamId = objMessageStream.MessageStraemId;
                    objMessageStreamIdVM.UserChallengeId = userChallengeId;
                    objMessageStreamIdVM.TotalPendingChallenges = PushNotificationBL.GetUserTotalFriendPendingChallenges(cred.Id);
                    return objMessageStreamIdVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  SubmitResult in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Send Receiver Notification To UserOrTrainer
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="credUserId"></param>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        /// <param name="userType"></param>
        /// <param name="certificate"></param>
        /// <param name="targetchallenegID"></param>
        public static void SendReceiverNotificationToUserOrTrainer(int challengeId, int credUserId, string name, int userID, string userType, byte[] certificate, int targetchallenegID = 0)
        {
            lock (syncLock)
            {
                StringBuilder traceLog = null;
                List<UserNotification> allactiveDevices = null;

                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        string notificationType = string.Empty;
                        bool isNTSent = false;
                        traceLog.AppendLine("Start: SendReceiverNotificationToUserOrTrainer()");
                        if (string.IsNullOrEmpty(name))
                        {
                            name = (from usr in dataContext.User
                                    where usr.UserId == userID
                                    select usr.FirstName + " " + usr.LastName
                                    ).FirstOrDefault();
                        }
                        var challengeDetail = dataContext.Challenge.Where(ctf => ctf.ChallengeId == challengeId && ctf.IsActive).FirstOrDefault();
                        if (challengeDetail != null)
                        {
                            int ret = 0;
                            switch (challengeDetail.ChallengeSubTypeId)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                    ret = 1;
                                    break;
                                case 13:
                                case 14:
                                    ret = 14;
                                    break;
                                case 16:
                                    ret = 16;
                                    break;
                            }
                            if (ret == ConstantHelper.constFittnessCommonSubTypeId || ret == ConstantHelper.constWorkoutChallengeSubType)
                            {
                                allactiveDevices = PushNotificationBL.GetLastUserDeviceID(credUserId);
                                bool isSendtNotification = false;
                                bool isSaveSuccessfully = false;
                                bool isSendNT = false;
                                DateTime createdNotificationUtcDateTime = DateTime.UtcNow;
                                foreach (UserNotification objuserNotification in allactiveDevices)
                                {
                                    // If Deives Token null then by pass to sending push notification
                                    if (!string.IsNullOrEmpty(objuserNotification.DeviceID))
                                    {
                                        notificationType = NotificationType.UserSentToReceiver.ToString();
                                        isSendNT = true;
                                        if (isSendNT)
                                        {
                                            if (!isSaveSuccessfully)
                                            {
                                                UserNotificationsVM objNT = new UserNotificationsVM();
                                                objNT.UserID = userID;
                                                objNT.UserType = userType;
                                                objNT.ReceiverCredID = credUserId;
                                                objNT.NotificationType = notificationType;
                                                objNT.SenderUserName = name;
                                                objNT.TokenDevicesID = objuserNotification.DeviceID;
                                                objNT.TargetPostID = targetchallenegID;
                                                objNT.CreatedNotificationUtcDateTime = createdNotificationUtcDateTime;
                                                CommonWebApiBL.SaveDevicesNotification(objNT);
                                                isSaveSuccessfully = true;
                                            }
                                            isNTSent = ChallengeToFriendBL.SendNotificationToUser(credUserId, name, notificationType, objuserNotification.DeviceID, objuserNotification.DeviceType, certificate, userID);
                                            if (isNTSent)
                                            {
                                                if (!isSendtNotification)
                                                {
                                                    isSendtNotification = true;
                                                }
                                                isSaveSuccessfully = false;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogManagerInstance.WriteErrorLog(ex);
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SendReceiverNotificationToUserOrTrainer()  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                        allactiveDevices = null;
                    }
                }
            }
        }

        /// <summary>
        /// Share MessageToTeam
        /// </summary>
        /// <param name="userChallengeId"></param>
        /// <returns></returns>
        public static bool ShareMessageToTeam(int userChallengeId, string completedTextMessage)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: ShareMessageToTeam in BL---- " + DateTime.Now.ToLongDateString());
                    tblUserChallenges objUserChallenge = dataContext.UserChallenge.FirstOrDefault(uc => uc.Id == userChallengeId);
                    if (objUserChallenge != null)
                    {
                        objUserChallenge.CompletedTextMessage = completedTextMessage;
                        dataContext.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  ShareMessageToTeam in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update Program Workout by programWeekWorkoutId
        /// </summary>
        /// <param name="programWeekWorkoutId"></param>
        public static bool UpdateProgramWorkout(long programWeekWorkoutId, Credentials cred, int receiverUserChallengeId = 0)
        {
            StringBuilder traceLog = new StringBuilder();
            bool isAllWeekWorkoutsCompleted = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateProgramWorkout in BL----programWeekWorkoutId- " + Convert.ToString(programWeekWorkoutId));
                    //  Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (programWeekWorkoutId > 0)
                    {
                        long UserAcceptedProgramWeekId = 0;
                        tblUserAcceptedProgramWorkouts objUserAcceptedProgramWorkouts = dataContext.UserAcceptedProgramWorkouts.FirstOrDefault(uc => uc.PAcceptedWorkoutId == programWeekWorkoutId);
                        if (objUserAcceptedProgramWorkouts != null)
                        {
                            objUserAcceptedProgramWorkouts.IsCompleted = true;
                            objUserAcceptedProgramWorkouts.ChallengeDate = DateTime.Now;
                            UserAcceptedProgramWeekId = objUserAcceptedProgramWorkouts.UserAcceptedProgramWeekId;
                            dataContext.SaveChanges();
                            List<tblUserAssignmentByTrainer> challengeFreindList = dataContext.UserAssignments.Where(ctf => ctf.ChallengeId == objUserAcceptedProgramWorkouts.ProgramChallengeId
                                && ctf.TargetId == cred.Id  && !ctf.IsCompleted).ToList();
                            if (challengeFreindList != null)
                            {
                                if (challengeFreindList != null)
                                {
                                    challengeFreindList = challengeFreindList.OrderByDescending(asg => asg.UserAssignmentId).GroupBy(usr => new { usr.ChallengeId, usr.TargetId }).Select(uu => uu.FirstOrDefault()).ToList();
                                }
                                foreach (tblUserAssignmentByTrainer item in challengeFreindList)
                                {
                                    int challengeId = objUserAcceptedProgramWorkouts.WorkoutChallengeID;
                                    int receiverId = item.SubjectId;
                                    int userId = cred.UserId;
                                    string userType = cred.UserType;
                                    byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);
                                    string result = item.Result ?? string.Empty;
                                    string fraction = item.Fraction ?? string.Empty;
                                    if (receiverUserChallengeId == 0)
                                    {
                                        receiverUserChallengeId = dataContext.UserChallenge.Where(uc => uc.ChallengeId == challengeId && uc.UserId == cred.Id && uc.Result == result
                                            && (uc.Fraction == null || uc.Fraction == fraction)).OrderByDescending(uc => uc.AcceptedDate).Select(uc => uc.Id).FirstOrDefault();
                                    }
                                    if (challengeId > 0)
                                    {
                                        new Thread(() =>
                                        {
                                            Thread.CurrentThread.IsBackground = false;
                                            SendReceiverNotificationToUserOrTrainer(challengeId, receiverId, string.Empty, userId, userType, certificate, receiverUserChallengeId);

                                        }).Start();
                                    }
                                }
                            }
                        }
                        if (UserAcceptedProgramWeekId > 0)
                        {
                            int userAcceptedProgramId = (from pw in dataContext.UserAcceptedProgramWeeks
                                                         where pw.UserAcceptedProgramWeekId == UserAcceptedProgramWeekId
                                                         select pw.UserAcceptedProgramId).FirstOrDefault();
                            if (userAcceptedProgramId > 0)
                            {
                                int compltededaccptedId = 0;
                                compltededaccptedId = (from pw in dataContext.UserAcceptedProgramWeeks
                                                       join pww in dataContext.UserAcceptedProgramWorkouts
                                                       on pw.UserAcceptedProgramWeekId equals pww.UserAcceptedProgramWeekId
                                                       where pw.UserAcceptedProgramId == userAcceptedProgramId && !pww.IsCompleted
                                                       select pw.UserAcceptedProgramId).FirstOrDefault();
                                if (!(compltededaccptedId > 0))
                                {
                                    tblUserActivePrograms objtblUserActivePrograms = dataContext.UserActivePrograms.FirstOrDefault(ap => ap.UserAcceptedProgramId == userAcceptedProgramId);
                                    if (objtblUserActivePrograms != null)
                                    {
                                        objtblUserActivePrograms.IsCompleted = true;
                                        objtblUserActivePrograms.ModifiedDate = DateTime.Now;
                                        objtblUserActivePrograms.CompletedDate = DateTime.Now;
                                        dataContext.SaveChanges();
                                        tblUserChallenges objUserChallenge = new tblUserChallenges();
                                        objUserChallenge.AcceptedDate = DateTime.Now;
                                        objUserChallenge.ChallengeId = objtblUserActivePrograms.ProgramId;
                                        objUserChallenge.Result = string.Empty;
                                        objUserChallenge.Fraction = string.Empty;
                                        objUserChallenge.CreatedBy = cred.Id;
                                        objUserChallenge.CreatedDate = DateTime.Now;
                                        objUserChallenge.ModifiedBy = cred.Id;
                                        objUserChallenge.ModifiedDate = DateTime.Now;
                                        objUserChallenge.UserId = cred.Id;
                                        objUserChallenge.IsProgramchallenge = true;
                                        dataContext.UserChallenge.Add(objUserChallenge);
                                        dataContext.SaveChanges();
                                        isAllWeekWorkoutsCompleted = true;
                                        int completedProgramId = objtblUserActivePrograms.ProgramId;
                                        // Update When all workouts of program completed and remove the
                                        if (dataContext.UserAssignments.Any(ctf => ctf.ChallengeId == completedProgramId && ctf.TargetId == cred.Id ))
                                        {
                                            List<tblUserAssignmentByTrainer> objCTFList = dataContext.UserAssignments.Where(ctf => ctf.ChallengeId == completedProgramId && ctf.TargetId == cred.Id ).ToList();
                                            foreach (var item in objCTFList)
                                            {
                                                item.IsCompleted = true;
                                                item.IsActive = true;
                                                item.IsRead = false;
                                                item.ChallengeCompletedDate = DateTime.Now;
                                            }
                                            dataContext.SaveChanges();
                                        }
                                        if (dataContext.ChallengeToFriends.Any(ctf => ctf.ChallengeId == completedProgramId && ctf.TargetId == cred.Id ))
                                        {
                                            List<tblChallengeToFriend> objCTFList = dataContext.ChallengeToFriends.Where(ctf => ctf.ChallengeId == completedProgramId && ctf.TargetId == cred.Id ).ToList();
                                            foreach (var item in objCTFList)
                                            {
                                                item.IsPending = false;
                                                item.IsActive = true;
                                            }
                                            dataContext.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                        traceLog.AppendLine("UpdateProgramWorkout() successfully in BL----programWeekWorkoutId- " + Convert.ToString(programWeekWorkoutId));
                    }
                    return isAllWeekWorkoutsCompleted;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  SubmitResult in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to maintain submit pic in database
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/22/2015
        /// </devdoc>
        public static bool UploadResultPic(MessageStreamPicVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UploadResultPic in BL---- " + DateTime.Now.ToLongDateString());
                    tblMessageStreamPic objMessageStreamPic = new tblMessageStreamPic();
                    objMessageStreamPic.MessageStraemId = model.MessageStreamId;
                    objMessageStreamPic.PicUrl = model.PicName;
                    objMessageStreamPic.ImageMode = model.ImageMode;
                    objMessageStreamPic.Height = model.Height;
                    objMessageStreamPic.Width = model.Width;
                    dataContext.MessageStreamPic.Add(objMessageStreamPic);
                    if (dataContext.SaveChanges() > 0)
                    {
                        UpdateIsImageVideoFlag(model.MessageStreamId, true);
                    }
                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UploadResultPic in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to maintain submit video record in database
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/22/2015
        /// </devdoc>
        public static bool UploadResultVideo(MessageStreamVideoVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UploadResultVideo in BL---- " + DateTime.Now.ToLongDateString());
                    tblMessageStreamVideo objMessageStreamVideo = new tblMessageStreamVideo();
                    objMessageStreamVideo.MessageStraemId = model.MessageStreamId;
                    objMessageStreamVideo.VideoUrl = model.VideoName;
                    dataContext.MessageStreamVideo.Add(objMessageStreamVideo);
                    dataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UploadResultVideo in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to update MessageStream's IsTextImageVideo Flag
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        public static bool UpdateIsTextImageVideoFlag(string videoName, bool flag)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateIsTextImageVideoFlag in BL---- " + DateTime.Now.ToLongDateString());
                    tblMessageStream objMessageStream = (from ms in dataContext.MessageStraems
                                                         join msv in dataContext.MessageStreamVideo on ms.MessageStraemId equals msv.MessageStraemId
                                                         where msv.VideoUrl == videoName
                                                         select ms).FirstOrDefault();
                    if (objMessageStream != null)
                    {
                        objMessageStream.IsTextImageVideo = flag;
                        dataContext.SaveChanges();
                        UpdateIsImageVideoFlag(objMessageStream.MessageStraemId, flag);
                        return true;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UpdateIsTextImageVideoFlag in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to update MessageStream's IsImageVideo Flag
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 08/04/2015
        /// </devdoc>
        public static bool UpdateIsImageVideoFlag(int messageStreamId, bool flag)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateIsImageVideoFlag in BL---- " + DateTime.Now.ToLongDateString());
                    tblMessageStream objMessageStream = (from ms in dataContext.MessageStraems
                                                         where ms.MessageStraemId == messageStreamId
                                                         select ms).SingleOrDefault();
                    if (objMessageStream != null)
                    {
                        objMessageStream.IsImageVideo = flag;
                        objMessageStream.IsTextImageVideo = flag;
                        dataContext.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UpdateIsImageVideoFlag in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get GetChallenegeAcceptorList
        /// </summary>
        /// <param name="challenegeID"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetChallenegeAcceptorList(int challenegeID, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                // List<FollowerFollwingUserVM> userFollwingList = null;
                List<FollowerFollwingUserVM> objTeamTrainer = null;
                List<FollowerFollwingUserVM> objTeamUser = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetChallenegeAcceptorList");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> following = (from f in dataContext.Followings
                                           join c in dataContext.Credentials on f.UserId equals c.Id
                                           where c.UserType == cred.UserType && c.UserId == cred.UserId
                                           select f.FollowUserId).ToList();
                    // userFollwingList = new List<FollowerFollwingUserVM>();
                    objTeamTrainer = new List<FollowerFollwingUserVM>();
                    objTeamUser = new List<FollowerFollwingUserVM>();
                    List<FollowUserVM> challegeAcceptorsList = (from uc in dataContext.UserChallenge
                                                                join c in dataContext.Credentials on uc.UserId equals c.Id
                                                                where uc.ChallengeId == challenegeID
                                                                select new FollowUserVM
                                                                {
                                                                    UserId = uc.UserId,
                                                                    UserType = c.UserType
                                                                }).ToList();

                    List<int> follwingUserIDs = challegeAcceptorsList.Where(item => item.UserType == Message.UserTypeUser).Select(item => item.UserId).Distinct().ToList();
                    List<int> follwingtrainetIDs = challegeAcceptorsList.Where(item => item.UserType == Message.UserTypeTrainer).Select(item => item.UserId).Distinct().ToList();

                    if (follwingUserIDs != null && follwingUserIDs.Count > 0)
                    {
                        objTeamUser = (from t in dataContext.User
                                       join c in dataContext.Credentials on t.UserId equals c.UserId
                                       where c.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) && follwingUserIDs.Contains(c.Id)
                                       select new FollowerFollwingUserVM
                                       {
                                           ID = t.UserId,
                                           UserType = ConstantKey.UserSearchType,
                                           FullName = t.FirstName + " " + t.LastName,
                                           City = t.City,
                                           State = t.State,
                                           ImageUrl = t.UserImageUrl,
                                           TeamMemberCount = t.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                           HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty,
                                           Status = cred.Id == c.Id ? JoinStatus.CurrentUser : following.Contains(c.Id) ? JoinStatus.Follow : JoinStatus.Unfollow
                                       }).ToList();

                        /// Set image url of trainer
                        objTeamUser.ForEach(user =>
                        {
                            user.ImageUrl = (!string.IsNullOrEmpty(user.ImageUrl)
                                && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ?
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : string.Empty;
                            user.TeamName = string.IsNullOrEmpty(user.HashTag) ? string.Empty : user.HashTag.Substring(1);
                        });
                    }

                    if (follwingtrainetIDs != null && follwingtrainetIDs.Count > 0)
                    {
                        objTeamTrainer = (from t in dataContext.Trainer
                                          join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                          where c.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase) && follwingtrainetIDs.Contains(c.Id)
                                          select new FollowerFollwingUserVM
                                          {
                                              ID = t.TrainerId,
                                              UserType = ConstantKey.TrainerSerachType,
                                              FullName = t.FirstName + " " + t.LastName,
                                              Specilaization = (from ts in dataContext.TrainerSpecialization
                                                                join s in dataContext.Specialization
                                                                on ts.SpecializationId equals s.SpecializationId
                                                                where ts.IsInTopThree == 1 && ts.TrainerId == t.TrainerId
                                                                select s.SpecializationName).ToList(),
                                              City = t.City > 0 ? dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName : string.Empty,
                                              State = t.State,
                                              TeamMemberCount = t.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                              ImageUrl = t.TrainerImageUrl,
                                              IsVerifiedTrainer = 1,
                                              TrainerType = t.TrainerType,
                                              Status = cred.Id == c.Id ? JoinStatus.CurrentUser : following.Contains(c.Id) ? JoinStatus.Follow : JoinStatus.Unfollow,
                                              HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty,
                                          }).ToList();

                        /// Set image url of trainer
                        objTeamTrainer.ForEach(trainer =>
                        {
                            trainer.ImageUrl = (!string.IsNullOrEmpty(trainer.ImageUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + trainer.ImageUrl))) ?
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl : string.Empty;
                            trainer.TeamName = string.IsNullOrEmpty(trainer.HashTag) ? string.Empty : trainer.HashTag.Substring(1);
                        });
                    }
                    var userTotalFollwingList = objTeamUser != null ?objTeamUser.Union(objTeamTrainer).ToList(): objTeamTrainer;
                    Total<List<FollowerFollwingUserVM>> serachUserList = new Total<List<FollowerFollwingUserVM>>();
                    serachUserList.TotalList = (from l in userTotalFollwingList
                                                orderby l.FullName ascending
                                                select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    serachUserList.TotalCount = userTotalFollwingList.Count();
                    if ((serachUserList.TotalCount) > endIndex)
                    {
                        serachUserList.IsMoreAvailable = true;
                    }
                    return serachUserList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetChallenegeAcceptorList--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Delete Global Result Result for user
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="resultID"></param>
        /// <returns></returns>
        public static List<GlobalChallengeVM> DeleteGlobalResult(int challengeId, int resultID)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeleteGlobalResult in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var userChallengesresult = (from uc in dataContext.UserChallenge
                                                where uc.Id == resultID && uc.ChallengeId == challengeId && uc.IsGlobal && (uc.Result != null || uc.Fraction != null)
                                                select uc).FirstOrDefault();
                    if (userChallengesresult != null)
                    {
                        dataContext.UserChallenge.Remove(userChallengesresult);
                        dataContext.SaveChanges();
                    }
                    return GetGlobalCompletedChallengeList(dataContext, challengeId, cred);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  DeleteGlobalResult in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Delete the Personal Result of user or trainer
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="resultID"></param>
        /// <returns></returns>
        public static List<PersonalChallengeVM> DeletePersonalResult(int challengeId, int resultID)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeletePersonalResult in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var userChallengesresult = (from uc in dataContext.UserChallenge
                                                where uc.Id == resultID && uc.ChallengeId == challengeId && uc.UserId == cred.Id && (uc.Result != null || uc.Fraction != null)
                                                select uc).FirstOrDefault();
                    if (userChallengesresult != null)
                    {
                        dataContext.UserChallenge.Remove(userChallengesresult);
                        dataContext.SaveChanges();
                    }
                    return GetPersonalCompletedChallengeList(challengeId);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPersonalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Delete the completed User Challenege based on ID
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static CompletedChallengesWithPendingVM DeleteCompletedResult(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeletePersonalResult in BL---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var userChallengesresult = (from uc in dataContext.UserChallenge
                                                where uc.ChallengeId == challengeId && uc.UserId == cred.Id && (uc.Result != null || uc.Fraction != null)
                                                select uc).FirstOrDefault();
                    if (userChallengesresult != null)
                    {
                        dataContext.UserChallenge.Remove(userChallengesresult);
                        dataContext.SaveChanges();
                    }
                    return GetCompletedChallengeList(cred, dataContext);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPersonalCompletedChallengeList in BL : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get ProgramDetail base on program Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        /// <developer>Irshad Ansari</developer>
        /// <date>24 Aug,2016</date>
        public static ProgramDetailVM GetProgramDetail(int programId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramDetail---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    //Query to get Challenge Details from tblChallenge and tblChallengeType on the basis of ChallengeId
                    ProgramDetailVM objProgrametail = (from c in dataContext.Challenge
                                                       join cred in dataContext.Credentials on c.TrainerId equals cred.Id into tranr
                                                       join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                       from trainer in tranr.DefaultIfEmpty()
                                                       where c.ChallengeId == programId
                                                       select new ProgramDetailVM
                                                       {
                                                           ProgramId = c.ChallengeId,
                                                           ProgramName = c.ChallengeName,
                                                           DifficultyLevel = c.DifficultyLevel,
                                                           ProgramType = ct.ChallengeType,
                                                           ProgramDescription = ct.ChallengeSubType,
                                                           IsSubscription = c.IsSubscription,
                                                           WeekWorkouts = (from pw in dataContext.PWAssociation
                                                                           where pw.ProgramChallengeId == programId
                                                                           select new ProgramWeekWorkoutVM
                                                                           {
                                                                               ProgramWeekId = pw.PWRocordId,
                                                                               WeekSequenceNumber = 0,
                                                                               WeekWorkoutsRecords = (from pww in dataContext.PWWorkoutsAssociation
                                                                                                      join wchalg in dataContext.Challenge
                                                                                                      on pww.WorkoutChallengeId equals wchalg.ChallengeId
                                                                                                      where pww.PWRocordId == pw.PWRocordId
                                                                                                      select new WeekWorkoutVM
                                                                                                      {
                                                                                                          ProgramWeekId = pww.PWRocordId,
                                                                                                          WorkoutChallengeId = pw.ProgramChallengeId,
                                                                                                          workoutDuration = wchalg.FFChallengeDuration,
                                                                                                          WorkoutHashNumber = string.Empty,
                                                                                                          WorkoutName = wchalg.ChallengeName,
                                                                                                          ProgramChallengeId = wchalg.ChallengeId
                                                                                                      }).ToList()
                                                                           }).ToList(),
                                                           CreatedByUserType = trainer.UserType,
                                                           CreatedByTrainerId = trainer.UserId,
                                                           Description = c.Description,
                                                           ProgramDuration = c.FFChallengeDuration,
                                                           ProgramWorkouts = c.FFChallengeDuration,
                                                           ProgramImageUrl = c.ProgramImageUrl
                                                       }).FirstOrDefault();

                    if (objProgrametail != null)
                    {

                        int weeknumber = 0;
                        int totalworkouts = 0;
                        objProgrametail.WeekWorkouts.ForEach(wk =>
                        {
                            weeknumber++;
                            wk.WeekSequenceNumber = weeknumber;
                            int hashNumber = 0;
                            wk.WeekWorkoutsRecords.ForEach(pww =>
                            {
                                hashNumber++;
                                totalworkouts++;
                                pww.WorkoutHashNumber = ConstantHelper.constTeamNameHashTag + hashNumber;
                                pww.workoutDuration = string.IsNullOrEmpty(pww.workoutDuration) ? string.Empty : pww.workoutDuration;
                            });
                        });
                        objProgrametail.ProgramDuration = (weeknumber > 1) ? weeknumber + " " + ConstantHelper.constWeeks : weeknumber + " " + ConstantHelper.constWeek;
                        objProgrametail.ProgramWorkouts = (totalworkouts > 1) ? totalworkouts + " " + ConstantHelper.constWorkouts : totalworkouts + " " + ConstantHelper.constWorkout;
                        objProgrametail.ProgramType = objProgrametail.ProgramType.Split(' ')[0];
                        objProgrametail.CreatedByTrainerId = objProgrametail.CreatedByTrainerId ?? 0;
                        objProgrametail.ProgramLink = CommonUtility.VirtualPath + ConstantHelper.constProgramViewProgram + objProgrametail.ProgramId;
                        objProgrametail.ProgramDuration = string.IsNullOrEmpty(objProgrametail.ProgramDuration) ? string.Empty : objProgrametail.ProgramDuration;
                        if (!string.IsNullOrEmpty(objProgrametail.Description))
                        {
                            objProgrametail.Description = objProgrametail.Description.Replace(ConstantHelper.constBreakLine, ConstantHelper.constHTMLBreakLineTerninator);
                            const string HTML_TAG_PATTERN = ConstantHelper.constRemoveHTMLTag;
                            objProgrametail.Description = Regex.Replace(objProgrametail.Description, HTML_TAG_PATTERN, string.Empty).Replace(ConstantHelper.constBlankSpace, string.Empty);
                        }
                        //Check challenge to identify that it is created by Admin or Trainer
                        //If it is created by trainer then assign trainer profile pic and name
                        if (objProgrametail.CreatedByUserType != null)
                        {
                            if (objProgrametail.CreatedByUserType.Equals(Message.UserTypeTrainer))
                            {
                                tblTrainer objTrainer = dataContext.Trainer.FirstOrDefault(t => t.TrainerId == objProgrametail.CreatedByTrainerId);
                                if (objTrainer != null)
                                {
                                    objProgrametail.CreatedByTrainerName = objTrainer.FirstName + " " + objTrainer.LastName;
                                    objProgrametail.CreatedByProfilePic = string.IsNullOrEmpty(objTrainer.TrainerImageUrl) ? string.Empty :
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainer.TrainerImageUrl;
                                }
                            }
                            else
                            {
                                objProgrametail.CreatedByTrainerId = 0;
                                objProgrametail.CreatedByTrainerName = string.Empty;
                                objProgrametail.CreatedByProfilePic = string.Empty;
                            }
                        }
                        else
                        {
                            objProgrametail.CreatedByTrainerId = 0;
                            objProgrametail.CreatedByUserType = string.Empty;
                            objProgrametail.CreatedByTrainerName = string.Empty;
                            objProgrametail.CreatedByProfilePic = string.Empty;
                        }
                        objProgrametail.ProgramImageUrl = (string.IsNullOrEmpty(objProgrametail.ProgramImageUrl)) ? string.Empty :
                            File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + objProgrametail.ProgramImageUrl)) ?
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + objProgrametail.ProgramImageUrl : string.Empty;

                    }
                    // Show the Challenge to friend tag when user complete challenge at least one time.
                    if (objCred.UserType.Equals(Message.UserTypeTrainer))
                    {
                        objProgrametail.IsShowChallengeFriendToUser = dataContext.UserChallenge.Where(uc => uc.ChallengeId == programId && uc.UserId == objCred.Id).ToList().Count > 0;
                    }
                    return objProgrametail;
                }
                finally
                {
                    traceLog.AppendLine("End  GetProgramDetail : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Start Program and get all workout
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        /// <developer>Irshad Ansari</developer>
        /// <date>25 Aug,2016</date>
        public static List<ProgramWeekWorkoutVM> StartProgramById(int programId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: StartProgramById---- " + DateTime.Now.ToLongDateString());
                        if (programId > 0)
                        {
                            Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                            ProgramDetailVM objProgrametail = (from c in dataContext.Challenge
                                                               join cred in dataContext.Credentials on c.TrainerId equals cred.Id into tranr
                                                               join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                               from trainer in tranr.DefaultIfEmpty()
                                                               where c.ChallengeId == programId
                                                               select new ProgramDetailVM
                                                               {
                                                                   ProgramId = c.ChallengeId,
                                                                   ProgramName = c.ChallengeName,
                                                                   DifficultyLevel = c.DifficultyLevel,
                                                                   ProgramType = ct.ChallengeType,
                                                                   ProgramDescription = ct.ChallengeSubType,
                                                                   WeekWorkouts = (from pw in dataContext.PWAssociation
                                                                                   where pw.ProgramChallengeId == programId
                                                                                   select new ProgramWeekWorkoutVM
                                                                                   {
                                                                                       ProgramWeekId = pw.PWRocordId,
                                                                                       WeekWorkoutsRecords = (from pww in dataContext.PWWorkoutsAssociation
                                                                                                              join wchalg in dataContext.Challenge
                                                                                                              on pww.WorkoutChallengeId equals wchalg.ChallengeId
                                                                                                              where pww.PWRocordId == pw.PWRocordId
                                                                                                              select new WeekWorkoutVM
                                                                                                              {
                                                                                                                  ProgramWeekWorkoutId = pww.PWWorkoutId,
                                                                                                                  ProgramWeekId = pww.PWRocordId,
                                                                                                                  ProgramChallengeId = pw.ProgramChallengeId,
                                                                                                                  workoutDuration = wchalg.FFChallengeDuration,
                                                                                                                  WorkoutName = wchalg.ChallengeName,
                                                                                                                  WorkoutChallengeId = wchalg.ChallengeId
                                                                                                              }).ToList()
                                                                                   }).ToList(),

                                                                   CreatedByUserType = trainer.UserType,
                                                                   CreatedByTrainerId = trainer.UserId,
                                                                   Description = c.Description,
                                                                   ProgramDuration = c.FFChallengeDuration,
                                                                   ProgramWorkouts = c.FFChallengeDuration
                                                               }).FirstOrDefault();

                            if (!dataContext.UserActivePrograms.Any(pp => pp.UserCredId == objCred.Id && pp.ProgramId == programId && !pp.IsCompleted))
                            {
                                tblUserActivePrograms objtblUserActivePrograms = new tblUserActivePrograms()
                                {
                                    UserCredId = objCred.Id,
                                    ProgramId = programId,
                                    AcceptedDate = DateTime.Now,
                                    IsCompleted = false,
                                    CreatedBy = objCred.Id,
                                    ModifiedBy = objCred.Id,
                                    CreatedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now
                                };
                                dataContext.UserActivePrograms.Add(objtblUserActivePrograms);
                                dataContext.SaveChanges();
                                int userAcceptedProgramId = objtblUserActivePrograms.UserAcceptedProgramId;
                                if (objProgrametail != null)
                                {
                                    int weeknumbercount = 1;
                                    objProgrametail.WeekWorkouts.ForEach(pw =>
                                    {
                                        tblUserAcceptedProgramWeek objtblUserAcceptedProgramWeek = new tblUserAcceptedProgramWeek()
                                        {
                                            UserAcceptedProgramId = userAcceptedProgramId,
                                            CreatedBy = objCred.Id,
                                            WeekSequenceNumber = weeknumbercount,
                                            CretaedDate = DateTime.Now
                                        };
                                        dataContext.UserAcceptedProgramWeeks.Add(objtblUserAcceptedProgramWeek);
                                        dataContext.SaveChanges();
                                        long userAcceptedProgramWeekId = objtblUserAcceptedProgramWeek.UserAcceptedProgramWeekId;

                                        List<tblUserAcceptedProgramWorkouts> weekworkoutlist = new List<tblUserAcceptedProgramWorkouts>();
                                        int weekworkoutnumbercount = 1;
                                        pw.WeekWorkoutsRecords.ForEach(pww =>
                                        {
                                            tblUserAcceptedProgramWorkouts objtblUserAcceptedProgramWorkouts = new tblUserAcceptedProgramWorkouts()
                                            {
                                                UserAcceptedProgramWeekId = userAcceptedProgramWeekId,
                                                IsCompleted = false,
                                                ProgramChallengeId = programId,
                                                PWeekID = pww.ProgramWeekId,
                                                PWWorkoutID = pww.ProgramWeekWorkoutId,
                                                WorkoutChallengeID = pww.WorkoutChallengeId,
                                                ChallengeDate = DateTime.Now,
                                                UserCredID = objCred.Id,
                                                WeekWorkoutSequenceNumber = weekworkoutnumbercount
                                            };
                                            weekworkoutlist.Add(objtblUserAcceptedProgramWorkouts);
                                            weekworkoutnumbercount++;
                                        });
                                        dataContext.UserAcceptedProgramWorkouts.AddRange(weekworkoutlist);
                                        dataContext.SaveChanges();
                                        weeknumbercount++;
                                    });
                                }
                            }
                            List<ProgramWeekWorkoutVM> weekworkout = (from pw in dataContext.UserAcceptedProgramWeeks
                                                                      join pra in dataContext.UserActivePrograms
                                                                      on pw.UserAcceptedProgramId equals pra.UserAcceptedProgramId
                                                                      where pra.ProgramId == programId && pra.UserCredId == objCred.Id && !pra.IsCompleted
                                                                      select new ProgramWeekWorkoutVM
                                                                      {
                                                                          ProgramWeekId = pw.UserAcceptedProgramWeekId,
                                                                          WeekSequenceNumber = pw.WeekSequenceNumber,
                                                                          IsActive = !pra.IsCompleted,
                                                                          WeekWorkoutsRecords = (from pww in dataContext.UserAcceptedProgramWorkouts
                                                                                                 join wchalg in dataContext.Challenge
                                                                                                 on pww.WorkoutChallengeID equals wchalg.ChallengeId
                                                                                                 where pww.UserAcceptedProgramWeekId == pw.UserAcceptedProgramWeekId
                                                                                                 select new WeekWorkoutVM
                                                                                                 {
                                                                                                     ProgramWeekWorkoutId = pww.PAcceptedWorkoutId,
                                                                                                     UserAcceptedProgramId = pw.UserAcceptedProgramId,
                                                                                                     WorkoutChallengeId = pww.WorkoutChallengeID,
                                                                                                     ProgramChallengeId = pww.ProgramChallengeId,
                                                                                                     WorkoutName = wchalg.ChallengeName,
                                                                                                     IsCompleted = pww.IsCompleted
                                                                                                 }).ToList()
                                                                      }).ToList();

                            if (weekworkout != null)
                            {
                                weekworkout = weekworkout.Where(ww => ww.WeekWorkoutsRecords != null && ww.WeekWorkoutsRecords.Count > 0).ToList();
                                int weekhashNumber = 0;
                                weekworkout.ForEach(pw =>
                                {
                                    if (pw.WeekWorkoutsRecords.Count > 0)
                                    {
                                        int workouthashNumber = 0;
                                        weekhashNumber++;
                                        pw.WeekSequenceNumber = weekhashNumber;
                                        pw.WeekWorkoutsRecords.ForEach(pww =>
                                        {
                                            workouthashNumber++;
                                            pww.WorkoutHashNumber = ConstantHelper.constTeamNameHashTag + workouthashNumber;
                                        });
                                    }
                                });
                            }

                            dbTran.Commit();
                            return weekworkout;
                        }
                        return null;

                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End  StartProgramById : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Get Program Workouts list By ProgramId
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        /// <developer>Irshad Ansari</developer>
        /// <date>25 Aug,2016</date>
        public static List<ProgramWeekWorkoutVM> GetProgramWorkoutsByProgramId(int programId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {

                try
                {
                    traceLog.AppendLine("Start: GetProgramWorkoutsByProgramId---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<ProgramWeekWorkoutVM> weekworkout = (from pw in dataContext.UserAcceptedProgramWeeks
                                                              join pra in dataContext.UserActivePrograms
                                                              on pw.UserAcceptedProgramId equals pra.UserAcceptedProgramId
                                                              where pra.ProgramId == programId && pra.UserCredId == objCred.Id && !pra.IsCompleted
                                                              select new ProgramWeekWorkoutVM
                                                              {
                                                                  ProgramWeekId = pw.UserAcceptedProgramWeekId,
                                                                  WeekSequenceNumber = pw.WeekSequenceNumber,
                                                                  ProgramId = pra.ProgramId,
                                                                  UserAcceptedProgramId = pra.UserAcceptedProgramId,
                                                                  IsActive = !pra.IsCompleted,
                                                                  WeekWorkoutsRecords = (from pww in dataContext.UserAcceptedProgramWorkouts
                                                                                         join wchalg in dataContext.Challenge
                                                                                         on pww.WorkoutChallengeID equals wchalg.ChallengeId
                                                                                         where pww.UserAcceptedProgramWeekId == pw.UserAcceptedProgramWeekId
                                                                                         select new WeekWorkoutVM
                                                                                         {
                                                                                             ProgramWeekWorkoutId = pww.PAcceptedWorkoutId,
                                                                                             UserAcceptedProgramId = pw.UserAcceptedProgramId,
                                                                                             WorkoutChallengeId = wchalg.ChallengeId,
                                                                                             ProgramChallengeId = pww.ProgramChallengeId,
                                                                                             WorkoutName = wchalg.ChallengeName,
                                                                                             IsCompleted = pww.IsCompleted
                                                                                         }).ToList()
                                                              }).ToList();

                    int weekhashNumber = 0;
                    weekworkout.ForEach(pw =>
                    {
                        int workouthashNumber = 0;
                        weekhashNumber++;
                        pw.WeekSequenceNumber = weekhashNumber;
                        pw.WeekWorkoutsRecords.ForEach(pww =>
                        {
                            workouthashNumber++;
                            pww.WorkoutHashNumber = ConstantHelper.constTeamNameHashTag + workouthashNumber;
                        });
                    });
                    return weekworkout;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetProgramWorkoutsByProgramId : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Active Programs list with Detail
        /// </summary>
        /// <returns></returns>
        /// <developer>Irshad Ansari</developer>
        /// <date>25 Aug,2016</date>
        public static List<ProgramDetailVM> GetActiveProgramDetail()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramDetail---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    //Query to get Challenge Details from tblChallenge and tblChallengeType on the basis of ChallengeId
                    List<ProgramDetailVM> objProgrametaillist = (from p in dataContext.UserActivePrograms
                                                                 join c in dataContext.Challenge
                                                                 on p.ProgramId equals c.ChallengeId
                                                                 join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                 join cred in dataContext.Credentials on c.TrainerId equals cred.Id into tranr
                                                                 from trainer in tranr.DefaultIfEmpty()
                                                                 orderby c.CreatedDate descending
                                                                 where !p.IsCompleted && p.UserCredId == objCred.Id
                                                                 select new ProgramDetailVM
                                                                 {
                                                                     ProgramId = c.ChallengeId,
                                                                     ProgramName = c.ChallengeName,
                                                                     DifficultyLevel = c.DifficultyLevel,
                                                                     ProgramType = ct.ChallengeType,
                                                                     IsSubscription = c.IsSubscription,
                                                                     ProgramDescription = ct.ChallengeSubType,
                                                                     Description = c.Description,
                                                                     ProgramDuration = c.FFChallengeDuration,
                                                                     ProgramWorkouts = c.FFChallengeDuration,
                                                                     ProgramImageUrl = c.ProgramImageUrl,
                                                                     CreatedByUserType = trainer.UserType ?? string.Empty,
                                                                     CreatedByTrainerId = (int?)trainer.UserId ?? 0,
                                                                 }).ToList();

                    objProgrametaillist.ForEach(pp =>
                    {
                        if (!string.IsNullOrEmpty(pp.CreatedByUserType))
                        {
                            if (pp.CreatedByUserType.Equals(Message.UserTypeTrainer))
                            {
                                tblTrainer objTrainer = dataContext.Trainer.FirstOrDefault(t => t.TrainerId == pp.CreatedByTrainerId);
                                if (objTrainer != null)
                                {
                                    pp.CreatedByTrainerName = objTrainer.FirstName + " " + objTrainer.LastName;
                                    pp.CreatedByProfilePic = string.IsNullOrEmpty(objTrainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainer.TrainerImageUrl;
                                }
                            }
                            else
                            {
                                pp.CreatedByTrainerId = 0;
                                pp.CreatedByTrainerName = string.Empty;
                                pp.CreatedByProfilePic = string.Empty;
                            }
                        }
                        else
                        {
                            pp.CreatedByTrainerId = 0;
                            pp.CreatedByTrainerId = 0;
                            pp.CreatedByUserType = string.Empty;
                            pp.CreatedByTrainerName = string.Empty;
                            pp.CreatedByProfilePic = string.Empty;
                        }
                        string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + pp.ProgramImageUrl;
                        pp.ProgramImageUrl = (string.IsNullOrEmpty(pp.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + pp.ProgramImageUrl)) ?
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + pp.ProgramImageUrl : string.Empty;
                        if (System.IO.File.Exists(filePath))
                        {
                            using (Bitmap objBitmap = new Bitmap(filePath))
                            {
                                double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                pp.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                pp.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                            }
                        }
                        else
                        {
                            pp.Height = string.Empty;
                            pp.Width = string.Empty;
                        }
                    });
                    return objProgrametaillist;
                }
                finally
                {
                    traceLog.AppendLine("End  GetProgramDetail : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Get User Active ProgramCount
        /// </summary>
        /// <returns></returns>
        public static int GetUserActiveProgramCount(int uSerCredId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramDetail---- " + DateTime.Now.ToLongDateString());
                    //Query to get Challenge Details from tblChallenge and tblChallengeType on the basis of ChallengeId
                    var objProgrametaillist = (from p in dataContext.UserActivePrograms
                                               where !p.IsCompleted && p.UserCredId == uSerCredId
                                               select new
                                               {
                                                   p.ProgramId
                                               }).ToList();
                    if (objProgrametaillist != null)
                    {
                        return objProgrametaillist.Count;
                    }
                    return 0;
                }
                finally
                {
                    traceLog.AppendLine("End  GetProgramDetail : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get User Offline Chat Count
        /// </summary>
        /// <param name="personaltrainerCredId"></param>
        /// <returns></returns>
        public static int GetUserOfflineChatCount(int personaltrainerCredId, int userCredId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserOfflineChatCount---- " + DateTime.Now.ToLongDateString());
                    //Query to get Challenge Details from tblChallenge and tblChallengeType on the basis of ChallengeId
                    int count = dataContext.ChatHistory.Count(p => !p.IsRead && p.ReceiverCredId == userCredId && p.SenderCredId == personaltrainerCredId);
                    return count;
                }
                finally
                {
                    traceLog.AppendLine("End  GetUserOfflineChatCount : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        #endregion
    }
}