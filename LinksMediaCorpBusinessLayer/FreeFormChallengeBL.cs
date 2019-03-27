
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using System.IO;
using System.Globalization;
using System.Web.Mvc;
using LinksMediaCorpUtility;
using AutoMapper;
using System.Web;
using System.Drawing;
using System.Data.Entity.Validation;
using LinksMediaCorpUtility.Resources;

namespace LinksMediaCorpBusinessLayer
{
    public class FreeFormChallengeBL
    {
        /// <summary>
        /// Create free form Challege from Admin or tariner.
        /// </summary>
        /// <param name="objFFChallenge"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        /// <param name="draft"></param>
        public static void SubmitFreeFormChallenge(FreeFormChallengeVM objFFChallenge, int credentialId, int trainerId, string draft)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: SubmitFreeFormChallenge for creating challenge");
                        tblChallenge objChalange = new tblChallenge();
                        objChalange.CreatedBy = credentialId;
                        if (!trainerId.Equals(0))
                        {
                            objChalange.TrainerId = trainerId;
                        }
                        if (objFFChallenge != null)
                        {
                            objChalange.Description = objFFChallenge.Description;
                            objChalange.ChallengeName = objFFChallenge.ChallengeName;
                            objChalange.ModifiedBy = objChalange.CreatedBy;
                            objChalange.CreatedDate = DateTime.Now;
                            objChalange.ModifiedDate = DateTime.Now;
                            string fitchallengeType = ConstantHelper.FreeFormChallengeType;
                            objChalange.ChallengeSubTypeId = dataContext.ChallengeType.FirstOrDefault(ct => ct.ChallengeType == fitchallengeType).ChallengeSubTypeId;
                            tblChallenge checkChallenge = dataContext.Challenge.Find(objFFChallenge.ChallengeId);
                            if (draft.Equals(Message.SavetoDraft))
                            {
                                objChalange.IsDraft = true;
                            }
                            else
                            {
                                objChalange.IsDraft = false;
                            }
                            var challengeId = 0;
                            if (checkChallenge == null)
                            {
                                dataContext.Challenge.Add(objChalange);
                                dataContext.SaveChanges();
                                challengeId = Convert.ToInt32(dataContext.Challenge.Max(x => x.ChallengeId));
                            }
                            else
                            {
                                checkChallenge.Description = objFFChallenge.Description;
                                checkChallenge.ChallengeName = objFFChallenge.ChallengeName;
                                checkChallenge.ModifiedBy = credentialId;
                                checkChallenge.ModifiedDate = DateTime.Now;
                                checkChallenge.IsDraft = objChalange.IsDraft;
                                dataContext.SaveChanges();
                                challengeId = objFFChallenge.ChallengeId;
                            }


                            ///*set result to userresult table*/
                            List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == objFFChallenge.ChallengeId).ToList();
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            dataContext.SaveChanges();
                            if (!string.IsNullOrEmpty(objFFChallenge.ExeName1))
                            {
                                ExeciseChallengeCEVM objExeciseChallengeCEVM = new ExeciseChallengeCEVM
                                {
                                    ExeName = objFFChallenge.ExeName1,
                                    ExeDesc = objFFChallenge.ExeDesc1,
                                    ChallengeId = challengeId,
                                    UserId = credentialId,
                                    Reps = objFFChallenge.Reps1,
                                    WeightForMan = objFFChallenge.WeightForMan1,
                                    WeightForWoman = objFFChallenge.WeightForWoman1
                                };
                                ChallengesBL.SubmitCEAssociation(objExeciseChallengeCEVM);
                            }

