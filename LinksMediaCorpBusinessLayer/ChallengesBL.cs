namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using System.Globalization;
    using System.Web.Mvc;
    using LinksMediaCorpUtility;
    using AutoMapper;
    using LinksMediaCorpUtility.Resources;
    public class ChallengesBL
    {
        /// <summary>
        /// Function to get challenge types from database
        /// </summary>        
        /// <returns>List<ChallengeTypes></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<ChallengeTypes> GetChallengeType()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChellagneType for retrieving challenges from database ");
                    Mapper.CreateMap<tblChallengeType, ChallengeTypes>();
                    List<tblChallengeType> lstChalangeType = dataContext.ChallengeType.Where(ct => ct.ChallengeSubTypeId != ConstantHelper.constProgramChallengeSubType).ToList();
                    List<ChallengeTypes> lstChalangeTypeVM =
                        Mapper.Map<List<tblChallengeType>, List<ChallengeTypes>>(lstChalangeType);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChellagneType  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Workout Type
        /// </summary>
        /// <returns></returns>
        public static List<ChallengeTypes> GetWorkoutType()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetWorkoutType for retrieving challenges from database ");
                    Mapper.CreateMap<tblChallengeType, ChallengeTypes>();
                    List<tblChallengeType> lstWorkoutChalangeType = dataContext.ChallengeType.Where(ct => ct.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType || ct.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType).ToList();
                    List<ChallengeTypes> lstChalangeTypeVM =
                        Mapper.Map<List<tblChallengeType>, List<ChallengeTypes>>(lstWorkoutChalangeType);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetWorkoutType  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get challenge sub type from the database  
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <returns>List<ChallengeTypes></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<ChallengeTypes> GetChallengeSubType(int challengeSubTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChellagneSubType for retrieving challenges from database ");
                    if (challengeSubTypeId > 0)
                    {
                        Mapper.CreateMap<tblChallengeType, ChallengeTypes>();
                        string strChallengeType = dataContext.ChallengeType
                                                 .FirstOrDefault(c => c.ChallengeSubTypeId == challengeSubTypeId)
                                                 .ChallengeType;
                        List<tblChallengeType> lstChalangeType = dataContext.ChallengeType
                                                                 .Where(ct => ct.ChallengeType == strChallengeType && ct.IsActive == true).ToList();
                        List<ChallengeTypes> lstChalangeTypeVM =
                            Mapper.Map<List<tblChallengeType>, List<ChallengeTypes>>(lstChalangeType);
                        return lstChalangeTypeVM;
                    }
                    return null;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChellagneSubType  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get FitnessTest OR Workout SubTypeId
        /// </summary>
        /// <param name="subTypeId"></param>
        /// <returns></returns>
        public static List<int> GetFitnessTestORWorkoutSubTypeId(int challengeType)
        {
            List<int> ChallengeSubTypeList = new List<int>();
            switch (challengeType)
            {
                case ConstantHelper.FreeformChallangeId:
                    ChallengeSubTypeList.Add(ConstantHelper.constWellnessChallengeSubType);
                    ChallengeSubTypeList.Add(ConstantHelper.constWorkoutChallengeSubType);
                    break;
                case ConstantHelper.constFittnessCommonSubTypeId:
                    ChallengeSubTypeList.Add(ConstantHelper.constPowerChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constPowerChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constEnduranceChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constEnduranceChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constStrengthChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constStrengthChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType3);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType4);
                    break;
                case ConstantHelper.constantAllfittnessWorkout:
                    ChallengeSubTypeList.Add(ConstantHelper.constWellnessChallengeSubType);
                    ChallengeSubTypeList.Add(ConstantHelper.constWorkoutChallengeSubType);
                    ChallengeSubTypeList.Add(ConstantHelper.constPowerChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constPowerChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constEnduranceChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constEnduranceChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constStrengthChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constStrengthChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType1);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType2);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType3);
                    ChallengeSubTypeList.Add(ConstantHelper.constCardioChallengeType4);
                    break;
            }
            return ChallengeSubTypeList;
        }
        /// <summary>
        /// Function to get trainer challenges from the database  
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns>List<ViewChallenes></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<ViewChallenes> GetTrainerChallenges(int trainerId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerChellagnes for retrieving challenges from database ");
                    List<ViewChallenes> challenges = null;
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();
                    List<int> ChallengeSubTypeList = GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where ChallengeSubTypeList.Contains(CT.ChallengeSubTypeId)
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     ModifiedDate = C.ModifiedDate,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeSubTypeId = CT.ChallengeSubTypeId,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                 }).ToList();
                    /*challenges for all trainer else for related trainer*/
                    if (trainerId == -1)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId > 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else if (trainerId == -2)
                    {
                        challenges = query.OrderBy(ch => ch.ChallengeName).ToList<ViewChallenes>();
                    }
                    else if (trainerId == 0)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId == 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else
                    {
                        challenges = query.Where(q => q.TrainerId == trainerId).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    // string challengeType = ConstantHelper.FreeFormChallengeType;
                    challenges.ForEach(c =>
                    {
                        c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                        if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                        {
                            c.Equipment = string.Join(", ", c.TempEquipments);
                        }
                        c.TempEquipments = null;
                        if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                        {
                            c.TargetZone = string.Join(", ", c.TempTargetZone);
                        }
                        c.TempTargetZone = null;
                        c.IsWorkout = c.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType;
                    });
                    foreach (var item in challenges)
                    {
                        tblChallengeofTheDayQueue challengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        tblSponsorChallengeQueue trainerChallenge = dataContext.TrainerChallenge.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        bool flagAdd = false;
                        bool flag = false;
                        if (challengeofTheDayQueue != null || trainerChallenge != null)
                        {
                            if (challengeofTheDayQueue != null)
                            {
                                flag = (challengeofTheDayQueue.StartDate <= DateTime.Now.Date) && (challengeofTheDayQueue.EndDate >= DateTime.Now.Date);
                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                                if (trainerChallenge != null)
                                {
                                    flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);
                                    if (flagAdd)
                                    {
                                        if (!flag)
                                        {
                                            flagAdd = true;
                                        }
                                    }
                                }
                            }
                            else if (trainerChallenge != null)
                            {
                                flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);

                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                            }
                        }
                        else
                        {
                            flagAdd = true;
                        }
                        if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                        {
                            flagAdd = false;
                        }
                        if (flagAdd)
                        {
                            ViewChallenes filterdChallenge = new ViewChallenes();
                            filterdChallenge = item;
                            filterdChallenges.Add(filterdChallenge);
                        }
                    }
                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerChellagnes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// GetTrainerAdminChallenges
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetTrainerAdminChallenges(int trainerId, string userType, int challengeTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerChellagnes for retrieving challenges from database ");
                    List<ViewChallenes> challenges = null;
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();
                    List<int> ChallengeSubTypeList = GetFitnessTestORWorkoutSubTypeId(challengeTypeId);
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where C.TrainerId == trainerId && ChallengeSubTypeList.Contains(C.ChallengeSubTypeId)
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     ModifiedDate = C.ModifiedDate,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeSubTypeId = CT.ChallengeSubTypeId,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                 }).ToList();
                    /*challenges for all trainer else for related trainer*/
                    if (trainerId == -1)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId > 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else if (trainerId == -2)
                    {
                        challenges = query.OrderBy(ch => ch.ChallengeName).ToList<ViewChallenes>();
                    }
                    else if (trainerId == 0)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId == 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else
                    {
                        challenges = query.Where(q => q.TrainerId == trainerId).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    // string challengeType = ConstantHelper.FreeFormChallengeType;
                    challenges.ForEach(c =>
                    {
                        c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                        if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                        {
                            c.Equipment = string.Join(", ", c.TempEquipments);
                        }
                        c.TempEquipments = null;
                        if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                        {
                            c.TargetZone = string.Join(", ", c.TempTargetZone);
                        }
                        c.TempTargetZone = null;
                        c.IsWorkout = c.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType;
                    });
                    foreach (var item in challenges)
                    {
                        tblChallengeofTheDayQueue challengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        tblSponsorChallengeQueue trainerChallenge = dataContext.TrainerChallenge.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        bool flagAdd = false;
                        bool flag = false;
                        if (challengeofTheDayQueue != null || trainerChallenge != null)
                        {
                            if (challengeofTheDayQueue != null)
                            {
                                flag = (challengeofTheDayQueue.StartDate <= DateTime.Now.Date) && (challengeofTheDayQueue.EndDate >= DateTime.Now.Date);
                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                                if (trainerChallenge != null)
                                {
                                    flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);
                                    if (flagAdd)
                                    {
                                        if (!flag)
                                        {
                                            flagAdd = true;
                                        }
                                    }
                                }
                            }
                            else if (trainerChallenge != null)
                            {
                                flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);

                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                            }
                        }
                        else
                        {
                            flagAdd = true;
                        }
                        if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                        {
                            flagAdd = false;
                        }
                        if (flagAdd)
                        {
                            ViewChallenes filterdChallenge = new ViewChallenes();
                            filterdChallenge = item;
                            filterdChallenges.Add(filterdChallenge);
                        }
                    }
                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerChellagnes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Trainer Workout Challenges
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetTrainerWorkoutChallenges(int trainerId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerWorkoutChallenges for retrieving challenges from database for-trainerId" + trainerId);
                    List<ViewChallenes> challenges = null;
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();

                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType || CT.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     ModifiedDate = C.ModifiedDate,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeSubTypeId = CT.ChallengeSubTypeId,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                 }).ToList();
                    /*challenges for all trainer else for related trainer*/
                    if (trainerId == -1)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId > 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else if (trainerId == -2)
                    {
                        challenges = query.OrderBy(ch => ch.ChallengeName).ToList<ViewChallenes>();
                    }
                    else if (trainerId == 0)
                    {
                        challenges = query.Where(chllenge => chllenge.TrainerId == 0).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else
                    {
                        challenges = query.Where(q => q.TrainerId == trainerId).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    // string challengeType = ConstantHelper.FreeFormChallengeType;
                    challenges.ForEach(c =>
                    {
                        if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                        {
                            c.Equipment = string.Join(", ", c.TempEquipments);
                        }
                        c.TempEquipments = null;
                        if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                        {
                            c.TargetZone = string.Join(", ", c.TempTargetZone);
                        }
                        c.TempTargetZone = null;
                        c.IsWorkout = c.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType;
                    });
                    foreach (var item in challenges)
                    {
                        tblChallengeofTheDayQueue challengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        tblSponsorChallengeQueue trainerChallenge = dataContext.TrainerChallenge.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                        bool flagAdd = false;
                        bool flag = false;
                        if (challengeofTheDayQueue != null || trainerChallenge != null)
                        {
                            if (challengeofTheDayQueue != null)
                            {
                                flag = (challengeofTheDayQueue.StartDate <= DateTime.Now.Date) && (challengeofTheDayQueue.EndDate >= DateTime.Now.Date);
                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                                if (trainerChallenge != null)
                                {
                                    flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);
                                    if (flagAdd)
                                    {
                                        if (!flag)
                                        {
                                            flagAdd = true;
                                        }
                                    }
                                }
                            }
                            else if (trainerChallenge != null)
                            {
                                flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);

                                if (!flag)
                                {
                                    flagAdd = true;
                                }
                            }
                        }
                        else
                        {
                            flagAdd = true;
                        }
                        if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                        {
                            flagAdd = false;
                        }
                        if (flagAdd)
                        {
                            ViewChallenes filterdChallenge = new ViewChallenes();
                            filterdChallenge = item;
                            filterdChallenges.Add(filterdChallenge);
                        }
                    }
                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerWorkoutChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Seach Challenges based on trainerID, userType and searched text
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetSeachChallenges(int trainerId, string userType, string search)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {

                    traceLog.AppendLine("Start: GetTrainerChellagnes for retrieving challenges from database ");
                    List<ViewChallenes> challenges = null;
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();
                    List<int> challengeSubTypeList = GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where challengeSubTypeList.Contains(CT.ChallengeSubTypeId)
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     ModifiedDate = C.ModifiedDate,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeSubTypeId = CT.ChallengeSubTypeId,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                 }).ToList();
                    if (query != null)
                    {

                        if (!string.IsNullOrEmpty(search))
                        {
                            search = search.ToUpper(CultureInfo.InvariantCulture);
                        }
                        if (trainerId == -1)
                        {
                            challenges = query.Where(ch => ch.TrainerId > 0 && (ch.ChallengeName != null && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.Type != null && ch.Type.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                        }
                        else if (trainerId == 0)
                        {
                            challenges = query.Where(ch => ch.TrainerId == 0 && (ch.ChallengeName != null && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.Type != null && ch.Type.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                        }
                        else
                        {
                            challenges = query.Where(q => q.TrainerId == trainerId).ToList();
                            if (challenges != null && challenges.Count > 0)
                            {
                                challenges = challenges.Where(q => ((q.ChallengeName != null && q.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                    || (q.DifficultyLevel != null && q.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                    || (q.Type != null && q.Type.IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1))).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                            }
                        }
                        challenges.ForEach(c =>
                        {
                            c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                            if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                            {
                                c.Equipment = string.Join(", ", c.TempEquipments);
                            }
                            c.TempEquipments = null;
                            if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                            {
                                c.TargetZone = string.Join(", ", c.TempTargetZone);
                            }
                            c.TempTargetZone = null;

                            c.IsWorkout = c.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType;
                        });
                        foreach (var item in challenges)
                        {
                            tblChallengeofTheDayQueue challengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                            tblSponsorChallengeQueue trainerChallenge = dataContext.TrainerChallenge.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                            bool flagAdd = false;
                            bool flag = false;
                            if (challengeofTheDayQueue != null || trainerChallenge != null)
                            {
                                if (challengeofTheDayQueue != null)
                                {
                                    flag = (challengeofTheDayQueue.StartDate <= DateTime.Now.Date) && (challengeofTheDayQueue.EndDate >= DateTime.Now.Date);
                                    if (!flag)
                                    {
                                        flagAdd = true;
                                    }
                                    if (trainerChallenge != null)
                                    {
                                        flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);
                                        if (flagAdd)
                                        {
                                            if (!flag)
                                            {
                                                flagAdd = true;
                                            }
                                        }
                                    }
                                }
                                else if (trainerChallenge != null)
                                {
                                    flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);

                                    if (!flag)
                                    {
                                        flagAdd = true;
                                    }
                                }
                            }
                            else
                            {
                                flagAdd = true;
                            }
                            if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                            {
                                flagAdd = false;
                            }
                            if (flagAdd)
                            {
                                ViewChallenes filterdChallenge = new ViewChallenes();
                                filterdChallenge = item;
                                filterdChallenges.Add(filterdChallenge);
                            }
                        }
                    }
                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerChellagnes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Seach Workout Challenges
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetSeachWorkoutChallenges(int trainerId, string userType, string search)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {

                    traceLog.AppendLine("Start: GetSeachWorkoutChallenges for retrieving challenges from database ");
                    List<ViewChallenes> challenges = null;
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();

                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType || CT.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     ModifiedDate = C.ModifiedDate,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ChallengeSubTypeId = CT.ChallengeSubTypeId,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                 }).ToList();
                    if (query != null)
                    {

                        if (!string.IsNullOrEmpty(search))
                        {
                            search = search.ToUpper(CultureInfo.InvariantCulture);
                        }
                        if (trainerId == -1)
                        {
                            challenges = query.Where(ch => ch.TrainerId > 0 && (ch.ChallengeName != null && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.Type != null && ch.Type.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                        }
                        else if (trainerId == 0)
                        {
                            challenges = query.Where(ch => ch.TrainerId == 0 && (ch.ChallengeName != null && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                || (ch.Type != null && ch.Type.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                        }
                        else
                        {
                            challenges = query.Where(q => q.TrainerId == trainerId).ToList();
                            if (challenges != null && challenges.Count > 0)
                            {
                                challenges = challenges.Where(q => ((q.ChallengeName != null && q.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                    || (q.DifficultyLevel != null && q.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                    || (q.Type != null && q.Type.IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1))).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                            }
                        }
                        challenges.ForEach(c =>
                        {
                            c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                            if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                            {
                                c.Equipment = string.Join(", ", c.TempEquipments);
                            }
                            c.TempEquipments = null;
                            if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                            {
                                c.TargetZone = string.Join(", ", c.TempTargetZone);
                            }
                            c.TempTargetZone = null;

                            c.IsWorkout = c.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType;
                        });
                        foreach (var item in challenges)
                        {
                            tblChallengeofTheDayQueue challengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                            tblSponsorChallengeQueue trainerChallenge = dataContext.TrainerChallenge.FirstOrDefault(c => c.ChallengeId == item.ChallengeId);
                            bool flagAdd = false;
                            bool flag = false;
                            if (challengeofTheDayQueue != null || trainerChallenge != null)
                            {
                                if (challengeofTheDayQueue != null)
                                {
                                    flag = (challengeofTheDayQueue.StartDate <= DateTime.Now.Date) && (challengeofTheDayQueue.EndDate >= DateTime.Now.Date);
                                    if (!flag)
                                    {
                                        flagAdd = true;
                                    }
                                    if (trainerChallenge != null)
                                    {
                                        flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);
                                        if (flagAdd)
                                        {
                                            if (!flag)
                                            {
                                                flagAdd = true;
                                            }
                                        }
                                    }
                                }
                                else if (trainerChallenge != null)
                                {
                                    flag = (trainerChallenge.StartDate <= DateTime.Now.Date) && (trainerChallenge.EndDate >= DateTime.Now.Date);

                                    if (!flag)
                                    {
                                        flagAdd = true;
                                    }
                                }
                            }
                            else
                            {
                                flagAdd = true;
                            }
                            if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                            {
                                flagAdd = false;
                            }
                            if (flagAdd)
                            {
                                ViewChallenes filterdChallenge = new ViewChallenes();
                                filterdChallenge = item;
                                filterdChallenges.Add(filterdChallenge);
                            }
                        }
                    }
                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetSeachWorkoutChallenges  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Challenge by Id for view only
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetViewChallenge(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetViewChallenge for retrieving challenges from database ");
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();
                    string ProgramType = ConstantHelper.ProgramChallengeType;
                    List<ViewChallenes> challenges = (from C in dataContext.Challenge
                                                      join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                                      where CT.ChallengeType != ProgramType && C.ChallengeId == challengeId
                                                      orderby C.ModifiedDate descending
                                                      select new ViewChallenes
                                                      {
                                                          TrainerId = C.TrainerId,
                                                          ChallengeName = C.ChallengeName,
                                                          Type = CT.ChallengeType,
                                                          ResultUnit = CT.ResultUnit,
                                                          ChallengeId = C.ChallengeId,
                                                          DifficultyLevel = C.DifficultyLevel,
                                                          IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                                          TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                            join bp in dataContext.Equipments
                                                                            on trzone.EquipmentId equals bp.EquipmentId
                                                                            where trzone.ChallengeId == C.ChallengeId
                                                                            select bp.Equipment).Distinct().ToList<string>(),
                                                          TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                            join bp in dataContext.BodyPart
                                                                            on trzone.PartId equals bp.PartId
                                                                            where trzone.ChallengeId == C.ChallengeId
                                                                            select bp.PartName).Distinct().ToList<string>(),
                                                          ChallengeDesc = CT.ChallengeSubType,
                                                          VariableValue = C.VariableValue,
                                                          IsDrafted = C.IsDraft,
                                                          Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                                      }).ToList();

                    challenges.ForEach(c =>
                    {
                        c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                        if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                        {
                            c.Equipment = string.Join(", ", c.TempEquipments);
                        }
                        c.TempEquipments = null;
                        if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                        {
                            c.TargetZone = string.Join(", ", c.TempTargetZone);
                        }
                        c.TempTargetZone = null;
                    });
                    foreach (var item in challenges)
                    {
                        ViewChallenes filterdChallenge = new ViewChallenes();
                        filterdChallenge = item;
                        filterdChallenges.Add(filterdChallenge);
                    }

                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetViewChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to insert challenge in to database  
        /// </summary>
        /// <param name="objCreateChallengeVM"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void SubmitChallenge(CreateChallengeVM objCreateChallengeVM, int credentialId, int trainerId, string draft)
        {
            StringBuilder traceLog = new StringBuilder();
            if (objCreateChallengeVM.ChallengeType == ConstantHelper.FreeformChallangeId)
            {
                FreeFormChallengeBL.SubmitAdminFreeFormChallenge(objCreateChallengeVM, credentialId, trainerId, draft);
            }
            else
            {
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            traceLog.AppendLine("Start: SubmitChallenge for creating challenge");
                            if (objCreateChallengeVM.ResultTime == null)
                            {
                                objCreateChallengeVM.ResultTime = ConstantHelper.constNotValidate;
                            }
                            if (!objCreateChallengeVM.ResultTime.Equals(ConstantHelper.constNotValidate))
                            {
                                objCreateChallengeVM.TrainerMainResult = objCreateChallengeVM.ResultTime;
                            }
                            else if (objCreateChallengeVM.ResultFrection != null)
                            {
                                if (!objCreateChallengeVM.ResultFrection.Equals(ConstantHelper.constNotValidate))
                                {
                                    if (!objCreateChallengeVM.ResultRepsRound.Equals(0) && !objCreateChallengeVM.ResultRepsRound.Equals(null))
                                    {
                                        objCreateChallengeVM.TrainerMainResult = Convert.ToString(objCreateChallengeVM.ResultRepsRound);
                                    }
                                    objCreateChallengeVM.ResultFrection = objCreateChallengeVM.ResultFrection;
                                }
                            }
                            else if (!objCreateChallengeVM.ResultRepsRound.Equals(0) && !objCreateChallengeVM.ResultRepsRound.Equals(null))
                            {
                                objCreateChallengeVM.TrainerMainResult = Convert.ToString(objCreateChallengeVM.ResultRepsRound);
                            }
                            else if (!objCreateChallengeVM.ResultWeightorDestance.Equals(0.0) && !objCreateChallengeVM.ResultWeightorDestance.Equals(null))
                            {
                                objCreateChallengeVM.TrainerMainResult = Convert.ToString(objCreateChallengeVM.ResultWeightorDestance);
                            }
                            if (!string.IsNullOrEmpty(objCreateChallengeVM.VariableUnit))
                            {
                                if (objCreateChallengeVM.VariableUnit.Equals(Message.VariableUnitTypeSecond))
                                {
                                    objCreateChallengeVM.VariableValue = ConstantHelper.constTimeHHMMFormat + (objCreateChallengeVM.VariableValue.Length == 1 ?
                                        ConstantHelper.constSingleZero + objCreateChallengeVM.VariableValue
                                        : objCreateChallengeVM.VariableValue) + ConstantHelper.constDotDoubleZero;
                                    objCreateChallengeVM.GlobalResultFilterValue = ConstantHelper.constTimeHHMMFormat + (objCreateChallengeVM.GlobalResultFilterValue.Length == 1 ?
                                        ConstantHelper.constSingleZero + objCreateChallengeVM.GlobalResultFilterValue
                                        : objCreateChallengeVM.GlobalResultFilterValue) + ConstantHelper.constDotDoubleZero;
                                }
                            }
                            //Add ".00" at the end of Variablevalue of type HH:MM:SS
                            //We have to work on VariableValue at create and update challenge by trainer
                            if (objCreateChallengeVM.VariableValue != null)
                            {
                                if (objCreateChallengeVM.VariableValue.Contains(ConstantHelper.constColon))
                                {
                                    objCreateChallengeVM.VariableValue = objCreateChallengeVM.VariableValue;
                                }
                            }
                            if (objCreateChallengeVM.GlobalResultFilterValue != null)
                            {
                                if (objCreateChallengeVM.GlobalResultFilterValue.Contains(ConstantHelper.constColon))
                                {
                                    objCreateChallengeVM.GlobalResultFilterValue = objCreateChallengeVM.GlobalResultFilterValue;
                                }
                            }
                            Mapper.CreateMap<CreateChallengeVM, tblChallenge>();
                            tblChallenge objChalange = Mapper.Map<CreateChallengeVM, tblChallenge>(objCreateChallengeVM);
                            objChalange.CreatedBy = credentialId;
                            if (!trainerId.Equals(0))
                            {
                                objChalange.TrainerId = trainerId;
                            }
                            // objChalange.TrendingCategoryId = objCreateChallengeVM.TrendingCategoryId??0;
                            objChalange.ModifiedBy = objChalange.CreatedBy;
                            objChalange.CreatedDate = DateTime.Now;
                            objChalange.ModifiedDate = objChalange.CreatedDate;
                            if (draft.Equals(Message.SavetoDraft))
                            {
                                objChalange.IsDraft = true;
                            }
                            else
                            {
                                objChalange.IsDraft = false;
                            }
                            tblChallenge checkChallenge = dataContext.Challenge.Find(objCreateChallengeVM.ChallengeId);
                            var challengeId = 0;
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
                            if (objCreateChallengeVM.TrainerMainResult != null && objCreateChallengeVM.TrainerMainResult != ConstantHelper.constSingleOne)
                            {
                                tblUserChallenges userchallenge = new tblUserChallenges();
                                tblUserChallenges checkUserChallenge = dataContext.UserChallenge.Where(y => challengeId > 0 && y.ChallengeId == challengeId).FirstOrDefault();
                                if (checkUserChallenge == null)
                                {
                                    userchallenge.ChallengeId = challengeId;
                                    userchallenge.UserId = trainerId;
                                    userchallenge.Result = objCreateChallengeVM.TrainerMainResult;
                                    userchallenge.IsGlobal = true;
                                    userchallenge.AcceptedDate = objChalange.CreatedDate;
                                    userchallenge.CreatedDate = objChalange.CreatedDate;
                                    userchallenge.ModifiedDate = objChalange.CreatedDate;
                                    if (!string.IsNullOrEmpty(objCreateChallengeVM.ResultFrection))
                                    {
                                        userchallenge.Fraction = objCreateChallengeVM.ResultFrection;
                                    }
                                    userchallenge.CreatedBy = credentialId;
                                    userchallenge.ModifiedBy = credentialId;
                                    dataContext.UserChallenge.Add(userchallenge);
                                }
                                else
                                {
                                    checkUserChallenge.UserId = trainerId;
                                    checkUserChallenge.Result = objCreateChallengeVM.TrainerMainResult;
                                    checkUserChallenge.IsGlobal = true;
                                    checkUserChallenge.AcceptedDate = objChalange.CreatedDate;
                                    checkUserChallenge.CreatedDate = objChalange.CreatedDate;
                                    if (!string.IsNullOrEmpty(objCreateChallengeVM.ResultFrection))
                                    {
                                        checkUserChallenge.Fraction = objCreateChallengeVM.ResultFrection;
                                    }
                                    checkUserChallenge.ModifiedDate = objChalange.CreatedDate;
                                    checkUserChallenge.CreatedBy = credentialId;
                                    checkUserChallenge.ModifiedBy = credentialId;
                                }

                                dataContext.SaveChanges();
                            }

                            if (objCreateChallengeVM.HypeVideoLink != null)
                            {
                                tblHypeVideo hypeVideo = new tblHypeVideo();
                                tblHypeVideo checkHypeVideos = dataContext.HypeVideos.Where(y => y.ChallengeId == objCreateChallengeVM.ChallengeId).FirstOrDefault();
                                if (checkHypeVideos == null)
                                {
                                    hypeVideo.HypeVideo = objCreateChallengeVM.HypeVideoLink;
                                    hypeVideo.UserId = trainerId;
                                    hypeVideo.ChallengeId = challengeId;
                                    dataContext.HypeVideos.Add(hypeVideo);
                                }
                                else
                                {
                                    checkHypeVideos.HypeVideo = objCreateChallengeVM.HypeVideoLink;
                                    checkHypeVideos.UserId = trainerId;
                                }

                                dataContext.SaveChanges();
                            }

                            /*set result to userresult table*/

                            List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            dataContext.SaveChanges();

                            if (!string.IsNullOrEmpty(objCreateChallengeVM.ExeName1))
                            {
                                ExeciseChallengeCEVM objExeciseChallengeCEVM = new ExeciseChallengeCEVM
                                {
                                    ExeName = objCreateChallengeVM.ExeName1,
                                    ExeDesc = objCreateChallengeVM.ExeDesc1,
                                    ChallengeId = challengeId,
                                    ExeciseId = objCreateChallengeVM.ExerciseId1,
                                    UserId = credentialId,
                                    Reps = objCreateChallengeVM.Reps1,
                                    WeightForMan = objCreateChallengeVM.WeightForMan1,
                                    WeightForWoman = objCreateChallengeVM.WeightForWoman1
                                };
                                SubmitCEAssociation(objExeciseChallengeCEVM, string.Empty, false, objCreateChallengeVM.SelectedFitcomEquipment1,
                                    objCreateChallengeVM.SelectedFitcomTrainingZone1, objCreateChallengeVM.SelectedFitcomExeciseType1);
                            }



                            //Remove Old ExerciseType
                            List<tblETCAssociation> objETCAssociationList = dataContext.ETCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            dataContext.ETCAssociations.RemoveRange(objETCAssociationList);
                            dataContext.SaveChanges();
                            /*Add specialization information into database*/
                            List<tblETCAssociation> addObjETCAssociationList = new List<tblETCAssociation>();
                            /*primary specialization*/
                            if (objCreateChallengeVM.PostedExerciseTypes != null)
                            {
                                if (objCreateChallengeVM.PostedExerciseTypes.SelectedExerciseTypeIDs != null)
                                {
                                    for (int i = 0; i < objCreateChallengeVM.PostedExerciseTypes.SelectedExerciseTypeIDs.Count; i++)
                                    {
                                        tblETCAssociation objETCAssociation = new tblETCAssociation();
                                        objETCAssociation.ExerciseTypeId = Convert.ToInt32(objCreateChallengeVM.PostedExerciseTypes.SelectedExerciseTypeIDs[i]);
                                        objETCAssociation.ChallengeId = challengeId;
                                        addObjETCAssociationList.Add(objETCAssociation);
                                    }
                                }
                            }

                            /*Traiing Zone*/
                            List<tblTrainingZoneCAssociation> objtblTrainingZoneCAssociationnList = dataContext.TrainingZoneCAssociations
                                                                                                    .Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            dataContext.TrainingZoneCAssociations.RemoveRange(objtblTrainingZoneCAssociationnList);
                            dataContext.SaveChanges();
                            List<tblTrainingZoneCAssociation> addObjtblTrainingZoneCAssociationnList = new List<tblTrainingZoneCAssociation>();
                            if (objCreateChallengeVM.PostedTargetZones != null)
                            {
                                if (objCreateChallengeVM.PostedTargetZones.SelectedTargetZoneIDs != null)
                                {
                                    for (int i = 0; i < objCreateChallengeVM.PostedTargetZones.SelectedTargetZoneIDs.Count; i++)
                                    {
                                        tblTrainingZoneCAssociation objTrainingZoneCAssociation = new tblTrainingZoneCAssociation();
                                        objTrainingZoneCAssociation.PartId = Convert.ToInt32(objCreateChallengeVM.PostedTargetZones.SelectedTargetZoneIDs[i]);
                                        objTrainingZoneCAssociation.ChallengeId = challengeId;
                                        addObjtblTrainingZoneCAssociationnList.Add(objTrainingZoneCAssociation);
                                    }
                                }
                            }

                            dataContext.ETCAssociations.AddRange(addObjETCAssociationList);
                            if (addObjtblTrainingZoneCAssociationnList != null)
                            {
                                dataContext.TrainingZoneCAssociations.AddRange(addObjtblTrainingZoneCAssociationnList);
                            }

                            // Remove exixting trainer zone for challenge
                            List<tblCEquipmentAssociation> objtblCEquipmentAssociationList = dataContext.ChallengeEquipmentAssociations
                                                                                             .Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            if (objtblCEquipmentAssociationList != null)
                            {
                                dataContext.ChallengeEquipmentAssociations.RemoveRange(objtblCEquipmentAssociationList);
                            }
                            dataContext.SaveChanges();

                            // Added new Equipment for challenge
                            List<tblCEquipmentAssociation> eqipmentCAssociationList = new List<tblCEquipmentAssociation>();
                            if (objCreateChallengeVM.PostedEquipments != null)
                            {
                                if (objCreateChallengeVM.PostedEquipments.SelectedEquipmentIDs != null)
                                {
                                    for (int i = 0; i < objCreateChallengeVM.PostedEquipments.SelectedEquipmentIDs.Count; i++)
                                    {
                                        tblCEquipmentAssociation objEqipmentCAssociation = new tblCEquipmentAssociation();
                                        objEqipmentCAssociation.EquipmentId = Convert.ToInt32(objCreateChallengeVM.PostedEquipments.SelectedEquipmentIDs[i]);
                                        objEqipmentCAssociation.ChallengeId = challengeId;
                                        eqipmentCAssociationList.Add(objEqipmentCAssociation);
                                    }
                                    dataContext.ChallengeEquipmentAssociations.AddRange(eqipmentCAssociationList);
                                }
                            }
                            // Add primary trending category
                            List<tblChallengeTrendingAssociation> selectedTrendingCategory = new List<tblChallengeTrendingAssociation>();
                            if (objCreateChallengeVM.PostedTrendingCategory != null)
                            {
                                if (objCreateChallengeVM.PostedTrendingCategory.TrendingCategoryID != null)
                                {
                                    for (int i = 0; i < objCreateChallengeVM.PostedTrendingCategory.TrendingCategoryID.Count; i++)
                                    {
                                        int trendingCategoryId = Convert.ToInt32(objCreateChallengeVM.PostedTrendingCategory.TrendingCategoryID[i]);
                                        if (!dataContext.ChallengeTrendingAssociations.Any(utm => utm.ChallengeId == challengeId && utm.TrendingCategoryId == trendingCategoryId && utm.IsProgram == false))
                                        {
                                            tblChallengeTrendingAssociation tms = new tblChallengeTrendingAssociation()
                                            {
                                                TrendingCategoryId = trendingCategoryId,
                                                ChallengeId = challengeId,
                                                IsProgram = false
                                            };
                                            selectedTrendingCategory.Add(tms);
                                        }
                                    }                                   
                                }
                            }
                            // Fittness Secondary Trending category
                            if (objCreateChallengeVM.PostedSecondaryTrendingCategory != null)
                            {
                                if (objCreateChallengeVM.PostedSecondaryTrendingCategory.TrendingCategoryID != null)
                                {
                                    for (int i = 0; i < objCreateChallengeVM.PostedSecondaryTrendingCategory.TrendingCategoryID.Count; i++)
                                    {
                                        int trendingCategoryId = Convert.ToInt32(objCreateChallengeVM.PostedSecondaryTrendingCategory.TrendingCategoryID[i]);
                                        if (!dataContext.ChallengeTrendingAssociations.Any(utm => utm.ChallengeId == challengeId && utm.TrendingCategoryId == trendingCategoryId && utm.IsProgram == false))
                                        {
                                            tblChallengeTrendingAssociation tms = new tblChallengeTrendingAssociation()
                                            {
                                                TrendingCategoryId = trendingCategoryId,
                                                ChallengeId = challengeId,
                                                IsProgram = false
                                            };
                                            selectedTrendingCategory.Add(tms);
                                        }
                                    }                                    
                                }
                            }
                            if(selectedTrendingCategory.Count > 0)
                            { 
                            dataContext.ChallengeTrendingAssociations.AddRange(selectedTrendingCategory);
                            }
                            // Add No Trainer Teams assignment
                            if (!(trainerId > 0))
                            {
                                if (objCreateChallengeVM.PostedTeams != null)
                                {
                                    var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objCreateChallengeVM.PostedTeams, challengeId, false, true);
                                    dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                                }
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
        }
        /// <summary>
        /// Function to insert associate exercises in to database  
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="exeDesc"></param>
        /// <param name="challengeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void SubmitCEAssociation(ExeciseChallengeCEVM objExeciseChallengeCEVM, string alternateEName = "", bool isAlternateEName = false,
            string selectedEquipment = "", string selectedTraingZone = "", string selectedExeciseType = "")
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {

                    int exeId = -1;
                    var exercisedetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseId == objExeciseChallengeCEVM.ExeciseId);
                    if (exercisedetails != null)
                    {
                        exeId = exercisedetails.ExerciseId;
                    }
                    traceLog.AppendLine("Start: SubmitCEAssociation for submitting challenges exercise in to database");
                    tblCEAssociation objECAssociation = new tblCEAssociation();
                    objECAssociation.ChallengeId = objExeciseChallengeCEVM.ChallengeId;
                    objECAssociation.ExerciseId = exeId;
                    objECAssociation.Description = objExeciseChallengeCEVM.ExeDesc;
                    objECAssociation.CreatedBy = objExeciseChallengeCEVM.UserId;
                    objECAssociation.Reps = objExeciseChallengeCEVM.Reps == null ? 0 : (int)objExeciseChallengeCEVM.Reps;
                    objECAssociation.WeightForMan = objExeciseChallengeCEVM.WeightForMan == null ? 0 : (int)objExeciseChallengeCEVM.WeightForMan;
                    objECAssociation.WeightForWoman = objExeciseChallengeCEVM.WeightForWoman == null ? 0 : (int)objExeciseChallengeCEVM.WeightForWoman;
                    objECAssociation.CreatedDate = DateTime.Now;
                    objECAssociation.ModifiedBy = objECAssociation.CreatedBy;
                    objECAssociation.ModifiedDate = objECAssociation.CreatedDate;
                    objECAssociation.IsAlternateExeciseName = isAlternateEName;
                    objECAssociation.AlternateExeciseName = alternateEName;
                    objECAssociation.SelectedEquipment = selectedEquipment;
                    objECAssociation.SelectedExeciseType = selectedExeciseType;
                    objECAssociation.SelectedTraingZone = selectedTraingZone;
                    dataContext.CEAssociation.Add(objECAssociation);
                    dataContext.SaveChanges();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("SubmitCEAssociation  end() : --- " + DateTime.Now.ToLongDateString());
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
        public static List<Exercise> GetExerciseIndex(string term)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExerciseIndex for retrieving exercise index from database ");
                    List<Exercise> listOut = listOut = (from e in dataContext.Exercise
                                                        where e.ExerciseName.Contains(term) && e.IsActive && e.ExerciseStatus == 1
                                                        orderby e.ExerciseName ascending
                                                        select new Exercise { ExerciseName = e.ExerciseName, VedioLink = e.VideoLink }).ToList();
                    listOut.ForEach(
                        exer =>
                        {
                            exer.VedioLink = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exer.VedioLink;
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
                    traceLog.AppendLine("GetExerciseIndex  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to delete challenge  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void DeleteChallenge(int Id)
        {
            StringBuilder traceLog = new StringBuilder();

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: DeleteChallenge for deleting challenge");
                        if (Id > 0)
                        {
                            /*Delete challenge and exercise association*/
                            List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == Id).ToList();
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            /*Delete challenge and Traiing Zone association*/
                            List<tblTrainingZoneCAssociation> objTrainingZoneCAssociationList = dataContext.TrainingZoneCAssociations.Where(ce => ce.ChallengeId == Id).ToList();
                            dataContext.TrainingZoneCAssociations.RemoveRange(objTrainingZoneCAssociationList);

                            /*Delete hypevodeo*/
                            List<tblHypeVideo> objHypeVideo = dataContext.HypeVideos.Where(ce => ce.ChallengeId == Id).ToList();
                            dataContext.HypeVideos.RemoveRange(objHypeVideo);

                            /*Delete COD*/
                            List<tblChallengeofTheDayQueue> objChallengeofTheDayQueue = dataContext.ChallengeofTheDayQueue.Where(ce => ce.ChallengeId == Id).ToList();
                            dataContext.ChallengeofTheDayQueue.RemoveRange(objChallengeofTheDayQueue);

                            /*Delete sponser CHallenge*/
                            List<tblSponsorChallengeQueue> objTrainerChallenge = dataContext.TrainerChallenge.Where(ce => ce.ChallengeId == Id).ToList();
                            dataContext.TrainerChallenge.RemoveRange(objTrainerChallenge);

                            /*Delete userChallenge CHallenge*/
                            List<tblUserChallenges> objUserChallenges = dataContext.UserChallenge.Where(ce => ce.ChallengeId == Id).ToList();
                            if (objUserChallenges != null)
                            {
                                dataContext.UserChallenge.RemoveRange(objUserChallenges);
                            }
                            /*delete challenege*/
                            tblChallenge challenge = dataContext.Challenge.Find(Id);
                            if (challenge != null && challenge.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                            {
                                var deletedactiveweekworkouts = (from userActiveWorkout in dataContext.UserAcceptedProgramWorkouts
                                                                 where userActiveWorkout.WorkoutChallengeID == Id
                                                                 select userActiveWorkout).ToList();
                                if (deletedactiveweekworkouts != null)
                                {
                                    dataContext.UserAcceptedProgramWorkouts.RemoveRange(deletedactiveweekworkouts);
                                }
                            }

                            /*Delete usernotification CHallenge*/
                            string trainerChallege = NotificationType.TrainerChallege.ToString();
                            string friendChallege = NotificationType.FriendChallege.ToString();
                            List<tblUserNotifications> objUserNotifications = (from uc in dataContext.UserNotifications
                                                                               join c in dataContext.Challenge
                                                                               on uc.TargetID equals c.ChallengeId
                                                                               where c.ChallengeId == Id && uc.NotificationType == trainerChallege || uc.NotificationType == friendChallege
                                                                               select uc).ToList();
                            if (objUserNotifications != null)
                                dataContext.UserNotifications.RemoveRange(objUserNotifications);

                            dataContext.Challenge.Remove(challenge);
                            dataContext.SaveChanges();
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
                        traceLog.AppendLine("DeleteChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Function to get challenge by challenge id  
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="objSelectList"></param>
        /// <param name="userType"></param>
        /// <returns>CreateChallengeVM</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static CreateChallengeVM GetChallangeById(int id, ref List<SelectListItem> objSelectList, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetChallangeById for retrieving challenge by challengeid:" + id);
                    tblChallenge challenge = dataContext.Challenge.Find(id);
                    Mapper.CreateMap<tblChallenge, CreateChallengeVM>();
                    CreateChallengeVM objChalange =
                        Mapper.Map<tblChallenge, CreateChallengeVM>(challenge);
                    /*Get exercise detail for the respective challenge*/
                    List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == id).OrderByDescending(cc => cc.IsShownFirstExecise).ToList();
                    for (int i = 0; i < objCEAssociationList.Count; i++)
                    {
                        tblExercise exercise = dataContext.Exercise.Find(objCEAssociationList[i].ExerciseId);
                        if (i == 0)
                        {
                            objChalange.ExeDesc1 = objCEAssociationList[i].Description;
                            objChalange.ExerciseId = exercise != null ? exercise.ExerciseId : 0;
                            objChalange.ExerciseId1 = objChalange.ExerciseId;
                            objChalange.ExeName1 = exercise != null ? exercise.ExerciseName : string.Empty;
                            objChalange.ExeVideoLink1 = exercise != null ? exercise.V720pUrl : string.Empty;
                            objChalange.ExeVideoUrl1 = exercise != null ? exercise.V720pUrl : string.Empty; objChalange.Reps1 = objCEAssociationList[i].Reps;
                            objChalange.WeightForMan1 = objCEAssociationList[i].WeightForMan;
                            objChalange.WeightForWoman1 = objCEAssociationList[i].WeightForWoman;
                            objChalange.ExeIndexLink1 = exercise != null ? exercise.Index : string.Empty;
                            objChalange.FFExeDesc1 = objCEAssociationList[i].Description == ConstantHelper.constFFChallangeDescription ? string.Empty : objCEAssociationList[i].Description;
                            objChalange.FFExeName1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ? exercise.ExerciseName == ConstantHelper.constFFChallangeDescription ? string.Empty : exercise.ExerciseName : string.Empty;

                            objChalange.FFExeVideoLink1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ? exercise.V720pUrl : string.Empty;
                            objChalange.FFExeVideoUrl1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ? exercise.V720pUrl : string.Empty;
                            objChalange.IsFFAExeName1 = objCEAssociationList[i].IsAlternateExeciseName;
                            objChalange.FFAExeName1 = objCEAssociationList[i].AlternateExeciseName == ConstantHelper.constFFChallangeDescription ? string.Empty : objCEAssociationList[i].AlternateExeciseName;
                            objChalange.ExeciseSetDaetails = GetFFChallangeExeciseSetById(objCEAssociationList[i].RocordId);
                            objChalange.SelectedEquipment1 = objCEAssociationList[i].SelectedEquipment;
                            objChalange.SelectedTrainingZone1 = objCEAssociationList[i].SelectedTraingZone;
                            objChalange.SelectedExeciseType1 = objCEAssociationList[i].SelectedExeciseType;
                            objChalange.SelectedFitcomEquipment1 = objCEAssociationList[i].SelectedEquipment;
                            objChalange.SelectedFitcomTrainingZone1 = objCEAssociationList[i].SelectedTraingZone;
                            objChalange.SelectedFitcomExeciseType1 = objCEAssociationList[i].SelectedExeciseType;
                            objChalange.CEARocordId1 = objCEAssociationList[i].RocordId;
                            objChalange.IsSetFirstExecise = objCEAssociationList[i].IsShownFirstExecise;
                        }

                    }
                    if (objChalange.TrainerId != null)
                        objChalange.TrainerCredntialId = dataContext.Credentials.Where(ce => ce.Id == (int)objChalange.TrainerId).Select(y => y.UserId).FirstOrDefault();
                    objChalange.TrainerId = null;
                    /*get COD detail for the respective challenge */
                    if (userType == Message.UserTypeAdmin)
                    {
                        List<tblChallengeofTheDayQueue> challengeoftheDay = dataContext.ChallengeofTheDayQueue.Where(ce => ce.ChallengeId == id).ToList();
                        if (challengeoftheDay.Count != 0)
                        {
                            objChalange.CODStartDate = challengeoftheDay[0].StartDate;
                            objChalange.CODEndDate = challengeoftheDay[0].EndDate;
                            objChalange.EndUserNameId = challengeoftheDay[0].UserId;
                            objChalange.UserResultId = challengeoftheDay[0].ResultId;
                            objChalange.EndUserCredntialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId == objChalange.EndUserNameId && m.UserType == Message.UserTypeUser).Select(y => y.Id).FirstOrDefault());
                            int recordId = challengeoftheDay[0].HypeVideoId;
                            objChalange.UserVideoLink = dataContext.HypeVideos.Where(m => m.RecordId == recordId).Select(y => y.HypeVideo).FirstOrDefault();
                        }
                        /*get Trainer challenge detail for the respective challenge */
                        List<tblSponsorChallengeQueue> trainerChallenge = dataContext.TrainerChallenge.Where(ce => ce.ChallengeId == id).ToList();
                        if (trainerChallenge.Count != 0)
                        {
                            objChalange.TCStartDate = trainerChallenge[0].StartDate;
                            objChalange.TCEndDate = trainerChallenge[0].EndDate;
                            int recordId = trainerChallenge[0].HypeVideoId;
                            objChalange.TrainerVideoLink = dataContext.HypeVideos.Where(m => m.RecordId == recordId).Select(y => y.HypeVideo).FirstOrDefault();
                            objChalange.TrainerId = trainerChallenge[0].TrainerId;
                            objChalange.ResultId = trainerChallenge[0].ResultId;
                            objChalange.SponsorName = trainerChallenge[0].SponsorName;
                        }
                    }
                    List<ExerciseType> objListExerciseType = ChallengesCommonBL.GetExerciseTypes();
                    objChalange.AvailableExerciseTypes = objListExerciseType;
                    List<ExerciseType> objListSelectedExerciseType = ChallengesCommonBL.GetExerciseTypeOnChallengeId(objChalange.ChallengeId);
                    objChalange.SelectedExerciseTypes = objListSelectedExerciseType;

                    List<BodyPart> objListTrainingZone = ChallengesCommonBL.GetBodyParts();
                    objChalange.AvailableTargetZones = objListTrainingZone;
                    List<BodyPart> objListSelectedTargetZone = ChallengesCommonBL.GetTrainingZoneOnChallengeId(objChalange.ChallengeId);
                    objChalange.SelectedTargetZones = objListSelectedTargetZone;

                    List<Equipments> objListEquipments = ChallengesCommonBL.GetEquipments();
                    objChalange.AvailableEquipments = objListEquipments;
                    List<Equipments> objListSelectedEquipment = ChallengesCommonBL.GetEqipmentOnChallengeId(objChalange.ChallengeId);
                    objChalange.SelectedEquipments = objListSelectedEquipment;

                    /*return fraction list*/
                    if (objCEAssociationList.Count > 1)
                    {
                        objSelectList = ChallengesCommonBL.GetFraction(objCEAssociationList.Count);
                    }
                    string variablUnit = dataContext.ChallengeType.Where(ct => ct.ChallengeSubTypeId == objChalange.ChallengeSubTypeId).Select(u => u.Unit).FirstOrDefault();
                    if (!string.IsNullOrEmpty(variablUnit))
                    {
                        if (variablUnit.Equals(Message.VariableUnitTypeSecond))
                        {
                            string secondVariableValue = objChalange.VariableValue.Substring(6);
                            if (secondVariableValue.Contains('.'))
                            {
                                secondVariableValue = secondVariableValue.Split('.')[0];
                            }
                            objChalange.VariableValue = secondVariableValue;
                        }
                    }

                    objChalange.AvailableTeams = TeamBL.GetAllTeamName();
                    if (objChalange != null
                        && (objChalange.ChallengeSubTypeId != ConstantHelper.constProgramChallengeSubType && objChalange.ChallengeSubTypeId != ConstantHelper.constWellnessChallengeSubType)
                        && objChalange.ChallengeId > 0 && !(objChalange.TrainerId > 0))
                    {
                        objChalange.SelecetdTeams = (from tm in dataContext.Teams
                                                     join tt in dataContext.NoTrainerChallengeTeams
                                                     on tm.TeamId equals tt.TeamId
                                                     where tt.ChallengeId == objChalange.ChallengeId
                                                     orderby tm.TeamName ascending
                                                     select new DDTeams
                                                     {
                                                         TeamId = tt.TeamId,
                                                         TeamName = tm.TeamName,
                                                         IsDefaultTeam = tm.IsDefaultTeam
                                                     }).ToList();
                    }
                    var allTrendinglist= ChallengesCommonBL.GetTrendingCategory(challenge.ChallengeSubTypeId);
                    objChalange.AvailableTrendingCategory = allTrendinglist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                    if (objChalange != null && objChalange.ChallengeId > 0)
                    {
                        objChalange.SelecetdTrendingCategory = (from tm in dataContext.ChallengeTrendingAssociations
                                                                join tt in dataContext.TrendingCategory
                                                                on tm.TrendingCategoryId equals tt.TrendingCategoryId
                                                                where tm.ChallengeId == challenge.ChallengeId && tm.IsProgram == false
                                                                 && tt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId
                                                                orderby tt.TrendingName ascending
                                                                select new TrendingCategory
                                                                {
                                                                    TrendingCategoryId = tm.TrendingCategoryId,
                                                                    TrendingCategoryName = tt.TrendingName,
                                                                    challengeSubTypeId = 16
                                                                }).ToList();
                    }
                    objChalange.AvailableSecondaryTrendingCategory = allTrendinglist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    if (objChalange != null && objChalange.ChallengeId > 0)
                    {
                        objChalange.SelecetdSecondaryTrendingCategory = (from tm in dataContext.ChallengeTrendingAssociations
                                                                join tt in dataContext.TrendingCategory
                                                                on tm.TrendingCategoryId equals tt.TrendingCategoryId
                                                                where tm.ChallengeId == challenge.ChallengeId && tm.IsProgram == false
                                                                && tt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId
                                                                orderby tt.TrendingName ascending
                                                                select new TrendingCategory
                                                                {
                                                                    TrendingCategoryId = tm.TrendingCategoryId,
                                                                    TrendingCategoryName = tt.TrendingName,
                                                                    challengeSubTypeId = 16
                                                                }).ToList();
                    }

                    objChalange.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(challenge.ChallengeSubTypeId);
                    if (objChalange != null && objChalange.ChallengeId > 0)
                    {
                        objChalange.SelecetdChallengeCategory = (from tm in dataContext.ChallengeCategoryAssociations
                                                                 join tt in dataContext.ChallengeCategory
                                                                 on tm.ChallengeCategoryId equals tt.ChallengeCategoryId
                                                                 where tm.ChallengeId == challenge.ChallengeId && tm.IsProgram == false
                                                                 orderby tt.ChallengeCategoryName ascending
                                                                 select new ChallengeCategory
                                                                 {
                                                                     ChallengeCategoryId = tm.ChallengeCategoryId,
                                                                     ChallengeCategoryName = tt.ChallengeCategoryName
                                                                 }).ToList();
                        objChalange.SelectedChallengeCategoryCheck = Message.NotAvailable;
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
        /// Create Copy of existing workout challenges
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objSelectList"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static CreateChallengeVM GetCopyChallangeById(int id, ref List<SelectListItem> objSelectList, string userType, int credentialId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetChallangeById for retrieving challenge by challengeid:" + id);
                    var existingchallenge = dataContext.Challenge.Find(id);
                    tblChallenge challenge = null;
                    int newChallengeId = 0;
                    CreateChallengeVM objChalange = null;
                    if (existingchallenge != null)
                    {
                        challenge = new tblChallenge
                        {
                            TrainerId = existingchallenge.TrainerId,
                            IsActive = existingchallenge.IsActive,
                            IsDraft = existingchallenge.IsDraft,
                            Description = existingchallenge.Description,
                            DifficultyLevel = existingchallenge.DifficultyLevel,
                            FFChallengeDuration = existingchallenge.FFChallengeDuration,
                            GlobalResultFilterValue = existingchallenge.GlobalResultFilterValue,
                            IsPremium = existingchallenge.IsPremium,
                            ProgramImageUrl = existingchallenge.ProgramImageUrl,
                            VariableValue = existingchallenge.VariableValue,
                            ChallengeName = existingchallenge.ChallengeName + ConstantHelper.constChallengeCopyAppenedName,
                            ChallengeSubTypeId = existingchallenge.ChallengeSubTypeId,
                            ChallengeDetail = existingchallenge.ChallengeDetail,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = credentialId,
                            ModifiedBy = credentialId,
                            IsSubscription = existingchallenge.IsSubscription,
                            IsFeatured = existingchallenge.IsFeatured,
                            FeaturedImageUrl = existingchallenge.FeaturedImageUrl,
                            IsFreeFitnessTest = existingchallenge.IsFreeFitnessTest,
                        };
                        dataContext.Challenge.Add(challenge);
                        dataContext.SaveChanges();
                        newChallengeId = challenge.ChallengeId;
                        // Save new Challenge Execise with set
                        if (newChallengeId > 0)
                        {
                            var existingCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == id).ToList();
                            existingCEAssociationList.ForEach(ce =>
                             {
                                 tblCEAssociation chaleExec = new tblCEAssociation()
                                 {
                                     AlternateExeciseName = ce.AlternateExeciseName,
                                     ChallengeId = newChallengeId,
                                     CreatedBy = credentialId,
                                     ModifiedBy = credentialId,
                                     CreatedDate = DateTime.Now,
                                     ModifiedDate = DateTime.Now,
                                     Description = ce.Description,
                                     IsAlternateExeciseName = ce.IsAlternateExeciseName,
                                     Reps = ce.Reps,
                                     SelectedEquipment = ce.SelectedEquipment,
                                     SelectedExeciseType = ce.SelectedExeciseType,
                                     SelectedTraingZone = ce.SelectedTraingZone,
                                     WeightForMan = ce.WeightForMan,
                                     WeightForWoman = ce.WeightForMan,
                                     ExerciseId = ce.ExerciseId,
                                     IsShownFirstExecise = ce.IsShownFirstExecise
                                 };
                                 dataContext.CEAssociation.Add(chaleExec);
                                 dataContext.SaveChanges();
                                 int recordId = chaleExec.RocordId;
                                 var exercisesetlist = dataContext.CESAssociations.Where(ces => ces.RecordId == ce.RocordId);
                                 // Save the execise set data
                                 var chalgExeSetList = new List<tblCESAssociation>();
                                 foreach (var ces in exercisesetlist)
                                 {
                                     tblCESAssociation objtblCESAssociation = new tblCESAssociation()
                                     {
                                         Description = ces.Description,
                                         AutoCountDown = ces.AutoCountDown,
                                         IsRestType = ces.IsRestType,
                                         RecordId = recordId,
                                         RestTime = ces.RestTime,
                                         SetReps = ces.SetReps,
                                         CreatedBy = credentialId,
                                         ModifiedBy = credentialId,
                                         CreatedDate = DateTime.Now,
                                         ModifiedDate = DateTime.Now,
                                         SetResult = ces.SetResult
                                     };
                                     chalgExeSetList.Add(objtblCESAssociation);

                                 }
                                 dataContext.CESAssociations.AddRange(chalgExeSetList);
                                 dataContext.SaveChanges();
                             });

                            //Existing Old ExerciseType
                            var objETCAssociationList = dataContext.ETCAssociations.Where(ce => ce.ChallengeId == id).ToList();
                            /*Add specialization information into database*/
                            var ObjETCAssociationList = new List<tblETCAssociation>();
                            /*primary specialization*/
                            foreach (var cs in objETCAssociationList)
                            {
                                var ObjETCAssociation = new tblETCAssociation();
                                ObjETCAssociation.ExerciseTypeId = cs.ExerciseTypeId;
                                ObjETCAssociation.ChallengeId = newChallengeId;
                                ObjETCAssociationList.Add(ObjETCAssociation);
                            }
                            dataContext.ETCAssociations.AddRange(ObjETCAssociationList);
                            /*Traiing Zone*/
                            var objtblTrainingZoneCAssociationnList = dataContext.TrainingZoneCAssociations.Where(ce => ce.ChallengeId == id).ToList();
                            var addObjtblTrainingZoneCAssociationnList = new List<tblTrainingZoneCAssociation>();
                            foreach (var ctz in objtblTrainingZoneCAssociationnList)
                            {
                                var objTrainingZoneCAssociation = new tblTrainingZoneCAssociation();
                                objTrainingZoneCAssociation.PartId = ctz.PartId;
                                objTrainingZoneCAssociation.ChallengeId = newChallengeId;
                                addObjtblTrainingZoneCAssociationnList.Add(objTrainingZoneCAssociation);
                            }
                            dataContext.TrainingZoneCAssociations.AddRange(addObjtblTrainingZoneCAssociationnList);
                            // Exiting trainer zone for challenge
                            var objtblCEquipmentAssociationList = dataContext.ChallengeEquipmentAssociations.Where(ce => ce.ChallengeId == id).ToList();
                            // Added new Equipment for challenge
                            var eqipmentCAssociationList = new List<tblCEquipmentAssociation>();
                            foreach (var ctzc in objtblCEquipmentAssociationList)
                            {
                                var objEqipmentCAssociation = new tblCEquipmentAssociation();
                                objEqipmentCAssociation.EquipmentId = ctzc.EquipmentId;
                                objEqipmentCAssociation.ChallengeId = newChallengeId;
                                eqipmentCAssociationList.Add(objEqipmentCAssociation);
                            }
                            dataContext.ChallengeEquipmentAssociations.AddRange(eqipmentCAssociationList);
                            dataContext.SaveChanges();

                            var existingNoTrainerChallengeTeamsList = dataContext.NoTrainerChallengeTeams.Where(ce => ce.ChallengeId == id).ToList();
                            List<tblNoTrainerChallengeTeam> objChallengeTeamList = new List<tblNoTrainerChallengeTeam>();
                            existingNoTrainerChallengeTeamsList.ForEach(ce =>
                             {
                                 tblNoTrainerChallengeTeam notrainerWorkout = new tblNoTrainerChallengeTeam()
                                 {
                                     TeamId = ce.TeamId,
                                     ChallengeId = newChallengeId,
                                     IsProgram = false,
                                     IsFittnessTest = ce.IsFittnessTest
                                 };
                                 objChallengeTeamList.Add(notrainerWorkout);
                             });
                            dataContext.NoTrainerChallengeTeams.AddRange(objChallengeTeamList);

                            // Copy of Category category
                            var existingChallengeCategoryList = dataContext.ChallengeCategoryAssociations.Where(ce => ce.ChallengeId == id && ce.IsProgram == false).ToList();
                            List<tblChallengeCategoryAssociation> objChallengeCategoryList = new List<tblChallengeCategoryAssociation>();
                            existingChallengeCategoryList.ForEach(ce =>
                            {
                                tblChallengeCategoryAssociation chgCat = new tblChallengeCategoryAssociation()
                                {
                                    ChallengeCategoryId = ce.ChallengeCategoryId,
                                    ChallengeId = newChallengeId,
                                    IsProgram = false,

                                };
                                objChallengeCategoryList.Add(chgCat);
                            });
                            dataContext.ChallengeCategoryAssociations.AddRange(objChallengeCategoryList);

                            // Copy of Trending category
                            var existingTrendingCategoryList = dataContext.ChallengeTrendingAssociations.Where(ce => ce.ChallengeId == id && ce.IsProgram == false).ToList();
                            List<tblChallengeTrendingAssociation> objTrendingCategoryList = new List<tblChallengeTrendingAssociation>();
                            existingTrendingCategoryList.ForEach(ce =>
                            {
                                tblChallengeTrendingAssociation trenCat = new tblChallengeTrendingAssociation()
                                {
                                    TrendingCategoryId = ce.TrendingCategoryId,
                                    ChallengeId = newChallengeId,
                                    IsProgram = false,

                                };
                                objTrendingCategoryList.Add(trenCat);
                            });
                            dataContext.ChallengeTrendingAssociations.AddRange(objTrendingCategoryList);
                            dataContext.SaveChanges();

                            Mapper.CreateMap<tblChallenge, CreateChallengeVM>();
                            objChalange = Mapper.Map<tblChallenge, CreateChallengeVM>(challenge);
                            // Set new challenge id for copy challenges                   

                            /*Get exercise detail for the respective challenge*/
                            var objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == newChallengeId).OrderByDescending(cc => cc.IsShownFirstExecise).ToList();
                            for (int i = 0; i < objCEAssociationList.Count; i++)
                            {
                                var exercise = dataContext.Exercise.Find(objCEAssociationList[i].ExerciseId);
                                if (i == 0)
                                {
                                    objChalange.ExeDesc1 = objCEAssociationList[i].Description;
                                    objChalange.ExerciseId = exercise != null ? exercise.ExerciseId : 0;
                                    objChalange.ExeName1 = exercise != null ? exercise.ExerciseName : string.Empty;
                                    objChalange.ExeVideoLink1 = exercise != null ? CommonUtility.VirtualFitComExercisePath +
                                        Message.ExerciseVideoDirectory + exercise.VideoLink : string.Empty;
                                    objChalange.ExeVideoUrl1 = exercise != null ? CommonUtility.VirtualFitComExercisePath +
                                        Message.ExerciseVideoDirectory + exercise.VideoLink.Replace(" ", "%20") : string.Empty;
                                    objChalange.Reps1 = objCEAssociationList[i].Reps;
                                    objChalange.WeightForMan1 = objCEAssociationList[i].WeightForMan;
                                    objChalange.WeightForWoman1 = objCEAssociationList[i].WeightForWoman;
                                    objChalange.ExeIndexLink1 = exercise != null ? exercise.Index : string.Empty;
                                    objChalange.FFExeName1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ?
                                        exercise.ExerciseName == ConstantHelper.constFFChallangeDescription ? string.Empty : exercise.ExerciseName : string.Empty;
                                    objChalange.FFExeVideoLink1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ?
                                        CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exercise.VideoLink : string.Empty;
                                    objChalange.FFExeVideoUrl1 = (exercise != null && objCEAssociationList[i].IsAlternateExeciseName != true) ?
                                        CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exercise.VideoLink.Replace(" ", "%20") : string.Empty;
                                    objChalange.IsFFAExeName1 = objCEAssociationList[i].IsAlternateExeciseName;
                                    objChalange.FFAExeName1 = objCEAssociationList[i].AlternateExeciseName == ConstantHelper.constFFChallangeDescription ?
                                        string.Empty : objCEAssociationList[i].AlternateExeciseName;
                                    objChalange.ExeciseSetDaetails = GetFFChallangeExeciseSetById(objCEAssociationList[i].RocordId);
                                    objChalange.SelectedEquipment1 = objCEAssociationList[i].SelectedEquipment;
                                    objChalange.SelectedTrainingZone1 = objCEAssociationList[i].SelectedTraingZone;
                                    objChalange.SelectedExeciseType1 = objCEAssociationList[i].SelectedExeciseType;
                                    objChalange.SelectedFitcomEquipment1 = objCEAssociationList[i].SelectedEquipment;
                                    objChalange.SelectedFitcomTrainingZone1 = objCEAssociationList[i].SelectedTraingZone;
                                    objChalange.SelectedFitcomExeciseType1 = objCEAssociationList[i].SelectedExeciseType;
                                    objChalange.CEARocordId1 = objCEAssociationList[i].RocordId;
                                    objChalange.IsSetFirstExecise = objCEAssociationList[i].IsShownFirstExecise;
                                }

                            }
                            /*return fraction list*/
                            if (objCEAssociationList.Count > 1)
                            {
                                objSelectList = ChallengesCommonBL.GetFraction(objCEAssociationList.Count);
                            }
                        }
                        if (objChalange.TrainerId != null)
                            objChalange.TrainerCredntialId = dataContext.Credentials.Where(ce => ce.Id == (int)objChalange.TrainerId).Select(y => y.UserId).FirstOrDefault();
                        objChalange.TrainerId = null;
                        /*get COD detail for the respective challenge */
                        if (userType == Message.UserTypeAdmin)
                        {
                            var challengeoftheDay = dataContext.ChallengeofTheDayQueue.Where(ce => ce.ChallengeId == id).ToList();
                            if (challengeoftheDay.Count != 0)
                            {
                                objChalange.CODStartDate = challengeoftheDay[0].StartDate;
                                objChalange.CODEndDate = challengeoftheDay[0].EndDate;
                                objChalange.EndUserNameId = challengeoftheDay[0].UserId;
                                objChalange.UserResultId = challengeoftheDay[0].ResultId;
                                objChalange.EndUserCredntialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId == objChalange.EndUserNameId && m.UserType == Message.UserTypeUser)
                                    .Select(y => y.Id).FirstOrDefault());
                                int recordId = challengeoftheDay[0].HypeVideoId;
                                objChalange.UserVideoLink = dataContext.HypeVideos.Where(m => m.RecordId == recordId).Select(y => y.HypeVideo).FirstOrDefault();
                            }
                            /*get Trainer challenge detail for the respective challenge */
                            var trainerChallenge = dataContext.TrainerChallenge.Where(ce => ce.ChallengeId == id).ToList();
                            if (trainerChallenge.Count != 0)
                            {
                                objChalange.TCStartDate = trainerChallenge[0].StartDate;
                                objChalange.TCEndDate = trainerChallenge[0].EndDate;
                                int recordId = trainerChallenge[0].HypeVideoId;
                                objChalange.TrainerVideoLink = dataContext.HypeVideos.Where(m => m.RecordId == recordId)
                                    .Select(y => y.HypeVideo).FirstOrDefault();
                                objChalange.TrainerId = trainerChallenge[0].TrainerId;
                                objChalange.ResultId = trainerChallenge[0].ResultId;
                                objChalange.SponsorName = trainerChallenge[0].SponsorName;
                            }
                        }
                        // List<ExerciseType> objListExerciseType = ChallengesCommonBL.GetExerciseTypes();
                        objChalange.AvailableExerciseTypes = ChallengesCommonBL.GetExerciseTypes();
                        //  List<ExerciseType> objListSelectedExerciseType = ChallengesCommonBL.GetExerciseTypeOnChallengeId(objChalange.ChallengeId);
                        objChalange.SelectedExerciseTypes = ChallengesCommonBL.GetExerciseTypeOnChallengeId(objChalange.ChallengeId);

                        // List<BodyPart> objListTrainingZone = ChallengesCommonBL.GetBodyParts();
                        objChalange.AvailableTargetZones = ChallengesCommonBL.GetBodyParts(); ;
                        // List<BodyPart> objListSelectedTargetZone = ChallengesCommonBL.GetTrainingZoneOnChallengeId(objChalange.ChallengeId);
                        objChalange.SelectedTargetZones = ChallengesCommonBL.GetTrainingZoneOnChallengeId(objChalange.ChallengeId);

                        // List<Equipments> objListEquipments = ChallengesCommonBL.GetEquipments();
                        objChalange.AvailableEquipments = ChallengesCommonBL.GetEquipments();
                        // List<Equipments> objListSelectedEquipment = ChallengesCommonBL.GetEqipmentOnChallengeId(objChalange.ChallengeId);
                        objChalange.SelectedEquipments = ChallengesCommonBL.GetEqipmentOnChallengeId(objChalange.ChallengeId);
                        string variablUnit = dataContext.ChallengeType.Where(ct => ct.ChallengeSubTypeId == objChalange.ChallengeSubTypeId).Select(u => u.Unit).FirstOrDefault();
                        if (!string.IsNullOrEmpty(variablUnit))
                        {
                            if (variablUnit.Equals(Message.VariableUnitTypeSecond))
                            {
                                string secondVariableValue = objChalange.VariableValue.Substring(6);
                                if (secondVariableValue.Contains('.'))
                                {
                                    secondVariableValue = secondVariableValue.Split('.')[0];
                                }
                                objChalange.VariableValue = secondVariableValue;
                            }
                        }
                        objChalange.AvailableTeams = TeamBL.GetAllTeamName();
                        if (objChalange != null && (objChalange.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType
                            || objChalange.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType) && objChalange.ChallengeId > 0 && !(objChalange.TrainerId > 0))
                        {
                            objChalange.SelecetdTeams = CommonReportingUtility.GetSelectedNoTrainerChallengeTeam(dataContext, challenge.ChallengeId, false);
                        }
                        objChalange.AvailableTrendingCategory = ChallengesCommonBL.GetTrendingCategory(challenge.ChallengeSubTypeId);
                        if (objChalange != null && objChalange.ChallengeId > 0)
                        {
                            objChalange.SelecetdTrendingCategory = CommonReportingUtility.GetSelectedTrendingCategory(dataContext, challenge.ChallengeId, false);
                        }
                        objChalange.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(challenge.ChallengeSubTypeId);
                        if (objChalange != null && objChalange.ChallengeId > 0)
                        {
                            objChalange.SelecetdChallengeCategory = CommonReportingUtility.GetSelectedChallengeCategory(dataContext, challenge.ChallengeId, false);
                        }

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
        /// Get Free form Challange ExeciseSet ById
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
                    if (Id > 0)
                    {
                        execisesetList = new List<ExeciseSetVM>();
                        List<tblCESAssociation> objCESAssociationList = dataContext.CESAssociations.Where(ce => ce.RecordId == Id).ToList();
                        for (int i = 0; i < objCESAssociationList.Count; i++)
                        {
                            ExeciseSetVM objExerciseSet = new ExeciseSetVM();
                            objExerciseSet.SetResult = (objCESAssociationList[i].SetResult != null && objCESAssociationList[i].SetResult == ConstantHelper.constExeciseSetSeperator) ?
                                objCESAssociationList[i].SetResult : string.Empty;
                            objExerciseSet.RestTime = (objCESAssociationList[i].RestTime != null && objCESAssociationList[i].RestTime == ConstantHelper.constExeciseSetSeperator) ?
                                objCESAssociationList[i].RestTime : string.Empty;
                            objExerciseSet.Description = (objCESAssociationList[i].Description != null && objCESAssociationList[i].Description == ConstantHelper.constExeciseSetSeperator) ?
                                objCESAssociationList[i].Description : string.Empty;
                            objExerciseSet.SetReps = objCESAssociationList[i].SetReps;
                            objExerciseSet.IsAutoCountDown = objCESAssociationList[i].AutoCountDown;
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
        /// Function to update challenge
        /// </summary>
        /// <param name="objCreateChallengeVM"></param>
        /// <param name="credentialId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void UpdateChallenges(CreateChallengeVM objCreateChallengeVM, int credentialId)
        {
            StringBuilder traceLog = new StringBuilder();
            if (objCreateChallengeVM.ChallengeType == ConstantHelper.FreeformChallangeId)
            {
                FreeFormChallengeBL.UpdateAdminFreeFormChallenges(objCreateChallengeVM, credentialId);
            }
            else
            {
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
                                dataContext.SaveChanges();
                            }
                            var challengeId = objCreateChallengeVM.ChallengeId;
                            if (!string.IsNullOrEmpty(objCreateChallengeVM.ExeName1))
                            {
                                ExeciseChallengeCEVM objExeciseChallengeCEVM = new ExeciseChallengeCEVM
                                {
                                    ExeName = objCreateChallengeVM.ExeName1,
                                    ExeDesc = objCreateChallengeVM.ExeDesc1,
                                    ChallengeId = challengeId,
                                    UserId = credentialId,
                                    ExeciseId = objCreateChallengeVM.ExerciseId1,
                                    Reps = objCreateChallengeVM.Reps1,
                                    WeightForMan = objCreateChallengeVM.WeightForMan1,
                                    WeightForWoman = objCreateChallengeVM.WeightForWoman1
                                };
                                SubmitCEAssociation(objExeciseChallengeCEVM);
                            }

                            if (!string.IsNullOrEmpty(objCreateChallengeVM.VariableUnit))
                            {
                                if (objCreateChallengeVM.VariableUnit.Equals(Message.VariableUnitTypeSecond))
                                {
                                    objCreateChallengeVM.VariableValue = "00:00:" + (objCreateChallengeVM.VariableValue.Length == 1 ? "0" + objCreateChallengeVM.VariableValue : objCreateChallengeVM.VariableValue) + ".00";
                                }
                            }
                            //Add ".00" at the end of Variablevalue of type HH:MM:SS
                            //We have to work on VariableValue at create and update challenge by trainer
                            if (objCreateChallengeVM.VariableValue != null)
                            {
                                if (objCreateChallengeVM.VariableValue.Contains(ConstantHelper.constColon))
                                {
                                    objCreateChallengeVM.VariableValue = objCreateChallengeVM.VariableValue;
                                }
                            }

                            /*Update challenge*/
                            tblChallenge checkChallenge = dataContext.Challenge.Find(objCreateChallengeVM.ChallengeId);
                            checkChallenge.IsFeatured = objCreateChallengeVM.IsFeatured;
                            if (!string.IsNullOrEmpty(objCreateChallengeVM.FeaturedImageUrl))
                            {
                                checkChallenge.FeaturedImageUrl = objCreateChallengeVM.FeaturedImageUrl;
                            }
                            else
                            {
                                objCreateChallengeVM.FeaturedImageUrl = checkChallenge.FeaturedImageUrl;
                            }
                            // checkChallenge.TrendingCategoryId = objCreateChallengeVM.TrendingCategoryId??0;
                            dataContext.SaveChanges();
                            Mapper.CreateMap<CreateChallengeVM, tblChallenge>();
                            tblChallenge objChalange =
                            Mapper.Map<CreateChallengeVM, tblChallenge>(objCreateChallengeVM);
                            int trainerId = objChalange.TrainerId;
                            int sponsorTrainerCredentialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId
                                    == objCreateChallengeVM.TrainerId && m.UserType == Message.UserTypeTrainer)
                                    .Select(y => y.Id).FirstOrDefault());
                            int trainerCredentialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId
                                    == objCreateChallengeVM.TrainerCredntialId && m.UserType == Message.UserTypeTrainer)
                                    .Select(y => y.Id).FirstOrDefault());
                            objChalange.TrainerId = trainerCredentialId;
                            objChalange.ModifiedBy = credentialId;
                            objChalange.ModifiedDate = DateTime.Now;
                            dataContext.Entry(checkChallenge).CurrentValues.SetValues(objChalange);
                            dataContext.SaveChanges();
                            if (objCreateChallengeVM.IsActive)
                            {
                                int videoId = 0;
                                if (objCreateChallengeVM.IsSetToCOD)
                                {
                                    if (objCreateChallengeVM.CODStartDate != null && objCreateChallengeVM.CODEndDate != null
                                        && objCreateChallengeVM.EndUserNameId != 0 && objCreateChallengeVM.EndUserNameId != null
                                         && objCreateChallengeVM.UserResultId != 0 && objCreateChallengeVM.UserResultId != null
                                        && objCreateChallengeVM.UserVideoLink != null)
                                    {
                                        tblChallengeofTheDayQueue challengeoftheDay = dataContext.ChallengeofTheDayQueue.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).FirstOrDefault();
                                        tblChallengeofTheDayQueue challengeOfTheDay = new tblChallengeofTheDayQueue();
                                        challengeOfTheDay.ChallengeId = objCreateChallengeVM.ChallengeId;
                                        challengeOfTheDay.UserId = (int)objCreateChallengeVM.EndUserNameId;
                                        challengeOfTheDay.ResultId = (int)objCreateChallengeVM.UserResultId;
                                        challengeOfTheDay.NameOfChallenge = objCreateChallengeVM.ChallengeName;
                                        challengeOfTheDay.StartDate = objCreateChallengeVM.CODStartDate;
                                        challengeOfTheDay.EndDate = objCreateChallengeVM.CODEndDate;
                                        /*submit video*/
                                        int userIdCredentialId = Convert.ToInt32(dataContext.Credentials.Where(m => m.UserId
                                        == objCreateChallengeVM.EndUserNameId && m.UserType == Message.UserTypeUser)
                                        .Select(y => y.Id).FirstOrDefault());
                                        tblHypeVideo checkvideoUser = dataContext.HypeVideos.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId && ce.UserId == userIdCredentialId).FirstOrDefault();
                                        tblHypeVideo hypeVideo = new tblHypeVideo();
                                        hypeVideo.HypeVideo = objCreateChallengeVM.UserVideoLink;
                                        hypeVideo.ChallengeId = objCreateChallengeVM.ChallengeId;
                                        hypeVideo.UserId = userIdCredentialId;
                                        if (checkvideoUser != null)
                                        {
                                            hypeVideo.RecordId = checkvideoUser.RecordId;
                                            dataContext.Entry(checkvideoUser).CurrentValues.SetValues(hypeVideo);
                                            dataContext.SaveChanges();
                                            challengeOfTheDay.HypeVideoId = checkvideoUser.RecordId;
                                        }
                                        else
                                        {
                                            dataContext.HypeVideos.Add(hypeVideo);
                                            dataContext.SaveChanges();
                                            videoId = Convert.ToInt32(dataContext.HypeVideos.Max(x => x.RecordId));
                                            challengeOfTheDay.HypeVideoId = videoId;
                                        }
                                        if (challengeoftheDay != null)
                                        {
                                            challengeOfTheDay.QueueId = challengeoftheDay.QueueId;
                                            dataContext.Entry(challengeoftheDay).CurrentValues.SetValues(challengeOfTheDay);
                                        }
                                        else
                                        {
                                            dataContext.ChallengeofTheDayQueue.Add(challengeOfTheDay);
                                        }
                                        dataContext.SaveChanges();
                                    }
                                }
                                if (objCreateChallengeVM.IsSetToSponsorChallenge)
                                {
                                    if (objCreateChallengeVM.TCStartDate != null && objCreateChallengeVM.TCEndDate != null
                                        && objCreateChallengeVM.TrainerId != null && objCreateChallengeVM.ResultId != null && objCreateChallengeVM.SponsorName != null
                                        && objCreateChallengeVM.TrainerId != 0 && objCreateChallengeVM.ResultId != 0 && objCreateChallengeVM.TrainerVideoLink != null)
                                    {
                                        tblSponsorChallengeQueue trainerchallenges = dataContext.TrainerChallenge.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).FirstOrDefault();
                                        tblSponsorChallengeQueue trainerChallenge = new tblSponsorChallengeQueue();
                                        trainerChallenge.ChallengeId = objCreateChallengeVM.ChallengeId;
                                        trainerChallenge.NameOfChallenge = objCreateChallengeVM.ChallengeName;
                                        trainerChallenge.StartDate = objCreateChallengeVM.TCStartDate;
                                        trainerChallenge.SponsorName = objCreateChallengeVM.SponsorName;
                                        trainerChallenge.TrainerId = trainerId;
                                        trainerChallenge.EndDate = objCreateChallengeVM.TCEndDate;
                                        trainerChallenge.HypeVideoId = videoId;
                                        trainerChallenge.ResultId = (int)(objCreateChallengeVM.ResultId);
                                        tblHypeVideo checkvideoTrainer = dataContext.HypeVideos.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId && ce.UserId == sponsorTrainerCredentialId).FirstOrDefault();
                                        tblHypeVideo hypeVideo = new tblHypeVideo();
                                        hypeVideo.HypeVideo = objCreateChallengeVM.TrainerVideoLink;
                                        hypeVideo.ChallengeId = objCreateChallengeVM.ChallengeId;
                                        hypeVideo.UserId = sponsorTrainerCredentialId;
                                        if (checkvideoTrainer != null)
                                        {
                                            hypeVideo.RecordId = checkvideoTrainer.RecordId;
                                            dataContext.Entry(checkvideoTrainer).CurrentValues.SetValues(hypeVideo);
                                            dataContext.SaveChanges();
                                            trainerChallenge.HypeVideoId = checkvideoTrainer.RecordId;
                                        }
                                        else
                                        {
                                            dataContext.HypeVideos.Add(hypeVideo);
                                            dataContext.SaveChanges();
                                            videoId = Convert.ToInt32(dataContext.HypeVideos.Max(x => x.RecordId));
                                            trainerChallenge.HypeVideoId = videoId;
                                        }
                                        if (trainerchallenges != null)
                                        {
                                            trainerChallenge.QueueId = trainerchallenges.QueueId;
                                            dataContext.Entry(trainerchallenges).CurrentValues.SetValues(trainerChallenge);
                                        }
                                        else
                                        {
                                            dataContext.TrainerChallenge.Add(trainerChallenge);
                                        }

                                        dataContext.SaveChanges();
                                    }
                                }
                            }
                            //Remove Old ExerciseType
                            List<tblETCAssociation> objETCAssociationList = dataContext.ETCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            if (objETCAssociationList != null)
                            {
                                dataContext.ETCAssociations.RemoveRange(objETCAssociationList);
                            }
                            dataContext.SaveChanges();
                            /*Update ExerciseType information into database*/
                            if (objCreateChallengeVM.PostedExerciseTypes != null)
                            {
                                var eTCAssociationList = CommonReportingUtility.GetPostedExerciseTypeBasedChallenge(objCreateChallengeVM.PostedExerciseTypes, challengeId);
                                dataContext.ETCAssociations.AddRange(eTCAssociationList);
                            }

                            // Remove exixting trainer zone for challenge
                            List<tblTrainingZoneCAssociation> objtblTrainingZoneCAssociationList = dataContext.TrainingZoneCAssociations.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            if (objtblTrainingZoneCAssociationList != null)
                            {
                                dataContext.TrainingZoneCAssociations.RemoveRange(objtblTrainingZoneCAssociationList);
                            }
                            dataContext.SaveChanges();

                            // Added new Trainer zone for challenge

                            if (objCreateChallengeVM.PostedTargetZones != null)
                            {
                                List<tblTrainingZoneCAssociation> trainingZoneCAssociationList = CommonReportingUtility.GetPostedTargetZonesBasedChallenge(objCreateChallengeVM.PostedTargetZones, challengeId);
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
                            var challengeTrendingAssociationsList = dataContext.ChallengeTrendingAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == false).ToList();
                            if (challengeTrendingAssociationsList != null)
                            {
                                dataContext.ChallengeTrendingAssociations.RemoveRange(challengeTrendingAssociationsList);
                            }
                            dataContext.SaveChanges();
                            // Add the challenge trending associated trening categorys
                            if (objCreateChallengeVM.PostedTrendingCategory != null || objCreateChallengeVM.PostedSecondaryTrendingCategory != null)
                            {
                                List<tblChallengeTrendingAssociation> primaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedTrendingCategory, challengeId, false);
                                List<tblChallengeTrendingAssociation> secondaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objCreateChallengeVM.PostedSecondaryTrendingCategory, challengeId, false);
                                List<tblChallengeTrendingAssociation> allselectedTrendCatlist = primaryselectedTrendingCategory !=null ?primaryselectedTrendingCategory.Union(secondaryselectedTrendingCategory).ToList()
                                                                                                : secondaryselectedTrendingCategory;
                                dataContext.ChallengeTrendingAssociations.AddRange(allselectedTrendCatlist);
                            }

                            var challengeCategoryAssociationsList = dataContext.ChallengeCategoryAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == false).ToList();
                            if (challengeCategoryAssociationsList != null)
                            {
                                dataContext.ChallengeCategoryAssociations.RemoveRange(challengeCategoryAssociationsList);
                            }

                            var objtblNoTrainerChallengeTeamsList = dataContext.NoTrainerChallengeTeams.Where(ce => ce.ChallengeId == objCreateChallengeVM.ChallengeId).ToList();
                            if (objtblNoTrainerChallengeTeamsList != null)
                            {
                                dataContext.NoTrainerChallengeTeams.RemoveRange(objtblNoTrainerChallengeTeamsList);
                            }
                            dataContext.SaveChanges();
                            if (!(trainerId > 0))
                            {
                                if (objCreateChallengeVM.PostedTeams != null)
                                {
                                    var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objCreateChallengeVM.PostedTeams, challengeId, false, true);
                                    dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                                }
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
                            traceLog.AppendLine("UpdateChallennges  end() : --- " + DateTime.Now.ToLongDateString());
                            LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        }
                    }
                }
            }
        }
        /// Function to get credential id by trainer id
        /// </summary>
        /// <param name="trainerId"></param>        
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetCredentialId(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetCredentialId retrieving trainer Credentil Id from database ");
                    tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == trainerId && c.UserType == Message.UserTypeTrainer);
                    if (objCred != null)
                    {
                        return objCred.Id;
                    }
                    return 0;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetCredentialId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get trainers from database
        /// </summary>
        /// <returns>List<ViewTrainers></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<ViewTrainers> GetTrainers()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainers for retrieving trainer from database ");
                    return (from T in dataContext.Trainer
                            orderby (T.FirstName + " " + T.LastName)
                            select new ViewTrainers
                            {
                                TrainerName = T.FirstName + " " + T.LastName,
                                TrainerId = T.TrainerId
                            }).ToList<ViewTrainers>();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainers  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        #region Program
        /// <summary>
        /// Get Trainer Programs
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<ViewChallenes> GetTrainerPrograms(int trainerId, string userType, string search = null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerChellagnes for retrieving challenges from database ");
                    List<ViewChallenes> challenges = new List<ViewChallenes>();
                    List<ViewChallenes> filterdChallenges = new List<ViewChallenes>();

                    var query = (from C in dataContext.Challenge
                                 join CT in dataContext.ChallengeType on C.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                 where CT.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType
                                 orderby C.ModifiedDate descending
                                 select new ViewChallenes
                                 {
                                     TrainerId = C.TrainerId,
                                     ChallengeName = C.ChallengeName,
                                     Type = CT.ChallengeType,
                                     ResultUnit = CT.ResultUnit,
                                     ChallengeId = C.ChallengeId,
                                     DifficultyLevel = C.DifficultyLevel,
                                     ModifiedDate = C.ModifiedDate,
                                     IsActive = C.IsActive ? ConstantHelper.constYes : ConstantHelper.constNo,
                                     TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                       join bp in dataContext.Equipments
                                                       on trzone.EquipmentId equals bp.EquipmentId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.Equipment).Distinct().ToList<string>(),
                                     TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                       join bp in dataContext.BodyPart
                                                       on trzone.PartId equals bp.PartId
                                                       where trzone.ChallengeId == C.ChallengeId
                                                       select bp.PartName).Distinct().ToList<string>(),
                                     ChallengeDesc = CT.ChallengeSubType,
                                     VariableValue = C.VariableValue,
                                     IsDrafted = C.IsDraft,
                                     Strength = C.IsDraft ? 0 : dataContext.UserChallenge.Where(c => c.ChallengeId == C.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                     ChallengeCategoryName = (from cc in dataContext.ChallengeCategoryAssociations
                                                              join c in dataContext.ChallengeCategory
                                                              on cc.ChallengeCategoryId equals c.ChallengeCategoryId
                                                              where cc.ChallengeId == C.ChallengeId
                                                              && cc.IsProgram == (C.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                                                              select c.ChallengeCategoryName).ToList<string>()
                                 }).ToList();
                    /*challenges for all trainer else for related trainer*/
                    bool isSeached = false;
                    if (!string.IsNullOrEmpty(search))
                    {
                        isSeached = true;
                        search = search.ToUpper(CultureInfo.InvariantCulture);
                    }
                    if (trainerId == -1)
                    {
                        challenges = query.Where(ch => ch.TrainerId > 0 && (!isSeached || ((ch.ChallengeName != null && search != null
                        && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                       || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1))
                       )).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else if (trainerId == -2)
                    {
                        challenges = query.OrderBy(ch => ch.ChallengeName).ToList<ViewChallenes>();
                    }
                    else if (trainerId == 0)
                    {
                        challenges = query.Where(ch => ch.TrainerId == 0 && (!isSeached || ((ch.ChallengeName != null && search != null
                        && ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                       || (ch.DifficultyLevel != null && ch.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1))
                       )).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                    }
                    else
                    {
                        challenges = query.Where(q => q.TrainerId == trainerId).ToList();
                        if (challenges != null && challenges.Count > 0)
                        {
                            challenges = challenges.Where(q => !isSeached || ((q.ChallengeName != null
                            && q.ChallengeName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                            || (q.DifficultyLevel != null && q.DifficultyLevel.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)
                            )).OrderByDescending(ch => ch.ModifiedDate).ToList<ViewChallenes>();
                        }
                    }
                    challenges.ForEach(c =>
                    {
                        c.ChallengeDesc = c.ChallengeDesc.Replace("____", Convert.ToString(c.VariableValue, CultureInfo.InvariantCulture));
                        if (c.TempEquipments != null && c.TempEquipments.Count > 0)
                        {
                            c.Equipment = string.Join(", ", c.TempEquipments);
                        }
                        c.TempEquipments = null;
                        if (c.TempTargetZone != null && c.TempTargetZone.Count > 0)
                        {
                            c.TargetZone = string.Join(", ", c.TempTargetZone);
                        }
                        c.TempTargetZone = null;
                    });
                    foreach (var item in challenges)
                    {
                        bool flagAdd = true;
                        if (userType == Message.UserTypeAdmin && item.IsDrafted == true)
                        {
                            flagAdd = false;
                        }
                        if (flagAdd)
                        {
                            ViewChallenes filterdChallenge = new ViewChallenes();
                            filterdChallenge = item;
                            filterdChallenges.Add(filterdChallenge);
                        }
                    }

                    return filterdChallenges;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerChellagnes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        /// <summary>
        /// Get Program Type
        /// </summary>
        /// <returns></returns>
        public static List<ChallengeTypes> GetProgramType()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetChellagneType for retrieving challenges from database ");
                    Mapper.CreateMap<tblChallengeType, ChallengeTypes>();
                    List<tblChallengeType> lstChalangeType = dataContext.ChallengeType.Where(ct => ct.ChallengeType == ConstantHelper.constProgram).ToList();
                    List<ChallengeTypes> lstChalangeTypeVM =
                        Mapper.Map<List<tblChallengeType>, List<ChallengeTypes>>(lstChalangeType);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetChellagneType  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Remove the Progarm Weeks and their associted Workouts
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="programId"></param>

        public static void RemoveProgramWeeksworkouts(LinksMediaContext dataContext, int programId)
        {
            StringBuilder traceLog = new StringBuilder();

            try
            {
                traceLog.AppendLine("Start: RemoveProgramWeeksworkouts for program");
                List<tblPWAssociation> objPWAssociationList = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == programId).ToList();
                dataContext.PWAssociation.RemoveRange(objPWAssociationList);
                List<tblPWWorkoutsAssociation> objPWWorkoutsAssociationList = (from pw in dataContext.PWAssociation
                                                                               join pww in dataContext.PWWorkoutsAssociation
                                                                               on pw.PWRocordId equals pww.PWRocordId
                                                                               where pw.ProgramChallengeId == programId
                                                                               select pww
                                                               ).ToList();
                if (objPWWorkoutsAssociationList != null && objPWWorkoutsAssociationList.Count > 0)
                {
                    dataContext.PWWorkoutsAssociation.RemoveRange(objPWWorkoutsAssociationList);
                }

                dataContext.SaveChanges();

            }
            catch
            {

                throw;
            }
            finally
            {
                traceLog.AppendLine("RemoveProgramWeeksworkouts  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Submit Program
        /// </summary>
        /// <param name="objModels"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        /// <param name="draft"></param>
        public static void SubmitProgram(CreateAdminProgram objModels, int credentialId, int trainerId, string draft)
        {
            StringBuilder traceLog = new StringBuilder();

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: SubmitProgram for creating Program");
                        Mapper.CreateMap<CreateAdminProgram, tblChallenge>();
                        tblChallenge objChalange = Mapper.Map<CreateAdminProgram, tblChallenge>(objModels);
                        objChalange.CreatedBy = credentialId;
                        if (!trainerId.Equals(0))
                        {
                            objChalange.TrainerId = trainerId;
                        }
                        objChalange.ChallengeSubTypeId = objModels.ProgramType;
                        objChalange.ChallengeName = objModels.ProgramName;
                        objChalange.ModifiedBy = credentialId;
                        objChalange.CreatedDate = DateTime.Now;
                        objChalange.ModifiedDate = objChalange.CreatedDate;
                        objChalange.ProgramImageUrl = objChalange.ProgramImageUrl;
                        objChalange.FeaturedImageUrl = objChalange.FeaturedImageUrl;
                        if (!string.IsNullOrEmpty(draft) && draft.Equals(Message.SavetoDraft))
                        {
                            objChalange.IsDraft = true;
                        }
                        else
                        {
                            objChalange.IsDraft = false;
                        }
                        tblChallenge checkChallenge = dataContext.Challenge.Find(objModels.ChallengeId);
                        var challengeId = 0;
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
                            challengeId = objModels.ChallengeId;
                            // Remove Associated Week and Program Workouts
                            RemoveProgramWeeksworkouts(dataContext, challengeId);
                        }
                        //Add First Week Workouts witj sellected filter option//
                        traceLog.AppendLine("Start: Add First Week Workouts witj sellected filter option");
                        if (!string.IsNullOrEmpty(objModels.ProgramWorkouts))
                        {
                            if (!string.IsNullOrEmpty(objModels.ProgramWorkouts))
                            {
                                string[] selecetedWorkoutIds = objModels.ProgramWorkouts.Split(new char[1] { '^' });
                                tblPWAssociation objtblPWAssociation = new tblPWAssociation();
                                objtblPWAssociation.ProgramChallengeId = challengeId;
                                objtblPWAssociation.CreatedBy = credentialId;
                                objtblPWAssociation.ModifiedBy = credentialId;
                                objtblPWAssociation.CreatedDate = DateTime.Now;
                                objtblPWAssociation.ModifiedDate = DateTime.Now;
                                objtblPWAssociation.AssignedTrainerId = objModels.WorkoutTrainerId1 ?? 0;
                                objtblPWAssociation.AssignedTrainingzone = objModels.WorkoutTraingZoneId1 ?? 0;
                                objtblPWAssociation.AssignedDifficulyLevelId = objModels.WorkoutDifficultyLevelId1 ?? 0;
                                dataContext.PWAssociation.Add(objtblPWAssociation);
                                dataContext.SaveChanges();
                                SaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, selecetedWorkoutIds, objtblPWAssociation.CreatedBy);
                                dataContext.SaveChanges();
                            }
                        }
                        /*Add Week Workouts witj sellected filter option*/
                        traceLog.AppendLine("Start: Add All Week Workouts witj sellected filter option");
                        if (!string.IsNullOrEmpty(objModels.ProgramWeekWorkoutList))
                        {
                            string[] stringPieSeparators = new string[] { ConstantHelper.constSeperatorBarPipe };
                            string[] weekworkoutlist = objModels.ProgramWeekWorkoutList.Split(stringPieSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (weekworkoutlist != null && weekworkoutlist.Count() > 0)
                            {
                                for (int i = 0; i < weekworkoutlist.Count(); i++)
                                {
                                    string[] stringSeparators = new string[] { ConstantHelper.constSeperatorTildPipe };
                                    string[] selectedweekworkouts = weekworkoutlist[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                    if (selectedweekworkouts != null && selectedweekworkouts.Count() > 0)
                                    {
                                        string[] selecetedWorkoutIds = selectedweekworkouts[0].Split(new char[1] { ConstantHelper.constSeperatorCapPipe });

                                        tblPWAssociation objtblPWAssociation = null;
                                        int assignedTrainerId, assignedTrainingzone, assignedDifficulyLevelId;
                                        if (!int.TryParse(selectedweekworkouts[1], out assignedTrainerId))
                                        { assignedTrainerId = 0; }
                                        if (!int.TryParse(selectedweekworkouts[2], out assignedTrainingzone))
                                        { assignedTrainingzone = 0; }
                                        if (!int.TryParse(selectedweekworkouts[3], out assignedDifficulyLevelId))
                                        { assignedDifficulyLevelId = 0; }

                                        objtblPWAssociation = new tblPWAssociation();
                                        objtblPWAssociation.ProgramChallengeId = challengeId;
                                        objtblPWAssociation.CreatedBy = credentialId;
                                        objtblPWAssociation.ModifiedBy = credentialId;
                                        objtblPWAssociation.CreatedDate = DateTime.Now;
                                        objtblPWAssociation.ModifiedDate = DateTime.Now;
                                        objtblPWAssociation.AssignedTrainerId = assignedTrainerId;
                                        objtblPWAssociation.AssignedTrainingzone = assignedTrainingzone;
                                        objtblPWAssociation.AssignedDifficulyLevelId = assignedDifficulyLevelId;
                                        dataContext.PWAssociation.Add(objtblPWAssociation);
                                        dataContext.SaveChanges();
                                        SaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, selecetedWorkoutIds, credentialId);
                                        dataContext.SaveChanges();
                                    }
                                }
                            }
                        }
                        // Add all team except primary team
                        if (!(trainerId > 0))
                        {
                            if (objModels.PostedTeams != null)
                            {
                                var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objModels.PostedTeams, challengeId, true);
                                dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                            }
                        }
                        var challengeTrendingAssociationsList = dataContext.ChallengeTrendingAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == true).ToList();
                        if (challengeTrendingAssociationsList != null)
                        {
                            dataContext.ChallengeTrendingAssociations.RemoveRange(challengeTrendingAssociationsList);
                        }
                        dataContext.SaveChanges();
                        // Add the challenge trending associated trening categorys
                        if (objModels.PostedTrendingCategory != null || objModels.PostedSecondaryTrendingCategory != null)
                        {
                            var primaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objModels.PostedTrendingCategory, challengeId, true);
                            var secondaryselectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objModels.PostedSecondaryTrendingCategory, challengeId, true);
                            List<tblChallengeTrendingAssociation> allSelectedTrending = primaryselectedTrendingCategory != null ?primaryselectedTrendingCategory.Union(secondaryselectedTrendingCategory).ToList()
                                                                                         : secondaryselectedTrendingCategory;
                            dataContext.ChallengeTrendingAssociations.AddRange(allSelectedTrending);
                        }
                        // Add the challenge trending associated trening categorys

                        var challengeCategoryAssociationsList = dataContext.ChallengeCategoryAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == true).ToList();
                        if (challengeCategoryAssociationsList != null)
                        {
                            dataContext.ChallengeCategoryAssociations.RemoveRange(challengeCategoryAssociationsList);
                        }
                        if (objModels.PostedChallengeCategory != null)
                        {
                            var selectedChallengeCategory = CommonReportingUtility.GetPostedChallengeCategoryBasedChallenge(dataContext, objModels.PostedChallengeCategory, challengeId, true);
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
                        traceLog.AppendLine("SubmitProgram  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objModels = null;
                    }
                }
            }
        }
        /// <summary>
        /// Copy Program with new Program Name
        /// </summary>
        /// <param name="objModels"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        public static void CopyProgram(CreateAdminProgram objModels, int credentialId, int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: SubmitProgram for creating Program");
                        Mapper.CreateMap<CreateAdminProgram, tblChallenge>();
                        tblChallenge objChalange = Mapper.Map<CreateAdminProgram, tblChallenge>(objModels);
                        objChalange.CreatedBy = credentialId;
                        if (!trainerId.Equals(0))
                        {
                            objChalange.TrainerId = trainerId;
                        }
                        objChalange.ChallengeSubTypeId = objModels.ProgramType;
                        objChalange.ChallengeName = objModels.ProgramName;
                        objChalange.ModifiedBy = credentialId;
                        objChalange.CreatedDate = DateTime.Now;
                        objChalange.ModifiedDate = objChalange.CreatedDate;
                        tblChallenge checkChallenge = dataContext.Challenge.Find(objModels.ChallengeId);
                        var challengeId = 0;
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
                            challengeId = objModels.ChallengeId;
                        }

                        //Add First Week Workouts witj sellected filter option//
                        traceLog.AppendLine("Start: Add First Week Workouts witj sellected filter option");
                        if (!string.IsNullOrEmpty(objModels.ProgramWorkouts))
                        {
                            if (!string.IsNullOrEmpty(objModels.ProgramWorkouts))
                            {
                                string[] selecetedWorkoutIds = objModels.ProgramWorkouts.Split(new char[1] { '^' });
                                tblPWAssociation objtblPWAssociation = new tblPWAssociation();
                                objtblPWAssociation.ProgramChallengeId = challengeId;
                                objtblPWAssociation.CreatedBy = credentialId;
                                objtblPWAssociation.ModifiedBy = credentialId;
                                objtblPWAssociation.CreatedDate = DateTime.Now;
                                objtblPWAssociation.ModifiedDate = DateTime.Now;
                                objtblPWAssociation.AssignedTrainerId = objModels.WorkoutTrainerId1 ?? 0;
                                objtblPWAssociation.AssignedTrainingzone = objModels.WorkoutTraingZoneId1 ?? 0;
                                objtblPWAssociation.AssignedDifficulyLevelId = objModels.WorkoutDifficultyLevelId1 ?? 0;
                                dataContext.PWAssociation.Add(objtblPWAssociation);
                                dataContext.SaveChanges();
                                SaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, selecetedWorkoutIds, objtblPWAssociation.CreatedBy);
                                dataContext.SaveChanges();
                            }
                        }
                        /*Add Week Workouts witj sellected filter option*/
                        traceLog.AppendLine("Start: Add All Week Workouts witj sellected filter option");
                        if (!string.IsNullOrEmpty(objModels.ProgramWeekWorkoutList))
                        {
                            string[] stringPieSeparators = new string[] { ConstantHelper.constSeperatorBarPipe };
                            string[] weekworkoutlist = objModels.ProgramWeekWorkoutList.Split(stringPieSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (weekworkoutlist != null && weekworkoutlist.Count() > 0)
                            {
                                for (int i = 0; i < weekworkoutlist.Count(); i++)
                                {
                                    string[] stringSeparators = new string[] { ConstantHelper.constSeperatorTildPipe };
                                    string[] selectedweekworkouts = weekworkoutlist[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                    if (selectedweekworkouts != null && selectedweekworkouts.Count() > 0)
                                    {
                                        string[] selecetedWorkoutIds = selectedweekworkouts[0].Split(new char[1] { ConstantHelper.constSeperatorCapPipe });

                                        tblPWAssociation objtblPWAssociation = null;
                                        int assignedTrainerId, assignedTrainingzone, assignedDifficulyLevelId;
                                        if (!int.TryParse(selectedweekworkouts[1], out assignedTrainerId))
                                        { assignedTrainerId = 0; }
                                        if (!int.TryParse(selectedweekworkouts[2], out assignedTrainingzone))
                                        { assignedTrainingzone = 0; }
                                        if (!int.TryParse(selectedweekworkouts[3], out assignedDifficulyLevelId))
                                        { assignedDifficulyLevelId = 0; }

                                        objtblPWAssociation = new tblPWAssociation();
                                        objtblPWAssociation.ProgramChallengeId = challengeId;
                                        objtblPWAssociation.CreatedBy = credentialId;
                                        objtblPWAssociation.ModifiedBy = credentialId;
                                        objtblPWAssociation.CreatedDate = DateTime.Now;
                                        objtblPWAssociation.ModifiedDate = DateTime.Now;
                                        objtblPWAssociation.AssignedTrainerId = assignedTrainerId;
                                        objtblPWAssociation.AssignedTrainingzone = assignedTrainingzone;
                                        objtblPWAssociation.AssignedDifficulyLevelId = assignedDifficulyLevelId;
                                        dataContext.PWAssociation.Add(objtblPWAssociation);
                                        dataContext.SaveChanges();
                                        SaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, selecetedWorkoutIds, credentialId);
                                        dataContext.SaveChanges();
                                    }
                                }
                            }
                        }
                        // Add all team except primary team
                        if (!(trainerId > 0))
                        {
                            if (objModels.PostedTeams != null)
                            {
                                var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objModels.PostedTeams, challengeId, true);
                                dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);
                            }
                        }
                        // Add the challenge trending associated trening categorys
                        if (objModels.PostedTrendingCategory != null)
                        {
                            var selectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objModels.PostedTrendingCategory, challengeId, true);
                            dataContext.ChallengeTrendingAssociations.AddRange(selectedTrendingCategory);
                        }
                        // Add the challenge trending associated trening categorys

                        if (objModels.PostedChallengeCategory != null)
                        {
                            var selectedChallengeCategory = CommonReportingUtility.GetPostedChallengeCategoryBasedChallenge(dataContext, objModels.PostedChallengeCategory, challengeId, true);
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
                        traceLog.AppendLine("SubmitProgram  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objModels = null;
                    }
                }
            }
        }

        /// <summary>
        /// Submit Program Week WorkoutIds 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="programWeekId"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        private static bool UpdateSaveWeekWorkoutData(LinksMediaContext dataContext, long programWRocordId, string selecetedWorkoutIds, int createdBy)
        {
            try
            {
                if (!string.IsNullOrEmpty(selecetedWorkoutIds))
                {
                    // List<tblPWWorkoutsAssociation> listtblPWWorkoutsAssociation = new List<tblPWWorkoutsAssociation>();
                    string[] stringSeparators = new string[] { "<>" };
                    string[] selecetedwekworkoutList = selecetedWorkoutIds.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (selecetedwekworkoutList != null && selecetedwekworkoutList.Count() > 0)
                    {
                        for (int i = 0; i < selecetedwekworkoutList.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(selecetedwekworkoutList[i]))
                            {
                                string[] selecetedExeeciseList = selecetedwekworkoutList[i].Split(new char[1] { '^' });
                                int workoutId, existingworkoutId;
                                if (!int.TryParse(selecetedExeeciseList[0], out workoutId))
                                {
                                    workoutId = 0;
                                }
                                if (!int.TryParse(selecetedExeeciseList[1], out existingworkoutId))
                                {
                                    existingworkoutId = 0;
                                }
                                if (workoutId > 0)
                                {
                                    tblPWWorkoutsAssociation objtblPWWorkoutsAssociation = null;
                                    if (existingworkoutId > 0)
                                    {
                                        objtblPWWorkoutsAssociation = dataContext.PWWorkoutsAssociation.FirstOrDefault(pww => pww.PWWorkoutId == existingworkoutId);
                                        objtblPWWorkoutsAssociation.PWRocordId = programWRocordId;
                                        objtblPWWorkoutsAssociation.WorkoutChallengeId = workoutId;
                                        objtblPWWorkoutsAssociation.ModifiedBy = createdBy;
                                        objtblPWWorkoutsAssociation.ModifiedDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        objtblPWWorkoutsAssociation = new tblPWWorkoutsAssociation();
                                        objtblPWWorkoutsAssociation.PWRocordId = programWRocordId;
                                        objtblPWWorkoutsAssociation.WorkoutChallengeId = workoutId;
                                        objtblPWWorkoutsAssociation.CreatedBy = createdBy;
                                        objtblPWWorkoutsAssociation.ModifiedBy = createdBy;
                                        objtblPWWorkoutsAssociation.CreatedDate = DateTime.Now;
                                        objtblPWWorkoutsAssociation.ModifiedDate = DateTime.Now;
                                        dataContext.PWWorkoutsAssociation.Add(objtblPWWorkoutsAssociation);
                                    }
                                    dataContext.SaveChanges();
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Save Week Workouts Data in database
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="programWRocordId"></param>
        /// <param name="selecetedWorkoutIds"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        private static bool SaveWeekWorkoutData(LinksMediaContext dataContext, long programWRocordId, string[] selecetedWorkoutIds, int createdBy)
        {
            try
            {
                if (selecetedWorkoutIds != null && selecetedWorkoutIds.Count() > 0)
                {
                    List<tblPWWorkoutsAssociation> listtblPWWorkoutsAssociation = new List<tblPWWorkoutsAssociation>();
                    foreach (string workoutId in selecetedWorkoutIds)
                    {
                        int workoutvalue;
                        if (int.TryParse(workoutId, out workoutvalue))
                        {
                            tblPWWorkoutsAssociation objtblPWWorkoutsAssociation = new tblPWWorkoutsAssociation();
                            objtblPWWorkoutsAssociation.PWRocordId = programWRocordId;
                            objtblPWWorkoutsAssociation.WorkoutChallengeId = workoutvalue;
                            objtblPWWorkoutsAssociation.CreatedBy = createdBy;
                            objtblPWWorkoutsAssociation.ModifiedBy = createdBy;
                            objtblPWWorkoutsAssociation.CreatedDate = DateTime.Now;
                            objtblPWWorkoutsAssociation.ModifiedDate = DateTime.Now;
                            listtblPWWorkoutsAssociation.Add(objtblPWWorkoutsAssociation);
                        }
                    }
                    if (listtblPWWorkoutsAssociation.Count > 0)
                    {
                        dataContext.PWWorkoutsAssociation.AddRange(listtblPWWorkoutsAssociation);
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Get Program By ProgramId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CreateAdminProgram GetProgramById(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetProgramById for retrieving Program by challengeid:" + id);
                    tblChallenge challenge = dataContext.Challenge.Find(id);
                    Mapper.CreateMap<tblChallenge, CreateAdminProgram>();
                    CreateAdminProgram objCreateAdminProgram =
                        Mapper.Map<tblChallenge, CreateAdminProgram>(challenge);
                    objCreateAdminProgram.ProgramType = challenge.ChallengeSubTypeId;
                    objCreateAdminProgram.SelectedChallengeTypeId = challenge.ChallengeSubTypeId;
                    objCreateAdminProgram.ProgramName = challenge.ChallengeName;
                    objCreateAdminProgram.FeaturedImageUrl = challenge.FeaturedImageUrl;
                    objCreateAdminProgram.IsFeatured = challenge.IsFeatured;
                    objCreateAdminProgram.IsFeatured = challenge.IsDraft;
                    if (challenge.TrainerId > 0)
                    {
                        objCreateAdminProgram.TrainerCredId = dataContext.Credentials.Where(ce => ce.Id == challenge.TrainerId).Select(u => u.UserId).FirstOrDefault();
                    }
                    tblPWAssociation objPWAssociation = dataContext.PWAssociation.Where(ce => ce.ProgramChallengeId == id).FirstOrDefault();
                    if (objPWAssociation != null)
                    {
                        objCreateAdminProgram.WorkoutTrainerId1 = objPWAssociation.AssignedTrainerId;
                        objCreateAdminProgram.WorkoutTraingZoneId1 = objPWAssociation.AssignedTrainingzone;
                        objCreateAdminProgram.WorkoutDifficultyLevelId1 = objPWAssociation.AssignedDifficulyLevelId;

                        objCreateAdminProgram.IsProgramNewWeek1 = objPWAssociation.PWRocordId;
                        var firstweekworkout = dataContext.PWWorkoutsAssociation.Where(ce => ce.PWRocordId == objPWAssociation.PWRocordId).FirstOrDefault();
                        if (firstweekworkout != null)
                        {
                            objCreateAdminProgram.IsProgramNewWeekWorkout1 = firstweekworkout.PWRocordId;
                            objCreateAdminProgram.ProgramWeekHidenWorkout1 = firstweekworkout.WorkoutChallengeId;
                            objCreateAdminProgram.ProgramWorkoutLink1 = CommonUtility.VirtualPath + ConstantHelper.constProgramViewChallenge + firstweekworkout.WorkoutChallengeId;
                        }

                    }
                    objCreateAdminProgram.AvailableTeams = TeamBL.GetAllTeamName();
                    if (objCreateAdminProgram != null && objCreateAdminProgram.ChallengeId > 0 && !(objCreateAdminProgram.TrainerId > 0))
                    {
                        objCreateAdminProgram.SelecetdTeams = CommonReportingUtility.GetSelectedTeamByChallengeId(dataContext, objCreateAdminProgram.ChallengeId, true);
                    }
                    var allSaveTrendinglist=ChallengesCommonBL.GetTrendingCategory(challenge.ChallengeSubTypeId);
                    if (allSaveTrendinglist != null)
                    {
                        objCreateAdminProgram.AvailableTrendingCategory = allSaveTrendinglist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                        objCreateAdminProgram.AvailableSecondaryTrendingCategory = allSaveTrendinglist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                    }
                    if (objCreateAdminProgram != null && objCreateAdminProgram.ChallengeId > 0)
                    {
                        var allSavedTrendingCatlist= CommonReportingUtility.GetChallengeTrendingAssociationsList(dataContext, challenge.ChallengeId, ConstantHelper.constProgramChallengeSubType, true);
                        if (allSavedTrendingCatlist != null)
                        {
                            objCreateAdminProgram.SelecetdTrendingCategory = allSavedTrendingCatlist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                            objCreateAdminProgram.SelecetdSecondaryTrendingCategory = allSavedTrendingCatlist.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                        }
                    }
                    //objCreateAdminProgram.AvailableSecondaryTrendingCategory = ChallengesCommonBL.GetTrendingCategory(challenge.ChallengeSubTypeId);
                    //if (objCreateAdminProgram != null && objCreateAdminProgram.ChallengeId > 0)
                    //{
                    //    objCreateAdminProgram.SelecetdTrendingCategory = CommonReportingUtility.GetChallengeTrendingAssociationsList(dataContext, challenge.ChallengeId, ConstantHelper.constProgramChallengeSubType, true);
                    //}

                    objCreateAdminProgram.AvailableChallengeCategory = ChallengesCommonBL.GetChallengeCategory(challenge.ChallengeSubTypeId);
                    if (objCreateAdminProgram != null && objCreateAdminProgram.ChallengeId > 0)
                    {
                        objCreateAdminProgram.SelecetdChallengeCategory = CommonReportingUtility.GetSelectedChallengeCategoryAssociations(dataContext, challenge.ChallengeId, true);

                        objCreateAdminProgram.SelectedChallengeCategoryCheck = Message.NotAvailable;
                    }
                    return objCreateAdminProgram;
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
        /// Update Program
        /// </summary>
        /// <param name="objModels"></param>
        /// <param name="credentialId"></param>
        /// <param name="trainerId"></param>
        /// <param name="draft"></param>
        public static void UpdateProgram(CreateAdminProgram objModels, int credentialId, int trainerId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: UpdateProgram for creating Program");
                        tblChallenge objChalange = dataContext.Challenge.FirstOrDefault(ch => ch.ChallengeId == objModels.ChallengeId && ch.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType);
                        if (objChalange != null)
                        {
                            objChalange.CreatedBy = credentialId;
                            if (!trainerId.Equals(0))
                            {
                                objChalange.TrainerId = trainerId;
                            }
                            objChalange.DifficultyLevel = objModels.DifficultyLevel;
                            objChalange.ModifiedBy = credentialId;
                            objChalange.ModifiedDate = DateTime.Now;
                            objChalange.ChallengeSubTypeId = objModels.ProgramType;
                            objChalange.ChallengeName = objModels.ProgramName;
                            objChalange.IsActive = objModels.IsActive;
                            objChalange.Description = objModels.Description;
                            objChalange.IsPremium = objModels.IsPremium;
                            objChalange.IsSubscription = objModels.IsSubscription;
                            objChalange.IsFeatured = objModels.IsFeatured;
                            if (!string.IsNullOrEmpty(objModels.ProgramImageUrl))
                            {
                                objChalange.ProgramImageUrl = objModels.ProgramImageUrl;
                            }
                            if (!string.IsNullOrEmpty(objModels.FeaturedImageUrl))
                            {
                                objChalange.FeaturedImageUrl = objModels.FeaturedImageUrl;
                            }
                            int challengeId = 0;
                            dataContext.Entry(objChalange).CurrentValues.SetValues(objChalange);
                            dataContext.SaveChanges();
                            challengeId = objModels.ChallengeId;
                            // Remove the Deleted week and weekworkouts
                            if (!string.IsNullOrEmpty(objModels.RemovedWeekWorkouts))
                            {
                                string[] stringPieSeparators = new string[] { ConstantHelper.constSeperatorBarPipe };
                                string[] removeweekworkoutlist = objModels.RemovedWeekWorkouts.Split(stringPieSeparators, StringSplitOptions.RemoveEmptyEntries);
                                if (removeweekworkoutlist != null && removeweekworkoutlist.Count() > 0)
                                {
                                    for (int i = 0; i < removeweekworkoutlist.Count(); i++)
                                    {
                                        string[] stringSeparators = new string[] { ConstantHelper.constSeperatorTildPipe };
                                        string[] selectedweekworkouts = removeweekworkoutlist[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                        if (selectedweekworkouts != null && selectedweekworkouts.Count() > 0)
                                        {
                                            int removedWeekworkout, removedWeekId;
                                            if (!int.TryParse(selectedweekworkouts[0], out removedWeekId))
                                            { removedWeekId = 0; }
                                            if (!int.TryParse(selectedweekworkouts[1], out removedWeekworkout))
                                            {
                                                removedWeekworkout = 0;
                                            }
                                            if (removedWeekId > 0 && removedWeekworkout > 0)
                                            {
                                                var deletedweekworkouts = dataContext.PWWorkoutsAssociation.FirstOrDefault(pww => pww.PWRocordId == removedWeekId && pww.PWWorkoutId == removedWeekworkout);
                                                if (deletedweekworkouts != null)
                                                {
                                                    dataContext.PWWorkoutsAssociation.Remove(deletedweekworkouts);
                                                }
                                                var deletedactiveweekworkouts = (from userActiveWorkout in dataContext.UserAcceptedProgramWorkouts
                                                                                 where userActiveWorkout.ProgramChallengeId == objModels.ChallengeId && userActiveWorkout.PWeekID == removedWeekId && userActiveWorkout.PWWorkoutID == removedWeekworkout
                                                                                 select userActiveWorkout).ToList();
                                                if (deletedactiveweekworkouts != null)
                                                {
                                                    dataContext.UserAcceptedProgramWorkouts.RemoveRange(deletedactiveweekworkouts);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(selectedweekworkouts[1]) && selectedweekworkouts[1] == ConstantHelper.constALL && removedWeekId > 0)
                                            {
                                                var objPWAssociation = dataContext.PWAssociation.FirstOrDefault(ce => ce.PWRocordId == removedWeekId);
                                                if (objPWAssociation != null)
                                                {
                                                    long pWRocordId = objPWAssociation.PWRocordId;
                                                    dataContext.PWAssociation.Remove(objPWAssociation);
                                                    var objPWWorkoutsAssociationList = (from pw in dataContext.PWAssociation
                                                                                        join pww in dataContext.PWWorkoutsAssociation
                                                                                        on pw.PWRocordId equals pww.PWRocordId
                                                                                        where pww.PWRocordId == pWRocordId
                                                                                        select pww).ToList();
                                                    if (objPWWorkoutsAssociationList != null)
                                                    {
                                                        dataContext.PWWorkoutsAssociation.RemoveRange(objPWWorkoutsAssociationList);
                                                    }
                                                    var deletedactiveweekworkouts = (from userActiveWorkout in dataContext.UserAcceptedProgramWorkouts
                                                                                     where userActiveWorkout.ProgramChallengeId == objModels.ChallengeId && userActiveWorkout.PWeekID == pWRocordId
                                                                                     select userActiveWorkout).ToList();
                                                    if (deletedactiveweekworkouts != null)
                                                    {
                                                        dataContext.UserAcceptedProgramWorkouts.RemoveRange(deletedactiveweekworkouts);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dataContext.SaveChanges();
                                }
                            }
                            // Add unlimited Execise Type
                            traceLog.AppendLine("Start: SubmitWeekWorkoutAssociation for submitting Program");
                            if (!string.IsNullOrEmpty(objModels.ProgramWorkouts))
                            {

                                tblPWAssociation objtblPWAssociation = null;
                                if (objModels.IsProgramNewWeek1 > 0)
                                {
                                    objtblPWAssociation = dataContext.PWAssociation.FirstOrDefault(pw => pw.PWRocordId == objModels.IsProgramNewWeek1);
                                    if (objtblPWAssociation != null)
                                    {
                                        objtblPWAssociation.ProgramChallengeId = challengeId;
                                        objtblPWAssociation.CreatedBy = credentialId;
                                        objtblPWAssociation.ModifiedBy = credentialId;
                                        objtblPWAssociation.CreatedDate = DateTime.Now;
                                        objtblPWAssociation.ModifiedDate = DateTime.Now;
                                        objtblPWAssociation.AssignedTrainerId = objModels.WorkoutTrainerId1 ?? 0;
                                        objtblPWAssociation.AssignedTrainingzone = objModels.WorkoutTraingZoneId1 ?? 0;
                                        objtblPWAssociation.AssignedDifficulyLevelId = objModels.WorkoutDifficultyLevelId1 ?? 0;
                                    }
                                }
                                else
                                {
                                    objtblPWAssociation.ProgramChallengeId = challengeId;
                                    objtblPWAssociation.CreatedBy = credentialId;
                                    objtblPWAssociation.ModifiedBy = credentialId;
                                    objtblPWAssociation.CreatedDate = DateTime.Now;
                                    objtblPWAssociation.ModifiedDate = DateTime.Now;
                                    objtblPWAssociation.AssignedTrainerId = objModels.WorkoutTrainerId1 ?? 0;
                                    objtblPWAssociation.AssignedTrainingzone = objModels.WorkoutTraingZoneId1 ?? 0;
                                    objtblPWAssociation.AssignedDifficulyLevelId = objModels.WorkoutDifficultyLevelId1 ?? 0;
                                    dataContext.PWAssociation.Add(objtblPWAssociation);

                                }
                                dataContext.SaveChanges();
                                UpdateSaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, objModels.ProgramWorkouts, objtblPWAssociation.CreatedBy);
                                dataContext.SaveChanges();

                            }
                            if (!string.IsNullOrEmpty(objModels.ProgramWeekWorkoutList))
                            {
                                string[] stringPieSeparators = new string[] { ConstantHelper.constSeperatorBarPipe };
                                string[] weekworkoutlist = objModels.ProgramWeekWorkoutList.Split(stringPieSeparators, StringSplitOptions.RemoveEmptyEntries);
                                if (weekworkoutlist != null && weekworkoutlist.Count() > 0)
                                {
                                    for (int i = 0; i < weekworkoutlist.Count(); i++)
                                    {
                                        string[] stringSeparators = new string[] { ConstantHelper.constSeperatorTildPipe };
                                        string[] selectedweekworkouts = weekworkoutlist[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                        if (selectedweekworkouts != null && selectedweekworkouts.Count() > 0)
                                        {
                                            tblPWAssociation objtblPWAssociation = null;
                                            int assignedTrainerId, assignedTrainingzone, assignedDifficulyLevelId, existingWeekId;
                                            if (!int.TryParse(selectedweekworkouts[1], out assignedTrainerId))
                                            { assignedTrainerId = 0; }
                                            if (!int.TryParse(selectedweekworkouts[2], out assignedTrainingzone))
                                            { assignedTrainingzone = 0; }
                                            if (!int.TryParse(selectedweekworkouts[3], out assignedDifficulyLevelId))
                                            { assignedDifficulyLevelId = 0; }
                                            if (!int.TryParse(selectedweekworkouts[4], out existingWeekId))
                                            { existingWeekId = 0; }

                                            if (existingWeekId > 0)
                                            {
                                                objtblPWAssociation = dataContext.PWAssociation.FirstOrDefault(pw => pw.PWRocordId == existingWeekId);
                                                if (objtblPWAssociation != null)
                                                {
                                                    objtblPWAssociation.ProgramChallengeId = challengeId;
                                                    objtblPWAssociation.ModifiedBy = credentialId;
                                                    objtblPWAssociation.ModifiedDate = DateTime.Now;
                                                    objtblPWAssociation.AssignedTrainerId = assignedTrainerId;
                                                    objtblPWAssociation.AssignedTrainingzone = assignedTrainingzone;
                                                    objtblPWAssociation.AssignedDifficulyLevelId = assignedDifficulyLevelId;
                                                }
                                            }
                                            else
                                            {
                                                objtblPWAssociation = new tblPWAssociation();
                                                objtblPWAssociation.ProgramChallengeId = challengeId;
                                                objtblPWAssociation.CreatedBy = credentialId;
                                                objtblPWAssociation.ModifiedBy = credentialId;
                                                objtblPWAssociation.CreatedDate = DateTime.Now;
                                                objtblPWAssociation.ModifiedDate = DateTime.Now;
                                                objtblPWAssociation.AssignedTrainerId = assignedTrainerId;
                                                objtblPWAssociation.AssignedTrainingzone = assignedTrainingzone;
                                                objtblPWAssociation.AssignedDifficulyLevelId = assignedDifficulyLevelId;
                                                dataContext.PWAssociation.Add(objtblPWAssociation);
                                                dataContext.SaveChanges();
                                            }

                                            dataContext.SaveChanges();
                                            UpdateSaveWeekWorkoutData(dataContext, objtblPWAssociation.PWRocordId, selectedweekworkouts[0], credentialId);
                                            dataContext.SaveChanges();
                                        }
                                    }
                                }
                            }

                            if (!(trainerId > 0))
                            {
                                // Remove exixting No trainer for challenge
                                var objtblNoTrainerChallengeTeamsList = dataContext.NoTrainerChallengeTeams.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == true).ToList();
                                if (objtblNoTrainerChallengeTeamsList != null)
                                {
                                    dataContext.NoTrainerChallengeTeams.RemoveRange(objtblNoTrainerChallengeTeamsList);
                                }
                                dataContext.SaveChanges();
                                // Add all team except primary team

                                if (objModels.PostedTeams != null)
                                {
                                    var selextedteams = CommonReportingUtility.GetPostedTeamsBasedChallenge(dataContext, objModels.PostedTeams, challengeId, true);
                                    dataContext.NoTrainerChallengeTeams.AddRange(selextedteams);

                                }
                            }
                            var challengeTrendingAssociationsList = dataContext.ChallengeTrendingAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == true).ToList();
                            if (challengeTrendingAssociationsList != null)
                            {
                                dataContext.ChallengeTrendingAssociations.RemoveRange(challengeTrendingAssociationsList);
                            }
                            dataContext.SaveChanges();
                            // Add the challenge trending associated trening categorys
                            if (objModels.PostedTrendingCategory != null || objModels.PostedSecondaryTrendingCategory != null)
                            {
                               var selectedTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objModels.PostedTrendingCategory, challengeId, true);
                               var selectedSecondaryTrendingCategory = CommonReportingUtility.GetPostedTrendingCategoryBasedChallenge(dataContext, objModels.PostedSecondaryTrendingCategory, challengeId, true);
                               List<tblChallengeTrendingAssociation> allSelectedTrendinfcat = selectedTrendingCategory != null ? selectedTrendingCategory.Union(selectedSecondaryTrendingCategory).ToList()
                                                                                                                              : selectedSecondaryTrendingCategory;
                               dataContext.ChallengeTrendingAssociations.AddRange(allSelectedTrendinfcat);
                            }

                            var challengeCategoryAssociationsList = dataContext.ChallengeCategoryAssociations.Where(ce => ce.ChallengeId == challengeId && ce.IsProgram == true).ToList();
                            if (challengeCategoryAssociationsList != null)
                            {
                                dataContext.ChallengeCategoryAssociations.RemoveRange(challengeCategoryAssociationsList);
                            }
                            dataContext.SaveChanges();
                            // Add the challenge trending associated trening categorys
                            if (objModels.PostedChallengeCategory != null)
                            {
                                var selectedChallengeCategory = CommonReportingUtility.GetPostedChallengeCategoryBasedChallenge(dataContext, objModels.PostedChallengeCategory, challengeId, true);
                                dataContext.ChallengeCategoryAssociations.AddRange(selectedChallengeCategory);
                            }
                            dataContext.SaveChanges();
                        }
                        dbTran.Commit();
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("UpdateProgram  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objModels = null;
                    }
                }
            }
        }

        /// <summary>
        /// Get Selecetd Index Exercises based on searched cateria
        /// </summary>
        /// <param name="objSearchExeciseVM"></param>
        /// <returns></returns>
        public static List<Exercise> GetSelecetdIndexExercises(SearchExeciseVM objSearchExeciseVM)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    List<Exercise> listOut = new List<Exercise>();
                    if (objSearchExeciseVM != null)
                    {
                        traceLog.AppendLine("Start: GetExerciseIndex for retrieving exercise index from database ");
                        List<string> selectedIndex = new List<string>();
                        bool IsSerch = true;
                        bool IsSerchItem = true;
                        if (!string.IsNullOrEmpty(objSearchExeciseVM.SelectedEquipement))
                        {
                            selectedIndex.Add(objSearchExeciseVM.SelectedEquipement);
                            IsSerch = false;
                        }
                        if (!string.IsNullOrEmpty(objSearchExeciseVM.SelectedTrainingZone))
                        {
                            selectedIndex.Add(objSearchExeciseVM.SelectedTrainingZone);
                            IsSerch = false;
                        }
                        if (!string.IsNullOrEmpty(objSearchExeciseVM.SelectedExeciseType))
                        {
                            selectedIndex.Add(objSearchExeciseVM.SelectedExeciseType);
                            IsSerch = false;
                        }
                        if (!string.IsNullOrEmpty(objSearchExeciseVM.SearchTerm))
                        {
                            objSearchExeciseVM.SearchTerm = objSearchExeciseVM.SearchTerm.Trim().ToUpper(CultureInfo.InvariantCulture);
                            IsSerchItem = false;
                        }
                        List<Exercise> execiselist = (from e in dataContext.Exercise
                                                      where e.IsActive && !string.IsNullOrEmpty(e.V720pUrl) && e.ExerciseStatus == 1
                                                      orderby e.ExerciseName ascending
                                                      select new Exercise { ExerciseId = e.ExerciseId, ExerciseName = e.ExerciseName, Index = e.Index, Description = e.Description, VedioLink = e.V720pUrl, IsActive = e.IsActive }).ToList();
                        if (!IsSerch && !IsSerchItem)
                        {
                            listOut = execiselist.Where(exe => ((!string.IsNullOrEmpty(exe.Index) && selectedIndex.All(e => exe.Index.Split(',').Select(p => p.Trim()).ToList().Contains(e)))
                                   && exe.ExerciseName.ToUpper(CultureInfo.InvariantCulture).Contains(objSearchExeciseVM.SearchTerm))).ToList();
                        }
                        else if (!IsSerch)
                        {
                            listOut = execiselist.Where(exe => !string.IsNullOrEmpty(exe.Index) && selectedIndex.All(e => exe.Index.Split(',').Select(p => p.Trim()).ToList().Contains(e))).ToList();
                        }
                        else if (!IsSerchItem)
                        {
                            listOut = execiselist.Where(exe => !string.IsNullOrEmpty(exe.ExerciseName) && exe.ExerciseName.ToUpper(CultureInfo.InvariantCulture).Contains(objSearchExeciseVM.SearchTerm)).ToList();
                        }
                        listOut.ForEach(
                            exer =>
                            {
                                exer.VedioLink = string.IsNullOrEmpty(exer.VedioLink) ? CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exer.VedioLink : exer.VedioLink;
                            });
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
        /// Seach onborading index execise videos
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static List<Exercise> GetSelecetdIndexExercises(string searchTerm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExerciseIndex for retrieving exercise index from database ");
                    List<string> selectedIndex = new List<string>();
                    selectedIndex.Add(ConstantHelper.constOnboardingVideoIndex);
                    bool IsSerchItem = true;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        searchTerm = searchTerm.ToUpper(CultureInfo.InvariantCulture);
                        IsSerchItem = false;
                    }
                    List<Exercise> execiselist = (from e in dataContext.Exercise
                                                  where e.IsActive && !string.IsNullOrEmpty(e.V720pUrl) && e.ExerciseStatus == 1
                                                  orderby e.ExerciseName ascending
                                                  select new Exercise { ExerciseId = e.ExerciseId, ExerciseName = e.ExerciseName, Index = e.Index, Description = e.Description, VedioLink = e.V720pUrl, IsActive = e.IsActive }).ToList();
                    if (execiselist != null)
                    {
                        execiselist = execiselist.Where(exe => ((!string.IsNullOrEmpty(exe.Index) && selectedIndex.All(e => exe.Index.Split(',').Select(p => p.Trim()).ToList().Contains(e))))).ToList();
                    }
                    // Search the onborading execise video
                    if (!IsSerchItem)
                    {
                        execiselist = execiselist.Where(exe => !string.IsNullOrEmpty(exe.ExerciseName) && exe.ExerciseName.ToUpper(CultureInfo.InvariantCulture).Contains(searchTerm)).ToList();
                    }

                    return execiselist;
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
        #endregion
    }
}