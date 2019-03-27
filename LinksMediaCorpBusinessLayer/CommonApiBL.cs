using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinksMediaCorpBusinessLayer
{
    public class CommonApiBL
    {
        /// <summary>
        /// Get Challenge Eexecise Set Association List
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblCESAssociation> GetCESAssociationList(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetCESAssociationList challengeId:" + challengeId);
                    List<tblCESAssociation> listtblCESAssociation = null;
                    listtblCESAssociation = (from challaExe in dataContext.CEAssociation
                                             join challaExeset in dataContext.CESAssociations
                                             on challaExe.RocordId equals challaExeset.RecordId
                                             where challaExe.ChallengeId == challengeId
                                             select challaExeset).ToList();
                    if(listtblCESAssociation == null)
                    {
                        listtblCESAssociation = new List<tblCESAssociation>();
                    }
                    return listtblCESAssociation;
                }
                finally
                {
                    traceLog.AppendLine("GetCESAssociationList end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Challenge VariableValue Based On Time
        /// </summary>
        /// <param name="variableValue"></param>
        /// <returns></returns>
        public static string GetChallengeVariableValueBasedOnTime(string variableValue)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetChallengeVariableValueBasedOnTime variableValue:" + variableValue);
                string timeVariableValue = string.Empty;
                if (!string.IsNullOrEmpty(variableValue))
                {
                    string[] variableValueWithMS = variableValue.Split(new char[1] { '.' });
                    if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                    {
                        if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                        {
                            //Code for HH:MM:SS And MM:SS format
                            string tempValue = variableValueWithMS[0];
                            char[] splitChar = { ':' };
                            string[] spliResult = tempValue.Split(splitChar);
                            if (spliResult[0].Equals("00"))
                            {
                                variableValueWithMS[0] = spliResult[1] + ":" + spliResult[2];
                            }
                            else if (spliResult[2].Equals("00"))
                            {
                                variableValueWithMS[0] = spliResult[0] + ":" + spliResult[1];
                            }
                            timeVariableValue = variableValueWithMS[0] + (variableValueWithMS[1].Equals("00") ? ".00" : ("." + variableValueWithMS[1]));
                        }
                    }
                }
                return timeVariableValue;
            }
            finally
            {
                traceLog.AppendLine("GetChallengeVariableValueBasedOnTime end() : --- " + DateTime.Now.ToLongDateString());

            }
        }
        /// <summary>
        /// Get Challenge Variable Value Based Variable Unit
        /// </summary>
        /// <param name="variableValue"></param>
        /// <param name="variableUnit"></param>
        /// <returns></returns>
        public static string GetChallengeVariableValueBasedVariableUnit(string variableValue, string variableUnit)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetChallengeVariableValueBasedOnTime variableValue:" + variableValue);
                string tempdesc = string.Empty;
                if (!string.IsNullOrEmpty(variableValue))
                {
                    if (variableUnit == "minutes" || variableUnit == "seconds")
                    {
                        string[] variableValueWithMS = variableValue.Split(new char[1] { '.' });
                        if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                        {
                            if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                            {
                                //Code for HH:MM:SS And MM:SS format
                                string tempValue = string.Empty;
                                char[] splitChar = { ':' };
                                string[] spliResult = variableValueWithMS[0].Split(splitChar);
                                tempValue = spliResult[0].Equals("00") ? string.Empty : spliResult[0] + " Hour(s)";
                                if (!string.IsNullOrEmpty(tempValue))
                                {
                                    tempValue += spliResult[1].Equals("00") ? string.Empty : ", " + spliResult[1] + " Minute(s)";
                                }
                                else
                                {
                                    tempValue += spliResult[1].Equals("00") ? string.Empty : spliResult[1] + " Minute(s)";
                                }
                                if (!string.IsNullOrEmpty(tempValue))
                                {
                                    tempValue += spliResult[2].Equals("00") ? string.Empty : ", " + spliResult[2] + " Second(s)";
                                }
                                else
                                {
                                    tempValue += spliResult[2].Equals("00") ? string.Empty : spliResult[2] + " Second(s)";
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
                        tempdesc = variableValue;
                    }
                }
                return tempdesc;
            }
            finally
            {
                traceLog.AppendLine("GetChallengeVariableValueBasedOnTime end() : --- " + DateTime.Now.ToLongDateString());

            }
        }

        /// <summary>
        /// Get Challenge Result BasedOn Result Unit
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetChallengeResultBasedOnResultUnit(string result)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetChallengeVariableValueBasedOnTime Result:" + result);
                string ChallengeResult = string.Empty;
                string tempResult = result;
                char[] splitChar = { ':' };
                string[] spliResult = tempResult.Split(splitChar);
                if (spliResult[0].Equals("00"))
                {
                    ChallengeResult = spliResult[1] + ":" + spliResult[2];
                }
                else if (spliResult[2].Equals("00"))
                {
                    ChallengeResult = spliResult[0] + ":" + spliResult[1];
                }
                return ChallengeResult;
            }
            finally
            {
                traceLog.AppendLine("GetChallengeVariableValueBasedOnTime end() : --- " + DateTime.Now.ToLongDateString());

            }
        }
        /// <summary>
        /// Get Challenge First Exercise Association List based on ChallengeId
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static ExeciseVideoDetail GetChallengeFirstExerciseAssociationList(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetChallengeFirstExerciseAssociationList challengeId:" + challengeId);
                  
                    ExeciseVideoDetail objExeciseVideoDetail = (from cexe in dataContext.CEAssociation
                                             join ex in dataContext.Exercise
                                             on cexe.ExerciseId equals ex.ExerciseId
                                             where cexe.ChallengeId == challengeId
                                             orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                             select new ExeciseVideoDetail
                                             {
                                                 ExeciseName = ex.ExerciseName,
                                                 ExeciseUrl = ex.V720pUrl,
                                                 ExerciseThumnail = ex.ThumnailUrl,
                                                 ChallengeExeciseId = cexe.RocordId
                                             }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault();
                    if (objExeciseVideoDetail == null)
                    {
                        objExeciseVideoDetail = new ExeciseVideoDetail();
                    }
                    return objExeciseVideoDetail;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeFirstExerciseAssociationList end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Challenge Of Day List
        /// </summary>
        /// <returns></returns>
        public static List<int> GetCODList(LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                DateTime today = DateTime.Now.Date;
                traceLog.AppendLine("Start: GetexceptListCOD ");
                List<int> challengeId = (from cod in dataContext.ChallengeofTheDayQueue
                                         join c in dataContext.Challenge on cod.ChallengeId equals c.ChallengeId
                                         where c.IsActive && cod.StartDate <= today && cod.EndDate >= today
                                         select c.ChallengeId).ToList();
                return challengeId;
            }
            finally
            {
                traceLog.AppendLine("GetexceptListCOD end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Sponsor COD List
        /// </summary>
        /// <returns></returns>
        public static List<int> GetSponsorCODList(LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                DateTime today = DateTime.Now.Date;
                traceLog.AppendLine("Start: GetSponsorCODList ");
                List<int> exceptListSponsor = (from tc in dataContext.TrainerChallenge
                                               join c in dataContext.Challenge on tc.ChallengeId equals c.ChallengeId
                                               where c.IsActive && tc.StartDate <= today && tc.EndDate >= today
                                               select c.ChallengeId).ToList();
                return exceptListSponsor;
            }
            finally
            {
                traceLog.AppendLine("GetSponsorCODList end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Format Set TimeFormat
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
                            //Code for MM:SS format
                            // string tempValue = string.Empty;
                            char[] splitChar = { ':' };
                            string[] spliResult = variableValueWithMS[0].Split(splitChar);
                            setresultvalue = spliResult.Length > 2 ? spliResult[1] + ConstantHelper.constColon + spliResult[2] : "00:00";
                        }
                        else
                        {
                            setresultvalue = "00:00";
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
        /// Get Challenge Detail
        /// </summary>
        /// <param name="objChallengedetail"></param>
        /// <param name="dataContext"></param>
        /// <param name="challengeId"></param>
        /// <param name="objCred"></param>
        /// <returns></returns>
        public static ChallengeDetailVM ChallengeDetail(ChallengeDetailVM objChallengedetail, LinksMediaContext dataContext, int challengeId, Credentials objCred)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: ChallengeDetail()-challengeId---- " + challengeId);
                var challengeBestresult = TeamBL.GetGlobalPersonalBestResult(objChallengedetail.ChallengeId, objCred.Id, dataContext);
                if (challengeBestresult != null)
                {
                    objChallengedetail.PersonalBestResult = string.IsNullOrEmpty(challengeBestresult.Result) ? string.Empty : challengeBestresult.Result;
                }
                objChallengedetail.LatestResult = TeamBL.GetLatestResult(objChallengedetail.ChallengeId, objCred.Id, dataContext);
                if (objChallengedetail.TempTargetZone != null && objChallengedetail.TempTargetZone.Count > 0)
                {
                    objChallengedetail.TargetZone = string.Join(", ", objChallengedetail.TempTargetZone);
                }
                objChallengedetail.TempTargetZone = null;
                if (objChallengedetail.TempEquipments != null && objChallengedetail.TempEquipments.Count > 0)
                {
                    objChallengedetail.Equipment = string.Join(", ", objChallengedetail.TempEquipments);
                }
                objChallengedetail.TempEquipments = null;
                objChallengedetail.IsWellness = (objChallengedetail.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false;
                objChallengedetail.ChallengeType = string.IsNullOrEmpty(objChallengedetail.ChallengeType) ? string.Empty : objChallengedetail.ChallengeType.Split(' ')[0];
                objChallengedetail.CreatedByTrainerId = objChallengedetail.CreatedByTrainerId ?? 0;
                objChallengedetail.ChallengeDuration = string.IsNullOrEmpty(objChallengedetail.ChallengeDuration) ? string.Empty : objChallengedetail.ChallengeDuration;
                objChallengedetail.ChallengeLink = CommonUtility.VirtualPath + ConstantHelper.constProgramViewChallenge + objChallengedetail.ChallengeId;
                if (!string.IsNullOrEmpty(objChallengedetail.Description))
                {
                    objChallengedetail.Description = CommonUtility.RemoveHtmStyle(objChallengedetail.Description);
                    objChallengedetail.Description = objChallengedetail.Description.Replace("<br />", "||br||");
                    const string HTML_TAG_PATTERN = "<.*?>";
                    objChallengedetail.Description = Regex.Replace(objChallengedetail.Description, HTML_TAG_PATTERN, string.Empty).Replace("&nbsp;", string.Empty);
                    Regex r = new Regex(@"<([^>]*)(\sstyle="".+?""(\s|))(.*?)>");
                    objChallengedetail.Description = r.Replace(objChallengedetail.Description, string.Empty);
                }
                // Replace the challenge Details
                if (!string.IsNullOrEmpty(objChallengedetail.ChallengeDetail))
                {
                    objChallengedetail.ChallengeDetail = CommonUtility.RemoveHtmStyle(objChallengedetail.ChallengeDetail);
                    objChallengedetail.ChallengeDetail = objChallengedetail.ChallengeDetail.Replace("<br />", "||br||");
                    const string HTML_TAG_PATTERN = "<.*?>";
                    objChallengedetail.ChallengeDetail = Regex.Replace(objChallengedetail.ChallengeDetail, HTML_TAG_PATTERN, string.Empty).Replace("&nbsp;", string.Empty);
                }
                //Check challenge to identify that it is created by Admin or Trainer
                //If it is created by trainer then assign trainer profile pic and name
                if (objChallengedetail.CreatedByUserType != null)
                {
                    if (objChallengedetail.CreatedByUserType.Equals(Message.UserTypeTrainer))
                    {
                        var objTrainer = dataContext.Trainer.FirstOrDefault(t => t.TrainerId == objChallengedetail.CreatedByTrainerId);
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
                else
                {
                    objChallengedetail.CreatedByTrainerId = 0;
                }
                if (objChallengedetail.Excercises != null && objChallengedetail.Excercises.Count > 0 && !(objChallengedetail.ChallengeSubTypeId == ConstantHelper.FreeformChallangeId || objChallengedetail.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType || objChallengedetail.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType))
                {
                    objChallengedetail.Excercises = objChallengedetail.Excercises.Take(1).ToList();
                }
                objChallengedetail.Excercises.ForEach(
                           exer =>
                           {
                               exer.WeightForMan = exer.WeightForManDB > 0 ? Convert.ToString(exer.WeightForManDB) + " " + "lbs" : null;
                               exer.WeightForWoman = exer.WeightForWomanDB > 0 ? Convert.ToString(exer.WeightForWomanDB) + " " + "lbs" : null;
                               exer.VedioLink = !string.IsNullOrEmpty(exer.VedioLink) ? exer.VedioLink : string.Empty;
                               if (string.IsNullOrEmpty(exer.ExerciseThumnail))
                               {
                                   if (!string.IsNullOrEmpty(exer.ExerciseName))
                                   {
                                       string thumnailName = exer.ExerciseName.Replace(" ", string.Empty);
                                       exer.ExerciseThumnail = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                   }
                                   else
                                   {
                                       exer.ExerciseThumnail = string.Empty;
                                   }
                               }
                               exer.ChallengeExcerciseDescription = (string.IsNullOrEmpty(exer.ChallengeExcerciseDescription)
                                                                 || exer.ChallengeExcerciseDescription == ConstantHelper.constFFChallangeDescription) ? string.Empty : exer.ChallengeExcerciseDescription;
                               exer.ExerciseName = (exer.IsAlternateExeciseName) ? ((string.IsNullOrEmpty(exer.AlternateExcerciseDescription) ||
                                    exer.AlternateExcerciseDescription == ConstantHelper.constFFChallangeDescription) ? string.Empty : exer.AlternateExcerciseDescription) : exer.ExerciseName;
                               exer.ExeciseSetList.ForEach(exeset =>
                               {
                                   exeset.RestTime = formatSetTimeFormat(exeset.RestTime);
                                   exeset.SetResult = formatSetTimeFormat(exeset.SetResult);
                                   exeset.Description = (string.IsNullOrEmpty(exeset.Description)
                                                      || string.Compare(exer.ChallengeExcerciseDescription, ConstantHelper.constExeciseSetSeperator, StringComparison.OrdinalIgnoreCase) == 0) ? string.Empty : exeset.Description;
                               });
                           });
                if (objChallengedetail.Excercises != null)
                {
                    objChallengedetail.Excercises = objChallengedetail.Excercises.OrderByDescending(cc => cc.IsFirstExecise).ToList();
                }
                //// Modify the varivale in 02 Hour(s), 05 Minute(s), 10 Second(s) format
                string tempdesc = string.Empty;
                if (!string.IsNullOrEmpty(objChallengedetail.VariableValue))
                {
                    if (objChallengedetail.VariableUnit == ConstantHelper.constVariableUnitminutes || objChallengedetail.VariableUnit == ConstantHelper.constVariableUnitseconds)
                    {
                        string[] variableValueWithMS = objChallengedetail.VariableValue.Split(new char[1] { '.' });
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

                //Modify the variable value if it's type is "HH:MM:SS.MS
                if (!string.IsNullOrEmpty(objChallengedetail.VariableValue))
                {
                    string[] variableValueWithMS = objChallengedetail.VariableValue.Split(new char[1] { '.' });
                    if (!string.IsNullOrEmpty(variableValueWithMS[0]))
                    {
                        if (variableValueWithMS[0].Contains(':'))  // If containd colon(:) then true for time type like minutes and seconds
                        {
                            //Code for HH:MM:SS And MM:SS format
                            string tempValue = variableValueWithMS[0];
                            char[] splitChar = { ':' };
                            string[] spliResult = tempValue.Split(splitChar);
                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                variableValueWithMS[0] = spliResult[1] + ConstantHelper.constColon + spliResult[2];
                            }
                            else if (spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                variableValueWithMS[0] = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                            }
                            objChallengedetail.VariableValue = variableValueWithMS[0] + (variableValueWithMS[1].Equals(ConstantHelper.constTimeVariableUnit) ? ".00" : ("." + variableValueWithMS[1]));
                        }
                    }
                }
                // Show the Challenge to friend tag when user complete challenge at least one time.
                if (objCred.UserType.Equals(Message.UserTypeTrainer))
                {
                    objChallengedetail.IsShowChallengeFriendToUser = dataContext.UserChallenge.Where(uc => uc.ChallengeId == challengeId && uc.UserId == objCred.Id).ToList().Count > 0;
                }
                return objChallengedetail;
            }
            finally
            {
                traceLog.AppendLine("End  ChallengeDetail : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Team User MemberList
        /// </summary>
        /// <param name="teamuserCredtialList"></param>
        /// <param name="following"></param>
        /// <param name="dataContext"></param>
        /// <param name="objCredential"></param>
        /// <returns></returns>
        public static List<FollowerFollwingUserVM> TeamUserMemberList(List<int> teamuserCredtialList, List<int> following, LinksMediaContext dataContext, Credentials objCredential)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:TeamUserMemberList() method");
                List<string> userspecization = new List<string>() { ConstantHelper.constFFChallangeDescription };
                List<FollowerFollwingUserVM> objUserList = (from cred in dataContext.Credentials
                                                            join usr in dataContext.User on cred.UserId equals usr.UserId
                                                            where cred.UserType == Message.UserTypeUser && teamuserCredtialList.Contains(cred.Id)
                                                            select new FollowerFollwingUserVM
                                                            {
                                                                CredID = cred.Id,
                                                                ID = cred.UserId,
                                                                FullName = usr.FirstName + " " + usr.LastName,
                                                                ImageUrl = usr.UserImageUrl,
                                                                City = usr.City,
                                                                State = usr.State,
                                                                Specilaization = userspecization.AsEnumerable().ToList(),
                                                                UserType = ConstantKey.UserSearchType,
                                                                TrainerType = ConstantHelper.constFFChallangeDescription,
                                                                IsVerifiedTrainer = 0,
                                                                TeamMemberCount = usr.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == usr.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                                                HashTag = usr.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == usr.TeamId).TeamName : string.Empty,
                                                                Status = cred.Id == objCredential.Id ? JoinStatus.CurrentUser : following.Contains(cred.Id) ? JoinStatus.Follow : JoinStatus.Unfollow,
                                                            }).ToList();
                return objUserList;
            }
            finally
            {
                traceLog.AppendLine("End: TeamUserMemberList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Team Trainer MemberList
        /// </summary>
        /// <param name="teamuserCredtialList"></param>
        /// <param name="following"></param>
        /// <param name="dataContext"></param>
        /// <param name="objCredential"></param>
        /// <returns></returns>
        public static List<FollowerFollwingUserVM> TeamTrainerMemberList(List<int> teamuserCredtialList, List<int> following, LinksMediaContext dataContext, Credentials objCredential)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:TeamTrainerMemberList()");
               // List<string> userspecization = new List<string>() { ConstantHelper.constFFChallangeDescription };
                List<FollowerFollwingUserVM> objTrainerList = (from cred in dataContext.Credentials
                                                               join tr in dataContext.Trainer on cred.UserId equals tr.TrainerId
                                                               where cred.UserType == Message.UserTypeTrainer && teamuserCredtialList.Contains(cred.Id)
                                                               select new FollowerFollwingUserVM
                                                               {
                                                                   CredID = cred.Id,
                                                                   ID = cred.UserId,
                                                                   FullName = tr.FirstName + " " + tr.LastName,
                                                                   ImageUrl = tr.TrainerImageUrl,
                                                                   City = dataContext.Cities.FirstOrDefault(ct => ct.CityId == tr.City).CityName,
                                                                   State = tr.State,
                                                                   Specilaization = (from ts in dataContext.TrainerSpecialization
                                                                                     join s in dataContext.Specialization
                                                                                     on ts.SpecializationId equals s.SpecializationId
                                                                                     where ts.IsInTopThree == 1 && ts.TrainerId == tr.TrainerId
                                                                                     select s.SpecializationName).AsEnumerable().ToList(),
                                                                   UserType = ConstantKey.TrainerSerachType,
                                                                   TrainerType = tr.TrainerType,
                                                                   IsVerifiedTrainer = 1,
                                                                   TeamMemberCount = tr.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == tr.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                                                   HashTag = tr.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == tr.TeamId).TeamName : string.Empty,
                                                                   Status = cred.Id == objCredential.Id ? JoinStatus.CurrentUser : following.Contains(cred.Id) ? JoinStatus.Follow : JoinStatus.Unfollow
                                                               }).ToList();
                return objTrainerList;
            }
            finally
            {
                traceLog.AppendLine("End: TeamTrainerMemberList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get User TeamList
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="userCredId"></param>
        /// <returns></returns>
        public static List<int> GetUserTeamList(LinksMediaContext dataContext, int userCredId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:GetUserTeamList()");
                List<int> teamIds = (from usr in dataContext.User
                                     join crd in dataContext.Credentials
                                     on usr.UserId equals crd.UserId
                                     where crd.Id == userCredId && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                     select usr.TeamId).ToList();
                return teamIds;
            }
            finally
            {
                traceLog.AppendLine("End: GetUserTeamList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Trainer TeamList
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="trainerCredId"></param>
        /// <returns></returns>
        public static List<int> GetTrainerTeamList(LinksMediaContext dataContext, int trainerCredId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:GetTrainerTeamList()");
                List<int> teamIds = (from crd in dataContext.Credentials
                                     join tms in dataContext.TrainerTeamMembers
                                     on crd.Id equals tms.UserId
                                     orderby tms.RecordId ascending
                                     where crd.Id == trainerCredId && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                     select tms.TeamId).ToList();
                return teamIds;
            }
            finally
            {
                traceLog.AppendLine("End: GetTrainerTeamList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get TeamTrending CategoryId List
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public static List<int> GetTeamTrendingCategoryIdList(LinksMediaContext dataContext, List<int> teamIds)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:GetTeamTrendingCategoryIdList()");
                List<int> teamTrendingCategoryList = (from tm in dataContext.Teams
                                                      join tms in dataContext.TeamTrendingAssociations
                                                      on tm.TeamId equals tms.TeamId
                                                      where teamIds.Contains(tm.TeamId)
                                                      select tms.TrendingCategoryId).Distinct().ToList();
                return teamTrendingCategoryList;
            }
            finally
            {
                traceLog.AppendLine("End: GetTeamTrendingCategoryIdList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Challenge Execise with Set Deatils
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        public static List<ExerciseVM> ChallengeExeciseDeatils(LinksMediaContext dataContext, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start:GetTeamTrendingCategoryIdList()-challengeId" + challengeId);
                List<ExerciseVM> execiseList = (from ce in dataContext.CEAssociation
                                                join exer in dataContext.Exercise on ce.ExerciseId equals exer.ExerciseId into execisegroup
                                                from alExe in execisegroup.DefaultIfEmpty()
                                                where ce.ChallengeId == challengeId
                                                orderby ce.RocordId ascending
                                                select new ExerciseVM
                                                {
                                                    ExerciseId = (int?) alExe.ExerciseId ?? ce.ExerciseId,
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
                                                                          Description = (string.IsNullOrEmpty(exsetce.Description) || string.Compare(exsetce.Description, ConstantHelper.constExeciseSetSeperator) == 0) ? string.Empty : exsetce.Description,
                                                                          IsRestType = exsetce.IsRestType,
                                                                          RestTime = string.IsNullOrEmpty(exsetce.RestTime) ? string.Empty : exsetce.RestTime,
                                                                          SetReps = exsetce.SetReps,
                                                                          SetResult = string.IsNullOrEmpty(exsetce.SetResult) ? string.Empty : exsetce.SetResult,
                                                                          IsAutoCountDown = (bool?)exsetce.AutoCountDown??false
                                                                      }).ToList()
                                                }).ToList();
                return execiseList;
            }
            finally
            {
                traceLog.AppendLine("End: GetTeamTrendingCategoryIdList: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }



        /// <summary>
        /// Notification Receiver Result
        /// </summary>
        /// <param name="resultQueryData"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static RecentResultVM NotificationReceiverResult(RecentResultVM resultQueryData, LinksMediaContext dataContext)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: NotificationReceiverResult()");
                string resultMethod = string.Empty;
                if (!string.IsNullOrEmpty(resultQueryData.ResultUnit) && resultQueryData.PostType == ConstantHelper.ResultFeed)
                {
                    resultMethod = resultQueryData.ResultUnit.Trim();
                    // Find all result for latest  result submit  result
                    PersonalChallengeVM personalbestresult = TeamBL.GetGlobalPersonalBestResult(resultQueryData.ChallengeId, resultQueryData.UserCredID, dataContext);
                    switch (resultMethod)
                    {
                        case ConstantHelper.constTime:
                            if (!string.IsNullOrEmpty(resultQueryData.Result))
                            {
                                resultQueryData.Result = resultQueryData.Result.Trim();
                            }
                            resultQueryData.TempOrderIntValue = Convert.ToInt32(resultQueryData.Result.Replace(":", string.Empty).Replace(".", string.Empty));
                            //Code for HH:MM:SS And MM:SS format
                            string tempResult = string.Empty;
                            tempResult = resultQueryData.Result;
                            char[] splitChar = { ':' };
                            string[] spliResult = tempResult.Split(splitChar);
                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                resultQueryData.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                            }
                            else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                            {
                                resultQueryData.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                            }
                            // Store Persoan Best
                            if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                            {
                                resultQueryData.PersonalBestResult = personalbestresult.Result.Trim();
                            }
                            // Check user personal result or not based on challege ID
                            if (resultQueryData.ChallengeSubTypeid == 6)
                            {
                                resultQueryData.IsRecentChallengUserBest = (resultQueryData.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                            }
                            else
                            {
                                resultQueryData.IsRecentChallengUserBest = (resultQueryData.TempOrderIntValue > personalbestresult.TempOrderIntValue) ? false : true;
                            }
                            break;
                        case ConstantHelper.constReps:
                        case ConstantHelper.constWeight:
                        case ConstantHelper.constDistance:
                            resultQueryData.TempOrderIntValue = (string.IsNullOrEmpty(resultQueryData.Result)) ? 0 : (float)Convert.ToDouble(resultQueryData.Result.Replace(",", string.Empty));
                            // Store Persoan Best
                            if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                            {
                                resultQueryData.PersonalBestResult = personalbestresult.Result.Replace(",", string.Empty).Trim();
                            }
                            if (!string.IsNullOrEmpty(resultQueryData.Result))
                            {
                                resultQueryData.Result = resultQueryData.Result.Replace(",", string.Empty).Trim();
                            }
                            resultQueryData.IsRecentChallengUserBest = (resultQueryData.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                            break;
                        case ConstantHelper.conRounds:
                        case ConstantHelper.constInterval:
                            if (!string.IsNullOrEmpty(resultQueryData.Fraction))
                            {
                                string[] arrString = resultQueryData.Fraction.Split(new char[1] { '/' });
                                resultQueryData.TempOrderIntValue = (float)(Convert.ToDecimal(resultQueryData.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                            }
                            else
                            {
                                resultQueryData.TempOrderIntValue = (string.IsNullOrEmpty(resultQueryData.Result)) ? 0 : (float)Convert.ToInt16(resultQueryData.Result);
                            }
                            if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                            {
                                resultQueryData.PersonalBestResult = personalbestresult.Result.Replace(",", string.Empty).Trim();
                            }
                            if (!string.IsNullOrEmpty(resultQueryData.Result))
                            {
                                resultQueryData.Result = resultQueryData.Result.Replace(",", string.Empty).Trim();
                            }
                            resultQueryData.IsRecentChallengUserBest = (resultQueryData.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                            resultQueryData.Result = resultQueryData.Result + " " + resultQueryData.Fraction;
                            break;
                    }
                }
                else
                {
                    resultQueryData.Result = "";
                    resultQueryData.ResultUnit = "";
                }
                return resultQueryData;
            }
            finally
            {
                traceLog.AppendLine("End: NotificationReceiverResult  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}
