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
    using LinksMediaCorpUtility.Resources;
    using System.IO;
    using System.Globalization;
    using System.Drawing;
    public class TeamApiBL
    {
        public static TeamsDetails GetFilterTeams(MainSearchBarParam serachTeam)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFilterTeams");
                    int teamId = 0;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    TeamsDetails objTeamsDetails = new TeamsDetails();
                    /// Check user is Trainer Or User
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var userteamdetails = (from usr in dataContext.User
                                               join tm in dataContext.Teams
                                               on usr.TeamId equals tm.TeamId into tms
                                               from tmd in tms.DefaultIfEmpty()
                                               where usr.UserId == cred.UserId
                                               select new UserTeamDetail
                                               {
                                                   TeamId = usr.TeamId,
                                                   IsJoinDefaultTeam = (bool?)tmd.IsDefaultTeam ?? false
                                               }).FirstOrDefault();
                        if (userteamdetails != null)
                        {
                            objTeamsDetails.IsJoinDefaultTeam = userteamdetails.IsJoinDefaultTeam;
                            teamId = userteamdetails.TeamId;
                        }
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var userteamdetails = (from usr in dataContext.Trainer
                                               join tm in dataContext.Teams
                                               on usr.TeamId equals tm.TeamId into tms
                                               from tmd in tms.DefaultIfEmpty()
                                               where usr.TrainerId == cred.UserId
                                               select new UserTeamDetail
                                               {
                                                   TeamId = usr.TeamId,
                                                   IsJoinDefaultTeam = (bool?)tmd.IsDefaultTeam ??false
                                               }).FirstOrDefault();
                        if (userteamdetails != null)
                        {
                            objTeamsDetails.IsJoinDefaultTeam = userteamdetails.IsJoinDefaultTeam;
                            teamId = userteamdetails.TeamId;
                        }
                    }
                    List<TeamsVM> objallTeams = null;
                    IQueryable<TeamsVM> allTeams = (from t in dataContext.Teams
                                                    join c in dataContext.Credentials on t.TeamId equals c.UserId
                                                    where c.UserType == Message.UserTypeTeam && t.IsDefaultTeam != true
                                                    orderby t.TeamName ascending
                                                    select new TeamsVM
                                                    {
                                                        CredTeamId = c.Id,
                                                        TeamName = t.TeamName,
                                                        Address = t.Address,
                                                        TeamId = t.TeamId,
                                                        City = dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName,
                                                        TeamMemberCount = dataContext.TrainerTeamMembers.Where(tms => tms.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count,
                                                        State = t.State,
                                                        ImagePicUrl = t.ProfileImageUrl,
                                                        ImagePremiumUrl = t.PremiumImageUrl,
                                                        UserType = c.UserType,
                                                        UserId = c.UserId,
                                                        Status = JoinStatus.JoinTeam,
                                                        HashTag = t.TeamName
                                                    });
                    if (allTeams != null)
                    {
                        objallTeams = allTeams.Where(c => c.TeamName.ToLower().IndexOf(serachTeam.SearchString.ToLower()) > -1).OrderBy(c => c.TeamName).ToList();
                        if (!string.IsNullOrEmpty(serachTeam.SearchString))
                        {
                            int TotalCount = 0;
                            TotalCount = objallTeams != null ? objallTeams.Count() : 0;
                            if (TotalCount > serachTeam.EndIndex)
                            {
                                objTeamsDetails.isMoreFlag = true;
                            }
                            objallTeams = objallTeams.Skip(serachTeam.StartIndex).Take(serachTeam.EndIndex - serachTeam.StartIndex).ToList();
                        }
                    }
                    /// Set status to the each Trainer corresponding to login user
                    if (teamId > 0)
                    {
                        List<int> following = (from f in dataContext.Followings
                                               join c in dataContext.Credentials on f.UserId equals c.Id
                                               where c.UserType == cred.UserType && f.UserId == cred.Id
                                               select f.FollowUserId).ToList();
                        objallTeams.ForEach(tt =>
                        {
                            tt.Status = (objTeamsDetails.IsJoinDefaultTeam && cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)) ? JoinStatus.JoinTeam :
                                (following.Contains(tt.CredTeamId)) ? (JoinStatus.Follow) : (JoinStatus.Unfollow);
                            tt.TeamName = string.IsNullOrEmpty(tt.TeamName) ? string.Empty : tt.TeamName.Substring(1);
                        });
                        if (objallTeams.Any(trnr => trnr.TeamId == teamId))
                        {
                            TeamsVM obmyteam = objallTeams.FirstOrDefault(ott => ott.TeamId == teamId);
                            if (!objTeamsDetails.IsJoinDefaultTeam)
                            {
                                obmyteam.Status = JoinStatus.MyTeam;
                                objallTeams.Remove(obmyteam);
                                objallTeams.Insert(0, obmyteam);
                            }
                            objTeamsDetails.IsJoinTeam = true;
                        }
                    }
                    /// Set image url of trainer
                    objallTeams.ForEach(team =>
                    {
                        team.ImagePicUrl = string.IsNullOrEmpty(team.ImagePicUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ProfilePicDirectory + team.ImagePicUrl;
                        team.ImagePremiumUrl = string.IsNullOrEmpty(team.ImagePremiumUrl) ? string.Empty :
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + team.ImagePremiumUrl;
                    });
                    objTeamsDetails.Teams = objallTeams;                   
                    return objTeamsDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFilterTeams--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serachTeam"></param>
        /// <returns></returns>
        public static TeamsDetails GetAllTeams()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetAllTeams");
                    int teamId = 0;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    TeamsDetails objTeamsDetails = new TeamsDetails();
                    /// Check user is Trainer Or User
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var userteamdetails = (from usr in dataContext.User
                                               join tm in dataContext.Teams
                                               on usr.TeamId equals tm.TeamId into tms
                                               from tmd in tms.DefaultIfEmpty()
                                               where usr.UserId == cred.UserId
                                               select new UserTeamDetail
                                               {
                                                   TeamId = usr.TeamId,
                                                   IsJoinDefaultTeam = (bool?)tmd.IsDefaultTeam ?? false
                                               }).FirstOrDefault();
                        if (userteamdetails != null)
                        {
                            objTeamsDetails.IsJoinDefaultTeam = userteamdetails.IsJoinDefaultTeam;
                            teamId = userteamdetails.TeamId;
                        }
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var userteamdetails = (from usr in dataContext.Trainer
                                               join tm in dataContext.Teams
                                               on usr.TeamId equals tm.TeamId into tms
                                               from tmd in tms.DefaultIfEmpty()
                                               where usr.TrainerId == cred.UserId
                                               select new UserTeamDetail
                                               {
                                                   TeamId = usr.TeamId,
                                                   IsJoinDefaultTeam = (bool?)tmd.IsDefaultTeam ?? false
                                               }).FirstOrDefault();
                        if (userteamdetails != null)
                        {
                            objTeamsDetails.IsJoinDefaultTeam = userteamdetails.IsJoinDefaultTeam;
                            teamId = userteamdetails.TeamId;
                        }
                    }
                    List<TeamsVM> objallTeams = null;
                    IQueryable<TeamsVM> allTeams = (from t in dataContext.Teams
                                                    join c in dataContext.Credentials on t.TeamId equals c.UserId
                                                    where c.UserType == Message.UserTypeTeam && t.IsDefaultTeam != true
                                                    orderby t.TeamName ascending
                                                    select new TeamsVM
                                                    {
                                                        CredTeamId = c.Id,
                                                        TeamName = t.TeamName,
                                                        Address = t.Address,
                                                        TeamId = t.TeamId,
                                                        City = dataContext.Cities.FirstOrDefault(cty => cty.CityId == t.City).CityName,
                                                        TeamMemberCount = dataContext.TrainerTeamMembers.Where(tms => tms.TeamId == t.TeamId).GroupBy(g => g.UserId).ToList().Count,
                                                        State = t.State,
                                                        ImagePicUrl = t.ProfileImageUrl,
                                                        ImagePremiumUrl = t.PremiumImageUrl,
                                                        UserType = c.UserType,
                                                        UserId = c.UserId,
                                                        Status = JoinStatus.JoinTeam,
                                                        HashTag = t.TeamName
                                                    });
                    if (allTeams != null)
                    {
                        objallTeams = allTeams.OrderBy(c => c.TeamName).ToList();

                    }
                    /// Set status to the each Trainer corresponding to login user
                    if (teamId > 0)
                    {
                        List<int> following = (from f in dataContext.Followings
                                               join c in dataContext.Credentials on f.UserId equals c.Id
                                               where c.UserType == cred.UserType && f.UserId == cred.Id
                                               select f.FollowUserId).ToList();
                        objallTeams.ForEach(tt =>
                        {
                            tt.Status = (objTeamsDetails.IsJoinDefaultTeam && cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)) ? JoinStatus.JoinTeam :
                                (following.Contains(tt.CredTeamId)) ? (JoinStatus.Follow) : (JoinStatus.Unfollow);
                            tt.TeamName = string.IsNullOrEmpty(tt.TeamName) ? string.Empty : tt.TeamName.Substring(1);
                        });
                        if (objallTeams.Any(trnr => trnr.TeamId == teamId))
                        {
                            TeamsVM obmyteam = objallTeams.FirstOrDefault(ott => ott.TeamId == teamId);
                            if (!objTeamsDetails.IsJoinDefaultTeam)
                            {
                                obmyteam.Status = JoinStatus.MyTeam;
                                objallTeams.Remove(obmyteam);
                                objallTeams.Insert(0, obmyteam);
                            }
                            objTeamsDetails.IsJoinTeam = true;
                        }
                    }
                    /// Set image url of trainer
                    objallTeams.ForEach(team =>
                    {
                        team.ImagePicUrl = string.IsNullOrEmpty(team.ImagePicUrl) ? string.Empty :
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + team.ImagePicUrl;
                        team.ImagePremiumUrl = string.IsNullOrEmpty(team.ImagePremiumUrl) ? string.Empty :
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + team.ImagePremiumUrl;
                    });
                    objTeamsDetails.Teams = objallTeams;
                    return objTeamsDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetAllTeams--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Mobile Coach Trainers based on TeamId
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<TeamTrainerVM>> GetTeamMobileCoachTrainer(int startIndex, int endIndex)  
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamMobileCoachTrainer");
                    int teamId = 0;
                    Total<List<TeamTrainerVM>> alltrainerlist = new Total<List<TeamTrainerVM>>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    /// Check user is Trainer Or User
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.User.Where(usr => usr.UserId == cred.UserId).Select(tm => tm.TeamId).FirstOrDefault();
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.Trainer.Where(usr => usr.TrainerId == cred.UserId).Select(tm => tm.TeamId).FirstOrDefault();
                    }
                    if (teamId > 0)
                    {
                        // Modify the show Mobile Coaching trainer only
                        List<int> teamTrainerIds = (from tms in dataContext.TrainerTeamMembers
                                                    join crd in dataContext.Credentials
                                                     on tms.UserId equals crd.Id
                                                    where tms.TeamId == teamId && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                                    select tms.UserId).
                                                    Intersect(
                                                     from tms in dataContext.TrainerMobileCoachTeams                           
                                                     where tms.TeamId == teamId 
                                                     select tms.TrainerCredId
                                                   ).ToList();

                        List<TeamTrainerVM> objTeamTrainer = (from t in dataContext.Trainer
                                                              join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                                              where c.UserType == Message.UserTypeTrainer && teamTrainerIds.Contains(c.Id)
                                                              select new TeamTrainerVM
                                                              {
                                                                  TrainerId = t.TrainerId,
                                                                  TeamId = teamId,
                                                                  CredTrainerId = c.Id,
                                                                  TrainerName = t.FirstName + " " + t.LastName,
                                                                  ImageUrl = t.TrainerImageUrl,
                                                                  TrainerType = t.TrainerType,
                                                                  EmailId = c.EmailId,
                                                                  Specilaization = (from ts in dataContext.TrainerSpecialization
                                                                                    join s in dataContext.Specialization
                                                                                    on ts.SpecializationId equals s.SpecializationId
                                                                                    where ts.IsInTopThree == 1 && ts.TrainerId == t.TrainerId
                                                                                    select s.SpecializationName).ToList(),
                                                                  TeamMemberCount = dataContext.TrainerTeamMembers.Where(tms => tms.TeamId == teamId).GroupBy(g => g.UserId).ToList().Count
                                                              }).ToList();
                        if (objTeamTrainer != null)
                        {
                            objTeamTrainer = objTeamTrainer.Where(tm => tm.TeamId == teamId).OrderBy(c => c.TrainerName).ToList();
                            int totalcount = objTeamTrainer.Count;
                            objTeamTrainer = (from l in objTeamTrainer
                                              select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                            alltrainerlist.TotalCount = totalcount;
                            if ((totalcount) > endIndex)
                            {
                                alltrainerlist.IsMoreAvailable = true;
                            }
                        }
                        /// Set image url of trainer
                        objTeamTrainer.ForEach(trainer =>
                        {
                            trainer.ImageUrl = string.IsNullOrEmpty(trainer.ImageUrl) ? string.Empty :
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl;
                            trainer.Address = CommonWebApiBL.GetConcateString(trainer.City, trainer.State);
                        });
                        alltrainerlist.TotalList = objTeamTrainer;
                    }
                    return alltrainerlist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamMobileCoachTrainer--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        /// <summary>
        /// Function to get all trainers
        /// </summary>
        /// <returns>TeamTrainerVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/22/2015
        /// </devdoc>
        public static Total<List<TeamTrainerVM>> GetAllTeamTrainer(int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetAllTeamTrainer");
                    int teamId = 0;
                    Total<List<TeamTrainerVM>> alltrainerlist = new Total<List<TeamTrainerVM>>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    /// Check user is Trainer Or User
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.User.Where(usr => usr.UserId == cred.UserId).Select(tm => tm.TeamId).FirstOrDefault();
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.Trainer.Where(usr => usr.TrainerId == cred.UserId).Select(tm => tm.TeamId).FirstOrDefault();
                    }
                    if (teamId > 0)
                    {

                        List<int> teamTrainerIds = (from tms in dataContext.TrainerTeamMembers
                                                    join crd in dataContext.Credentials
                                                     on tms.UserId equals crd.Id
                                                    where tms.TeamId == teamId && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                                    select tms.UserId).ToList();

                        List<TeamTrainerVM> objTeamTrainer = (from t in dataContext.Trainer
                                                              join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                                              where c.UserType == Message.UserTypeTrainer && teamTrainerIds.Contains(c.Id)
                                                              select new TeamTrainerVM
                                                              {
                                                                  TrainerId = t.TrainerId,
                                                                  TeamId = teamId,
                                                                  CredTrainerId = c.Id,
                                                                  TrainerName = t.FirstName + " " + t.LastName,
                                                                  ImageUrl = t.TrainerImageUrl,
                                                                  TrainerType = t.TrainerType,
                                                                  EmailId = c.EmailId,
                                                                  Specilaization = (from ts in dataContext.TrainerSpecialization
                                                                                    join s in dataContext.Specialization
                                                                                    on ts.SpecializationId equals s.SpecializationId
                                                                                    where ts.IsInTopThree == 1 && ts.TrainerId == t.TrainerId
                                                                                    select s.SpecializationName).ToList(),
                                                                  TeamMemberCount = dataContext.TrainerTeamMembers.Where(tms => tms.TeamId == teamId).GroupBy(g => g.UserId).ToList().Count
                                                              }).ToList();
                        if (objTeamTrainer != null)
                        {
                            objTeamTrainer = objTeamTrainer.Where(tm => tm.TeamId == teamId).OrderBy(c => c.TrainerName).ToList();
                            int totalcount = objTeamTrainer.Count;
                            objTeamTrainer = (from l in objTeamTrainer
                                              select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                            alltrainerlist.TotalCount = totalcount;
                            if ((totalcount) > endIndex)
                            {
                                alltrainerlist.IsMoreAvailable = true;
                            }
                        }
                        /// Set image url of trainer
                        objTeamTrainer.ForEach(trainer =>
                        {
                            trainer.ImageUrl = string.IsNullOrEmpty(trainer.ImageUrl) ? string.Empty :
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl;
                            trainer.Address = CommonWebApiBL.GetConcateString(trainer.City, trainer.State);
                        });
                        alltrainerlist.TotalList = objTeamTrainer;
                    }
                    return alltrainerlist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetAllTeamTrainer--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get filter trainers
        /// </summary>
        /// <returns>TeamTrainerVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/22/2015
        /// </devdoc>
        public static List<TeamTrainerVM> GetFilterTrainer(TrainerFilterParam model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetFilterTrainer");
                    int teamId = 0;
                    List<TeamTrainerVM> objTeamTrainer = (from result in
                                                              (from t in dataContext.Trainer
                                                               join c in dataContext.Credentials on t.TrainerId equals c.UserId
                                                               where c.UserType == Message.UserTypeTrainer
                                                               select new TeamTrainerVM
                                                               {
                                                                   TrainerId = t.TrainerId,
                                                                   CredTrainerId = c.Id,
                                                                   TeamId = t.TeamId,
                                                                   TrainerName = t.FirstName + " " + t.LastName,
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
                                                                   TrainerType = t.TrainerType,
                                                                   Status = JoinStatus.JoinTeam,
                                                                   HashTag = dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName
                                                               })
                                                          where (model.Specialization == null || model.Specialization == Message.All || result.Specilaization.Contains(model.Specialization))
                                                              && (model.City == null || result.City == model.City)
                                                              && (model.State == null || result.State == model.State)
                                                          select result).OrderByDescending(c => c.TeamMemberCount).ToList();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);

                    /// Check user is Trainer Or User
                    if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.User.FirstOrDefault(usr => usr.UserId == cred.UserId).TeamId;
                    }
                    else if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        teamId = dataContext.Trainer.FirstOrDefault(usr => usr.TrainerId == cred.UserId).TeamId;
                    }
                    if (objTeamTrainer != null)
                        objTeamTrainer = objTeamTrainer.OrderByDescending(c => c.TeamMemberCount).ToList();
                    /// Set status to the each Trainer corresponding to login user
                    if (teamId > 0)
                    {
                        List<int> following = (from f in dataContext.Followings
                                               join c in dataContext.Credentials on f.FollowUserId equals c.Id
                                               where c.UserType == cred.UserType && f.UserId == cred.Id
                                               select f.FollowUserId).ToList();
                        objTeamTrainer.ForEach(tt =>
                        {
                            if (following.Contains(tt.CredTrainerId))
                            {
                                tt.Status = JoinStatus.Follow;
                            }
                            else
                            {
                                tt.Status = JoinStatus.Unfollow;
                            }
                            if (tt.TeamId == teamId)
                            {
                                tt.Status = JoinStatus.MyTeam;
                            }
                        });
                    }
                    /// Set image url of trainer
                    objTeamTrainer.ForEach(trainer =>
                    {
                        trainer.ImageUrl = string.IsNullOrEmpty(trainer.ImageUrl) ? string.Empty :
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.ImageUrl;
                        trainer.Address = CommonWebApiBL.GetConcateString(trainer.City, trainer.State);
                    });
                    return objTeamTrainer.OrderBy(c => c.TrainerName).ToList();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetFilterTrainer--- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get post list of my team
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/30/2015
        /// </devdoc>
        public static Total<List<ViewPostVM>> GetMyTeamPostList(string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<ViewPostVM> objList = null;
                List<int> teamMemberIDS = null;
                Total<List<ViewPostVM>> objresult = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL GetMyTeamPostList");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    // Find the primary trainer ID
                    int teamId = objCredential.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase) ?
                        dataContext.Trainer.FirstOrDefault(tm => tm.TrainerId == objCredential.UserId).TeamId :
                        objCredential.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase) ?
                        dataContext.User.FirstOrDefault(tm => tm.UserId == objCredential.UserId).TeamId : 0;
                    bool IsDefaultTeam = false;
                    IsDefaultTeam = dataContext.Teams.FirstOrDefault(tt => tt.TeamId == teamId).IsDefaultTeam;
                    bool isTeamJoined = (teamId > 0 && IsDefaultTeam != true) ? true : false;
                    teamMemberIDS = (from usrtm in dataContext.TrainerTeamMembers
                                     join crd in dataContext.Credentials on usrtm.UserId equals crd.Id
                                     where usrtm.TeamId == teamId
                                     select usrtm.UserId).Distinct().ToList();
                    if (teamMemberIDS != null)
                    {

                        if (userType.Equals(Message.UserTypeUser) || userType.Equals(Message.UserTypeTrainer))
                        {
                            int teamcredID = (from crd in dataContext.Credentials
                                              where crd.UserId == teamId && crd.UserType.Equals(Message.UserTypeTeam)
                                              select crd.Id).FirstOrDefault();
                            teamMemberIDS.Add(teamcredID);
                        }
                        objList = (from m in dataContext.MessageStraems
                                   join c in dataContext.Credentials on m.SubjectId equals c.Id
                                   where m.IsImageVideo && m.IsNewsFeedHide != true
                                   && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                   && ((teamMemberIDS.Contains(m.SubjectId) && m.TargetType != Message.UserTargetType)
                                   || (teamMemberIDS.Contains(m.SubjectId) && m.TargetType == Message.UserTargetType && teamMemberIDS.Contains(m.TargetId)))
                                   && m.IsTextImageVideo
                                   orderby m.PostedDate descending
                                   select new ViewPostVM
                                   {
                                       PostId = m.MessageStraemId,
                                       DbPostedDate = m.PostedDate,
                                       Message = m.Content,
                                       BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                       CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                       PostedBy = m.SubjectId,
                                       UserId = c.UserId,
                                       UserType = c.UserType,
                                       IsLoginUserBoom = dataContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == objCredential.Id),
                                       IsLoginUserComment = dataContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == objCredential.Id),
                                       VideoList = (from vl in dataContext.MessageStreamVideo
                                                    where vl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                                    select new VideoInfo
                                                    {
                                                        RecordId = vl.RecordId,
                                                        VideoUrl = vl.VideoUrl
                                                    }).ToList(),
                                       PicList = (from pl in dataContext.MessageStreamPic
                                                  where pl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                                  select new PicsInfo
                                                  {
                                                      PicId = pl.RecordId,
                                                      PicsUrl = pl.PicUrl,
                                                      ImageMode = pl.ImageMode,
                                                      Height = pl.Height,
                                                      Width = pl.Width
                                                  }).ToList(),
                                       TargetID = m.TargetId,
                                       TargetType = m.TargetType
                                   }).ToList();
                        // Remove the other team memeber showing list
                        objList = (from m in objList
                                   where !(m.TargetType == Message.UserTargetType && !teamMemberIDS.Contains(m.TargetID))
                                   select m).ToList();
                        foreach (var item in objList)
                        {
                            tblCredentials objCred = dataContext.Credentials.FirstOrDefault(cr => cr.Id == item.PostedBy);
                            string imageUrl = string.Empty;
                            if (objCred.UserType.Equals(Message.UserTypeUser))
                            {
                                tblUser userTemp = dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                                if (userTemp != null)
                                    imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty :
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + userTemp.UserImageUrl;
                            }
                            else if (objCred.UserType.Equals(Message.UserTypeTrainer))
                            {
                                tblTrainer trainerTemp = dataContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                                if (trainerTemp != null)
                                    imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty :
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                            }
                            item.PostedByImageUrl = imageUrl;
                            item.UserName = (objCred.UserType.Equals(Message.UserTypeUser)
                                             ? dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                             + dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                             : dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                             + dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName);
                            item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                            //Code For Getting Posted Pics 
                            item.PicList.ForEach(pic =>
                            {
                                pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) &&
                                    System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl)))
                                    ? CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                            });
                            string thumnailHeight, thumnailWidth;
                            //Code For Getting Posted Videos
                            item.VideoList.ForEach(vid =>
                            {
                                string thumnailFileName = vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                    Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath +
                                    Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                                thumnailHeight = string.Empty;
                                thumnailWidth = string.Empty;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                vid.ThumbNailHeight = thumnailHeight;
                                vid.ThumbNailWidth = thumnailWidth;
                            });
                        }
                    }
                    objresult = new Total<List<ViewPostVM>>();
                    objresult.IsJoinedTeam = isTeamJoined;
                    objresult.TotalList = (from l in objList
                                           orderby l.DbPostedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objList.Count();
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
                    traceLog.AppendLine("End : GetMyTeamPostList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objList = null;
                    teamMemberIDS = null;
                }
            }
        }
        /// <summary>
        /// Get Follwoing POst List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<ViewPostVM>> GetFollowingPostList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            List<int> userfollowIds = null;
            List<ViewPostVM> objList = null;
            Total<List<ViewPostVM>> objresult = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL GetFollowingPostList");
                    tblCredentials objCredential = dataContext.Credentials.FirstOrDefault(crd => crd.UserId == userId && crd.UserType.Equals(userType, StringComparison.OrdinalIgnoreCase));
                    if (objCredential != null)
                    {
                        userfollowIds = (from fol in dataContext.Followings
                                         where fol.UserId == objCredential.Id
                                         select fol.FollowUserId).ToList();
                        if (userfollowIds != null)
                        {
                            userfollowIds.Add(objCredential.Id);
                            objList = (from m in dataContext.MessageStraems
                                       join c in dataContext.Credentials
                                       on m.SubjectId equals c.Id
                                       where m.IsImageVideo && m.IsNewsFeedHide != true &&
                                      (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                        && (userfollowIds.Contains(m.SubjectId) || (userfollowIds.Contains(m.TargetId) && m.TargetType == Message.UserTargetType))
                                       orderby m.PostedDate descending
                                       select new ViewPostVM
                                       {
                                           PostId = m.MessageStraemId,
                                           DbPostedDate = m.PostedDate,
                                           Message = m.Content,
                                           BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                           CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                           PostedBy = m.SubjectId,
                                           UserId = c.UserId,
                                           UserType = c.UserType,
                                           IsLoginUserBoom = dataContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == objCredential.Id),
                                           IsLoginUserComment = dataContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == objCredential.Id),
                                           VideoList = (from vl in dataContext.MessageStreamVideo
                                                        where vl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                                        select new VideoInfo
                                                        {
                                                            RecordId = vl.RecordId,
                                                            VideoUrl = vl.VideoUrl
                                                        }).ToList(),
                                           PicList = (from pl in dataContext.MessageStreamPic
                                                      where pl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                                      select new PicsInfo
                                                      {
                                                          PicId = pl.RecordId,
                                                          PicsUrl = pl.PicUrl,
                                                          ImageMode = pl.ImageMode,
                                                          Height = pl.Height,
                                                          Width = pl.Width
                                                      }).ToList()
                                       }).ToList();
                            foreach (var item in objList)
                            {
                                tblCredentials objCred = dataContext.Credentials.FirstOrDefault(cr => cr.Id == item.PostedBy);
                                string imageUrl = string.Empty;
                                if (objCred != null && objCred.UserType.Equals(Message.UserTypeUser))
                                {
                                    tblUser userTemp = dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                                    if (userTemp != null)
                                        imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty :
                                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + userTemp.UserImageUrl;
                                }
                                else if (objCred != null && objCred.UserType.Equals(Message.UserTypeTrainer))
                                {
                                    tblTrainer trainerTemp = dataContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                                    if (trainerTemp != null)
                                        imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty :
                                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                                }
                                item.PostedByImageUrl = imageUrl;
                                item.UserName = (objCred.UserType.Equals(Message.UserTypeUser)
                                                 ? dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                                 + dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                                 : dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                                 + dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName);
                                item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                                //Code For Getting Posted Pics 
                                item.PicList.ForEach(pic =>
                                {
                                    pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ?
                                        CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                                }
                                );
                                string thumnailHeight, thumnailWidth;
                                //Code For Getting Posted Videos
                                item.VideoList.ForEach(vid =>
                                {
                                    string thumnailFileName = vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                    vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath +
                                        Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                    vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                                    thumnailHeight = string.Empty;
                                    thumnailWidth = string.Empty;
                                    CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                    vid.ThumbNailHeight = thumnailHeight;
                                    vid.ThumbNailWidth = thumnailWidth;
                                });
                            }
                        }
                    }
                    else
                    {
                        objList = new List<ViewPostVM>();
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
                    return objresult;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetFollowingPostList  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    userfollowIds = null;
                    objList = null;
                    traceLog = null;
                }
            }
        }


        /// <summary>
        /// Get Team member list of trainer
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetTeamMemberListold(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamMemberList");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> following = CommonReportingUtility.GetLogInUserFollowingCredIdList(dataContext, objCredential.UserId, objCredential.UserType);
                    List<string> userspecization = new List<string>() { ConstantHelper.constFFChallangeDescription };
                    int teamId = CommonReportingUtility.GetLogInUserPrimaryTeamId(dataContext, userId, userType);
                    List<FollowerFollwingUserVM> objUserList = null;
                    if (teamId > 0)
                    {
                        List<int> teamuserCredtialList = CommonReportingUtility.GetTeamMemeberCredIdList(dataContext, teamId);
                        if (teamuserCredtialList != null)
                        {

                            objUserList = (from cred in dataContext.Credentials
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
                                           }).Union(from cred in dataContext.Credentials
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
                            if (objUserList != null)
                            {
                                // Remove the duplicate user from  team group
                                objUserList = objUserList.GroupBy(ul => ul.CredID)
                                                                 .Select(grp => grp.FirstOrDefault())
                                                                 .ToList();
                                objUserList.ForEach(user =>
                                {
                                    user.ImageUrl = (!string.IsNullOrEmpty(user.ImageUrl) &&
                                        System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ?
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : string.Empty;

                                });
                            }
                        }
                    }
                    Total<List<FollowerFollwingUserVM>> objresult = new Total<List<FollowerFollwingUserVM>>();
                    objresult.TotalList = (from l in objUserList
                                           orderby l.FullName ascending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objUserList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }

                    return objresult;

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamMemberList: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Member List based user userId and usertype
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetTeamMemberList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeamMemberList");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> following = (from f in dataContext.Followings
                                           join c in dataContext.Credentials on f.UserId equals c.Id
                                           where c.UserType == objCredential.UserType && c.UserId == objCredential.UserId
                                           select f.FollowUserId).ToList();
                  //  List<string> userspecization = new List<string>() { ConstantHelper.constFFChallangeDescription };
                    int teamId = userType.Equals(Message.UserTypeTrainer) ? (from crd in dataContext.Credentials
                                                                             join usr in dataContext.Trainer on crd.UserId equals usr.TrainerId
                                                                             where usr.TrainerId == userId && crd.UserType == Message.UserTypeTrainer
                                                                             select usr.TeamId).FirstOrDefault()
                                                                             : userType.Equals(Message.UserTypeUser) ?
                                               (from crd in dataContext.Credentials
                                                join usr in dataContext.User on crd.UserId equals usr.UserId
                                                where usr.UserId == userId && crd.UserType == Message.UserTypeUser
                                                select usr.TeamId).FirstOrDefault() : 0;
                    List<FollowerFollwingUserVM> objUserList = null;
                    if (teamId > 0)
                    {
                        List<int> teamuserCredtialList = (from cr in dataContext.Credentials
                                                          join tms in dataContext.TrainerTeamMembers
                                                          on cr.Id equals tms.UserId
                                                          where tms.TeamId == teamId
                                                          select cr.Id).ToList();

                        if (teamuserCredtialList != null)
                        {
                            objUserList = CommonApiBL.TeamUserMemberList(teamuserCredtialList, following, dataContext, objCredential);
                            var trainerlist = CommonApiBL.TeamTrainerMemberList(teamuserCredtialList, following, dataContext, objCredential);
                            if (objUserList != null && trainerlist != null)
                            {
                                objUserList = objUserList.Union(trainerlist).ToList();
                            }
                            else if (trainerlist != null)
                            {
                                objUserList = trainerlist;
                            }
                            // Remove the duplicate user from  team group
                            objUserList = objUserList.GroupBy(ul => ul.CredID)
                                                             .Select(grp => grp.FirstOrDefault())
                                                             .ToList();
                            objUserList.ForEach(user =>
                            {
                                user.ImageUrl = (!string.IsNullOrEmpty(user.ImageUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ?
                                    CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : string.Empty;

                            });
                        }
                    }
                    Total<List<FollowerFollwingUserVM>> objresult = new Total<List<FollowerFollwingUserVM>>();
                    objresult.TotalList = (from l in objUserList
                                           orderby l.FullName ascending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objUserList.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }

                    return objresult;

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamMemberList: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team UserMember List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<FollowerFollwingUserVM>> GetTeamUserMemberList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamMemberList");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> following = CommonReportingUtility.GetLogInUserFollowingCredIdList(dataContext, objCredential.UserId, objCredential.UserType);
                    List<string> userspecization = new List<string>() { ConstantHelper.constFFChallangeDescription };
                    int teamId = CommonReportingUtility.GetLogInUserPrimaryTeamId(dataContext, userId, userType);
                    List<FollowerFollwingUserVM> objUserList = null;
                    if (teamId > 0)
                    {
                        List<int> teamuserCredtialList = CommonReportingUtility.GetTeamMemeberCredIdList(dataContext, teamId);
                        if (teamuserCredtialList != null)
                        {

                            objUserList = (from cred in dataContext.Credentials
                                           join usr in dataContext.User on cred.UserId equals usr.UserId
                                           where cred.UserType == Message.UserTypeUser && teamuserCredtialList.Contains(cred.Id) && usr.MTActive
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

                            // Remove the duplicate user from  team group
                            if (objUserList != null)
                            {
                                objUserList = objUserList.GroupBy(ul => ul.CredID)
                                                                 .Select(grp => grp.FirstOrDefault())
                                                                 .ToList();
                                objUserList.ForEach(user =>
                                {
                                    user.ImageUrl = (!string.IsNullOrEmpty(user.ImageUrl)
                                        && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ?
                                        CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : string.Empty;

                                });
                            }
                        }
                    }
                    Total<List<FollowerFollwingUserVM>> objresult = new Total<List<FollowerFollwingUserVM>>();
                    objresult.TotalList = (from l in objUserList
                                           orderby l.FullName ascending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = objUserList.Count();
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
                    traceLog.AppendLine("End: GetTeamMemberList: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Post details by ID
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public static ViewPostVM GetPostDetailsByPostId(int postId, long notificationID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                ViewPostVM objList = null;

                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL GetHomePostDetailsByPostId()-postId-" + postId);
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    // Update the Notifaction by read ststaus
                    NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    objList = (from m in dataContext.MessageStraems
                               join c in dataContext.Credentials on m.SubjectId equals c.Id
                               where m.IsImageVideo && m.IsNewsFeedHide != true
                               && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                && m.MessageStraemId == postId
                               select new ViewPostVM
                               {
                                   PostId = m.MessageStraemId,
                                   DbPostedDate = m.PostedDate,
                                   Message = m.Content,
                                   BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                   CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                   PostedBy = m.SubjectId,
                                   UserId = c.UserId,
                                   UserType = c.UserType,
                                   IsLoginUserBoom = dataContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == objCredential.Id),
                                   IsLoginUserComment = dataContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == objCredential.Id),
                                   VideoList = (from vl in dataContext.MessageStreamVideo
                                                where vl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                                select new VideoInfo
                                                {
                                                    RecordId = vl.RecordId,
                                                    VideoUrl = vl.VideoUrl
                                                }).ToList(),
                                   PicList = (from pl in dataContext.MessageStreamPic
                                              where pl.MessageStraemId == m.MessageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                              select new PicsInfo
                                              {
                                                  PicId = pl.RecordId,
                                                  PicsUrl = pl.PicUrl,
                                                  ImageMode = pl.ImageMode,
                                                  Height = pl.Height,
                                                  Width = pl.Width
                                              }).ToList(),
                                   TargetID = m.TargetId,
                                   TargetType = m.TargetType
                               }).FirstOrDefault();
                    if (objList != null)
                    {
                        tblCredentials objCred = dataContext.Credentials.FirstOrDefault(cr => cr.Id == objList.PostedBy);
                        string imageUrl = string.Empty;
                        if (objCred.UserType.Equals(Message.UserTypeUser))
                        {
                            tblUser userTemp = dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                            if (userTemp != null)
                                imageUrl = string.IsNullOrEmpty(userTemp.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + userTemp.UserImageUrl;
                        }

                        if (objCred.UserType.Equals(Message.UserTypeTrainer))
                        {
                            tblTrainer trainerTemp = dataContext.Trainer.FirstOrDefault(usr => usr.TrainerId == objCred.UserId);
                            if (trainerTemp != null)
                                imageUrl = string.IsNullOrEmpty(trainerTemp.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerTemp.TrainerImageUrl;
                        }
                        objList.PostedByImageUrl = imageUrl;
                        objList.UserName = (objCred.UserType.Equals(Message.UserTypeUser)
                                         ? dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).FirstName + " "
                                         + dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId).LastName
                                         : dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).FirstName + " "
                                         + dataContext.Trainer.FirstOrDefault(tnr => tnr.TrainerId == objCred.UserId).LastName);
                        objList.PostedDate = CommonWebApiBL.GetDateTimeFormat(objList.DbPostedDate);
                        //Code For Getting Posted Pics 
                        objList.PicList.ForEach(pic =>
                        {
                            pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl)))
                                ? CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;
                        });

                        string thumnailHeight, thumnailWidth;
                        //Code For Getting Posted Videos
                        objList.VideoList.ForEach(vid =>
                        {
                            string thumnailFileName = vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                            vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                            thumnailHeight = string.Empty;
                            thumnailWidth = string.Empty;
                            CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                            vid.ThumbNailHeight = thumnailHeight;
                            vid.ThumbNailWidth = thumnailWidth;
                        });
                    }

                    return objList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End : GetHomePostDetailsByPostId  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get Post result MyResultPost Details by posted result
        /// </summary>
        public static RecentResultVM GetPostedResultDetails(int resultId, long notificationID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                RecentResultVM resultQueryData = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL Get GeMyResultPostDetails-postResultId" + resultId);

                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    // Update the Notifaction by read ststaus
                    NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    //Query to get completed challenge list
                    resultQueryData = (from uc in dataContext.UserChallenge
                                       join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                       join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                       orderby uc.AcceptedDate descending
                                       where uc.Id == resultId && (uc.Result != null || uc.Fraction != null) && c.IsActive
                                       select new RecentResultVM
                                       {
                                           ResultId = uc.Id,
                                           ChallengeId = uc.ChallengeId,
                                           ChallengeName = c.ChallengeName,
                                           DifficultyLevel = c.DifficultyLevel,
                                           ChallengeSubTypeid = c.ChallengeSubTypeId,
                                           ChallengeType = ct.ChallengeType,
                                           Duration = c.FFChallengeDuration,
                                           ThumbnailUrl = (from cexe in dataContext.CEAssociation
                                                           join ex in dataContext.Exercise
                                                           on cexe.ExerciseId equals ex.ExerciseId
                                                           where cexe.ChallengeId == c.ChallengeId
                                                           orderby cexe.RocordId
                                                           select ex.ThumnailUrl
                                                           ).FirstOrDefault(),
                                           TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                             join bp in dataContext.BodyPart
                                                             on trzone.PartId equals bp.PartId
                                                             where trzone.ChallengeId == c.ChallengeId
                                                             select bp.PartName).Distinct().ToList<string>(),
                                           Strength = dataContext.UserChallenge.Where(ucc => ucc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                           Result = uc.Result,
                                           Fraction = uc.Fraction,
                                           ResultUnit = ct.ResultUnit,
                                           ResultUnitSuffix = uc.ResultUnit,
                                           userID = uc.UserId,
                                           DbPostedDate = uc.AcceptedDate,
                                           BoomsCount = dataContext.ResultBooms.Count(b => b.ResultId == uc.Id),
                                           CommentsCount = dataContext.ResultComments.Count(cmnt => cmnt.Id == uc.Id),
                                           UserType = dataContext.Credentials.Where(ucrd => ucrd.Id == uc.UserId).FirstOrDefault().UserType,
                                           IsLoginUserBoom = dataContext.ResultBooms.Where(bm => bm.ResultId == uc.Id).Any(b => b.BoomedBy == objCredential.Id),
                                           IsLoginUserComment = dataContext.ResultComments.Where(cm => cm.Id == uc.Id).Any(b => b.CommentedBy == objCredential.Id),
                                       }).FirstOrDefault();


                    if (resultQueryData != null)
                    {
                        resultQueryData.ChallengeType = string.IsNullOrEmpty(resultQueryData.ChallengeType) ? string.Empty : resultQueryData.ChallengeType.Split(' ')[0];
                        // Find the user details based on user credId and user Type
                        ProfileDetails userdetails = ProfileApiBL.GetProfileDetailsByCredId(resultQueryData.userID, resultQueryData.UserType);
                        if (userdetails != null)
                        {
                            resultQueryData.UserName = userdetails.UserName;
                            resultQueryData.UserImageUrl = userdetails.ProfileImageUrl;
                            resultQueryData.userID = userdetails.UserId;
                        }
                        resultQueryData.DbPostedDate = resultQueryData.DbPostedDate.ToUniversalTime();
                        // combing the traing zone into one string
                        if (resultQueryData.TempTargetZone != null && resultQueryData.TempTargetZone.Count > 0)
                        {
                            resultQueryData.TargetZone = string.Join(", ", resultQueryData.TempTargetZone);
                        }
                        resultQueryData.TempTargetZone = null;
                        string resultMethod = string.Empty;
                        if (!string.IsNullOrEmpty(resultQueryData.ResultUnit))
                        {
                            resultMethod = resultQueryData.ResultUnit.Trim();
                            // Find all result for latest  result submit  result
                            PersonalChallengeVM personalbestresult = TeamBL.GetGlobalPersonalBestResult(resultQueryData.ChallengeId, objCredential.Id, dataContext);
                            switch (resultMethod)
                            {
                                case ConstantHelper.constTime:
                                    resultQueryData.TempOrderIntValue = Convert.ToInt32(resultQueryData.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(ConstantHelper.constDot, string.Empty));
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
                                    resultQueryData.TempOrderIntValue = (float)Convert.ToDouble(resultQueryData.Result);
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
                                        resultQueryData.TempOrderIntValue = (float)(Convert.ToDecimal(resultQueryData.Result) + (arrString.Count() == 2
                                            ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                    }
                                    else
                                    {
                                        resultQueryData.TempOrderIntValue = (float)Convert.ToInt16(resultQueryData.Result);
                                    }
                                    resultQueryData.Result = resultQueryData.Result + " " + resultQueryData.Fraction;
                                    if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                                    {
                                        resultQueryData.PersonalBestResult = personalbestresult.Result.Trim();
                                    }
                                    if (!string.IsNullOrEmpty(resultQueryData.Result))
                                    {
                                        resultQueryData.Result = resultQueryData.Result.Trim();
                                    }
                                    resultQueryData.IsRecentChallengUserBest = (resultQueryData.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                                    break;
                            }
                        }
                    }
                    return resultQueryData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GeMyResultPostDetails  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }

        /// <summary>
        /// Get Team FeaturedList based team Id
        /// </summary>
        /// <param name="teamId"></param>
        public static List<FeaturedResponse> GetTeamFeaturedList(LinksMediaContext dataContext, int challengeSubTypeId, List<int> teamIds, int primaryTeamId)
        {
            StringBuilder traceLog = null;
            List<FeaturedResponse> featuredResponseData = new List<FeaturedResponse>();
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamBL Get GetTeamFeaturedList-teamIds");
                List<int> ChallengeSubTypeList = GetChallengeAllSubTypeId(challengeSubTypeId);
                int fittnessTestCommonSubTypeId = GetFittnessTestCommonSubTypeId(challengeSubTypeId);
                bool isShownNoTrainerWorkoutProgram = false;
                bool isShownNoTrainerFitnessTest = false;
                bool isDefaultTeam = false;
                if (primaryTeamId > 0)
                {
                    var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                    if (primaryTeam != null)
                    {
                        isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                        isShownNoTrainerFitnessTest = primaryTeam.IsShownNoTrainerFitnessTests;
                        isDefaultTeam = primaryTeam.IsDefaultTeam;
                    }
                }
                List<int> teamuserCredtialList = (from cr in dataContext.Credentials
                                                  join tms in dataContext.TrainerTeamMembers
                                                  on cr.Id equals tms.UserId
                                                  where cr.UserType == Message.UserTypeTrainer && teamIds.Contains(tms.TeamId)
                                                  select cr.Id).Distinct().ToList();
                if (isDefaultTeam)
                {
                    if (teamuserCredtialList != null)
                    {
                        teamuserCredtialList.Clear();
                    }
                    else
                    {
                        teamuserCredtialList = new List<int>();
                    }
                    teamuserCredtialList.Add(ConstantHelper.constDefaultFitcomTrainer);
                }

                var featuredResponselist = (from ch in dataContext.Challenge
                                            join CT in dataContext.ChallengeType on ch.ChallengeSubTypeId equals CT.ChallengeSubTypeId
                                            where (ch.IsActive
                                                  && (ch.IsPremium || fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId)
                                                  && ChallengeSubTypeList.Contains(CT.ChallengeSubTypeId)
                                                  && ch.IsFeatured
                                                  && (teamuserCredtialList.Contains(ch.TrainerId) || ch.TrainerId == 0))
                                            orderby ch.TrainerId descending
                                            select new
                                            {
                                                ChallengeId = ch.ChallengeId,
                                                FeaturedImageUrl = ch.FeaturedImageUrl,
                                                ChallengeSubTypeId = ch.ChallengeSubTypeId,
                                                ChallengeName = ch.ChallengeName,
                                                TrainerId = ch.TrainerId,
                                                NoTrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(wt => wt.ChallengeId == ch.ChallengeId).Select(tt => tt.TeamId).ToList()
                                            }).ToList();

                if (featuredResponselist != null)
                {

                    if (isDefaultTeam)
                    {                        
                        featuredResponselist = featuredResponselist.Where(ct => ct.TrainerId > 0  || (!(ct.TrainerId > 0) &&
                        (
                          (fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && isShownNoTrainerFitnessTest)
                          || (isShownNoTrainerWorkoutProgram && (ct.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType 
                                                            || ct.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                        ))
                        ).ToList();

                    }
                    else
                    {      

                        featuredResponselist = featuredResponselist.Where(ct => (ct.TrainerId > 0 && teamuserCredtialList.Contains(ct.TrainerId))
                         
                        || (
                         (ct.TrainerId == 0 && isShownNoTrainerWorkoutProgram && (ct.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType
                                                                                  || ct.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                         || (fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && ct.TrainerId == 0 && isShownNoTrainerFitnessTest)
                         )
                         ).ToList();

                    }
                    // remove the duplicate challenge in featured
                    if (featuredResponselist != null)
                    {
                        featuredResponselist = featuredResponselist.GroupBy(tt => tt.ChallengeId).Select(cc => cc.FirstOrDefault()).ToList();
                    }

                    featuredResponselist.ForEach(fr =>
                    {
                        int featuredTypeId = fr.ChallengeSubTypeId;
                        if (featuredTypeId > 0)
                        {
                            FeaturedResponse objFeaturedResponse = new FeaturedResponse();
                            objFeaturedResponse.FeaturedImageUrl = string.IsNullOrEmpty(fr.FeaturedImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.FeaturedImagePath + fr.FeaturedImageUrl;
                            objFeaturedResponse.ChallengeId = fr.ChallengeId;
                            objFeaturedResponse.ChallengeSubTypeId = featuredTypeId;
                            objFeaturedResponse.ChallengeName = fr.ChallengeName;
                            if (featuredTypeId == ConstantHelper.constProgramChallengeSubType)
                            {
                                var cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                                objFeaturedResponse.IsActiveProgram = dataContext.UserActivePrograms.Any(ap => ap.ProgramId == fr.ChallengeId && ap.UserCredId == cred.Id && !ap.IsCompleted);

                            }
                            switch (featuredTypeId)
                            {
                                case ConstantHelper.constProgramChallengeSubType:
                                    objFeaturedResponse.FeaturedType = FeaturedType.Program;
                                    break;
                                case ConstantHelper.constWorkoutChallengeSubType:
                                    objFeaturedResponse.FeaturedType = FeaturedType.Workout;
                                    break;
                                case ConstantHelper.constWellnessChallengeSubType:
                                    objFeaturedResponse.FeaturedType = FeaturedType.Wellness;
                                    break;
                                default:
                                    objFeaturedResponse.FeaturedType = FeaturedType.FitnessTest;
                                    break;
                            }
                            featuredResponseData.Add(objFeaturedResponse);
                        }
                    });
                }
                return featuredResponseData;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: GetTeamFeaturedList  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get Team Trending List based on teamId
        /// </summary>
        /// <param name="teamId"></param>
        public static List<TrendingResponse> GetTeamTrendingCategoryList(LinksMediaContext dataContext, int challengeSubTypeId, List<int> teamIds, int primaryTeamId)
        {
            StringBuilder traceLog = null;
            List<TrendingResponse> trendingResponseeData = new List<TrendingResponse>();
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TeamBL Get GetTeamTrendingList-teamIds");
                List<int> ChallengeSubTypeList = GetChallengeAllSubTypeId(challengeSubTypeId);
                int fittnessTestCommonSubTypeId = GetFittnessTestCommonSubTypeId(challengeSubTypeId);
                bool isShownNoTrainerWorkoutProgram = false;
                bool isShownNoTrainerFitnessTest = false;
                bool isDefaultTeam = false;
                if (primaryTeamId > 0)
                {
                    var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                    if (primaryTeam != null)
                    {
                        isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                        isShownNoTrainerFitnessTest = primaryTeam.IsShownNoTrainerFitnessTests;
                        isDefaultTeam = primaryTeam.IsDefaultTeam;
                    }
                }
                List<int> teamtrainerIds = (from cr in dataContext.Credentials
                                                  join tms in dataContext.TrainerTeamMembers
                                                  on cr.Id equals tms.UserId
                                                  where cr.UserType == Message.UserTypeTrainer && teamIds.Contains(tms.TeamId)
                                                  select cr.Id).Distinct().ToList();

                //List<int> teamtrainerIds = new List<int>();
                //if (teamIds.Count > 0)
                //{
                //    teamtrainerIds = (from crd in dataContext.Credentials
                //                      join tms in dataContext.TrainerTeamMembers
                //                      on crd.Id equals tms.UserId
                //                      where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                //                      select tms.UserId).ToList();
                //}

                //To find out team selected team trending assocaites category Id
                List<int> teamTrendingCategoryList = (from tm in dataContext.Teams
                                                      join tms in dataContext.TeamTrendingAssociations
                                                      on tm.TeamId equals tms.TeamId
                                                      where teamIds.Contains(tm.TeamId)
                                                      select tms.TrendingCategoryId).Distinct().ToList();
                if (isDefaultTeam)
                {
                    if (teamtrainerIds != null)
                    {
                        teamtrainerIds.Clear();
                    }else
                    {
                        teamtrainerIds = new List<int>();
                    }
                    teamtrainerIds.Add(ConstantHelper.constDefaultFitcomTrainer);
                }
                var trendingResponselist = (from ch in dataContext.Challenge
                                            join CT in dataContext.ChallengeTrendingAssociations
                                            on ch.ChallengeId equals CT.ChallengeId
                                            join tc in dataContext.TrendingCategory
                                            on CT.TrendingCategoryId equals tc.TrendingCategoryId
                                            where (ch.IsActive
                                               && (teamtrainerIds.Contains(ch.TrainerId) || ch.TrainerId == 0)
                                               && (ch.IsPremium || fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId)
                                                && ch.ChallengeSubTypeId > 0
                                                && ChallengeSubTypeList.Contains(ch.ChallengeSubTypeId)
                                                && tc.IsActive
                                                && (teamTrendingCategoryList.Contains(CT.TrendingCategoryId)
                                                ))
                                            orderby ch.TrainerId descending
                                            select new
                                            {
                                                TrendingCategoryId = tc.TrendingCategoryId,
                                                ChallengeSubTypeId = ch.ChallengeSubTypeId,
                                                TrendingImageUrl = tc.TrendingPicUrl,
                                                TrainerId = ch.TrainerId,
                                                NoTrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(wt => wt.ChallengeId == ch.ChallengeId).Select(tt => tt.TeamId).ToList()
                                            }).ToList();

                if (trendingResponselist != null)
                {
                    if (isDefaultTeam)
                    {
                        trendingResponselist = trendingResponselist.Where(ct => (ct.TrainerId > 0 && teamtrainerIds.Contains(ct.TrainerId)) ||(!(ct.TrainerId > 0) &&
                        ((fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && isShownNoTrainerFitnessTest)
                        || (isShownNoTrainerWorkoutProgram && (ct.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType
                                                            || ct.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                        ))).ToList();
                    }
                    else
                    {
                                            
                            trendingResponselist = trendingResponselist.Where(ct => ((ct.TrainerId > 0 && teamtrainerIds.Contains(ct.TrainerId))
                             || (
                            (ct.TrainerId == 0 && fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && isShownNoTrainerFitnessTest)
                            || ( ct.TrainerId == 0 && isShownNoTrainerWorkoutProgram && (ct.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType || ct.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                            )
                            )).ToList();                        
                       
                    }
                    if (trendingResponselist != null)
                    {
                        trendingResponselist = trendingResponselist.GroupBy(tt => tt.TrendingCategoryId).Select(cc => cc.FirstOrDefault()).ToList();
                    }
                    trendingResponselist.ForEach(tr =>
                    {
                        TrendingResponse objTrendingResponse = new TrendingResponse();
                        objTrendingResponse.TrendingCategoryId = tr.TrendingCategoryId;
                        objTrendingResponse.TrendingImageUrl = string.IsNullOrEmpty(tr.TrendingImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.TrendingImagePath + tr.TrendingImageUrl;
                        int featuredTypeId = tr.ChallengeSubTypeId;
                        objTrendingResponse.ChallengeSubTypeId = featuredTypeId;
                        if (featuredTypeId > 0)
                        {
                            switch (featuredTypeId)
                            {
                                case ConstantHelper.constWorkoutChallengeSubType:
                                    objTrendingResponse.TrendingType = TrendingType.Workout;
                                    break;
                                case ConstantHelper.constWellnessChallengeSubType:
                                    objTrendingResponse.TrendingType = TrendingType.Wellness;
                                    break;
                                case ConstantHelper.constProgramChallengeSubType:
                                    objTrendingResponse.TrendingType = TrendingType.Program;
                                    break;
                                default:
                                    objTrendingResponse.TrendingType = TrendingType.FitnessTest;
                                    break;
                            }

                        }
                        trendingResponseeData.Add(objTrendingResponse);
                    });
                }

                return trendingResponseeData;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: GetTeamTrendingList  --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get the all subtyle list based on fittness coomon Id, program and workout
        /// </summary>
        /// <param name="subTypeId"></param>
        /// <returns></returns>
        public static List<int> GetChallengeAllSubTypeId(int subTypeId)
        {
            List<int> ChallengeSubTypeList = new List<int>();
            switch (subTypeId)
            {
                case ConstantHelper.constWorkoutChallengeSubType:
                    ChallengeSubTypeList.Add(subTypeId);
                    break;
                case ConstantHelper.constProgramChallengeSubType:
                    ChallengeSubTypeList.Add(subTypeId);
                    break;
                case ConstantHelper.constWellnessChallengeSubType:
                case ConstantHelper.FreeformChallangeId:
                case 0:
                    ChallengeSubTypeList.Add(0);
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
            }
            return ChallengeSubTypeList;
        }
        /// <summary>
        /// Get FittnessTest Common SubTypeId
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
        /// 
        /// </summary>
        /// <param name="challengeSubTypeId"></param>
        /// <param name="trendingCategoryId"></param>
        /// <returns></returns>
        public static List<TrendingChallengeListResponse> GetTeamTrendingList(int challengeSubTypeId, int trendingCategoryId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<TrendingChallengeListResponse> trendingResponseeData = new List<TrendingChallengeListResponse>();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL Get GetTeamTrendingList-teamIds");
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> ChallengeSubTypeList = GetChallengeAllSubTypeId(challengeSubTypeId);
                    int fittnessTestCommonSubTypeId = GetFittnessTestCommonSubTypeId(challengeSubTypeId);
                    int primaryTeamId = 0;
                    List<int> teamIds = new List<int>();
                    switch (cred.UserType)
                    {
                        case ConstantHelper.constuser:
                            teamIds = CommonApiBL.GetUserTeamList(dataContext, cred.Id);
                            primaryTeamId = teamIds.FirstOrDefault();
                            break;
                        case ConstantHelper.consttrainer:
                            teamIds = CommonApiBL.GetTrainerTeamList(dataContext, cred.Id);
                            primaryTeamId = dataContext.Trainer.Where(tr => tr.TrainerId == cred.UserId).Select(t => t.TeamId).FirstOrDefault();
                            break;
                    }
                    bool isShownNoTrainerWorkoutProgram = false;
                    bool isShownNoTrainerFitnessTest = false;
                    bool isDefaultTeam = false;
                    if (primaryTeamId > 0)
                    {
                        var primaryTeam = dataContext.Teams.Where(tm => tm.TeamId == primaryTeamId).FirstOrDefault();
                        if (primaryTeam != null)
                        {
                            isShownNoTrainerWorkoutProgram = primaryTeam.IsShownNoTrainerWorkoutPrograms;
                            isShownNoTrainerFitnessTest= primaryTeam.IsShownNoTrainerFitnessTests;
                            isDefaultTeam = primaryTeam.IsDefaultTeam;
                        }
                    }

                    List<int> teamTrainerCredtialList = (from cr in dataContext.Credentials
                                                      join tms in dataContext.TrainerTeamMembers
                                                      on cr.Id equals tms.UserId
                                                      where cr.UserType == Message.UserTypeTrainer && teamIds.Contains(tms.TeamId)
                                                      select cr.Id).Distinct().ToList();
                    //To find out team selected team trending assocaites category Id
                    List<int> teamTrendingCategoryList = CommonApiBL.GetTeamTrendingCategoryIdList(dataContext, teamIds);
                    if (isDefaultTeam)
                    {
                        if (teamTrainerCredtialList != null)
                        {
                            teamTrainerCredtialList.Clear();
                        }
                        else
                        {
                            teamTrainerCredtialList = new List<int>();
                        }
                        teamTrainerCredtialList.Add(ConstantHelper.constDefaultFitcomTrainer);
                    }
                    var trendingResponseList = (from ch in dataContext.Challenge
                                                join tc in dataContext.ChallengeTrendingAssociations
                                                on ch.ChallengeId equals tc.ChallengeId
                                                where (ch.IsActive
                                                       && (ch.TrainerId == 0 || teamTrainerCredtialList.Contains(ch.TrainerId))
                                                       && (ch.IsPremium || fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId)
                                                       && ChallengeSubTypeList.Contains(ch.ChallengeSubTypeId)
                                                       && tc.TrendingCategoryId > 0
                                                       && tc.TrendingCategoryId == trendingCategoryId
                                                       && teamTrendingCategoryList.Contains(tc.TrendingCategoryId)
                                                      )
                                                orderby ch.TrainerId descending
                                                select new
                                                {
                                                    ChallengeId = ch.ChallengeId,
                                                    ChallengeSubTypeId = ch.ChallengeSubTypeId,
                                                    DifficultyLevel = ch.DifficultyLevel,
                                                    tc.TrendingCategoryId,
                                                    TrainerId = ch.TrainerId,
                                                    ExeciseVideoDetails = (from cexe in dataContext.CEAssociation
                                                                           join ex in dataContext.Exercise
                                                                           on cexe.ExerciseId equals ex.ExerciseId
                                                                           where cexe.ChallengeId == ch.ChallengeId
                                                                           orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                                                           select new ExeciseVideoDetail
                                                                           {
                                                                               ExeciseName = ex.ExerciseName,
                                                                               ExeciseUrl = ex.V720pUrl,
                                                                               ExerciseThumnail = ex.ThumnailUrl,
                                                                               ChallengeExeciseId = cexe.RocordId
                                                                           }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault(),
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
                                                    challengeDuration = ch.FFChallengeDuration,
                                                    ProgramImageUrl = ch.ProgramImageUrl,
                                                    ChallengeName = ch.ChallengeName,
                                                    NoTrainerWorkoutTeamList = dataContext.NoTrainerChallengeTeams.Where(wt => wt.ChallengeId == ch.ChallengeId).Select(tt => tt.TeamId).ToList()
                                                }).ToList();

                    if (trendingResponseList != null)
                    {
                        if (isDefaultTeam)
                        {
                            // trendingResponseList = trendingResponseList.Where(ctt => (!(ctt.TrainerId > 0) && (fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId || (isShownNoTrainerWorkoutProgram && ctt.NoTrainerWorkoutTeamList.Contains(primaryTeamId))))).ToList();
                            trendingResponseList = trendingResponseList.Where(ctt => (ctt.TrainerId > 0) ||
                            (!(ctt.TrainerId > 0)
                            &&(
                            (fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && isShownNoTrainerFitnessTest) 
                            || (isShownNoTrainerWorkoutProgram && (ctt.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType
                                                                                  || ctt.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                            ))).ToList();
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
                            trendingResponseList = trendingResponseList.Where(ctt => ((ctt.TrainerId > 0 && teamtrainerIds.Contains(ctt.TrainerId))
                            //|| (primaryTeamId > 0 && ctt.NoTrainerWorkoutTeamList.Contains(primaryTeamId))
                           || (ctt.TrainerId == 0  && fittnessTestCommonSubTypeId == ConstantHelper.constFittnessCommonSubTypeId && isShownNoTrainerFitnessTest)
                           || (ctt.TrainerId == 0 && isShownNoTrainerWorkoutProgram && (ctt.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType
                                                                                  || ctt.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType))
                           )).ToList();
                        }
                        if (trendingResponseList != null)
                        {
                            trendingResponseList = trendingResponseList.GroupBy(tt => tt.ChallengeId).Select(cc => cc.FirstOrDefault()).ToList();
                        }
                        trendingResponseList.ForEach(tr =>
                        {
                            TrendingChallengeListResponse objTrendingChallengeResponse = new TrendingChallengeListResponse();
                            if (tr.TempTargetZone != null && tr.TempTargetZone.Count > 0)
                            {
                                objTrendingChallengeResponse.TargetZone = string.Join(", ", tr.TempTargetZone);
                            }
                            if (tr.TempEquipments != null && tr.TempEquipments.Count > 0)
                            {
                                objTrendingChallengeResponse.Equipment = string.Join(", ", tr.TempEquipments);
                            }
                            objTrendingChallengeResponse.ChallengeName = tr.ChallengeName;

                            if (tr.ExeciseVideoDetails != null)
                            {
                                if (string.IsNullOrEmpty(tr.ExeciseVideoDetails.ExeciseUrl))
                                {
                                    objTrendingChallengeResponse.ExeciseVideoLink = !string.IsNullOrEmpty(tr.ExeciseVideoDetails.ExeciseName) ?
                                        CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + tr.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                                }
                                else
                                {
                                    objTrendingChallengeResponse.ExeciseVideoLink = tr.ExeciseVideoDetails.ExeciseUrl;
                                }
                                if (!string.IsNullOrEmpty(tr.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(tr.ExeciseVideoDetails.ExerciseThumnail))
                                {
                                    string thumnailName = tr.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                    string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                    string thumnailHeight = string.Empty;
                                    string thumnailWidth = string.Empty;
                                    objTrendingChallengeResponse.ThumbnailUrl = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                    CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                    objTrendingChallengeResponse.ThumbNailHeight = thumnailHeight;
                                    objTrendingChallengeResponse.ThumbNailWidth = thumnailWidth;
                                }
                                else
                                {
                                    objTrendingChallengeResponse.ThumbnailUrl = tr.ExeciseVideoDetails.ExerciseThumnail;
                                }
                            }
                            objTrendingChallengeResponse.IsSubscription = tr.IsSubscription;
                            int featuredTypeId = tr.ChallengeSubTypeId;
                            objTrendingChallengeResponse.ChallengeSubTypeId = featuredTypeId;
                            objTrendingChallengeResponse.ChallengeId = tr.ChallengeId;
                            objTrendingChallengeResponse.DifficultyLevel = tr.DifficultyLevel;
                            objTrendingChallengeResponse.ChallengeDuration = tr.challengeDuration;
                            if (featuredTypeId > 0)
                            {
                                switch (featuredTypeId)
                                {
                                    case ConstantHelper.constWorkoutChallengeSubType:
                                        objTrendingChallengeResponse.TrendingType = TrendingType.Workout;
                                        break;
                                    case ConstantHelper.constWellnessChallengeSubType:
                                        objTrendingChallengeResponse.TrendingType = TrendingType.Wellness;
                                        break;
                                    case ConstantHelper.constProgramChallengeSubType:
                                        objTrendingChallengeResponse.TrendingType = TrendingType.Program;
                                        break;
                                    default:
                                        objTrendingChallengeResponse.TrendingType = TrendingType.FitnessTest;
                                        break;
                                }
                            }
                            if (tr.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                            {
                                objTrendingChallengeResponse.ProgramImageUrl = (string.IsNullOrEmpty(tr.ProgramImageUrl)) ? string.Empty :
                                    File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + tr.ProgramImageUrl)) ?
                                    CommonUtility.VirtualPath + Message.ProfilePicDirectory + tr.ProgramImageUrl : string.Empty;
                                objTrendingChallengeResponse.IsActiveProgram = dataContext.UserActivePrograms.Any(ap => ap.ProgramId == tr.ChallengeId && ap.UserCredId == cred.Id && !ap.IsCompleted);

                                string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + tr.ProgramImageUrl;
                                if (System.IO.File.Exists(filePath))
                                {
                                    using (Bitmap objBitmap = new Bitmap(filePath))
                                    {
                                        double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                        double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                        objTrendingChallengeResponse.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                        objTrendingChallengeResponse.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                                    }
                                }
                                else
                                {
                                    objTrendingChallengeResponse.Height = string.Empty;
                                    objTrendingChallengeResponse.Width = string.Empty;
                                }
                            }

                            trendingResponseeData.Add(objTrendingChallengeResponse);
                        });
                    }

                    return trendingResponseeData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamTrendingList  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get Team Selected Trending Category List based trainerId or selected no trainer teamIDs
        /// </summary>
        /// <param name="mastertrainer"></param>
        /// <returns></returns>
        public static List<TrendingCategory> GetTeamSelectedTrendingCategoryList(TrendingCategoryVM mastertrainer)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL Get GetTeamSelectedTrendingCategoryList");
                    List<int> trainerTeamIds = new List<int>();
                    if (!mastertrainer.IsNoTrainer)
                    {
                        trainerTeamIds = (from crd in dataContext.Credentials
                                          join tms in dataContext.TrainerTeamMembers
                                          on crd.Id equals tms.UserId
                                          orderby tms.RecordId ascending
                                          where crd.Id == mastertrainer.TrendingCredId && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                          select tms.TeamId).ToList();
                    }
                    else
                    {
                        trainerTeamIds = mastertrainer.SelectedTeamIds.Split(',').Select(Id => Convert.ToInt32(Id)).ToList();
                    }
                    // To get team trainer list
                    var trendingCategoryList = (from t in dataContext.TrendingCategory
                                                join tc in dataContext.TeamTrendingAssociations
                                                on t.TrendingCategoryId equals tc.TrendingCategoryId
                                                where trainerTeamIds.Contains(tc.TeamId)
                                                orderby t.TrendingName ascending
                                                select new TrendingCategory
                                                {
                                                    TrendingCategoryId = tc.TreamTrendingId,
                                                    TrendingCategoryName = t.TrendingName
                                                }).ToList();

                    return trendingCategoryList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamSelectedTrendingCategoryList  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
    }
}