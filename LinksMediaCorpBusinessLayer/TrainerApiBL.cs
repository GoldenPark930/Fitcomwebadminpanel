
namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using LinksMediaCorpEntity;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Web;
    public class TrainerApiBL
    {
        /// <summary>
        /// Get trainer challenge list with free from for trainer
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/26/2016
        /// </devdoc>
        public static List<MainChallengeVM> GetTrainerLibraryChallengeListWithFreeForm(ref bool isTeamJoined)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibraryChallengeListWithFreeForm---- " + DateTime.Now.ToLongDateString());
                    int teamId = -1;
                    List<int> trainerCrediId = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        var objtrainer = dataContext.Trainer.FirstOrDefault(u => u.TrainerId == cred.UserId);
                        if (objtrainer != null)
                        {
                            teamId = objtrainer.TeamId;
                        }
                        trainerCrediId.Add(cred.Id);
                    }
                    else if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        var objuser = dataContext.User.FirstOrDefault(u => u.UserId == cred.UserId);
                        if (objuser != null)
                        {
                            teamId = objuser.TeamId;
                        }
                        trainerCrediId = (from tr in dataContext.Trainer
                                          join crd in dataContext.Credentials
                                          on tr.TrainerId equals crd.UserId
                                          where crd.UserType == Message.UserTypeTrainer && tr.TeamId == teamId
                                          select crd.Id
                                          ).ToList();

                    }
                    isTeamJoined = teamId > 0 ? true : false;
                    List<MainChallengeVM> listMainVM = (from c in dataContext.Challenge
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        where c.IsActive && trainerCrediId.Contains(c.TrainerId)
                                                        orderby c.CreatedDate descending
                                                        select new MainChallengeVM
                                                        {
                                                            ChallengeId = c.ChallengeId,
                                                            ChallengeName = c.ChallengeName,
                                                            DifficultyLevel = c.DifficultyLevel,
                                                            ChallengeType = ct.ChallengeType,
                                                            Description = c.Description,
                                                            IsSubscription = c.IsSubscription,
                                                            TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                              join bp in dataContext.Equipments
                                                                              on trzone.EquipmentId equals bp.EquipmentId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.Equipment).Distinct().ToList<string>(),
                                                            TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                              join bp in dataContext.BodyPart
                                                                              on trzone.PartId equals bp.PartId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.PartName).Distinct().ToList<string>(),
                                                            Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                            ResultUnit = ct.ResultUnit,
                                                            IsWellness = (ct.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false,
                                                        }).ToList();
                    if (listMainVM != null && listMainVM.Count > 0)
                    {
                        listMainVM.ForEach(r =>
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                            if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                            {
                                r.TargetZone = string.Join(", ", r.TempTargetZone);
                            }
                            r.TempTargetZone = null;
                            if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                            {
                                r.Equipment = string.Join(", ", r.TempEquipments);
                            }
                            r.TempEquipments = null;
                        });
                        //Challenge feed sorted by acceptors
                        listMainVM = listMainVM.OrderByDescending(chlng => chlng.Strenght).ToList();
                    }
                    return listMainVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTrainerLibraryChallengeListWithFreeForm : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get FreePremium ChallengeList basd on team Id
        /// </summary>
        /// <param name="isTeamJoined"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/26/2016
        /// </devdoc>
        public static PremimumChallengeVM GetFreePremiumChallengeList(ref bool isTeamJoined)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetFreePremiumChallengeList---- " + DateTime.Now.ToLongDateString());
                    PremimumChallengeVM objPremimumChallengeVM = new PremimumChallengeVM();
                    List<int> trainerCrediId = new List<int>();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<int> teamIds = new List<int>();
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
                    if (teamIds.Count > 0)
                    {
                        trainerCrediId = (from crd in dataContext.Credentials
                                          join tms in dataContext.TrainerTeamMembers
                                          on crd.Id equals tms.UserId
                                          where teamIds.Contains(tms.TeamId) && crd.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase)
                                          select tms.UserId).ToList();
                    }
                    if (teamIds != null && teamIds.Count > 0)
                    {
                        isTeamJoined = true;
                    }
                    List<MainChallengeVM> listMainVM = (from c in dataContext.Challenge
                                                        join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                        where c.IsActive
                                                        && (trainerCrediId.Contains(c.TrainerId) || (c.IsPremium && c.TrainerId == 0))
                                                        orderby c.ChallengeName ascending
                                                        select new MainChallengeVM
                                                        {
                                                            ChallengeId = c.ChallengeId,
                                                            ChallengeName = c.ChallengeName,
                                                            DifficultyLevel = c.DifficultyLevel,
                                                            ChallengeType = ct.ChallengeType,
                                                            Description = c.Description,
                                                            IsSubscription = c.IsSubscription,
                                                            TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                              join bp in dataContext.Equipments
                                                                              on trzone.EquipmentId equals bp.EquipmentId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.Equipment).Distinct().ToList<string>(),
                                                            TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                              join bp in dataContext.BodyPart
                                                                              on trzone.PartId equals bp.PartId
                                                                              where trzone.ChallengeId == c.ChallengeId
                                                                              select bp.PartName).Distinct().ToList<string>(),
                                                            Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                            ResultUnit = ct.ResultUnit
                                                        }).ToList();
                    if (listMainVM != null && listMainVM.Count > 0)
                    {
                        listMainVM.ForEach(r =>
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                            if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                            {
                                r.TargetZone = string.Join(", ", r.TempTargetZone);
                            }
                            r.TempTargetZone = null;
                            if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                            {
                                r.Equipment = string.Join(", ", r.TempEquipments);
                            }
                            r.TempEquipments = null;
                        });
                        //Challenge feed sorted by acceptors
                        listMainVM = listMainVM.OrderBy(chlng => chlng.ChallengeName).ToList();
                    }

                    objPremimumChallengeVM.PremimumChallegeList = listMainVM;
                    objPremimumChallengeVM.PremimumTypeList = ChallengesCommonBL.GetPremiumChallengeCategoryList();

                    var challengeCategoryList = ChallengesCommonBL.GetapiPreminumChallengeCategoryList(ConstantHelper.constWorkoutChallengeSubType, trainerCrediId);
                    if (challengeCategoryList != null)
                    {
                        objPremimumChallengeVM.PremimumWorksoutList = challengeCategoryList.ChallengeCategoryList;
                        objPremimumChallengeVM.FeaturedList = challengeCategoryList.FeaturedList;
                        objPremimumChallengeVM.TrendingList = challengeCategoryList.TrendingCategoryList;
                    }
                    return objPremimumChallengeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetFreePremiumChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get TrainerLibrary ChallengeList
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isTeamJoined"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/26/2016
        /// </devdoc>
        public static ChallengeTabVM GetTrainerLibraryChallengeList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerLibraryChallengeList---- " + DateTime.Now.ToLongDateString());
                    int trainerCredId = -1;
                    ChallengeTabVM objChallengeTabVM = new ChallengeTabVM();
                    if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        trainerCredId = (from tr in dataContext.Trainer
                                         join crd in dataContext.Credentials
                                         on tr.TrainerId equals crd.UserId
                                         where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == userId
                                         select crd.Id).FirstOrDefault();
                    }
                    if (trainerCredId > 0)
                    {
                        List<MainChallengeVM> listMainVM = (from c in dataContext.Challenge
                                                            join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                            where c.IsActive && c.TrainerId == trainerCredId
                                                            orderby c.ChallengeName ascending
                                                            select new MainChallengeVM
                                                            {
                                                                ChallengeId = c.ChallengeId,
                                                                ChallengeName = c.ChallengeName,
                                                                DifficultyLevel = c.DifficultyLevel,
                                                                ChallengeType = ct.ChallengeType,
                                                                Description = c.Description,
                                                                IsSubscription = c.IsSubscription,
                                                                TempEquipments = (from trzone in dataContext.ChallengeEquipmentAssociations
                                                                                  join bp in dataContext.Equipments
                                                                                  on trzone.EquipmentId equals bp.EquipmentId
                                                                                  where trzone.ChallengeId == c.ChallengeId
                                                                                  select bp.Equipment).Distinct().ToList<string>(),
                                                                TempTargetZone = (from trzone in dataContext.TrainingZoneCAssociations
                                                                                  join bp in dataContext.BodyPart
                                                                                  on trzone.PartId equals bp.PartId
                                                                                  where trzone.ChallengeId == c.ChallengeId
                                                                                  select bp.PartName).Distinct().ToList<string>(),
                                                                Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                ResultUnit = ct.ResultUnit,
                                                                IsWellness = (ct.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType) ? true : false,
                                                            }).ToList();
                        if (listMainVM != null && listMainVM.Count > 0)
                        {
                            listMainVM.ForEach(r =>
                            {
                                r.ChallengeType = r.ChallengeType.Split(' ')[0];
                                if (r.TempTargetZone != null && r.TempTargetZone.Count > 0)
                                {
                                    r.TargetZone = string.Join(", ", r.TempTargetZone);
                                }
                                r.TempTargetZone = null;
                                if (r.TempEquipments != null && r.TempEquipments.Count > 0)
                                {
                                    r.Equipment = string.Join(", ", r.TempEquipments);
                                }
                                r.TempEquipments = null;
                            });

                            int totalcount = listMainVM.Count;
                            listMainVM = (from l in listMainVM
                                          select l).Skip(startIndex).Take(endIndex - startIndex).ToList();

                            if ((totalcount) > endIndex)
                            {
                                objChallengeTabVM.IsMoreAvailable = true;
                            }
                            //Challenge feed sorted by acceptors
                            if (listMainVM != null)
                            {
                                listMainVM = listMainVM.OrderBy(chlng => chlng.ChallengeName).ToList();
                            }
                            objChallengeTabVM.ChallengeList = listMainVM;
                        }
                    }
                    return objChallengeTabVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetTrainerLibraryChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Select UserPersonalTrainer
        /// </summary>
        /// <param name="usertrainerdetails"></param>
        /// <returns></returns>
        public static bool SelectUserPersonalTrainer(UserPersonalTrainerVM usertrainerdetails)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: SelectUserPersonalTrainer---- " + DateTime.Now.ToLongDateString());
                    if (usertrainerdetails.TrainerCredID > 0)
                    {
                        Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        if (cred.UserType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                        {
                            tblUser user = dataContext.User.FirstOrDefault(usr => usr.UserId == cred.UserId);
                            if (user != null)
                            {
                                user.PersonalTrainerCredId = usertrainerdetails.TrainerCredID;
                                dataContext.SaveChanges();
                                if (usertrainerdetails.TrainerCredID > 0)
                                {
                                    string selectedUserName = string.Empty;
                                    selectedUserName = user.FirstName + " " + user.LastName;
                                    NotificationApiBL.SendSelectPrimaryTrainerNotificationToTrainer(usertrainerdetails.TrainerCredID, selectedUserName, cred.UserId, cred.UserType);
                                }
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
                    traceLog.AppendLine("End  GetTrainerLibraryChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Trainer Team all Users
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<TeamUserVM>> GetTrainerTeamUsers(int userId, string userType, int startIndex, int endIndex, bool isMTActive)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTrainerTeamUsers-userId-" + Convert.ToString(userId) + ",UserType-" + userType);
                    List<int> teamIds = new List<int>();
                    int trainerCredId = 0;
                    Total<List<TeamUserVM>> objresult = new Total<List<TeamUserVM>>();
                    var trainer = userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase) ? (from crd in dataContext.Credentials
                                                                                                                  join trainersteams in dataContext.TrainerTeamMembers
                                                                                                                   on crd.Id equals trainersteams.UserId
                                                                                                                  where crd.UserId == userId && crd.UserType == Message.UserTypeTrainer
                                                                                                                  select new
                                                                                                                  {
                                                                                                                      crd.Id,
                                                                                                                      trainersteams.TeamId,
                                                                                                                      crd.EmailId
                                                                                                                  }).ToList() : null;
                    if (trainer != null)
                    {
                        teamIds = trainer.Select(tr => tr.TeamId).ToList();
                        trainerCredId = trainer.Select(tr => tr.Id).FirstOrDefault();
                    }
                    if (teamIds != null && teamIds.Count > 0)
                    {
                        List<TeamUserVM> objUserList = (from cred in dataContext.Credentials
                                                        join usr in dataContext.User on cred.UserId equals usr.UserId
                                                        where teamIds.Contains(usr.TeamId) && cred.UserType == Message.UserTypeUser
                                                        select new TeamUserVM
                                                        {
                                                            CredID = cred.Id,
                                                            ID = cred.UserId,
                                                            FullName = usr.FirstName + " " + usr.LastName,
                                                            ImageUrl = usr.UserImageUrl,
                                                            UserType = cred.UserType,
                                                            UserId = cred.UserId,
                                                            PersonalTrainerId = usr.PersonalTrainerCredId,
                                                            IsMTActive = usr.MTActive,
                                                            EmailId = cred.EmailId
                                                        }).ToList();
                        if (objUserList != null && objUserList.Count() > 0)
                        {
                            // Remove the duplicate user from  team group
                            objUserList = objUserList.GroupBy(ul => ul.ID)
                                                             .Select(grp => grp.FirstOrDefault())
                                                             .ToList();
                            objUserList.ForEach(user =>
                            {
                                user.IsPersonalTrainer = (user.PersonalTrainerId == trainerCredId);
                                user.ImageUrl = (user.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : null;
                                user.OffLineChatCount = dataContext.ChatHistory.Count(crd => crd.ReceiverCredId == trainerCredId && crd.SenderCredId == user.CredID && !crd.IsRead);
                                DateTime userChatSendDateTime = DateTime.MinValue;
                                DateTime userAssignmentSendDateTime = DateTime.MinValue;
                                if (user.OffLineChatCount > 0)
                                {
                                    userChatSendDateTime = dataContext.ChatHistory.Where(crd => crd.ReceiverCredId == trainerCredId && crd.SenderCredId == user.CredID && !crd.IsRead).OrderByDescending(ch => ch.TrasactionDateTime).Select(cc => cc.TrasactionDateTime).FirstOrDefault();
                                }
                                user.CompletedAssignmentCount = dataContext.UserAssignments.Count(crd => crd.SubjectId == trainerCredId && crd.TargetId == user.CredID && crd.IsCompleted && !crd.IsRead);
                                if (user.CompletedAssignmentCount > 0)
                                {
                                    var assignmentSendDateTime = dataContext.UserAssignments.Where(crd => crd.SubjectId == trainerCredId && crd.TargetId == user.CredID && crd.IsCompleted && !crd.IsRead).OrderByDescending(ch => ch.ChallengeCompletedDate).Select(cc => cc.ChallengeCompletedDate).FirstOrDefault();
                                    userAssignmentSendDateTime = assignmentSendDateTime ?? DateTime.MinValue;
                                }
                                int result = DateTime.Compare(userChatSendDateTime, userAssignmentSendDateTime);
                                if (result > 0 || result == 0)
                                {
                                    user.LatestTrainerUserNotifyDateTime = userChatSendDateTime;
                                }
                                else
                                {
                                    user.LatestTrainerUserNotifyDateTime = userAssignmentSendDateTime;
                                }
                            });

                            if (isMTActive)
                            {
                                objUserList = (from l in objUserList
                                               where l.IsMTActive && (l.CompletedAssignmentCount > 0 || l.OffLineChatCount > 0)
                                               orderby l.LatestTrainerUserNotifyDateTime descending
                                               select l)
                                                .Union(
                                                from ll in objUserList
                                                where ll.IsMTActive && !(ll.CompletedAssignmentCount > 0 || ll.OffLineChatCount > 0)
                                                orderby ll.FullName ascending
                                                select ll
                                                ).ToList();

                                objresult.TotalList = (from l in objUserList
                                                       select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                            }
                            else
                            {

                                objUserList = (from l in objUserList
                                               where (l.CompletedAssignmentCount > 0 || l.OffLineChatCount > 0)
                                               orderby l.LatestTrainerUserNotifyDateTime descending
                                               select l)
                                              .Union(
                                              from ll in objUserList
                                              where !(ll.CompletedAssignmentCount > 0 || ll.OffLineChatCount > 0)
                                              orderby ll.FullName ascending
                                              select ll
                                              ).ToList();
                                objresult.TotalList = (from l in objUserList
                                                       select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                            }

                            objresult.TotalCount = objUserList.Count();
                            if ((objresult.TotalCount) > endIndex)
                            {
                                objresult.IsMoreAvailable = true;
                            }
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
                    traceLog.AppendLine("End: GetTrainerTeamUsers: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}