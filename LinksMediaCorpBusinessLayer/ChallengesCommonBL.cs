namespace LinksMediaCorpBusinessLayer
{

    using System;
    using System.Text;
    using System.Web.Mvc;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using System.Threading;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;

    public class ChallengesCommonBL
    {
        /// <summary>
        /// Function to get Exercise Types on the basis of ChallengeId
        /// </summary>
        /// <returns>List<ExerciseType></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 06/06/2015
        /// </devdoc>
        public static List<ExerciseType> GetExerciseTypeOnChallengeId(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExerciseTypeOnChallengeId for retrieving ExerciseTypes from database ");
                    List<ExerciseType> objExerciseTypes = (from exerType in dataContext.ExerciseTypes
                                                           join etcAsso in dataContext.ETCAssociations on exerType.ExerciseTypeId equals etcAsso.ExerciseTypeId
                                                           where etcAsso.ChallengeId == challengeId
                                                           select new ExerciseType
                                                           {
                                                               ExerciseTypeId = exerType.ExerciseTypeId,
                                                               ExerciseName = exerType.ExerciseName
                                                           }).ToList();
                    return objExerciseTypes;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetExerciseTypeOnChallengeId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Training Zone On ChallengeId
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<BodyPart> GetTrainingZoneOnChallengeId(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTraiingZoneOnChallengeId() for retrieving Traiing Zone from database ");
                    List<BodyPart> objTrainingZones = (from exerType in dataContext.BodyPart
                                                       join etcAsso in dataContext.TrainingZoneCAssociations on exerType.PartId equals etcAsso.PartId
                                                       where etcAsso.ChallengeId == challengeId
                                                       select new BodyPart
                                                       {
                                                           PartId = exerType.PartId,
                                                           PartName = exerType.PartName
                                                       }).ToList();
                    return objTrainingZones;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTraiingZoneOnChallengeId()  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get FitnessTest HoneyComb Body Parts List
        /// </summary>
        /// <returns></returns>
        public static List<ChallengeCategory> GetFitnessTestHoneyCombBodyPartsList(LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetFitnessTestHoneyCombd() for retrieving Traiing Zone from database ");
                List<ChallengeCategory> objTrainingZones = (from bp in dataContext.BodyPart
                                                            where bp.IsShownHoneyComb
                                                            orderby bp.PartName ascending
                                                            select new ChallengeCategory
                                                            {
                                                                ChallengeCategoryId = bp.PartId,
                                                                ChallengeCategoryName = bp.PartName

                                                            }).ToList();
                return objTrainingZones;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetFitnessTestHoneyCombd()  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Eqipments  Associates with ChallengeId
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<Equipments> GetEqipmentOnChallengeId(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetEqipmentOnChallengeId() for retrieving Equipment from database ");
                    List<Equipments> objEquipments = (from exerType in dataContext.Equipments
                                                      join etcAsso in dataContext.ChallengeEquipmentAssociations
                                                      on exerType.EquipmentId equals etcAsso.EquipmentId
                                                      where etcAsso.ChallengeId == challengeId
                                                      select new Equipments
                                                      {
                                                          EquipmentId = exerType.EquipmentId,
                                                          Equipment = exerType.Equipment
                                                      }).ToList();
                    return objEquipments;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetEqipmentOnChallengeId()  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get bodyparts from database
        /// </summary>        
        /// <returns>List<string></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 05/19/2015
        /// </devdoc>
        public static List<BodyPart> GetBodyParts()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetBodyParts for retrieving body parts from database ");
                    Mapper.CreateMap<tblBodyPart, BodyPart>();
                    List<tblBodyPart> lstBodyParts = dataContext.BodyPart.OrderBy(ep => ep.PartName).ToList();
                    List<BodyPart> lstlstBodyPartVM =
                        Mapper.Map<List<tblBodyPart>, List<BodyPart>>(lstBodyParts);

                    return lstlstBodyPartVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetBodyParts  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get Equipments from database
        /// </summary>        
        /// <returns>List<Equipment></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 05/27/2015
        /// </devdoc>
        public static List<Equipments> GetEquipments()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetEquipments for retrieving Equipments from database ");
                    Mapper.CreateMap<tblEquipment, Equipments>();
                    List<tblEquipment> lstEquipments = dataContext.Equipments.OrderBy(ep => ep.Equipment).ToList();
                    List<Equipments> lstEquipmentsVM =
                        Mapper.Map<List<tblEquipment>, List<Equipments>>(lstEquipments);
                    return lstEquipmentsVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetEquipments  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get Difficultys from database
        /// </summary>        
        /// <returns>List<Equipment></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 05/27/2015
        /// </devdoc>
        public static List<Difficulties> GetDifficulties()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetDifficultys for retrieving Difficultys from database ");
                    Mapper.CreateMap<tblDifficulty, Difficulties>();
                    List<tblDifficulty> lstDifficulties = dataContext.Difficulties.ToList();
                    List<Difficulties> lstDifficultiesVM =
                        Mapper.Map<List<tblDifficulty>, List<Difficulties>>(lstDifficulties);
                    return lstDifficultiesVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetDifficultys  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get active challenge count fron the database  
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/28/2015
        /// </devdoc>
        public static int GetActiveChallengeCount()
        {
            StringBuilder traceLog = new StringBuilder();
            int challengeCount = 0;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetActiveChallengeCount");
                    var challengelist = dataContext.Challenge.Where(m => m.IsActive && m.ChallengeSubTypeId != ConstantHelper.constProgramChallengeSubType).ToList();
                    if (challengelist != null)
                    {
                        challengeCount = challengelist.Count;
                    }
                    return challengeCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetActiveChallengeCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get challenge count from the database  
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetChallengeCount()
        {
            StringBuilder traceLog = new StringBuilder();
            int challengeCount = 0;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeCount");
                    var challengelist = dataContext.Challenge.Where(y => !y.IsDraft && y.ChallengeSubTypeId != ConstantHelper.constProgramChallengeSubType).ToList();
                    if (challengelist != null)
                    {
                        challengeCount = challengelist.Count;
                    }
                    return challengeCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get related user by for any challenge by challenge id 
        /// </summary>
        /// <param name="id"></param>       
        /// <returns>List<CreateUserVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<CreateUserVM> GetUserByChallengeId(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserByChallengeId for retrieving user from database ");
                    List<CreateUserVM> result = (from cred in dataContext.Credentials
                                                 join chlng in dataContext.UserChallenge on cred.Id equals chlng.UserId
                                                 join user in dataContext.User on cred.UserId equals user.UserId
                                                 where chlng.ChallengeId == Id && cred.UserType == Message.UserTypeUser && ((chlng.Result != null && chlng.Result != "0") || (chlng.Fraction != null))
                                                 select new CreateUserVM
                                                 {
                                                     UserId = user.UserId,
                                                     UserName = user.FirstName + " " + user.LastName
                                                 }).ToList();
                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUserByChallengeId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Function to getchallenge category by challenge id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ///  /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 06/28/2016
        /// </devdoc> 
        public static List<ChallengeCategory> GetChallengeCategoryList(int challengeSubTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeCategoryList for retrieving user from database ");
                    if (challengeSubTypeId > 0)
                    {
                        var challengeCategoryList = (from cc in dataContext.ChallengeCategory
                                                     where cc.ChallengeSubTypeId == challengeSubTypeId && cc.Isactive
                                                     select new ChallengeCategory
                                                     {
                                                         ChallengeCategoryName = cc.ChallengeCategoryName,
                                                         ChallengeCategoryId = cc.ChallengeCategoryId
                                                     }).OrderBy(cn => cn.ChallengeCategoryName).ToList();
                        return challengeCategoryList;
                    }
                    return null;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeCategoryList  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get created challenge category Id
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetapiChallengeCategoryList(int challengeSubTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeCategoryList for retrieving user from database- " + challengeSubTypeId);
                    bool IsProgramChallengeType = false;
                    if (challengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                    {
                        IsProgramChallengeType = true;
                    }
                    else
                    {
                        IsProgramChallengeType = false;
                    }
                    List<int> workoutcategoryId = (from C in dataContext.Challenge
                                                   join CT in dataContext.ChallengeCategoryAssociations on
                                                   C.ChallengeId equals CT.ChallengeId
                                                   where
                                                   CT.IsProgram == IsProgramChallengeType
                                                   && C.IsActive && C.TrainerId == 0
                                                   && C.ChallengeSubTypeId > 0
                                                   && C.ChallengeSubTypeId == challengeSubTypeId
                                                   && ((C.TrainerId > 0 && C.IsPremium) || (C.IsPremium && C.TrainerId == 0))
                                                   orderby C.ModifiedDate descending
                                                   select CT.ChallengeCategoryId).ToList();

                    var challengeCategoryList = (from cc in dataContext.ChallengeCategory
                                                 where cc.ChallengeSubTypeId == challengeSubTypeId && workoutcategoryId.Contains(cc.ChallengeCategoryId) && cc.Isactive
                                                 select new ChallengeCategory
                                                 {
                                                     ChallengeCategoryName = cc.ChallengeCategoryName,
                                                     ChallengeCategoryId = cc.ChallengeCategoryId,
                                                     ProgramTypeId = cc.ChallengeSubTypeId
                                                 }).OrderBy(cn => cn.ChallengeCategoryName).ToList();
                    if (!IsProgramChallengeType && challengeCategoryList != null && challengeCategoryList.Count > 0)
                    {
                        var freeWorkoutcategory = challengeCategoryList.Where(category => category.ChallengeCategoryName.Equals(ConstantHelper.FreeWorkouts)).FirstOrDefault();
                        if (freeWorkoutcategory != null)
                        {
                            challengeCategoryList.Remove(freeWorkoutcategory);
                            challengeCategoryList.Insert(0, freeWorkoutcategory);
                        }
                    }
                    else if (IsProgramChallengeType && challengeCategoryList != null && challengeCategoryList.Count > 0)
                    {
                        var freeProgramCategory = challengeCategoryList.Where(category => category.ChallengeCategoryName.Equals(ConstantHelper.FreePrograms)).FirstOrDefault();
                        if (freeProgramCategory != null)
                        {
                            challengeCategoryList.Remove(freeProgramCategory);
                            challengeCategoryList.Insert(0, freeProgramCategory);
                        }
                    }
                    return challengeCategoryList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeCategoryList  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Preminum ChallengeCategory List
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <param name="isPreminumworkout"></param>
        /// <param name="teamtrainers"></param>
        /// <returns></returns>
        public static CategoryResponse GetapiPreminumChallengeCategoryList(int challengeSubTypeId, List<int> teamtrainers)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetapiPreminumChallengeCategoryList for retrieving user from database- " + challengeSubTypeId);
                    CategoryResponse objResonse = new CategoryResponse();
                    int primaryTeamId = 0;
                    List<int> teamIds = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
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
                    if (teamtrainers == null)
                    {

                        if (teamIds.Count > 0)
                        {
                            teamtrainers = (from crd in dataContext.Credentials
                                            join tms in dataContext.TrainerTeamMembers
                                            on crd.Id equals tms.UserId
                                            where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                            select tms.UserId).ToList();

                        }
                    }
                    objResonse.ChallengeCategoryList = GetExistingChallengeBasedOnChallengeType(dataContext, challengeSubTypeId, teamtrainers, primaryTeamId);
                    objResonse.FeaturedList = TeamApiBL.GetTeamFeaturedList(dataContext, challengeSubTypeId, teamIds, primaryTeamId);
                    objResonse.TrendingCategoryList = TeamApiBL.GetTeamTrendingCategoryList(dataContext, challengeSubTypeId, teamIds, primaryTeamId);
                    return objResonse;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetapiPreminumChallengeCategoryList  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get challenge Category List based on team
        /// </summary>
        /// <param name="challengeType"></param>
        /// <param name="challengeSubTypeId"></param>
        /// <param name="teamtrainers"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private static List<ChallengeCategory> GetExistingChallengeBasedOnChallengeType(LinksMediaContext dataContext, int challengeSubTypeId, List<int> teamtrainers, int primaryteamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetExistingChallengeBasedOnChallengeType for retrieving user from database- " + challengeSubTypeId);
                List<int> workoutcategoryId = new List<int>();
                bool isProgram = false;
                if (challengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                {
                    isProgram = true;
                }
                bool isDefaultTeam = false;
                bool isShownNoTrainerWorkoutProgram = false;
                bool isShownNoTrainerFitnessTest = false;
                if (primaryteamId > 0)
                {
                    var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryteamId).FirstOrDefault();
                    if (primaryTeam != null)
                    {
                        isDefaultTeam = primaryTeam.IsDefaultTeam;
                        isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                        isShownNoTrainerFitnessTest = primaryTeam.IsShownNoTrainerFitnessTests;
                    }
                }
                var workoutcategorylist = (from C in dataContext.Challenge
                                           join CC in dataContext.ChallengeCategoryAssociations
                                           on C.ChallengeId equals CC.ChallengeId
                                           where
                                           C.IsActive
                                           && CC.IsProgram == isProgram
                                           && C.ChallengeSubTypeId > 0
                                           && C.ChallengeSubTypeId == challengeSubTypeId
                                           && ((C.TrainerId > 0 && teamtrainers.Contains(C.TrainerId) && C.IsPremium) || (C.IsPremium && C.TrainerId == 0))
                                           orderby C.ModifiedDate descending
                                           select new
                                           {
                                               C.TrainerId,
                                               C.ChallengeSubTypeId,
                                              // NotrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(t => t.ChallengeId == C.ChallengeId).Select(tm => tm.TeamId).ToList(),
                                               WorkoutCategoryList = dataContext.ChallengeCategoryAssociations.
                                               Where(t => t.ChallengeId == C.ChallengeId && t.IsProgram == isProgram)
                                               .Select(tm => tm.ChallengeCategoryId).ToList()
                                           }).ToList();
                if (workoutcategorylist != null && workoutcategorylist.Count > 0)
                {
                    if (isDefaultTeam)
                    {
                        // workoutcategoryId = workoutcategorylist.Where(ct => !(ct.TrainerId > 0) && isShownNoTrainerWorkoutProgram && ct.NotrainerWorkoutTeamList.Contains(primaryteamId)).SelectMany(w => w.WorkoutCategoryList).Distinct().ToList();
                        workoutcategoryId = workoutcategorylist.Where(ct => ct.TrainerId > 0 || (!(ct.TrainerId > 0) && isShownNoTrainerWorkoutProgram)).SelectMany(w => w.WorkoutCategoryList).Distinct().ToList();
                    }
                    else
                    {
                        workoutcategoryId = workoutcategorylist.Where(ct => ct.TrainerId > 0 || isShownNoTrainerWorkoutProgram).SelectMany(w => w.WorkoutCategoryList).Distinct().ToList();
                    }
                }
                List<ChallengeCategory> challengeCategoryList = (from cc in dataContext.ChallengeCategory
                                             where cc.ChallengeSubTypeId == challengeSubTypeId && workoutcategoryId.Contains(cc.ChallengeCategoryId) && cc.Isactive
                                             select new ChallengeCategory
                                             {
                                                 ChallengeCategoryName = cc.ChallengeCategoryName,
                                                 ChallengeCategoryId = cc.ChallengeCategoryId,
                                                 ProgramTypeId = cc.ChallengeSubTypeId
                                             }).OrderBy(cn => cn.ChallengeCategoryName).ToList();

                if (!isProgram && challengeCategoryList != null && challengeCategoryList.Count > 0)
                {
                    var freeWorkoutcategory = challengeCategoryList.Where(category => category.ChallengeCategoryName.Equals(ConstantHelper.FreeWorkouts)).FirstOrDefault();
                    if (freeWorkoutcategory != null)
                    {
                        challengeCategoryList.Remove(freeWorkoutcategory);
                        challengeCategoryList.Insert(0, freeWorkoutcategory);
                    }
                }
                else if (isProgram && challengeCategoryList != null && challengeCategoryList.Count > 0)
                {
                    var freeProgramCategory = challengeCategoryList.Where(category => category.ChallengeCategoryName.Equals(ConstantHelper.FreePrograms)).FirstOrDefault();
                    if (freeProgramCategory != null)
                    {
                        challengeCategoryList.Remove(freeProgramCategory);
                        challengeCategoryList.Insert(0, freeProgramCategory);
                    }
                }
                return challengeCategoryList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetExistingChallengeBasedOnChallengeType  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Existing Fittnest Challenge Based On Challenge Type
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <param name="teamtrainers"></param>
        /// <param name="primaryteamId"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetExistingFittnestFittnessTestChallenge(LinksMediaContext dataContext, int challengeSubTypeId, List<int> teamtrainers, int primaryteamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetExistingFittnestFittnessTestChallenge for retrieving user from database- " + challengeSubTypeId);
                List<int> ChallengeSubTypeList = TeamApiBL.GetChallengeAllSubTypeId(challengeSubTypeId);
                // int fittnessTestCommonSubTypeId = TeamApiBL.GetFittnessTestCommonSubTypeId(challengeSubTypeId);
                List<int> fittnessTestBodyPartId = new List<int>();
                bool isDefaultTeam = false;
                bool isShownNoTrainerFittnessTest = false;
                if (primaryteamId > 0)
                {
                    var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryteamId).FirstOrDefault();
                    if (primaryTeam != null)
                    {
                        isDefaultTeam = primaryTeam.IsDefaultTeam;
                        isShownNoTrainerFittnessTest = primaryTeam.IsShownNoTrainerFitnessTests;
                    }
                }
                if (isDefaultTeam)
                  {
                    
                    if (teamtrainers != null)
                    {
                        teamtrainers.Clear();
                    }
                    else
                    {
                        teamtrainers = new List<int>();
                    }
                    teamtrainers.Add(ConstantHelper.constDefaultFitcomTrainer);
                  }
                    var fittnessTestChallengelist = (from C in dataContext.Challenge
                                                 where
                                                 C.IsActive
                                                 && C.ChallengeSubTypeId > 0
                                                 && ChallengeSubTypeList.Contains(C.ChallengeSubTypeId)
                                                 && ((C.TrainerId > 0 && teamtrainers.Contains(C.TrainerId)) || (C.TrainerId == 0 && isShownNoTrainerFittnessTest))
                                                 orderby C.ModifiedDate descending
                                                 select new
                                                 {
                                                     C.TrainerId,
                                                     C.ChallengeSubTypeId,
                                                     FittnestPartNameList = (from trzone in dataContext.TrainingZoneCAssociations
                                                                             join bp in dataContext.BodyPart
                                                                             on trzone.PartId equals bp.PartId
                                                                             where trzone.ChallengeId == C.ChallengeId
                                                                             select bp.PartId).ToList<int>(),
                                                     C.IsFreeFitnessTest
                                                 }).ToList();
                if (fittnessTestChallengelist != null && fittnessTestChallengelist.Count > 0)
                {
                    //if (isDefaultTeam)
                    //{
                    //    fittnessTestBodyPartId = fittnessTestChallengelist.Where(ct => !(ct.TrainerId > 0) ).SelectMany(w => w.FittnestPartNameList).Distinct().ToList();
                    //}
                    //else
                    //{
                        fittnessTestBodyPartId = fittnessTestChallengelist.Where(ct => ct.TrainerId > 0 || (ct.TrainerId == 0))
                            .SelectMany(w => w.FittnestPartNameList).Distinct().ToList();
                   // }
                }
                List<ChallengeCategory> fittnessTestCategoryList = (from bp in dataContext.BodyPart
                                                                    where bp.IsShownHoneyComb && fittnessTestBodyPartId.Contains(bp.PartId)
                                                                    orderby bp.PartName ascending
                                                                    select new ChallengeCategory
                                                                    {
                                                                        ChallengeCategoryId = bp.PartId,
                                                                        ChallengeCategoryName = bp.PartName
                                                                    }).ToList();
                if (fittnessTestChallengelist != null && fittnessTestChallengelist.Any(freetest => freetest.IsFreeFitnessTest) 
                    && fittnessTestCategoryList != null && fittnessTestCategoryList.Count > 0)
                {
                    var freeFitnessTestCate = new ChallengeCategory
                    {
                        ChallengeCategoryId = -1,
                        ChallengeCategoryName = ConstantHelper.FreeFitnessTests
                    };
                    fittnessTestCategoryList.Insert(0, freeFitnessTestCate);
                }
                return fittnessTestCategoryList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetExistingFittnestFittnessTestChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// To get Premium challenge Sub Type
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetPremiumChallengeCategoryList()
        {
            List<ChallengeCategory> objPremiumtypes = new List<ChallengeCategory>();
            objPremiumtypes.Add(new ChallengeCategory() { ChallengeCategoryId = ConstantHelper.constWorkoutChallengeSubType, ChallengeCategoryName = ConstantHelper.constWorkoutChallenge });
            objPremiumtypes.Add(new ChallengeCategory() { ChallengeCategoryId = ConstantHelper.constWellnessChallengeSubType, ChallengeCategoryName = ConstantHelper.constWellnessChallenges });
            objPremiumtypes.Add(new ChallengeCategory() { ChallengeCategoryId = ConstantHelper.constProgramChallengeSubType, ChallengeCategoryName = ConstantHelper.constProgramChallenge });
            return objPremiumtypes;
        }
        /// <summary>
        /// Function to get challenge related result 
        /// </summary>
        /// <param name="id"></param>       
        /// <returns>List<TrainerViewVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 05/28/2015
        /// </devdoc>
        public static UserResult GetResultByChallengeId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetVideoByChallengeId for retrieving video from database ");
                    tblUserChallenges objUserResult = dataContext.UserChallenge.Where(m => m.ChallengeId == id).FirstOrDefault();
                    Mapper.CreateMap<tblUserChallenges, UserResult>();
                    UserResult userResult =
                        Mapper.Map<tblUserChallenges, UserResult>(objUserResult);
                    return userResult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetVideoByChallengeId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get challenge related video 
        /// </summary>
        /// <param name="id"></param>       
        /// <returns>List<TrainerViewVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 05/28/2015
        /// </devdoc>
        public static string GetVideoByChallengeId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetVideoByChallengeId for retrieving video from database ");
                    string videoLink = dataContext.HypeVideos.Where(m => m.ChallengeId == id).Select(y => y.HypeVideo).FirstOrDefault();
                    return videoLink;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetVideoByChallengeId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get trainer for any challenge by challenge id 
        /// </summary>
        /// <param name="id"></param>       
        /// <returns>List<TrainerViewVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<TrainerViewVM> GetTrainersByChallengeId(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainersByChallengeId for retrieving trainerId from database ");
                    List<TrainerViewVM> result = (from cred in dataContext.Credentials
                                                  join chlng in dataContext.UserChallenge on cred.Id equals chlng.UserId
                                                  join trnr in dataContext.Trainer on cred.UserId equals trnr.TrainerId
                                                  where chlng.ChallengeId == Id && cred.UserType == Message.UserTypeTrainer && ((chlng.Result != null && chlng.Result != "0") || chlng.Fraction != null)
                                                  select new TrainerViewVM
                                                  {
                                                      TrainerId = trnr.TrainerId,
                                                      TrainerName = trnr.FirstName + " " + trnr.LastName
                                                  }).ToList();
                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainersByChallengeId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get exercise count for any challenge 
        /// </summary>
        /// <param name="id"></param>       
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static bool GetExerciseIsMOrethenOne(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            bool flag = false;
            try
            {
                traceLog.AppendLine("Start: GetExerciseIsMOrethenOne method");

                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    string exerciseSelectionType = dataContext.ChallengeType.Where(C => C.ChallengeSubTypeId == id && C.IsExerciseMoreThanOne == "Yes").Select(y => y.IsExerciseMoreThanOne).FirstOrDefault();
                    if (exerciseSelectionType != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            finally
            {
                traceLog.AppendLine("GetExerciseIsMOrethenOne end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            return flag;
        }


        /// <summary>
        /// Function to get fraction for interval or round
        /// </summary>
        /// <param name="id"></param>       
        /// <returns>List<SelectListItem></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<SelectListItem> GetFraction(int id)
        {
            StringBuilder traceLog = new StringBuilder();

            try
            {
                traceLog.AppendLine("Start: GetFraction method");
                /*ViewBag for Fraction detai dropdown*/
                List<SelectListItem> objSelectList = new List<SelectListItem>();
                if (id.Equals(2))
                {
                    objSelectList = new List<SelectListItem>()
                {
                        new SelectListItem()
                        {
                            Text = Message.OneByTwo, Value = Message.OneByTwo
                        }
                };
                }
                else if (id.Equals(3))
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
                else if (id.Equals(4))
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

                return objSelectList;
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
        /// Function to get challenge syub-type id
        /// </summary>
        /// <param name="id"></param>       
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetChallengeSubTypeId(int id)
        {
            int ret = 0;
            switch (id)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    ret = 1;
                    break;
                case 5:
                case 6:
                    ret = 5;
                    break;
                case 7:
                case 8:
                    ret = 7;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    ret = 9;
                    break;
                case 13:
                case 14:
                case 15:
                    ret = 13;
                    break;
                case 16:
                    ret = 16;
                    break;
                default:
                    ret = 0;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// Function to get exercise index  
        /// </summary>
        /// <param name="tearm"></param>
        /// <returns>List<string></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<Exercise> GetSearchExerciseIndex(string term, string bodyPart, string equipment, string exerciseType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExerciseIndex for retrieving exercise index from database ");
                    List<Exercise> listOut = null; 
                    listOut = (from e in dataContext.Exercise
                               where e.IsActive && (e.ExerciseName.Contains(term) || e.Index.StartsWith(bodyPart) || e.Index.StartsWith(equipment) || e.Index.StartsWith(exerciseType))
                               orderby e.ExerciseName ascending
                               select new Exercise { ExerciseName = e.ExerciseName, VedioLink = e.VideoLink }).ToList();
                    listOut.ForEach(
                        exer =>
                        {
                            exer.VedioLink = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exer.VedioLink;
                            // exer.VedioLink = CommonUtility.VirtualPath + Message.ExerciseVideoDirectory + exer.VedioLink;
                            // exer.ExerciseThumnail = CommonWebApiBL.GetSaveExerciseThumbnail(exer.VedioLink, exer.ExerciseName);
                        });
                    if(listOut == null)
                    {
                        listOut= new List<Exercise>();
                    }
                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetExerciseIndex  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get exercise index  
        /// </summary>
        /// <param name="tearm"></param>
        /// <returns>List<string></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<Exercise> GetAllExerciseIndex()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllExerciseIndex for retrieving exercise index from database ");
                 
                    List<Exercise> listOut = (from e in dataContext.Exercise
                               orderby e.ExerciseName ascending
                               where e.IsActive
                               select new Exercise { ExerciseName = e.ExerciseName, VedioLink = e.VideoLink, Index = e.Index }).ToList();
                    listOut.ForEach(
                        exer =>
                        {
                            exer.VedioLink = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exer.VedioLink;
                            exer.ExerciseName = exer.ExerciseName;
                        });
                    if (listOut == null)
                    {
                        listOut = new List<Exercise>();
                    }
                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetAllExerciseIndex  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get challenge count from the database  
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <returns>ChallengeTypes</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static ChallengeTypes GetChallengeVal(int challengeSubTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeVal for retrieving challenges type id from database ");
                    if (challengeSubTypeId > 0)
                    {
                        Mapper.CreateMap<tblChallengeType, ChallengeTypes>();                       
                        tblChallengeType challengeType = dataContext.ChallengeType.Where(m => m.ChallengeSubTypeId == challengeSubTypeId).SingleOrDefault<tblChallengeType>();
                        ChallengeTypes chalangeTypeVM =Mapper.Map<tblChallengeType, ChallengeTypes>(challengeType);
                        return chalangeTypeVM;
                    }
                    return null;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeVal  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get Exercise Types from database
        /// </summary>
        /// <returns>List<exreciseTpye></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 06/04/2015
        /// </devdoc>
        public static List<ExerciseType> GetExerciseTypes()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExerciseTypes for retrieving exercise type from database ");
                    List<ExerciseType> exreciseTpye = (from E in dataContext.ExerciseTypes
                                                       orderby E.ExerciseName
                                                       select new ExerciseType
                                                       {
                                                           ExerciseTypeId = E.ExerciseTypeId,
                                                           ExerciseName = E.ExerciseName
                                                       }).ToList<ExerciseType>();
                    return exreciseTpye;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetExerciseTypes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Check the duplicate challenge name for trainer only
        /// </summary>
        /// <param name="trainercredtialId"></param>
        /// <param name="challengeName"></param>
        /// <returns></returns>
        public static bool GetTrainersByChallengeName(int trainercredtialId, string challengeName)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainersByChallengeName for checking dupliacte challenge name for trainer ");
                    if (!string.IsNullOrWhiteSpace(challengeName) && trainercredtialId > 0)
                    {
                        var isexist = dataContext.Challenge.Any(ch => ch.ChallengeName == challengeName && ch.TrainerId == trainercredtialId);

                        return isexist;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainersByChallengeName  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        public static bool ValidateOnTeamboardingExeciseID(int execiseId, int teamID = 0)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: ValidateOnTeamboardingExeciseID for checking dupliacte On Team boarding Execise");
                    if (execiseId > 0)
                    {
                        var isexist = dataContext.Teams.Any(ch => ch.OnboardingExeciseVideoId == execiseId && ch.TeamId != teamID);
                        return isexist;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("ValidateOnTeamboardingExeciseID  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        #region Program
        public static List<Difficulties> GetProgramDifficulties()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetDifficultys for retrieving Difficultys from database ");
                    Mapper.CreateMap<tblDifficulty, Difficulties>();
                    List<tblDifficulty> lstDifficulties = dataContext.Difficulties.Where(pd => pd.IsShowInProgram).ToList();
                    List<Difficulties> lstDifficultiesVM =
                        Mapper.Map<List<tblDifficulty>, List<Difficulties>>(lstDifficulties);

                    return lstDifficultiesVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetDifficultys  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="challengeTypeId"></param>
        /// <returns></returns>
        public static List<TrendingCategory> GetTrendingCategory(int challengeTypeId = 0)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrendingCategory from database ");
                    List<TrendingCategory> lstTrendingCategory = new List<TrendingCategory>();
                    if (challengeTypeId > 0)
                    {
                        string trendingCategoryType = string.Empty;
                        switch (challengeTypeId)
                        {
                            case ConstantHelper.constWorkoutChallengeSubType:
                                trendingCategoryType = ConstantHelper.constWorkoutChallenge;
                                break;
                            case ConstantHelper.constProgramChallengeSubType:
                                trendingCategoryType = ConstantHelper.constProgramChallenge;
                                break;
                            case ConstantHelper.constWellnessChallengeSubType:
                            case ConstantHelper.FreeformChallangeId:
                            case 0:
                                trendingCategoryType = string.Empty;
                                break;
                            case ConstantHelper.constPowerChallengeType1:
                            case ConstantHelper.constPowerChallengeType2:
                            case ConstantHelper.constEnduranceChallengeType1:
                            case ConstantHelper.constEnduranceChallengeType2:
                            case ConstantHelper.constStrengthChallengeType1:
                            case ConstantHelper.constStrengthChallengeType2:
                            case ConstantHelper.constCardioChallengeType1:
                            case ConstantHelper.constCardioChallengeType2:
                            case ConstantHelper.constCardioChallengeType3:
                            case ConstantHelper.constCardioChallengeType4:
                                trendingCategoryType = ConstantHelper.constFitnessTestName;
                                break;
                        }
                        lstTrendingCategory = (from tr in dataContext.TrendingCategory
                                               orderby tr.TrendingCategoryId
                                               where string.Compare(tr.TrendingType, trendingCategoryType, true) == 0
                                               select new TrendingCategory
                                               {
                                                   TrendingCategoryId = tr.TrendingCategoryId,
                                                   TrendingCategoryName = tr.TrendingName,
                                                   TrendingCategoryGroupId=tr.TrendingCategoryGroupId
                                               }).Select(tc => tc).ToList();
                    }
                    return lstTrendingCategory;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrendingCategory  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="challengeTypeId"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetChallengeCategory(int challengeTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeCategory from database ");
                    List<ChallengeCategory> lstChallengeCategory = new List<ChallengeCategory>();
                    if (challengeTypeId > 0)
                    {
                        lstChallengeCategory = (from tr in dataContext.ChallengeCategory
                                                orderby tr.ChallengeCategoryName
                                                where tr.ChallengeSubTypeId == challengeTypeId
                                                select new ChallengeCategory
                                                {
                                                    ChallengeCategoryId = tr.ChallengeCategoryId,
                                                    ChallengeCategoryName = tr.ChallengeCategoryName
                                                }).Select(tc => tc).ToList();
                    }



                    return lstChallengeCategory;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeCategory  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}