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
    public class SponsorChallengeBL
    {
        /// <summary>
        /// Function to get challenge details of sponsor challenge
        /// </summary>
        /// <returns>SponsorChallengeDetailVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/31/2015
        /// </devdoc>
        public static SponsorChallengeDetailVM GetSponsorChallengeDetails(int challengeId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetSponsorChallengeDetails---- " + DateTime.Now.ToLongDateString());
                    SponsorChallengeDetailVM objChallengedetail = (from c in _dbContext.Challenge
                                                                   join cred in _dbContext.Credentials on c.CreatedBy equals cred.Id
                                                                   join ct in _dbContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                   join tc in _dbContext.TrainerChallenge on c.ChallengeId equals tc.ChallengeId
                                                                   join uc in _dbContext.UserChallenge on tc.ResultId equals uc.Id
                                                                   join h in _dbContext.HypeVideos on tc.HypeVideoId equals h.RecordId
                                                                   where c.ChallengeId == challengeId && c.IsActive == true
                                                                   select new SponsorChallengeDetailVM
                                                                   {
                                                                       ChallengeId = c.ChallengeId,
                                                                       ChallengeName = c.ChallengeName,
                                                                       DifficultyLevel = c.DifficultyLevel,
                                                                       ChallengeType = ct.ChallengeType,
                                                                       Strenght = _dbContext.UserChallenge.Where(uchlng => uchlng.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                       ResultUnit = ct.ResultUnit,
                                                                       ResultUnitSuffix = uc.ResultUnit,
                                                                       ChallengeDescription = ct.ChallengeSubType,
                                                                       Excercises = (from exer in _dbContext.Exercise
                                                                                     join ce in _dbContext.CEAssociation on exer.ExerciseId equals ce.ExerciseId
                                                                                     where ce.ChallengeId == c.ChallengeId
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
                                                                       Result = (uc.Result == null ? "" : uc.Result) + " " + (uc.Fraction == null ? "" : uc.Fraction),
                                                                       VariableValue = c.VariableValue,
                                                                       ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                       VariableUnit = ct.Unit,
                                                                       CreatedByUserType = cred.UserType,
                                                                       CreatedByTrainerId = cred.UserId,
                                                                       PersonalBestUserId = uc.UserId
                                                                   }).FirstOrDefault();
                    if (objChallengedetail != null)
                    {
                        if (objChallengedetail.ResultUnit.Equals("Time"))
                        {
                            objChallengedetail.Result = CommonApiBL.GetChallengeResultBasedOnResultUnit(objChallengedetail.Result);
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
                                objChallengedetail.CreatedByProfilePic = string.IsNullOrEmpty(objTrainer.TrainerImageUrl) ? string.Empty :
                                    CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainer.TrainerImageUrl;
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
                           exer.VedioLink = CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + exer.VedioLink;
                           exer.ExerciseThumnail = CommonWebApiBL.GetSaveExerciseThumbnail(exer.VedioLink, exer.ExerciseName);
                       });
                    string tempdesc = string.Empty;
                    if (objChallengedetail != null && !string.IsNullOrEmpty(objChallengedetail.VariableValue))
                    {
                        tempdesc = CommonApiBL.GetChallengeVariableValueBasedVariableUnit(objChallengedetail.VariableValue, objChallengedetail.VariableUnit);
                    }
                    ////End the modification
                    if (!string.IsNullOrEmpty(tempdesc))
                    {
                        tempdesc = tempdesc.Trim();
                    }
                    objChallengedetail.ChallengeDescription = objChallengedetail.ChallengeDescription.Replace("____", tempdesc)
                        .Replace(ConstantHelper.constAmoutOfTime, ConstantHelper.constQustionMark).
                        Replace(ConstantHelper.constAmoutOfTimeSecond, ConstantHelper.constQustionMark);
                    if (objChallengedetail != null && !string.IsNullOrEmpty(objChallengedetail.VariableValue))
                    {
                        objChallengedetail.VariableValue = CommonApiBL.GetChallengeVariableValueBasedOnTime(objChallengedetail.VariableValue);
                    }
                    tblCredentials objCredentials = _dbContext.Credentials.FirstOrDefault(c => c.Id == objChallengedetail.PersonalBestUserId);
                    if (objCredentials != null)
                    {
                        objChallengedetail.PersonalBestUserName = CommonReportingUtility.GetUserFullNameBasedUserCredIdAndUserType(_dbContext, objCredentials.Id, objCredentials.UserType);
                    }
                    return objChallengedetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetSponsorChallengeDetails : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get challenge details of sponsor challenge
        /// </summary>
        /// <returns>IEnumerable<SponsorChallengeQueue></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/20/2015
        /// </devdoc>
        public static List<SponsorChallengeQueue> GetAllSponsorChallenge()
        {

            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetAllSponsorChallenge---- " + DateTime.Now.ToLongDateString());
                    DateTime todayDate = DateTime.Now.Date;
                    var objSponsorChallengeQueueList = (from tc in _dbContext.TrainerChallenge
                                                        join chlnge in _dbContext.Challenge on tc.ChallengeId equals chlnge.ChallengeId
                                                        join t in _dbContext.Trainer on tc.TrainerId equals t.TrainerId
                                                        join c in _dbContext.Credentials on tc.TrainerId equals c.UserId
                                                        join uc in _dbContext.UserChallenge on tc.ResultId equals uc.Id
                                                        where c.UserType == Message.UserTypeTrainer
                                                        && tc.StartDate <= todayDate
                                                        && tc.EndDate >= todayDate
                                                        && chlnge.IsActive == true
                                                        select new SponsorChallengeQueue
                                                        {
                                                            QueueId = tc.QueueId,
                                                            ChallengeName = chlnge.ChallengeName,
                                                            TrainerName = t.FirstName + " " + t.LastName,
                                                            SponsorName = tc.SponsorName,
                                                            StrengthCount = _dbContext.UserChallenge.Where(s => s.ChallengeId == chlnge.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                            AcceptedDate = tc.EndDate
                                                        }).ToList();

                    return objSponsorChallengeQueueList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetAllSponsorChallenge : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to delete sponsor challenge
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh
        /// Date - 04/21/2015
        /// </devdoc>
        public static void DeleteSponsorChallenge(int Id)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: DeleteSponsorChallenge---- " + DateTime.Now.ToLongDateString());
                    //Delete sponser CHallenge
                    List<tblSponsorChallengeQueue> objTrainerChallenge = _dbContext.TrainerChallenge.Where(ce => ce.QueueId == Id).ToList();
                    if (objTrainerChallenge != null)
                    {
                        _dbContext.TrainerChallenge.RemoveRange(objTrainerChallenge);
                    }
                    _dbContext.SaveChanges();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("DeleteSponsorChallenge  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}