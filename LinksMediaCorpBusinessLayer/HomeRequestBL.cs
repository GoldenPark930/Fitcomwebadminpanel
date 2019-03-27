namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpUtility.Resources;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using Newtonsoft.Json;
    public class HomeRequestBL
    {
        /// <summary>
        /// Funtion to get Home Page Data
        /// </summary>
        /// <returns>HomeData</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>        
        public static HomeDataUpdated<List<ViewPostVM>> GetHomeRequestDataUpdated(string strUserToken)
        {

            StringBuilder traceLog = null;
            Credentials objCred = null;
            HomeDataUpdated<List<ViewPostVM>> objresult = new HomeDataUpdated<List<ViewPostVM>>();
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetHomeRequestDataUpdated----" + DateTime.Now);
                objCred = CommonWebApiBL.GetUserId(strUserToken);
                int userId = objCred.UserId;
                ProfileDetails profileDetails = ProfileApiBL.GetHomeProfileDetails(userId, objCred.UserType);
                bool isTeamJoined = (profileDetails != null && profileDetails.TeamId > 0 && profileDetails.IsDefaultTeam != true) ? true : false;
                NotificationSettingVM UserNotificationInfo = ProfileApiBL.GetUserNotificationSetting(objCred.Id, objCred.DeviceID, objCred.DeviceType);
                if (profileDetails != null && UserNotificationInfo != null)
                {
                    profileDetails.IsFriend_challenge_notification = UserNotificationInfo.FriendChallegeNotify;
                    profileDetails.IsJoin_team_notification = UserNotificationInfo.TrainerJoinTeamNotify;
                    profileDetails.IsTrainer_challenge_notification = UserNotificationInfo.TrainerChallegeNotify;
                    if (profileDetails.UserPersoanlTrainerCredId > 0)
                    {
                        UserPersoanlDetailsVM persaonatrainerdetails = ProfileApiBL.GetUserPersonalTrainerDetails(profileDetails.UserPersoanlTrainerCredId);
                        if (persaonatrainerdetails != null)
                        {
                            objresult.UserPersoanlDetail = persaonatrainerdetails;
                        }
                    }

                }
                if (profileDetails != null)
                {
                    profileDetails.ProfileImageUrl = (profileDetails.ProfileImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                        Message.ProfilePicDirectory + profileDetails.ProfileImageUrl))) ? CommonUtility.VirtualPath +
                        Message.ProfilePicDirectory + profileDetails.ProfileImageUrl : string.Empty;
                    profileDetails.TeamPremiumpicUrl = (profileDetails.TeamPremiumpicUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                        Message.ProfilePicDirectory + profileDetails.TeamPremiumpicUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + profileDetails.TeamPremiumpicUrl : string.Empty;
                    profileDetails.TeamProfileUrl = (!string.IsNullOrEmpty(profileDetails.TeamProfileUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                        Message.ProfilePicDirectory + profileDetails.TeamProfileUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + profileDetails.TeamProfileUrl : string.Empty;
                    profileDetails.PendingChallengeCount = PushNotificationBL.GetMyFriendChallengeCount(objCred.Id);
                    profileDetails.CompletedChallengeCount = ChallengeApiBL.GetTotalCompletedChallenegList(objCred.Id);
                    profileDetails.NotificationCount = NotificationApiBL.GetTotalActiveNotification(objCred.Id);
                    profileDetails.ProgramActiveCount = ChallengeApiBL.GetUserActiveProgramCount(objCred.Id);
                    if (profileDetails.IsPersonalTrainer)
                    {
                        profileDetails.OffLineChatCount = ChallengeApiBL.GetUserOfflineChatCount(profileDetails.UserPersoanlTrainerCredId, objCred.Id);
                    }
                }
                else
                {
                    profileDetails.ProfileImageUrl = string.Empty;
                    profileDetails.TeamProfileUrl = string.Empty;
                }
                objresult.UserDetail = profileDetails;
                objresult.IsJoinedTeam = isTeamJoined;
                objresult.UserCredId = objCred.Id;
                objresult.MyAssignmentCount = UseresBL.GetUserMyAssignmentCount(objCred.UserType, objCred.Id);
                objresult.PremimumTypeList = ChallengesCommonBL.GetPremiumChallengeCategoryList();
                return objresult;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End GetHomeRequestDataUpdated : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;

            }
        }
        /// <summary>
        /// Function to get Trainer Profile Data
        /// </summary>
        /// <returns>TrainerViewVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>
        /// 
        public static TrainerViewVM GetTrainerViewData(int trainerId, long notificationID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTrainerViewData");
                    TrainerViewVM objTrainerView = new TrainerViewVM();
                    Credentials userCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblCredentials tranerCredential = _dbContext.Credentials.FirstOrDefault(c => c.UserId == trainerId && c.UserType == Message.UserTypeTrainer);
                    int teamId = 0;
                    var objTrainerInfo = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == trainerId);
                    if (objTrainerInfo != null)
                    {
                        teamId = objTrainerInfo.TeamId;
                    }
                    objTrainerView.TrainerId = objTrainerInfo.TrainerId;
                    objTrainerView.TrainerName = objTrainerInfo.FirstName + " " + objTrainerInfo.LastName;
                    objTrainerView.FirstName = objTrainerInfo.FirstName;
                    objTrainerView.LastName = objTrainerInfo.LastName;
                    objTrainerView.EmailId = objTrainerInfo.EmailId;
                    objTrainerView.HashTag = objTrainerInfo.TeamId > 0 ? _dbContext.Teams.FirstOrDefault(tt => tt.TeamId == objTrainerInfo.TeamId).TeamName : string.Empty;
                    objTrainerView.IsVerifiedTrainer = 1;
                    objTrainerView.TrainerType = objTrainerInfo.TrainerType;
                    objTrainerView.TrainerImageURL = string.IsNullOrEmpty(objTrainerInfo.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                        Message.ProfilePicDirectory + objTrainerInfo.TrainerImageUrl;
                    objTrainerView.TeamMemberCount = teamId > 0 ? _dbContext.TrainerTeamMembers.Where(t => t.TeamId == teamId).GroupBy(g => g.UserId).ToList().Count : 0;
                    objTrainerView.FollowingCount = _dbContext.Followings.Count(f => f.UserId == tranerCredential.Id);
                    objTrainerView.FollowerCount = _dbContext.Followings.Count(f => f.FollowUserId == tranerCredential.Id);
                    Bio objBio = new Bio();
                    DateTime today = DateTime.Today;
                    int age = 0;
                    if (objTrainerInfo.DateOfBirth != null)
                    {
                        age = today.Year - ((DateTime)(objTrainerInfo.DateOfBirth)).Year;
                    }
                    objBio.Age = Convert.ToString(age);
                    objBio.Gender = objTrainerInfo.Gender;
                    objBio.Height = objTrainerInfo.Height;
                    objBio.Weight = objTrainerInfo.Weight;
                    objBio.AboutME = objTrainerInfo.AboutMe;
                    objBio.IsVerifiedTrainer = 1;
                    objTrainerView.TrainerType = objTrainerInfo.TrainerType;
                    objBio.TrainerId = objTrainerInfo.TrainerId;
                    objBio.TrainerImageURL = objTrainerInfo.TrainerImageUrl;
                    objBio.SpecializationList = (from s in _dbContext.Specialization
                                                 join t in _dbContext.TrainerSpecialization on s.SpecializationId equals t.SpecializationId
                                                 where t.TrainerId == trainerId
                                                 select s.SpecializationName).ToList<string>();
                    objTrainerView.BioData = objBio;
                    objTrainerView.IsJoinTeam = _dbContext.User.Any(u => u.UserId == userCredential.UserId && u.TeamId != 0);
                    objTrainerView.IsFollowByLoginUser = _dbContext.Followings.Any(f => f.UserId == userCredential.Id && f.FollowUserId == tranerCredential.Id);
                    NotificationApiBL.UpdateNotificationReadStatus(_dbContext, notificationID);
                    return objTrainerView;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerViewData  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get Trainer pics
        /// </summary>
        /// <returns>List<PicsVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>       
        public static List<PicsVM> GetPics(int userId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetPics");
                    List<int> teamMemberIDS = new List<int>();
                    int teamId = 0;
                    int teamcredID = 0;
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var teamdetails = objCredential.UserType.Equals(Message.UserTypeTrainer) ? (from crd in _dbContext.Credentials
                                                                                                join usr in _dbContext.Trainer on crd.UserId equals usr.TrainerId
                                                                                                where usr.TrainerId == userId
                                                                                                select new
                                                                                                {
                                                                                                    usr.TeamId,
                                                                                                    crd.Id
                                                                                                }).FirstOrDefault() :
                                                                                                 objCredential.UserType.Equals(Message.UserTypeUser) ?
                                               (from crd in _dbContext.Credentials
                                                join usr in _dbContext.User on crd.UserId equals usr.UserId
                                                where usr.UserId == userId
                                                select new
                                                {
                                                    usr.TeamId,
                                                    crd.Id
                                                }).FirstOrDefault() : null;
                    if (teamdetails != null)
                    {
                        teamId = teamdetails.TeamId;
                        teamcredID = teamdetails.Id;
                    }

                    teamMemberIDS = (from usrtm in _dbContext.TrainerTeamMembers
                                     join crd in _dbContext.Credentials on usrtm.UserId equals crd.Id
                                     where usrtm.TeamId == teamId
                                     select usrtm.UserId).ToList();

                    teamMemberIDS.Add(teamcredID);
                    List<PicsVM> objpicList = (from m in _dbContext.MessageStraems
                                               join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                               join p in _dbContext.MessageStreamPic on m.MessageStraemId equals p.MessageStraemId
                                               where m.IsImageVideo
                                               && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                               && (teamMemberIDS.Contains(m.SubjectId) || (m.TargetType == Message.UserTargetType
                                               && m.SubjectId != m.TargetId && teamMemberIDS.Contains(m.TargetId)))
                                               orderby m.PostedDate descending
                                               select new PicsVM
                                               {
                                                   RecordId = p.RecordId,
                                                   PicUrl = p.PicUrl,
                                                   Height = p.Height,
                                                   Width = p.Width
                                               }).ToList();

                    objpicList.ForEach(v =>
                    {
                        //v.PicUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + v.PicUrl;
                        v.PicUrl = (!string.IsNullOrEmpty(v.PicUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                            Message.ResultPicDirectory + v.PicUrl))) ? CommonUtility.VirtualPath + Message.ResultPicDirectory + v.PicUrl : string.Empty;
                    });
                    return objpicList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetPics  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get Trainer videos
        /// </summary>
        /// <returns>List<VideoInfo></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>
        /// 
        public static List<VideoInfo> GetVideos(int userId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetVideos");
                    int teamId = 0;
                    int teamcredID = 0;
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);

                    var teamdetails = objCredential.UserType.Equals(Message.UserTypeTrainer) ? (from crd in _dbContext.Credentials
                                                                                                join usr in _dbContext.Trainer on crd.UserId equals usr.TrainerId
                                                                                                where usr.TrainerId == userId
                                                                                                select new
                                                                                                {
                                                                                                    usr.TeamId,
                                                                                                    crd.Id
                                                                                                }).FirstOrDefault() :
                                                                                                 objCredential.UserType.Equals(Message.UserTypeUser) ?
                                               (from crd in _dbContext.Credentials
                                                join usr in _dbContext.User on crd.UserId equals usr.UserId
                                                where usr.UserId == userId
                                                select new
                                                {
                                                    usr.TeamId,
                                                    crd.Id
                                                }).FirstOrDefault() : null;
                    if (teamdetails != null)
                    {
                        teamId = teamdetails.TeamId;
                        teamcredID = teamdetails.Id;
                    }
                    List<int> teamMemberIDS = (from usrtm in _dbContext.TrainerTeamMembers
                                               join crd in _dbContext.Credentials on usrtm.UserId equals crd.Id
                                               where usrtm.TeamId == teamId
                                               select usrtm.UserId).ToList();
                    teamMemberIDS.Add(teamcredID);
                    List<VideoInfo> objList = (from m in _dbContext.MessageStraems
                                               join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                               join p in _dbContext.MessageStreamVideo on m.MessageStraemId equals p.MessageStraemId
                                               where m.IsImageVideo
                                              && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                              && (teamMemberIDS.Contains(m.SubjectId) || (m.TargetType == Message.UserTargetType && m.SubjectId != m.TargetId
                                              && teamMemberIDS.Contains(m.TargetId))) && !string.IsNullOrEmpty(p.VideoUrl)
                                               orderby m.PostedDate descending
                                               select new VideoInfo
                                               {
                                                   RecordId = p.RecordId,
                                                   VideoUrl = p.VideoUrl
                                               }).ToList<VideoInfo>();

                    objList.ForEach(v =>
                    {
                        //v.VideoUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + v.VideoUrl;
                        v.VideoUrl = string.IsNullOrEmpty(v.VideoUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.VideoUrl;
                    });
                    return objList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetVideos  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        /// <summary>
        /// Function to get Trainer Photo and Pics for user or trainer
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/31/2015
        /// </devdoc>
        /// 
        public static Total<List<VideoAndPicVM>> GetVideosAndPics(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetVideosAndPics");
                    List<int> teamMemberIDS = new List<int>();
                    // Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int teamCredID = 0;
                    int teamId = 0;
                    if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                        || userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.User.FirstOrDefault(c => c.UserId == userId).TeamId;
                        teamCredID = (from crd in dataContext.Credentials
                                      join usr in dataContext.Teams on crd.UserId equals usr.TeamId
                                      where usr.TeamId == teamId && crd.UserType == Message.UserTypeTrainer
                                      select crd.Id).FirstOrDefault();
                    }
                    teamMemberIDS = (from usrtm in dataContext.TrainerTeamMembers
                                     join crd in dataContext.Credentials on usrtm.UserId equals crd.Id
                                     where usrtm.TeamId == teamId
                                     select usrtm.UserId).ToList();
                    if (teamCredID > 0)
                        teamMemberIDS.Add(teamCredID);
                    List<VideoAndPicVM> objVideoList = (from m in dataContext.MessageStraems
                                                        join c in dataContext.Credentials on m.SubjectId equals c.Id
                                                        join p in dataContext.MessageStreamVideo on m.MessageStraemId equals p.MessageStraemId
                                                        where m.IsImageVideo
                                                       && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                                       && (teamMemberIDS.Contains(m.SubjectId) || (m.TargetType == Message.UserTargetType && m.SubjectId != m.TargetId && teamMemberIDS.Contains(m.TargetId)))
                                                        orderby m.PostedDate descending
                                                        select new VideoAndPicVM
                                                        {
                                                            RecordId = p.RecordId,
                                                            MediaUrl = p.VideoUrl,
                                                            MediaType = Message.MediaTypeVideo,
                                                            PostedDate = m.PostedDate,
                                                            Height = string.Empty,
                                                            Width = string.Empty
                                                        }).ToList<VideoAndPicVM>();
                    objVideoList.ForEach(v =>
                    {
                        v.ThumbnailUrl = string.IsNullOrEmpty(v.MediaUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ResultVideoDirectory + v.MediaUrl.Split('.')[0] + Message.JpgImageExtension;
                        v.MediaUrl = string.IsNullOrEmpty(v.MediaUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ResultVideoDirectory + v.MediaUrl;
                    });
                    List<VideoAndPicVM> objPicList = (from m in dataContext.MessageStraems
                                                      join c in dataContext.Credentials on m.SubjectId equals c.Id
                                                      join p in dataContext.MessageStreamPic on m.MessageStraemId equals p.MessageStraemId
                                                      where m.IsImageVideo
                                                      && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                                      && (teamMemberIDS.Contains(m.SubjectId) || (m.TargetType == Message.UserTargetType && m.SubjectId != m.TargetId && teamMemberIDS.Contains(m.TargetId)))
                                                      orderby m.PostedDate descending
                                                      select new VideoAndPicVM
                                                      {
                                                          RecordId = p.RecordId,
                                                          MediaUrl = p.PicUrl,
                                                          MediaType = Message.MediaTypeImage,
                                                          PostedDate = m.PostedDate,
                                                          Height = p.Height,
                                                          Width = p.Width
                                                      }).ToList();

                    objPicList.ForEach(p =>
                    {
                        //p.MediaUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + p.MediaUrl;
                        p.MediaUrl = (!string.IsNullOrEmpty(p.MediaUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                            Message.ResultPicDirectory + p.MediaUrl))) ? CommonUtility.VirtualPath + Message.ResultPicDirectory + p.MediaUrl : string.Empty;
                    });
                    Total<List<VideoAndPicVM>> objresult = new Total<List<VideoAndPicVM>>();
                    var totaltrainerPhoto = objVideoList != null ? objVideoList.Union(objPicList).ToList() : objPicList;
                    objresult.TotalList = (from l in totaltrainerPhoto
                                           orderby l.PostedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = totaltrainerPhoto.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetVideosAndPics : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get FittnessTestCommon SubTypeId
        /// </summary>
        /// <param name="subTypeId"></param>
        /// <returns></returns>
        public static int GetFittnessTestCommonSubTypeId(int subTypeId)
        {
            switch (subTypeId)
            {
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
                    subTypeId = ConstantHelper.constFittnessCommonSubTypeId;
                    break;

            }
            return subTypeId;
        }
        /// <summary>
        /// Get MainSearch BarList
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TotalCount"></param>
        /// <param name="isMoreFlag"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetMainSearchBarList(MainSearchBarParam model)
        {
            StringBuilder traceLog = null;
            List<UserSearchVM> userSearchResultList = new List<UserSearchVM>();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetMainSearchBarList");
                    Credentials credobj = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int primaryTeamId = 0;
                    List<int> teamIds = new List<int>();
                    if (credobj != null)
                    {
                        switch (credobj.UserType)
                        {
                            case ConstantHelper.constuser:
                                teamIds = CommonApiBL.GetUserTeamList(dataContext, credobj.Id);
                                primaryTeamId = teamIds.FirstOrDefault();
                                break;
                            case ConstantHelper.consttrainer:
                                teamIds = CommonApiBL.GetTrainerTeamList(dataContext, credobj.Id);
                                primaryTeamId = dataContext.Trainer.Where(tr => tr.TrainerId == credobj.UserId).Select(t => t.TeamId).FirstOrDefault();
                                break;
                        }
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
                    Dictionary<string, object> objSearchedList = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(model.SearchString))
                    {
                        model.SearchString = model.SearchString.Trim();
                    }
                    List<UserSearchVM> objChallengeList = (from c in dataContext.Challenge
                                                           join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                           join ce in dataContext.ETCAssociations on c.ChallengeId equals ce.ChallengeId into challenge
                                                           from cech in challenge.DefaultIfEmpty()
                                                           join extype in dataContext.ExerciseTypes on cech.ExerciseTypeId equals extype.ExerciseTypeId into tempexerciseType
                                                           from exercisetype in tempexerciseType.DefaultIfEmpty()
                                                           where c.IsActive &&
                                                           ct.ChallengeType != ConstantHelper.ProgramChallengeType && (c.ChallengeName.ToLower().Contains(model.SearchString.ToLower())
                                                           || (from trzone in dataContext.ChallengeEquipmentAssociations
                                                               join bp in dataContext.Equipments
                                                               on trzone.EquipmentId equals bp.EquipmentId
                                                               where trzone.ChallengeId == c.ChallengeId
                                                               select bp.Equipment).ToList<string>().Contains(model.SearchString) || (from trzone in dataContext.TrainingZoneCAssociations
                                                                                                                                      join bp in dataContext.BodyPart
                                                                                                                                      on trzone.PartId equals bp.PartId
                                                                                                                                      where trzone.ChallengeId == c.ChallengeId
                                                                                                                                      select bp.PartName).ToList<string>().Contains(model.SearchString) || exercisetype.ExerciseName.ToLower().Contains(model.SearchString.ToLower()))
                                                           orderby c.ChallengeName ascending
                                                           select new UserSearchVM
                                                           {
                                                               ID = c.ChallengeId,
                                                               IsPremium = c.IsPremium,
                                                               TrainerId = c.TrainerId,
                                                               SelectSearchType = ConstantKey.ChallengeSerachType,
                                                               FullName = c.ChallengeName,
                                                               DifficultyLevel = c.DifficultyLevel,
                                                               ChallengeType = ct.ChallengeType,
                                                               Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                               ResultUnit = ct.ResultUnit,
                                                               ChallengeSubTypeId = ct.ChallengeSubTypeId
                                                           }).ToList();

                    if (isDefaultTeam)
                    {
                        objChallengeList = objChallengeList.Where(challenge => (!(challenge.TrainerId > 0)
                                            && (challenge.IsPremium || (GetFittnessTestCommonSubTypeId(challenge.ChallengeSubTypeId) == ConstantHelper.constFittnessCommonSubTypeId))
                                            && (GetFittnessTestCommonSubTypeId(challenge.ChallengeSubTypeId) == ConstantHelper.constFittnessCommonSubTypeId || isShownNoTrainerWorkoutProgram))).ToList();
                    }
                    else
                    {
                        List<int> teamtrainerIds = new List<int>();
                        if (teamIds.Count > 0)
                        {
                            teamtrainerIds = (from crd in dataContext.Credentials
                                              join tms in dataContext.TrainerTeamMembers
                                              on crd.Id equals tms.UserId
                                              where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                              select tms.UserId).ToList();

                        }
                        objChallengeList = objChallengeList.Where(challenge => (challenge.IsPremium || (GetFittnessTestCommonSubTypeId(challenge.ChallengeSubTypeId) == ConstantHelper.constFittnessCommonSubTypeId)) &&
                        ((challenge.TrainerId > 0 && teamtrainerIds.Contains(challenge.TrainerId))
                        || (challenge.TrainerId == 0 && (GetFittnessTestCommonSubTypeId(challenge.ChallengeSubTypeId) == ConstantHelper.constFittnessCommonSubTypeId || isShownNoTrainerWorkoutProgram)))).ToList();
                    }
                    objChallengeList.ForEach(ch =>
                    {
                        ch.IsWellness = ch.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        ch.ChallengeType = ch.ChallengeType.Split(' ')[0];
                    });

                    IQueryable<UserSearchVM> objQueryTrainerList = (from t in dataContext.Trainer
                                                                    join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                                                    where c.UserType == Message.UserTypeTrainer
                                                                    select new UserSearchVM
                                                                    {
                                                                        ID = t.TrainerId,
                                                                        SelectSearchType = ConstantKey.TrainerSerachType,
                                                                        FullName = t.FirstName + " " + t.LastName,
                                                                        FirstName = t.FirstName,
                                                                        Specilaization = (from ts in dataContext.TrainerSpecialization
                                                                                          join s in dataContext.Specialization
                                                                                          on ts.SpecializationId equals s.SpecializationId
                                                                                          where ts.IsInTopThree == 1 && ts.TrainerId == t.TrainerId
                                                                                          select s.SpecializationName).ToList(),
                                                                        City = dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName,
                                                                        State = t.State,
                                                                        TeamMemberCount = dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count,
                                                                        ImageUrl = t.TrainerImageUrl,
                                                                        IsVerifiedTrainer = 1,
                                                                        TrainerType = t.TrainerType
                                                                    });

                    List<UserSearchVM> objTrainerList = objQueryTrainerList.Where(trnr => trnr.FullName.ToLower().Contains(model.SearchString.ToLower())
                        || trnr.Specilaization.Where(item => item.ToLower().Contains(model.SearchString.ToLower())).Any()).OrderBy(p => p.FirstName).ToList();
                    objTrainerList.ForEach(trainer =>
                    {
                        trainer.ImageUrl = string.IsNullOrEmpty(trainer.ImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl;
                    });
                    IQueryable<UserSearchVM> objQueryUserList = (from cred in dataContext.Credentials
                                                                 join usr in dataContext.User on cred.UserId equals usr.UserId
                                                                 where cred.UserType == Message.UserTypeUser
                                                                 select new UserSearchVM
                                                                 {
                                                                     ID = cred.UserId,
                                                                     FullName = usr.FirstName + " " + usr.LastName,
                                                                     FirstName = usr.FirstName,
                                                                     ImageUrl = usr.UserImageUrl,
                                                                     City = usr.City,
                                                                     State = usr.State,
                                                                     SelectSearchType = ConstantKey.UserSearchType
                                                                 });
                    List<UserSearchVM> objUserList = objQueryUserList.Where(u => u.FullName.ToLower().Contains(model.SearchString.ToLower())).OrderBy(p => p.FirstName).ToList();
                    objUserList.ForEach(user =>
                    {
                        user.ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl;
                    });
                    //  For Combined ther user,trainer and challenege List based on index
                    switch (model.SelectTab)
                    {
                        case TabEnum.Home:
                            userSearchResultList = objTrainerList.Union(objUserList).Union(objChallengeList).ToList();
                            break;
                        case TabEnum.Challenge:
                            userSearchResultList = objChallengeList.Union(objTrainerList).Union(objUserList).ToList();
                            break;
                        case TabEnum.Team:
                            userSearchResultList = objChallengeList.Union(objTrainerList).Union(objUserList).ToList();
                            break;
                        case TabEnum.NewsFeed:
                            userSearchResultList = objUserList.Union(objTrainerList).Union(objChallengeList).ToList();
                            break;
                    }
                    List<UserSearchVM> serachList = (from l in userSearchResultList
                                                     select l).Skip(model.StartIndex).Take(model.EndIndex - model.StartIndex).ToList();
                    objTrainerList = serachList.Where(item => item.SelectSearchType == ConstantKey.TrainerSerachType).ToList();
                    objUserList = serachList.Where(item => item.SelectSearchType == ConstantKey.UserSearchType).ToList();
                    objChallengeList = serachList.Where(item => item.SelectSearchType == ConstantKey.ChallengeSerachType).ToList();
                    if (objChallengeList != null)
                    {
                        objChallengeList.ForEach(challenge =>
                        {
                            challenge.ChallengeName = challenge.FullName;
                        });
                    }
                    int TotalCount = 0;
                    TotalCount = userSearchResultList.Count();
                    bool isMoreFlag = false;
                    if (TotalCount > model.EndIndex)
                    {
                        isMoreFlag = true;
                    }
                    switch (model.SelectTab)
                    {
                        case TabEnum.Home:
                            objSearchedList.Add(ConstantKey.SearchedTrainerList, objTrainerList);
                            objSearchedList.Add(ConstantKey.SearchedUserList, objUserList);
                            objSearchedList.Add(ConstantKey.SearchedChallengeList, objChallengeList);

                            break;
                        case TabEnum.Challenge:
                            objSearchedList.Add(ConstantKey.SearchedChallengeList, objChallengeList);
                            objSearchedList.Add(ConstantKey.SearchedTrainerList, objTrainerList);
                            objSearchedList.Add(ConstantKey.SearchedUserList, objUserList);
                            break;
                        case TabEnum.Team:
                            objSearchedList.Add(ConstantKey.SearchedTrainerList, objTrainerList);
                            objSearchedList.Add(ConstantKey.SearchedUserList, objUserList);
                            objSearchedList.Add(ConstantKey.SearchedChallengeList, objChallengeList);
                            break;
                        case TabEnum.NewsFeed:
                            objSearchedList.Add(ConstantKey.SearchedUserList, objUserList);
                            objSearchedList.Add(ConstantKey.SearchedTrainerList, objTrainerList);
                            objSearchedList.Add(ConstantKey.SearchedChallengeList, objChallengeList);
                            break;
                    }
                    objSearchedList.Add("TotalCount", TotalCount);
                    objSearchedList.Add("isMoreFlag", isMoreFlag);
                    return objSearchedList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetMainSearchBarList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update app Subscription status
        /// </summary>
        /// <param name="appSubscriptionInfo"></param>
        /// <returns></returns>
        public static bool UpdateAppSubscriptionStatus(AppSubscriptionVM appSubscriptionInfo)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: UpdateAppSubscription() in HomeRequestBL");
                        if (!string.IsNullOrEmpty(appSubscriptionInfo.SubscriptionId) && appSubscriptionInfo.SubscriptionId != ConstantHelper.constSingleZero)
                        {
                            Credentials credobj = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                            tblCredentials objtblCredentials = dataContext.Credentials.Where(crdential => crdential.Id == credobj.Id).FirstOrDefault();
                            traceLog.AppendLine("UserCredId-" + credobj.Id + ",UserId-" + credobj.UserId + ",UserType-" + credobj.UserType);
                            bool isSubscriptionStatus = false;
                            if (objtblCredentials != null)
                            {
                                if (appSubscriptionInfo.DeviceType == DeviceType.IOS)
                                {
                                    traceLog.AppendLine("Receipt validation by Server and setting expiration date and SubscriptionId");
                                    IOSReceiptResponse objIOSReceiptResponse = ReceiptResponse(appSubscriptionInfo.ReceiptData);
                                    if (objIOSReceiptResponse != null)
                                    {
                                        if (objIOSReceiptResponse.status == 0)
                                        {                                           
                                            if (objIOSReceiptResponse.latest_receipt_info != null && objIOSReceiptResponse.latest_receipt_info.Count > 0)
                                            {
                                                int latestreceiptinfoCount = objIOSReceiptResponse.latest_receipt_info.Count;
                                                appSubscriptionInfo.Expires_date_ms = Convert.ToInt64(objIOSReceiptResponse.latest_receipt_info[latestreceiptinfoCount - 1].expires_date_ms);
                                                appSubscriptionInfo.SubscriptionId = objIOSReceiptResponse.latest_receipt_info[latestreceiptinfoCount - 1].original_transaction_id;
                                            }
                                            if(objIOSReceiptResponse.pending_renewal_info != null && objIOSReceiptResponse.pending_renewal_info[0].auto_renew_status == "1")
                                            { appSubscriptionInfo.AutoRenewing = true; }
                                            else { appSubscriptionInfo.AutoRenewing = false; }
                                           // appSubscriptionInfo.AutoRenewing = Boolean.Parse(objIOSReceiptResponse.pending_renewal_info[0].auto_renew_status);
                                        }
                                    }
                                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                                    var localsubscribtion = epoch.AddMilliseconds(appSubscriptionInfo.Expires_date_ms);
                                    if (localsubscribtion.Date >= DateTime.Now.Date)
                                    {
                                        appSubscriptionInfo.SubscriptionStatus = SubscriptionPurchaseStatus.Buy;
                                    }
                                    else
                                    {
                                        appSubscriptionInfo.SubscriptionStatus = SubscriptionPurchaseStatus.Cancellation;
                                    }
                                    traceLog.Append("Server validated Updated SubscriptionStatus" + appSubscriptionInfo.SubscriptionStatus);
                                }
                                switch (appSubscriptionInfo.SubscriptionStatus)
                                {
                                    case SubscriptionPurchaseStatus.Buy:
                                        isSubscriptionStatus = true;
                                        break;

                                    case SubscriptionPurchaseStatus.Cancellation:
                                        isSubscriptionStatus = false;
                                        break;
                                    case SubscriptionPurchaseStatus.Refund:
                                        isSubscriptionStatus = false;
                                        break;
                                }
                                if (appSubscriptionInfo.DeviceType == DeviceType.IOS)
                                {
                                    objtblCredentials.IOSSubscriptionStatus = isSubscriptionStatus;
                                }
                                else if (appSubscriptionInfo.DeviceType == DeviceType.Android)
                                {
                                    objtblCredentials.AndriodSubscriptionStatus = isSubscriptionStatus;
                                }
                                if (!dataContext.AppSubscriptions.Any(app => app.UserCredId == credobj.Id))
                                {
                                    tblAppSubscription objtblAppSubscription = new tblAppSubscription()
                                    {
                                        DeviceId = appSubscriptionInfo.DeviceId,
                                        DeviceType = Convert.ToString(appSubscriptionInfo.DeviceType),
                                        UserCredId = credobj.Id,
                                        SubscriptionBuyDate = FromUnixTime(appSubscriptionInfo.Purchase_date_ms, appSubscriptionInfo.Purchase_date),
                                        SubscriptionStatus = Convert.ToString(appSubscriptionInfo.SubscriptionStatus),
                                        SubscriptionId = appSubscriptionInfo.SubscriptionId,
                                        Purchase_date_ms = appSubscriptionInfo.Purchase_date_ms,
                                        Expires_date_ms = appSubscriptionInfo.Expires_date_ms,
                                        Is_Trial_Period = appSubscriptionInfo.Is_Trial_Prerod,
                                        AutoRenewing = appSubscriptionInfo.AutoRenewing,
                                        ReceiptData = appSubscriptionInfo.ReceiptData,
                                        CreatedDate = DateTime.Now,
                                        Purchase_date = appSubscriptionInfo.Purchase_date,
                                        Expires_date = null,
                                        AndriodPurchaseToken = appSubscriptionInfo.AndriodPurchaseToken,
                                        AndroidOrderId = appSubscriptionInfo.AndroidOrderId,
                                        IOSPassword = appSubscriptionInfo.IOSPassword
                                    };
                                    dataContext.AppSubscriptions.Add(objtblAppSubscription);                                    
                                }
                                else
                                {
                                    tblAppSubscription app = dataContext.AppSubscriptions.Where(appsub => appsub.UserCredId == credobj.Id).FirstOrDefault();
                                    if (app != null)
                                    {
                                        app.ModifiedDate = DateTime.Now;
                                        app.SubscriptionId = appSubscriptionInfo.SubscriptionId;
                                        if (isSubscriptionStatus && app.SubscriptionStatus == Convert.ToString(SubscriptionPurchaseStatus.Cancellation))
                                        {
                                            app.SubscriptionBuyDate = FromUnixTime(appSubscriptionInfo.Purchase_date_ms, appSubscriptionInfo.Purchase_date);
                                            app.Purchase_date_ms = appSubscriptionInfo.Purchase_date_ms;
                                            app.Expires_date_ms = appSubscriptionInfo.Expires_date_ms;                                           
                                            app.CreatedDate = DateTime.Now;
                                            app.Expires_date = null;
                                            app.Purchase_date = appSubscriptionInfo.Purchase_date;
                                        }
                                        else if (app.AutoRenewing == true && appSubscriptionInfo.AutoRenewing == false)
                                        {
                                            app.Expires_date = app.SubscriptionBuyDate ?? DateTime.Now.AddMonths(1);
                                        }
                                        app.Is_Trial_Period = appSubscriptionInfo.Is_Trial_Prerod;
                                        app.ReceiptData = appSubscriptionInfo.ReceiptData;
                                        app.DeviceId = appSubscriptionInfo.DeviceId;
                                        app.AutoRenewing = appSubscriptionInfo.AutoRenewing;
                                        app.DeviceType = Convert.ToString(appSubscriptionInfo.DeviceType);
                                        app.SubscriptionStatus = Convert.ToString(appSubscriptionInfo.SubscriptionStatus);
                                        app.AndriodPurchaseToken = appSubscriptionInfo.AndriodPurchaseToken;
                                        app.AndroidOrderId = appSubscriptionInfo.AndroidOrderId;
                                        app.IOSPassword = appSubscriptionInfo.IOSPassword;
                                    }
                                }
                            }
                            dataContext.SaveChanges();
                            dbTran.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: UpdateAppSubscription  : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Receipt Response based on send receipt by IOS app
        /// </summary>
        /// <param name="receiptData"></param>
        /// <returns></returns>
        private static IOSReceiptResponse ReceiptResponse(string receiptData)
        {
            StringBuilder traceLog = new StringBuilder();
            IOSReceiptResponse receiptResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(receiptData))
                {
                    traceLog.AppendLine("start ReceiptResponse() for receiptData-" + receiptData);
                    string receipt = receiptData;
                    receipt = receipt.Replace("{", string.Empty).Replace("}", string.Empty);
                    if (!string.IsNullOrEmpty(receipt))
                    {
                        string[] arrReceiptData = receipt.Split(';');
                        if (arrReceiptData != null && arrReceiptData.Length > 1)
                        {
                            string password = arrReceiptData[0];
                            string receiptdata = arrReceiptData[1];
                            string passwordValue = string.IsNullOrEmpty(password) ? string.Empty : password.Split('=')[1];
                            string receiptdataValue = string.Empty;
                            if (!string.IsNullOrEmpty(receiptdata))
                            {
                              int receiptKeyLength=  receiptdata.Split('=')[0].Length;
                                if (receiptKeyLength > 0)
                                {
                                    receiptdataValue = receiptdata.Substring(receiptKeyLength + 1);
                                }
                            }                            
                            //   string receiptdataValue = string.IsNullOrEmpty(receiptdata) ? string.Empty : receiptdata.Split('=')[1];
                            ReceiptRequest objReceiptRequest = new ReceiptRequest();
                            if (!string.IsNullOrEmpty(receiptdataValue) && !string.IsNullOrEmpty(passwordValue))
                            {
                                objReceiptRequest.ReceiptData = receiptdataValue;
                                objReceiptRequest.Password = passwordValue.Trim();
                                string response = ValidateIOSReceipt(objReceiptRequest);
                                if (!string.IsNullOrEmpty(response))
                                {
                                    receiptResponse = JsonConvert.DeserializeObject<IOSReceiptResponse>(response);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: ReceiptResponse  : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            return receiptResponse;
        }
        /// <summary>
        /// convert epcoc to Datetime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime, string purchaseddate)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start to get purchase datetime FromUnixTime()");
                if (unixTime > 0)
                {
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                    var localsubscribtion = epoch.AddMilliseconds(unixTime);
                    return localsubscribtion;
                }
                else
                {
                    string[] purchase = purchaseddate.Split('-');
                    int yy = Convert.ToInt32(purchase[0]);
                    int mm = Convert.ToInt32(purchase[1]);
                    int dd = Convert.ToInt32(purchase[2]);
                    return new DateTime(yy, mm, dd, 0, 0, 0, DateTimeKind.Local);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return DateTime.Now;
            }
            finally
            {
                traceLog.AppendLine("End: Server side validate FromUnixTime()  : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Validate IOS Receipt data in Server side
        /// </summary>
        /// <param name="appSubscriptionInfo"></param>
        /// <returns></returns>
        public static string ValidateIOSReceipt(ReceiptRequest appSubscriptionInfo)
        {
            StringBuilder traceLog = new StringBuilder();
            string returnmessage = string.Empty;
            try
            {
                traceLog.AppendLine("start Server side validate ValidateIOSReceipt()");
                traceLog.AppendLine("password-" + ConstantHelper.IOSpassword + ",receipt-data-" + appSubscriptionInfo.ReceiptData);
                appSubscriptionInfo.ReceiptData = appSubscriptionInfo.ReceiptData.Replace("\"", string.Empty);
                var json = new JObject(new JProperty("password", appSubscriptionInfo.Password), new JProperty("receipt-data", appSubscriptionInfo.ReceiptData.Trim())).ToString();
                ASCIIEncoding ascii = new ASCIIEncoding();
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                var request = System.Net.HttpWebRequest.Create("https://buy.itunes.apple.com/verifyReceipt");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = postBytes.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postBytes, 0, postBytes.Length);
                    stream.Flush();
                }
                var sendresponse = request.GetResponse();
                string sendresponsetext = "";
                using (var streamReader = new StreamReader(sendresponse.GetResponseStream()))
                {
                    sendresponsetext = streamReader.ReadToEnd().Trim();
                }
                returnmessage = sendresponsetext;
                traceLog.AppendLine("IOS subscrition Response"+ returnmessage);
                
            }
            catch (Exception ex )
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return returnmessage;
            }
            finally
            {               
                traceLog.AppendLine("End: Server side validate ValidateIOSReceipt()  : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            return returnmessage;
        }
        /// <summary>
        /// Validate Andriod Receipt data in Server side
        /// </summary>
        /// <returns></returns>
        public static bool ValidateAndriodReceipt()
        {

            return true;
        }

        /// <summary>
        /// Test Primium user subscrition 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool TestPrimiumSubscrition(int id)
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    string receipt = dataContext.AppSubscriptions.Where(app => app.AppSubscriptionId == id).FirstOrDefault().ReceiptData;
                    IOSReceiptResponse objtest = ReceiptResponse(receipt);
                    if (objtest != null)
                    {
                        return true;
                    }
                    return false;
                }
                catch { return false; }                
            }
           
        }
    }
}