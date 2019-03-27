namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;

    public class ChallengeOfTheDayBL
    {
        /// <summary>
        /// Function to get challenge details of challenge of the day
        /// </summary>
        /// <returns>ChallengeOfTheDayDetailVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/30/2015
        /// </devdoc>
        public static ChallengeOfTheDayDetailVM GetChallengeOfTheDayDetails(int challengeId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetChallengeOfTheDayDetails---- " + DateTime.Now.ToLongDateString());
                    char[] splitChar = { ConstantHelper.constCharchatColon };
                    string tempValue = string.Empty;
                    ChallengeOfTheDayDetailVM objChallengedetail = (from c in _dbContext.Challenge
                                                                    join cred in _dbContext.Credentials on c.CreatedBy equals cred.Id
                                                                    join ct in _dbContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                    join cod in _dbContext.ChallengeofTheDayQueue on c.ChallengeId equals cod.ChallengeId
                                                                    join uc in _dbContext.UserChallenge on cod.ResultId equals uc.Id
                                                                    join h in _dbContext.HypeVideos on cod.HypeVideoId equals h.RecordId
                                                                    where c.ChallengeId == challengeId && c.IsActive
                                                                    select new ChallengeOfTheDayDetailVM
                                                                    {
                                                                        ChallengeId = c.ChallengeId,
                                                                        ChallengeName = c.ChallengeName,
                                                                        DifficultyLevel = c.DifficultyLevel,
                                                                        ChallengeType = ct.ChallengeType,
                                                                        TempEquipments = (from trzone in _dbContext.ChallengeEquipmentAssociations
                                                                                          join bp in _dbContext.Equipments
                                                                                          on trzone.EquipmentId equals bp.EquipmentId
                                                                                          where trzone.ChallengeId == c.ChallengeId
                                                                                          select bp.Equipment).Distinct().ToList<string>(),
                                                                        TempTargetZone = (from trzone in _dbContext.TrainingZoneCAssociations
                                                                                          join bp in _dbContext.BodyPart
                                                                                          on trzone.PartId equals bp.PartId
                                                                                          where trzone.ChallengeId == c.ChallengeId
                                                                                          select bp.PartName).Distinct().ToList<string>(),
                                                                        Strenght = _dbContext.UserChallenge.Where(uchlng => uchlng.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                        ResultUnit = ct.ResultUnit,
                                                                        ResultUnitSuffix = uc.ResultUnit,
                                                                        ChallengeDescription = ct.ChallengeSubType,
                                                                        Excercises = (from exer in _dbContext.Exercise
                                                                                      join ce in _dbContext.CEAssociation on exer.ExerciseId equals ce.ExerciseId
                                                                                      where ce.ChallengeId == challengeId
                                                                                      select new ExerciseVM
                                                                                      {
                                                                                          ExerciseId = exer.ExerciseId,
                                                                                          ExerciseName = exer.ExerciseName,
                                                                                          ExcersiceDescription = exer.Description,
                                                                                          ChallengeExcerciseDescription = ce.Description,
                                                                                          VedioLink = exer.VideoLink,
                                                                                          Index = exer.Index,
                                                                                          Reps = ce.Reps,
                                                                                          WeightForManDB = ce.WeightForMan,
                                                                                          WeightForWomanDB = ce.WeightForWoman,
                                                                                      }).ToList(),
                                                                        HypeVideo = h.HypeVideo,
                                                                        Result = (uc.Result == null ? string.Empty : uc.Result) + " " + (uc.Fraction == null ? string.Empty : uc.Fraction),
                                                                        VariableValue = c.VariableValue,
                                                                        ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                        VariableUnit = ct.Unit,
                                                                        CreatedByUserType = cred.UserType,
                                                                        CreatedByTrainerId = cred.UserId,
                                                                        PersonalBestUserId = uc.UserId
                                                                    }).FirstOrDefault();
                    if (objChallengedetail != null)
                    {
                        if (objChallengedetail.TempEquipments != null && objChallengedetail.TempEquipments.Count > 0)
                        {
                            objChallengedetail.Equipment = string.Join(", ", objChallengedetail.TempEquipments);
                        }
                        objChallengedetail.TempEquipments = null;
                        if (objChallengedetail.TempTargetZone != null && objChallengedetail.TempTargetZone.Count > 0)
                        {
                            objChallengedetail.TargetZone = string.Join(", ", objChallengedetail.TempTargetZone);
                        }
                        objChallengedetail.TempTargetZone = null;
                        if (objChallengedetail.ResultUnit.Equals(ConstantHelper.constTime))
                        {
                            string tempResult = objChallengedetail.Result;
                            string[] spliResult = tempResult.Split(splitChar);
                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                objChallengedetail.Result = spliResult[1] + ConstantHelper.constColon + spliResult[2];
                            }
                            else if (spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                objChallengedetail.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                            }
                        }
                        if (!string.IsNullOrEmpty(objChallengedetail.Result))
                        {
                            objChallengedetail.Result = objChallengedetail.Result.Trim();
                        }
                        if (!string.IsNullOrEmpty(objChallengedetail.ChallengeType))
                        {
                            objChallengedetail.ChallengeType = objChallengedetail.ChallengeType.Split(' ')[0];
                        }
                        if (objChallengedetail.CreatedByUserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var objTrainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == objChallengedetail.CreatedByTrainerId);
                            if (objTrainer != null)
                            {
                                objChallengedetail.CreatedByTrainerName = objTrainer.FirstName + " " + objTrainer.LastName;
                                objChallengedetail.CreatedByProfilePic = string.IsNullOrEmpty(objTrainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainer.TrainerImageUrl;
                            }
                        }
                        else
                        {
                            objChallengedetail.CreatedByTrainerId = 0;
                        }
                    }

                    objChallengedetail.Excercises.ForEach(
                       exer =>
                       {
                           exer.WeightForMan = exer.WeightForManDB > 0 ? exer.WeightForManDB.ToString() + " " + ConstantHelper.constlbs : null;
                           exer.WeightForWoman = exer.WeightForWomanDB > 0 ? exer.WeightForWomanDB.ToString() + " " + ConstantHelper.constlbs : null;
                           exer.VedioLink = string.IsNullOrEmpty(exer.VedioLink) ? string.Empty : CommonUtility.VirtualPath + Message.ExerciseVideoDirectory + exer.VedioLink;
                           exer.ExerciseThumnail = CommonWebApiBL.GetSaveExerciseThumbnail(exer.VedioLink, exer.ExerciseName);

                       });
                    //// Modify the varivale in 02 Hour(s), 05 Minute(s), 10 Second(s) format
                    string tempdesc = string.Empty;
                    if (objChallengedetail != null && !string.IsNullOrEmpty(objChallengedetail.VariableValue))
                    {
                        if (objChallengedetail.VariableUnit == ConstantHelper.constVariableUnitminutes || objChallengedetail.VariableUnit == ConstantHelper.constVariableUnitseconds)
                        {
                            string[] variableValueWithMS = objChallengedetail.VariableValue.Split(new char[1] { '.' });
                            if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                            {
                                if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                                {
                                    //Code for HH:MM:SS And MM:SS format                                                                   
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
                                    tempdesc = tempValue;
                                }
                                else
                                {
                                    tempdesc = variableValueWithMS[0].ToString();
                                }
                            }
                        }
                        else
                        {
                            tempdesc = objChallengedetail.VariableValue;
                        }
                    }
                    ////End the modification
                    if (!string.IsNullOrEmpty(tempdesc))
                    {
                        tempdesc = tempdesc.Trim();
                    }
                    objChallengedetail.ChallengeDescription = objChallengedetail.ChallengeDescription.Replace("____", tempdesc).Replace(" amount of time?", "?").Replace(" seconds?", "?");
                    if (objChallengedetail != null && !string.IsNullOrEmpty(objChallengedetail.VariableValue))
                    {
                        string[] variableValueWithMS = objChallengedetail.VariableValue.Split(new char[1] { '.' });
                        if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                        {
                            if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                            {
                                //Code for HH:MM:SS And MM:SS format
                                tempValue = variableValueWithMS[0];
                                string[] spliResult = tempValue.Split(splitChar);
                                if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    variableValueWithMS[0] = spliResult[1] + ConstantHelper.constColon + spliResult[2];
                                }
                                else if (spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    variableValueWithMS[0] = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                }
                                objChallengedetail.VariableValue = variableValueWithMS[0] + (variableValueWithMS[1].Equals(ConstantHelper.constTimeVariableUnit) ?
                                    ConstantHelper.constDotDoubleZero : (ConstantHelper.constDot + variableValueWithMS[1]));
                            }
                        }
                    }
                    var objCredentials = _dbContext.Credentials.FirstOrDefault(c => c.Id == objChallengedetail.PersonalBestUserId);
                    if (objCredentials != null && objCredentials.UserType.Equals(Message.UserTypeUser))
                    {
                        objChallengedetail.PersonalBestUserName = (from usr in _dbContext.User
                                                                   join creden in _dbContext.Credentials on usr.UserId equals creden.UserId
                                                                   where creden.Id == objCredentials.Id
                                                                   select new { UserName = usr.FirstName }).FirstOrDefault().UserName;
                    }
                    else if (objCredentials != null && objCredentials.UserType.Equals(Message.UserTypeTrainer))
                    {
                        objChallengedetail.PersonalBestUserName = (from trnr in _dbContext.Trainer
                                                                   join creden in _dbContext.Credentials on trnr.TrainerId equals creden.UserId
                                                                   where creden.Id == objCredentials.Id
                                                                   select new { UserName = trnr.FirstName }).FirstOrDefault().UserName;
                    }
                    return objChallengedetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetChallengeOfTheDayDetails : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get challenge details of challenge of the day
        /// </summary>
        /// <returns>IEnumerable<CODQueue></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/20/2015
        /// </devdoc>
        public static List<CODQueue> GetAllCOD()
        {

            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetAllCOD---- " + DateTime.Now.ToLongDateString());
                    DateTime todayDate = DateTime.Now.Date;
                    List<CODQueue> objCODQueueList = (from cod in _dbContext.ChallengeofTheDayQueue
                                                      join c in _dbContext.Credentials on cod.UserId equals c.UserId
                                                      join u in _dbContext.User on cod.UserId equals u.UserId
                                                      join chlnge in _dbContext.Challenge on cod.ChallengeId equals chlnge.ChallengeId
                                                      where cod.StartDate <= todayDate
                                                      && cod.EndDate >= todayDate
                                                      && c.UserType == Message.UserTypeUser
                                                      && chlnge.IsActive == true
                                                      select new CODQueue
                                                      {
                                                          QueueId = cod.QueueId,
                                                          ChallengeName = chlnge.ChallengeName,
                                                          Featuring = u.FirstName + " " + u.LastName,
                                                          StrengthCount = _dbContext.UserChallenge.Where(s => s.ChallengeId == chlnge.ChallengeId).Select(y => y.UserId).Distinct().Count()
                                                      }).ToList();
                    return objCODQueueList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetAllCOD : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to delete COD
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/21/2015
        /// </devdoc>
        public static void DeleteCOD(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    //Delete COD
                    traceLog.AppendLine("Start:DeleteCOD() with challenge Id-" + id);
                    List<tblChallengeofTheDayQueue> objChallengeofTheDayQueue = _dbContext.ChallengeofTheDayQueue.Where(ce => ce.QueueId == id).ToList();
                    if (objChallengeofTheDayQueue != null)
                    {
                        _dbContext.ChallengeofTheDayQueue.RemoveRange(objChallengeofTheDayQueue);
                        _dbContext.SaveChanges();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("DeleteCOD end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }

        }
    }
}