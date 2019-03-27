
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
using System.Globalization;
using System.Drawing;
namespace LinksMediaCorpBusinessLayer
{
    public class CommonBL
    {
        public static void DeleteUserAssociatedRecords(LinksMediaContext _dbContext, StringBuilder traceLog, int userCredId)
        {
            try
            {
                traceLog.Append("DeleteUserAssociatedRecords() Start Delete the user associated details");
               
                List<tblUserChallenges> acceptedChallengeList = _dbContext.UserChallenge.Where(ce => ce.UserId == userCredId).ToList();
                /*delete accepted change by the user*/
                if (acceptedChallengeList != null)
                {
                    List<int> resultFeedId = acceptedChallengeList.Select(Id => Id.Id).ToList();
                    var resultList=  _dbContext.ResultBooms.Where(ce => resultFeedId.Contains(ce.ResultId)).ToList();
                    if (resultList != null)
                    {
                        _dbContext.ResultBooms.RemoveRange(resultList);
                    }

                    var resultCommentList = _dbContext.ResultComments.Where(ce => resultFeedId.Contains(ce.Id)).ToList();
                    if (resultCommentList != null)
                    {
                        _dbContext.ResultComments.RemoveRange(resultCommentList);
                    }
                    _dbContext.UserChallenge.RemoveRange(acceptedChallengeList);
                    // _dbContext.SaveChanges();
                }
                /*Delete related message stream from database based on credential Id*/
                List<tblMessageStream> subjectStreamList = _dbContext.MessageStraems.Where(ms => ms.SubjectId == userCredId || ms.TargetId == userCredId).ToList();
                if (subjectStreamList != null)
                {
                    List<int> messageStraemIds = subjectStreamList.Select(Id => Id.MessageStraemId).ToList();
                 
                    var boomList = _dbContext.Booms.Where(ce => messageStraemIds.Contains(ce.MessageStraemId)).ToList();
                    if (boomList != null)
                    {
                        _dbContext.Booms.RemoveRange(boomList);
                    }
                    var commentList = _dbContext.Comments.Where(ce => messageStraemIds.Contains(ce.MessageStraemId)).ToList();
                    if (commentList != null)
                    {
                        _dbContext.Comments.RemoveRange(commentList);
                    }
                    _dbContext.MessageStraems.RemoveRange(subjectStreamList);
                    // _dbContext.SaveChanges();
                }
                /*Delete team member record from database*/
                List<tblTrainerTeamMembers> trainerTeamMemberList = _dbContext.TrainerTeamMembers.Where(ttm => ttm.UserId == userCredId).ToList();
                if (trainerTeamMemberList != null)
                {
                    _dbContext.TrainerTeamMembers.RemoveRange(trainerTeamMemberList);
                    // _dbContext.SaveChanges();
                }
                /*Delete Related Following of this user*/
                List<tblFollowings> usertblFollowingsList = _dbContext.Followings.Where(uf => uf.UserId == userCredId || uf.FollowUserId == userCredId).ToList();
                if (usertblFollowingsList != null)
                {
                    _dbContext.Followings.RemoveRange(usertblFollowingsList);
                    //  _dbContext.SaveChanges();
                }
                /*Delete Related user token of this user*/
                List<tblUserToken> usertokenList = _dbContext.UserToken.Where(uf => uf.UserId == userCredId).ToList();
                if (usertokenList != null)
                {
                    _dbContext.UserToken.RemoveRange(usertokenList);
                    //  _dbContext.SaveChanges();
                }
                /*Delete Related user devices Notification of this user*/
                List<tblUserNotificationSetting> userdevicesNotificationList = _dbContext.UserNotificationSetting.Where(uf => uf.UserCredId == userCredId).ToList();
                if (userdevicesNotificationList != null)
                {
                    _dbContext.UserNotificationSetting.RemoveRange(userdevicesNotificationList);
                    // _dbContext.SaveChanges();
                }

                /*Delete Related user UserNotifications Notification of this user*/
                List<tblUserNotifications> userNotificationList = _dbContext.UserNotifications.Where(uf => uf.SenderCredlID == userCredId || uf.ReceiverCredID == userCredId).ToList();
                if (userNotificationList != null)
                {
                    _dbContext.UserNotifications.RemoveRange(userNotificationList);
                    // _dbContext.SaveChanges();
                }
                tblAppSubscription objtblAppSubscription = _dbContext.AppSubscriptions.Where(uf => uf.UserCredId == userCredId ).FirstOrDefault();
                if (objtblAppSubscription != null)
                {
                    _dbContext.AppSubscriptions.Remove(objtblAppSubscription);
                    // _dbContext.SaveChanges();
                }

                
                _dbContext.SaveChanges();
                traceLog.Append("DeleteUserAssociatedRecords() End Delete the user associated details");
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get Get Following Users List
        /// </summary>
        /// <param name="cred"></param>
        /// <param name="follwingUserIDs"></param>
        /// <param name="following"></param>
        /// <returns></returns>
        /// <dev>Irshad Anasri</dev>
        /// <DateTime>23 Jun 2016</DateTime>
        public static List<FollowerFollwingUserVM> GetFollowingUsersList(Credentials cred, List<int> follwingUserIDs, List<int> following)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFollowingUsersList()");
                    List<FollowerFollwingUserVM> objTeamUser = new List<FollowerFollwingUserVM>();
                    // Get user foolwing list
                    if (follwingUserIDs != null && follwingUserIDs.Count > 0)
                    {
                        objTeamUser = (from t in dataContext.User
                                       join c in dataContext.Credentials on t.UserId equals c.UserId
                                       where c.UserType == Message.UserTypeUser && follwingUserIDs.Contains(c.Id)
                                       select new FollowerFollwingUserVM
                                       {
                                           ID = t.UserId,
                                           UserType = ConstantKey.UserSearchType,
                                           FullName = t.FirstName + " " + t.LastName,
                                           City = t.City,
                                           State = t.State,
                                           ImageUrl = t.UserImageUrl,
                                           Status = cred.Id == c.Id ? JoinStatus.CurrentUser : following.Contains(c.Id) ? JoinStatus.Follow : JoinStatus.Unfollow,
                                           TeamMemberCount = t.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                           HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty
                                       }).ToList();
                        /// Set image url of trainer
                        objTeamUser.ForEach(user =>
                        {
                            user.ImageUrl = (user.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : null;
                        });
                    }

                    return objTeamUser;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFollowingUsersList()--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Following TrainerList
        /// </summary>
        /// <param name="cred"></param>
        /// <param name="follwingtrainetIDs"></param>
        /// <param name="following"></param>
        /// <returns></returns>
        /// <dev>Irshad Anasri</dev>
        /// <DateTime>23 Jun 2016</DateTime>
        public static List<FollowerFollwingUserVM> GetFollowingTrainerList(Credentials cred, List<int> follwingtrainetIDs, List<int> following)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<FollowerFollwingUserVM> objTeamTrainer = new List<FollowerFollwingUserVM>();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFollowingTrainerList()");
                    // Get  trainer foolowing list
                    if (follwingtrainetIDs != null && follwingtrainetIDs.Count > 0)
                    {
                        objTeamTrainer = (from t in dataContext.Trainer
                                          join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                          where c.UserType == Message.UserTypeTrainer && follwingtrainetIDs.Contains(c.Id)
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
                                              City = dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName,
                                              State = t.State,
                                              TeamMemberCount = t.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                              ImageUrl = t.TrainerImageUrl,
                                              IsVerifiedTrainer = 1,
                                              TrainerType = t.TrainerType,
                                              Status = cred.Id == c.Id ? JoinStatus.CurrentUser : following.Contains(c.Id) ? JoinStatus.Follow : JoinStatus.Unfollow,
                                              HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty
                                          }).ToList();
                        /// Set image url of trainer
                        objTeamTrainer.ForEach(trainer =>
                        {
                            trainer.ImageUrl = (trainer.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + trainer.ImageUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl : null;
                        });
                    }

                    return objTeamTrainer;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFollowingTrainerList()--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;

                }
            }
        }
        /// <summary>
        /// Get Following TeamList
        /// </summary>
        /// <param name="cred"></param>
        /// <param name="follwingTeamIDs"></param>
        /// <param name="following"></param>
        /// <returns></returns>
        ///  /// <dev>Irshad Anasri</dev>
        /// <DateTime>23 Jun 2016</DateTime>
        public static List<FollowerFollwingUserVM> GetFollowingTeamList(Credentials cred, List<int> follwingTeamIDs, List<int> following)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<FollowerFollwingUserVM> objTeamlist = new List<FollowerFollwingUserVM>();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFollowingTeamList()");
                    // Get team list
                    if (follwingTeamIDs != null && follwingTeamIDs.Count > 0)
                    {
                        objTeamlist = (from t in dataContext.Teams
                                       join c in dataContext.Credentials on t.TeamId equals c.UserId
                                       where c.UserType == Message.UserTypeTeam && follwingTeamIDs.Contains(c.Id)
                                       select new FollowerFollwingUserVM
                                       {
                                           ID = t.TeamId,
                                           UserType = ConstantKey.TeamSearchType,
                                           FullName = t.TeamName.Substring(1),
                                           City = dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName,
                                           State = t.State,
                                           ImageUrl = t.ProfileImageUrl,
                                           Status = cred.Id == c.Id ? JoinStatus.CurrentUser : following.Contains(c.Id) ? JoinStatus.Follow : JoinStatus.Unfollow,
                                           TeamMemberCount = t.TeamId > 0 ? dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count : 0,
                                           HashTag = t.TeamName
                                       }).ToList();
                        /// Set image url of trainer
                        objTeamlist.ForEach(user =>
                        {
                            user.ImageUrl = (user.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : string.Empty;
                        });
                    }
                    return objTeamlist;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFollowingTeamList()--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get All team trainer Is based on team Id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <dev>Irshad Anasri</dev>
        /// <DateTime>23 Jun 2016</DateTime>
        public static List<int> GetTeamTrainerID(int teamId)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamTrainerID() with TeamID" + teamId);
                    List<int> teamtrainelist = null;
                    if (teamId > 0)
                    {
                        teamtrainelist = (from crd in dataContext.Credentials
                                          join tr in dataContext.Trainer
                                          on crd.UserId equals tr.TrainerId
                                          where tr.TeamId == teamId && crd.UserType == Message.UserTypeTrainer
                                          select crd.Id).ToList();
                    }
                    return teamtrainelist;

                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamTrainerID() --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Id based on user Id and User Type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static int GetTeamIdBasedOnUserCredId(int userId, string userType)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamIdBasedOnUserCredId() with userId-" + userId);
                    if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var teamdetails = dataContext.Trainer.FirstOrDefault(u => u.TrainerId == userId);
                        return teamdetails != null ? teamdetails.TeamId : 0;
                    }
                    else if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var teamdetails = dataContext.User.FirstOrDefault(u => u.UserId == userId);
                        return teamdetails != null ? teamdetails.TeamId : 0;
                    }
                    return 0;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamIdBasedOnUserCredId() --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Save AutoSave ExeciseSet details in database
        /// </summary>
        /// <param name="ExeciseRecordId"></param>
        /// <param name="execiseSetdata"></param>
        /// <returns></returns>
        public static bool SaveExeciseSet(int ExeciseRecordId, string execisesetdetails, int credentialId)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SaveExeciseSet() with ExeciseRecordId-" + ExeciseRecordId);
                    if (ExeciseRecordId > 0)
                    {
                        if (!string.IsNullOrEmpty(execisesetdetails))
                        {
                            string[] stringSeparators = new string[] { "<>" };
                            string[] selecetedExeecisesetList = execisesetdetails.Split(stringSeparators, StringSplitOptions.None);
                            if (selecetedExeecisesetList != null && selecetedExeecisesetList.Count() > 0)
                            {
                                for (int i = 0; i < selecetedExeecisesetList.Count(); i++)
                                {
                                    if (!string.IsNullOrEmpty(selecetedExeecisesetList[i]))
                                    {
                                        string[] selecetedExeeciseList = selecetedExeecisesetList[i].Split(new char[1] { '^' });
                                        int reps = 0;
                                        if (!int.TryParse(selecetedExeeciseList[0], out reps))
                                        {
                                            reps = 0;
                                        }
                                        string resulttime = selecetedExeeciseList[1];
                                        string resttime = selecetedExeeciseList[2];
                                        string description = selecetedExeeciseList[3];
                                        string autocountdown = selecetedExeeciseList[4];
                                        bool autocountdownflag = false;
                                        if (!string.IsNullOrEmpty(autocountdown))
                                        {
                                            autocountdownflag = autocountdown.Equals("Yes", StringComparison.OrdinalIgnoreCase);
                                        }
                                        resulttime = (string.IsNullOrEmpty(resulttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resulttime;
                                        resttime = (string.IsNullOrEmpty(resttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resttime;
                                        description = (string.IsNullOrEmpty(description) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : description;
                                        if (reps > 0 || !string.IsNullOrEmpty(resulttime) || !string.IsNullOrEmpty(resttime) || !string.IsNullOrEmpty(description))
                                        {
                                            tblCESAssociation objtblCESAssociation = new tblCESAssociation();
                                            objtblCESAssociation.SetReps = reps;
                                            objtblCESAssociation.RestTime = resttime;
                                            objtblCESAssociation.SetResult = resulttime;
                                            objtblCESAssociation.Description = description;
                                            objtblCESAssociation.CreatedBy = credentialId;
                                            objtblCESAssociation.CreatedDate = DateTime.Now;
                                            objtblCESAssociation.ModifiedDate = DateTime.Now;
                                            objtblCESAssociation.ModifiedBy = credentialId;
                                            objtblCESAssociation.RecordId = ExeciseRecordId;
                                            objtblCESAssociation.IsRestType = reps > 0 ? true : false;
                                            objtblCESAssociation.AutoCountDown = autocountdownflag;
                                            dataContext.CESAssociations.Add(objtblCESAssociation);
                                            dataContext.SaveChanges();
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        return true;
                    }
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamIdBasedOnUserCredId() --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return false;
        }
        /// <summary>
        /// AutoSave New Added Execise
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="execisedetails"></param>
        /// <param name="credentialId"></param>
        /// <returns></returns>
        public static bool AutoSaveExecise(int challengeId, string execisedetails, int credentialId)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: AutoSaveExecise() with challengeId-" + challengeId);
                    if (challengeId > 0)
                    {
                        /*primary FreeFormExerciseNameDescriptionList*/
                        if (!string.IsNullOrEmpty(execisedetails))
                        {
                            string[] selecetedExeecisesList = execisedetails.Split(new char[1] { '|' });
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
                                        string execiseIds = execiseCount > 5 ? selecetedExeeciseList[5] : "0";
                                        string selectedEquipemet = execiseCount > 6 ? selecetedExeeciseList[6] : string.Empty;
                                        string selectedTrainingZone = execiseCount > 7 ? selecetedExeeciseList[7] : string.Empty;
                                        string selectedExeciseType = execiseCount > 8 ? selecetedExeeciseList[8] : string.Empty;
                                        // string selectedCEARecordId = execiseCount > 9 ? selecetedExeeciseList[9] : "0";
                                        string IsCheckedFirstExecise = execiseCount > 10 ? selecetedExeeciseList[10] : "false";
                                        bool isAlternate = string.IsNullOrEmpty(isAlternateEName) == true ? false : bool.Parse(isAlternateEName);
                                        if ((!string.IsNullOrEmpty(execiseName) && execiseName != ConstantHelper.constFFChallangeDescription) || isAlternate)
                                        {
                                            if (!string.IsNullOrEmpty(description) && description == ConstantHelper.constFFChallangeDescription)
                                            {
                                                description = string.Empty;
                                            }
                                            int exerciseId = 0;
                                            if (!string.IsNullOrEmpty(execiseIds) && execiseIds == ConstantHelper.constFFChallangeDescription)
                                            {
                                                exerciseId = 0;
                                            }
                                            else if (!string.IsNullOrEmpty(execiseIds) && execiseIds != ConstantHelper.constFFChallangeDescription)
                                            {
                                                exerciseId = int.Parse(execiseIds);
                                            }
                                            string allindex = string.Empty;
                                            if (!string.IsNullOrEmpty(selectedEquipemet))
                                            {
                                                allindex += selectedEquipemet + " , ";
                                            }
                                            if (!string.IsNullOrEmpty(selectedTrainingZone))
                                            {
                                                allindex += selectedTrainingZone + " , ";
                                            }
                                            if (!string.IsNullOrEmpty(selectedExeciseType))
                                            {
                                                allindex += selectedExeciseType;
                                            }
                                            if (isAlternate && !string.IsNullOrEmpty(alternateEName))
                                            {
                                                // Add the aletrnate execise with inactive status
                                                if (!(exerciseId > 0) && !dataContext.Exercise.Any(ex => ex.ExerciseName == alternateEName))
                                                {
                                                    execiseName = alternateEName;
                                                }
                                                else if (exerciseId > 0 && dataContext.Exercise.Any(ex => ex.ExerciseId == exerciseId)) //Update the Execise master table
                                                {

                                                    execiseName = alternateEName;
                                                }
                                            }
                                            int exeId = -1;
                                            var execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == execiseName);
                                            if (execiseDetails != null)
                                            {
                                                exeId = execiseDetails.ExerciseId;
                                            }
                                            tblCEAssociation objECAssociation = new tblCEAssociation();
                                            objECAssociation.ChallengeId = challengeId;
                                            objECAssociation.ExerciseId = exeId;
                                            objECAssociation.Description = selecetedExeeciseList[1];
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
                                            objECAssociation.IsShownFirstExecise = string.IsNullOrEmpty(isAlternateEName) ? false : bool.Parse(IsCheckedFirstExecise);
                                            //  objtblCEAssociationList.Add(objECAssociation);
                                            dataContext.CEAssociation.Add(objECAssociation);
                                            dataContext.SaveChanges();
                                            SaveExeciseSetData(dataContext, description, objECAssociation.RocordId, objECAssociation.CreatedBy);
                                        }
                                    }
                                }
                            }
                        }
                        dataContext.SaveChanges();
                        return true;
                    }
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamIdBasedOnUserCredId() --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
            return false;
        }
        /// <summary>
        /// Add Execise set of New Added Execise
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="execisesetdetails"></param>
        /// <param name="challengeExeciseId"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        private static bool SaveExeciseSetData(LinksMediaContext dataContext, string execisesetdetails, int challengeExeciseId, int createdBy)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SaveExeciseSetData() with execisesetdetails-" + execisesetdetails);
                if (!string.IsNullOrEmpty(execisesetdetails))
                {
                    List<tblCESAssociation> listtblCESAssociation = new List<tblCESAssociation>();
                    string[] stringSeparators = new string[] { "<>" };
                    string[] selecetedExeecisesetList = execisesetdetails.Split(stringSeparators, StringSplitOptions.None);
                    if (selecetedExeecisesetList != null && selecetedExeecisesetList.Count() > 0)
                    {
                        for (int i = 0; i < selecetedExeecisesetList.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(selecetedExeecisesetList[i]))
                            {
                                string[] selecetedExeeciseList = selecetedExeecisesetList[i].Split(new char[1] { '^' });
                                int reps = 0;
                                if (!int.TryParse(selecetedExeeciseList[0], out reps))
                                {
                                    reps = 0;
                                }
                                string resulttime = selecetedExeeciseList[1];
                                string resttime = selecetedExeeciseList[2];
                                string description = selecetedExeeciseList[3];
                                string autocountdown = selecetedExeeciseList[4];
                                bool autocountdownflag = false;
                                if (!string.IsNullOrEmpty(autocountdown))
                                {
                                    autocountdownflag = autocountdown.Equals("Yes", StringComparison.OrdinalIgnoreCase);
                                }
                                resulttime = (string.IsNullOrEmpty(resulttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resulttime;
                                resttime = (string.IsNullOrEmpty(resttime) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : resttime;
                                description = (string.IsNullOrEmpty(description) && description == ConstantHelper.constExeciseSetSeperator) ? string.Empty : description;
                                if (reps > 0 || !string.IsNullOrEmpty(resulttime) || !string.IsNullOrEmpty(resttime) || !string.IsNullOrEmpty(description))
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
                        dataContext.CESAssociations.AddRange(listtblCESAssociation);
                    }
                    return true;
                }
                return false;
            }
            finally
            {
                traceLog.AppendLine("End: SaveExeciseSetData() --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Auto save all execise with sets in database
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="firstChallengeExecise"></param>
        /// <param name="execisedetails"></param>
        /// <param name="credentialId"></param>
        public static bool AutoSaveAllExeciseWithSets(int challengeId, string execisedetails, int credentialId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: AutoSaveAllExeciseWithSets for updaing challenge execise ");
                        List<tblCEAssociation> objCEAssociationList = dataContext.CEAssociation.Where(ce => ce.ChallengeId == challengeId).ToList();
                        if (objCEAssociationList != null)
                        {
                            dataContext.CEAssociation.RemoveRange(objCEAssociationList);
                            List<tblCESAssociation> objCESAssociationList = (from challaExe in dataContext.CEAssociation
                                                                             join challaExeset in dataContext.CESAssociations
                                                                             on challaExe.RocordId equals challaExeset.RecordId
                                                                             where challaExe.ChallengeId == challengeId
                                                                             select challaExeset).ToList();
                            if (objCESAssociationList != null)
                            {
                                dataContext.CESAssociations.RemoveRange(objCESAssociationList);
                            }
                            dataContext.SaveChanges();
                        }
                        List<tblCEAssociation> objtblCEAssociationList = new List<tblCEAssociation>();
                        /*primary FreeFormExerciseNameDescriptionList*/
                        if (!string.IsNullOrEmpty(execisedetails))
                        {
                            string[] selecetedExeecisesList = execisedetails.Split(new char[1] { '|' });
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
                                        string execiseIds = execiseCount > 5 ? selecetedExeeciseList[5] : "0";
                                        string selectedEquipemet = execiseCount > 6 ? selecetedExeeciseList[6] : string.Empty;
                                        string selectedTrainingZone = execiseCount > 7 ? selecetedExeeciseList[7] : string.Empty;
                                        string selectedExeciseType = execiseCount > 8 ? selecetedExeeciseList[8] : string.Empty;
                                        string IsCheckedFirstExecise = execiseCount > 10 ? selecetedExeeciseList[10] : "false";
                                        bool isAlternate = string.IsNullOrEmpty(isAlternateEName) == true ? false : bool.Parse(isAlternateEName);
                                        if ((!string.IsNullOrEmpty(execiseName) && execiseName != ConstantHelper.constFFChallangeDescription) || isAlternate)
                                        {
                                            if (!string.IsNullOrEmpty(description) && description == ConstantHelper.constFFChallangeDescription)
                                            {
                                                description = string.Empty;
                                            }
                                            int exerciseId = 0;
                                            if (!string.IsNullOrEmpty(execiseIds) && execiseIds == ConstantHelper.constFFChallangeDescription)
                                            {
                                                exerciseId = 0;
                                            }
                                            else if (!string.IsNullOrEmpty(execiseIds) && execiseIds != ConstantHelper.constFFChallangeDescription)
                                            {
                                                exerciseId = int.Parse(execiseIds);
                                            }
                                            if (isAlternate && !string.IsNullOrEmpty(alternateEName))
                                            {
                                                // Add the aletrnate execise with inactive status
                                                if (!(exerciseId > 0) && !dataContext.Exercise.Any(ex => ex.ExerciseName == alternateEName))
                                                {

                                                    execiseName = alternateEName;
                                                }
                                                else if (exerciseId > 0 && dataContext.Exercise.Any(ex => ex.ExerciseId == exerciseId)) //Update the Execise master table
                                                {
                                                    execiseName = alternateEName;
                                                }
                                            }
                                            int exeId = -1;
                                            var execiseDetails = dataContext.Exercise.FirstOrDefault(c => c.ExerciseName == execiseName);
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
                                            objECAssociation.IsShownFirstExecise = string.IsNullOrEmpty(IsCheckedFirstExecise) ? false : bool.Parse(IsCheckedFirstExecise);
                                            objtblCEAssociationList.Add(objECAssociation);
                                            dataContext.CEAssociation.Add(objECAssociation);
                                            dataContext.SaveChanges();
                                            List<tblCESAssociation> savedSetdata = FreeFormChallengeBL.SaveExeciseSetData(description, objECAssociation.RocordId, objECAssociation.CreatedBy);
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
                        dbTran.Commit();
                        return true;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("AutoSaveAllExeciseWithSets  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }

            }
        }
        /// <summary>
        /// Get All Filter Team SeachChallenge based on search Item ,challengeType and search difficulty level
        /// </summary>
        /// <param name="serachItem"></param>
        /// <param name="challengeType"></param>
        /// <param name="seacheddifficultyLevelType"></param>
        /// <returns></returns>
        public static List<TeamSearchResponse> GetAllFilterTeamSeachChallenge(string serachItem, string challengeType, int seacheddifficultyLevelType)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllFilterTeamSeachChallenge for retrieving challenges from database ");
                    List<TeamSearchResponse> challengelist = new List<TeamSearchResponse>();
                    if (!string.IsNullOrEmpty(challengeType))
                    {
                        List<int> challengeSubTypeIdList = new List<int>();
                        if (string.Compare(challengeType, ConstantHelper.constProgramChallenge, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            challengeSubTypeIdList.Add(ConstantHelper.constProgramChallengeSubType);
                        }
                        else if (string.Compare(challengeType, ConstantHelper.constFitnessTestName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            challengeSubTypeIdList.Add(ConstantHelper.constPowerChallengeType1);
                            challengeSubTypeIdList.Add(ConstantHelper.constPowerChallengeType2);
                            challengeSubTypeIdList.Add(ConstantHelper.constEnduranceChallengeType1);
                            challengeSubTypeIdList.Add(ConstantHelper.constEnduranceChallengeType2);
                            challengeSubTypeIdList.Add(ConstantHelper.constStrengthChallengeType1);
                            challengeSubTypeIdList.Add(ConstantHelper.constStrengthChallengeType2);
                            challengeSubTypeIdList.Add(ConstantHelper.constCardioChallengeType1);
                            challengeSubTypeIdList.Add(ConstantHelper.constCardioChallengeType2);
                            challengeSubTypeIdList.Add(ConstantHelper.constCardioChallengeType3);
                            challengeSubTypeIdList.Add(ConstantHelper.constCardioChallengeType4);
                        }
                        challengelist = (from chl in dataContext.Challenge
                                         where chl.IsActive == true && challengeSubTypeIdList.Contains(chl.ChallengeSubTypeId)
                                         orderby chl.ChallengeName
                                         select new TeamSearchResponse
                                         {
                                             ChallengeId = chl.ChallengeId,
                                             ChallengeName = chl.ChallengeName,
                                             ChallengeUrl = chl.ChallengeName,
                                             DifficulyLevel = chl.DifficultyLevel,
                                             ChallengeSubTypeId = chl.ChallengeSubTypeId
                                         }).ToList();

                        if (challengelist != null && challengelist.Count() > 0)
                        {
                            if (!string.IsNullOrEmpty(serachItem))
                            {
                                serachItem = serachItem.ToUpper(CultureInfo.InvariantCulture);
                                challengelist = challengelist.Where(ch => ch.ChallengeName.ToUpper(CultureInfo.InvariantCulture).Contains(serachItem)).ToList();
                            }
                            if (seacheddifficultyLevelType > 0 && string.Compare(challengeType, ConstantHelper.constProgramChallenge, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                List<string> difficultLevelList = new List<string>();
                                if (seacheddifficultyLevelType == ConstantHelper.constTeamSearchBeginnerLevel)
                                {
                                    difficultLevelList.Add(FitnessLevel.Beginner.ToString());
                                }
                                else if (seacheddifficultyLevelType == ConstantHelper.constTeamSearchIntAdvLevel)
                                {
                                    difficultLevelList.Add(FitnessLevel.Intermediate.ToString());
                                    difficultLevelList.Add(FitnessLevel.Advanced.ToString());
                                }
                                challengelist = challengelist.Where(ch => difficultLevelList.Contains(ch.DifficulyLevel)).ToList();
                            }
                            if (challengelist != null)
                            {
                                challengelist.ForEach(p =>
                                {
                                    p.ChallengeUrl = (p.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType) ? CommonUtility.VirtualPath + "Program/ViewProgram?id=" + p.ChallengeId : CommonUtility.VirtualPath + "Program/ViewChallenge?id=" + p.ChallengeId;
                                });
                            }
                        }
                    }
                    return challengelist;
                }
                finally
                {
                    traceLog.AppendLine("GetChallengeVal  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        ///  Get Program Image Demension based Url
        /// </summary>
        /// <param name="programImageUrl"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void GetProgramDemension(string programImageUrl, out string height, out string width)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("start GetProgramDemension for ProgramImageUrl" + programImageUrl);
                if (!string.IsNullOrEmpty(programImageUrl))
                {
                    string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + programImageUrl;                   
                    if (System.IO.File.Exists(filePath))
                    {
                        using (Bitmap objBitmap = new Bitmap(filePath))
                        {
                            double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                            double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                            height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                            width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                        }
                    }
                    else
                    {
                        height = string.Empty;
                        width = string.Empty;
                    }
                }
                else
                {
                    height = string.Empty;
                    width = string.Empty;
                }
            }
            catch
            {
                height = string.Empty;
                width = string.Empty;
            }
            finally
            {
                traceLog.AppendLine("GetProgramDemension  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
    }

}