                            dbTran.Commit();
                        }
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("SubmitChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objFFChallenge = null;
                    }
                }
            }
        }
        /// <summary>
        /// Create the Free Form challenge
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public static bool CreateFreeFormChallenge(FreeFormChallenges objFreeFormChallenges)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: Create FreeForm Challenge by devices");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblChallenge objChalange = new tblChallenge();
                    objChalange.CreatedBy = cred.Id;
                    objChalange.IsActive = false;
                    objChalange.IsDraft = false;
                    objChalange.TrainerId = cred.Id;
                    objChalange.Description = objFreeFormChallenges.Description;
                    objChalange.ChallengeName = objFreeFormChallenges.ChallengeName;
                    objChalange.ModifiedBy = cred.Id;
                    objChalange.CreatedDate = DateTime.Now;
                    objChalange.ModifiedDate = DateTime.Now;
                    objChalange.DifficultyLevel = ConstantHelper.FreeFormChallengeDifficultyLevel;
                    string freechallengeType = ConstantHelper.FreeFormChallengeType;
                    objChalange.ChallengeSubTypeId = dataContext.ChallengeType.FirstOrDefault(ct => ct.ChallengeType == freechallengeType).ChallengeSubTypeId;
                    dataContext.Challenge.Add(objChalange);
                    int isSuccess = dataContext.SaveChanges();
                    if (isSuccess > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("SubmitChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get FreeForm Challange By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="objSelectList"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static FreeFormChallengeVM GetFreeFormChallangeById(int Id, ref List<SelectListItem> objSelectList)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetChallangeById for retrieving challenge by challengeid:" + Id);
                    tblChallenge challenge = dataContext.Challenge.Find(Id);
                    Mapper.CreateMap<tblChallenge, FreeFormChallengeVM>();
                    FreeFormChallengeVM objChalange =
                        Mapper.Map<tblChallenge, FreeFormChallengeVM>(challenge);
                    /*Get exercise detail for the respective challenge*/
                    List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == Id).ToList();
                    for (int i = 0; i < objCEAssociationList.Count; i++)
                    {
                        tblExercise exercise = dataContext.Exercise.Find(objCEAssociationList[i].ExerciseId);
                        if (i == 0)
                        {
                            objChalange.ExeDesc1 = objCEAssociationList[i].Description;
                            objChalange.ExeName1 = exercise != null ? exercise.ExerciseName : string.Empty;
                            objChalange.ExeVideoLink1 = exercise != null ? CommonUtility.VirtualFitComExercisePath +
                                Message.ExerciseVideoDirectory + exercise.VideoLink : string.Empty;
                            objChalange.ExeVideoUrl1 = exercise != null ? CommonUtility.VirtualFitComExercisePath +
                                Message.ExerciseVideoDirectory + exercise.VideoLink.Replace(" ", "%20") : string.Empty;
                        }
                    }
                    if (objChalange.TrainerId != null)
                        objChalange.TrainerCredntialId = dataContext.Credentials.Where(ce => ce.Id == (int)objChalange.TrainerId).Select(y => y.UserId).FirstOrDefault();
                    objChalange.TrainerId = null;
                    /*return fraction list*/
                    if (objCEAssociationList.Count > 1)
                    {
                        objSelectList = ChallengesCommonBL.GetFraction(objCEAssociationList.Count);
                    }
                    return objChalange;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChallangeById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update the Free form challemge by Admin
        /// </summary>
        /// <param name="objFreeFormChallengeVM"></param>
        /// <param name="credentialId"></param>
        public static void UpdateFreeFormChallenges(FreeFormChallengeVM objFreeFormChallengeVM, int credentialId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: UpdateChallenges for updaing challenge ");
                        List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == objFreeFormChallengeVM.ChallengeId).ToList();
                        if (objCEAssociationList != null)
                        {
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            dataContext.SaveChanges();
                        }
                        var challengeId = objFreeFormChallengeVM.ChallengeId;
                        if (!string.IsNullOrEmpty(objFreeFormChallengeVM.ExeName1))
                        {
                            ExeciseChallengeCEVM objExeciseChallengeCEVM = new ExeciseChallengeCEVM
                            {
                                ExeName = objFreeFormChallengeVM.ExeName1,
                                ExeDesc = objFreeFormChallengeVM.ExeDesc1,
                                ChallengeId = challengeId,
                                UserId = credentialId,
                                Reps = objFreeFormChallengeVM.Reps1,
                                WeightForMan = objFreeFormChallengeVM.WeightForMan1,
                                WeightForWoman = objFreeFormChallengeVM.WeightForWoman1
                            };
                            ChallengesBL.SubmitCEAssociation(objExeciseChallengeCEVM);
                        }
                        /*Update challenge*/
                        tblChallenge checkChallenge = dataContext.Challenge.Find(objFreeFormChallengeVM.ChallengeId);
                        checkChallenge.Description = objFreeFormChallengeVM.Description;
                        checkChallenge.ChallengeName = objFreeFormChallengeVM.ChallengeName;
                        checkChallenge.IsActive = objFreeFormChallengeVM.IsActive;
                        int trainerCredentialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId
                                == objFreeFormChallengeVM.TrainerCredntialId && m.UserType == Message.UserTypeTrainer)
                                .Select(y => y.Id).FirstOrDefault());
                        checkChallenge.TrainerId = trainerCredentialId;
                        checkChallenge.ModifiedBy = credentialId;
                        checkChallenge.ModifiedDate = DateTime.Now;
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
                        traceLog.AppendLine("UpdateFreeFormChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objFreeFormChallengeVM = null;
                    }
                }
            }
        }
        /// <summary>
        /// Get FreeForm Challenges for retrieving Free form challenges from database
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<FreeFormChallengeVM> GetFreeFormTrainerChallenges(int trainerId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormTrainerChallenges for retrieving Free form challenges from database ");
                    List<FreeFormChallengeVM> challenges = new List<FreeFormChallengeVM>();
                    string challengeType = ConstantHelper.FreeFormChallengeType;
                    bool currentAdminUser = false;
                    if (userType == Message.UserTypeAdmin)
                    {
                        currentAdminUser = true;
                    }
                    var query = from C in dataContext.Challenge
                                join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                where CT.ChallengeType == challengeType
                                orderby C.ModifiedDate descending
                                select new FreeFormChallengeVM
                                {
                                    TrainerId = C.TrainerId,
                                    Description = C.Description,
                                    ChallengeName = C.ChallengeName,
                                    ChallengeId = C.ChallengeId,
                                    ChallengeStaus = C.IsActive ? ConstantHelper.constApproved : ConstantHelper.constPending,
                                    IsAdminUser = currentAdminUser,
                                    IsDrafted = C.IsDraft,
                                    Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                };
                    /*challenges for all trainer else for related trainer*/
                    if (trainerId == -1)
                    {
                        challenges = query.ToList<FreeFormChallengeVM>();
                    }
                    else
                    {
                        challenges = query.Where(q => q.TrainerId == trainerId).ToList<FreeFormChallengeVM>();
                    }
                    challenges.ForEach(ch =>
                    {
                        ch.Description = CommonUtility.RemoveHTMLTags(ch.Description);
                    });
                    if (userType == Message.UserTypeAdmin)
                    {
                        challenges = challenges.Where(ch => ch.IsDrafted != true).ToList();
                    }
                    return challenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeFormTrainerChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Free form challenge By ID
        /// </summary>
        /// <param name="challengeID"></param>
        /// <returns></returns>
        public static FreeFormChallenges GetFreeFormChallengeById(int challengeID)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormChallengeById for retrieving Free form challenge from database:-challengeID-" + challengeID.ToString());
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where C.ChallengeId == challengeID
                                 orderby C.ModifiedDate descending
                                 select new FreeFormChallenges
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ChallengeName = C.ChallengeName,
                                     ChallengeId = C.ChallengeId
                                 }).FirstOrDefault();
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeFormChallengeById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get  all active FreeForm Challenges for devices
        /// </summary>
        /// <returns></returns>
        public static List<FreeFormChallenges> GetUnAssignedFreeFormChallenges()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormChallenges for retrieving Free form challenge from database");
                    string challengeType = ConstantHelper.FreeFormChallengeType;
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeType == challengeType && C.IsActive && C.TrainerId == 0
                                 orderby C.ModifiedDate descending
                                 select new FreeFormChallenges
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ChallengeName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeId = C.ChallengeId,
                                     ChallengeType = CT.ChallengeType,
                                     Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                 }).ToList();
                    //Challenge feed sorted by acceptors
                    query.ForEach(r =>
                    {
                        r.ChallengeType = r.ChallengeType.Split(' ')[0];
                    });
                    query = query.OrderBy(chlng => chlng.ChallengeName).ToList();
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeFormChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Free form Workouts Challenges By CategoryId and SubCategory Id
        /// </summary>
        /// <returns></returns>
        public static List<FreeFormChallenges> GetWorkoutChallengesByCategoryId(WorkoutChallengeRequest model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormChallenges for retrieving Free form challenge from database");
                    List<FreeFormChallenges> query = null;
                    List<int> teamtrainerIds = new List<int>();
                    List<int> teamIds = new List<int>();
                    int primaryTeamId = 0;
                    if (model.IsPremiumWorkout)
                    {
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
                        if (teamIds.Count > 0)
                        {
                            teamtrainerIds = (from crd in dataContext.Credentials
                                              join tms in dataContext.TrainerTeamMembers
                                              on crd.Id equals tms.UserId
                                              where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                              select tms.UserId).ToList();

                        }
                    }
                    else
                    {
                        teamtrainerIds.Add(0);
                    }
                    if (model.WorkoutCategoryID > 0)
                    {
                        string challengeType = ConstantHelper.FreeFormChallengeType;
                        bool isShownNoTrainerWorkoutProgram = false;
                        bool isDefaultTeam = false;
                        if (primaryTeamId > 0)
                        {
                            var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                            if (primaryTeam != null)
                            {
                                isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                                isDefaultTeam = primaryTeam.IsDefaultTeam;
                            }
                        }
                        if (isDefaultTeam)
                        {
                            //teamtrainerIds.Clear();
                            if (teamtrainerIds != null)
                            {
                                teamtrainerIds.Clear();
                            }
                            else
                            {
                                teamtrainerIds = new List<int>();
                            }
                            teamtrainerIds.Add(ConstantHelper.constDefaultFitcomTrainer);
                        }
                        query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where C.IsActive
                                 && CT.ChallengeType == challengeType
                                 && CT.ChallengeSubTypeId == model.WorkoutCategoryID
                                 && ((C.TrainerId > 0 && teamtrainerIds.Contains(C.TrainerId) && C.IsPremium) || (C.IsPremium && C.TrainerId == 0))
                                 orderby C.ChallengeName ascending
                                 select new FreeFormChallenges
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ChallengeName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     Duration = C.FFChallengeDuration,
                                     ChallengeId = C.ChallengeId,
                                     ChallengeType = CT.ChallengeType,
                                     IsSubscription = C.IsSubscription,
                                    // NoTrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(wt => wt.ChallengeId == C.ChallengeId).Select(tt => tt.TeamId).ToList(),
                                     ChalengeCategoryList = (from chgcat in dataContext.ChallengeCategoryAssociations
                                                             where chgcat.ChallengeId == C.ChallengeId
                                                             && chgcat.IsProgram == (C.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                                                             select chgcat.ChallengeCategoryId).Distinct().ToList<int>(),
                                     IsWelness = (CT.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false,
                                 }).ToList();
                        // Remove duplicate challenge
                        if (query != null)
                        {
                            if (isDefaultTeam)
                            {
                                query = query.Where(ct => ct.ChalengeCategoryList != null && ct.ChalengeCategoryList.Contains(model.WorkoutSubCategoryID)
                                   &&(ct.TrainerId > 0 || (!(ct.TrainerId > 0) && isShownNoTrainerWorkoutProgram))).ToList();
                            }
                            else
                            {
                                query = query.Where(ct => ct.ChalengeCategoryList != null && ct.ChalengeCategoryList.Contains(model.WorkoutSubCategoryID)
                                && (ct.TrainerId > 0 || (ct.TrainerId == 0 && isShownNoTrainerWorkoutProgram ))).ToList();
                            }
                            if (query != null)
                            {
                                query = query.GroupBy(c => c.ChallengeId).Select(chg => chg.FirstOrDefault()).ToList();
                            }
                        }
                        //Challenge feed sorted by acceptors
                        query.ForEach(r =>
                        {
                            r.ExeciseVideoDetails = (from cexe in dataContext.CEAssociation
                                                     join ex in dataContext.Exercise
                                                     on cexe.ExerciseId equals ex.ExerciseId
                                                     where cexe.ChallengeId == r.ChallengeId
                                                     orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                                     select new ExeciseVideoDetail
                                                     {
                                                         ExeciseName = ex.ExerciseName,
                                                         ExerciseThumnail = ex.ThumnailUrl,
                                                         ExeciseUrl = ex.V720pUrl,
                                                         ChallengeExeciseId = cexe.RocordId
                                                     }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault();
                            r.TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                join bp in dataContext.BodyPart
                                                on trzone.PartId equals bp.PartId
                                                where trzone.ChallengeId == r.ChallengeId
                                                select bp.PartName).Distinct().ToList<string>();
                            r.TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                join bp in dataContext.Equipments
                                                on trzone.EquipmentId equals bp.EquipmentId
                                                where trzone.ChallengeId == r.ChallengeId
                                                select bp.Equipment).Distinct().ToList<string>();
                            r.Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == r.ChallengeId).Select(y => y.UserId).Distinct().Count();

                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                            if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                            {
                                r.TargetZone = string.Join(",", r.TempTargetZone);
                            }
                            r.TempTargetZone = null;
                            if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                            {
                                r.Equipment = string.Join(",", r.TempEquipments);
                            }
                            r.TempEquipments = null;

                            if (r.ExeciseVideoDetails != null)
                            {
                                if (string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseUrl))
                                {
                                    r.ExeciseVideoLink = !string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) ? CommonUtility.VirtualFitComExercisePath
                                        + Message.ExerciseVideoDirectory + r.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                                }
                                else
                                {
                                    r.ExeciseVideoLink = r.ExeciseVideoDetails.ExeciseUrl;
                                }

                                if (!string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(r.ExeciseVideoDetails.ExerciseThumnail))
                                {
                                    string thumnailName = r.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                    string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                    string thumnailHeight = string.Empty;
                                    string thumnailWidth = string.Empty;
                                    r.ThumbnailUrl = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                    CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                    r.ThumbNailHeight = thumnailHeight;
                                    r.ThumbNailWidth = thumnailWidth;
                                }
                                else
                                {
                                    r.ThumbNailHeight = string.Empty;
                                    r.ThumbNailWidth = string.Empty;
                                    r.ThumbnailUrl = r.ExeciseVideoDetails.ExerciseThumnail;
                                }
                            }
                        });
                        query = query.OrderBy(chlng => chlng.ChallengeName).ToList();
                    }
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeFormChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Primum Program list By Selected CategoryId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProgramVM> GetPrimumProgramByCategoryId(ProgramChallengeRequest model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetPrimumProgramByCategoryId for retrieving Free form challenge from database" + model.ProgramCategoryID);
                    List<ProgramVM> query = null;
                    List<int> teamtrainerIds = new List<int>();
                    List<int> teamIds = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int primaryTeamId = 0;
                    if (model.IsProgramPremium)
                    {
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
                        if (teamIds.Count > 0)
                        {
                            teamtrainerIds = (from crd in dataContext.Credentials
                                              join tms in dataContext.TrainerTeamMembers
                                              on crd.Id equals tms.UserId
                                              where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                              select tms.UserId).ToList();

                        }
                    }
                    else
                    {
                        teamtrainerIds.Add(0);
                    }
                    bool isShownNoTrainerWorkoutProgram = false;
                    bool isDefaultTeam = false;
                    if (primaryTeamId > 0)
                    {
                        var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                        if (primaryTeam != null)
                        {
                            isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                            isDefaultTeam = primaryTeam.IsDefaultTeam;
                        }
                    }
                    if (model.ProgramCategoryID > 0)
                    {
                        string challengeType = ConstantHelper.ProgramChallengeType;
                        if (isDefaultTeam)
                        {
                            if (teamtrainerIds != null)
                            {
                                teamtrainerIds.Clear();
                            }
                            else
                            {
                                teamtrainerIds = new List<int>();
                            }                            
                            teamtrainerIds.Add(ConstantHelper.constDefaultFitcomTrainer);
                        }
                        query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType
                                 on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 join NTW in dataContext.NoTrainerChallengeTeams on C.ChallengeId equals NTW.ChallengeId into tempNoTrainerWorkouts
                                 from notrainrs in tempNoTrainerWorkouts.DefaultIfEmpty()
                                 where CT.ChallengeType == challengeType
                                 && C.IsActive
                                 && ((C.TrainerId > 0 && teamtrainerIds.Contains(C.TrainerId) && C.IsPremium) || (C.IsPremium && ((int?)notrainrs.TeamId ?? 0) != 0 && notrainrs.TeamId == primaryTeamId))
                                 && CT.ChallengeSubTypeId == model.ProgramTypeID
                                 orderby C.ChallengeName ascending
                                 select new ProgramVM
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ProgramName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ProgramId = C.ChallengeId,
                                     ProgramType = CT.ChallengeType,
                                     IsSubscription = C.IsSubscription,
                                     NoTrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(wt => wt.ChallengeId == C.ChallengeId).Select(tt => tt.TeamId).ToList(),
                                     ProgramImageUrl = C.ProgramImageUrl,
                                     ChallengeCategoryList = (from chgcat in dataContext.ChallengeCategoryAssociations
                                                              where chgcat.ChallengeId == C.ChallengeId
                                                              && chgcat.IsProgram
                                                              select chgcat.ChallengeCategoryId
                                                             ).Distinct().ToList<int>()

                                 }).ToList();

                        // Remove duplicate program List
                        if (query != null)
                        {
                            if (isDefaultTeam)
                            {
                                query = query.Where(ct => ct.ChallengeCategoryList.Contains(model.ProgramCategoryID)
                                    && (ct.TrainerId > 0 || (!(ct.TrainerId > 0) && isShownNoTrainerWorkoutProgram ))).ToList();
                            }
                            else
                            {
                                query = query.Where(ct => ct.ChallengeCategoryList.Contains(model.ProgramCategoryID)
                                    && (ct.TrainerId > 0 || (ct.TrainerId == 0 && isShownNoTrainerWorkoutProgram ))).ToList();
                            }
                            if (query != null)
                            {
                                query = query.GroupBy(c => c.ProgramId).Select(chg => chg.FirstOrDefault()).ToList();
                            }
                        }
                        query.ForEach(r =>
                        {
                            r.IsActive = dataContext.UserActivePrograms.Any(uc => uc.ProgramId == r.ProgramId && !uc.IsCompleted && uc.UserCredId == cred.Id);
                            r.ProgramType = r.ProgramType.Split(' ')[0];
                            string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + r.ProgramImageUrl;
                            r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" +
                                Message.ProfilePicDirectory + r.ProgramImageUrl)) ?
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;
                            if (System.IO.File.Exists(filePath))
                            {
                                using (Bitmap objBitmap = new Bitmap(filePath))
                                {
                                    double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                    double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                    r.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                    r.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                                }
                            }
                            else
                            {
                                r.Height = string.Empty;
                                r.Width = string.Empty;
                            }
                        });
                    }
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetPrimumProgramByCategoryId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Free form challenge based on workout with wellness
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<FreeFormChallenges> GetFreeformUnassignedWorkoutChallengesByCategoryId(WorkoutChallengeRequest model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeformUnassignedWorkoutChallengesByCategoryId() for retrieving Free form challenge from database");
                    List<FreeFormChallenges> query = null;
                    //  List<int> SubChallengeChallengeList = new List<int>();
                    if (model.WorkoutCategoryID > 0)
                    {
                        string challengeType = ConstantHelper.FreeFormChallengeType;
                        query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeType == challengeType && C.IsActive
                                 && C.TrainerId == 0
                                 && CT.ChallengeSubTypeId == model.WorkoutCategoryID
                                 && !C.IsPremium
                                 orderby C.ChallengeName ascending
                                 select new FreeFormChallenges
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ChallengeName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeId = C.ChallengeId,
                                     ChallengeType = CT.ChallengeType,
                                     Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                     ChalengeCategoryList = dataContext.ChallengeCategoryAssociations.Where(cc => cc.ChallengeId == C.ChallengeId
                                         && cc.IsProgram == (ConstantHelper.constProgramChallengeSubType == C.ChallengeSubTypeId)).Select(ch => ch.ChallengeCategoryId).ToList(),
                                 }).ToList();

                        if (query != null)
                        {
                            query = query.Where(ch => ch.ChalengeCategoryList != null && ch.ChalengeCategoryList.Contains(model.WorkoutSubCategoryID)).ToList();
                        }
                        query.ForEach(r =>
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        });
                        if (query != null)
                        {
                            query = query.OrderBy(chlng => chlng.ChallengeName).ToList();
                        }
                    }
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeformUnassignedWorkoutChallengesByCategoryId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Free form Unassigned Program Challenges ByCategoryId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProgramVM> GetFreeformUnassignedProgramChallengesByCategoryId(ProgramChallengeRequest model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeformUnassignedProgramChallengesByCategoryId() for retrieving Free form challenge from database");
                    List<ProgramVM> query = null;
                    // List<int> SubChallengeChallengeList = new List<int>();
                    if (model.ProgramTypeID > 0)
                    {
                        string challengeType = ConstantHelper.FreeFormChallengeType;
                        if (model.ProgramTypeID == ConstantHelper.constProgramChallengeSubType)
                        {
                            challengeType = ConstantHelper.ProgramChallengeType;
                        }

                        query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType
                                 on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeType == challengeType && C.IsActive
                                 && C.TrainerId == 0
                                 && CT.ChallengeSubTypeId == model.ProgramTypeID
                                 && !C.IsPremium
                                 orderby C.ChallengeName ascending
                                 select new ProgramVM
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ProgramName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ProgramId = C.ChallengeId,
                                     ProgramType = CT.ChallengeType,
                                     IsSubscription = C.IsSubscription,
                                     ProgramImageUrl = C.ProgramImageUrl,
                                     ChallengeCategoryList = dataContext.ChallengeCategoryAssociations.Where(cc => cc.ChallengeId == C.ChallengeId && cc.IsProgram)
                                                                        .Select(ch => ch.ChallengeCategoryId).ToList()
                                 }).ToList();
                        if (query != null)
                        {
                            query = query.Where(ch => ch.ChallengeCategoryList != null && ch.ChallengeCategoryList.Contains(model.ProgramCategoryID)).ToList();

                        }
                        query.ForEach(r =>
                      {
                          r.ProgramType = r.ProgramType.Split(' ')[0];
                          r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" +
                              Message.ProfilePicDirectory + r.ProgramImageUrl)) ?
                              CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;

                      });

                    }
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeformUnassignedWorkoutChallengesByCategoryId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get the Wellness Free Form challenge based on wellness challenge sub type Id and  is it premium or not 
        /// </summary>
        /// <returns></returns>
        public static List<FreeFormChallenges> GetFreeformWellnessChallengeList(int workoutSubTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                string challengeType = ConstantHelper.FreeFormChallengeType;
                try
                {
                    traceLog.AppendLine("Start: GetFreeformWellnessChallengeList for SubCategoryId-" + workoutSubTypeId);
                    return (from C in dataContext.Challenge
                            join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                            where CT.ChallengeType == challengeType && C.IsActive && C.TrainerId == 0
                            && CT.ChallengeSubTypeId == workoutSubTypeId
                            && !C.IsPremium
                            orderby C.ModifiedDate descending
                            select new FreeFormChallenges
                            {
                                TrainerId = C.TrainerId,
                                Description = C.Description,
                                ChallengeName = C.ChallengeName,
                                DifficultyLevel = C.DifficultyLevel,
                                ChallengeId = C.ChallengeId,
                                ChallengeType = CT.ChallengeType,
                                Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                            }).ToList();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeformWellnessChallengeList  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Update Free form by ChallengeID
        /// </summary>
        /// <param name="objFreeFormChallenges"></param>
        /// <returns></returns>
        public static FreeFormChallenges UpdateFreeFormChallengeByID(FreeFormChallenges objFreeFormChallenges)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateFreeFormChallengeByID for updaing challenge:-ChallengeId " + objFreeFormChallenges.ChallengeId.ToString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    /*Update challenge*/
                    tblChallenge checkChallenge = dataContext.Challenge.Find(objFreeFormChallenges.ChallengeId);
                    checkChallenge.Description = objFreeFormChallenges.Description;
                    checkChallenge.ChallengeName = objFreeFormChallenges.ChallengeName;
                    checkChallenge.IsActive = objFreeFormChallenges.IsActive;
                    checkChallenge.TrainerId = objFreeFormChallenges.TrainerId ?? 0;
                    checkChallenge.ModifiedBy = cred.Id;
                    checkChallenge.ModifiedDate = DateTime.Now;
                    dataContext.SaveChanges();
                    return objFreeFormChallenges;
                }
                catch
                {

                    throw;
                }
                finally
                {
                    traceLog.AppendLine("UpdateFreeFormChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get FreeForm Challenges ByTrainer
        /// </summary>
        /// <returns></returns>
        public static List<FreeFormChallenges> GetFreeFormChallengesByTrainer()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormChallengesByTrainer() for retrieving Free form challenge by Trainer from database");
                    string challengeType = ConstantHelper.FreeFormChallengeType;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeType == challengeType && C.IsActive && C.TrainerId == cred.Id
                                 orderby C.ModifiedDate descending
                                 select new FreeFormChallenges
                                 {
                                     TrainerId = C.TrainerId,
                                     Description = C.Description,
                                     ChallengeName = C.ChallengeName,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeId = C.ChallengeId,
                                     Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                 }).ToList();


                    //Challenge feed sorted by acceptors
                    query = query.OrderByDescending(chlng => chlng.Strenght).ToList();
                    return query;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeFormChallengesByTrainer  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Submit FreeForm Challenge by Admin
        /// </summary>
        /// <param name="objInput"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        /// <param name="draft"></param>    
        public static void SubmitAdminFreeFormChallenge(CreateChallengeVM objCreateChallengeVM, int credentialId, int trainerId, string draft)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: SubmitAdminFreeFormChallenge for creating challenge");
                        Mapper.CreateMap<CreateChallengeVM, tblChallenge>();
                        tblChallenge objChalange = Mapper.Map<CreateChallengeVM, tblChallenge>(objCreateChallengeVM);

                        objChalange.CreatedBy = credentialId;
                        if (!trainerId.Equals(0))
                        {
                            objChalange.TrainerId = trainerId;
                        }
                        objChalange.ModifiedBy = objChalange.CreatedBy;
                        objChalange.CreatedDate = DateTime.Now;
                        objChalange.ModifiedDate = objChalange.CreatedDate;
                        if (draft.Equals(Message.SavetoDraft, StringComparison.OrdinalIgnoreCase))
                        {
                            objChalange.IsDraft = true;
                        }
                        else
                        {
                            objChalange.IsDraft = false;
                        }
                        tblChallenge checkChallenge = dataContext.Challenge.Find(objCreateChallengeVM.ChallengeId);
                        int challengeId = 0;
                        if (checkChallenge == null)
                        {
                            dataContext.Challenge.Add(objChalange);
                            dataContext.SaveChanges();
                            challengeId = Convert.ToInt32(dataContext.Challenge.Max(x => x.ChallengeId));
                        }
                        else
                        {
                            dataContext.Entry(checkChallenge).CurrentValues.SetValues(objChalange);
                            dataContext.SaveChanges();
                            challengeId = objCreateChallengeVM.ChallengeId;
                        }
                        /*set result to userresult table*/
                        var objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objCEAssociationList != null)
                        {
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                        }
                        dataContext.SaveChanges();
                        traceLog.AppendLine("Start: Saves changes for each execise in free form challenges");
                        // Saves changes for checkbox uncheck and save on new execise im master table                                                
                        if (objCreateChallengeVM.IsFFAExeName1 && !string.IsNullOrEmpty(objCreateChallengeVM.FFAExeName1))
                        {
                            // Add the aletrnate execise with inactive status
                            if (!dataContext.Exercise.Any(ex => ex.ExerciseName == objCreateChallengeVM.FFAExeName1))
                            {
                                objCreateChallengeVM.FFExeName1 = objCreateChallengeVM.FFAExeName1;
                            }
                        }
                        // Add unlimited Execise Type
                        traceLog.AppendLine("Start: SubmitCEAssociation for submitting Free form challenges exercise in to database");
                        if ((!string.IsNullOrEmpty(objCreateChallengeVM.FFExeName1)) || objCreateChallengeVM.IsFFAExeName1)
                        {
                            int exeId = -1;
                            tblExercise exercisedetails = null;
                            if (objCreateChallengeVM.ExerciseId1 > 0 && ! objCreateChallengeVM.IsFFAExeName1)
                            {
                                exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseId == objCreateChallengeVM.ExerciseId1);
                            }
                            else
                            {
                                 exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == objCreateChallengeVM.FFExeName1 || c.ExerciseName == objCreateChallengeVM.FFAExeName1);

                            }
                            if (exercisedetails != null)
                            {
                                exeId = exercisedetails.ExerciseId;
                            }
                            if (!string.IsNullOrEmpty(objCreateChallengeVM.FFExeName1) || !string.IsNullOrEmpty(objCreateChallengeVM.FFAExeName1))
                            {
                                tblCEAssociation objECAssociation = new tblCEAssociation();
                                objECAssociation.ChallengeId = challengeId;
                                objECAssociation.ExerciseId = exeId;
                                objECAssociation.CreatedBy = objChalange.CreatedBy;
                                objECAssociation.Reps = 0;
                                objECAssociation.WeightForMan = 0;
                                objECAssociation.WeightForWoman = 0;
                                objECAssociation.CreatedDate = DateTime.Now;
                                objECAssociation.ModifiedBy = objECAssociation.CreatedBy;
                                objECAssociation.ModifiedDate = objECAssociation.CreatedDate;
                                objECAssociation.IsAlternateExeciseName = objCreateChallengeVM.IsFFAExeName1;
                                objECAssociation.AlternateExeciseName = objCreateChallengeVM.IsFFAExeName1 ? objCreateChallengeVM.FFAExeName1 : string.Empty;
                                objECAssociation.SelectedEquipment = objCreateChallengeVM.SelectedEquipment1;
                                objECAssociation.SelectedTraingZone = objCreateChallengeVM.SelectedTrainingZone1;
                                objECAssociation.IsShownFirstExecise = objCreateChallengeVM.IsSetFirstExecise;
                                objECAssociation.SelectedExeciseType = objCreateChallengeVM.SelectedExeciseType1;
                                // challengeId = objInput.ChallengeId;
                                dataContext.CEAssociation.Add(objECAssociation);
                                dataContext.SaveChanges();
                                List<tblCESAssociation> savedSetdata = SaveExeciseSetData(objCreateChallengeVM.FFExeDesc1, objECAssociation.RocordId, objECAssociation.CreatedBy);
                                if (savedSetdata != null)
                                {
                                    dataContext.CESAssociations.AddRange(savedSetdata);
                                    //  dataContext.Entry(savedSetdata).State = System.Data.Entity.EntityState.Modified;  
                                    dataContext.SaveChanges();
                                }
                            }
                        }
                        /*primary specialization*/
                        if (!string.IsNullOrEmpty(objCreateChallengeVM.FreeFormExerciseNameDescriptionList))
                        {
                            string[] stringSeparators = new string[] { ConstantHelper.constSeperatorBarPipe };
                            string[] selecetedExeecisesList = objCreateChallengeVM.FreeFormExerciseNameDescriptionList.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (selecetedExeecisesList != null && selecetedExeecisesList.Count() > 0)
                            {
                                for (int i = 0; i < selecetedExeecisesList.Count(); i++)
                                {
                                    if (!string.IsNullOrEmpty(selecetedExeecisesList[i]))
                                    {
                                        string[] selecetedExeeciseList = selecetedExeecisesList[i].Split(new char[1] { '~' });
                                        int execiseCount = selecetedExeeciseList != null ? selecetedExeeciseList.Length : 0;
                                        string execiseName = execiseCount > 0 ? selecetedExeeciseList[0] : string.Empty;
                                        string description = execiseCount > 1 ? selecetedExeeciseList[1] : string.Empty;
                                        string isAlternateEName = execiseCount > 3 ? selecetedExeeciseList[3] : "false";
                                        string alternateEName = execiseCount > 4 ? selecetedExeeciseList[4] : string.Empty;
                                        string selectedEquipemet = execiseCount > 5 ? selecetedExeeciseList[5] : string.Empty;
                                        string selectedTrainingZone = execiseCount > 6 ? selecetedExeeciseList[6] : string.Empty;
                                        string selectedExeciseType = execiseCount > 7 ? selecetedExeeciseList[7] : string.Empty;
                                        string chechedFirstExecise = execiseCount > 8 ? selecetedExeeciseList[8] : "false";
                                        string selectedExeciseId= execiseCount > 9 ? selecetedExeeciseList[9] : "0";
                                        bool isAlterExeciseName = string.IsNullOrEmpty(isAlternateEName) == true ? false : bool.Parse(isAlternateEName);
                                        bool isChechedFirstExecise = string.IsNullOrEmpty(chechedFirstExecise) == true ? false : bool.Parse(chechedFirstExecise);
                                        if ((!string.IsNullOrEmpty(execiseName) && execiseName != ConstantHelper.constFFChallangeDescription) || isAlterExeciseName)
                                        {
                                            if (!string.IsNullOrEmpty(description) && description == ConstantHelper.constFFChallangeDescription)
                                            {
                                                description = string.Empty;
                                            }
                                            // Add Execise in master execise with Alternate
                                            if (isAlterExeciseName && !string.IsNullOrEmpty(alternateEName))
                                            {
                                                // Add the aletrnate execise with inactive status
                                                execiseName = alternateEName;
                                            }
                                            // For Alternate Name exeicuse id will be -1
                                            int exeId = -1;
                                            // var exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == execiseName);
                                            selectedExeciseId = (selectedExeciseId == ConstantHelper.constFFChallangeDescription) ? "0" : selectedExeciseId;
                                            int exerciseId = Convert.ToInt32(selectedExeciseId);
                                            tblExercise execiseDetails = null;
                                            if (exerciseId > 0)
                                            {
                                                execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseId == exerciseId);
                                            }
                                            else
                                            {
                                                execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == execiseName);
                                            }
                                            if (execiseDetails != null)
                                            {
                                                exeId = execiseDetails.ExerciseId;
                                            }
                                            var objECAssociation = new tblCEAssociation();
                                            objECAssociation.ChallengeId = challengeId;
                                            objECAssociation.ExerciseId = exeId;
                                            objECAssociation.CreatedBy = objChalange.CreatedBy;
                                            objECAssociation.Reps = 0;
                                            objECAssociation.WeightForMan = 0;
                                            objECAssociation.WeightForWoman = 0;
                                            objECAssociation.CreatedDate = DateTime.Now;
                                            objECAssociation.ModifiedBy = objECAssociation.CreatedBy;
                                            objECAssociation.ModifiedDate = objECAssociation.CreatedDate;
                                            objECAssociation.IsAlternateExeciseName = isAlterExeciseName;
                                            objECAssociation.AlternateExeciseName = string.IsNullOrEmpty(alternateEName) == true ? string.Empty : alternateEName;
                                            objECAssociation.SelectedEquipment = selectedEquipemet;
                                            objECAssociation.SelectedTraingZone = selectedTrainingZone;
                                            objECAssociation.SelectedExeciseType = selectedExeciseType;
                                            objECAssociation.IsShownFirstExecise = isChechedFirstExecise;
                                            dataContext.CEAssociation.Add(objECAssociation);
                                            dataContext.SaveChanges();
                                            var savedSetdata = SaveExeciseSetData(description, objECAssociation.RocordId, objECAssociation.CreatedBy);
                                            if (savedSetdata != null)
                                            {
                                                dataContext.CESAssociations.AddRange(savedSetdata);
                                                dataContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Remove Old ExerciseType
                        var objETCAssociationList = dataContext.ETCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objETCAssociationList != null)
                        {
                            dataContext.ETCAssociations.RemoveRange(objETCAssociationList);
                        }
                        dataContext.SaveChanges();
                        /*Add PostedExerciseTypes information into database*/

                        if (objCreateChallengeVM.PostedExerciseTypes != null)
                        {
                            var ObjETCAssociationList = CommonReportingUtility.GetPostedExerciseTypeBasedChallenge(objCreateChallengeVM.PostedExerciseTypes, challengeId);
                            dataContext.ETCAssociations.AddRange(ObjETCAssociationList);
                        }
                        /*Training Zone*/
                        List<tblTrainingZoneCAssociation> objtblTrainingZoneCAssociationnList = dataContext.TrainingZoneCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        dataContext.TrainingZoneCAssociations.RemoveRange(objtblTrainingZoneCAssociationnList);
                        dataContext.SaveChanges();
                        if (objCreateChallengeVM.PostedTargetZones != null)
                        {
                            var addObjtblTrainingZoneCAssociationnList = CommonReportingUtility.GetPostedTargetZonesBasedChallenge(objCreateChallengeVM.PostedTargetZones, challengeId);
                            dataContext.TrainingZoneCAssociations.AddRange(addObjtblTrainingZoneCAssociationnList);
                        }
                        // Remove exixting trainer zone for challenge
                        List<tblCEquipmentAssociation> objtblCEquipmentAssociationList = dataContext.ChallengeEquipmentAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objtblCEquipmentAssociationList != null)
                        {
                            dataContext.ChallengeEquipmentAssociations.RemoveRange(objtblCEquipmentAssociationList);
                        }
                        dataContext.SaveChanges();
                        // Added new Equipment for challenge
                        if (objCreateChallengeVM.PostedEquipments != null)
                        {
                            var eqipmentCAssociationList = CommonReportingUtility.GetPostedEquipmentsBasedChallenge(objCreateChallengeVM.PostedEquipments, challengeId);
                            dataContext.ChallengeEquipmentAssociations.AddRange(eqipmentCAssociationList);
                        }
                        // Add all team except primary team
                        if (objChalange.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType && !(trainerId > 0))
                        {
                            if (objCreateChallengeVM.PostedTeams != null)
                            {
                                var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objCreateChallengeVM.PostedTeams, challengeId);
                                dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                            }
                        }
                        // Add PostedTrendingCategory in Database
                        if (objCreateChallengeVM.PostedTrendingCategory != null || objCreateChallengeVM.PostedSecondaryTrendingCategory != null)
                        {
                            var primarySelectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedTrendingCategory, challengeId);
                            var secondaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedSecondaryTrendingCategory, challengeId);
                            List< tblChallengeTrendingAssociation > allSelectedTrending = primarySelectedTrendingCategory != null ?primarySelectedTrendingCategory.Union(secondaryselectedTrendingCategory).ToList()
                                                                                          : secondaryselectedTrendingCategory;
                            dataContext.ChallengeTrendingAssociations.AddRange(allSelectedTrending);
                        }
                        if (objCreateChallengeVM.PostedChallengeCategory != null)
                        {
                            var selectedChallengeCategory = CommonReportingUtility.GetPostedChallengeCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedChallengeCategory, challengeId);
                            dataContext.ChallengeCategoryAssociations.AddRange(selectedChallengeCategory);
                        }

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
                        traceLog.AppendLine("SubmitChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objCreateChallengeVM = null;
                    }
                }
            }
        }
        /// <summary>
        /// Save Execise SetData based on assocites execise Name
        /// </summary>
        /// <param name="execisesetdetails"></param>
        /// <returns></returns>
        public static List<tblCESAssociation> SaveExeciseSetData(string execisesetdetails, int challengeExeciseId, int createdBy)
        {
            if (!string.IsNullOrEmpty(execisesetdetails))
            {
                List<tblCESAssociation> listtblCESAssociation = new List<tblCESAssociation>();
                string[] stringSeparators = new string[] { ConstantHelper.constNOTEQUAL };
                string[] selecetedExeecisesetList = execisesetdetails.Split(stringSeparators, StringSplitOptions.None);
                if (selecetedExeecisesetList != null && selecetedExeecisesetList.Count() > 0)
                {
                    for (int i = 0; i < selecetedExeecisesetList.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(selecetedExeecisesetList[i]))
                        {
                            string[] selecetedExeeciseList = selecetedExeecisesetList[i].Split(new char[1] { ConstantHelper.constSeperatorCapPipe });
                            int execisesetCount = selecetedExeeciseList != null ? selecetedExeeciseList.Length : 0;
                            int reps = 0;
                            if (!int.TryParse(selecetedExeeciseList[0], out reps))
                            {
                                reps = 0;
                            }
                            string resulttime = execisesetCount > 1 ? selecetedExeeciseList[1] : string.Empty;
                            string resttime = execisesetCount > 2 ? selecetedExeeciseList[2] : string.Empty;
                            string description = execisesetCount > 3 ? selecetedExeeciseList[3] : string.Empty;
                            string autocountdown = execisesetCount > 4 ? selecetedExeeciseList[4] : string.Empty;
                            bool autocountdownflag = false;
                            if (!string.IsNullOrEmpty(autocountdown))
                            {
                                autocountdownflag = autocountdown.Equals(ConstantHelper.constYes, StringComparison.OrdinalIgnoreCase);
                            }
                            resulttime = (string.IsNullOrEmpty(resulttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resulttime;
                            resttime = (string.IsNullOrEmpty(resttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resttime;
                            description = (string.IsNullOrEmpty(description) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : description;
                            if (reps > 0 || !string.IsNullOrEmpty(resulttime))
                            {
                                tblCESAssociation objtblCESAssociation = new tblCESAssociation();
                                objtblCESAssociation.SetReps = reps;
                                objtblCESAssociation.RestTime = resttime;
                                objtblCESAssociation.SetResult = resulttime;
                                objtblCESAssociation.Description = description;
                                objtblCESAssociation.CreatedBy = createdBy;
                                objtblCESAssociation.CreatedDate = DateTime.Now;
                                objtblCESAssociation.ModifiedDate = objtblCESAssociation.CreatedDate;
                                objtblCESAssociation.ModifiedBy = createdBy;
                                objtblCESAssociation.RecordId = challengeExeciseId;
                                objtblCESAssociation.IsRestType = reps > 0 ? true : false;
                                objtblCESAssociation.AutoCountDown = autocountdownflag;
                                listtblCESAssociation.Add(objtblCESAssociation);
                            }
                        }
                    }
                }
                return listtblCESAssociation;
            }
            return null;
        }

        /// <summary>
        /// Update FreeForm Challenges by Admin
        /// </summary>
        /// <param name="objCreateChallengeVM"></param>
        /// <param name="credentialId"></param>
        public static void UpdateAdminFreeFormChallenges(CreateChallengeVM objCreateChallengeVM, int credentialId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: UpdateChallenges for updaing challenge ");
                        List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objCEAssociationList != null)
                        {
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            List<tblCESAssociation> objCESAssociationList = (from challaExe in dataContext.CEAssociation
                                                                             join challaExeset in dataContext.CESAssociations
                                                                             on challaExe.RocordId equals challaExeset.RecordId
                                                                             where challaExe.ChallengeId == objCreateChallengeVM.ChallengeId
                                                                             select challaExeset).ToList();
                            if (objCESAssociationList != null && objCESAssociationList.Count > 0)
                            {
                                dataContext.CESAssociations.RemoveRange(objCESAssociationList);
                            }
                            dataContext.SaveChanges();
                        }
                        int challengeId = objCreateChallengeVM.ChallengeId;
                        /*Update challenge*/
                        tblChallenge checkChallenge = dataContext.Challenge.Find(objCreateChallengeVM.ChallengeId);
                        List<tblCEAssociation> objtblCEAssociationList = new List<tblCEAssociation>();
                        if ((!string.IsNullOrEmpty(objCreateChallengeVM.FFExeName1)) || objCreateChallengeVM.IsFFAExeName1)
                        {
                            if (objCreateChallengeVM.IsFFAExeName1 && !string.IsNullOrEmpty(objCreateChallengeVM.FFAExeName1))
                            {
                                objCreateChallengeVM.FFExeName1 = objCreateChallengeVM.FFAExeName1;
                            }
                        }
                        if ((!string.IsNullOrEmpty(objCreateChallengeVM.FFExeName1)) || objCreateChallengeVM.IsFFAExeName1)
                        {
                           
                            int exeId = -1;
                            tblExercise exercisedetails = null;
                            if (objCreateChallengeVM.ExerciseId1 > 0 && !objCreateChallengeVM.IsFFAExeName1)
                            {
                                exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseId == objCreateChallengeVM.ExerciseId1);
                            }
                            else
                            {
                                exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == objCreateChallengeVM.FFExeName1 || c.ExerciseName == objCreateChallengeVM.FFAExeName1);

                            }
                            
                            // var exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == objCreateChallengeVM.FFExeName1 || c.ExerciseName == objCreateChallengeVM.FFAExeName1);
                            if (exercisedetails != null)
                            {
                                exeId = exercisedetails.ExerciseId;
                            }
                            tblCEAssociation objECAssociation = new tblCEAssociation();
                            objECAssociation.ChallengeId = challengeId;
                            objECAssociation.ExerciseId = exeId;
                            objECAssociation.CreatedBy = credentialId;
                            objECAssociation.Reps = 0;
                            objECAssociation.WeightForMan = 0;
                            objECAssociation.WeightForWoman = 0;
                            objECAssociation.CreatedDate = DateTime.Now;
                            objECAssociation.ModifiedBy = objECAssociation.CreatedBy;
                            objECAssociation.ModifiedDate = objECAssociation.CreatedDate;
                            objECAssociation.IsAlternateExeciseName = objCreateChallengeVM.IsFFAExeName1;
                            objECAssociation.AlternateExeciseName = objCreateChallengeVM.IsFFAExeName1 ? objCreateChallengeVM.FFAExeName1 : string.Empty;
                            objECAssociation.SelectedEquipment = objCreateChallengeVM.SelectedEquipment1;
                            objECAssociation.SelectedTraingZone = objCreateChallengeVM.SelectedTrainingZone1;
                            objECAssociation.SelectedExeciseType = objCreateChallengeVM.SelectedExeciseType1;
                            objECAssociation.IsShownFirstExecise = objCreateChallengeVM.IsSetFirstExecise;
                            dataContext.CEAssociation.Add(objECAssociation);
                            dataContext.SaveChanges();
                            List<tblCESAssociation> savedSetdata = SaveExeciseSetData(objCreateChallengeVM.FFExeDesc1, objECAssociation.RocordId, objECAssociation.CreatedBy);
                            if (savedSetdata != null)
                            {
                                dataContext.CESAssociations.AddRange(savedSetdata);
                                dataContext.SaveChanges();
                            }
                        }
                        /*primary FreeFormExerciseNameDescriptionList*/
                        if (!string.IsNullOrEmpty(objCreateChallengeVM.FreeFormExerciseNameDescriptionList))
                        {
                            string[] selecetedExeecisesList = objCreateChallengeVM.FreeFormExerciseNameDescriptionList.Split(new char[1] { ConstantHelper.constSeperatorCharBarPipe });
                            if (selecetedExeecisesList != null && selecetedExeecisesList.Count() > 0)
                            {
                                for (int i = 0; i < selecetedExeecisesList.Count(); i++)
                                {
                                    if (!string.IsNullOrEmpty(selecetedExeecisesList[i]))
                                    {
                                        string[] selecetedExeeciseList = selecetedExeecisesList[i].Split(new char[1] { ConstantHelper.constSeperatorCharTildPipe });
                                        int execiseCount = selecetedExeeciseList != null ? selecetedExeeciseList.Length : 0;
                                        string execiseName = execiseCount > 0 ? selecetedExeeciseList[0] : string.Empty;
                                        string description = execiseCount > 1 ? selecetedExeeciseList[1] : string.Empty;
                                        string isAlternateEName = execiseCount > 3 ? selecetedExeeciseList[3] : "false";
                                        string alternateEName = execiseCount > 4 ? selecetedExeeciseList[4] : string.Empty;
                                        string selectedEquipemet = execiseCount > 6 ? selecetedExeeciseList[6] : string.Empty;
                                        string selectedTrainingZone = execiseCount > 7 ? selecetedExeeciseList[7] : string.Empty;
                                        string selectedExeciseType = execiseCount > 8 ? selecetedExeeciseList[8] : string.Empty;
                                        string checkFirstExecise = execiseCount > 10 ? selecetedExeeciseList[10] : "false";
                                        string selectedExeciseId = execiseCount > 11 ? selecetedExeeciseList[11] : "0";
                                        bool isAlternate = string.IsNullOrEmpty(isAlternateEName) == true ? false : bool.Parse(isAlternateEName);
                                        bool isFirstExecise = string.IsNullOrEmpty(checkFirstExecise) == true ? false : bool.Parse(checkFirstExecise);
                                        if ((!string.IsNullOrEmpty(execiseName) && execiseName != ConstantHelper.constFFChallangeDescription) || isAlternate)
                                        {
                                            if (!string.IsNullOrEmpty(description) && description == ConstantHelper.constFFChallangeDescription)
                                            {
                                                description = string.Empty;
                                            }
                                            if (isAlternate && !string.IsNullOrEmpty(alternateEName))
                                            {
                                                execiseName = alternateEName;
                                            }
                                            int exeId = -1;
                                            selectedExeciseId = (selectedExeciseId == ConstantHelper.constFFChallangeDescription) ? "0" : selectedExeciseId;
                                            int exerciseId = Convert.ToInt32(selectedExeciseId); 
                                            tblExercise execiseDetails = null;
                                            if (exerciseId > 0)
                                            {
                                                execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseId == exerciseId);
                                            }
                                            else {
                                                execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == execiseName);
                                            }
                                            if (execiseDetails != null)
                                            {
                                                exeId = execiseDetails.ExerciseId;
                                            }
                                            tblCEAssociation objECAssociation = new tblCEAssociation();
                                            objECAssociation.ChallengeId = challengeId;
                                            objECAssociation.ExerciseId = exeId;
                                            objECAssociation.CreatedBy = credentialId;
                                            objECAssociation.Reps = 0;
                                            objECAssociation.WeightForMan = 0;
                                            objECAssociation.WeightForWoman = 0;
                                            objECAssociation.CreatedDate = DateTime.Now;
                                            objECAssociation.ModifiedBy = objECAssociation.CreatedBy;
                                            objECAssociation.ModifiedDate = objECAssociation.CreatedDate;
                                            objECAssociation.IsAlternateExeciseName = string.IsNullOrEmpty(isAlternateEName) ? false : bool.Parse(isAlternateEName);
                                            objECAssociation.AlternateExeciseName = string.IsNullOrEmpty(alternateEName) ? string.Empty : alternateEName;
                                            objECAssociation.SelectedEquipment = selectedEquipemet;
                                            objECAssociation.SelectedTraingZone = selectedTrainingZone;
                                            objECAssociation.SelectedExeciseType = selectedExeciseType;
                                            objECAssociation.IsShownFirstExecise = isFirstExecise;
                                            objtblCEAssociationList.Add(objECAssociation);
                                            dataContext.CEAssociation.Add(objECAssociation);
                                            dataContext.SaveChanges();
                                            List<tblCESAssociation> savedSetdata = SaveExeciseSetData(description, objECAssociation.RocordId, objECAssociation.CreatedBy);
                                            if (savedSetdata != null)
                                            {
                                                dataContext.CESAssociations.AddRange(savedSetdata);
                                                dataContext.SaveChanges();
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        dataContext.SaveChanges();
                        if (!string.IsNullOrEmpty(objCreateChallengeVM.FeaturedImageUrl))
                        {
                            checkChallenge.FeaturedImageUrl = objCreateChallengeVM.FeaturedImageUrl;
                        }
                        else
                        {
                            objCreateChallengeVM.FeaturedImageUrl = checkChallenge.FeaturedImageUrl;
                        }
                        Mapper.CreateMap<CreateChallengeVM, tblChallenge>();
                        tblChallenge objChalange =
                        Mapper.Map<CreateChallengeVM, tblChallenge>(objCreateChallengeVM);
                        int trainerCredentialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId
                                 == objCreateChallengeVM.TrainerCredntialId && m.UserType == Message.UserTypeTrainer)
                                 .Select(y => y.Id).FirstOrDefault());
                        objChalange.TrainerId = trainerCredentialId;
                        objChalange.ModifiedBy = credentialId;
                        objChalange.ModifiedDate = DateTime.Now;
                        objChalange.IsActive = objCreateChallengeVM.IsActive;
                        objChalange.IsPremium = objCreateChallengeVM.IsPremium;
                        objChalange.IsSubscription = objCreateChallengeVM.IsSubscription;
                        objChalange.IsFeatured = objCreateChallengeVM.IsFeatured;
                        objChalange.FFChallengeDuration = objCreateChallengeVM.FFChallengeDuration;
                        objChalange.ChallengeDetail = objCreateChallengeVM.ChallengeDetail;
                        objChalange.ChallengeSubTypeId = objCreateChallengeVM.ChallengeSubTypeId;
                        dataContext.Entry(checkChallenge).CurrentValues.SetValues(objChalange);
                        dataContext.SaveChanges();
                        //Remove Old ExerciseType
                        var objETCAssociationList = dataContext.ETCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objETCAssociationList != null)
                        {
                            dataContext.ETCAssociations.RemoveRange(objETCAssociationList);
                        }
                        dataContext.SaveChanges();
                        /*Update ExerciseType information into database*/
                        if (objCreateChallengeVM.PostedExerciseTypes != null)
                        {
                            var ObjETCAssociationList = CommonReportingUtility.GetPostedExerciseTypeBasedChallenge(objCreateChallengeVM.PostedExerciseTypes, challengeId);
                            dataContext.ETCAssociations.AddRange(ObjETCAssociationList);
                        }

                        //  Remove and Added the Training zone into database
                        var objtblTrainingZoneCAssociationList = dataContext.TrainingZoneCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objtblTrainingZoneCAssociationList != null)
                        {
                            dataContext.TrainingZoneCAssociations.RemoveRange(objtblTrainingZoneCAssociationList);
                        }
                        dataContext.SaveChanges();
                        if (objCreateChallengeVM.PostedTargetZones != null)
                        {
                            var trainingZoneCAssociationList = CommonReportingUtility.GetPostedTargetZonesBasedChallenge(objCreateChallengeVM.PostedTargetZones, challengeId);
                            dataContext.TrainingZoneCAssociations.AddRange(trainingZoneCAssociationList);

                        }
                        // Remove exixting trainer zone for challenge
                        List<tblCEquipmentAssociation> objtblCEquipmentAssociationList = dataContext.ChallengeEquipmentAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                        if (objtblCEquipmentAssociationList != null)
                        {
                            dataContext.ChallengeEquipmentAssociations.RemoveRange(objtblCEquipmentAssociationList);
                        }
                        dataContext.SaveChanges();
                        // Added new Equipment for challenge                      
                        if (objCreateChallengeVM.PostedEquipments != null)
                        {
                            var eqipmentCAssociationList = CommonReportingUtility.GetPostedEquipmentsBasedChallenge(objCreateChallengeVM.PostedEquipments, challengeId);
                            dataContext.ChallengeEquipmentAssociations.AddRange(eqipmentCAssociationList);
                        }
                        if (!(trainerCredentialId > 0) && objChalange != null && objChalange.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType && objChalange.ChallengeId > 0)
                        {
                            // Remove exixting No trainer for challenge
                            var objtblNoTrainerChallengeTeamsList = dataContext.NoTrainerChallengeTeams.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            if (objtblNoTrainerChallengeTeamsList != null)
                            {
                                dataContext.NoTrainerChallengeTeams.RemoveRange(objtblNoTrainerChallengeTeamsList);
                            }
                            dataContext.SaveChanges();
                            // Add all team except primary team
                            if (objCreateChallengeVM.PostedTeams != null)
                            {
                                var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objCreateChallengeVM.PostedTeams, challengeId);
                                dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                            }
                        }
                        var challengeTrendingAssociationsList = dataContext.ChallengeTrendingAssociations.Where(ce => ce.ChallengeId == challengeId && !ce.IsProgram).ToList();
                        if (challengeTrendingAssociationsList != null)
                        {
                            dataContext.ChallengeTrendingAssociations.RemoveRange(challengeTrendingAssociationsList);
                        }
                        dataContext.SaveChanges();
                        // Add the challenge trending associated trening categorys                       
                        if (objCreateChallengeVM.PostedTrendingCategory != null || objCreateChallengeVM.PostedSecondaryTrendingCategory != null)
                        {
                            var selectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedTrendingCategory, challengeId);
                            var secondaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedSecondaryTrendingCategory, challengeId);
                            var allSelectedTrending = selectedTrendingCategory != null ? selectedTrendingCategory.Union(secondaryselectedTrendingCategory)
                                                                                     : secondaryselectedTrendingCategory;
                            dataContext.ChallengeTrendingAssociations.AddRange(allSelectedTrending);
                        }
                        var challengeCategoryAssociationsList = dataContext.ChallengeCategoryAssociations.Where(ce => ce.ChallengeId == challengeId && !ce.IsProgram).ToList();
                        if (challengeCategoryAssociationsList != null)
                        {
                            dataContext.ChallengeCategoryAssociations.RemoveRange(challengeCategoryAssociationsList);
                        }
                        dataContext.SaveChanges();
                        // Add the challenge trending associated trening categorys                           
                        if (objCreateChallengeVM.PostedChallengeCategory != null)
                        {
                            List<tblChallengeCategoryAssociation> selectedChallengeCategory = CommonReportingUtility.GetPostedChallengeCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedChallengeCategory, challengeId);
                            if (selectedChallengeCategory != null && selectedChallengeCategory.Count > 0)
                            {
                                dataContext.ChallengeCategoryAssociations.AddRange(selectedChallengeCategory);
                            }
                        }
                        dataContext.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                               .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);
                        var fullErrorMessage = string.Join("; ", errorMessages);
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("UpdateChallennges  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objCreateChallengeVM = null;
                    }
                }
            }
        }
        /// <summary>
        /// Get Freeform ChallangeExecise By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <summary>
        /// Get Freeform ChallangeExecise By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<Exercise> GetFreeformChallangeExeciseById(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetFreeformChallangeExeciseById for retrieving challenge by challengeid:" + Id);
                    /*Get exercise detail for the respective challenge*/
                    List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == Id).ToList();
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
                        objExercise.CEARocordId = objCEAssociationList[i].RocordId;
                        objExercise.Description = objCEAssociationList[i].Description;
                        objExercise.ExerciseName = (exercise != null && !objCEAssociationList[i].IsAlternateExeciseName) ? exercise.ExerciseName : string.Empty;
                        objExercise.VedioLink = (exercise != null && !objCEAssociationList[i].IsAlternateExeciseName) ? exercise.V720pUrl : string.Empty;
                        objExercise.ExerciseThumnail = (exercise != null && !objCEAssociationList[i].IsAlternateExeciseName) ? exercise.ThumnailUrl : string.Empty;

                        objExercise.IsAlternateExeciseName = objCEAssociationList[i].IsAlternateExeciseName;
                        objExercise.AlternateExeciseName = (objCEAssociationList[i].AlternateExeciseName == ConstantHelper.constFFChallangeDescription) ? string.Empty : objCEAssociationList[i].AlternateExeciseName;
                        objExercise.Index = exercise != null ? exercise.Index : string.Empty;
                        objExercise.ExeciseSetRecords = GetFFChallangeExeciseSetById(objCEAssociationList[i].RocordId);
                        objExercise.SelectedEquipment = (string.IsNullOrEmpty(objCEAssociationList[i].SelectedEquipment) || objCEAssociationList[i].SelectedEquipment == ConstantHelper.constFFChallangeDescription) ? "0" :
                            objCEAssociationList[i].SelectedEquipment;
                        objExercise.SelectedTraingZone = (string.IsNullOrEmpty(objCEAssociationList[i].SelectedTraingZone) || objCEAssociationList[i].SelectedTraingZone == ConstantHelper.constFFChallangeDescription) ? "0" :
                            objCEAssociationList[i].SelectedTraingZone;
                        objExercise.SelectedExeciseType = (string.IsNullOrEmpty(objCEAssociationList[i].SelectedExeciseType) || objCEAssociationList[i].SelectedExeciseType == ConstantHelper.constFFChallangeDescription) ? "0" :
                            objCEAssociationList[i].SelectedExeciseType;
                        objExercise.IsSetFirstExecise = objCEAssociationList[i].IsShownFirstExecise;
                        if (objCEAssociationList[i].IsShownFirstExecise)
                        {
                            execisevideoList.Insert(0, objExercise);
                        }
                        else
                        {
                            execisevideoList.Add(objExercise);
                        }

                    }
                    return execisevideoList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeformChallangeExeciseById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get execise assocates set details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<ExeciseSetVM> GetFFChallangeExeciseSetById(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetFFChallangeExeciseSetById for retrieving set details by execise  id:" + Id);
                    /*Get exercise detail for the respective challenge*/
                    List<ExeciseSetVM> execisesetList = null;
                    ExeciseSetVM objExerciseSet = null;
                    if (Id > 0)
                    {
                        execisesetList = new List<ExeciseSetVM>();
                        List<tblCESAssociation> objCESAssociationList = dataContext.CESAssociations.Where(ce => ce.RecordId == Id).ToList();
                        if (objCESAssociationList != null && objCESAssociationList.Count > 0)
                        {
                            for (int i = 0; i < objCESAssociationList.Count; i++)
                            {
                                objExerciseSet = new ExeciseSetVM();
                                objExerciseSet.SetResult = (objCESAssociationList[i].SetResult != null && objCESAssociationList[i].SetResult != ConstantHelper.constExeciseSetSeperator) ?
                                    objCESAssociationList[i].SetResult : string.Empty;
                                objExerciseSet.RestTime = (objCESAssociationList[i].RestTime != null && objCESAssociationList[i].RestTime != ConstantHelper.constExeciseSetSeperator) ?
                                    objCESAssociationList[i].RestTime : string.Empty;
                                objExerciseSet.Description = (objCESAssociationList[i].Description != null && objCESAssociationList[i].Description != ConstantHelper.constExeciseSetSeperator) ?
                                    objCESAssociationList[i].Description : string.Empty;
                                objExerciseSet.SetReps = objCESAssociationList[i].SetReps;
                                objExerciseSet.AutoCountDown = objCESAssociationList[i].AutoCountDown ? ConstantHelper.constAutoCountDownYes : ConstantHelper.constAutoCountDownNo;

                                execisesetList.Add(objExerciseSet);
                            }
                        }
                        else
                        {
                            objExerciseSet = new ExeciseSetVM();
                            objExerciseSet.SetResult = string.Empty;
                            objExerciseSet.RestTime = string.Empty;
                            objExerciseSet.Description = string.Empty;
                            objExerciseSet.SetReps = 0;
                            objExerciseSet.AutoCountDown = string.Empty;
                            execisesetList.Add(objExerciseSet);
                        }
                    }
                    return execisesetList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFFChallangeExeciseSetById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get all execise free form challeneges
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Exercise> GetAllFreeformChallangeExeciseById(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetFreeformChallangeExeciseById for retrieving challenge by challengeid:" + id);
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
                        objExercise.Description = objCEAssociationList[i].Description;
                        objExercise.ExerciseName = exercise.ExerciseName;
                        objExercise.VedioLink = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exercise.VideoLink;
                        objExercise.ExerciseThumnail = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exercise.VideoLink.Replace(" ", "%20");
                        objExercise.Index = exercise.Index;
                        objExercise.AlternateExeciseName = objCEAssociationList[i].AlternateExeciseName;
                        objExercise.IsAlternateExeciseName = objCEAssociationList[i].IsAlternateExeciseName;
                        objExercise.ExeciseSetRecords = FreeFormChallengeBL.GetFFChallangeExeciseSetById(objCEAssociationList[i].RocordId);
                        execisevideoList.Add(objExercise);
                    }
                    return execisevideoList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFreeformChallangeExeciseById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        ///// <summary>
        /// Get All Week Workouts Details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static ProgramWeekWorkoutDetails GetWeekWorkoutByProgramId(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get Program Week Workouts list detail by Program Id*/
                    traceLog.AppendLine("Start: GetWeekWorkoutByProgramId for retrieving challenge by Program Id-:" + Id);
                    string totalWeek = string.Empty;
                    string totakweekworkouts = string.Empty;
                    List<ProgramWeekWorkouts> objProgramWeekWorkouts = new List<ProgramWeekWorkouts>();
                    List<tblPWAssociation> objtblPWAssociationList = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == Id).ToList();
                    int totalweekworkouts = 0;
                    for (int i = 0; i < objtblPWAssociationList.Count; i++)
                    {
                        ProgramWeekWorkouts objworks = new ProgramWeekWorkouts();
                        if (objtblPWAssociationList[i].PWRocordId > 0)
                        {
                            long weekrecordId = objtblPWAssociationList[i].PWRocordId;
                            objworks.ProgramWeekId = weekrecordId;
                            objworks.AssignedTrainerId = objtblPWAssociationList[i].AssignedTrainerId;
                            objworks.AssignedTrainingzone = objtblPWAssociationList[i].AssignedTrainingzone;
                            objworks.AssignedDifficulyLevelId = objtblPWAssociationList[i].AssignedDifficulyLevelId;
                            var weekworkoutsList = (from pw in dataContext.PWWorkoutsAssociation
                                                    join ch in dataContext.Challenge
                                                    on pw.WorkoutChallengeId equals ch.ChallengeId
                                                    where pw.PWRocordId == weekrecordId
                                                    select new WeekWorkouts
                                                    {
                                                        ProgramWeekId = pw.PWRocordId,
                                                        ProgramWeekWorkoutId = pw.PWWorkoutId,
                                                        WorkoutName = ch.ChallengeName,
                                                        WorkoutChallengeId = pw.WorkoutChallengeId
                                                    }).ToList();
                            if (weekworkoutsList != null && weekworkoutsList.Count > 0)
                            {
                                weekworkoutsList.ForEach(p =>
                                {
                                    p.WorkoutUrl = CommonUtility.VirtualPath + ConstantHelper.constProgramViewChallenge + p.WorkoutChallengeId;
                                });
                                objworks.WeekWorkoutsRecords = weekworkoutsList;
                                totalweekworkouts += weekworkoutsList.Count();
                                objProgramWeekWorkouts.Add(objworks);
                            }
                        }
                    }
                    totalWeek = objProgramWeekWorkouts.Count() + ConstantHelper.constShowWeek;
                    totakweekworkouts = totalweekworkouts + ConstantHelper.constShaowWorkout;
                    ProgramWeekWorkoutDetails obProgramWeekWorkoutDetails = new ProgramWeekWorkoutDetails();
                    obProgramWeekWorkoutDetails.ProgramWeekWorkouts = objProgramWeekWorkouts;
                    obProgramWeekWorkoutDetails.Workouts = totakweekworkouts;
                    obProgramWeekWorkoutDetails.Durations = totalWeek;
                    return obProgramWeekWorkoutDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetWeekWorkoutByProgramId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


    }
}