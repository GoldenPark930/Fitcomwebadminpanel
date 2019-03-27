using LinksMediaCorpEntity;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
using System.Globalization;

namespace LinksMediaCorpBusinessLayer
{
    public class ProgramBL
    {
        /// <summary>
        /// Get all filter workout based on selected trainer ,Target Zone and Difficulty Level
        /// </summary>
        /// <param name="selectedWorkoutTrainerId"></param>
        /// <param name="selectedWorkoutTargetZone"></param>
        /// <param name="selectedWorkoutDifficultyLevel"></param>
        /// <returns></returns>
        public static List<WorkoutResponse> GetAllFilterWorkout(SearchWeekWorkoutVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChallengeVal for retrieving challenges type id from database ");
                    List<WorkoutResponse> listOut = new List<WorkoutResponse>();
                    if (model != null)
                    {
                        bool IsSerch = true;
                        if (model.SelectedWorkoutTrainerId > 0 || !string.IsNullOrEmpty(model.SelectedWorkoutDifficultyLevel)
                            || model.SelectedWorkoutTraingZone > 0 || !string.IsNullOrEmpty(model.SearchTerm))
                        {
                            IsSerch = false;
                        }

                        List<WorkoutResponse> workoutchallengelist = (from chl in dataContext.Challenge
                                                                      where chl.IsActive == true && chl.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType
                                                                      orderby chl.ChallengeName
                                                                      select new WorkoutResponse
                                                                      {
                                                                          WorkoutId = chl.ChallengeId,
                                                                          WorkoutName = chl.ChallengeName,
                                                                          WorkoutUrl = chl.ChallengeName,
                                                                          TrainerId = dataContext.Credentials.Where(crd => crd.Id == chl.TrainerId && crd.UserType ==
                                                                              Message.UserTypeTrainer).Select(c => c.UserId).FirstOrDefault(),
                                                                          DifficultyLevel = chl.DifficultyLevel,
                                                                          TargetZone = dataContext.TrainingZoneCAssociations.
                                                                          Where(zone => zone.ChallengeId == chl.ChallengeId).Select(c => c.PartId).ToList<int>(),
                                                                      }).ToList();

                        if (workoutchallengelist != null && workoutchallengelist.Count() > 0)
                        {
                            if (!IsSerch)
                            {
                                listOut = GetFilterWorkouts(ref workoutchallengelist, model.SelectedWorkoutTrainerId,
                                    model.SelectedWorkoutTraingZone, model.SelectedWorkoutDifficultyLevel, model.SearchTerm);
                            }
                            else
                            {
                                listOut = workoutchallengelist;
                            }
                            if (listOut != null)
                            {
                                listOut.ForEach(p =>
                                {
                                    p.WorkoutUrl = CommonUtility.VirtualPath + ConstantHelper.constProgramViewChallenge + p.WorkoutId;
                                });
                            }
                        }
                    }
                    return listOut;
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
        /// return filter Workouts
        /// </summary>
        /// <param name="worklist"></param>
        /// <param name="selectedWorkoutTrainerId"></param>
        /// <param name="selectedWorkoutTargetZone"></param>
        /// <param name="selectedWorkoutDifficultyLevel"></param>
        /// <param name="searchitem"></param>
        /// <returns></returns>
        private static List<WorkoutResponse> GetFilterWorkouts(ref List<WorkoutResponse> listOut, int selectedWorkoutTrainerId,
            int selectedWorkoutTargetZone, string selectedWorkoutDifficultyLevel, string searchitem)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetfilterWorkouts() based on filter option");
                if (selectedWorkoutTrainerId > 0)
                {
                    listOut = listOut.Where(c => c.TrainerId == selectedWorkoutTrainerId).OrderBy(cc => cc.WorkoutName).ToList();
                }
                if (selectedWorkoutTargetZone > 0)
                {
                    listOut = listOut.Where(c => c.TargetZone.Contains(selectedWorkoutTargetZone)).OrderBy(cc => cc.WorkoutName).ToList();
                }
                if (!string.IsNullOrEmpty(selectedWorkoutDifficultyLevel))
                {
                    listOut = listOut.Where(c => string.Compare(c.DifficultyLevel, selectedWorkoutDifficultyLevel, StringComparison.OrdinalIgnoreCase) == 0).OrderBy(cc => cc.WorkoutName).ToList();
                }
                if (!string.IsNullOrEmpty(searchitem))
                {
                    listOut = listOut.Where(c => c.WorkoutName.ToUpper(CultureInfo.InvariantCulture).Contains(searchitem.ToUpper(CultureInfo.InvariantCulture))).OrderBy(cc => cc.WorkoutName).ToList();
                }
                return listOut;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetfilterWorkouts  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Delete the Program based on program Id
        /// </summary>
        /// <param name="Id"></param>
        public static void DeleteProgram(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: DeleteProgram for deleting challenge");
                        /*Delete challenge and exercise association*/
                        List<tblPWAssociation> objPWAssociationList = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == Id).ToList();
                        dataContext.PWAssociation.RemoveRange(objPWAssociationList);
                        List<tblPWWorkoutsAssociation> objPWWorkoutsAssociationList = (from pw in dataContext.PWAssociation
                                                                                       join pww in dataContext.PWWorkoutsAssociation
                                                                                       on pw.PWRocordId equals pww.PWRocordId
                                                                                       where pw.ProgramChallengeId == Id
                                                                                       select pww
                                                                       ).ToList();
                        if (objPWWorkoutsAssociationList != null && objPWWorkoutsAssociationList.Count > 0)
                        {
                            dataContext.PWWorkoutsAssociation.RemoveRange(objPWWorkoutsAssociationList);
                        }
                        // Get 
                        List<tblUserAcceptedProgramWorkouts> objUserAcceptedProgramWorkoutsList = (from apgm in dataContext.UserAcceptedProgramWeeks
                                                                                                   join pww in dataContext.UserAcceptedProgramWorkouts
                                                                                                   on apgm.UserAcceptedProgramWeekId equals pww.UserAcceptedProgramWeekId
                                                                                                   where pww.ProgramChallengeId == Id
                                                                                                   select pww
                                                                       ).ToList();
                        List<tblUserAcceptedProgramWeek> objUserAcceptedProgramWeeknList = (from apgm in dataContext.UserActivePrograms
                                                                                            join pww in dataContext.UserAcceptedProgramWeeks
                                                                                            on apgm.UserAcceptedProgramId equals pww.UserAcceptedProgramId
                                                                                            where apgm.ProgramId == Id
                                                                                            select pww
                                                                     ).ToList();
                        List<tblUserActivePrograms> objUserActiveProgramslist = dataContext.UserActivePrograms.Where(ce => ce.ProgramId == Id).ToList();

                        // Delete  the User AcceptedProgramW orkoutsList
                        if (objUserAcceptedProgramWorkoutsList != null)
                        {
                            dataContext.UserAcceptedProgramWorkouts.RemoveRange(objUserAcceptedProgramWorkoutsList);
                        }
                        // Delete  the User AcceptedProgramW orkoutsList
                        if (objUserAcceptedProgramWeeknList != null)
                        {
                            dataContext.UserAcceptedProgramWeeks.RemoveRange(objUserAcceptedProgramWeeknList);
                        }
                        // Delete  the User User Accepted Program Weekn List
                        if (objUserAcceptedProgramWeeknList != null)
                        {
                            dataContext.UserActivePrograms.RemoveRange(objUserActiveProgramslist);
                        }

                        /*Delete userChallenge Challenge*/
                        List<tblUserChallenges> objUserChallenges = dataContext.UserChallenge.Where(ce => ce.ChallengeId == Id).ToList();
                        dataContext.UserChallenge.RemoveRange(objUserChallenges);

                        /*delete challenege*/
                        tblChallenge challenge = dataContext.Challenge.Find(Id);
                        dataContext.Challenge.Remove(challenge);

                        dataContext.SaveChanges();
                        dbTran.Commit();
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("DeleteProgram  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ViewWorkoutDetailVM GetProgramWorkoutById(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetProgramWorkoutById() for retrieving challenge by challengeid:" + id);
                    ViewWorkoutDetailVM challenge = (from c in dataContext.Challenge
                                                     join ct in dataContext.ChallengeType
                                                     on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                     where c.ChallengeId == id
                                                     select new ViewWorkoutDetailVM
                                                     {
                                                         ChallengeId = c.ChallengeId,
                                                         ChallengeName = c.ChallengeName,
                                                         ChallengeType = ct.ChallengeSubTypeId,
                                                         ChallengeType_Name = ct.ChallengeSubType,
                                                         ChallengeSubType_Description = c.Description,
                                                         ChallengeCategoryNameList = (from trzone in dataContext.ChallengeCategoryAssociations
                                                                                      join bp in dataContext.ChallengeCategory
                                                                                      on trzone.ChallengeCategoryId equals bp.ChallengeCategoryId
                                                                                      where trzone.ChallengeId == c.ChallengeId
                                                                                      && trzone.IsProgram == true
                                                                                      select bp.ChallengeCategoryName).Distinct().ToList<string>(),
                                                         DifficultyLevel = c.DifficultyLevel,
                                                         FFChallengeDuration = c.FFChallengeDuration,
                                                         TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                           join bp in dataContext.BodyPart
                                                                           on trzone.PartId equals bp.PartId
                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                           select bp.PartName).Distinct().ToList<string>(),
                                                         TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                           join bp in dataContext.Equipments
                                                                           on trzone.EquipmentId equals bp.EquipmentId
                                                                           where trzone.ChallengeId == c.ChallengeId
                                                                           select bp.Equipment).Distinct().ToList<string>()

                                                     }).FirstOrDefault();
                    if (challenge != null)
                    {
                        var execiseTypelist = (from trzone in dataContext.ETCAssociations
                                               join bp in dataContext.ExerciseTypes
                                               on trzone.ExerciseTypeId equals bp.ExerciseTypeId
                                               where trzone.ChallengeId == challenge.ChallengeId
                                               select bp.ExerciseName).Distinct().ToList<string>();
                        if (execiseTypelist != null && execiseTypelist.Count > 0)
                        {
                            challenge.ExeciseType = string.Join(",", execiseTypelist);
                        }
                        if (challenge.TempEquipments != null && challenge.TempEquipments.Count > 0)
                        {
                            challenge.Equipment = string.Join(", ", challenge.TempEquipments);
                        }
                        challenge.TempEquipments = null;
                        if (challenge.TempTargetZone != null && challenge.TempTargetZone.Count > 0)
                        {
                            challenge.TargetZone = string.Join(", ", challenge.TempTargetZone);
                        }
                        challenge.TempTargetZone = null;
                        /*Get exercise detail for the respective challenge*/
                        List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == id).ToList();
                        List<Exercise> execisevideoList = new List<Exercise>();
                        for (int i = 0; i < objCEAssociationList.Count; i++)
                        {
                            tblExercise exercise = dataContext.Exercise.Find(objCEAssociationList[i].ExerciseId);
                            Exercise objExercise = new Exercise();
                            if (objCEAssociationList[i].Description != null && objCEAssociationList[i].Description == ConstantHelper.constFFChallangeDescription)
                            {
                                objCEAssociationList[i].Description = string.Empty;
                            }
                            objExercise.ExerciseId = exercise != null ? exercise.ExerciseId : 0;
                            objExercise.Description = objCEAssociationList[i].Description;
                            objExercise.ExerciseName = (exercise != null && !objCEAssociationList[i].IsAlternateExeciseName) ? exercise.ExerciseName : string.Empty;
                            objExercise.VedioLink = (exercise != null && !objCEAssociationList[i].IsAlternateExeciseName) ? exercise.V720pUrl : string.Empty;
                            objExercise.IsAlternateExeciseName = objCEAssociationList[i].IsAlternateExeciseName;

                            objExercise.AlternateExeciseName = (objCEAssociationList[i].AlternateExeciseName == ConstantHelper.constFFChallangeDescription) ? string.Empty :
                                objCEAssociationList[i].AlternateExeciseName;
                            objExercise.Index = exercise != null ? exercise.Index : string.Empty;
                            objExercise.ExeciseSetRecords = FreeFormChallengeBL.GetFFChallangeExeciseSetById(objCEAssociationList[i].RocordId);
                            execisevideoList.Add(objExercise);
                        }
                        challenge.SetAvailableExerciseVideoList(execisevideoList);
                    }
                    return challenge;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetProgramWorkoutById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Program  details By Program Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ViewProgramDetail GetProgramById(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetProgramWorkoutById() for retrieving challenge by challengeid:" + id);
                    ViewProgramDetail challenge = (from c in dataContext.Challenge
                                                   join ct in dataContext.ChallengeType
                                                   on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                   where c.ChallengeId == id
                                                   select new ViewProgramDetail
                                                   {
                                                       ChallengeId = c.ChallengeId,
                                                       ChallengeName = c.ChallengeName,
                                                       IsFeatured=c.IsFeatured,
                                                       FeaturedImageUrl=c.FeaturedImageUrl,
                                                       ProgramImageUrl=c.ProgramImageUrl,
                                                       ChallengeType = ct.ChallengeSubTypeId,
                                                       ChallengeType_Name = ct.ChallengeSubType,
                                                       ChallengeSubType_Description = c.Description,
                                                       ChallengeCategoryNameList = (from chgCat in dataContext.ChallengeCategoryAssociations
                                                                                    join cc in dataContext.ChallengeCategory
                                                                                    on chgCat.ChallengeCategoryRecordId equals cc.ChallengeCategoryId
                                                                                    where chgCat.ChallengeId == c.ChallengeId
                                                                                    && chgCat.IsProgram == (ConstantHelper.constProgramChallengeSubType == c.ChallengeSubTypeId)
                                                                                    select cc.ChallengeCategoryName
                                                                                    ).Distinct().ToList<string>(),
                                                       DifficultyLevel = c.DifficultyLevel


                                                   }).FirstOrDefault();
                    if (challenge != null)
                    {

                        /*Get exercise detail for the respective challenge*/
                        List<tblPWAssociation> objPWAssociationnList = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == id).ToList();
                        List<ProgramWeekWorkout> programWeekWorkouList = new List<ProgramWeekWorkout>();
                        objPWAssociationnList.ForEach(pw =>
                            {
                                ProgramWeekWorkout objProgramWeekWorkout = new ProgramWeekWorkout();
                                List<tblPWWorkoutsAssociation> prmWAssociationList = dataContext.PWWorkoutsAssociation.Where(pww => pww.PWRocordId == pw.PWRocordId).ToList();
                                List<ProgramWorkout> weekWorkouList = new List<ProgramWorkout>();
                                prmWAssociationList.ForEach(pww =>
                                 {
                                     ProgramWorkout objPgrmWorkout = new ProgramWorkout();
                                     objPgrmWorkout.WorkoutId = pww.WorkoutChallengeId;
                                     objPgrmWorkout.WorkoutName = dataContext.Challenge.Where(c => c.ChallengeId == pww.WorkoutChallengeId).Select(ch => ch.ChallengeName).FirstOrDefault();
                                     weekWorkouList.Add(objPgrmWorkout);
                                 });
                                objProgramWeekWorkout.WeekId = pw.PWRocordId;
                                objProgramWeekWorkout.WeekWorkoutList = weekWorkouList;
                                programWeekWorkouList.Add(objProgramWeekWorkout);
                            });
                        challenge.SetProgramWeekWorkoutList(programWeekWorkouList);

                    }
                    return challenge;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetProgramWorkoutById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Copy of Program By exitsing program Id
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="userType"></param>
        /// <param name="credentialId"></param>
        /// <returns></returns>
        public static int CreateCopyProgramById(int programId, string userType, int credentialId)   
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        /*Get challenge detail by GetCopyProgrameById*/
                        traceLog.AppendLine("Start: GetCopyProgrameById for retrieving challenge by programId:" + programId+ ",userType-" + userType);
                        tblChallenge copyProgram = null;
                        int newProgramId = 0;
                        tblChallenge programdDetails = dataContext.Challenge.Find(programId);
                        if (programdDetails != null)
                        {
                            copyProgram = new tblChallenge
                            {
                                TrainerId = programdDetails.TrainerId,
                                IsActive = programdDetails.IsActive,
                                IsDraft = programdDetails.IsDraft,
                                Description = programdDetails.Description,
                                DifficultyLevel = programdDetails.DifficultyLevel,
                                FFChallengeDuration = programdDetails.FFChallengeDuration,
                                GlobalResultFilterValue = programdDetails.GlobalResultFilterValue,
                                IsPremium = programdDetails.IsPremium,
                                ProgramImageUrl = programdDetails.ProgramImageUrl,
                                VariableValue = programdDetails.VariableValue,
                                ChallengeName = programdDetails.ChallengeName + ConstantHelper.constChallengeCopyAppenedName,
                                ChallengeSubTypeId = programdDetails.ChallengeSubTypeId,
                                ChallengeDetail = programdDetails.ChallengeDetail,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                CreatedBy = credentialId,
                                ModifiedBy = credentialId,
                                IsSubscription = programdDetails.IsSubscription,
                                IsFeatured = programdDetails.IsFeatured,
                                FeaturedImageUrl = programdDetails.FeaturedImageUrl,
                            };
                            dataContext.Challenge.Add(copyProgram);
                            dataContext.SaveChanges();
                            newProgramId = copyProgram.ChallengeId;
                            /*Get exercise detail for the respective challenge*/
                            List<tblPWAssociation> objPWAssociationnList = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == programId).ToList();
                            objPWAssociationnList.ForEach(pw =>
                            {
                                tblPWAssociation objProgramWeekWorkout = new tblPWAssociation();
                                objProgramWeekWorkout.ProgramChallengeId = newProgramId;
                                objProgramWeekWorkout.AssignedDifficulyLevelId = pw.AssignedDifficulyLevelId;
                                objProgramWeekWorkout.AssignedTrainerId = pw.AssignedTrainerId;
                                objProgramWeekWorkout.AssignedTrainingzone = pw.AssignedTrainingzone;
                                objProgramWeekWorkout.CreatedBy = credentialId;
                                objProgramWeekWorkout.CreatedDate = DateTime.Now;
                                objProgramWeekWorkout.ModifiedBy = credentialId;
                                objProgramWeekWorkout.ModifiedDate = DateTime.Now;
                                dataContext.PWAssociation.Add(objProgramWeekWorkout);
                                dataContext.SaveChanges();
                                long newPWRocordId = objProgramWeekWorkout.PWRocordId;
                                List<tblPWWorkoutsAssociation> prmWAssociationList = dataContext.PWWorkoutsAssociation.Where(pww => pww.PWRocordId == pw.PWRocordId).ToList();
                                List<tblPWWorkoutsAssociation> weekWorkouList = new List<tblPWWorkoutsAssociation>();
                                prmWAssociationList.ForEach(pww =>
                                {
                                    var objPgrmWorkout = new tblPWWorkoutsAssociation();
                                    objPgrmWorkout.WorkoutChallengeId = pww.WorkoutChallengeId;
                                    objPgrmWorkout.PWRocordId = newPWRocordId;
                                    objPgrmWorkout.CreatedBy = credentialId;
                                    objPgrmWorkout.CreatedDate = DateTime.Now;
                                    objPgrmWorkout.ModifiedBy = credentialId;
                                    objPgrmWorkout.ModifiedDate = DateTime.Now;
                                    weekWorkouList.Add(objPgrmWorkout);
                                });
                                dataContext.PWWorkoutsAssociation.AddRange(weekWorkouList);
                                dataContext.SaveChanges();
                            });

                            if (newProgramId > 0)
                            {
                                List<DDTeams> selecetdTeams = CommonReportingUtility.GetSelectedTeamByChallengeId(dataContext, programId, true);
                                if (selecetdTeams != null && selecetdTeams.Count > 0)
                                {
                                    List<tblNoTrainerChallengeTeam> objtblNoTrainerChallengeTeam = new List<tblNoTrainerChallengeTeam>();
                                    selecetdTeams.ForEach(notrainer =>
                                    {
                                        objtblNoTrainerChallengeTeam.Add(new tblNoTrainerChallengeTeam() { TeamId = notrainer.TeamId, IsProgram = true, ChallengeId = newProgramId ,IsFittnessTest=false});
                                    });
                                    dataContext.NoTrainerChallengeTeams.AddRange(objtblNoTrainerChallengeTeam);
                                }
                                List<TrendingCategory> selecetdTrendingCategory = CommonReportingUtility.GetChallengeTrendingAssociationsList(dataContext, programId, ConstantHelper.constProgramChallengeSubType, true);
                                if (selecetdTrendingCategory != null && selecetdTrendingCategory.Count > 0)
                                {
                                    List<tblChallengeTrendingAssociation> lstChallengeTrendingAssociation = new List<tblChallengeTrendingAssociation>();
                                    selecetdTrendingCategory.ForEach(trendCat =>
                                    {
                                        lstChallengeTrendingAssociation.Add(new tblChallengeTrendingAssociation() { TrendingCategoryId = trendCat.TrendingCategoryId, IsProgram = true, ChallengeId = newProgramId });
                                    });
                                    dataContext.ChallengeTrendingAssociations.AddRange(lstChallengeTrendingAssociation);
                                }
                                List<ChallengeCategory> selecetdChallengeCategory = CommonReportingUtility.GetSelectedChallengeCategoryAssociations(dataContext, programId, true);
                                if (selecetdChallengeCategory != null && selecetdChallengeCategory.Count > 0)
                                {
                                    List<tblChallengeCategoryAssociation> listChallengeCategoryAssociation = new List<tblChallengeCategoryAssociation>();
                                    selecetdChallengeCategory.ForEach(progCat =>
                                    {
                                        listChallengeCategoryAssociation.Add(new tblChallengeCategoryAssociation() { ChallengeCategoryId = progCat.ChallengeCategoryId, IsProgram = true, ChallengeId = newProgramId });
                                    });
                                    dataContext.ChallengeCategoryAssociations.AddRange(listChallengeCategoryAssociation);
                                }
                                dataContext.SaveChanges();
                            }
                        }
                        dbTran.Commit(); 
                        return newProgramId;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("GetChallangeById  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
    }
}