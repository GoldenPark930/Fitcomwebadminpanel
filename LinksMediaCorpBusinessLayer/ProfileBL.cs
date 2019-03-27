namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System.Web;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// Business logic class for Profile details of User and Trainer
    /// </summary>
    /// <devdoc>
    /// Developer Name - Arvind Kumar
    /// Date - 06/17/2015
    /// </devdoc>
    public class ProfileBL
    {
        /// <summary>
        /// Method to get user profile details
        /// </summary>
        /// <returns>UserProfileVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/17/2015
        /// </devdoc>
        public static UserProfileVM GetUserProfileDetails(int userId, ref bool isJoinedTeam, long notificationID = 0)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserProfileDetails");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    UserProfileVM objUserProfile = (from cred in dataContext.Credentials
                                                    join user in dataContext.User on cred.UserId equals user.UserId
                                                    where cred.UserType == Message.UserTypeUser && cred.UserId == userId
                                                    select new UserProfileVM
                                                    {
                                                        UserId = cred.UserId,
                                                        UserType = cred.UserType,
                                                        ProfileImageURL = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl,
                                                        UserBrief = user.UserBrief,
                                                        FirstName = user.FirstName,
                                                        TeamId = user.TeamId,
                                                        LastName = user.LastName,
                                                        EmailId = user.EmailId,
                                                        Zipcode = user.ZipCode,
                                                        IsFollowByLoginUser = dataContext.Followings.Any(f => f.UserId == objCred.Id && f.FollowUserId == cred.Id),
                                                        FollowingCount = dataContext.Followings.Where(f => f.UserId == cred.Id).Select(f => f.FollowUserId).Distinct().Count(),
                                                        FollowerCount = dataContext.Followings.Where(f => f.FollowUserId == cred.Id).Select(f => f.UserId).Distinct().Count(),
                                                    }).FirstOrDefault();

                    if (notificationID > 0)
                    {  // Update the Notifaction by read ststaus
                        NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    }
                    if (objUserProfile != null && objUserProfile.TeamId > 0)
                    {
                        isJoinedTeam = true;
                        objUserProfile.isJoinedTeam = true;
                        objUserProfile.IsDefaultTeam = dataContext.Teams.Any(ott => ott.TeamId == objUserProfile.TeamId && ott.IsDefaultTeam == true);
                    }
                    else
                    {
                        isJoinedTeam = false;
                    }
                    return objUserProfile;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserProfileDetails--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get Trainer Profile details
        /// </summary>
        /// <returns>TrainerViewVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/13/2015
        /// </devdoc>
        /// 
        public static TrainerViewVM GetTrainerProfileDetails(int trainerId, ref bool isJoinedTeam, long notificationID = 0)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTrainerProfileDetails");
                    bool isMyTeam = false;
                    TrainerViewVM objTrainerView = new TrainerViewVM();
                    Credentials userCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblCredentials tranerCredential = _dbContext.Credentials.FirstOrDefault(c => c.UserId == trainerId && c.UserType == Message.UserTypeTrainer);
                    var objTrainerInfo = (from t in _dbContext.Trainer
                                          join team in _dbContext.Teams
                                          on t.TeamId equals team.TeamId into trainetm
                                          from tm in trainetm.DefaultIfEmpty()
                                          where t.TrainerId == trainerId
                                          select new
                                          {
                                              t.TrainerId,
                                              t.FirstName,
                                              t.LastName,
                                              tm.TeamName,
                                              tm.TeamId,
                                              t.TrainerType,
                                              t.TrainerImageUrl,
                                              t.DateOfBirth,
                                              t.Gender,
                                              t.Height,
                                              t.Weight,
                                              t.AboutMe,
                                              t.EmailId
                                          }).FirstOrDefault();
                    if (objTrainerInfo != null)
                    {
                        objTrainerView.TrainerId = objTrainerInfo.TrainerId;
                        objTrainerView.TrainerName = objTrainerInfo.FirstName + " " + objTrainerInfo.LastName;
                        objTrainerView.FirstName = objTrainerInfo.FirstName;
                        objTrainerView.LastName = objTrainerInfo.LastName;
                        objTrainerView.EmailId = objTrainerInfo.EmailId;
                        objTrainerView.HashTag = objTrainerInfo.TeamName;
                        objTrainerView.IsVerifiedTrainer = 1;
                        objTrainerView.TrainerType = objTrainerInfo.TrainerType;
                        objTrainerView.TrainerImageURL = string.IsNullOrEmpty(objTrainerInfo.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objTrainerInfo.TrainerImageUrl;
                        objTrainerView.TeamMemberCount = (objTrainerInfo.TeamId > 0) ? _dbContext.TrainerTeamMembers.Where(t => t.TeamId == objTrainerInfo.TeamId).GroupBy(g => g.UserId).ToList().Count : 0;
                        objTrainerView.FollowingCount = _dbContext.Followings.Where(f => f.UserId == tranerCredential.Id).Select(f => f.FollowUserId).Distinct().Count();
                        objTrainerView.FollowerCount = _dbContext.Followings.Where(f => f.FollowUserId == tranerCredential.Id).Select(f => f.UserId).Distinct().Count();
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
                        objBio.TrainerType = objTrainerInfo.TrainerType;
                        objBio.TrainerId = objTrainerInfo.TrainerId;
                        objBio.TrainerImageURL = objTrainerInfo.TrainerImageUrl;
                        objBio.SpecializationList = (from s in _dbContext.Specialization
                                                     join t in _dbContext.TrainerSpecialization on s.SpecializationId equals t.SpecializationId
                                                     where t.TrainerId == trainerId
                                                     select s.SpecializationName).ToList<string>();
                        objTrainerView.BioData = objBio;
                        objTrainerView.IsJoinTeam = _dbContext.User.Any(u => u.UserId == userCredential.UserId && u.TeamId != 0);
                        objTrainerView.IsFollowByLoginUser = _dbContext.Followings.Any(f => f.UserId == userCredential.Id && f.FollowUserId == tranerCredential.Id);
                        if (objTrainerInfo.TeamId > 0 && userCredential.UserType == Message.UserTypeTrainer)
                        {
                            isMyTeam = _dbContext.Trainer.Any(u => u.TrainerId == userCredential.UserId && u.TeamId == objTrainerInfo.TeamId);
                        }
                        else if (objTrainerInfo.TeamId > 0 && userCredential.UserType == Message.UserTypeUser)
                        {
                            isMyTeam = _dbContext.User.Any(u => u.UserId == userCredential.UserId && u.TeamId == objTrainerInfo.TeamId);
                        }
                        if (objTrainerInfo.TeamId > 0)
                        {
                            objTrainerView.IsJoinTeam = isMyTeam;
                            objTrainerView.IsDefaultTeam = _dbContext.Teams.Any(ott => ott.TeamId == objTrainerInfo.TeamId && ott.IsDefaultTeam == true);
                        }
                        isJoinedTeam = isMyTeam;
                        if (notificationID > 0)
                        {
                            // Update the Notifaction by read ststaus
                            NotificationApiBL.UpdateNotificationReadStatus(_dbContext, notificationID);
                        }
                    }
                    return objTrainerView;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerProfileDetails  Start end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Method to update user profile
        /// </summary>
        /// <returns>UserProfileVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/23/2015
        /// </devdoc>
        public static async Task<UserProfileVM> UpdateUserProfile(UserProfileVM model, int currentUserID)
        {
            StringBuilder traceLog = null;
            int userId = 0;
            bool? IsZipcodeExist;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                UserProfileVM objUserProfile = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: UpdateUserProfile");
                    tblUser objUser = dataContext.User.FirstOrDefault(u => u.UserId == model.UserId);
                    if (objUser != null)
                    {
                        // Assigned the changed ZipCode and validate further it is valide zipcode or not.If yes assigned it
                        objUser.ZipCode = model.Zipcode;
                        if (!string.IsNullOrEmpty(model.Zipcode) && !objUser.ZipCode.Equals(model.Zipcode))
                        {
                            IsZipcodeExist = await UseresBL.IsZipcodeExist(model.Zipcode);
                            if (IsZipcodeExist.HasValue && IsZipcodeExist == false)
                            {
                                return objUserProfile = new UserProfileVM() { ZipcodeNotExist = true };
                            }
                            string[] arrCityState = await UseresBL.GetCityAndStateOnZipcode(model.Zipcode);
                            if (arrCityState != null && arrCityState.Count() == 2)
                            {
                                objUser.City = arrCityState[0];
                                objUser.State = arrCityState[1];
                            }
                            objUser.ZipCode = model.Zipcode;
                        }
                        objUser.FirstName = model.FirstName;
                        objUser.LastName = model.LastName;
                        objUser.EmailId = model.EmailId;
                        objUser.UserBrief = model.UserBrief;

                        objUser.ModifiedBy = dataContext.Credentials.FirstOrDefault(u => u.UserId == model.UserId && u.UserType == Message.UserTypeUser).Id;
                        objUser.ModifiedDate = DateTime.Now;
                        tblCredentials objCredetials = dataContext.Credentials.FirstOrDefault(u => u.Id == currentUserID);
                        objCredetials.EmailId = model.EmailId;
                        dataContext.SaveChanges();
                        userId = model.UserId;
                    }
                    objUserProfile = (from cred in dataContext.Credentials
                                      join user in dataContext.User on cred.UserId equals user.UserId
                                      where cred.UserType == Message.UserTypeUser && cred.UserId == userId
                                      select new UserProfileVM
                                      {
                                          UserId = cred.UserId,
                                          UserType = cred.UserType,
                                          ProfileImageURL = user.UserImageUrl,
                                          UserBrief = user.UserBrief,
                                          FirstName = user.FirstName,
                                          LastName = user.LastName,
                                          EmailId = user.EmailId,
                                          Zipcode = user.ZipCode,
                                          IsFollowByLoginUser = dataContext.Followings.Any(f => f.UserId == currentUserID && f.FollowUserId == cred.Id)
                                      }).FirstOrDefault();
                    if (objUserProfile != null)
                    {
                        objUserProfile.FollowingCount = dataContext.Followings.Count(f => f.UserId == dataContext.Credentials.FirstOrDefault(c => c.UserId == userId && c.UserType == Message.UserTypeUser).Id);
                        objUserProfile.FollowerCount = dataContext.Followings.Count(f => f.FollowUserId == dataContext.Credentials.FirstOrDefault(c => c.UserId == userId && c.UserType == Message.UserTypeUser).Id);
                        objUserProfile.ProfileImageURL = string.IsNullOrEmpty(objUserProfile.ProfileImageURL) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + objUserProfile.ProfileImageURL;

                    }
                    return objUserProfile;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: UpdateUserProfile--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get user pics
        /// </summary>
        /// <returns>List<PicsVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        /// 
        public static Total<List<PicsVM>> GetPics(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetPics");
                    List<PicsVM> objPicList = (from ms in dataContext.MessageStraems
                                               join c in dataContext.Credentials on ms.SubjectId equals c.Id
                                               join msp in dataContext.MessageStreamPic on ms.MessageStraemId equals msp.MessageStraemId
                                               where ms.IsImageVideo && c.UserId == userId && c.UserType == userType && ms.TargetType == Message.UserTargetType
                                               select new PicsVM
                                               {
                                                   RecordId = msp.RecordId,
                                                   PicUrl = msp.PicUrl,
                                                   PostedDate = Convert.ToString(ms.PostedDate),
                                                   Height = msp.Height,
                                                   Width = msp.Width
                                               }).Union(
                                               from ms in dataContext.MessageStraems
                                               join c in dataContext.Credentials on ms.TargetId equals c.Id
                                               join msp in dataContext.MessageStreamPic on ms.MessageStraemId equals msp.MessageStraemId
                                               where ms.IsImageVideo && c.UserId == userId && c.UserType == userType && ms.TargetType == Message.UserTargetType
                                               select new PicsVM
                                               {
                                                   RecordId = msp.RecordId,
                                                   PicUrl = msp.PicUrl,
                                                   PostedDate = Convert.ToString(ms.PostedDate),
                                                   Height = msp.Height,
                                                   Width = msp.Width
                                               }).ToList<PicsVM>();

                    objPicList.ForEach(p =>
                                        {
                                            // p.PicUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + p.PicUrl;
                                            p.PicUrl = (!string.IsNullOrEmpty(p.PicUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + p.PicUrl))) ? CommonUtility.VirtualPath + Message.ResultPicDirectory + p.PicUrl : string.Empty;

                                        });
                    Total<List<PicsVM>> objresult = new Total<List<PicsVM>>();
                    objresult.TotalList = (from l in objPicList
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objPicList.Count();
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
                    traceLog.AppendLine("End: GetPics: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get user videos
        /// </summary>
        /// <returns>List<VideoInfo></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/18/2015
        /// </devdoc>
        /// 
        public static Total<List<VideoInfo>> GetVideos(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetVideos");
                    List<VideoInfo> objVideoList = (from ms in dataContext.MessageStraems
                                                    join c in dataContext.Credentials on ms.SubjectId equals c.Id
                                                    join msv in dataContext.MessageStreamVideo on ms.MessageStraemId equals msv.MessageStraemId
                                                    where ms.IsImageVideo && c.UserId == userId && c.UserType == userType && ms.TargetType == Message.UserTargetType && !string.IsNullOrEmpty(msv.VideoUrl)
                                                    select new VideoInfo
                                                    {
                                                        RecordId = msv.RecordId,
                                                        VideoUrl = msv.VideoUrl
                                                    })
                                                    .Union(
                                                    (from ms in dataContext.MessageStraems
                                                     join c in dataContext.Credentials on ms.TargetId equals c.Id
                                                     join msv in dataContext.MessageStreamVideo on ms.MessageStraemId equals msv.MessageStraemId
                                                     where ms.IsImageVideo && c.UserId == userId && c.UserType == userType && ms.TargetType == Message.UserTargetType && !string.IsNullOrEmpty(msv.VideoUrl)
                                                     select new VideoInfo
                                                     {
                                                         RecordId = msv.RecordId,
                                                         VideoUrl = msv.VideoUrl
                                                     })).ToList<VideoInfo>();
                    objVideoList.ForEach(v =>
                    {
                        //v.VideoUrl = CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.VideoUrl;
                        v.VideoUrl = !string.IsNullOrEmpty(v.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.VideoUrl : string.Empty;

                    });
                    Total<List<VideoInfo>> objresult = new Total<List<VideoInfo>>();
                    objresult.TotalList = (from l in objVideoList
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objVideoList.Count();
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
                    traceLog.AppendLine("End: GetVideos : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get user Videos and Pics
        /// </summary>
        /// <returns>List<VideoAndPicVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
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

                    int userCredID = 0;
                    if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        userCredID = (from crd in dataContext.Credentials
                                      join usr in dataContext.Trainer on crd.UserId equals usr.TrainerId
                                      where usr.TrainerId == userId && crd.UserType == Message.UserTypeTrainer
                                      select crd.Id).FirstOrDefault();
                    }
                    else
                    {
                        userCredID = (from crd in dataContext.Credentials
                                      join usr in dataContext.User on crd.UserId equals usr.UserId
                                      where usr.UserId == userId && crd.UserType == Message.UserTypeUser
                                      select crd.Id).FirstOrDefault();
                    }

                    List<VideoAndPicVM> objVideoList = (from ms in dataContext.MessageStraems
                                                        join c in dataContext.Credentials on ms.SubjectId equals c.Id
                                                        join msv in dataContext.MessageStreamVideo on ms.MessageStraemId equals msv.MessageStraemId
                                                        where ms.IsImageVideo && c.UserId == userId && c.UserType == userType// && ms.TargetType == Message.UserTargetType
                                                        select new VideoAndPicVM
                                                        {
                                                            RecordId = msv.RecordId,
                                                            MediaUrl = msv.VideoUrl,
                                                            MediaType = Message.MediaTypeVideo,
                                                            PostedDate = ms.PostedDate
                                                        })
                                                    .Union(
                                                    (from ms in dataContext.MessageStraems
                                                     join c in dataContext.Credentials on ms.TargetId equals c.Id
                                                     join msv in dataContext.MessageStreamVideo on ms.MessageStraemId equals msv.MessageStraemId
                                                     //where c.UserId == userId && c.UserType == userType// && ms.TargetType == Message.UserTargetType
                                                     where ms.IsImageVideo && ms.TargetId == userCredID && ms.TargetType == Message.UserTargetType && ms.SubjectId != ms.TargetId
                                                     select new VideoAndPicVM
                                                     {
                                                         RecordId = msv.RecordId,
                                                         MediaUrl = msv.VideoUrl,
                                                         MediaType = Message.MediaTypeVideo,
                                                         PostedDate = ms.PostedDate
                                                     })).ToList<VideoAndPicVM>();
                    objVideoList.ForEach(v =>
                    {
                        v.ThumbnailUrl = string.IsNullOrEmpty(v.MediaUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.MediaUrl.Split('.')[0] + Message.JpgImageExtension;
                        //v.MediaUrl = CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.MediaUrl;
                        v.MediaUrl = !string.IsNullOrEmpty(v.MediaUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + v.MediaUrl : string.Empty;
                    });

                    List<VideoAndPicVM> objPicList = (from ms in dataContext.MessageStraems
                                                      join c in dataContext.Credentials on ms.SubjectId equals c.Id
                                                      join msp in dataContext.MessageStreamPic on ms.MessageStraemId equals msp.MessageStraemId
                                                      where ms.IsImageVideo && c.UserId == userId && c.UserType == userType //&& ms.TargetType == Message.UserTargetType
                                                      select new VideoAndPicVM
                                                      {
                                                          RecordId = msp.RecordId,
                                                          MediaUrl = msp.PicUrl,
                                                          MediaType = Message.MediaTypeImage,
                                                          PostedDate = ms.PostedDate,
                                                          Height = msp.Height,
                                                          Width = msp.Width
                                                      }).Union(
                                          from ms in dataContext.MessageStraems
                                          join c in dataContext.Credentials on ms.TargetId equals c.Id
                                          join msp in dataContext.MessageStreamPic on ms.MessageStraemId equals msp.MessageStraemId
                                          // where c.UserId == userId && c.UserType == userType && ms.TargetType == Message.UserTargetType
                                          where ms.IsImageVideo && ms.TargetId == userCredID && ms.TargetType == Message.UserTargetType && ms.SubjectId != ms.TargetId
                                          select new VideoAndPicVM
                                          {
                                              RecordId = msp.RecordId,
                                              MediaUrl = msp.PicUrl,
                                              MediaType = Message.MediaTypeImage,
                                              PostedDate = ms.PostedDate,
                                              Height = msp.Height,
                                              Width = msp.Width
                                          }).ToList<VideoAndPicVM>();

                    objPicList.ForEach(p =>
                    {
                        // p.MediaUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + p.MediaUrl;
                        p.MediaUrl = (!string.IsNullOrEmpty(p.MediaUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + p.MediaUrl))) ? CommonUtility.VirtualPath + Message.ResultPicDirectory + p.MediaUrl : string.Empty;
                    });

                    Total<List<VideoAndPicVM>> objresult = new Total<List<VideoAndPicVM>>();
                    objresult.TotalList = (from l in objVideoList.Union(objPicList)
                                           orderby l.PostedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objVideoList.Count();
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
        /// Method to update profile pic
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static bool UpdateProfilePic(int userId, string userType, string newPic)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserProfileDetails");
                    bool flag = false;
                    if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        tblUser objUser = dataContext.User.FirstOrDefault(u => u.UserId == userId);
                        if (objUser != null)
                        {
                            objUser.UserImageUrl = newPic;
                            objUser.ModifiedDate = DateTime.Now;
                        }
                        if (dataContext.SaveChanges() > 0)
                        {
                            flag = true;
                        }
                    }
                    else if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var objTrainer = dataContext.Trainer.FirstOrDefault(t => t.TrainerId == userId);
                        if (objTrainer != null)
                        {
                            objTrainer.TrainerImageUrl = newPic;
                            objTrainer.ModifiedDate = DateTime.Now;
                        }
                        if (dataContext.SaveChanges() > 0)
                        {
                            flag = true;
                        }
                    }

                    return flag;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserProfileDetails--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to post Share message text on user profile
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static ViewPostVM PostShare(ProfilePostVM<TextMessageStream> message)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostShare");
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblMessageStream objMessageStream = new tblMessageStream();
                    objMessageStream.Content = message.Stream.TextMessage;
                    objMessageStream.MessageType = Message.TextMessageType;
                    objMessageStream.PostedDate = DateTime.Now;
                    objMessageStream.TargetType = Message.UserTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    objMessageStream.TargetId = _dbContext.Credentials.Where(c => c.UserId == message.UserId && c.UserType == message.UserType).Select(crd => crd.Id).FirstOrDefault();
                    objMessageStream.IsImageVideo = message.IsImageVideo;
                    _dbContext.MessageStraems.Add(objMessageStream);
                    _dbContext.SaveChanges();
                    // User Notification when user post message on other user or trainer post data their team member
                    if (!(objCred.UserId == message.UserId && objCred.UserType.Equals(message.UserType, StringComparison.OrdinalIgnoreCase)))
                    {
                        NotificationApiBL.SendProfileNotificationToUser(objCred.UserType, objCred.UserId, message.UserId, message.UserType, objMessageStream.MessageStraemId);
                    }

                    ViewPostVM objViewPostVM = new ViewPostVM()
                    {
                        PostId = objMessageStream.MessageStraemId,
                        Message = objMessageStream.Content,
                        PostedDate = CommonWebApiBL.GetDateTimeFormat(objMessageStream.PostedDate),
                        BoomsCount = _dbContext.Booms.Count(b => b.MessageStraemId == objMessageStream.MessageStraemId),
                        CommentsCount = _dbContext.Comments.Count(cmnt => cmnt.MessageStraemId == objMessageStream.MessageStraemId),
                        UserType = objCred.UserType,
                        UserId = objCred.UserId
                    };
                    if (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        //tblUser user1 = _dbContext.User.FirstOrDefault(u => u.UserId == objMessageStream.SubjectId);
                        var user = (from usr in _dbContext.User
                                    join crd in _dbContext.Credentials on usr.UserId equals crd.UserId
                                    where crd.UserType == Message.UserTypeUser && crd.Id == objMessageStream.SubjectId
                                    select new
                                    {
                                        usr.FirstName,
                                        usr.LastName,
                                        usr.UserImageUrl,
                                        crd.UserId,
                                        crd.UserType,
                                        usr.TeamId
                                    }).FirstOrDefault();
                        if (user != null)
                        {
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(user.UserImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl : string.Empty;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                            objViewPostVM.UserId = user.UserId;
                            objViewPostVM.UserType = user.UserType;
                        }
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        // tblTrainer trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == objMessageStream.SubjectId);
                        var trainer = (from t in _dbContext.Trainer
                                       join crd in _dbContext.Credentials on t.TrainerId equals crd.UserId
                                       where crd.UserType == Message.UserTypeTrainer && crd.Id == objMessageStream.SubjectId
                                       select new
                                       {
                                           t.FirstName,
                                           t.LastName,
                                           t.TrainerImageUrl,
                                           crd.UserId,
                                           crd.UserType
                                       }).FirstOrDefault();
                        if (trainer != null)
                        {
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(trainer.TrainerImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl : string.Empty;
                            objViewPostVM.UserName = trainer.FirstName + " " + trainer.LastName;
                            objViewPostVM.UserId = trainer.UserId;
                            objViewPostVM.UserType = trainer.UserType;
                        }
                    }

                    return objViewPostVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End PostShare: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Fucntion to post comment on trainer profile UI
        /// </summary>
        /// <returns>CommentVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/19/2015
        /// </devdoc>
        public static CommentVM PostComment(CommentVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: PostComment");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblComment objComment = new tblComment();
                    objComment.CommentedBy = cred.Id;
                    objComment.CommentedDate = DateTime.Now;
                    objComment.Message = model.Message;
                    objComment.MessageStraemId = model.PostId;
                    _dbContext.Comments.Add(objComment);
                    _dbContext.SaveChanges();
                    // User Notification when user post message on other user or trainer post data their team member
                    if (!string.IsNullOrEmpty(model.UserType) && model.UserId == cred.UserId && cred.UserType == model.UserType)
                    {
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.NewsFeedCommented.ToString(), NotificationType.PostCommentedReplyMsg.ToString(), cred);
                    }
                    else if (model.UserId > 0 && !string.IsNullOrEmpty(model.UserType))
                    {
                        NotificationApiBL.SendNotificationToALLNType(model.UserId, model.UserType, NotificationType.NewsFeedCommented.ToString(), cred, model.PostId);
                        NotificationApiBL.SendALLSenderNotificationByUser(model.PostId, NotificationType.NewsFeedCommented.ToString(), NotificationType.PostCommentedReplyMsg.ToString(), cred);
                    }
                    CommentVM objCommentVM = new CommentVM()
                    {
                        CommentId = objComment.CommentId,
                        PostId = model.PostId,
                        CommentBy = objComment.CommentedBy,
                        Message = model.Message,
                        CommentDate = CommonWebApiBL.GetDateTimeFormat(objComment.CommentedDate),
                        PostedCommentDate = objComment.CommentedDate.ToUniversalTime()
                    };
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = _dbContext.User.FirstOrDefault(u => u.UserId == cred.UserId);
                        if (user != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.UserImageUrl;
                            objCommentVM.UserName = user.FirstName + " " + user.LastName;
                            objCommentVM.UserId = user.UserId;
                            objCommentVM.UserType = Message.UserTypeUser;
                        }
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = _dbContext.Trainer.FirstOrDefault(t => t.TrainerId == cred.UserId);
                        if (trainer != null)
                        {
                            objCommentVM.ImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            objCommentVM.UserName = trainer.FirstName + " " + trainer.LastName;
                            objCommentVM.UserId = trainer.TrainerId;
                            objCommentVM.UserType = Message.UserTypeTrainer;
                        }
                    }
                    return objCommentVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : PostComment  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get trainer last activity
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        public static ViewPostVM GetUserTrainerLatestActivity(int userId, string userType)
        {
            StringBuilder traceLog = null;

            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTrainerLatestActivity");
                    Credentials userCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> teamcredList = new List<int>();
                    TeamVM teamdetails = null;
                    if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        teamdetails = (from crd in _dbContext.Credentials
                                       join tr in _dbContext.Trainer
                                       on crd.UserId equals tr.TrainerId
                                       where crd.UserId == userId && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                       select new TeamVM
                                       {
                                           CredUserId = crd.Id,
                                           TeamId = tr.TeamId,
                                           TeamCredId = tr.TeamId != 0 ? _dbContext.Credentials.Where(cm => cm.UserId == tr.TeamId && cm.UserType == Message.UserTypeTeam)
                                           .Select(ucrd => ucrd.Id).FirstOrDefault() : 0
                                       }).FirstOrDefault();

                    }
                    else if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        teamdetails = (from crd in _dbContext.Credentials
                                       join usr in _dbContext.User
                                       on crd.UserId equals usr.UserId
                                       where crd.UserId == userId && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                       select new TeamVM
                                       {
                                           CredUserId = crd.Id,
                                           TeamId = usr.TeamId,
                                           TeamCredId = usr.TeamId != 0 ? _dbContext.Credentials.Where(cm => cm.UserId == usr.TeamId && cm.UserType == Message.UserTypeTeam)
                                           .Select(ucrd => ucrd.Id).FirstOrDefault() : 0
                                       }).FirstOrDefault();

                    }
                    if (teamdetails != null)
                    {
                        teamcredList = _dbContext.TrainerTeamMembers.Where(tm => tm.TeamId == teamdetails.TeamCredId).Select(t => t.UserId).ToList();
                    }

                    // Add  trainer profile post message
                    IQueryable<ViewPostVM> objList = (from m in _dbContext.MessageStraems
                                                      join c in _dbContext.Credentials on m.SubjectId equals c.Id
                                                      where m.IsImageVideo == true
                                                      && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo == true)
                                                      // && (m.SubjectId == enterdUserId ) // Not showing other posting in his profile ML9
                                                      && teamcredList.Contains(m.SubjectId) // For showing all team post
                                                      orderby m.PostedDate descending
                                                      select new ViewPostVM
                                                      {
                                                          PostId = m.MessageStraemId,
                                                          DbPostedDate = m.PostedDate,
                                                          Message = m.Content,
                                                          BoomsCount = _dbContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                                          CommentsCount = _dbContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                                          PostedBy = m.SubjectId,
                                                          UserId = _dbContext.Credentials.FirstOrDefault(crede => crede.Id == m.SubjectId).UserId,
                                                          UserType = _dbContext.Credentials.FirstOrDefault(crede => crede.Id == m.SubjectId).UserType,
                                                          IsLoginUserBoom = _dbContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == userCredential.Id),
                                                          IsLoginUserComment = _dbContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == userCredential.Id),
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
                                                      });
                    ViewPostVM latestTrainerActivity = null;
                    if (objList != null)
                    {
                        latestTrainerActivity = new ViewPostVM();
                        latestTrainerActivity = objList.FirstOrDefault();
                    }
                    //End the Modification on 08/17/2015
                    if (latestTrainerActivity != null)
                    {
                        tblCredentials objCred = _dbContext.Credentials.FirstOrDefault(cr => cr.Id == latestTrainerActivity.PostedBy);
                        string imageUrl = string.Empty;
                        if (objCred.UserType.Equals(Message.UserTypeUser))
                        {
                            tblUser userTemp = _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                            if (userTemp != null)
                                imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + userTemp.UserImageUrl;
                        }
                        else if (objCred.UserType.Equals(Message.UserTypeTrainer))
                        {
                            tblTrainer trainerTemp = _dbContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                            if (trainerTemp != null)
                            {
                                imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;

                            }
                        }
                        latestTrainerActivity.PostedByImageUrl = imageUrl;
                        latestTrainerActivity.UserName = (objCred.UserType.Equals(Message.UserTypeUser)
                                         ? _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                         + _dbContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                         : objCred.UserType.Equals(Message.UserTypeTrainer) ? _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                         + _dbContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName : string.Empty);
                        latestTrainerActivity.PostedDate = CommonWebApiBL.GetDateTimeFormat(latestTrainerActivity.DbPostedDate);
                        //Code For Getting Posted Pics 
                        latestTrainerActivity.PicList.ForEach(pic =>
                        {
                            // pic.PicsUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl;
                            pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ?
                                CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                        }
                        );
                        string thumnailHeight, thumnailWidth;
                        //Code For Getting Posted Videos
                        latestTrainerActivity.VideoList.ForEach(vid =>
                        {
                            string thumnailFileName = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            //  vid.VideoUrl = CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl;
                            vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                            thumnailHeight = string.Empty;
                            thumnailWidth = string.Empty;
                            CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                            vid.ThumbNailHeight = thumnailHeight;
                            vid.ThumbNailWidth = thumnailWidth;
                        });
                    }

                    return latestTrainerActivity;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetTrainerLatestActivity  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get user credential id by user id
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetUserCredentialId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserCredentialId retrieving user Credentil Id from database ");
                    tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == id && c.UserType == Message.UserTypeUser);
                    return objCred.Id;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUserCredentialId  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}