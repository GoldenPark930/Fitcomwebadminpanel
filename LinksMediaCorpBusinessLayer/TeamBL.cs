namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System.Web;
    using AutoMapper;
    using System.Data.Entity;
    using LinksMediaCorpUtility.Resources;
    using System.Drawing;
    public class TeamBL
    {
        static object syncLock = new object();
        /// <summary>
        /// Function to join team under trainer
        /// </summary>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        /// 
        public static bool JoinTeam(TeamVM objTeamInfo)
        {
            StringBuilder traceLog = null;
            bool isSucceess = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        int teamId = objTeamInfo.TeamId;
                        traceLog.AppendLine("Start: JoinTeam" + teamId);
                        if (objTeamInfo.IsJoinByUniqueTeamId)
                        {
                            var teamdetails = dataContext.Teams.FirstOrDefault(t => t.UniqueTeamId == teamId);
                            if (teamdetails != null)
                            {
                                teamId = teamdetails.TeamId;
                            }
                            else
                            {
                                isSucceess = false;
                                return isSucceess;
                            }
                        }
                        if (teamId > 0)
                        {
                            Credentials credUser = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                            if (credUser.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                            {
                                tblUser objUser = dataContext.User.FirstOrDefault(u => u.UserId == credUser.UserId);
                                if (objUser != null)
                                {
                                    int oldUserJoinedteamId = objUser.TeamId;
                                    objUser.TeamId = teamId;
                                    objUser.ModifiedBy = credUser.Id;
                                    objUser.ModifiedDate = DateTime.Now;
                                    tblTrainerTeamMembers objTrainerTeamMember = dataContext.TrainerTeamMembers.Where(tm => tm.UserId == credUser.Id).FirstOrDefault();
                                    if (objTrainerTeamMember != null)
                                    {
                                        objTrainerTeamMember.TeamId = teamId;
                                        objTrainerTeamMember.ModifiedBy = credUser.Id;
                                        objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        objTrainerTeamMember = new tblTrainerTeamMembers();
                                        objTrainerTeamMember.TeamId = teamId;
                                        objTrainerTeamMember.UserId = credUser.Id;
                                        objTrainerTeamMember.CreatedBy = credUser.Id;
                                        objTrainerTeamMember.CreatedDate = DateTime.Now;
                                        objTrainerTeamMember.ModifiedBy = credUser.Id;
                                        objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                        dataContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                                    }
                                    List<int> teamtrainerList = (from tr in dataContext.TrainerTeamMembers
                                                                 join crd in dataContext.Credentials
                                                                 on tr.UserId equals crd.Id
                                                                 where crd.UserType == Message.UserTypeTrainer && tr.TeamId == teamId
                                                                 select crd.Id).Distinct().ToList();
                                    // Remove the Default team trainer following list from from user following list
                                    if (oldUserJoinedteamId > 0)
                                    {
                                        List<int> defaulttrainerList = (from tr in dataContext.TrainerTeamMembers
                                                                        join crd in dataContext.Credentials
                                                                        on tr.UserId equals crd.Id
                                                                        where crd.UserType == Message.UserTypeTrainer && tr.TeamId == oldUserJoinedteamId
                                                                        select crd.Id).Distinct().ToList();
                                        var defaultTeamtrainerFollowinglist = dataContext.Followings.Where(f => f.UserId == credUser.Id
                                            && f.IsManuallyFollow == false && defaulttrainerList.Contains(f.FollowUserId)).ToList();
                                        if (defaultTeamtrainerFollowinglist != null && defaultTeamtrainerFollowinglist.Count > 0)
                                        {
                                            dataContext.Followings.RemoveRange(defaultTeamtrainerFollowinglist);
                                            dataContext.SaveChanges();
                                        }
                                    }

                                    // Adding joined team trainer in following list
                                    List<tblFollowings> followingteamtrainers = new List<tblFollowings>();
                                    foreach (int teamtrainerID in teamtrainerList)
                                    {
                                        if (!dataContext.Followings.Any(f => f.UserId == credUser.Id && f.FollowUserId == teamtrainerID))
                                        {
                                            tblFollowings objFollowing = new tblFollowings();
                                            objFollowing.UserId = credUser.Id;
                                            objFollowing.FollowUserId = teamtrainerID;
                                            followingteamtrainers.Add(objFollowing);

                                        }
                                    }
                                    if (followingteamtrainers != null && followingteamtrainers.Count > 0)
                                    {
                                        dataContext.Followings.AddRange(followingteamtrainers);
                                    }
                                    // Joined team and sent to notification for team trainers
                                    dataContext.SaveChanges();
                                    string userFullName = string.Empty;
                                    userFullName = objUser.FirstName + " " + objUser.LastName;
                                    string NtType = NotificationType.TrainerJoinTeam.ToString();
                                    NotificationApiBL.SendJoinedTeamNotificationToTrainers(teamId, userFullName, NtType, credUser.UserId, credUser.UserType);
                                    isSucceess = true;
                                }
                            }
                            else if (credUser.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                            {
                                tblTrainer objtrainer = dataContext.Trainer.FirstOrDefault(u => u.TrainerId == credUser.UserId);
                                if (objtrainer != null)
                                {
                                    objtrainer.TeamId = teamId;
                                    if (dataContext.SaveChanges() > 0)
                                    {
                                        // Mofified by 08/26/2015 for remove the user from thiers team member list.
                                        tblTrainerTeamMembers objTrainerTeamMember = dataContext.TrainerTeamMembers
                                            .Where(tm => tm.TeamId == teamId && tm.UserId == credUser.Id).FirstOrDefault();
                                        if (objTrainerTeamMember != null)
                                        {
                                            objTrainerTeamMember.TeamId = teamId;
                                            dataContext.SaveChanges();
                                        }
                                        isSucceess = true;
                                    }
                                }

                            }
                            dbTran.Commit();
                        }
                        else
                        {
                            isSucceess = false;
                        }
                        return isSucceess;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: JoinTeam  --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        traceLog = null;
                    }
                }
            }
        }
        /// <summary>
        /// Get Nutrition Image  and hyper link details
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static List<NutritionVM> GetTeamNutritionList()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<NutritionVM> objNutrition = new List<NutritionVM>();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamNutritionList for current user");
                    List<int> teamIds = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    switch (cred.UserType)
                    {
                        case ConstantHelper.constuser:
                            teamIds = (from usr in dataContext.User
                                       join crd in dataContext.Credentials
                                       on usr.UserId equals crd.UserId
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                       select usr.TeamId).ToList();
                            break;
                        case ConstantHelper.consttrainer:
                            teamIds = (from crd in dataContext.Credentials
                                       join tms in dataContext.TrainerTeamMembers
                                       on crd.Id equals tms.UserId
                                       orderby tms.RecordId ascending
                                       where crd.Id == cred.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                       select tms.TeamId).ToList();
                            break;
                    }
                    if (teamIds != null && teamIds.Count > 0)
                    {
                        var nutritionList = (from tm in dataContext.Teams
                                             where teamIds.Contains(tm.TeamId)
                                             select new
                                             {
                                                 Nutrition1PicUrl = tm.Nutrition1ImageUrl,
                                                 Nutrition1HeyerLink = tm.Nutrition1HyerLink,
                                                 Nutrition2PicUrl = tm.Nutrition2ImageUrl,
                                                 Nutrition2HeyerLink = tm.Nutrition2HyerLink
                                             }).ToList();

                        if (nutritionList != null)
                        {
                            nutritionList.ForEach(nt =>
                                {
                                    if (!string.IsNullOrEmpty(nt.Nutrition1PicUrl))
                                    {
                                        var firstNutritionVM = GetNutricationData(nt.Nutrition1PicUrl, nt.Nutrition1HeyerLink);
                                        if (firstNutritionVM != null)
                                        {
                                            objNutrition.Add(firstNutritionVM);

                                        }
                                    }
                                    if (!string.IsNullOrEmpty(nt.Nutrition2PicUrl))
                                    {
                                        var secondNutritionVM = GetNutricationData(nt.Nutrition2PicUrl, nt.Nutrition2HeyerLink);
                                        if (secondNutritionVM != null)
                                        {
                                            objNutrition.Add(secondNutritionVM);
                                        }
                                    }

                                });
                        }
                    }
                    return objNutrition;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamNutritionList()  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get Nutrication Data
        /// </summary>
        /// <param name="nutritionPicUrl"></param>
        /// <param name="nutritionHyperLink"></param>
        /// <returns></returns>
        private static NutritionVM GetNutricationData(string nutritionPicUrl, string nutritionHyperLink)
        {
            NutritionVM nutritionVM = new NutritionVM();
            try
            {
                string nutritionfilePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + nutritionPicUrl;
                nutritionVM.NutritionPicUrl = (!string.IsNullOrEmpty(nutritionPicUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                Message.ProfilePicDirectory + nutritionPicUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + nutritionPicUrl : string.Empty;
                nutritionVM.NutritionHyperLink = string.IsNullOrEmpty(nutritionHyperLink) ? string.Empty : new UriBuilder(nutritionHyperLink).Uri.AbsoluteUri;
                if (System.IO.File.Exists(nutritionfilePath))
                {
                    using (Bitmap objBitmap = new Bitmap(nutritionfilePath))
                    {
                        double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.InvariantCulture);
                        double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.InvariantCulture);
                        nutritionVM.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.InvariantCulture) : string.Empty;
                        nutritionVM.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.InvariantCulture) : string.Empty;
                    }
                }
                else
                {
                    nutritionVM.Height = string.Empty;
                    nutritionVM.Width = string.Empty;
                }
                return nutritionVM;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Send Join Notification to user Trainer
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceToken"></param>
        /// <param name="notificationType"></param>
        /// <param name="fullName"></param>
        /// <param name="deviceType"></param>
        /// <param name="totalpendingNotification"></param>
        /// <param name="certificate"></param>
        public static bool SendJoinNotification(int joinTeamUserId, int userId, string deviceToken, string notificationType,
            string fullName, string deviceType, int totalpendingNotification, string teamName, byte[] certificate)
        {
            StringBuilder traceLog = new StringBuilder();
            traceLog.AppendLine("Start the trace in Lock");
            //  string NotificationSentStatus = string.Empty;
            LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            lock (syncLock)
            {
                traceLog.AppendLine("Enter the trace in Lock");
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                int totalbudget = CommonWebApiBL.GetTotalUSerNotification(userId, deviceToken);
                bool isSendNotification = PushNotificationBL.ValidateAndSendNotification(userId, deviceToken, notificationType,
                    fullName, deviceType, totalpendingNotification, certificate, totalbudget, teamName, joinTeamUserId);
                traceLog.AppendLine("End the trace in Lock");
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                return isSendNotification;
            }
        }

        /// <summary>
        /// Function to join team under trainer
        /// </summary>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/18/2015
        /// </devdoc>
        public static bool LeaveTeam()
        {
            StringBuilder traceLog = null;
            bool flag = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: LeaveTeam");
                    int teamId = -1;
                    // tblUser objUser = null;
                    tblTrainer objtrainer = null;
                    Credentials credUser = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    var defaultTeamDetails = dataContext.Teams.FirstOrDefault(tm => tm.IsDefaultTeam);
                    int defaultTeamID = 0;
                    if (credUser.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        tblUser objUser = dataContext.User.FirstOrDefault(u => u.UserId == credUser.UserId);
                        if (objUser != null)
                        {
                            teamId = objUser.TeamId;
                            // List of trainers in current team
                            List<int> teamtrainerList = (from tr in dataContext.TrainerTeamMembers
                                                         join cr in dataContext.Credentials
                                                         on tr.UserId equals cr.Id
                                                         where cr.UserType == Message.UserTypeTrainer && tr.TeamId == teamId
                                                         select cr.Id).Distinct().ToList();
                            var userfollowingList = dataContext.Followings.Where(f => f.IsManuallyFollow == false && teamtrainerList.Contains(f.FollowUserId) && f.UserId == credUser.Id).ToList();
                            if (userfollowingList != null)
                            {
                                dataContext.Followings.RemoveRange(userfollowingList);
                                dataContext.SaveChanges();
                            }
                            objUser.TeamId = defaultTeamDetails != null ? defaultTeamDetails.TeamId : 0;
                            objUser.PersonalTrainerCredId = 0;
                            if (defaultTeamDetails != null)
                            {
                                objUser.TeamId = defaultTeamDetails.TeamId;
                                defaultTeamID = objUser.TeamId;
                            }
                            dataContext.SaveChanges();
                        }

                        // Mofified by 08/26/2015 for remove the user from thiers team member list.
                        tblTrainerTeamMembers objTrainerTeamMember = dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == teamId && tm.UserId == credUser.Id).FirstOrDefault();
                        if (objTrainerTeamMember != null)
                        {
                            objTrainerTeamMember.TeamId = defaultTeamID;
                            dataContext.SaveChanges();
                        }
                        //// Get tariners list from default team
                        List<int> defautteamtrainerList = (from tr in dataContext.TrainerTeamMembers
                                                           join cr in dataContext.Credentials
                                                           on tr.UserId equals cr.Id
                                                           where cr.UserType == Message.UserTypeTrainer && tr.TeamId == defaultTeamID
                                                           select cr.Id).Distinct().ToList();
                        //Follow of all trainers of Default team
                        List<tblFollowings> usersFollowingsList = new List<tblFollowings>();
                        foreach (int tranercrdId in defautteamtrainerList)
                        {
                            if (!dataContext.Followings.Any(f => f.UserId == credUser.Id && f.FollowUserId == tranercrdId))
                            {
                                tblFollowings objtblFollowings = new tblFollowings();
                                objtblFollowings.FollowUserId = tranercrdId;
                                objtblFollowings.UserId = credUser.Id;
                                objtblFollowings.IsManuallyFollow = false;
                                usersFollowingsList.Add(objtblFollowings);
                            }
                        }
                        dataContext.Followings.AddRange(usersFollowingsList);
                        dataContext.SaveChanges();
                        flag = true;

                    }
                    else if (credUser.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        objtrainer = dataContext.Trainer.FirstOrDefault(u => u.TrainerId == credUser.UserId);
                        teamId = (objtrainer != null) ? objtrainer.TeamId : 0;
                        if (defaultTeamDetails != null)
                        {
                            objtrainer.TeamId = teamId;
                        }
                        dataContext.SaveChanges();

                        // Mofified by 08/26/2015 for remove the user from thiers team member list.
                        tblTrainerTeamMembers objTrainerTeamMember = dataContext.TrainerTeamMembers.Where(tm => tm.TeamId == teamId && tm.UserId == credUser.Id).FirstOrDefault();
                        if (objTrainerTeamMember != null)
                        {
                            objTrainerTeamMember.TeamId = teamId;
                            dataContext.SaveChanges();
                        }
                        flag = true;

                    }
                    return flag;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: LeaveTeam  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get team credId based on team Id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static int GetTeamCredIdByTeamId(int teamId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTeamCredIdByTeamId" + Convert.ToString(teamId));
                    return (from tm in dataContext.Teams
                            join crd in dataContext.Credentials
                             on tm.TeamId equals crd.UserId
                            where crd.UserType == Message.UserTypeTeam && tm.TeamId == teamId
                            select crd.Id
                           ).FirstOrDefault();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetTeamCredIdByTeamId  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        /// <summary>
        /// Function to Follow/Unfollow user
        /// </summary>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
        /// </devdoc>       
        public static bool FollowUnfollow(FollowUserVM modelFollow)
        {
            StringBuilder traceLog = null;
            bool isFollowUnfollow = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: FollowUnfollow");
                    Credentials credSubject = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblCredentials credTarget = dataContext.Credentials.FirstOrDefault(ct => ct.UserId == modelFollow.UserId && ct.UserType == modelFollow.UserType);
                    tblFollowings objFollowing = dataContext.Followings.FirstOrDefault(f => f.UserId == credSubject.Id && f.FollowUserId == credTarget.Id);
                    if (modelFollow.IsFollow && objFollowing == null)
                    {
                        if (credSubject.Id != credTarget.Id)
                        {
                            objFollowing = new tblFollowings();
                            objFollowing.UserId = credSubject.Id;
                            objFollowing.FollowUserId = credTarget.Id;
                            objFollowing.IsManuallyFollow = true;
                            dataContext.Followings.Add(objFollowing);
                        }
                        isFollowUnfollow = true;
                    }
                    else
                    {
                        if (objFollowing != null)
                            dataContext.Followings.Remove(objFollowing);
                    }
                    dataContext.SaveChanges();

                    // Send the User Notification to user on result boomed
                    if (isFollowUnfollow && modelFollow.UserId > 0 && !string.IsNullOrEmpty(modelFollow.UserType)
                        && !(credSubject.UserType == modelFollow.UserType && credSubject.UserId == modelFollow.UserId))
                    {
                        NotificationApiBL.SendNotificationToALLNType(modelFollow.UserId, modelFollow.UserType, NotificationType.Following.ToString(), credSubject);
                    }
                    return true;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: FollowUnfollow  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to Follow/Unfollow user to team
        /// </summary>
        /// <param name="modelFollow"></param>
        /// <returns></returns>
        public static bool FollowUnfollowTeam(FollowUserVM modelFollow)
        {
            StringBuilder traceLog = null;
            bool isFollowUnfollow = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: FollowUnfollowTeam of Team-UserId" + Convert.ToString(modelFollow.UserId) + ",UserType-" + modelFollow.UserType);
                    Credentials credSubject = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    tblCredentials credTarget = dataContext.Credentials.FirstOrDefault(ct => ct.UserId == modelFollow.UserId && ct.UserType == modelFollow.UserType);

                    var teamtrainerListdata = (from tr in dataContext.TrainerTeamMembers
                                               join crd in dataContext.Credentials
                                               on tr.UserId equals crd.Id
                                               where crd.UserType == Message.UserTypeTrainer && tr.TeamId == credTarget.UserId
                                               select new
                                               {
                                                   crd.Id,
                                                   crd.UserId,
                                               }).Distinct().ToList();
                    List<int> teamtrainerList = null;
                    if (teamtrainerListdata != null)
                    {
                        teamtrainerList = teamtrainerListdata.Select(tt => tt.Id).ToList();
                    }
                    List<tblFollowings> objFollowing = dataContext.Followings.Where(f => f.UserId == credSubject.Id && teamtrainerList.Contains(f.FollowUserId)).ToList();
                    // Follow all trainers of team
                    tblFollowings following;
                    if (modelFollow.IsFollow && objFollowing == null)
                    {
                        objFollowing = new List<tblFollowings>();
                        foreach (int teamtrainerID in teamtrainerList)
                        {
                            if (credSubject.Id != teamtrainerID)
                            {
                                if (!dataContext.Followings.Any(f => f.UserId == credSubject.Id && f.FollowUserId == teamtrainerID))
                                {
                                    following = new tblFollowings();
                                    following.UserId = credSubject.Id;
                                    following.FollowUserId = teamtrainerID;
                                    following.IsManuallyFollow = true;
                                    objFollowing.Add(following);
                                }
                            }
                        }
                        // Follow the team also
                        if (!dataContext.Followings.Any(f => f.UserId == credSubject.Id && f.FollowUserId == credTarget.Id))
                        {
                            following = new tblFollowings();
                            following.UserId = credSubject.Id;
                            following.FollowUserId = credTarget.Id;
                            following.IsManuallyFollow = true;
                            objFollowing.Add(following);
                        }
                        if (objFollowing != null)
                        {
                            dataContext.Followings.AddRange(objFollowing);
                        }
                        isFollowUnfollow = true;
                    }
                    else if (modelFollow.IsFollow && objFollowing != null)
                    {
                        objFollowing = new List<tblFollowings>();

                        var followingtrainers = objFollowing.Select(tt => tt.FollowUserId).ToList();
                        List<int> remaingtrainers = teamtrainerList.Where(s => !followingtrainers.Contains(s)).ToList();

                        foreach (int teamtrainerID in remaingtrainers)
                        {
                            if (credSubject.Id != teamtrainerID)
                            {
                                if (!dataContext.Followings.Any(f => f.UserId == credSubject.Id && f.FollowUserId == teamtrainerID))
                                {
                                    following = new tblFollowings();
                                    following.UserId = credSubject.Id;
                                    following.FollowUserId = teamtrainerID;
                                    following.IsManuallyFollow = true;
                                    objFollowing.Add(following);
                                }
                            }
                        }
                        //                       
                        if (!dataContext.Followings.Any(f => f.UserId == credSubject.Id && f.FollowUserId == credTarget.Id))
                        {
                            following = new tblFollowings();
                            following.UserId = credSubject.Id;
                            following.FollowUserId = credTarget.Id;
                            following.IsManuallyFollow = true;
                            objFollowing.Add(following);

                        }
                        if (objFollowing != null)
                        {
                            dataContext.Followings.AddRange(objFollowing);
                        }
                        isFollowUnfollow = true;
                    }
                    else if (modelFollow.IsFollow == false)
                    {
                        objFollowing = new List<tblFollowings>();
                        if (dataContext.Followings.Any(f => f.UserId == credSubject.Id && f.FollowUserId == credTarget.Id))
                        {
                            following = dataContext.Followings.FirstOrDefault(f => f.UserId == credSubject.Id && f.FollowUserId == credTarget.Id);
                            objFollowing.Add(following);
                            dataContext.Followings.RemoveRange(objFollowing);
                        }
                        isFollowUnfollow = false;
                    }
                    dataContext.SaveChanges();
                    // Send the User Notification to Trainer when user join the team
                    foreach (var trainer in teamtrainerListdata)
                    {
                        if (isFollowUnfollow && modelFollow.UserId > 0 && !string.IsNullOrEmpty(modelFollow.UserType))
                        {
                            NotificationApiBL.SendNotificationToALLNType(trainer.UserId, Message.UserTypeTrainer, NotificationType.Following.ToString(), credSubject);
                        }
                    }
                    return true;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: FollowUnfollowTeam  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get specialization list
        /// </summary>
        /// <returns>List<Specialization></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/26/2015
        /// </devdoc>
        public static List<Specialization> GetSpecializationList()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetSpecializationList");
                    List<Specialization> objSpecilizationList = (from s in dataContext.Specialization
                                                                 select new Specialization
                                                                 {
                                                                     SpecializationId = s.SpecializationId,
                                                                     SpecializationName = s.SpecializationName
                                                                 }).ToList();
                    Specialization allSpecilization = new Specialization();
                    allSpecilization.SpecializationId = -1;
                    allSpecilization.SpecializationName = Message.All;
                    objSpecilizationList.Insert(0, allSpecilization);
                    return objSpecilizationList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetSpecializationList  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to post Share message text on my team section
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/26/2015
        /// </devdoc>
        public static ViewPostVM PostShare(ProfilePostVM<TextMessageStream> message)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
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
                    objMessageStream.TargetType = Message.MyTeamTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    objMessageStream.TargetId = objCred.UserType.Equals(Message.UserTypeTrainer) ? dataContext.Trainer
                        .Where(tt => tt.TrainerId == objCred.UserId).Select(tt => tt.TeamId).FirstOrDefault() :
                                                objCred.UserType.Equals(Message.UserTypeTrainer) ?
                                                (from usr in dataContext.User
                                                 where usr.UserId == objCred.UserId
                                                 select usr.TeamId).FirstOrDefault() : 0;
                    objMessageStream.IsImageVideo = message.IsImageVideo;
                    dataContext.MessageStraems.Add(objMessageStream);
                    dataContext.SaveChanges();
                    if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        // User Notification when user post message on other user or trainer post data their team member                       
                        NotificationApiBL.SendTrainerTeamsUserNotificationByTrainer(objCred.UserId, objMessageStream.MessageStraemId);
                    }
                    ViewPostVM objViewPostVM = new ViewPostVM()
                    {
                        PostId = objMessageStream.MessageStraemId,
                        Message = objMessageStream.Content,
                        PostedDate = objMessageStream.PostedDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff"),
                       // PostedDate = CommonWebApiBL.GetDateTimeFormat(objMessageStream.PostedDate),
                        BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == objMessageStream.MessageStraemId),
                        CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == objMessageStream.MessageStraemId)
                    };
                    if (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = (from usr in dataContext.User
                                    join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                    where crd.UserType == Message.UserTypeUser && crd.Id == objMessageStream.SubjectId
                                    select new
                                    {
                                        usr.FirstName,
                                        usr.LastName,
                                        usr.UserImageUrl,
                                        crd.UserId,
                                        crd.UserType
                                    }).FirstOrDefault();
                        if (user != null)
                        {
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(user.UserImageUrl)) ? CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + user.UserImageUrl : string.Empty;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                            objViewPostVM.UserId = user.UserId;
                            objViewPostVM.UserType = user.UserType;
                        }
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = (from t in dataContext.Trainer
                                       join crd in dataContext.Credentials on t.TrainerId equals crd.UserId
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
                            objViewPostVM.PostedByImageUrl = (!string.IsNullOrEmpty(trainer.TrainerImageUrl)) ? CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + trainer.TrainerImageUrl : string.Empty;
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
        /// Post Following Share
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ViewPostVM PostFollowingShare(ProfilePostVM<TextMessageStream> message)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
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
                    objMessageStream.TargetType = Message.FollowingsTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    objMessageStream.IsImageVideo = message.IsImageVideo;
                    objMessageStream.TargetId = 0;
                    dataContext.MessageStraems.Add(objMessageStream);
                    dataContext.SaveChanges();
                    /// start from here -------------------------------------------------------------------------
                    ViewPostVM objViewPostVM = new ViewPostVM()
                    {
                        PostId = objMessageStream.MessageStraemId,
                        Message = objMessageStream.Content,
                        PostedDate = CommonWebApiBL.GetDateTimeFormat(objMessageStream.PostedDate),
                        BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == objMessageStream.MessageStraemId),
                        CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == objMessageStream.MessageStraemId)
                    };
                    if (objCred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = (from usr in dataContext.User
                                    join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                    where crd.Id == objMessageStream.SubjectId
                                    select new
                                    {
                                        usr.FirstName,
                                        usr.LastName,
                                        usr.UserImageUrl,
                                        crd.UserId,
                                        crd.UserType
                                    }).FirstOrDefault();
                        if (user != null)
                        {
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + user.UserImageUrl;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                            objViewPostVM.UserId = user.UserId;
                            objViewPostVM.UserType = user.UserType;
                        }
                    }
                    else if (objCred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var trainer = (from t in dataContext.Trainer
                                       join crd in dataContext.Credentials on t.TrainerId equals crd.UserId
                                       where crd.Id == objMessageStream.SubjectId
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
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + trainer.TrainerImageUrl;
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
        /// Get HomePostList based on Team Id
        /// </summary>
        /// <param name="objCredential"></param>
        /// <param name="teamId"></param>
        /// <param name="teamMemberIDS"></param>
        /// <param name="userfollowIds"></param>
        /// <returns></returns>
        public static List<RecentResultVM> GetHomePostList(Credentials objCredential, int teamId, List<int> teamMemberIDS, List<int> userfollowIds)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<RecentResultVM> objList = null;
                try
                {
                  traceLog = new StringBuilder();
                  traceLog.AppendLine("Start: TeamBL GetHomePostList()");
                 
                 
                    if (teamMemberIDS != null)
                    {
                        //teamMemberIDS.AddRange(teamMemberdetails);
                        // If current user is user the find primary tariner credetial ID and add in teammember list
                        if(teamMemberIDS.Count > 0 && (objCredential.UserType == Message.UserTypeUser || objCredential.UserType == Message.UserTypeTrainer))
                        {
                            int trainercredID = (from crd in dataContext.Credentials
                                                 where crd.UserId == teamId && crd.UserType.Equals(Message.UserTypeTeam)
                                                 select crd.Id).FirstOrDefault();
                            teamMemberIDS.Add(trainercredID);
                        }
                       
                    }
                    objList = (from m in dataContext.MessageStraems
                               join c in dataContext.Credentials on m.SubjectId equals c.Id
                               where m.IsImageVideo && m.IsNewsFeedHide != true
                               && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                               && (
                               ((teamMemberIDS.Contains(m.SubjectId) && m.TargetType != Message.UserTypeUser)
                               || (teamMemberIDS.Contains(m.SubjectId) && m.SubjectId == m.TargetId && m.TargetType == Message.UserTypeUser)) // By team memeber, post every thing except on profile and show post on their own profile only
                               || ((userfollowIds.Contains(m.SubjectId) && m.TargetType != Message.UserTypeUser)
                               || (userfollowIds.Contains(m.SubjectId) && m.SubjectId == m.TargetId && m.TargetType == Message.UserTypeUser)) // By Following memeber, post every thing except on profile and show post on their own profile only
                               || ((m.SubjectId == objCredential.Id && m.TargetType != Message.UserTypeUser)
                               || (m.SubjectId == objCredential.Id && m.SubjectId == m.TargetId && m.TargetType == Message.UserTypeUser)) // By current user, post every thing except on profile and show post on their own profile only
                               )
                               orderby m.PostedDate descending
                               select new RecentResultVM
                               {
                                   PostId = m.MessageStraemId,
                                   DbPostedDate = m.PostedDate,
                                   Message = m.Content,
                                   BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == m.MessageStraemId),
                                   CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == m.MessageStraemId),
                                   PostedBy = m.SubjectId,
                                   userID = c.UserId,
                                   UserCredID = c.Id,
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
                                   TargetType = m.TargetType,
                                   PostType = ConstantHelper.NewsFeedPost,
                               }).ToList();

                    foreach (var item in objList)
                    {
                        item.PostedDate = CommonWebApiBL.GetDateTimeFormat(item.DbPostedDate);
                        //Code For Getting Posted Pics 
                        item.PicList.ForEach(pic =>
                        {
                            pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl)
                                && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ?
                                CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;

                        });
                        string thumnailHeight, thumnailWidth;
                        //Code For Getting Posted Videos
                        item.VideoList.ForEach(vid =>
                        {
                            string thumnailFileName = string.IsNullOrEmpty(vid.VideoUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension)) ? string.Empty : vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
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
                    return objList;
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
        /// Get PersonalBestChallengeResult
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static PersonalChallengeVM GetTeamBestResultByChallenge(int challengeId, LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();

            PersonalChallengeVM topPC = null;
            try
            {
                traceLog.AppendLine("Start: GetPersonalBestChallengeResult in BL---- " + DateTime.Now.ToLongDateString());
                List<PersonalChallengeVM> resultList = (from uc in dataContext.UserChallenge
                                                        join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        where uc.ChallengeId == challengeId && (uc.Result != null || uc.Fraction != null)
                                                        select new PersonalChallengeVM
                                                        {
                                                            UserChallengeId = uc.Id,
                                                            Result = uc.Result,
                                                            Fraction = uc.Fraction,
                                                            CompletedDateDb = uc.AcceptedDate,
                                                            ChallengeSubTypeid = ct.ChallengeSubTypeId,
                                                            ResultUnit = ct.ResultUnit,
                                                            ResultUnitSuffix = uc.ResultUnit
                                                        }).ToList<PersonalChallengeVM>();
                resultList.ForEach(r =>
                {
                    r.CompletedDate = r.CompletedDateDb.Day + " " +
                    CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(r.CompletedDateDb.Month) + " " +
                    r.CompletedDateDb.ToString("yy");
                    r.Result = (string.IsNullOrEmpty(r.Result)) ? string.Empty : r.Result.Trim();
                });
                //////////////////////////////////ORDER Implementation//////////////////////////////////////////////////////
                var resultMethoddata = (from c in dataContext.Challenge
                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                        where c.ChallengeId == challengeId
                                        select new { ct.ResultUnit, ct.ChallengeSubTypeId }).FirstOrDefault();
                string resultMethod = string.Empty;
                if (resultMethod != null && !string.IsNullOrEmpty(resultMethoddata.ResultUnit))
                {
                    resultMethod = resultMethoddata.ResultUnit.Trim();
                    switch (resultMethod)
                    {
                        case ConstantHelper.constTime:
                            resultList.ForEach(r =>
                            {
                                    //r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(":", string.Empty));
                                    r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(":", string.Empty).Replace(".", string.Empty));
                                    //Code for HH:MM:SS And MM:SS format
                                    string tempResult = r.Result;
                                char[] splitChar = { ':' };
                                string[] spliResult = tempResult.Split(splitChar);
                                if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    r.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] :
                                        ConstantHelper.constTimeVariableUnit);
                                }
                                else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    r.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                }

                            });
                            resultList = resultMethoddata.ChallengeSubTypeId == 6 ?
                                     resultList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                     : resultList.OrderBy(k => k.TempOrderIntValue).ToList();
                            topPC = resultList.FirstOrDefault();
                            break;
                        case ConstantHelper.constReps:
                        case ConstantHelper.constWeight:
                        case ConstantHelper.constDistance:
                            resultList.ForEach(r =>
                            {
                                r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : r.Result.Replace(",", string.Empty);
                                r.TempOrderIntValue = string.IsNullOrEmpty(r.Result) ? 0 : (float)Convert.ToDouble(r.Result);
                            });
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            topPC = resultList.FirstOrDefault();
                            break;
                        case ConstantHelper.conRounds:
                        case ConstantHelper.constInterval:
                            resultList.ForEach(r =>
                            {
                                if (!string.IsNullOrEmpty(r.Fraction))
                                {
                                    string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                    r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ?
                                        ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                }
                                else
                                {
                                    r.TempOrderIntValue = (float)Convert.ToInt16(r.Result);
                                }
                                r.Result = r.Result + " " + r.Fraction;
                            });
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            topPC = resultList.FirstOrDefault();
                            break;
                    }
                }

                return topPC;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetPersonalBestChallengeResult in BL : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Global Personal Best Result based on challenge Id and user userCred Id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="userCredId"></param>
        /// <returns></returns>
        public static PersonalChallengeVM GetGlobalPersonalBestResult(int challengeId, int userCredId, LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            PersonalChallengeVM topPC = null;
            try
            {
                traceLog.AppendLine("Start: GetGlobalPersonalBestResult in BL---- " + DateTime.Now.ToLongDateString());
                List<PersonalChallengeVM> resultList = (from uc in dataContext.UserChallenge
                                                        join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        where uc.ChallengeId == challengeId && uc.UserId == userCredId && (uc.Result != null || uc.Fraction != null)
                                                        // orderby uc.AcceptedDate descending
                                                        select new PersonalChallengeVM
                                                        {
                                                            UserChallengeId = uc.Id,
                                                            Result = uc.Result,
                                                            Fraction = uc.Fraction,
                                                            CompletedDateDb = uc.AcceptedDate,
                                                            ChallengeSubTypeid = ct.ChallengeSubTypeId,
                                                            ResultUnit = ct.ResultUnit,
                                                            ResultUnitSuffix = uc.ResultUnit
                                                        }).ToList<PersonalChallengeVM>();
                resultList.ForEach(r =>
                {
                    r.CompletedDate = r.CompletedDateDb.Day + " " +
                    CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(r.CompletedDateDb.Month) + " " +
                    r.CompletedDateDb.ToString("yy");
                    r.Result = (string.IsNullOrEmpty(r.Result)) ? string.Empty : r.Result.Trim();
                });
                //////////////////////////////////ORDER Implementation//////////////////////////////////////////////////////
                var resultMethoddata = (from c in dataContext.Challenge
                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                        where c.ChallengeId == challengeId
                                        select new { ct.ResultUnit, ct.ChallengeSubTypeId }).FirstOrDefault();
                string resultMethod = string.Empty;
                if (resultMethoddata != null && !string.IsNullOrEmpty(resultMethoddata.ResultUnit))
                {
                    resultMethod = resultMethoddata.ResultUnit.Trim();
                    switch (resultMethod)
                    {
                        case ConstantHelper.constTime:
                            resultList.ForEach(r =>
                            {
                                r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(":", string.Empty).Replace(".", string.Empty));
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
                            resultList = resultMethoddata.ChallengeSubTypeId == 6 ?
                                     resultList.OrderByDescending(k => k.TempOrderIntValue).ToList()
                                     : resultList.OrderBy(k => k.TempOrderIntValue).ToList();

                            topPC = resultList.FirstOrDefault();
                            break;
                        case ConstantHelper.constReps:
                        case ConstantHelper.constWeight:
                        case ConstantHelper.constDistance:
                            resultList.ForEach(r =>
                            {
                                r.Result = string.IsNullOrEmpty(r.Result) ? string.Empty : r.Result.Replace(",", string.Empty);
                                r.TempOrderIntValue = string.IsNullOrEmpty(r.Result) ? 0 : (float)Convert.ToDouble(r.Result);
                            });
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            topPC = resultList.FirstOrDefault();
                            break;
                        case ConstantHelper.conRounds:
                        case ConstantHelper.constInterval:
                            resultList.ForEach(r =>
                            {
                                if (!string.IsNullOrEmpty(r.Fraction))
                                {
                                    string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                    r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ?
                                        ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                }
                                else
                                {
                                    r.TempOrderIntValue = string.IsNullOrEmpty(r.Result) ? 0 : (float)Convert.ToInt16(r.Result);
                                }
                                r.Result = r.Result + " " + r.Fraction;
                            });
                            resultList = resultList.OrderByDescending(k => k.TempOrderIntValue).ToList();
                            topPC = resultList.FirstOrDefault();
                            break;
                    }
                }
                //  If object null the assigned the default values
                if (topPC == null)
                {
                    topPC = new PersonalChallengeVM();
                }
                return topPC;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetGlobalPersonalBestResult in BL : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Latest Result based on challengeId and usercredID
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="userCredId"></param>
        /// <returns></returns>
        public static string GetLatestResult(int challengeId, int userCredId, LinksMediaContext dataContext)
        {
            StringBuilder traceLog = new StringBuilder();
            string latestResult = string.Empty;
            try
            {
                traceLog.AppendLine("Start: GetGlobalPersonalBestResult in BL---- " + DateTime.Now.ToLongDateString());
                PersonalChallengeVM resultList = (from uc in dataContext.UserChallenge
                                                  join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                  join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                  where uc.ChallengeId == challengeId && uc.UserId == userCredId && (uc.Result != null || uc.Fraction != null)
                                                  orderby uc.AcceptedDate descending
                                                  select new PersonalChallengeVM
                                                  {
                                                      UserChallengeId = uc.Id,
                                                      Result = uc.Result,
                                                      Fraction = uc.Fraction,
                                                      CompletedDateDb = uc.AcceptedDate,
                                                      ChallengeSubTypeid = ct.ChallengeSubTypeId,
                                                      ResultUnit = ct.ResultUnit,
                                                      ResultUnitSuffix = uc.ResultUnit
                                                  }).FirstOrDefault();
                //  If object null the assigned the default values
                if (resultList != null)
                {
                    resultList.Result = (string.IsNullOrEmpty(resultList.Result)) ? string.Empty : resultList.Result.Trim();
                    //////////////////////////////////ORDER Implementation//////////////////////////////////////////////////////
                    var resultMethoddata = (from c in dataContext.Challenge
                                            join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                            where c.ChallengeId == challengeId
                                            select new { ct.ResultUnit, ct.ChallengeSubTypeId }).FirstOrDefault();
                    string resultMethod = string.Empty;
                    if (resultMethoddata != null && !string.IsNullOrEmpty(resultMethoddata.ResultUnit))
                    {
                        resultMethod = resultMethoddata.ResultUnit.Trim();
                        switch (resultMethod)
                        {
                            case ConstantHelper.constTime:
                                resultList.TempOrderIntValue = Convert.ToInt32(resultList.Result.Replace(":", string.Empty).Replace(".", string.Empty));
                                //Code for HH:MM:SS And MM:SS format
                                string tempResult = resultList.Result;
                                char[] splitChar = { ':' };
                                string[] spliResult = tempResult.Split(splitChar);
                                if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    resultList.Result = spliResult[1] + ConstantHelper.constColon + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                }
                                else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                {
                                    resultList.Result = spliResult[0] + ConstantHelper.constColon + spliResult[1];
                                }
                                latestResult = resultList.Result;
                                break;
                            case ConstantHelper.constReps:
                            case ConstantHelper.constWeight:
                            case ConstantHelper.constDistance:
                                latestResult = string.IsNullOrEmpty(resultList.Result) ? string.Empty : resultList.Result.Replace(",", string.Empty);
                                break;
                            case ConstantHelper.conRounds:
                            case ConstantHelper.constInterval:
                                resultList.Result = resultList.Result + " " + resultList.Fraction;
                                latestResult = resultList.Result;
                                break;
                        }
                    }
                }
                return latestResult;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End  GetGlobalPersonalBestResult in BL : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }

        }
        /// <summary>
        /// Get MyTeam RequestData
        /// </summary>
        public static MyTeamHomeVM GeMyTeamRequestData(int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<int> teamMemberIDS = new List<int>();
                List<int> userfollowIds = new List<int>();
                List<int> teamtrainercredId = new List<int>();
                MyTeamHomeVM myTeamHomedata = new MyTeamHomeVM();
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: TeamBL Get GeMyTeamRequestData");
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    myTeamHomedata.TrainerLatestActivity = ProfileBL.GetUserTrainerLatestActivity(objCredential.UserId, objCredential.UserType);
                    // Find the primary trainer ID
                    int teamId = 0;
                    if (objCredential.UserType.Equals(Message.UserTypeTrainer))
                    {
                        teamId = dataContext.Trainer.Where(tm => tm.TrainerId == objCredential.UserId).Select(tt => tt.TeamId).FirstOrDefault();
                    }
                    else if (objCredential.UserType.Equals(Message.UserTypeUser))
                    {
                        teamId = dataContext.User.Where(tm => tm.UserId == objCredential.UserId).Select(tt => tt.TeamId).FirstOrDefault();
                    }
                    TeamsVM teamdetails = (from t in dataContext.Teams
                                           join cr in dataContext.Credentials
                                           on t.TeamId equals cr.UserId
                                           join c in dataContext.Cities
                                           on t.City equals c.CityId
                                           where t.TeamId == teamId && cr.UserType == Message.UserTypeTeam
                                           select new TeamsVM
                                           {
                                               TeamName = t.TeamName,
                                               TeamId = t.TeamId,
                                               ImagePicUrl = t.ProfileImageUrl,
                                               ImagePremiumUrl = t.PremiumImageUrl,
                                               TeamMemberCount = teamId > 0 ? dataContext.TrainerTeamMembers.Where(tt => tt.TeamId == teamId).Select(g => g.UserId).Distinct().ToList().Count : 0,
                                               HashTag = t.TeamId > 0 ? dataContext.Teams.FirstOrDefault(tt => tt.TeamId == t.TeamId).TeamName : string.Empty,
                                               CredTeamId = cr.Id,
                                               City = c.CityName,
                                               State = t.State
                                           }).FirstOrDefault();
                    if (teamdetails != null)
                    {
                        teamdetails.TeamName = string.IsNullOrEmpty(teamdetails.TeamName) ? string.Empty : teamdetails.TeamName.Substring(1);
                        teamdetails.ImagePicUrl = string.IsNullOrEmpty(teamdetails.ImagePicUrl) ? string.Empty : CommonUtility.VirtualPath +
                            Message.ProfilePicDirectory + teamdetails.ImagePicUrl;
                        teamdetails.ImagePremiumUrl = string.IsNullOrEmpty(teamdetails.ImagePremiumUrl) ? string.Empty :
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + teamdetails.ImagePremiumUrl;
                        myTeamHomedata.Team = teamdetails;
                    }
                    var teamMemberdetails = (from usrtm in dataContext.TrainerTeamMembers
                                             join crd in dataContext.Credentials on usrtm.UserId equals crd.Id
                                             where usrtm.TeamId == teamId
                                             select new
                                             {
                                                 usrtm.UserId,
                                                 crd.UserType
                                             }).Distinct().ToList();
                    if (teamMemberdetails != null)
                    {
                        teamMemberIDS = teamMemberdetails.Select(tm => tm.UserId).Distinct().ToList();
                        teamtrainercredId = teamMemberdetails.Where(tm => tm.UserType == Message.UserTypeTrainer).Select(tm => tm.UserId).Distinct().ToList();
                    }
                    userfollowIds = (from fol in dataContext.Followings
                                     where fol.UserId == objCredential.Id || teamtrainercredId.Contains(fol.FollowUserId)
                                     select fol.FollowUserId).Distinct().ToList();
                    if (teamdetails != null)
                    {    // Add trainer credID for
                        teamMemberIDS.Add(objCredential.Id);
                    }
                    if (objCredential.UserType.Equals(Message.UserTypeTrainer,StringComparison.OrdinalIgnoreCase))
                    {
                        List<int> teamIds = (from crd in dataContext.Credentials
                                             join tms in dataContext.TrainerTeamMembers
                                             on crd.Id equals tms.UserId
                                             orderby tms.RecordId ascending
                                             where crd.Id == objCredential.Id && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                             select tms.TeamId).ToList();
                        List<int> allteammemeber = (from usrtm in dataContext.TrainerTeamMembers
                                                    join crd in dataContext.Credentials on usrtm.UserId equals crd.Id
                                                    where teamIds.Contains(usrtm.TeamId) && crd.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase)
                                                    select usrtm.UserId).Distinct().ToList();
                        teamMemberIDS.AddRange(allteammemeber);
                    }
                    //Query to get completed challenge list
                    IQueryable<RecentResultVM> resultQueryData = (from uc in dataContext.UserChallenge
                                                                  join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                                  join ct in dataContext.ChallengeType
                                                                  on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                  orderby uc.AcceptedDate descending
                                                                  where (teamMemberIDS.Contains(uc.UserId) || (userfollowIds.Contains(uc.UserId)))
                                                                  && (uc.Result != null || uc.Fraction != null) && c.IsActive == true
                                                                  select new RecentResultVM
                                                                  {
                                                                      ResultId = uc.Id,
                                                                      ChallengeId = uc.ChallengeId,
                                                                      ChallengeType = ct.ChallengeType,
                                                                      ChallengeName = c.ChallengeName,
                                                                      DifficultyLevel = c.DifficultyLevel,
                                                                      Duration = c.FFChallengeDuration,
                                                                      ChallengeSubTypeid = c.ChallengeSubTypeId,
                                                                      IsSubscription = c.IsSubscription,
                                                                      ExeciseVideoDetails = (from cexe in dataContext.CEAssociation
                                                                                             join ex in dataContext.Exercise
                                                                                             on cexe.ExerciseId equals ex.ExerciseId
                                                                                             where cexe.ChallengeId == c.ChallengeId
                                                                                             orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                                                                             select new ExeciseVideoDetail
                                                                                             {
                                                                                                 ExeciseName = ex.ExerciseName,
                                                                                                 ExeciseUrl = ex.V720pUrl,
                                                                                                 ExerciseThumnail = ex.ThumnailUrl,
                                                                                                 ChallengeExeciseId = cexe.RocordId
                                                                                             }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault(),
                                                                      TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                                        join bp in dataContext.BodyPart
                                                                                        on trzone.PartId equals bp.PartId
                                                                                        where trzone.ChallengeId == c.ChallengeId
                                                                                        select bp.PartName).Distinct().ToList<string>(),
                                                                      TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                                        join bp in dataContext.Equipments
                                                                                        on trzone.EquipmentId equals bp.EquipmentId
                                                                                        where trzone.ChallengeId == c.ChallengeId
                                                                                        select bp.Equipment).Distinct().ToList<string>(),
                                                                      Strength = dataContext.UserChallenge.Where(ucc => ucc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                      Result = uc.Result,
                                                                      Fraction = uc.Fraction,
                                                                      ResultUnit = ct.ResultUnit,
                                                                      ResultUnitSuffix = uc.ResultUnit,
                                                                      UserCredID = uc.UserId,
                                                                      PostType = ConstantHelper.ResultFeed,
                                                                      DbPostedDate = uc.AcceptedDate,
                                                                      BoomsCount = dataContext.ResultBooms.Count(b => b.ResultId == uc.Id),
                                                                      CommentsCount = dataContext.ResultComments.Count(cmnt => cmnt.Id == uc.Id),
                                                                      UserType = dataContext.Credentials.Where(ucrd => ucrd.Id == uc.UserId).FirstOrDefault().UserType,
                                                                      IsLoginUserBoom = dataContext.ResultBooms.Where(bm => bm.ResultId == uc.Id).Any(b => b.BoomedBy == objCredential.Id),
                                                                      IsLoginUserComment = dataContext.ResultComments.Where(cm => cm.Id == uc.Id).Any(b => b.CommentedBy == objCredential.Id),
                                                                  });

                    List<RecentResultVM> resultQuery = resultQueryData != null ? resultQueryData.ToList() : null;
                    if (resultQuery != null)
                    {
                        resultQuery = resultQuery.GroupBy(cr => cr.ResultId).Select(usr => usr.FirstOrDefault()).ToList();
                        resultQuery.ForEach(r =>
                        {
                            if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                            {
                                r.TargetZone = string.Join(", ", r.TempTargetZone);
                            }
                            r.TempTargetZone = null;
                            if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                            {
                                r.Equipments = string.Join(", ", r.TempEquipments);
                            }
                            r.TempEquipments = null;
                            if (r.ExeciseVideoDetails != null)
                            {
                                if (string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseUrl))
                                {
                                    r.ExeciseVideoLink = !string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) ? CommonUtility.VirtualFitComExercisePath +
                                        Message.ExerciseVideoDirectory + r.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                                }
                                else
                                {
                                    r.ExeciseVideoLink = r.ExeciseVideoDetails.ExeciseUrl;
                                }
                                if (!string.IsNullOrEmpty(r.ExeciseVideoDetails.ExerciseThumnail))
                                {
                                    r.ThumbnailUrl = r.ExeciseVideoDetails.ExerciseThumnail;
                                }
                                else
                                {
                                    r.ThumbnailUrl = string.Empty;
                                }
                                r.ThumbNailHeight = string.Empty;
                                r.ThumbNailWidth = string.Empty;
                            }

                        });
                    }
                    List<RecentResultVM> objList = new List<RecentResultVM>();
                    // Get the Team Post List
                    objList = TeamBL.GetHomePostList(objCredential, teamId, teamMemberIDS, userfollowIds);

                    if (resultQuery != null && objList != null)
                    {
                        resultQuery = resultQuery != null ?resultQuery.Union(objList).ToList(): objList;
                    }
                    else if (resultQuery == null && objList != null)
                    {
                        resultQuery = new List<RecentResultVM>();
                        resultQuery = objList;
                    }
                    if (resultQuery != null)
                    {
                        // Group the completed challenge based  credID and get lated result
                        resultQuery = resultQuery.OrderByDescending(item => item.DbPostedDate).ToList();
                        int totalcount = resultQuery.Count;
                        resultQuery = (from l in resultQuery
                                       orderby l.DbPostedDate descending
                                       select l).Skip(startIndex).Take(endIndex - startIndex).ToList();

                        if ((totalcount) > endIndex)
                        {
                            myTeamHomedata.IsMoreAvailable = true;
                        }
                        resultQuery.ForEach(r =>
                        {
                            r.isWellness = r.ChallengeSubTypeid == ConstantHelper.constWellnessChallengeSubType;
                            r.VideoList = r.VideoList != null ? r.VideoList : new List<VideoInfo>();
                            r.PicList = r.PicList != null ? r.PicList : new List<PicsInfo>();
                            r.TempTargetZone = null;
                            // Find the user details based on user credId and user Type
                            ProfileDetails userdetails = ProfileApiBL.GetProfileDetailsByCredId(r.UserCredID, r.UserType);
                            if (userdetails != null)
                            {
                                r.UserName = userdetails.UserName;
                                r.UserImageUrl = userdetails.ProfileImageUrl;
                                r.userID = userdetails.UserId;
                            }
                            r.DbPostedDate = r.DbPostedDate.ToUniversalTime();
                            string resultMethod = string.Empty;
                            if (!string.IsNullOrEmpty(r.ResultUnit) && r.PostType == ConstantHelper.ResultFeed)
                            {
                                resultMethod = r.ResultUnit.Trim();
                                // Find all result for latest  result submit  result
                                PersonalChallengeVM personalbestresult = GetGlobalPersonalBestResult(r.ChallengeId, r.UserCredID, dataContext);
                                switch (resultMethod)
                                {
                                    case ConstantHelper.constTime:
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = r.Result.Trim();
                                        }
                                        r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(":", string.Empty).Replace(".", string.Empty));
                                        //Code for HH:MM:SS And MM:SS format
                                        string tempResult = string.Empty;
                                        tempResult = r.Result;
                                        char[] splitChar = { ':' };
                                        string[] spliResult = tempResult.Split(splitChar);
                                        if (spliResult[0].Equals(ConstantHelper.constTimeVariableUnit))
                                        {
                                            r.Result = spliResult[1] + ":" + (spliResult.Length > 2 ? spliResult[2] : ConstantHelper.constTimeVariableUnit);
                                        }
                                        else if (spliResult.Length > 2 && spliResult[2].Equals(ConstantHelper.constTimeVariableUnit))
                                        {
                                            r.Result = spliResult[0] + ":" + spliResult[1];
                                        }

                                        // Store Persoan Best
                                        if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                                        {
                                            r.PersonalBestResult = personalbestresult.Result.Trim();
                                        }
                                        // Check user personal result or not based on challege ID
                                        if (r.ChallengeSubTypeid == 6)
                                        {
                                            r.IsRecentChallengUserBest = (r.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                                        }
                                        else
                                        {
                                            r.IsRecentChallengUserBest = (r.TempOrderIntValue > personalbestresult.TempOrderIntValue) ? false : true;
                                        }
                                        break;
                                    case ConstantHelper.constReps:
                                    case ConstantHelper.constWeight:
                                    case ConstantHelper.constDistance:
                                        r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToDouble(r.Result.Replace(",", string.Empty));
                                        // Store Persoan Best
                                        if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                                        {
                                            r.PersonalBestResult = personalbestresult.Result.Replace(",", string.Empty).Trim();
                                        }
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = r.Result.Replace(",", string.Empty).Trim();
                                        }
                                        r.IsRecentChallengUserBest = (r.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                                        break;
                                    case ConstantHelper.conRounds:
                                    case ConstantHelper.constInterval:
                                        if (!string.IsNullOrEmpty(r.Fraction))
                                        {
                                            string[] arrString = r.Fraction.Split(new char[1] { '/' });
                                            r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ?
                                                ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
                                        }
                                        else
                                        {
                                            r.TempOrderIntValue = (string.IsNullOrEmpty(r.Result)) ? 0 : (float)Convert.ToInt16(r.Result);
                                        }
                                        if (personalbestresult != null && !string.IsNullOrEmpty(personalbestresult.Result))
                                        {
                                            r.PersonalBestResult = personalbestresult.Result.Trim();
                                        }
                                        if (!string.IsNullOrEmpty(r.Result))
                                        {
                                            r.Result = r.Result.Trim();
                                        }
                                        r.IsRecentChallengUserBest = (r.TempOrderIntValue >= personalbestresult.TempOrderIntValue) ? true : false;
                                        r.Result = r.Result + " " + r.Fraction;
                                        break;
                                }
                            }
                            else
                            {
                                r.Result = "";
                                r.ResultUnit = "";

                            }
                            r.ChallengeType = string.IsNullOrEmpty(r.ChallengeType) ? string.Empty : r.ChallengeType.Split(' ')[0];
                        });
                        myTeamHomedata.TeamRecentChallenge = resultQuery;
                    }
                    return myTeamHomedata;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GeMyTeamRequestData  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get all teams from database
        /// </summary>
        /// <returns></returns>
        public static List<ViewTeams> GetALLTeams(string serachTeam=null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetALLTeams for retrieving all teams from database ");

                    List<ViewTeams> trainers = (from T in dataContext.Teams
                                                join S in dataContext.States on T.State equals S.StateCode
                                                join C in dataContext.Cities on T.City equals C.CityId
                                                orderby T.ModifiedDate descending
                                                select new ViewTeams
                                                {
                                                    TeamName = T.TeamName,
                                                    CreatedDate = T.CreatedDate,
                                                    TeamId = T.TeamId,
                                                    Address = C.CityName + ", " + S.StateName,
                                                    TeamCount = T.TeamId > 0 ? dataContext.TrainerTeamMembers
                                                                               .Where(t => t.TeamId == T.TeamId).Select(g => g.UserId).Distinct().ToList().Count : 0,
                                                    PhoneNumber = T.PhoneNumber,
                                                    Email = T.EmailId,
                                                    IsDefaultTeam = T.IsDefaultTeam,
                                                    UniqueTeamId = T.UniqueTeamId
                                                }).ToList<ViewTeams>();
                    if (!string.IsNullOrEmpty(serachTeam))
                    {
                        serachTeam = serachTeam.ToUpper(CultureInfo.InvariantCulture);
                        trainers = trainers.Where(usr =>  (usr.TeamName.ToUpper(CultureInfo.InvariantCulture).IndexOf(serachTeam, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderBy(tm => tm.TeamName).ToList();
                    }
                    trainers.ForEach(tt =>
                        {
                            tt.TeamName = string.IsNullOrEmpty(tt.TeamName) ? string.Empty : tt.TeamName.Substring(1);
                        });
                    return trainers;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetALLTeams  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


      
        /// <summary>
        /// Get Max UniqueTeamId while creating team
        /// </summary>
        /// <returns></returns>
        private static int GetMaxUniqueTeamId()
        {
            int maxUniqueTeamId = 0;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    maxUniqueTeamId = Convert.ToInt32(dataContext.Teams.Max(x => x.UniqueTeamId));
                    return maxUniqueTeamId;
                }
                catch
                {
                    return ConstantHelper.constTeamStartUniqueId;
                }
            }
        }
        /// <summary>
        /// Submit Team by admin
        /// </summary>
        /// <param name="objCreateTeamVM"></param>
        /// <returns></returns>
        public static int SubmitTeam(CreateTeamVM objCreateTeamVM)
        {
            StringBuilder traceLog = new StringBuilder();
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        objEncrypt = new EncryptDecrypt();
                        traceLog.AppendLine("Start: SubmitTeam for creating trainer");
                        if (!string.IsNullOrEmpty(objCreateTeamVM.TeamName))
                        {
                            objCreateTeamVM.TeamName = objCreateTeamVM.TeamName.Trim();
                        }
                        int maxUniqueTeamId = GetMaxUniqueTeamId();
                        if (!(maxUniqueTeamId > 0))
                        {
                            maxUniqueTeamId = ConstantHelper.constTeamStartUniqueId;
                        }
                        Mapper.CreateMap<CreateTeamVM, tblTeam>();
                        tblTeam objTeam =
                            Mapper.Map<CreateTeamVM, tblTeam>(objCreateTeamVM);
                        objTeam.UniqueTeamId = maxUniqueTeamId + 1;
                        string teamName = ConstantHelper.constTeamNameHashTag + objCreateTeamVM.TeamName;
                        objTeam.TeamName = teamName;
                        objTeam.CreatedDate = DateTime.Now;
                        objTeam.ModifiedDate = objCreateTeamVM.CreatedDate;
                        objTeam.ProfileImageUrl = objCreateTeamVM.ProfileImageUrl;
                        objTeam.PremiumImageUrl = objCreateTeamVM.PremiumImageUrl;
                        objTeam.Website = objCreateTeamVM.Website;
                        objTeam.PrimaryCommissionRate = objCreateTeamVM.PrimaryCommissionRate;
                        objTeam.Level1CommissionRate = objCreateTeamVM.Level1CommissionRate;
                        objTeam.Level2CommissionRate = objCreateTeamVM.Level2CommissionRate;
                      
                        if (objCreateTeamVM.OnboardingExeciseVideoId <= 0 && !string.IsNullOrEmpty(objCreateTeamVM.OnboardingVideo))
                        {
                            objTeam.OnboardingExeciseVideoId = dataContext.Exercise.Where(ex => ex.IsActive == true
                                && ex.ExerciseName == objCreateTeamVM.OnboardingVideo).Select(ee => ee.ExerciseId).FirstOrDefault();
                        }
                        else
                        {
                            objTeam.OnboardingExeciseVideoId = objTeam.OnboardingExeciseVideoId;
                        }
                        objTeam.FitcomtestChallengeId1 = objCreateTeamVM.FitcomtestChallengeId1;
                        objTeam.FitcomtestChallengeId2 = objCreateTeamVM.FitcomtestChallengeId2;
                        objTeam.BeginnerProgramId = objCreateTeamVM.BeginnerProgramId;
                        objTeam.AdvIntProgramId1 = objCreateTeamVM.AdvIntProgramId1;
                        objTeam.AdvIntProgramId2 = objCreateTeamVM.AdvIntProgramId2;
                        objTeam.AdvIntProgramId3 = objCreateTeamVM.AdvIntProgramId3;
                        objTeam.Nutrition1HyerLink = objCreateTeamVM.Nutrition1HyerLink;
                        objTeam.Nutrition1ImageUrl = objCreateTeamVM.Nutrition1ImageUrl;
                        objTeam.Nutrition2ImageUrl = objCreateTeamVM.Nutrition2ImageUrl;
                        objTeam.Nutrition2HyerLink = objCreateTeamVM.Nutrition2HyerLink;
                        objTeam.IsShownNoTrainerWorkoutPrograms = objCreateTeamVM.IsShownNoTrainerWorkoutPrograms;
                        dataContext.Teams.Add(objTeam);
                        dataContext.SaveChanges();
                        var teamId = Convert.ToInt32(dataContext.Teams.Max(x => x.TeamId));
                        /*Add credential information into database*/
                        tblCredentials objUserCredential = new tblCredentials();
                        objUserCredential.Password = objEncrypt.EncryptString(objCreateTeamVM.Password);
                        objUserCredential.UserId = teamId;
                        // objUserCredential.UserName = objCreateTeamVM.UserName;
                        objUserCredential.EmailId = objCreateTeamVM.EmailId;
                        objUserCredential.UserType = LinksMediaCorpUtility.Resources.Message.UserTypeTeam;
                        dataContext.Credentials.Add(objUserCredential);

                        /*Add Team Workout Trending Category information into database*/
                        List<tblTeamTrendingAssociation> teamTrendingAssociation = new List<tblTeamTrendingAssociation>();
                        /*Team Workout Trending Category*/
                        if (objCreateTeamVM.PostedWorkoutTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constWorkoutChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Program Trending Category*/
                        if (objCreateTeamVM.PostedProgramTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constProgramChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Fitness Test Trending Category*/
                        if (objCreateTeamVM.PostedFitnessTestTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constFitnessTestName
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }

                        /*Team Workout Trending Category*/
                        if (objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constWorkoutChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Program Trending Category*/
                        if (objCreateTeamVM.PostedSecondaryProgramTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constProgramChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Fitness Test Trending Category*/
                        if (objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = teamId,
                                        TrendingCategoryType = ConstantHelper.constFitnessTestName
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }

                        if (teamTrendingAssociation != null)
                        {
                            dataContext.TeamTrendingAssociations.AddRange(teamTrendingAssociation);
                        }                    

                        //Update Level Team Status
                        List<tblLevelTeam> listtblLevelTeam = dataContext.LevelTeams.Where(lvlteam => lvlteam.GuidRecordId == objCreateTeamVM.GuidRecordId).ToList();
                        listtblLevelTeam.ForEach(lvlTms =>
                        {
                            lvlTms.PrimaryTeamId = teamId;
                            lvlTms.IsActive = true;
                        });
                        dataContext.SaveChanges();
                        dbTran.Commit();
                        return teamId;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        objEncrypt.Dispose();
                        traceLog.AppendLine("SubmitTeam  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objCreateTeamVM = null;
                    }
                }
            }
        }
        /// <summary>
        /// Get team information based on team ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static CreateTeamVM GetTeamById(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetTeamById for retrieving team by id:" + Id);
                    tblTeam team = dataContext.Teams.Find(Id);
                    if (team != null && !string.IsNullOrEmpty(team.TeamName))
                    {
                        string teamName = team.TeamName.Substring(1);
                        team.TeamName = teamName;
                    }
                    Mapper.CreateMap<tblTeam, CreateTeamVM>();
                    CreateTeamVM objTeam = Mapper.Map<tblTeam, CreateTeamVM>(team);

                    tblCredentials trainerCredentials = dataContext.Credentials.Where(ce => ce.UserId == Id && ce.UserType == Message.UserTypeTeam).FirstOrDefault();
                    if (objTeam != null && trainerCredentials != null)
                    {
                        objTeam.EmailId = trainerCredentials.EmailId;
                        var onboradingProgramInfo = GetOnboradingProgramDetails(team.BeginnerProgramId);
                        if (onboradingProgramInfo != null)
                        {
                            objTeam.BeginnerProgramId = onboradingProgramInfo.ChallengeId;
                            objTeam.BeginnerProgram = onboradingProgramInfo.ChallengeName;
                            objTeam.BeginnerProgramUrl = objTeam.BeginnerProgramId > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.BeginnerProgramId : string.Empty;

                            objTeam.BeginnerProgramLink = objTeam.BeginnerProgramId > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.BeginnerProgramId : string.Empty;

                        }

                        var onboradingAdvIntProgramIdInfo1 = GetOnboradingProgramDetails(team.AdvIntProgramId1);
                        if (onboradingAdvIntProgramIdInfo1 != null)
                        {
                            objTeam.AdvIntProgramId1 = onboradingAdvIntProgramIdInfo1.ChallengeId;
                            objTeam.AdvIntProgram1 = onboradingAdvIntProgramIdInfo1.ChallengeName;
                            objTeam.AdvIntProgramLink1 = objTeam.AdvIntProgramId1 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId1 : string.Empty;
                            objTeam.AdvIntProgramUrl1 = objTeam.AdvIntProgramId1 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId1 : string.Empty;

                        }

                        var onboradingAdvIntProgramIdInfo2 = GetOnboradingProgramDetails(team.AdvIntProgramId2);
                        if (onboradingAdvIntProgramIdInfo2 != null)
                        {
                            objTeam.AdvIntProgramId2 = onboradingAdvIntProgramIdInfo2.ChallengeId;
                            objTeam.AdvIntProgram2 = onboradingAdvIntProgramIdInfo2.ChallengeName;
                            objTeam.AdvIntProgramLink2 = objTeam.AdvIntProgramId2 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId2 : string.Empty;
                            objTeam.AdvIntProgramUrl2 = objTeam.AdvIntProgramId2 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId2 : string.Empty;

                        }
                        var onboradingAdvIntProgramIdInfo3 = GetOnboradingProgramDetails(team.AdvIntProgramId3);
                        if (onboradingAdvIntProgramIdInfo3 != null)
                        {
                            objTeam.AdvIntProgramId3 = onboradingAdvIntProgramIdInfo3.ChallengeId;
                            objTeam.AdvIntProgram3 = onboradingAdvIntProgramIdInfo3.ChallengeName;
                            objTeam.AdvIntProgramLink3 = objTeam.AdvIntProgramId3 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId3 : string.Empty;
                            objTeam.AdvIntProgramUrl3 = objTeam.AdvIntProgramId3 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewProgram + objTeam.AdvIntProgramId3 : string.Empty;

                        }
                        var onboradingFitnessTestInfo1 = GetOnboradingFitnessTestDetails(team.FitcomtestChallengeId1);
                        if (onboradingFitnessTestInfo1 != null)
                        {
                            objTeam.FitcomtestChallengeId1 = onboradingFitnessTestInfo1.ChallengeId;
                            objTeam.FitnessTest1 = onboradingFitnessTestInfo1.ChallengeName;
                            objTeam.FitnessTestLink1 = objTeam.FitcomtestChallengeId1 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewChallenge + objTeam.FitcomtestChallengeId1 : string.Empty;
                            objTeam.FitnessTestUrl1 = objTeam.FitcomtestChallengeId1 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewChallenge + objTeam.FitcomtestChallengeId1 : string.Empty;
                        }
                        var onboradingFitnessTestInfo2 = GetOnboradingFitnessTestDetails(team.FitcomtestChallengeId2);
                        if (onboradingFitnessTestInfo2 != null)
                        {
                            objTeam.FitcomtestChallengeId2 = onboradingFitnessTestInfo2.ChallengeId;
                            objTeam.FitnessTest2 = onboradingFitnessTestInfo2.ChallengeName;
                            objTeam.FitnessTestLink2 = objTeam.FitcomtestChallengeId2 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewChallenge + objTeam.FitcomtestChallengeId2 : string.Empty;
                            objTeam.FitnessTestUrl2 = objTeam.FitcomtestChallengeId2 > 0 ? CommonUtility.VirtualPath +
                                ConstantHelper.constProgramViewChallenge + objTeam.FitcomtestChallengeId2 : string.Empty;
                        }
                        var onboradingVideoInfo = GetOnboradingVideoDetails(team.OnboardingExeciseVideoId);
                        if (onboradingVideoInfo != null)
                        {
                            objTeam.OnboardingExeciseVideoId = onboradingVideoInfo.ExerciseId;
                            objTeam.OnboardingVideo = onboradingVideoInfo.ExerciseName;
                            objTeam.OnboardingVideoLink = onboradingVideoInfo.VedioLink;
                            objTeam.OnboardingVideoUrl = onboradingVideoInfo.VedioLink;
                        }
                        List<TrendingCategory> listTeamTrendingAssociation = (from teamTrending in dataContext.TeamTrendingAssociations
                                                                              join trendingcat in dataContext.TrendingCategory
                                                                              on teamTrending.TrendingCategoryId equals trendingcat.TrendingCategoryId
                                                                              where teamTrending.TeamId == Id
                                                                              select new TrendingCategory
                                                                              {
                                                                                  TrendingCategoryId = teamTrending.TrendingCategoryId,
                                                                                  TrendingCategoryName = trendingcat.TrendingName,
                                                                                  TrendingCategoryType = teamTrending.TrendingCategoryType,
                                                                                  TrendingCategoryGroupId= trendingcat.TrendingCategoryGroupId
                                                                              }).ToList();
                        List<TrendingCategory> objWorkoutTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constWorkoutChallengeSubType);
                        objTeam.AvailableWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                        objTeam.SelecetdWorkoutTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                   where string.Compare(cat.TrendingCategoryType, ConstantHelper.constWorkoutChallenge, StringComparison.OrdinalIgnoreCase) == 0
                                                                   && cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId
                                                                   select cat).ToList();

                        objTeam.AvailableSecondaryWorkoutTrendingCategory = objWorkoutTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                        objTeam.SelecetdSecondaryWorkoutTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                   where string.Compare(cat.TrendingCategoryType, ConstantHelper.constWorkoutChallenge, StringComparison.OrdinalIgnoreCase) == 0
                                                                   && cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId
                                                                      select cat).ToList();


                        List<TrendingCategory> objProgramTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constProgramChallengeSubType);
                        objTeam.AvailableProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList();
                        objTeam.SelecetdProgramTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                   where string.Compare(cat.TrendingCategoryType, ConstantHelper.constProgramChallenge, StringComparison.OrdinalIgnoreCase) == 0
                                                                    && cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId
                                                                   select cat).ToList();

                        objTeam.AvailableSecondaryProgramTrendingCategory = objProgramTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                        objTeam.SelecetdSecondaryProgramTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                            where string.Compare(cat.TrendingCategoryType, ConstantHelper.constProgramChallenge, StringComparison.OrdinalIgnoreCase) == 0
                                                                            && cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId
                                                                            select cat).ToList();


                        List<TrendingCategory> objFitnessTestTrendingList = ChallengesCommonBL.GetTrendingCategory(ConstantHelper.constFittnessCommonSubTypeId);
                        objTeam.AvailableFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId).ToList(); 
                        objTeam.SelecetdFitnessTestTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                       where string.Compare(cat.TrendingCategoryType, ConstantHelper.constFitnessTestName, StringComparison.OrdinalIgnoreCase) == 0
                                                                        && cat.TrendingCategoryGroupId == ConstantHelper.constPrimaryTrendinGroupId
                                                                       select cat).ToList();

                        objTeam.AvailableSecondaryFitnessTestTrendingCategory = objFitnessTestTrendingList.Where(wt => wt.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId).ToList();
                        objTeam.SelecetdSecondaryFitnessTestTrendingCategory = (from cat in listTeamTrendingAssociation
                                                                       where string.Compare(cat.TrendingCategoryType, ConstantHelper.constFitnessTestName, StringComparison.OrdinalIgnoreCase) == 0
                                                                        && cat.TrendingCategoryGroupId == ConstantHelper.constSecondaryTrendinGroupId
                                                                                select cat).ToList();


                    }
                    return objTeam;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTeamById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Onborading Program Detail based on Program Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        private static OnboardingChallengeDetail GetOnboradingProgramDetails(int programId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetOnboradingProgramDetails for retrieving  by programId:" + programId);
                    OnboardingChallengeDetail objOnboardingChallengeDetail = new OnboardingChallengeDetail();
                    var challengeInfo = dataContext.Challenge.FirstOrDefault(ch => ch.ChallengeId == programId && ch.IsActive == true
                        && ch.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType);
                    if (challengeInfo != null)
                    {
                        objOnboardingChallengeDetail.ChallengeId = challengeInfo.ChallengeId;
                        objOnboardingChallengeDetail.ChallengeName = challengeInfo.ChallengeName;
                        objOnboardingChallengeDetail.ChallengeUrl = challengeInfo.ChallengeName;
                        objOnboardingChallengeDetail.ChallengeSubTypeId = challengeInfo.ChallengeSubTypeId;
                    }

                    return objOnboardingChallengeDetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetOnboradingProgramDetails  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Onborading FitnessTest Details on challenge Id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        private static OnboardingChallengeDetail GetOnboradingFitnessTestDetails(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetOnboradingFitnessTestDetails for retrieving  by challengeId:" + challengeId);
                    OnboardingChallengeDetail objOnboardingChallengeDetail = new OnboardingChallengeDetail();
                    var challengeInfo = dataContext.Challenge.FirstOrDefault(ch => ch.ChallengeId == challengeId && ch.IsActive == true);
                    if (challengeInfo != null)
                    {
                        objOnboardingChallengeDetail.ChallengeId = challengeInfo.ChallengeId;
                        objOnboardingChallengeDetail.ChallengeName = challengeInfo.ChallengeName;
                        objOnboardingChallengeDetail.ChallengeUrl = challengeInfo.ChallengeName;
                    }
                    return objOnboardingChallengeDetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetOnboradingFitnessTestDetails  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Onborading Video Detail based on execise Id
        /// </summary>
        /// <param name="execiseId"></param>
        /// <returns></returns>
        private static Exercise GetOnboradingVideoDetails(int execiseId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetOnboradingVideoDetails for retrieving  by execiseId:" + execiseId);
                    Exercise objExercise = new Exercise();
                    var execisedetail = dataContext.Exercise.Find(execiseId);
                    if (execisedetail != null)
                    {
                        objExercise.ExerciseId = execisedetail.ExerciseId;
                        objExercise.ExerciseName = execisedetail.ExerciseName;
                        objExercise.VedioLink = execisedetail.V720pUrl;
                    }
                    return objExercise;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetOnboradingVideoDetails  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update Team based on team Id
        /// </summary>
        /// <param name="objCreateTeamVM"></param>
        public static void UpdateTeam(CreateTeamVM objCreateTeamVM)
        {
            StringBuilder traceLog = new StringBuilder();
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: UpdateTrainer for updating trainer");
                        objEncrypt = new EncryptDecrypt();
                        if (!string.IsNullOrEmpty(objCreateTeamVM.Address))
                        {
                            objCreateTeamVM.Address = objCreateTeamVM.Address.Trim();
                        }
                        if (!string.IsNullOrEmpty(objCreateTeamVM.TeamName))
                        {
                            objCreateTeamVM.TeamName = objCreateTeamVM.TeamName.Trim();
                        }
                        Mapper.CreateMap<CreateTeamVM, tblTeam>();
                        tblTeam objTeam = Mapper.Map<CreateTeamVM, tblTeam>(objCreateTeamVM);
                        objTeam.ModifiedDate = DateTime.Now;
                        string teamName = ConstantHelper.constTeamNameHashTag + objCreateTeamVM.TeamName;
                        objTeam.TeamName = teamName;

                        if (objCreateTeamVM.OnboardingExeciseVideoId <= 0 && !string.IsNullOrEmpty(objCreateTeamVM.OnboardingVideo))
                        {
                            objTeam.OnboardingExeciseVideoId = dataContext.Exercise.Where(ex => ex.IsActive == true
                                && ex.ExerciseName == objCreateTeamVM.OnboardingVideo).Select(ee => ee.ExerciseId).FirstOrDefault();
                        }
                        else
                        {
                            objTeam.OnboardingExeciseVideoId = objCreateTeamVM.OnboardingExeciseVideoId;
                        }
                        objTeam.FitcomtestChallengeId1 = objCreateTeamVM.FitcomtestChallengeId1;
                        objTeam.FitcomtestChallengeId2 = objCreateTeamVM.FitcomtestChallengeId2;
                        objTeam.BeginnerProgramId = objCreateTeamVM.BeginnerProgramId;
                        objTeam.AdvIntProgramId1 = objCreateTeamVM.AdvIntProgramId1;
                        objTeam.AdvIntProgramId2 = objCreateTeamVM.AdvIntProgramId2;
                        objTeam.AdvIntProgramId3 = objCreateTeamVM.AdvIntProgramId3;
                        objTeam.Nutrition1HyerLink = objCreateTeamVM.Nutrition1HyerLink;
                        objTeam.Nutrition1ImageUrl = objCreateTeamVM.Nutrition1ImageUrl;
                        objTeam.Nutrition2ImageUrl = objCreateTeamVM.Nutrition2ImageUrl;
                        objTeam.Nutrition2HyerLink = objCreateTeamVM.Nutrition2HyerLink;
                        objTeam.PrimaryCommissionRate = objCreateTeamVM.PrimaryCommissionRate;
                        objTeam.Level1CommissionRate = objCreateTeamVM.Level1CommissionRate;
                        objTeam.Level2CommissionRate = objCreateTeamVM.Level2CommissionRate;
                   
                        objTeam.Website = objCreateTeamVM.Website;
                        objTeam.IsShownNoTrainerWorkoutPrograms = objCreateTeamVM.IsShownNoTrainerWorkoutPrograms;
                        /*update Trainer in to database*/
                        dataContext.Entry(objTeam).State = EntityState.Modified;
                        dataContext.SaveChanges();
                        /*update credentials */
                        tblCredentials objTeamcredtial = dataContext.Credentials.Where(ce => ce.UserId == objCreateTeamVM.TeamId
                            && ce.UserType == Message.UserTypeTeam).FirstOrDefault();
                        if (!(objTeamcredtial == null))
                        {
                            if (objCreateTeamVM.IsChangePassword)
                            {
                                objTeamcredtial.Password = objEncrypt.EncryptString((objCreateTeamVM.Password).Trim());
                            }
                            // objTeamcredtial.UserName = objCreateTeamVM.UserName;
                            objTeamcredtial.EmailId = objCreateTeamVM.EmailId;
                            dataContext.SaveChanges();
                        }


                        // Remove the existing TeamTrending Category assication
                        var removeTeamTrendingCategoryList = dataContext.TeamTrendingAssociations.Where(tr => tr.TeamId == objCreateTeamVM.TeamId).ToList();
                        if (removeTeamTrendingCategoryList != null)
                        {
                            dataContext.TeamTrendingAssociations.RemoveRange(removeTeamTrendingCategoryList);
                            dataContext.SaveChanges();
                        }
                        /*Add Team Workout Trending Category information into database*/
                        List<tblTeamTrendingAssociation> teamTrendingAssociation = new List<tblTeamTrendingAssociation>();
                        /*Team Workout Trending Category*/
                        if (objCreateTeamVM.PostedWorkoutTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedWorkoutTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constWorkoutChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Program Trending Category*/
                        if (objCreateTeamVM.PostedProgramTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedProgramTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constProgramChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Fitness Test Trending Category*/
                        if (objCreateTeamVM.PostedFitnessTestTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedFitnessTestTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constFitnessTestName
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);

                                }
                            }
                        }

                        if (objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryWorkoutTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constWorkoutChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Program Trending Category*/
                        if (objCreateTeamVM.PostedSecondaryProgramTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryProgramTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constProgramChallenge
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);
                                }
                            }
                        }
                        /*Team Fitness Test Trending Category*/
                        if (objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory != null)
                        {
                            if (objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID != null)
                            {
                                for (int i = 0; i < objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID.Count; i++)
                                {
                                    tblTeamTrendingAssociation objteamTrendingAssociation = new tblTeamTrendingAssociation()
                                    {
                                        TrendingCategoryId = Convert.ToInt32(objCreateTeamVM.PostedSecondaryFitnessTestTrendingCategory.TrendingCategoryID[i]),
                                        TeamId = objCreateTeamVM.TeamId,
                                        TrendingCategoryType = ConstantHelper.constFitnessTestName
                                    };
                                    teamTrendingAssociation.Add(objteamTrendingAssociation);

                                }
                            }
                        }
                        if (teamTrendingAssociation != null)
                        {
                            dataContext.TeamTrendingAssociations.AddRange(teamTrendingAssociation);                           
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
                        objEncrypt.Dispose();
                        traceLog.AppendLine("UpdateTrainer  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        objCreateTeamVM = null;
                    }
                }
            }
        }
        /// <summary>
        /// Delete Team based Team Id
        /// </summary>
        /// <param name="Id"></param>
        public static void DeleteTeam(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: DeleteTeam for deleting trainer");
                        tblTeam team = dataContext.Teams.Find(Id);
                        if (team != null)
                        {
                            int defaultfitcomteamId = 0;
                            /*Update team member record from database*/
                            List<tblTrainerTeamMembers> trainerTeamMemberList = dataContext.TrainerTeamMembers.Where(ttm => ttm.TeamId == Id).ToList();
                            defaultfitcomteamId = dataContext.Teams.Where(t => t.IsDefaultTeam).Select(tm => tm.TeamId).FirstOrDefault();
                            if (trainerTeamMemberList != null)
                            {
                                trainerTeamMemberList.ForEach(tm =>
                                    {
                                        tm.TeamId = defaultfitcomteamId;
                                    });
                                dataContext.SaveChanges();
                            }
                            /// get list of users of Default Team
                            List<int> olddefaultteamUsers = (from usr in dataContext.User
                                                             join cr in dataContext.Credentials
                                                             on usr.UserId equals cr.UserId
                                                             where cr.UserType == Message.UserTypeUser && usr.TeamId == defaultfitcomteamId
                                                             select cr.Id).ToList();
                            List<int> defautteamtrainerList = (from tr in dataContext.TrainerTeamMembers
                                                               join crd in dataContext.Credentials
                                                               on tr.UserId equals crd.Id
                                                               where crd.UserType == Message.UserTypeTrainer && tr.TeamId == Id
                                                               select crd.Id).Distinct().ToList();
                            List<tblFollowings> usersFollowingsList = new List<tblFollowings>();
                            foreach (int tranercrdId in defautteamtrainerList)
                            {
                                foreach (int userCrdId in olddefaultteamUsers)
                                {
                                    tblFollowings objtblFollowings = new tblFollowings();
                                    objtblFollowings.FollowUserId = tranercrdId;
                                    objtblFollowings.UserId = userCrdId;
                                    usersFollowingsList.Add(objtblFollowings);
                                }
                            }
                            dataContext.Followings.AddRange(usersFollowingsList);
                            dataContext.SaveChanges();
                            /* Reset the primary trainer Id for users who has thsi trainer as primary trainer*/
                            List<tblUser> userList = dataContext.User.Where(usr => usr.TeamId == Id).ToList();
                            List<int> deleteteamUserId = null;
                            if (userList != null)
                            {
                                deleteteamUserId = userList.Select(u => u.UserId).ToList();
                                foreach (var item in userList)
                                {
                                    item.TeamId = defaultfitcomteamId;
                                }
                                dataContext.SaveChanges();
                            }
                            //Reset Deleted team trainer info and add following deleted user to Default trainers
                            List<tblTrainer> deleletdeTeamtrainers = dataContext.Trainer.Where(usr => usr.TeamId == Id).ToList();
                            if (deleletdeTeamtrainers != null)
                            {
                                foreach (var item in deleletdeTeamtrainers)
                                {
                                    item.TeamId = defaultfitcomteamId;
                                }
                                dataContext.SaveChanges();
                            }
                            defautteamtrainerList = (from tr in dataContext.TrainerTeamMembers
                                                     join crd in dataContext.Credentials
                                                     on tr.UserId equals crd.Id
                                                     where crd.UserType == Message.UserTypeTrainer && tr.TeamId == defaultfitcomteamId
                                                     select crd.Id).Distinct().ToList();
                            foreach (int tranercrdId in defautteamtrainerList)
                            {
                                foreach (int userCrdId in deleteteamUserId)
                                {
                                    tblFollowings objtblFollowings = new tblFollowings();
                                    objtblFollowings.FollowUserId = tranercrdId;
                                    objtblFollowings.UserId = userCrdId;
                                    usersFollowingsList.Add(objtblFollowings);
                                }
                            }
                            dataContext.Followings.AddRange(usersFollowingsList);
                            dataContext.SaveChanges();

                            /*Delete related message stream from database based on credential Id*/
                            tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == Id && c.UserType == Message.UserTypeTeam);
                            if (objCred != null)
                            {
                                dataContext.Credentials.Remove(objCred);
                                dataContext.SaveChanges();
                            }
                            /*Delete Related user token of this user*/
                            if (objCred != null)
                            {
                                List<tblUserToken> usertokenList = dataContext.UserToken.Where(uf => uf.UserId == objCred.Id).ToList();
                                if (usertokenList != null)
                                {
                                    dataContext.UserToken.RemoveRange(usertokenList);
                                    dataContext.SaveChanges();
                                }
                            }
                            /*Delete Trainer from database*/
                            dataContext.Teams.Remove(team);
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
                        traceLog.AppendLine("DeleteTeam  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Get All Team Name
        /// </summary>
        /// <returns></returns>
        public static List<DDTeams> GetAllTeamName()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllTeamName from database");
                    var teamlist = (from team in dataContext.Teams
                                    select new DDTeams
                                    {
                                        TeamId = team.TeamId,
                                        TeamName = team.TeamName,
                                        IsDefaultTeam = team.IsDefaultTeam
                                    }).OrderBy(tm=>tm.TeamName).ToList();
                    teamlist.ForEach(t =>
                        {
                            string teamName = string.IsNullOrEmpty(t.TeamName) ? string.Empty : t.TeamName.Substring(1);
                            t.TeamName = teamName;
                            if (t.IsDefaultTeam)
                            {
                                t.TeamName = t.TeamName + Convert.ToString(Message.TeamDefaultText);
                            }
                        });
                    return teamlist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetAllTeamName  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get TeamEmail base search key
        /// </summary>
        /// <param name="tearm"></param>
        /// <returns></returns>
        public static string GetTeamEmail(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerEmail");
                    string listOut = string.Empty;
                    /*if string conmtain ',' then check emial for another users not for current user else for all*/
                    if (!string.IsNullOrEmpty(tearm))
                    {
                        if (tearm.Contains(','))
                        {
                            string email = string.Empty;
                            int id = 0;
                            int index = tearm.LastIndexOf(',');
                            email = tearm.Substring(0, index);
                            id = Convert.ToInt32(tearm.Substring(index + 1));
                            listOut = _dbContext.Teams.Where(ct => ct.EmailId == email && ct.TeamId != id).Select(y => y.EmailId).FirstOrDefault();
                            tearm = email;
                        }
                        else
                        {
                            listOut = _dbContext.Teams.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                        }

                        if (listOut == null)
                        {
                            listOut = _dbContext.User.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                        }
                        if (listOut == null)
                        {
                            listOut = _dbContext.Trainer.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                        }
                    }
                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerEmail  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Count
        /// </summary>
        /// <returns></returns>
        public static int GetTeamCount()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeamCount");
                    int userCount = _dbContext.Teams.Count();
                    return userCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTeamCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get receiver completed challenge detail
        /// </summary>
        /// <param name="userchallengeId"></param>
        /// <returns></returns>
        public static RecentResultVM GetNotificationReceiverResult(int userchallengeId, long notificationID)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetNotificationReceiverResult-userchallengeId" + userchallengeId);
                    Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    NotificationApiBL.UpdateNotificationReadStatus(dataContext, notificationID);
                    RecentResultVM resultQueryData = (from uc in dataContext.UserChallenge
                                                      join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                      join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                      orderby uc.AcceptedDate descending
                                                      where uc.Id == userchallengeId && (uc.Result != null || uc.Fraction != null) && c.IsActive == true
                                                      select new RecentResultVM
                                                      {
                                                          ResultId = uc.Id,
                                                          ChallengeId = uc.ChallengeId,
                                                          ChallengeType = ct.ChallengeType,
                                                          ChallengeName = c.ChallengeName,
                                                          DifficultyLevel = c.DifficultyLevel,
                                                          Duration = c.FFChallengeDuration,
                                                          ChallengeSubTypeid = c.ChallengeSubTypeId,
                                                          IsSubscription = c.IsSubscription,
                                                          ExeciseVideoDetails = (from cexe in dataContext.CEAssociation
                                                                                 join ex in dataContext.Exercise
                                                                                 on cexe.ExerciseId equals ex.ExerciseId
                                                                                 where cexe.ChallengeId == c.ChallengeId
                                                                                 orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                                                                 select new ExeciseVideoDetail
                                                                                 {
                                                                                     ExeciseName = ex.ExerciseName,
                                                                                     ExeciseUrl = ex.V720pUrl,
                                                                                     ExerciseThumnail = ex.ThumnailUrl,
                                                                                     ChallengeExeciseId = cexe.RocordId
                                                                                 }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault(),
                                                          TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                            join bp in dataContext.BodyPart
                                                                            on trzone.PartId equals bp.PartId
                                                                            where trzone.ChallengeId == c.ChallengeId
                                                                            select bp.PartName).Distinct().ToList<string>(),
                                                          TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                            join bp in dataContext.Equipments
                                                                            on trzone.EquipmentId equals bp.EquipmentId
                                                                            where trzone.ChallengeId == c.ChallengeId
                                                                            select bp.Equipment).Distinct().ToList<string>(),
                                                          Strength = dataContext.UserChallenge.Where(ucc => ucc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                          Result = uc.Result,
                                                          Fraction = uc.Fraction,
                                                          ResultUnit = ct.ResultUnit,
                                                          ResultUnitSuffix = uc.ResultUnit,
                                                          UserCredID = uc.UserId,
                                                          PostType = ConstantHelper.ResultFeed,
                                                          DbPostedDate = uc.AcceptedDate,
                                                          BoomsCount = dataContext.ResultBooms.Count(b => b.ResultId == uc.Id),
                                                          CommentsCount = dataContext.ResultComments.Count(cmnt => cmnt.Id == uc.Id),
                                                          UserType = dataContext.Credentials.Where(ucrd => ucrd.Id == uc.UserId).FirstOrDefault().UserType,
                                                          IsLoginUserBoom = dataContext.ResultBooms.Where(bm => bm.ResultId == uc.Id).Any(b => b.BoomedBy == objCredential.Id),
                                                          IsLoginUserComment = dataContext.ResultComments.Where(cm => cm.Id == uc.Id).Any(b => b.CommentedBy == objCredential.Id),
                                                      }).FirstOrDefault();


                    if (resultQueryData != null)
                    {
                        if (resultQueryData.TempTargetZone != null && resultQueryData.TempTargetZone.Count > 0)
                        {
                            resultQueryData.TargetZone = string.Join(", ", resultQueryData.TempTargetZone);
                        }
                        resultQueryData.TempTargetZone = null;
                        if (resultQueryData.TempEquipments != null && resultQueryData.TempEquipments.Count > 0)
                        {
                            resultQueryData.Equipments = string.Join(", ", resultQueryData.TempEquipments);
                        }
                        resultQueryData.TempEquipments = null;

                        if (resultQueryData.ExeciseVideoDetails != null)
                        {
                            if (string.IsNullOrEmpty(resultQueryData.ExeciseVideoDetails.ExeciseUrl))
                            {
                                resultQueryData.ExeciseVideoLink = !string.IsNullOrEmpty(resultQueryData.ExeciseVideoDetails.ExeciseName) ? CommonUtility.VirtualFitComExercisePath
                                    + Message.ExerciseVideoDirectory + resultQueryData.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                            }
                            else
                            {
                                resultQueryData.ExeciseVideoLink = resultQueryData.ExeciseVideoDetails.ExeciseUrl;
                            }
                            if (!string.IsNullOrEmpty(resultQueryData.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(resultQueryData.ExeciseVideoDetails.ExerciseThumnail))
                            {
                                string thumnailName = resultQueryData.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                string thumnailHeight = string.Empty;
                                string thumnailWidth = string.Empty;
                                resultQueryData.ThumbnailUrl = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                resultQueryData.ThumbNailHeight = thumnailHeight;
                                resultQueryData.ThumbNailWidth = thumnailWidth;
                            }
                            else
                            {
                                resultQueryData.ThumbnailUrl = resultQueryData.ExeciseVideoDetails.ExerciseThumnail;
                            }
                        }
                        resultQueryData.isWellness = resultQueryData.ChallengeSubTypeid == ConstantHelper.constWellnessChallengeSubType;
                        resultQueryData.VideoList = resultQueryData.VideoList != null ? resultQueryData.VideoList : new List<VideoInfo>();
                        resultQueryData.PicList = resultQueryData.PicList != null ? resultQueryData.PicList : new List<PicsInfo>();
                        resultQueryData.TempTargetZone = null;
                        // Find the user details based on user credId and user Type
                        ProfileDetails userdetails = ProfileApiBL.GetProfileDetailsByCredId(resultQueryData.UserCredID, resultQueryData.UserType);
                        if (userdetails != null)
                        {
                            resultQueryData.UserName = userdetails.UserName;
                            resultQueryData.UserImageUrl = userdetails.ProfileImageUrl;
                            resultQueryData.userID = userdetails.UserId;
                        }
                        resultQueryData.DbPostedDate = resultQueryData.DbPostedDate.ToUniversalTime();
                        resultQueryData = CommonApiBL.NotificationReceiverResult(resultQueryData, dataContext);
                        resultQueryData.ChallengeType = string.IsNullOrEmpty(resultQueryData.ChallengeType) ? string.Empty : resultQueryData.ChallengeType.Split(' ')[0];

                    }
                    return resultQueryData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetNotificationReceiverResult  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }

        /// <summary>
        /// Send Joined DefaultTeam Notification to trainers
        /// </summary>
        /// <param name="isJoinDefautTeam"></param>
        /// <returns></returns>
        public static bool SendDefaultTeamNotification(bool isJoinDefautTeam)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SendDefaultTeamNotification-" + isJoinDefautTeam);
                    if (isJoinDefautTeam)
                    {
                        Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        if (objCredential != null)
                        {
                            tblUser objuser = dataContext.User.FirstOrDefault(usr => usr.UserId == objCredential.UserId);
                            if (objuser != null)
                            {
                                string userFullName = string.Empty;
                                userFullName = objuser.FirstName + " " + objuser.LastName;
                                string NtType = NotificationType.TrainerJoinTeam.ToString();
                                NotificationApiBL.SendJoinedTeamNotificationToTrainers(objuser.TeamId, userFullName, NtType, objuser.UserId, Message.UserTypeUser);
                                return true;
                            }
                        }
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: SendDefaultTeamNotification  --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
    }
}
