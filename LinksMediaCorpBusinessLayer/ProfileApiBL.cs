namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System.Web;
    using System.Data.Objects.SqlClient;
    using LinksMediaCorpUtility.Resources;
    public class ProfileApiBL
    {
        /// <summary>
        /// Get Profile Details based on user Id and User Type
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static ProfileDetails GetHomeProfileDetails(int userID, string userType)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    //  string userPersoanlTrainerImageUrl = string.Empty;
                    // string userPersoanlTrainerEmailId = string.Empty;
                    traceLog.AppendLine("Start: GetProfileDetails----userId-" + userID + ",userType" + userType);
                    ProfileDetails profileDetails = new ProfileDetails();
                    if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        profileDetails = (from u in _dbContext.User
                                          join c in _dbContext.Credentials on u.UserId equals c.UserId
                                          join t in _dbContext.Teams on u.TeamId equals t.TeamId into userteam
                                          from tmm in userteam.DefaultIfEmpty()
                                          where c.UserId == userID && c.UserType.Equals(Message.UserTypeUser)
                                          select new ProfileDetails
                                          {
                                              UserId = c.UserId,
                                              UserType = c.UserType,
                                              UserName = "",
                                              TeamId = u.TeamId,
                                              ProfileImageUrl = u.UserImageUrl,
                                              IsDefaultTeam = (bool?)tmm.IsDefaultTeam ?? false,
                                              TeamProfileUrl = (tmm.ProfileImageUrl != null) ? tmm.ProfileImageUrl : string.Empty,
                                              TeamPremiumpicUrl = (tmm.PremiumImageUrl != null) ? tmm.PremiumImageUrl : string.Empty,
                                              TeamName = (tmm.TeamName != null) ? tmm.TeamName.Substring(1) : string.Empty,
                                              FirstName = u.FirstName,
                                              LastName = u.LastName,
                                              Zipcode = u.ZipCode,
                                              UserBrief = u.UserBrief,
                                              DateofBirth = u.DateOfBirth,
                                              Email = u.EmailId,
                                              IsPersonalTrainer = u.PersonalTrainerCredId > 0,
                                              UserPersoanlTrainerCredId = u.PersonalTrainerCredId
                                          }).FirstOrDefault();
                    }
                    else if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var objTrainerInfo = (from t in _dbContext.Trainer
                                              join c in _dbContext.Credentials on t.TrainerId equals c.UserId
                                              join tms in _dbContext.Teams on t.TeamId equals tms.TeamId into userteam
                                              from tmm in userteam.DefaultIfEmpty()
                                              where c.UserId == userID && c.UserType.Equals(Message.UserTypeTrainer)
                                              select new
                                              {
                                                  UserId = c.UserId,
                                                  UserType = c.UserType,
                                                  UserName = "",
                                                  ProfileImageUrl = t.TrainerImageUrl,
                                                  TrainerId = t.TrainerId,
                                                  TeamId = t.TeamId,
                                                  IsDefaultTeam = (bool?)tmm.IsDefaultTeam ??false,
                                                  TeamProfileUrl = (tmm.ProfileImageUrl != null) ? tmm.ProfileImageUrl : string.Empty,
                                                  TeamPremiumpicUrl = (tmm.PremiumImageUrl != null) ? tmm.PremiumImageUrl : string.Empty,
                                                  TeamName = (tmm.TeamName != null) ? tmm.TeamName.Substring(1) : string.Empty,
                                                  FirstName = t.FirstName,
                                                  LastName = t.LastName,
                                                  Zipcode = t.ZipCode,
                                                  DateofBirth = t.DateOfBirth,
                                                  Gender = t.Gender,
                                                  Height = t.Height,
                                                  Weight = t.Weight,
                                                  AboutMe = t.AboutMe,
                                                  TrainerType = t.TrainerType,
                                                  Email = t.EmailId,
                                                  IsPersonalTrainer = false,
                                                  UserPersoanlTrainerCredId = 0
                                              }).FirstOrDefault();
                        if (objTrainerInfo != null)
                        {
                            Bio objBio = new Bio();
                            DateTime today = DateTime.Today;
                            int age = 0;
                            if (objTrainerInfo.DateofBirth != null)
                            {
                                age = today.Year - ((DateTime)(objTrainerInfo.DateofBirth)).Year;
                            }
                            objBio.Age = Convert.ToString(age);
                            objBio.Gender = objTrainerInfo.Gender;
                            objBio.Height = objTrainerInfo.Height;
                            objBio.Weight = objTrainerInfo.Weight;
                            objBio.AboutME = objTrainerInfo.AboutMe;
                            objBio.IsVerifiedTrainer = 1;
                            objBio.TrainerType = objTrainerInfo.TrainerType;
                            objBio.TrainerId = objTrainerInfo.TrainerId;
                            objBio.TrainerImageURL = string.IsNullOrEmpty(objTrainerInfo.ProfileImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainerInfo.ProfileImageUrl;
                            objBio.SpecializationList = (from s in _dbContext.Specialization
                                                         join t in _dbContext.TrainerSpecialization on s.SpecializationId equals t.SpecializationId
                                                         where t.TrainerId == objTrainerInfo.TrainerId
                                                         select s.SpecializationName).ToList<string>();


                            profileDetails = new ProfileDetails()
                            {
                                UserId = objTrainerInfo.UserId,
                                UserType = objTrainerInfo.UserType,
                                UserName = "",
                                ProfileImageUrl = objTrainerInfo.ProfileImageUrl,
                                TeamPremiumpicUrl = objTrainerInfo.TeamPremiumpicUrl,
                                TrainerId = objTrainerInfo.TrainerId,
                                TeamId = objTrainerInfo.TeamId,
                                TeamName = objTrainerInfo.TeamName,
                                IsDefaultTeam = objTrainerInfo.IsDefaultTeam,
                                TeamProfileUrl = objTrainerInfo.TeamProfileUrl,
                                FirstName = objTrainerInfo.FirstName,
                                LastName = objTrainerInfo.LastName,
                                Zipcode = objTrainerInfo.Zipcode,
                                Email = objTrainerInfo.Email,
                                BioData = objBio
                            };
                        }
                    }

                    return profileDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End GetProfileDetails : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get User Personal Trainer Details
        /// </summary>
        /// <param name="userPersonalTrianerCrdId"></param>
        /// <returns></returns>
        public static UserPersoanlDetailsVM GetUserPersonalTrainerDetails(int userPersonalTrianerCrdId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserPersonalTrainerDetails----userPersonalTrianerCrdId-" + userPersonalTrianerCrdId);
                    UserPersoanlDetailsVM objTrainerInfo = (from t in _dbContext.Trainer
                                                            join c in _dbContext.Credentials on t.TrainerId equals c.UserId
                                                            where c.Id == userPersonalTrianerCrdId && c.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                                            select new UserPersoanlDetailsVM
                                                            {
                                                                PersoanlTrainerUserId = c.UserId,
                                                                PersonalTrainerName = t.FirstName + " " + t.LastName,
                                                                UserPersoanlTrainerEmailId = c.EmailId,
                                                                UserPersoanlTrainerImageUrl = t.TrainerImageUrl
                                                            }).FirstOrDefault();

                    if (objTrainerInfo != null)
                    {
                        objTrainerInfo.UserPersoanlTrainerImageUrl = string.IsNullOrEmpty(objTrainerInfo.UserPersoanlTrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainerInfo.UserPersoanlTrainerImageUrl;
                    }

                    return objTrainerInfo;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End GetUserPersonalTrainerDetails : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }

        /// <summary>
        /// Get ProfileDetails By User CredId
        /// </summary>
        /// <param name="credId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static ProfileDetails GetProfileDetailsByCredId(int credId, string userType)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetProfileDetailsByCredId----credId-" + credId);
                    ProfileDetails profileDetails = new ProfileDetails();
                    if (!string.IsNullOrEmpty(userType) && userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trdetails = (from crd in dataContext.Credentials
                                         join tr in dataContext.Trainer on crd.UserId equals tr.TrainerId
                                         where crd.Id == credId
                                         //&& crd.UserType == Message.UserTypeTrainer
                                         select tr).FirstOrDefault();
                        if (trdetails != null)
                        {
                            profileDetails.UserName = trdetails.FirstName + " " + trdetails.LastName;
                            profileDetails.ProfileImageUrl = string.IsNullOrEmpty(trdetails.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trdetails.TrainerImageUrl;
                            profileDetails.UserId = trdetails.TrainerId;
                        }
                    }
                    else if (!string.IsNullOrEmpty(userType) && userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var usrdetails = (from crd in dataContext.Credentials
                                          join usr in dataContext.User on crd.UserId equals usr.UserId
                                          where crd.Id == credId
                                          //&& crd.UserType == Message.UserTypeUser
                                          select usr).FirstOrDefault();
                        if (usrdetails != null)
                        {
                            profileDetails.UserName = usrdetails.FirstName + " " + usrdetails.LastName;
                            profileDetails.ProfileImageUrl = string.IsNullOrEmpty(usrdetails.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + usrdetails.UserImageUrl;
                            profileDetails.UserId = usrdetails.UserId;
                        }
                    }
                    return profileDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End GetProfileDetailsByCredId : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;

                }
            }
        }
        /// <summary>
        /// Get User Follower List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetUserFollowerList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<FollowerFollwingUserVM> objTeamTrainer = new List<FollowerFollwingUserVM>();
                List<FollowerFollwingUserVM> objTeamUser = new List<FollowerFollwingUserVM>();
                List<FollowerFollwingUserVM> objTeamlist = new List<FollowerFollwingUserVM>();
                Total<List<FollowerFollwingUserVM>> serachUserList = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserFollowerList");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<FollowUserVM> userfollowers = (from f in dataContext.Followings
                                                        join c in dataContext.Credentials on f.FollowUserId equals c.Id
                                                        where c.UserType == userType && c.UserId == userId
                                                        select new FollowUserVM
                                                        {
                                                            UserId = f.UserId,
                                                            UserType = f.UserId != 0 ? dataContext.Credentials.Where(item => item.Id == f.UserId).FirstOrDefault().UserType : string.Empty
                                                        }).ToList();
                    List<int> follwerUserIDs = userfollowers.Where(item => item.UserType == Message.UserTypeUser).Select(item => item.UserId).ToList();
                    List<int> follwertrainetIDs = userfollowers.Where(item => item.UserType == Message.UserTypeTrainer).Select(item => item.UserId).ToList();
                    List<int> follwingTeamIDs = userfollowers.Where(item => item.UserType == Message.UserTypeTeam).Select(item => item.UserId).ToList();
                    List<int> following = (from f in dataContext.Followings
                                           join c in dataContext.Credentials on f.UserId equals c.Id
                                           where c.UserType == cred.UserType && c.UserId == cred.UserId
                                           select f.FollowUserId).ToList();
                    // Get user following list
                    if (follwerUserIDs != null && follwerUserIDs.Count > 0)
                    {
                        objTeamUser = CommonBL.GetFollowingUsersList(cred, follwerUserIDs, following);
                    }
                    // Get trainer following list
                    if (follwertrainetIDs != null && follwertrainetIDs.Count > 0)
                    {
                        objTeamTrainer = CommonBL.GetFollowingTrainerList(cred, follwertrainetIDs, following);
                    }
                    // Get team following list
                    if (follwingTeamIDs != null && follwingTeamIDs.Count > 0)
                    {
                        objTeamlist = CommonBL.GetFollowingTeamList(cred, follwingTeamIDs, following);
                    }
                    var objUser = objTeamUser.Union(objTeamTrainer).Union(objTeamlist).ToList();
                    serachUserList = new Total<List<FollowerFollwingUserVM>>();
                    serachUserList.TotalList = (from l in objUser
                                                orderby l.FullName ascending
                                                select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    serachUserList.TotalCount = objUser.Count;
                    if ((serachUserList.TotalCount) > endIndex)
                    {
                        serachUserList.IsMoreAvailable = true;
                    }
                    return serachUserList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserFollowerList--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    objTeamTrainer = null;
                    objTeamUser = null;
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get  Following List of user or Trainer List in application.
        /// </summary>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetUserFollowingList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<FollowerFollwingUserVM> objTeamTrainer = new List<FollowerFollwingUserVM>();
                List<FollowerFollwingUserVM> objTeamUser = new List<FollowerFollwingUserVM>();
                List<FollowerFollwingUserVM> objTeamlist = new List<FollowerFollwingUserVM>();
                Total<List<FollowerFollwingUserVM>> serachUserList = null;
                try
                {
                    traceLog = new StringBuilder();

                    List<FollowUserVM> userfollowing = null;
                    traceLog.AppendLine("Start: GetUserFollowingList");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> following = (from f in dataContext.Followings
                                           join c in dataContext.Credentials on f.UserId equals c.Id
                                           where c.UserType == cred.UserType && c.UserId == cred.UserId
                                           select f.FollowUserId).ToList();
                    userfollowing = (from f in dataContext.Followings
                                     join c in dataContext.Credentials on f.UserId equals c.Id
                                     where c.UserType == userType && c.UserId == userId
                                     select new FollowUserVM
                                     {
                                         UserId = f.FollowUserId,
                                         UserType = (f.FollowUserId != 0) ? dataContext.Credentials.Where(item => item.Id == f.FollowUserId).FirstOrDefault().UserType : string.Empty
                                     }).ToList();
                    List<int> follwingUserIDs = userfollowing.Where(item => item.UserType == Message.UserTypeUser).Select(item => item.UserId).ToList();
                    List<int> follwingtrainetIDs = userfollowing.Where(item => item.UserType == Message.UserTypeTrainer).Select(item => item.UserId).ToList();
                    List<int> follwingTeamIDs = userfollowing.Where(item => item.UserType == Message.UserTypeTeam).Select(item => item.UserId).ToList();
                    // Get user foolwing list
                    if (follwingUserIDs != null && follwingUserIDs.Count > 0)
                    {
                        objTeamUser = CommonBL.GetFollowingUsersList(cred, follwingUserIDs, following);
                    }
                    // Get  trainer foolowing list
                    if (follwingtrainetIDs != null && follwingtrainetIDs.Count > 0)
                    {
                        objTeamTrainer = CommonBL.GetFollowingTrainerList(cred, follwingtrainetIDs, following);
                    }
                    // Get team list
                    if (follwingTeamIDs != null && follwingTeamIDs.Count > 0)
                    {
                        objTeamlist = CommonBL.GetFollowingTeamList(cred, follwingTeamIDs, following);
                    }
                    var userTotalFollwingList = objTeamUser.Union(objTeamTrainer).Union(objTeamlist).ToList();
                    serachUserList = new Total<List<FollowerFollwingUserVM>>();
                    serachUserList.TotalList = (from l in userTotalFollwingList
                                                orderby l.FullName ascending
                                                select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    if (userTotalFollwingList != null)
                    {
                        serachUserList.TotalCount = userTotalFollwingList.Count();
                    }
                    if ((serachUserList.TotalCount) > endIndex)
                    {
                        serachUserList.IsMoreAvailable = true;
                    }
                    return serachUserList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserFollowingList--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objTeamTrainer = null;
                    objTeamUser = null;
                }
            }
        }
        /// <summary>
        ///  Get UserNotificationSetting absed on user credID and Devices ID
        /// </summary>
        /// <param name="userCredId"></param>
        /// <param name="devicesTokenID"></param>
        /// <returns></returns>
        public static NotificationSettingVM GetUserNotificationSetting(int userCredId, string devicesTokenID, string devicesType)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                NotificationSettingVM objNotification = new NotificationSettingVM();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserNotificationSetting()");
                    if (string.IsNullOrEmpty(devicesTokenID))
                    {
                        objNotification.FriendChallegeNotify = false;
                        objNotification.TrainerChallegeNotify = false;
                        objNotification.TrainerJoinTeamNotify = false;
                    }
                    var item = (from un in _dbContext.UserNotificationSetting
                                where un.UserCredId == userCredId && un.DeviceID.Equals(devicesTokenID, StringComparison.OrdinalIgnoreCase) && un.DeviceType.Equals(devicesType, StringComparison.OrdinalIgnoreCase)
                                select new
                                {
                                    un.DeviceID,
                                    un.NotificationType,
                                    un.IsNotify
                                }).ToList();
                    if (item != null)
                    {
                        item.ForEach(nt =>
                        {
                            NotificationType objNotificationType;
                            if (!Enum.TryParse(nt.NotificationType, out objNotificationType))
                            {
                                objNotificationType = NotificationType.None;
                            }
                            switch (objNotificationType)
                            {
                                case NotificationType.FriendChallege:
                                    objNotification.FriendChallegeNotify = nt.IsNotify;
                                    break;
                                case NotificationType.TrainerChallege:
                                    objNotification.TrainerChallegeNotify = nt.IsNotify;
                                    break;
                                case NotificationType.TrainerJoinTeam:
                                    objNotification.TrainerJoinTeamNotify = nt.IsNotify;
                                    break;
                            }
                        });
                    }
                    return objNotification;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetUserNotificationSetting  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Trainer or User Challages List
        /// </summary>
        /// <returns></returns>
        public static List<PendingChallengeVM> GetUserChallengeList(int userId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            List<PendingChallengeVM> objuserList = null;
            List<PendingChallengeVM> objTempuserList = null;
            List<ChallengeByUser> userAndResult = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetPendingChallengeList---- " + DateTime.Now.ToLongDateString());
                    // Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int userCredID = 0;
                    if (userType.Equals(Message.UserTypeUser))
                    {
                        userCredID = (from crd in dataContext.Credentials
                                      where crd.UserId == userId && crd.UserType.Equals(Message.UserTypeUser)
                                      select crd.Id).FirstOrDefault();
                    }
                    else
                    {
                        userCredID = (from crd in dataContext.Credentials
                                      where crd.UserId == userId && crd.UserType.Equals(Message.UserTypeTrainer)
                                      select crd.Id).FirstOrDefault();
                    }
                    if (userCredID > 0)
                    {
                        List<PendingChallengeVM> listUserChallenge = (from uc in dataContext.UserChallenge
                                                                      join c in dataContext.Credentials on uc.UserId equals c.Id
                                                                      join ch in dataContext.Challenge
                                                                      on uc.ChallengeId equals ch.ChallengeId
                                                                      join ct in dataContext.ChallengeType on ch.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                      orderby uc.AcceptedDate descending
                                                                      where ch.IsActive && uc.UserId == userCredID && (uc.Result != null || uc.Fraction != null)
                                                                      select new PendingChallengeVM
                                                                      {
                                                                          ChallengeId = ch.ChallengeId,
                                                                          ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                          ChallengeName = ch.ChallengeName,
                                                                          DifficultyLevel = ch.DifficultyLevel,
                                                                          ChallengeType = ct.ChallengeType,
                                                                          IsSubscription = ch.IsSubscription,
                                                                          TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                                            join bp in dataContext.Equipments
                                                                                            on trzone.EquipmentId equals bp.EquipmentId
                                                                                            where trzone.ChallengeId == ch.ChallengeId
                                                                                            select bp.Equipment).Distinct().ToList<string>(),
                                                                          TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                                            join bp in dataContext.BodyPart
                                                                                            on trzone.PartId equals bp.PartId
                                                                                            where trzone.ChallengeId == ch.ChallengeId
                                                                                            select bp.PartName).Distinct().ToList<string>(),
                                                                          Strenght = dataContext.UserChallenge.Where(uch => uch.ChallengeId == ch.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                          ResultUnit = ct.ResultUnit,
                                                                          ChallengeByUserName = c.EmailId,  // For Only First Name only
                                                                          Result = uc.Result,
                                                                          Fraction = uc.Fraction,
                                                                          SubjectId = uc.CreatedBy,
                                                                          ResultUnitSuffix = uc.ResultUnit
                                                                      }).ToList();
                        objuserList = new List<PendingChallengeVM>();
                        List<int> objDistinctChallengeId = listUserChallenge.GroupBy(pl => pl.ChallengeId)
                                                                              .Select(grp => grp.FirstOrDefault().ChallengeId)
                                                                              .ToList();
                        objTempuserList = new List<PendingChallengeVM>();
                        foreach (int chlngeId in objDistinctChallengeId)
                        {
                            objTempuserList = listUserChallenge.Where(list => list.ChallengeId == chlngeId).ToList();
                            var chalengeDetails = objTempuserList.FirstOrDefault();
                            if (chalengeDetails != null && !string.IsNullOrEmpty(chalengeDetails.ResultUnit))
                            {
                                switch (chalengeDetails.ResultUnit)
                                {
                                    case ConstantHelper.constTime:
                                        objTempuserList.ForEach(r =>
                                        {
                                            r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(ConstantHelper.constDot, string.Empty));
                                            //Code for HH:MM:SS And MM:SS format
                                            string tempResult = r.Result;
                                            char[] splitChar = { ':' };
                                            string[] spliResult = tempResult.Split(splitChar);
                                            if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                            }
                                            else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                            {
                                                r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                            }
                                        });
                                        objTempuserList = chalengeDetails.ChallengeSubTypeId == 6 ?
                                                     objTempuserList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                                     : objTempuserList.OrderBy(k => k.TempOrderIntValue).ToList();
                                        //Get all username and its result in temp object
                                        userAndResult = (from usrResult in objTempuserList
                                                         select new ChallengeByUser
                                                         {
                                                             UserName = usrResult.ChallengeByUserName,
                                                             Result = usrResult.Result
                                                         }).ToList();
                                        //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                        objTempuserList = objTempuserList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).ToList();
                                        objTempuserList[0].ChallengeBy = userAndResult;
                                        objuserList.Add(objTempuserList[0]);
                                        break;
                                    case ConstantHelper.constReps:
                                    case ConstantHelper.constWeight:
                                    case ConstantHelper.constDistance:
                                        objTempuserList.ForEach(r =>
                                        {
                                            r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : (r.Result.Replace(",", string.Empty));
                                            r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToDouble(r.Result);
                                        });
                                        objTempuserList = objTempuserList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                        //Get all username and its result in temp object
                                        userAndResult = (from usrResult in objTempuserList
                                                         select new ChallengeByUser
                                                         {
                                                             UserName = usrResult.ChallengeByUserName,
                                                             Result = usrResult.Result
                                                         }).ToList();
                                        //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                        objTempuserList = objTempuserList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                        objTempuserList[0].ChallengeBy = userAndResult;
                                        objuserList.Add(objTempuserList[0]);
                                        break;
                                    case ConstantHelper.conRounds:
                                    case ConstantHelper.constInterval:
                                        objTempuserList.ForEach(r =>
                                        {
                                            if (!string.IsNullOrEmpty(r.Fraction))
                                            {
                                                string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                                r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                                r.Result = r.Result + " " + r.Fraction;
                                            }
                                            else
                                            {
                                                r.TempOrderIntValue = (float)Convert.ToInt16(r.Result);
                                            }
                                        });
                                        objTempuserList = objTempuserList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                                        //Get all username and its result in temp object
                                        userAndResult = (from usrResult in objTempuserList
                                                         select new ChallengeByUser
                                                         {
                                                             UserName = usrResult.ChallengeByUserName,
                                                             Result = usrResult.Result
                                                         }).ToList();

                                        //Code for removing duplicate user challenge on the basis of UserId(Keep Min result for Time
                                        objTempuserList = objTempuserList.GroupBy(result => new { result.ChallengeId }).Select(res => res.FirstOrDefault()).Take(3).ToList();
                                        objTempuserList[0].ChallengeBy = userAndResult;
                                        objuserList.Add(objTempuserList[0]);
                                        break;
                                }
                            }
                        }
                        listUserChallenge.ForEach(r =>
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                            if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                            {
                                r.Equipment = string.Join(", ", r.TempEquipments);
                            }
                            r.TempEquipments = null;
                            if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                            {
                                r.TargetZone = string.Join(", ", r.TempTargetZone);
                            }
                            r.TempTargetZone = null;
                        });
                        // Modified by 08/20/2015
                        objuserList.ForEach(r =>
                        {
                            r.ChallengeByUserName = r.ChallengeByUserName.Split(' ')[0];
                        });
                    }
                    return objuserList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetPendingChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get boom list of a comment on user profile UI
        /// </summary>
        /// <returns>Total<List<BoomVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static Total<List<BoomVM>> GetBoomList(int messageStreamId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetBoomList");
                    var result = (from b in _dbContext.Booms
                                  join c in _dbContext.Credentials on b.BoomedBy equals c.Id
                                  where b.MessageStraemId == messageStreamId
                                  select new
                                  {
                                      _Credential = c,
                                      _Boom = b
                                  }).ToList();
                    List<BoomVM> objBoomList = new List<BoomVM>();
                    foreach (var item in result)
                    {
                        BoomVM objBoom = new BoomVM();
                        objBoom.BoomedtDate = CommonWebApiBL.GetDateTimeFormat(item._Boom.BoomedDate);
                        objBoom.BoomId = item._Boom.BoomId;
                        if (item._Credential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var user = _dbContext.User.FirstOrDefault(u => u.UserId == item._Credential.UserId);
                            if (user != null)
                            {
                                objBoom.UserName = user.FirstName + " " + user.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
                            }
                        }
                        else if (item._Credential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                        {
                            var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == item._Credential.UserId);
                            if (trainer != null)
                            {
                                objBoom.UserName = trainer.FirstName + " " + trainer.LastName;
                                objBoom.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            }
                        }
                        objBoomList.Add(objBoom);
                    }
                    Total<List<BoomVM>> objresult = new Total<List<BoomVM>>();
                    objresult.TotalList = (from l in objBoomList
                                           orderby l.BoomedtDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objBoomList.Count();
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
                    traceLog.AppendLine("End : GetBoomList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to post boom on user profile UI
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static int PostBoom(ViewPostVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostBoom");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (!dataContext.Booms.Any(b => b.MessageStraemId == model.PostId && b.BoomedBy == objCred.Id))
                    {
                        tblBoom objBoom = new tblBoom();
                        objBoom.BoomedBy = objCred.Id;
                        objBoom.BoomedDate = DateTime.Now;
                        objBoom.MessageStraemId = model.PostId;
                        dataContext.Booms.Add(objBoom);
                        dataContext.SaveChanges();
                        // Send the User Notification to user on result boomed
                        if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType) && !(objCred.UserType == model.UserType && objCred.UserId == model.UserId))
                        {
                            NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.NewsFeedBoomed.ToString(), objCred, model.PostId);
                        }
                    }
                    return dataContext.Booms.Count(b => b.MessageStraemId == model.PostId);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostBoom  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to delete post
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/16/2015
        /// </devdoc>
        public static bool DeletePost(int messageStreamId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: DeletePost");
                    // Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (dataContext.MessageStraems.Any(m => m.MessageStraemId == messageStreamId))
                    {
                        if (dataContext.Comments.Any(cmnt => cmnt.MessageStraemId == messageStreamId))
                        {
                            dataContext.Comments.RemoveRange(dataContext.Comments.Where(cmnt => cmnt.MessageStraemId == messageStreamId));
                        }
                        dataContext.Booms.RemoveRange(dataContext.Booms.Where(b => b.MessageStraemId == messageStreamId));
                        dataContext.MessageStreamPic.RemoveRange(dataContext.MessageStreamPic.Where(b => b.MessageStraemId == messageStreamId));
                        dataContext.MessageStreamVideo.RemoveRange(dataContext.MessageStreamVideo.Where(b => b.MessageStraemId == messageStreamId));
                        dataContext.MessageStraems.RemoveRange(dataContext.MessageStraems.Where(m => m.MessageStraemId == messageStreamId));
                    }
                    dataContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : DeletePost  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get post list on on user profile UI
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static Total<List<ViewPostVM>> GetPostList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            Total<List<ViewPostVM>> objresult = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: ChallengeMessageFeedBL GetPostList");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    int userCredID = 0;
                    userCredID = (from crd in _dbContext.Credentials
                                  join usr in _dbContext.User
                                  on crd.UserId equals usr.UserId
                                  where crd.UserId == userId && string.Compare(crd.UserType, userType, StringComparison.OrdinalIgnoreCase) == 0
                                  select crd.Id).FirstOrDefault();
                    if (userCredID > 0)
                    {
                        List<ViewPostVM> objList = (from m in _dbContext.MessageStraems
                                                    join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                                    where m.IsImageVideo && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo) &&
                                                    !(m.TargetType == Message.UserTargetType && m.SubjectId != m.TargetId && m.SubjectId == userCredID)
                                                    &&
                                                    (m.SubjectId == userCredID
                                                    || (m.TargetType == Message.UserTargetType && m.SubjectId != m.TargetId && m.TargetId == userCredID))
                                                    orderby m.PostedDate descending
                                                    select new ViewPostVM
                                                    {
                                                        PostId = m.MessageStraemId,
                                                        DbPostedDate = m.PostedDate,
                                                        Message = m.Content,
                                                        BoomsCount = _dbContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                                        CommentsCount = _dbContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                                        IsLoginUserBoom = _dbContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == objCredential.Id),
                                                        IsLoginUserComment = _dbContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == objCredential.Id),
                                                        UserId = m.SubjectId,
                                                        UserType = _dbContext.Credentials.FirstOrDefault(crede => crede.Id == m.SubjectId).UserType,
                                                        PostedBy = m.SubjectId,
                                                        VideoList = (from vl in _dbContext.MessageStreamVideo
                                                                     where vl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                                                     select new VideoInfo
                                                                     {
                                                                         RecordId = vl.RecordId,
                                                                         VideoUrl = vl.VideoUrl
                                                                     }).ToList(),
                                                        PicList = (from pl in _dbContext.MessageStreamPic
                                                                   where pl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                                                   select new PicsInfo
                                                                   {
                                                                       PicId = pl.RecordId,
                                                                       PicsUrl = pl.PicUrl,
                                                                       ImageMode = pl.ImageMode,
                                                                       Width = pl.Width,
                                                                       Height = pl.Height
                                                                   }).ToList()
                                                    }).ToList();

                        // End the Modification
                        foreach (var item in objList)
                        {
                            tblCredentials objCred = _dbContext.Credentials.FirstOrDefault(cr => cr.Id == item.PostedBy);
                            item.UserId = objCred.UserId;
                            string imageUrl = string.Empty;
                            if (objCred != null && objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                            {
                                tblUser userTemp = _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                                if (userTemp != null)
                                    imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                        Message.ProfilePicDirectory + userTemp.UserImageUrl;
                            }
                            else if (objCred != null && objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                            {
                                tblTrainer trainerTemp = _dbContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                                if (trainerTemp != null)
                                    imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                        Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                            }
                            item.PostedByImageUrl = imageUrl;
                            item.UserName = (objCred.UserType.Equals(Message.UserTypeUser)
                                             ? _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                             + _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                             : _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                             + _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName);
                            item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                            //Code For Getting Posted Pics 
                            item.PicList.ForEach(pic =>
                            {
                                // pic.PicsUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl;
                                pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                                    Message.ResultPicDirectory + pic.PicsUrl))) ? CommonUtility.VirtualPath +
                                    Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                            });
                            string thumnailHeight, thumnailWidth;
                            //Code For Getting Posted Videos
                            item.VideoList.ForEach(vid =>
                            {
                                string thumnailFileName = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : vid.VideoUrl.Split('.')[0] +
                                    Message.JpgImageExtension;
                                vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                vid.VideoUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ResultVideoDirectory + vid.VideoUrl;
                                thumnailHeight = string.Empty;
                                thumnailWidth = string.Empty;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                vid.ThumbNailHeight = thumnailHeight;
                                vid.ThumbNailWidth = thumnailWidth;
                            });
                        }
                        objresult = new Total<List<ViewPostVM>>();
                        objresult.TotalList = (from l in objList
                                               orderby l.DbPostedDate descending
                                               select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                        objresult.TotalCount = objList.Count();
                        if ((objresult.TotalCount) > endIndex)
                        {
                            objresult.IsMoreAvailable = true;
                        }
                    }
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetPostList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get users  
        /// </summary>
        /// <param name="tearm"></param>
        /// <returns>List<string></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<string> GetUsers(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUsers for retrieving user from database ");                 
                    List<string> listOut = dataContext.User.Where(ct => (ct.FirstName + " " + ct.LastName).Contains(tearm)).Select(y => (SqlFunctions.StringConvert((double)y.UserId) + " " + y.FirstName + " " + y.LastName)).ToList();
                    if(listOut == null)
                    {
                        listOut = new List<string>();
                    }
                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUsers  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


    }
}