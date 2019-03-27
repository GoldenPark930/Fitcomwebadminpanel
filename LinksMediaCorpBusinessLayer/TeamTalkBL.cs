namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Threading;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Collections.Generic;

    public class TeamTalkBL
    {
        /// <summary>
        /// Function to post Share message text on Team 
        /// </summary>
        /// <returns>ViewPostVM</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 06/24/2015
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
                    objMessageStream.TargetType = Message.UserTargetType;
                    objMessageStream.SubjectId = objCred.Id;
                    objMessageStream.TargetId = dataContext.Credentials.FirstOrDefault(c => c.UserId == message.UserId && c.UserType == message.UserType).Id;
                    dataContext.MessageStraems.Add(objMessageStream);
                    dataContext.SaveChanges();
                    ViewPostVM objViewPostVM = new ViewPostVM()
                    {
                        PostId = objMessageStream.MessageStraemId,
                        Message = objMessageStream.Content,
                        PostedDate = CommonWebApiBL.GetDateTimeFormat(objMessageStream.PostedDate),
                        BoomsCount = dataContext.Booms.Count(b => b.MessageStraemId == objMessageStream.MessageStraemId),
                        CommentsCount = dataContext.Comments.Count(cmnt => cmnt.MessageStraemId == objMessageStream.MessageStraemId)
                    };
                    if (objCred.UserType.Equals(Message.UserTypeUser))
                    {
                        var user = (from usr in dataContext.User
                                    join crd in dataContext.Credentials on usr.UserId equals crd.UserId
                                    where crd.Id == objMessageStream.SubjectId
                                    select new
                                    {
                                        usr.FirstName,
                                        usr.LastName,
                                        usr.UserImageUrl
                                    }).FirstOrDefault();
                        if (user != null)
                        {
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(user.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                Message.ProfilePicDirectory + user.UserImageUrl;
                            objViewPostVM.UserName = user.FirstName + " " + user.LastName;
                        }
                    }
                    else
                    {
                        var trainer = (from t in dataContext.Trainer
                                       join crd in dataContext.Credentials on t.TrainerId equals crd.UserId
                                       where crd.Id == objMessageStream.SubjectId
                                       select new
                                       {
                                           t.FirstName,
                                           t.LastName,
                                           t.TrainerImageUrl
                                       }).FirstOrDefault();
                        if (trainer != null)
                        {
                            objViewPostVM.PostedByImageUrl = string.IsNullOrEmpty(trainer.TrainerImageUrl) ? string.Empty :
                                CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainer.TrainerImageUrl;
                            objViewPostVM.UserName = trainer.FirstName + " " + trainer.LastName;
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
        /// Get Seach Level Teams
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<LevelTeamVM> GetSeachLevelTeam(string searchTerm, string guidRecord, int teamId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetSeachLevelTeam() by seach Item=" + searchTerm);
                    List<LevelTeamVM> seachLevelTeams = null;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        List<int> insertedLevelTeam = new List<int>();
                        bool isnewTeam = false;
                        if (!string.IsNullOrEmpty(guidRecord) && teamId == 0)
                        {
                            insertedLevelTeam = dataContext.LevelTeams.Where(lt => lt.GuidRecordId.Equals(guidRecord, StringComparison.OrdinalIgnoreCase)).Select(lt => lt.LevelTeamId).ToList();
                            isnewTeam = true;
                        }
                        else
                        {
                            insertedLevelTeam = dataContext.LevelTeams.Where(lt => lt.PrimaryTeamId == teamId).Select(lt => lt.LevelTeamId).ToList();
                            isnewTeam = false;
                        }

                        seachLevelTeams = (from team in dataContext.Teams
                                           where team.TeamName.ToUpper().Contains(searchTerm.ToUpper()) && !insertedLevelTeam.Contains(team.TeamId) && (isnewTeam || team.TeamId != teamId)
                                           orderby team.TeamName
                                           select new LevelTeamVM
                                           {
                                               TeamName = team.TeamName,
                                               TeamId = team.TeamId,
                                               PhoneNumber = team.PhoneNumber,
                                               EmailId = team.EmailId
                                           }).ToList();

                        seachLevelTeams.ForEach(tm =>
                        {
                            List<tblCredentials> teamprimunuser = (from user in dataContext.User
                                                                   join crd in dataContext.Credentials
                                                                   on user.UserId equals crd.UserId
                                                                   where crd.UserType == Message.UserTypeUser && user.TeamId == tm.TeamId
                                                                   select crd).ToList();
                            tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                            tm.Premium = teamprimunuser.Count(usr => usr.AndriodSubscriptionStatus || usr.IOSSubscriptionStatus);
                            tm.Users = teamprimunuser.Count();

                        });
                    }
                    if (seachLevelTeams == null)
                    {
                        seachLevelTeams = new List<LevelTeamVM>();
                    }
                    return seachLevelTeams;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetSeachLevelTeam  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Add Level Teama and get partial view
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static LevelTeamVM AddLevelTeam(int primaryTeamId, string GuidRecord, int teamId, string teamName, int levelTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: AddLevelTeam() by seach teamId=" + teamName);
                    LevelTeamVM addedLevelItem = new LevelTeamVM();
                    if (!string.IsNullOrEmpty(teamName))
                    {

                        tblLevelTeam tblLevelTeam = new tblLevelTeam();
                        if (!(primaryTeamId > 0))
                        {
                            tblLevelTeam.GuidRecordId = GuidRecord;
                            tblLevelTeam.PrimaryTeamId = -1;
                            tblLevelTeam.IsActive = false;
                        }
                        else
                        {
                            tblLevelTeam.GuidRecordId = ConstantHelper.constFFChallangeDescription;
                            tblLevelTeam.PrimaryTeamId = primaryTeamId;
                            tblLevelTeam.IsActive = true;
                        }
                        tblLevelTeam.LevelTypeId = levelTypeId;
                        tblLevelTeam.LevelTeamId = teamId;
                        tblLevelTeam.CreatedDate = DateTime.Now;
                        dataContext.LevelTeams.Add(tblLevelTeam);
                        dataContext.SaveChanges();
                        int teamLevelId = tblLevelTeam.TeamLevelId;
                        if (teamLevelId > 0)
                        {
                            addedLevelItem = (from team in dataContext.Teams
                                              where team.TeamId == teamId
                                              select new LevelTeamVM
                                              {
                                                  TeamName = team.TeamName,
                                                  TeamId = team.TeamId,
                                                  PhoneNumber = team.PhoneNumber,
                                                  EmailId = team.EmailId
                                              }).FirstOrDefault();


                            //List<tblCredentials> teamprimum = (from user in dataContext.User
                            //                                   join crd in dataContext.Credentials
                            //                                   on user.UserId equals crd.UserId
                            //                                   where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                            //                                   select crd).Union(
                            //    from trainer in dataContext.TrainerTeamMembers
                            //    join crd in dataContext.Credentials
                            //    on trainer.UserId equals crd.UserId
                            //    where crd.UserType == Message.UserTypeTrainer && trainer.TeamId == teamId
                            //    select crd
                            //    ).ToList();

                            List<tblCredentials> teamprimunuser = (from user in dataContext.User
                                                                   join crd in dataContext.Credentials
                                                                   on user.UserId equals crd.UserId
                                                                   where crd.UserType != Message.UserTypeUser && user.TeamId == teamId
                                                                   select crd).ToList();
                            addedLevelItem.TeamName = string.IsNullOrEmpty(teamName) ? string.Empty : teamName;
                            addedLevelItem.Premium = teamprimunuser.Count(usr => usr.AndriodSubscriptionStatus || usr.IOSSubscriptionStatus);
                            addedLevelItem.Users = teamprimunuser.Count();
                            addedLevelItem.LevelTeamId = teamLevelId;
                        }


                    }
                    return addedLevelItem;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("AddLevelTeam  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Level Teams based on  primary team or new created team guid Id
        /// </summary>
        /// <param name="primaryTeamId"></param>
        /// <param name="GuidRecord"></param>
        /// <param name="IsLevel1Team"></param>
        /// <returns></returns>
        public static List<LevelTeamVM> GetLevelTeams(int primaryTeamId, string GuidRecord, int LevelTypeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetLevelTeams() by seach primaryTeamId=" + primaryTeamId + ",GuidRecord-" + GuidRecord);
                    List<LevelTeamVM> seachLevelTeams = null;
                    if (!string.IsNullOrEmpty(GuidRecord) || primaryTeamId >= -1)
                    {
                        bool isGuidRecord = false;
                        bool isprimaryTeamId = false;
                        if (!(primaryTeamId > 0))
                        {
                            isGuidRecord = true;
                            isprimaryTeamId = false;
                        }
                        else
                        {
                            isGuidRecord = false;
                            isprimaryTeamId = true;
                        }
                        seachLevelTeams = (from team in dataContext.Teams
                                           join levelTeam in dataContext.LevelTeams
                                           on team.TeamId equals levelTeam.LevelTeamId
                                           where levelTeam.LevelTypeId == LevelTypeId
                                           && ((levelTeam.GuidRecordId == GuidRecord && isGuidRecord) || (isprimaryTeamId && levelTeam.PrimaryTeamId == primaryTeamId))
                                           orderby team.TeamName
                                           select new LevelTeamVM
                                           {
                                               TeamName = team.TeamName,
                                               TeamId = team.TeamId,
                                               LevelTeamId = levelTeam.TeamLevelId,
                                               PhoneNumber = team.PhoneNumber,
                                               EmailId = team.EmailId
                                           }).ToList();

                        seachLevelTeams.ForEach(tm =>
                        {
                            //List<tblCredentials> teamprimum = (from user in dataContext.User
                            //                                   join crd in dataContext.Credentials
                            //                                   on user.UserId equals crd.UserId
                            //                                   where crd.UserType == Message.UserTypeUser && user.TeamId == tm.TeamId
                            //                                   select crd).Union(
                            //    from trainer in dataContext.TrainerTeamMembers
                            //    join crd in dataContext.Credentials
                            //    on trainer.UserId equals crd.UserId
                            //    where crd.UserType == Message.UserTypeTrainer && trainer.TeamId == tm.TeamId
                            //    select crd
                            //    ).ToList();

                            List<tblCredentials> teamprimunuser = (from user in dataContext.User
                                                                   join crd in dataContext.Credentials
                                                                   on user.UserId equals crd.UserId
                                                                   where crd.UserType == Message.UserTypeUser && user.TeamId == tm.TeamId
                                                                   select crd).ToList();
                            tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                            tm.Premium = teamprimunuser.Count(usr => usr.AndriodSubscriptionStatus || usr.IOSSubscriptionStatus);
                            tm.Users = teamprimunuser.Count();

                        });
                    }
                    return seachLevelTeams;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetLevelTeams  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Delete Level Teams
        /// </summary>
        /// <param name="Level TeamId"></param>
        /// <returns></returns>
        public static bool DeleteLevelTeams(int levelTeamId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: DeleteLevelTeams() of  levelTeamId=" + levelTeamId);
                    if (levelTeamId > 0)
                    {
                        var levelteam = dataContext.LevelTeams.Where(lvlTm => lvlTm.TeamLevelId == levelTeamId).FirstOrDefault();
                        if (levelteam != null)
                        {
                            dataContext.LevelTeams.Remove(levelteam);
                            dataContext.SaveChanges();
                            return true;
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
                    traceLog.AppendLine("DeleteLevelTeams  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Primum User Count based on TeamId
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static int GetTeamPrimumUsersCount(LinksMediaContext dataContext, int teamId, DateTime startDate, DateTime endDate)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamUserPrimumCount() of  TeamId=" + teamId);
                int primumUsersCount = 0;
                bool isfilterByDate = true;
                string purchase = Convert.ToString(SubscriptionPurchaseStatus.Buy);
                List<tblAppSubscription> teamUsers = (from user in dataContext.User
                                                      join crd in dataContext.Credentials
                                                      on user.UserId equals crd.UserId
                                                      join appsub in dataContext.AppSubscriptions
                                                      on crd.Id equals appsub.UserCredId
                                                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                                                      && (crd.AndriodSubscriptionStatus || crd.IOSSubscriptionStatus)
                                                      && appsub.SubscriptionStatus == purchase
                                                      select appsub).ToList();
                if (startDate.Equals(DateTime.MinValue) && endDate.Equals(DateTime.MinValue))
                {
                    isfilterByDate = true;
                    DateTime now = DateTime.Now;
                    startDate = new DateTime(now.Year, now.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
                else if (startDate.Equals(DateTime.MinValue))
                {
                    startDate = DateTime.MinValue;
                    isfilterByDate = true;
                }
                else if (endDate.Equals(DateTime.MinValue))
                {
                    endDate = DateTime.MaxValue;
                    isfilterByDate = true;
                }
                if (isfilterByDate)
                {
                    teamUsers = teamUsers.Where(usr => (usr.SubscriptionBuyDate ?? DateTime.MinValue).Date >= startDate.Date && (usr.SubscriptionBuyDate ?? DateTime.MinValue).Date <= endDate.Date).ToList();
                }
                teamUsers = teamUsers.GroupBy(tc => new { tc.UserCredId, tc.SubscriptionId }).Select(tc => tc.FirstOrDefault()).ToList();
                primumUsersCount = teamUsers.Count(usr => usr.SubscriptionStatus == Convert.ToString(SubscriptionPurchaseStatus.Buy));
                return primumUsersCount;
            }
            finally
            {
                traceLog.AppendLine("GetTeamUserPrimumCount  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Team non Primuim user count based on team Id
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static int GetTeamUsersCount(LinksMediaContext dataContext, int teamId, DateTime startDate, DateTime endDate)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamUsersCount() of  TeamId=" + teamId);
                int usercount = 0;
                bool isfilterByDate = true;
                var primunteamuser = (from user in dataContext.User
                                      join crd in dataContext.Credentials
                                      on user.UserId equals crd.UserId
                                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                                      select new
                                      {
                                          crd.AndriodSubscriptionStatus,
                                          crd.IOSSubscriptionStatus,
                                          user.CreatedDate
                                      }).ToList();
                if (primunteamuser != null)
                {
                    if (startDate.Equals(DateTime.MinValue) && endDate.Equals(DateTime.MinValue))
                    {
                        isfilterByDate = true;
                        DateTime now = DateTime.Now;
                        startDate = new DateTime(now.Year, now.Month, 1);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                    }
                    else if (startDate.Equals(DateTime.MinValue))
                    {
                        startDate = DateTime.MinValue;
                        isfilterByDate = true;
                    }
                    else if (endDate.Equals(DateTime.MinValue))
                    {
                        endDate = DateTime.MaxValue;
                        isfilterByDate = true;
                    }
                    if (isfilterByDate)
                    {
                        primunteamuser = primunteamuser.Where(usr => usr.CreatedDate.Date >= startDate.Date && usr.CreatedDate.Date <= endDate.Date).ToList();
                    }
                    usercount = primunteamuser.Count();
                }
                return usercount;
            }
            finally
            {
                traceLog.AppendLine("GetTeamUsersCount  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private static int GetTeamPrimumUsersCount(LinksMediaContext dataContext, int teamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamUsersCount() of  TeamId=" + teamId);
                int usercount = 0;
                //var primunteamuser = (from user in dataContext.User
                //                      join crd in dataContext.Credentials
                //                      on user.UserId equals crd.UserId
                //                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                //                      select new
                //                      {
                //                          crd.AndriodSubscriptionStatus,
                //                          crd.IOSSubscriptionStatus
                //                      }).Union(
                //    from trainer in dataContext.TrainerTeamMembers
                //    join crd in dataContext.Credentials
                //    on trainer.UserId equals crd.UserId
                //    where crd.UserType == Message.UserTypeTrainer && trainer.TeamId == teamId
                //    select new
                //    {
                //        crd.AndriodSubscriptionStatus,
                //        crd.IOSSubscriptionStatus
                //    }).ToList();

                var primunteamuser = (from user in dataContext.User
                                      join crd in dataContext.Credentials
                                      on user.UserId equals crd.UserId
                                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                                      select new
                                      {
                                          crd.AndriodSubscriptionStatus,
                                          crd.IOSSubscriptionStatus
                                      }).ToList();
                if (primunteamuser != null)
                {
                    usercount = primunteamuser.Count(usr => (usr.AndriodSubscriptionStatus || usr.IOSSubscriptionStatus));
                }
                return usercount;
            }
            finally
            {
                traceLog.AppendLine("GetTeamUsersCount  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private static int GetTeamUsersCount(LinksMediaContext dataContext, int teamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamUsersCount() of  TeamId=" + teamId);
                int usercount = 0;
                //var primunteamuser = (from user in dataContext.User
                //                      join crd in dataContext.Credentials
                //                      on user.UserId equals crd.UserId
                //                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                //                      select new
                //                      {
                //                          crd.AndriodSubscriptionStatus,
                //                          crd.IOSSubscriptionStatus
                //                      }).Union(
                //    from trainer in dataContext.TrainerTeamMembers
                //    join crd in dataContext.Credentials
                //    on trainer.UserId equals crd.UserId
                //    where crd.UserType == Message.UserTypeTrainer && trainer.TeamId == teamId
                //    select new
                //    {
                //        crd.AndriodSubscriptionStatus,
                //        crd.IOSSubscriptionStatus
                //    }).ToList();
                var primunteamuser = (from user in dataContext.User
                                      join crd in dataContext.Credentials
                                      on user.UserId equals crd.UserId
                                      where crd.UserType == Message.UserTypeUser && user.TeamId == teamId
                                      select new
                                      {
                                          crd.AndriodSubscriptionStatus,
                                          crd.IOSSubscriptionStatus
                                      }).ToList();
                if (primunteamuser != null)
                {
                    usercount = primunteamuser.Count();
                }
                return usercount;
            }
            finally
            {
                traceLog.AppendLine("GetTeamUsersCount  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// Get Team Commision Details
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static TeamViewData GetTeamCommisionDetails(int teamId, int month, int year)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeamCommisionDetails() of  TeamId=" + teamId);
                    TeamViewData objTeamViewData = new TeamViewData();

                    if (teamId > 0)
                    {
                        //  DateTime startDate = new DateTime(year, month, 1);
                        //  DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                        bool isYearSearch = true;
                        bool isMonthSearch = true;
                        if (month > 0)
                        {
                            isMonthSearch = false;
                        }
                        if (year > 0)
                        {
                            isYearSearch = false;
                        }
                        LevelTeamVM primaryteamDetails = (from team in dataContext.Teams
                                                          join primeTeam in dataContext.TeamCommissionReport
                                                          on team.TeamId equals primeTeam.TeamId
                                                          where team.TeamId == teamId
                                                          && (primeTeam.ReportMonth == month || isMonthSearch)
                                                          && (primeTeam.ReportYear == year || isYearSearch)
                                                          select new LevelTeamVM
                                                          {
                                                              TeamName = team.TeamName,
                                                              TeamId = team.TeamId,
                                                              PhoneNumber = team.PhoneNumber,
                                                              EmailId = team.EmailId,
                                                              CommissionRate = primeTeam.TeamPrimaryCommissionRate,
                                                              Level1CommissionRate = primeTeam.Level1CommissionRate,
                                                              Level2CommissionRate = primeTeam.Level2CommissionRate,
                                                              Users = primeTeam.NumberOfActiveUser,
                                                              Premium = primeTeam.NumberOfPremiumUser,
                                                              TeamCommissionReportId = primeTeam.TeamCommissionReportId
                                                          }).FirstOrDefault();
                        if (primaryteamDetails != null)
                        {
                            primaryteamDetails.TeamName = string.IsNullOrEmpty(primaryteamDetails.TeamName) ? string.Empty : primaryteamDetails.TeamName.Substring(1);
                            objTeamViewData.PrimaryTeamDetail = primaryteamDetails;
                            objTeamViewData.PrimaryTeamCommision = 0.0M;
                            objTeamViewData.PrimaryTeamCommision = primaryteamDetails.Premium * primaryteamDetails.CommissionRate;

                            var allLevelTeam = (from team in dataContext.Teams
                                                join levelTeam in dataContext.TeamLevelCommissionReport
                                                on team.TeamId equals levelTeam.LevelTeamId
                                                where levelTeam.PrimaryTeamId == primaryteamDetails.TeamId
                                                && levelTeam.TeamCommissionReportId == primaryteamDetails.TeamCommissionReportId
                                                orderby team.TeamName
                                                select new LevelTeamVM
                                                {
                                                    TeamName = team.TeamName,
                                                    TeamId = team.TeamId,
                                                    LevelTeamId = levelTeam.LevelTeamId,
                                                    PhoneNumber = team.PhoneNumber,
                                                    EmailId = team.EmailId,
                                                    LevelTypeId = levelTeam.LevelTypeId,
                                                    Level1CommissionRate = team.Level1CommissionRate,
                                                    Level2CommissionRate = team.Level2CommissionRate,
                                                    Users = levelTeam.NumberOfActiveUser,
                                                    Premium = levelTeam.NumberOfPremiumUser
                                                }).ToList();
                            if (allLevelTeam != null)
                            {
                                objTeamViewData.Level1TeamDetail = allLevelTeam.Where(lvl1tm => lvl1tm.LevelTypeId == 1).Select(lvl => new LevelTeamVM
                                {
                                    EmailId = lvl.EmailId,
                                    LevelTeamId = lvl.LevelTeamId,
                                    PhoneNumber = lvl.PhoneNumber,
                                    TeamId = lvl.TeamId,
                                    TeamName = lvl.TeamName,
                                    CommissionRate = lvl.Level1CommissionRate,
                                    Users = lvl.Users,
                                    Premium = lvl.Premium
                                }).ToList();

                                objTeamViewData.Level2TeamDetail = allLevelTeam.Where(lvl1tm => lvl1tm.LevelTypeId == 2).Select(lvl => new LevelTeamVM
                                {
                                    EmailId = lvl.EmailId,
                                    LevelTeamId = lvl.LevelTeamId,
                                    PhoneNumber = lvl.PhoneNumber,
                                    TeamId = lvl.TeamId,
                                    TeamName = lvl.TeamName,
                                    CommissionRate = lvl.Level2CommissionRate,
                                    Users = lvl.Users,
                                    Premium = lvl.Premium
                                }).ToList();
                            }
                            int totauser = 0;
                            int totalprimum = 0;
                            decimal level1commission = 0.0M;
                            decimal level2commission = 0.0M;
                            objTeamViewData.Level1TeamDetail.ForEach(tm =>
                            {
                                tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                                level1commission = level1commission + primaryteamDetails.Level1CommissionRate * tm.Premium;
                                totauser = totauser + tm.Users;
                                totalprimum = totalprimum + tm.Premium;

                            });
                            objTeamViewData.TotalLevel1Users = totauser;
                            objTeamViewData.TotalLevel1Premiums = totalprimum;
                            objTeamViewData.Level1TeamsCommision = 0;
                            objTeamViewData.Level1TeamsCommision = level1commission;
                            level1commission = 0;
                            totauser = 0;
                            totalprimum = 0;
                            objTeamViewData.Level2TeamDetail.ForEach(tm =>
                            {
                                tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                                level2commission = level2commission + primaryteamDetails.Level2CommissionRate * tm.Premium;
                                totauser = totauser + tm.Users;
                                totalprimum = totalprimum + tm.Premium;

                            });
                            objTeamViewData.TotalLevel2Users = totauser;
                            objTeamViewData.TotalLevel2Premiums = totalprimum;
                            objTeamViewData.Level2TeamsCommision = 0;
                            objTeamViewData.Level2TeamsCommision = level2commission;
                            level2commission = 0;
                            objTeamViewData.TotalTeamCommision = objTeamViewData.PrimaryTeamCommision + objTeamViewData.Level1TeamsCommision + objTeamViewData.Level2TeamsCommision;
                            totauser = 0;
                            totalprimum = 0;
                        }
                    }
                    return objTeamViewData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("DeleteLevelTeams  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Team Report Commision for sending email format
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="primaryTeamCommissionReportId"></param>
        /// <param name="currentMonthDateTime"></param>
        /// <returns></returns>
        public static TeamViewData GetTeamReportCommision(int teamId, long primaryTeamCommissionReportId, DateTime currentMonthDateTime)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeamReportCommision() of  TeamId=" + teamId);
                    TeamViewData objTeamViewData = new TeamViewData();
                    if (primaryTeamCommissionReportId > 0 && teamId > 0)
                    {
                        currentMonthDateTime = currentMonthDateTime.AddMonths(-1);
                        // DateTime startDate = new DateTime(currentMonthDateTime.Year, currentMonthDateTime.Month, 1);
                        // DateTime endDate = new DateTime(currentMonthDateTime.Year, currentMonthDateTime.Month, DateTime.DaysInMonth(currentMonthDateTime.Year, currentMonthDateTime.Month));
                        LevelTeamVM primaryteamDetails = (from team in dataContext.Teams
                                                          join primeTeam in dataContext.TeamCommissionReport
                                                          on team.TeamId equals primeTeam.TeamId
                                                          where primeTeam.TeamId == teamId && primeTeam.TeamCommissionReportId == primaryTeamCommissionReportId
                                                          select new LevelTeamVM
                                                          {
                                                              TeamName = team.TeamName,
                                                              TeamId = team.TeamId,
                                                              PhoneNumber = team.PhoneNumber,
                                                              EmailId = team.EmailId,
                                                              CommissionRate = primeTeam.TeamPrimaryCommissionRate,
                                                              Level1CommissionRate = primeTeam.Level1CommissionRate,
                                                              Level2CommissionRate = primeTeam.Level2CommissionRate,
                                                              Users = primeTeam.NumberOfActiveUser,
                                                              Premium = primeTeam.NumberOfPremiumUser
                                                          }).FirstOrDefault();
                        if (primaryteamDetails != null)
                        {
                            primaryteamDetails.TeamName = string.IsNullOrEmpty(primaryteamDetails.TeamName) ? string.Empty : primaryteamDetails.TeamName.Substring(1);
                            objTeamViewData.PrimaryTeamDetail = primaryteamDetails;
                            objTeamViewData.PrimaryTeamCommision = 0;
                            objTeamViewData.PrimaryTeamCommision = primaryteamDetails.Premium * primaryteamDetails.CommissionRate;
                            var allLevelTeam = (from team in dataContext.Teams
                                                join levelTeam in dataContext.TeamLevelCommissionReport
                                                on team.TeamId equals levelTeam.LevelTeamId
                                                where levelTeam.PrimaryTeamId == primaryteamDetails.TeamId && levelTeam.TeamCommissionReportId == primaryTeamCommissionReportId
                                                orderby team.TeamName
                                                select new LevelTeamVM
                                                {
                                                    TeamName = team.TeamName,
                                                    TeamId = team.TeamId,
                                                    LevelTeamId = levelTeam.LevelTeamId,
                                                    PhoneNumber = team.PhoneNumber,
                                                    EmailId = team.EmailId,
                                                    LevelTypeId = levelTeam.LevelTypeId,
                                                    Level1CommissionRate = team.Level1CommissionRate,
                                                    Level2CommissionRate = team.Level2CommissionRate,
                                                    Users = levelTeam.NumberOfActiveUser,
                                                    Premium = levelTeam.NumberOfPremiumUser
                                                }).ToList();
                            if (allLevelTeam != null)
                            {
                                objTeamViewData.Level1TeamDetail = allLevelTeam.Where(lvl1tm => lvl1tm.LevelTypeId == 1).Select(lvl => new LevelTeamVM
                                {
                                    EmailId = lvl.EmailId,
                                    LevelTeamId = lvl.LevelTeamId,
                                    PhoneNumber = lvl.PhoneNumber,
                                    TeamId = lvl.TeamId,
                                    TeamName = lvl.TeamName,
                                    CommissionRate = lvl.Level1CommissionRate,
                                    Users = lvl.Users,
                                    Premium = lvl.Premium
                                }).ToList();

                                objTeamViewData.Level2TeamDetail = allLevelTeam.Where(lvl1tm => lvl1tm.LevelTypeId == 2).Select(lvl => new LevelTeamVM
                                {
                                    EmailId = lvl.EmailId,
                                    LevelTeamId = lvl.LevelTeamId,
                                    PhoneNumber = lvl.PhoneNumber,
                                    TeamId = lvl.TeamId,
                                    TeamName = lvl.TeamName,
                                    CommissionRate = lvl.Level2CommissionRate,
                                    Users = lvl.Users,
                                    Premium = lvl.Premium
                                }).ToList();
                            }
                            int totauser = 0;
                            int totalprimum = 0;
                            decimal level1commission = 0;
                            decimal level2commission = 0;
                            objTeamViewData.Level1TeamDetail.ForEach(tm =>
                            {

                                tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                                level1commission = level1commission + primaryteamDetails.Level1CommissionRate * tm.Premium;
                                totauser = totauser + tm.Users;
                                totalprimum = totalprimum + tm.Premium;

                            });
                            objTeamViewData.TotalLevel1Users = totauser;
                            objTeamViewData.TotalLevel1Premiums = totalprimum;
                            objTeamViewData.Level1TeamsCommision = 0;
                            objTeamViewData.Level1TeamsCommision = level1commission;
                            level1commission = 0;
                            totauser = 0;
                            totalprimum = 0;
                            objTeamViewData.Level2TeamDetail.ForEach(tm =>
                            {
                                tm.TeamName = string.IsNullOrEmpty(tm.TeamName) ? string.Empty : tm.TeamName.Substring(1);
                                level2commission = level2commission + primaryteamDetails.Level2CommissionRate * tm.Premium;
                                totauser = totauser + tm.Users;
                                totalprimum = totalprimum + tm.Premium;
                            });
                            objTeamViewData.TotalLevel2Users = totauser;
                            objTeamViewData.TotalLevel2Premiums = totalprimum;
                            objTeamViewData.Level2TeamsCommision = 0;
                            objTeamViewData.Level2TeamsCommision = level2commission;
                            level2commission = 0;
                            objTeamViewData.TotalTeamCommision = objTeamViewData.PrimaryTeamCommision + objTeamViewData.Level1TeamsCommision + objTeamViewData.Level2TeamsCommision;
                            totauser = 0;
                            totalprimum = 0;
                        }
                    }
                    return objTeamViewData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTeamReportCommision  end() : --- " + DateTime.Now.ToLongDateString());
                   // LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Save primary Team Commission Report and their level team commsiion details
        /// </summary>
        /// <param name="currentMonthDateTime"></param>
        /// <param name="emailTempletePath"></param>
        /// <returns></returns>
        public static bool SaveTeamCommissionReport(DateTime currentMonthDateTime, string emailTempletePath = null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    List<tblTeam> teamlist = dataContext.Teams.ToList();
                    if (teamlist != null && teamlist.Count > 0)
                    {
                        DateTime teamReportDate = currentMonthDateTime;
                        currentMonthDateTime = currentMonthDateTime.AddMonths(-1);
                        DateTime startDate = new DateTime(currentMonthDateTime.Year, currentMonthDateTime.Month, 1);
                        DateTime endDate = new DateTime(currentMonthDateTime.Year, currentMonthDateTime.Month, DateTime.DaysInMonth(currentMonthDateTime.Year, currentMonthDateTime.Month));
                        EmailService emailservice = new EmailService();
                        teamlist.ForEach(tms =>
                        {
                            if (!dataContext.TeamCommissionReport.Any(teamrpt => teamrpt.TeamId == tms.TeamId && teamrpt.ReportMonth == currentMonthDateTime.Month
                                                                 && teamrpt.ReportYear == currentMonthDateTime.Year))
                            {
                                tblTeamCommissionReport teamCommisionreprt = new tblTeamCommissionReport()
                                {
                                    Level1CommissionRate = tms.Level1CommissionRate,
                                    Level2CommissionRate = tms.Level2CommissionRate,
                                    TeamPrimaryCommissionRate = tms.PrimaryCommissionRate,
                                    ReportGereratedDate = DateTime.Now,
                                    ReportYear = currentMonthDateTime.Year,
                                    ReportMonth = currentMonthDateTime.Month,
                                    TeamId = tms.TeamId,
                                    NumberOfPremiumUser = GetTeamPrimumUsersCount(dataContext, tms.TeamId),
                                    NumberOfActiveUser = GetTeamUsersCount(dataContext, tms.TeamId),
                                };
                                dataContext.TeamCommissionReport.Add(teamCommisionreprt);
                                dataContext.SaveChanges();
                                long teamCommissionReportId = teamCommisionreprt.TeamCommissionReportId;
                                // save the Level1 and Level Team.
                                var allLevelTeam = (from team in dataContext.Teams
                                                    join levelTeam in dataContext.LevelTeams
                                                    on team.TeamId equals levelTeam.LevelTeamId
                                                    where levelTeam.PrimaryTeamId == tms.TeamId
                                                    orderby team.TeamName
                                                    select new TeamLevelCommisionVM
                                                    {
                                                        PrimaryTeamId = tms.TeamId,
                                                        TeamCommissionReportId = teamCommissionReportId,
                                                        LevelTeamId = levelTeam.LevelTeamId,
                                                        LevelTypeId = levelTeam.LevelTypeId,
                                                    }).ToList();
                                List<tblTeamLevelCommissionReport> listTeamlevel = new List<tblTeamLevelCommissionReport>();
                                allLevelTeam.ForEach(tm =>
                                {
                                    tblTeamLevelCommissionReport objTeamLevel = new tblTeamLevelCommissionReport()
                                    {
                                        NumberOfPremiumUser = GetTeamPrimumUsersCount(dataContext, tm.LevelTeamId),
                                        NumberOfActiveUser = GetTeamUsersCount(dataContext, tm.LevelTeamId),
                                        PrimaryTeamId = tm.PrimaryTeamId,
                                        LevelTeamId = tm.LevelTeamId,
                                        LevelTypeId = tm.LevelTypeId,
                                        TeamCommissionReportId = tm.TeamCommissionReportId
                                    };
                                    listTeamlevel.Add(objTeamLevel);
                                });
                                dataContext.TeamLevelCommissionReport.AddRange(listTeamlevel);
                                dataContext.SaveChanges();
                                // To get all data to send in mail Id
                                TeamViewData teamDetails = GetTeamReportCommision(tms.TeamId, teamCommissionReportId, currentMonthDateTime);
                                if (!string.IsNullOrEmpty(emailTempletePath))
                                {
                                    string messagebody = emailservice.CreateEmailBody(emailTempletePath);
                                    if (teamDetails != null)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendLine("<tr>");
                                        sb.AppendLine("<tr class=\"row\">");
                                        sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + teamDetails.PrimaryTeamDetail.TeamName + "</td>");
                                        sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + teamDetails.PrimaryTeamDetail.EmailId + "</td>");
                                        sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + teamDetails.PrimaryTeamDetail.PhoneNumber + "</td>");
                                        sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + teamDetails.PrimaryTeamDetail.Users + "</td>");
                                        sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + teamDetails.PrimaryTeamDetail.Premium + "</td>");
                                        sb.AppendLine("</tr>");
                                        messagebody = messagebody.Replace("{PrimaryTeamDetail}", sb.ToString());
                                        sb.Clear();
                                        foreach (var item in teamDetails.Level1TeamDetail)
                                        {
                                            sb.AppendLine("<tr>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.TeamName + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.EmailId + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.PhoneNumber + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.Users + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.Premium + "</td>");
                                            sb.AppendLine("</tr>");
                                        }
                                        messagebody = messagebody.Replace("{Level1TeamDetail}", sb.ToString());
                                        sb.Clear();
                                        foreach (var item in teamDetails.Level2TeamDetail)
                                        {
                                            sb.AppendLine("<tr>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.TeamName + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.EmailId + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.PhoneNumber + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.Users + "</td>");
                                            sb.AppendLine("<td style=\"padding: 5px 14px; line-height: 20px; text-align: center; border: 1px solid #ccc;color: #656565;font-family: 'open_sanssemibold';\">" + item.Premium + "</td>");
                                            sb.AppendLine("</tr>");
                                        }
                                        messagebody = messagebody.Replace("{Level2TeamDetail}", sb.ToString());
                                        sb.Clear();
                                        string MonthLevel = GetMonthName(currentMonthDateTime.Month) + " " + currentMonthDateTime.Year;
                                        messagebody = messagebody.Replace("{MailBody}", string.Format(Message.EmailMainHeading, teamDetails.PrimaryTeamDetail.TeamName));
                                        messagebody = messagebody.Replace("{CurrentMonth}", string.Format(Message.CommissionMonthLevel, MonthLevel));
                                        messagebody = messagebody.Replace("{PrimaryTeamTotal}", "$ " + Convert.ToString(teamDetails.PrimaryTeamCommision));
                                        messagebody = messagebody.Replace("{TotalLevel1Teams}", "$ " + Convert.ToString(teamDetails.Level1TeamsCommision));
                                        messagebody = messagebody.Replace("{TotalLevel2Teams}", "$ " + Convert.ToString(teamDetails.Level2TeamsCommision));
                                        messagebody = messagebody.Replace("{Total}", "$ " + Convert.ToString(teamDetails.TotalTeamCommision));

                                        emailservice.SendMail(tms.EmailId, messagebody, "Fitcom Commissions Details");
                                    }
                                }
                            }
                        });
                    }
                    return true;
                }
                catch (Exception)
                {
                    throw;
                    // return false;
                }
                finally
                {
                    traceLog.AppendLine("End:Main() Response Result Status-" + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                  //  LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get MonthName based on Month number
        /// </summary>
        /// <param name="monthNum"></param>
        /// <returns></returns>
        private static string GetMonthName(int monthNum)
        {
            string monthName = "January";
            switch (monthNum)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "Febuary";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;

            }
            return monthName;
        }
        /// <summary>
        /// Get Team  Generetaed Commission Year
        /// </summary>
        /// <param name="searchCommissionRequest"></param>
        /// <returns></returns>
        public static List<CommisionYear> GetTeamCommissionYear(TeamCommisionRequest searchCommissionRequest)
        {
            StringBuilder traceLog = new StringBuilder();
            traceLog.AppendLine("Start: GetTeamCommissionYear() of  TeamId=" + searchCommissionRequest.TeamId);
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    var teamcommisisonYears = (from tc in dataContext.TeamCommissionReport
                                               where tc.TeamId == searchCommissionRequest.TeamId
                                               select new
                                               {
                                                   Year = tc.ReportYear
                                               }).Distinct().ToList();
                    if (teamcommisisonYears != null)
                    {
                        List<CommisionYear> resonse = teamcommisisonYears.Select(cm => new CommisionYear()
                        {
                            Year = cm.Year
                        }).OrderByDescending(yrs => yrs.Year).ToList();
                        return resonse;
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw;
                    // return false;
                }
                finally
                {
                    traceLog.AppendLine("End:GetTeamCommissionYear() Response Result Status-" + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Team Commission Year based on team
        /// </summary>
        /// <param name="searchCommissionRequest"></param> 
        /// <returns></returns>
        public static List<CommisionMonth> GetTeamCommissionMonth(TeamCommisionRequest searchCommissionRequest)
        {
            StringBuilder traceLog = new StringBuilder();
            traceLog.AppendLine("Start: GetTeamCommissionMonth() of  TeamId=" + searchCommissionRequest.TeamId);
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    var teamcommisionMonths = (from tc in dataContext.TeamCommissionReport
                                               where tc.TeamId == searchCommissionRequest.TeamId
                                               && tc.ReportYear == searchCommissionRequest.Year
                                               select new
                                               {
                                                   Month = tc.ReportMonth
                                               }).Distinct().ToList();
                    if (teamcommisionMonths != null)
                    {
                        List<CommisionMonth> resonse = teamcommisionMonths.Select(cm => new CommisionMonth()
                        {
                            Month = cm.Month,
                            Name = GetMonthName(cm.Month)
                        }).OrderBy(mn => mn.Month).ToList();
                        traceLog.AppendLine("Start: GetTeamCommissionYear() of  TeamId=" + searchCommissionRequest.TeamId);
                        return resonse;
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw;
                    // return false;
                }
                finally
                {
                    traceLog.AppendLine("End:GetTeamCommissionYear() Response Result Status-" + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

    }
}