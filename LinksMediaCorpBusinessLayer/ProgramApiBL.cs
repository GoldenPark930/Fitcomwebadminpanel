using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using AutoMapper;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
namespace LinksMediaCorpBusinessLayer
{
    public class ProgramApiBL
    {
        /// <summary>
        ///  Get TrainerLibrary Menu List with Name  and challenge Type Id
        /// </summary>
        /// <returns></returns>
        public static List<TrainerLibraryMenuVM> GetTrainerLibraryMenuList()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<TrainerLibraryMenuVM> objTrainerLibraryMenulist = null;
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibraryMenuList()---- " + DateTime.Now.ToLongDateString());
                    objTrainerLibraryMenulist = new List<TrainerLibraryMenuVM>{
                           new TrainerLibraryMenuVM(){ MenuItemId=1,MenuName=ConstantHelper.constFitnessTestName},
                           new TrainerLibraryMenuVM(){ MenuItemId=ConstantHelper.constWorkoutChallengeSubType,MenuName=ConstantHelper.constWorkoutChallenge},
                           new TrainerLibraryMenuVM(){ MenuItemId=ConstantHelper.constProgramChallengeSubType,MenuName=ConstantHelper.constProgramChallenge},
                           new TrainerLibraryMenuVM(){ MenuItemId=ConstantHelper.constWellnessChallengeSubType,MenuName=ConstantHelper.constWellnessName},
                       };
                    return objTrainerLibraryMenulist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTrainerLibraryMenuList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Get TrainerLibrary Sub CategoryList
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="challengeTypeId"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetTrainerLibrarySubCategoryList(int userId, string userType, int challengeTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibrarySubCategoryList---- " + DateTime.Now.ToLongDateString());
                    int trainerCredId = -1;
                    // ChallengeTabVM objChallengeTabVM = new ChallengeTabVM();
                    if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        trainerCredId = (from tr in dataContext.Trainer
                                         join crd in dataContext.Credentials
                                         on tr.TrainerId equals crd.UserId
                                         where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == userId
                                         select crd.Id).FirstOrDefault();
                    }
                    if (trainerCredId > 0 && challengeTypeId > 0)
                    {
                        List<int> categorylist = (from c in dataContext.Challenge
                                                  join ct in dataContext.ChallengeCategoryAssociations
                                                  on c.ChallengeId equals ct.ChallengeId
                                                  where c.IsActive && c.TrainerId == trainerCredId
                                                  && c.ChallengeSubTypeId == challengeTypeId
                                                  orderby c.CreatedDate descending
                                                  select ct.ChallengeCategoryId).Distinct().ToList();

                        var challengeCategoryList = (from cc in dataContext.ChallengeCategory
                                                     where cc.ChallengeSubTypeId == challengeTypeId && categorylist.Contains(cc.ChallengeCategoryId) && cc.Isactive
                                                     select new ChallengeCategory
                                                     {
                                                         ChallengeCategoryName = cc.ChallengeCategoryName,
                                                         ChallengeCategoryId = cc.ChallengeCategoryId,
                                                         ProgramTypeId = ConstantHelper.constProgramChallengeSubType
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
                    traceLog.AppendLine("End  GetTrainerLibrarySubCategoryList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Get Trainer Library All FitnnesTest challenge List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<MainChallengeVM> GetTrainerLibraryAllFitnnesTestList(int userId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibraryAllFitnnesTestList -userId-" + userId + ",userType-" + userType);
                    int trainerCredId = -1;
                    if (!string.IsNullOrEmpty(userType) && userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        trainerCredId = (from tr in dataContext.Trainer
                                         join crd in dataContext.Credentials
                                         on tr.TrainerId equals crd.UserId
                                         where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == userId
                                         select crd.Id).FirstOrDefault();
                    }
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
                                                        && c.IsActive && c.TrainerId == trainerCredId
                                                        //&& c.IsPremium == true
                                                        orderby c.CreatedDate descending
                                                        select new MainChallengeVM
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
                                                            Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId)
                                                                                  .Select(y => y.UserId).Distinct().Count(),
                                                            ResultUnit = ct.ResultUnit
                                                        }).ToList();

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

                    });
                    //Challenge feed sorted by acceptors
                    if (listMainVM != null)
                    {
                        listMainVM = listMainVM.OrderByDescending(chlng => chlng.Strenght).ToList();
                    }
                    return listMainVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTrainerLibraryAllFitnnesTestList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Get Trainer Library Filter Challenge List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<MainChallengeVM> GetTrainerLibraryFilterChallengeList(TrainerLibraryChallengeFilterParam model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFilterChallengeList---- " + DateTime.Now.ToLongDateString());
                    int trainerCredId = -1;
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        trainerCredId = (from tr in dataContext.Trainer
                                         join crd in dataContext.Credentials
                                         on tr.TrainerId equals crd.UserId
                                         where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == model.UserId
                                         select crd.Id).FirstOrDefault();
                    }
                    if (trainerCredId > 0)
                    {
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
                                              && (c.TrainerId == trainerCredId)
                                              && ct.ChallengeType != challengeType
                                              && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                          orderby c.CreatedDate descending
                                          select new MainChallengeVM
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
                                               && c.IsActive == true && c.IsPremium
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
                                               && (c.TrainerId == trainerCredId)
                                               && ct.ChallengeType != challengeType
                                               && ct.ChallengeType != ConstantHelper.ProgramChallengeType
                                          orderby c.CreatedDate descending
                                          select new MainChallengeVM
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
                                              Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                              ResultUnit = ct.ResultUnit
                                          }).ToList();
                        }
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
                        });
                        if (listMainVM != null)
                        {
                            listMainVM = listMainVM.OrderByDescending(item => item.Strenght).ToList();
                        }
                        return listMainVM;
                    }
                    return null;
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
        /// Get FreeForm TrainerLibrary Challenges By SubCategory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ChallengeTabVM GetFreeFormTrainerLibraryChallengesBySubCategory(TrainerLibraryWorkoutListByCategory model)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreeFormTrainerLibraryChallengesBySubCategory---- " + DateTime.Now.ToLongDateString());
                    int trainerCredId = -1;
                    ChallengeTabVM objChallengeTabVM = new ChallengeTabVM();
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int usercredId = objCred.Id;
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        trainerCredId = (from tr in dataContext.Trainer
                                         join crd in dataContext.Credentials
                                         on tr.TrainerId equals crd.UserId
                                         where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == model.UserId
                                         select crd.Id).FirstOrDefault();
                    }
                    if (trainerCredId > 0)
                    {
                        List<MainChallengeVM> listMainVM = (from c in dataContext.Challenge
                                                            join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                            where c.IsActive && c.TrainerId == trainerCredId
                                                            && ct.ChallengeSubTypeId == model.WorkoutCategoryID
                                                            //  && c.IsPremium == true
                                                            orderby c.CreatedDate descending
                                                            select new MainChallengeVM
                                                            {
                                                                ChallengeId = c.ChallengeId,
                                                                ChallengeName = c.ChallengeName,
                                                                DifficultyLevel = c.DifficultyLevel,
                                                                ChallengeType = ct.ChallengeType,
                                                                Description = c.Description,
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
                                                                IsActive = dataContext.UserActivePrograms.Where(uc => uc.ProgramId == c.ChallengeId && uc.IsCompleted == false
                                                                    && uc.UserCredId == usercredId).Select(y => y.ProgramId).Distinct().Count() > 0,
                                                                Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                ResultUnit = ct.ResultUnit,
                                                                IsWellness = (ct.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false,
                                                                ProgramImageUrl = c.ProgramImageUrl,
                                                                ChallengeCategoryList = dataContext.ChallengeCategoryAssociations.
                                                                Where(cc => cc.ChallengeId == c.ChallengeId && cc.IsProgram == (ConstantHelper.constProgramChallengeSubType == c.ChallengeSubTypeId)).Select(ch => ch.ChallengeCategoryId).ToList()
                                                            }).ToList();


                        if (listMainVM != null && listMainVM.Count > 0)
                        {
                            listMainVM = listMainVM.Where(ch => ch.ChallengeCategoryList != null && ch.ChallengeCategoryList.Contains(model.WorkoutSubCategoryID)).ToList();
                            listMainVM.ForEach(r =>
                            {
                                string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + r.ProgramImageUrl;
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
                                r.ChallengeType = r.ChallengeType.Split(' ')[0];
                                r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty :
                                    File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + r.ProgramImageUrl)) ?
                                    CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;
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

                            });

                            int totalcount = listMainVM.Count();
                            listMainVM = (from l in listMainVM
                                          select l).Skip(model.StartIndex).Take(model.EndIndex - model.StartIndex).ToList();

                            if ((totalcount) > model.StartIndex)
                            {
                                objChallengeTabVM.IsMoreAvailable = true;
                            }
                            //Challenge feed sorted by acceptors
                            if (listMainVM != null)
                            {
                                listMainVM = listMainVM.OrderByDescending(chlng => chlng.Strenght).ToList();
                            }
                            objChallengeTabVM.ChallengeList = new List<MainChallengeVM>();
                            objChallengeTabVM.ChallengeList = listMainVM;
                        }
                    }
                    return objChallengeTabVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetFreeFormTrainerLibraryChallengesBySubCategory : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Trainer Library FitcomTest BodyPartList based on trainer credtails ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>       
        public static FittnessTestChallenge GetTrainerLibraryFitcomTestBodyPartList(int userId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            FittnessTestChallenge objFittnessTestChallenge = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibraryFitcomTestBodyPartList---- " + DateTime.Now.ToLongDateString());
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
                                                        join crd in dataContext.Credentials
                                                        on c.TrainerId equals crd.Id
                                                        where !exceptListCOD.Contains(c.ChallengeId)
                                                        && !exceptListSponsor.Contains(c.ChallengeId)
                                                        && c.IsActive && crd.UserType == userType && crd.UserId == userId
                                                        orderby c.CreatedDate descending
                                                        select new MainChallengeVM
                                                        {
                                                            ChallengeId = c.ChallengeId,
                                                            ChallengeName = c.ChallengeName,
                                                            DifficultyLevel = c.DifficultyLevel,
                                                            ChallengeType = ct.ChallengeType,
                                                            IsSubscription = c.IsSubscription,
                                                            // Equipment = c.Equipment,
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
                                                            ResultUnit = ct.ResultUnit,
                                                            Description = c.Description
                                                        }).ToList();

                    listMainVM = listMainVM.Where(item => item.Strenght > 0).OrderByDescending(item => item.Strenght).Take(20).ToList();
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
                    });

                    objFittnessTestChallenge = new FittnessTestChallenge();


                    Mapper.CreateMap<tblBodyPart, BodyPart>();
                    List<tblBodyPart> lstBodyParts = (from trzone in dataContext.TrainingZoneCAssociations
                                                      join bp in dataContext.BodyPart
                                                      on trzone.PartId equals bp.PartId
                                                      //  where listTrainerchallengeId.Contains(trzone.ChallengeId)
                                                      select bp).Distinct().ToList();
                    List<BodyPart> lstlstBodyPartVM = Mapper.Map<List<tblBodyPart>, List<BodyPart>>(lstBodyParts);
                    objFittnessTestChallenge.BodyPart = lstlstBodyPartVM;
                    if (listMainVM != null)
                    {
                        listMainVM = listMainVM.OrderByDescending(chlng => chlng.Strenght).ToList();
                        objFittnessTestChallenge.FittnessTestChallenges = listMainVM;
                    }
                    return objFittnessTestChallenge;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTrainerLibraryFitcomTestBodyPartList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}