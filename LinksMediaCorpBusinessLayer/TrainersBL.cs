namespace LinksMediaCorpBusinessLayer
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using AutoMapper;
    using LinksMediaCorpUtility;
    using System.Data.Entity.Validation;
    using System.Web;
    using LinksMediaCorpUtility.Resources;
    using System.Threading;
    using System.IO;
    using System.Globalization;

    public class TrainersBL
    {
        /// <summary>
        /// Function to get specialization fron the database  
        /// </summary>
        /// <returns>List<Specialization></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<Specialization> GetSpecializations()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetSpecializations for retrieving Specializations from database ");
                    Mapper.CreateMap<tblSpecialization, Specialization>();
                    List<tblSpecialization> lstSpecialization = dataContext.Specialization.ToList();
                    List<Specialization> lstChalangeTypeVM =
                        Mapper.Map<List<tblSpecialization>, List<Specialization>>(lstSpecialization);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetSpecializations  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get top three specialization fron the database  
        /// </summary>
        /// <returns>List<Specialization></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<Specialization> GetTopThreeSpecializations(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTopThreeSpecializations for retrieving Specializations from database ");
                    Mapper.CreateMap<tblSpecialization, Specialization>();
                    List<tblSpecialization> lstSpecialization = (from S in dataContext.Specialization
                                                                 join TS in dataContext.TrainerSpecialization on S.SpecializationId equals TS.SpecializationId
                                                                 where TS.TrainerId == trainerId && TS.IsInTopThree == 1
                                                                 select S).ToList<tblSpecialization>();
                    List<Specialization> lstChalangeTypeVM =
                        Mapper.Map<List<tblSpecialization>, List<Specialization>>(lstSpecialization);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTopThreeSpecializations  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get all selected team Id
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        public static List<DDTeams> GetTrainerTeams(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerTeams() fro get all selected teams from database " + trainerId);
                    if (!(trainerId > 0))
                    {
                        return null;
                    }
                    return (from tm in dataContext.TrainerTeamMembers
                            join t in dataContext.Teams
                            on tm.TeamId equals t.TeamId
                            where tm.UserId == trainerId
                            select new DDTeams
                            {
                                TeamId = tm.TeamId,
                                TeamName = t.TeamName,
                                IsDefaultTeam = t.IsDefaultTeam
                            }).ToList();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerTeams  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get secondary specialization fron the database  
        /// </summary>
        /// <returns>List<Specialization></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<Specialization> GetSecondarySpecializations(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetSecondarySpecializations for retrieving Specializations from database ");
                    Mapper.CreateMap<tblSpecialization, Specialization>();
                    List<tblSpecialization> lstSpecialization = (from S in dataContext.Specialization
                                                                 join TS in dataContext.TrainerSpecialization on S.SpecializationId equals TS.SpecializationId
                                                                 where TS.TrainerId == trainerId && TS.IsInTopThree == 0
                                                                 select S).ToList<tblSpecialization>();

                    List<Specialization> lstChalangeTypeVM = Mapper.Map<List<tblSpecialization>, List<Specialization>>(lstSpecialization);
                    return lstChalangeTypeVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetSecondarySpecializations  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Trainer Mobile CoachTeam
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        public static List<DDTeams> GetTrainerMobileCoachTeam(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerMobileCoachTeam for retrieving Specializations from database ");
                    List<DDTeams> setedeMCoachTeams = (from tm in dataContext.TrainerMobileCoachTeams
                                                       join t in dataContext.Teams
                                                       on tm.TeamId equals t.TeamId
                                                       where tm.TrainerCredId == trainerId
                                                       select new DDTeams
                                                       {
                                                           TeamId = tm.TeamId,
                                                           TeamName = t.TeamName,
                                                           IsDefaultTeam = t.IsDefaultTeam
                                                       }).ToList();
                    return setedeMCoachTeams;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerMobileCoachTeam  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get trainres fron the database  
        /// </summary>
        /// <returns>List<Specialization></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<ViewTrainers> GetTrainers(string searchTrainer=null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainers for retrieving trainer from database ");

                    List<ViewTrainers> trainers = (from T in dataContext.Trainer
                                                   join Crd in dataContext.Credentials
                                                    on T.TrainerId equals Crd.UserId
                                                   join S in dataContext.States on T.State equals S.StateCode
                                                   join C in dataContext.Cities on T.City equals C.CityId
                                                   where Crd.UserType == Message.UserTypeTrainer
                                                   orderby T.ModifiedDate descending
                                                   select new ViewTrainers
                                                   {
                                                       TrainerName = T.FirstName + " " + T.LastName,
                                                       CreatedDate = T.CreatedDate,
                                                       TrainerId = T.TrainerId,
                                                       EnteredTrainerID = T.EnteredTrainerID,
                                                       Address = C.CityName + ", " + S.StateName,
                                                       SpecializationList = (from s in dataContext.Specialization
                                                                             join TS in dataContext.TrainerSpecialization on s.SpecializationId equals TS.SpecializationId
                                                                             where TS.TrainerId == T.TrainerId && TS.IsInTopThree == 1
                                                                             select s.SpecializationName).ToList<string>(),
                                                       TeamCount = dataContext.TrainerTeamMembers.Where(t => t.TeamId == T.TeamId).GroupBy(g => g.UserId).ToList().Count,
                                                       TrainerType = T.TrainerType,
                                                       TeamName = T.TeamId > 0 ? dataContext.Teams.Where(tt => tt.TeamId == T.TeamId).Select(tn => tn.TeamName).FirstOrDefault() : string.Empty,
                                                       UniqueTeamId = T.TeamId > 0 ? dataContext.Teams.Where(tt => tt.TeamId == T.TeamId).Select(tn => tn.UniqueTeamId).FirstOrDefault() : 0,
                                                       Email=T.EmailId,
                                                       PhoneNumber=T.PhoneNumber,
                                                       IsPremiumMember = Crd.IOSSubscriptionStatus ? Crd.IOSSubscriptionStatus : Crd.AndriodSubscriptionStatus,
                                                       UserCredId = Crd.Id
                                                   }).ToList<ViewTrainers>();

                    if (!string.IsNullOrEmpty(searchTrainer))
                    {
                        searchTrainer = searchTrainer.ToUpper(CultureInfo.InvariantCulture);
                        trainers = trainers.Where(usr =>  (usr.TrainerName.ToUpper(CultureInfo.InvariantCulture).IndexOf(searchTrainer, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderBy(tm => tm.TrainerName).ToList();
                    }
                    foreach (var item in trainers)
                    {
                        int count = 0;
                        foreach (var specialization in item.SpecializationList)
                        {
                            /*if specialzation is first then add else add with '|' in the string*/
                            if (count == 0)
                            {
                                item.TopThreeSpecialization = specialization;
                            }
                            else
                            {
                                item.TopThreeSpecialization = item.TopThreeSpecialization + " | " + specialization;
                            }

                            count = count + 1;
                        }
                        item.TeamName = String.IsNullOrEmpty(item.TeamName) ? String.Empty : item.TeamName.Substring(1);
                        item.PremiumMemberStatus = item.IsPremiumMember ? ConstantHelper.constYes : ConstantHelper.constNo;
                        var subscriptionStatus = dataContext.AppSubscriptions.Where(appSub => appSub.UserCredId == item.UserCredId).FirstOrDefault();
                        if (subscriptionStatus != null)
                        {
                            //if (subscriptionStatus.DeviceType == DeviceType.IOS.ToString())
                            //{
                              
                            //    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                            //    var localsubscribtion = epoch.AddMilliseconds(subscriptionStatus.Expires_date_ms);
                            //    if (localsubscribtion.Date >= DateTime.Now.Date && subscriptionStatus.AutoRenewing)
                            //    {
                                    
                            //        item.SubscriptionStatusLebel = ConstantHelper.constSubcribtionPurchase;
                            //    }
                            //    else
                            //    {
                            //        item.SubscriptionStatusLebel = ConstantHelper.constSubcribtionCancellation;
                            //    }
                            //}
                            //else
                            //{
                                item.SubscriptionStatusLebel = (item.IsPremiumMember && subscriptionStatus.AutoRenewing) ? ConstantHelper.constSubcribtionPurchase : ConstantHelper.constSubcribtionCancellation;
                           // }
                            // item.SubscriptionStatusLebel = subscriptionStatus.AutoRenewing ? ConstantHelper.constSubcribtionPurchase : ConstantHelper.constSubcribtionCancellation;
                        }
                        else
                        {
                            item.SubscriptionStatusLebel = "";
                        }
                    }
                    return trainers;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainers  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to create trainer 
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int SubmitTrainer(CreateTrainerVM objCreateTrainerVM)
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
                        traceLog.AppendLine("Start: SubmitTrainer for creating trainer");
                        if (!string.IsNullOrEmpty(objCreateTrainerVM.FirstName))
                        {
                            objCreateTrainerVM.FirstName = objCreateTrainerVM.FirstName.Trim();
                        }
                        if (!string.IsNullOrEmpty(objCreateTrainerVM.LastName))
                        {
                            objCreateTrainerVM.LastName = objCreateTrainerVM.LastName.Trim();
                        }
                        Mapper.CreateMap<CreateTrainerVM, tblTrainer>();
                        int maxTrainerID = GetMaxEnteredTrainerID();
                        if (!(maxTrainerID > 0))
                        {
                            maxTrainerID = ConstantHelper.constStartUniqueTrainerId;
                        }
                        tblTrainer objTrainer = Mapper.Map<CreateTrainerVM, tblTrainer>(objCreateTrainerVM);
                        objTrainer.EnteredTrainerID = maxTrainerID + 1;
                        objTrainer.CreatedDate = DateTime.Now;
                        objTrainer.Address = objCreateTrainerVM.Address;
                        objTrainer.PhoneNumber = objCreateTrainerVM.PhoneNumber;
                        objTrainer.Website = objCreateTrainerVM.Website;
                        objTrainer.ModifiedDate = DateTime.Now;
                        dataContext.Trainer.Add(objTrainer);
                        dataContext.SaveChanges();
                        int trainerId = 0;
                        if (objTrainer != null)
                        {
                            trainerId = objTrainer.TrainerId;
                        }

                        /*Add specialization information into database*/
                        var trainerSpecializations = CommonTrainerBL.GetPostedSpecializationsBasedTrainer(objCreateTrainerVM.PostedSpecializations, trainerId);
                        if (trainerSpecializations != null)
                        {
                            dataContext.TrainerSpecialization.AddRange(trainerSpecializations);
                        }
                        dataContext.SaveChanges();
                        /*Add credential information into database*/
                        tblCredentials objUserCredential = new tblCredentials();
                        objUserCredential.Password = objEncrypt.EncryptString(objCreateTrainerVM.Password);
                        objUserCredential.UserId = trainerId;
                        //objUserCredential.UserName = objCreateTrainerVM.EmailId;
                        objUserCredential.EmailId = objCreateTrainerVM.EmailId;
                        objUserCredential.UserType = LinksMediaCorpUtility.Resources.Message.UserTypeTrainer;
                        dataContext.Credentials.Add(objUserCredential);
                        dataContext.SaveChanges();
                        // Add assigned trainer team and get all users of team follow this trainer also
                        objCreateTrainerVM.TeamId = objCreateTrainerVM.TeamId ?? 0;
                        // Add all team except primary team
                        List<tblTrainerTeamMembers> selextedteams = new List<tblTrainerTeamMembers>();
                        if (objCreateTrainerVM.PostedTeams != null)
                        {
                            if (objCreateTrainerVM.PostedTeams.TeamsID != null)
                            {
                                int primaryTeamId = 0;
                                if (objCreateTrainerVM.TeamId.HasValue && objCreateTrainerVM.TeamId > 0)
                                {
                                    primaryTeamId = objCreateTrainerVM.TeamId ?? 0;
                                }
                                for (int i = 0; i < objCreateTrainerVM.PostedTeams.TeamsID.Count; i++)
                                {
                                    int teamId = Convert.ToInt32(objCreateTrainerVM.PostedTeams.TeamsID[i]);
                                    if (primaryTeamId != teamId)
                                    {
                                        if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == objUserCredential.Id && utm.TeamId == teamId))
                                        {
                                            tblTrainerTeamMembers tms = new tblTrainerTeamMembers()
                                            {
                                                TeamId = teamId,
                                                UserId = objUserCredential.Id,
                                                CreatedBy = objCreateTrainerVM.CreatedBy,
                                                CreatedDate = DateTime.Now,
                                                ModifiedBy = objCreateTrainerVM.CreatedBy,
                                                ModifiedDate = DateTime.Now
                                            };
                                            selextedteams.Add(tms);
                                        }
                                    }
                                }
                                dataContext.TrainerTeamMembers.AddRange(selextedteams);
                            }
                        }

                        // submit Trainer Mobile Team
                        List<tblTrainerMobileCoachTeam> selectedMobileTeams = new List<tblTrainerMobileCoachTeam>(); 
                        if (objCreateTrainerVM.PostedMobileCoachTeams != null)
                        {
                            if (objCreateTrainerVM.PostedMobileCoachTeams.TeamsID != null)
                            {                               
                                for (int i = 0; i < objCreateTrainerVM.PostedMobileCoachTeams.TeamsID.Count; i++)
                                {
                                    int teamId = Convert.ToInt32(objCreateTrainerVM.PostedMobileCoachTeams.TeamsID[i]);
                                      if (!dataContext.TrainerMobileCoachTeams.Any(utm => utm.TrainerCredId == objUserCredential.Id && utm.TeamId == teamId))
                                        {
                                        tblTrainerMobileCoachTeam tms = new tblTrainerMobileCoachTeam()
                                            {
                                                TeamId = teamId,
                                                TrainerCredId = objUserCredential.Id                                                
                                            };
                                          selectedMobileTeams.Add(tms);
                                        }
                                   
                                }
                                dataContext.TrainerMobileCoachTeams.AddRange(selectedMobileTeams);
                            }
                        }
                        
                        // Primary team user follwing the trainers                    
                        if (objCreateTrainerVM.TeamId.HasValue && objCreateTrainerVM.TeamId > 0)
                        {
                            var teamuserlist = (from user in dataContext.User
                                                join crd in dataContext.Credentials
                                                on user.UserId equals crd.UserId
                                                where crd.UserType == Message.UserTypeUser && user.TeamId == objCreateTrainerVM.TeamId
                                                select new
                                                {
                                                    crd.Id,
                                                    crd.UserId,
                                                    crd.UserType,
                                                    user.FirstName,
                                                    user.LastName
                                                }).ToList();
                            List<tblFollowings> listofteamuser = new List<tblFollowings>();
                            foreach (var userdetails in teamuserlist)
                            {
                                if (!dataContext.Followings.Any(ctf => ctf.UserId == userdetails.Id && ctf.FollowUserId == objUserCredential.Id))
                                {
                                    tblFollowings objfollwoing = new tblFollowings();
                                    objfollwoing.UserId = userdetails.Id;
                                    objfollwoing.FollowUserId = objUserCredential.Id;
                                    listofteamuser.Add(objfollwoing);

                                }
                            }
                            dataContext.Followings.AddRange(listofteamuser);
                            if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == objUserCredential.Id && utm.TeamId == objCreateTrainerVM.TeamId))
                            {
                                tblTrainerTeamMembers objTrainerTeamMember = new tblTrainerTeamMembers();
                                objTrainerTeamMember.TeamId = objCreateTrainerVM.TeamId ?? 0;
                                objTrainerTeamMember.UserId = objUserCredential.Id;
                                objTrainerTeamMember.CreatedBy = objCreateTrainerVM.CreatedBy;
                                objTrainerTeamMember.CreatedDate = DateTime.Now;
                                objTrainerTeamMember.ModifiedBy = objCreateTrainerVM.CreatedBy;
                                objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                dataContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                            }
                        }
                        else if (selextedteams.Count == 0)
                        {
                            var defautteam = dataContext.Teams.FirstOrDefault(t => t.IsDefaultTeam);
                            if (defautteam != null)
                            {
                                var teamtrainerList = (from tr in dataContext.Trainer
                                                       join crd in dataContext.Credentials
                                                       on tr.TrainerId equals crd.UserId
                                                       where crd.UserType == Message.UserTypeUser && tr.TeamId == defautteam.TeamId
                                                       select new
                                                       {
                                                           crd.Id,
                                                           crd.UserId,
                                                           crd.UserType,
                                                           tr.FirstName,
                                                           tr.LastName
                                                       }).ToList();
                                List<tblFollowings> followingteamtrainers = new List<tblFollowings>();
                                foreach (var teamtrainerDetail in teamtrainerList)
                                {
                                    if (!dataContext.Followings.Any(f => f.UserId == teamtrainerDetail.Id && f.FollowUserId == objUserCredential.Id))
                                    {
                                        tblFollowings objFollowing = new tblFollowings();
                                        objFollowing.UserId = teamtrainerDetail.Id;
                                        objFollowing.FollowUserId = objUserCredential.Id;
                                        //  objFollowing.IsAutoFollow = true;
                                        followingteamtrainers.Add(objFollowing);
                                    }
                                    dataContext.Followings.AddRange(followingteamtrainers);
                                }
                                if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == objUserCredential.Id && utm.TeamId == defautteam.TeamId))
                                {
                                    tblTrainerTeamMembers objTrainerTeamMember = new tblTrainerTeamMembers();
                                    objTrainerTeamMember.TeamId = defautteam.TeamId;
                                    objTrainerTeamMember.UserId = objUserCredential.Id;
                                    objTrainerTeamMember.CreatedBy = objCreateTrainerVM.CreatedBy;
                                    objTrainerTeamMember.CreatedDate = DateTime.Now;
                                    objTrainerTeamMember.ModifiedBy = objCreateTrainerVM.CreatedBy;
                                    objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                    dataContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                                    var objtrainer = dataContext.Trainer.FirstOrDefault(usr => usr.TrainerId == trainerId);
                                    if (objtrainer != null)
                                    {
                                        objtrainer.TeamId = defautteam.TeamId;
                                    }
                                }
                            }
                        }
                        dataContext.SaveChanges();
                        dbTran.Commit();
                        return trainerId;
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                               .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);
                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);
                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                        // Throw a new DbEntityValidationException with the improved exception message.
                        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        objEncrypt.Dispose();
                        traceLog.AppendLine("SubmitTrainer  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Get Maximum EnteredTrainerID
        /// </summary>
        /// <returns></returns>
        private static int GetMaxEnteredTrainerID()
        {
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    int maxTrainerID = Convert.ToInt32(dataContext.Trainer.Max(x => x.EnteredTrainerID));
                    return maxTrainerID;
                }
                catch
                {
                    return ConstantHelper.constStartUniqueTrainerId;
                }
            }
        }
        /// <summary>
        /// Function to delete trainer 
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void DeleteTrainer(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: DeleteTrainer for deleting trainer");
                        tblTrainer trainer = dataContext.Trainer.Find(Id);

                        /*Delete Related sponsor challenge of this trainer*/
                        List<tblSponsorChallengeQueue> sponsorChallenge = dataContext.TrainerChallenge.Where(ce => ce.TrainerId == Id).ToList();
                        if (sponsorChallenge != null)
                        {
                            dataContext.TrainerChallenge.RemoveRange(sponsorChallenge);
                            dataContext.SaveChanges();
                        }

                        /*Delete Related accepted challenge of this user*/
                        int userCredId = dataContext.Credentials.Where(ce => ce.UserId == Id && ce.UserType == Message.UserTypeTrainer).Select(y => y.Id).FirstOrDefault();

                        List<tblUserChallenges> acceptedChallengeList = dataContext.UserChallenge.Where(ce => ce.UserId == userCredId).ToList();
                        if (acceptedChallengeList != null)
                        {
                            dataContext.UserChallenge.RemoveRange(acceptedChallengeList);
                            dataContext.SaveChanges();
                        }

                        /*Modify Related challenge of this trainer*/
                        List<tblChallenge> challenge = dataContext.Challenge.Where(ce => ce.TrainerId == userCredId).ToList();
                        if (challenge != null)
                        {
                            foreach (var item in challenge)
                            {
                                item.TrainerId = 0;
                            }
                            dataContext.SaveChanges();
                        }
                        /*Delete Related Activity of this trainer*/
                        List<tblActivity> trainerActivityList = dataContext.Activity.Where(ce => ce.TrainerId == userCredId).ToList();
                        if (trainerActivityList != null)
                        {
                            dataContext.Activity.RemoveRange(trainerActivityList);
                            dataContext.SaveChanges();
                        }
                        /*Delete specialization information from database*/
                        List<tblTrainerSpecialization> trainerSpecializationList = dataContext.TrainerSpecialization.Where(ce => ce.TrainerId == Id).ToList();
                        if (trainerSpecializationList != null)
                        {
                            dataContext.TrainerSpecialization.RemoveRange(trainerSpecializationList);
                            dataContext.SaveChanges();
                        }

                        /*Delete team member record from database*/
                        tblTrainerTeamMembers trainerTeamMemberList = dataContext.TrainerTeamMembers.FirstOrDefault(ttm => ttm.UserId == userCredId);
                        if (trainerTeamMemberList != null)
                        {
                            dataContext.TrainerTeamMembers.Remove(trainerTeamMemberList);
                            dataContext.SaveChanges();

                        }
                        /*Delete related message stream from database based on credential Id*/
                        tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == Id && c.UserType.Equals(Message.UserTypeTrainer));
                        List<tblMessageStream> subjectStreamList = dataContext.MessageStraems.Where(ms => ms.SubjectId == objCred.Id || ms.TargetId == objCred.Id).ToList();
                        if (subjectStreamList != null)
                        {
                            dataContext.MessageStraems.RemoveRange(subjectStreamList);
                            dataContext.SaveChanges();

                        }

                        /*Delete related message stream from database based on Trainer Id*/
                        List<tblMessageStream> targetStreamList = dataContext.MessageStraems.Where(ms => ms.TargetId == Id && ms.TargetType.Equals(Message.MyTeamTargetType)).ToList();
                        if (targetStreamList != null)
                        {
                            dataContext.MessageStraems.RemoveRange(targetStreamList);
                            dataContext.SaveChanges();

                        }
                        /*Delete Related Following of this user*/
                        List<tblFollowings> usertblFollowingsList = dataContext.Followings.Where(uf => uf.UserId == userCredId || uf.FollowUserId == userCredId).ToList();
                        if (usertblFollowingsList != null)
                        {
                            dataContext.Followings.RemoveRange(usertblFollowingsList);
                            dataContext.SaveChanges();
                        }
                        /*Delete Trainer Credentials from database*/
                        if (objCred != null)
                        {
                            dataContext.Credentials.Remove(objCred);
                            dataContext.SaveChanges();
                        }
                        /*Delete Related user token of this user*/
                        List<tblUserToken> usertokenList = dataContext.UserToken.Where(uf => uf.UserId == userCredId).ToList();
                        if (usertokenList != null)
                        {
                            dataContext.UserToken.RemoveRange(usertokenList);
                            dataContext.SaveChanges();
                        }
                        /*Delete Related user devices Notification of this user*/
                        List<tblUserNotificationSetting> userdevicesNotificationList = dataContext.UserNotificationSetting.Where(uf => uf.UserCredId == userCredId).ToList();
                        if (userdevicesNotificationList != null)
                        {
                            dataContext.UserNotificationSetting.RemoveRange(userdevicesNotificationList);
                            dataContext.SaveChanges();
                        }
                        /*Delete Related user UserNotifications Notification of this user*/
                        List<tblUserNotifications> userNotificationList = dataContext.UserNotifications.Where(uf => uf.SenderCredlID == userCredId || uf.ReceiverCredID == userCredId).ToList();
                        if (userNotificationList != null)
                        {
                            dataContext.UserNotifications.RemoveRange(userNotificationList);
                        }
                        tblAppSubscription objtblAppSubscription = dataContext.AppSubscriptions.Where(uf => uf.UserCredId == userCredId).FirstOrDefault();
                        if (objtblAppSubscription != null)
                        {
                            dataContext.AppSubscriptions.Remove(objtblAppSubscription);                           
                        }

                        /*Delete Trainer from database*/
                        dataContext.Trainer.Remove(trainer);
                        int isDelete = dataContext.SaveChanges();
                        if (isDelete > 0)
                        {
                            string root = HttpContext.Current.Server.MapPath("~/images/profilepic");
                            string path = root + ConstantHelper.constDoubleBackSlash + trainer.TrainerImageUrl;
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        dbTran.Commit();


                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("DeleteTrainer  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to get trainer by trainer id 
        /// </summary>
        /// <returns>UpdateTrainerVM</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static UpdateTrainerVM GetTrainerById(int Id)
        {
            StringBuilder traceLog = new StringBuilder();

            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetTrainerById for retrieving trainer by id:" + Id);
                    tblTrainer trainer = dataContext.Trainer.Find(Id);
                    DDTeams currentTeamDetail = new DDTeams();
                    Mapper.CreateMap<tblTrainer, UpdateTrainerVM>();
                    UpdateTrainerVM objTrainer = Mapper.Map<tblTrainer, UpdateTrainerVM>(trainer);
                    if (objTrainer != null && objTrainer.TeamId > 0)
                    {
                        objTrainer.PrimaryTeamId = objTrainer.TeamId;
                        currentTeamDetail.TeamId = objTrainer.TeamId ?? 0;
                        var teamdetails = dataContext.Teams.FirstOrDefault(t => t.TeamId == objTrainer.TeamId);
                        if (teamdetails != null && teamdetails.IsDefaultTeam)
                        {
                            objTrainer.IsDefaultTeam = true;
                            objTrainer.TeamId = 0;
                            currentTeamDetail.IsDefaultTeam = true;
                        }
                        currentTeamDetail.TeamName = (teamdetails != null && string.IsNullOrEmpty(teamdetails.TeamName)) ? string.Empty : teamdetails.TeamName.Substring(1);
                    }
                    List<Specialization> objListSpecialization = GetSpecializations();
                    objTrainer.SetAvailableSpecializations(objListSpecialization);
                    List<Specialization> objListSelectedTopThreeSpecialization = GetTopThreeSpecializations(Id);
                    objTrainer.SetSelectedTopthreeSpecializations(objListSelectedTopThreeSpecialization);
                    List<Specialization> objListPrimarySpecialization = GetSecondarySpecializations(Id);
                    objTrainer.SetSelectedSecondarySpecializations(objListPrimarySpecialization);
                    tblCredentials trainerCredentials = dataContext.Credentials.Where(ce => ce.UserId == Id && ce.UserType == Message.UserTypeTrainer).FirstOrDefault();
                    int trainerCrdId = 0;
                    if (trainerCredentials != null)
                    {
                        objTrainer.EmailId = trainerCredentials.EmailId;
                        trainerCrdId = trainerCredentials.Id;
                    }
                    List<DDTeams> objListTeams = TeamBL.GetAllTeamName();
                    if (objTrainer != null && trainer.TeamId > 0)
                    {
                        objListTeams = objListTeams.Where(tm => tm.TeamId != trainer.TeamId).ToList();
                        objListTeams.Insert(0, currentTeamDetail);
                    }
                    objTrainer.SelecetdMobileCoachTeams = GetTrainerMobileCoachTeam(trainerCredentials.Id);
                    objTrainer.SetAvailableTeams(objListTeams);
                    objTrainer.SetSelecetdTeams(GetTrainerTeams(trainerCrdId));

                    return objTrainer;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to update trainer
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static void UpdateTrainer(UpdateTrainerVM objUpdateTrainerVM)
        {
            StringBuilder traceLog = new StringBuilder();
            EncryptDecrypt objEncrypt = null;
            int currentPrimaryTeamId = 0;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: UpdateTrainer for updating trainer");
                        objEncrypt = new EncryptDecrypt();
                        if (!string.IsNullOrEmpty(objUpdateTrainerVM.FirstName))
                        {
                            objUpdateTrainerVM.FirstName = objUpdateTrainerVM.FirstName.Trim();
                        }
                        if (!string.IsNullOrEmpty(objUpdateTrainerVM.LastName))
                        {
                            objUpdateTrainerVM.LastName = objUpdateTrainerVM.LastName.Trim();
                        }
                        // Follow/UnFollwed the team member user
                        if (objUpdateTrainerVM.TrainerId > 0)
                        {
                            objUpdateTrainerVM.TeamId = FollowTeamUsersToTrainer(objUpdateTrainerVM.TrainerId, objUpdateTrainerVM.TeamId ?? 0, objUpdateTrainerVM.ModifyBy);
                        }
                        tblTrainer objTrainer = dataContext.Trainer.FirstOrDefault(tr => tr.TrainerId == objUpdateTrainerVM.TrainerId);
                        if (objTrainer != null)
                        {
                            currentPrimaryTeamId = objTrainer.TeamId;
                            objTrainer.ModifiedDate = DateTime.Now;
                            objTrainer.ZipCode = objUpdateTrainerVM.ZipCode;
                            objTrainer.Weight = objUpdateTrainerVM.Weight;
                            objTrainer.TrainerType = objUpdateTrainerVM.TrainerType;
                            objTrainer.TrainerImageUrl = objUpdateTrainerVM.TrainerImageUrl;
                            if (objUpdateTrainerVM.TeamId.HasValue && objUpdateTrainerVM.TeamId > 0)
                            {
                                objTrainer.TeamId = objUpdateTrainerVM.TeamId ?? 0;
                            }
                            objTrainer.State = objUpdateTrainerVM.State;
                            objTrainer.ModifiedDate = DateTime.Now;
                            objTrainer.ModifiedBy = objUpdateTrainerVM.ModifyBy;
                            objTrainer.LoggedIn = objUpdateTrainerVM.LoggedIn;
                            objTrainer.FirstName = objUpdateTrainerVM.FirstName;
                            objTrainer.LastName = objUpdateTrainerVM.LastName;
                            objTrainer.Height = objUpdateTrainerVM.Height;
                            objTrainer.Gender = objUpdateTrainerVM.Gender;
                            objTrainer.EmailId = objUpdateTrainerVM.EmailId;
                            objTrainer.DateOfBirth = objUpdateTrainerVM.DateOfBirth;
                            objTrainer.City = string.IsNullOrEmpty(objUpdateTrainerVM.City) ? objTrainer.City : Convert.ToInt32(objUpdateTrainerVM.City);
                            objTrainer.AboutMe = objUpdateTrainerVM.AboutMe;
                            objTrainer.ActivityId = objUpdateTrainerVM.ActivityId;
                            objTrainer.Address = objUpdateTrainerVM.Address;
                            objTrainer.PhoneNumber = objUpdateTrainerVM.PhoneNumber;
                            objTrainer.Website = objUpdateTrainerVM.Website;
                            dataContext.SaveChanges();
                        }
                        /*Add specialization information into database*/
                        List<tblTrainerSpecialization> trainerSpecializationList = dataContext.TrainerSpecialization.Where(ce => ce.TrainerId == objUpdateTrainerVM.TrainerId).ToList();
                        dataContext.TrainerSpecialization.RemoveRange(trainerSpecializationList);
                        dataContext.SaveChanges();
                        List<tblTrainerSpecialization> trainerSpecializations = new List<tblTrainerSpecialization>();
                        /*primary specialization */
                        if (objUpdateTrainerVM.PostedSpecializations != null)
                        {
                            if (objUpdateTrainerVM.PostedSpecializations.PrimarySpecializationIDs != null)
                            {
                                for (int i = 0; i < objUpdateTrainerVM.PostedSpecializations.PrimarySpecializationIDs.Count; i++)
                                {
                                    tblTrainerSpecialization trainerSpecialization = new tblTrainerSpecialization();
                                    trainerSpecialization.SpecializationId = Convert.ToInt32(objUpdateTrainerVM.PostedSpecializations.PrimarySpecializationIDs[i]);
                                    trainerSpecialization.TrainerId = objUpdateTrainerVM.TrainerId;
                                    trainerSpecialization.IsInTopThree = 1;
                                    trainerSpecializations.Add(trainerSpecialization);
                                }
                            }
                        }
                        /*secondary specialization*/
                        if (objUpdateTrainerVM.PostedSpecializations != null)
                        {
                            if (objUpdateTrainerVM.PostedSpecializations.SecondarySpecializationIDs != null)
                            {
                                for (int i = 0; i < objUpdateTrainerVM.PostedSpecializations.SecondarySpecializationIDs.Count; i++)
                                {
                                    tblTrainerSpecialization trainerSpecialization = new tblTrainerSpecialization();
                                    trainerSpecialization.SpecializationId = Convert.ToInt32(objUpdateTrainerVM.PostedSpecializations.SecondarySpecializationIDs[i]);
                                    trainerSpecialization.TrainerId = objUpdateTrainerVM.TrainerId;
                                    trainerSpecialization.IsInTopThree = 0;
                                    trainerSpecializations.Add(trainerSpecialization);
                                }
                            }
                        }
                        dataContext.TrainerSpecialization.AddRange(trainerSpecializations);
                        dataContext.SaveChanges();
                        /*update credentials */
                        tblCredentials trainerCredentials = dataContext.Credentials.Where(ce => ce.UserId == objUpdateTrainerVM.TrainerId && ce.UserType == Message.UserTypeTrainer).FirstOrDefault();
                        if (!(trainerCredentials == null))
                        {
                            if (objUpdateTrainerVM.IsChangePassword)
                            {
                                trainerCredentials.Password = objEncrypt.EncryptString((objUpdateTrainerVM.Password).Trim());
                            }
                            trainerCredentials.EmailId = objUpdateTrainerVM.EmailId;
                            dataContext.SaveChanges();
                        }
                        //// Remove all Team member for trainer  and associated following list
                        List<int> selectedTeamIds = new List<int>();
                        if (objUpdateTrainerVM.PostedTeams != null && objUpdateTrainerVM.PostedTeams.TeamsID != null)
                        {
                            selectedTeamIds = objUpdateTrainerVM.PostedTeams.TeamsID.Select(tm => Convert.ToInt32(tm)).ToList();
                        }
                        List<tblTrainerTeamMembers> trainerAllTeams = (from tm in dataContext.TrainerTeamMembers
                                                                       where tm.UserId == trainerCredentials.Id && !selectedTeamIds.Contains(tm.TeamId)
                                                                       select tm).OrderByDescending(tt => tt.CreatedDate).ToList();
                        dataContext.TrainerTeamMembers.RemoveRange(trainerAllTeams);
                        dataContext.SaveChanges();

                        bool isCurrentchanged = false;
                        // Add all team except primary team
                        List<tblTrainerTeamMembers> selectedTeams = new List<tblTrainerTeamMembers>();
                        if (objUpdateTrainerVM.PostedTeams != null)
                        {
                            if (objUpdateTrainerVM.PostedTeams.TeamsID != null)
                            {
                                isCurrentchanged = objUpdateTrainerVM.PostedTeams.TeamsID.Any(tm => tm == currentPrimaryTeamId.ToString());
                                for (int i = 0; i < objUpdateTrainerVM.PostedTeams.TeamsID.Count; i++)
                                {
                                    int teamId = Convert.ToInt32(objUpdateTrainerVM.PostedTeams.TeamsID[i]);
                                    if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == trainerCredentials.Id && utm.TeamId == teamId))
                                    {
                                        tblTrainerTeamMembers tms = new tblTrainerTeamMembers()
                                        {
                                            TeamId = teamId,
                                            UserId = trainerCredentials.Id,
                                            CreatedBy = objTrainer.CreatedBy,
                                            CreatedDate = DateTime.Now,
                                            ModifiedBy = objTrainer.CreatedBy,
                                            ModifiedDate = DateTime.Now
                                        };
                                        selectedTeams.Add(tms);
                                    }
                                }
                                dataContext.TrainerTeamMembers.AddRange(selectedTeams);
                            }
                        }
                        int primaryTeamIds = 0;
                        if (objUpdateTrainerVM != null)
                        {
                            primaryTeamIds = objUpdateTrainerVM.PrimaryTeamId ?? 0;
                        }
                        List<tblTrainerMobileCoachTeam> savedTrainerTeamIds = (from tm in dataContext.TrainerMobileCoachTeams
                                                                               where tm.TrainerCredId == trainerCredentials.Id
                                                                               select tm).ToList(); 
                        dataContext.TrainerMobileCoachTeams.RemoveRange(savedTrainerTeamIds);
                        dataContext.SaveChanges();
                        // submit Trainer Mobile Team
                        List<tblTrainerMobileCoachTeam> selectedMobileTeams = new List<tblTrainerMobileCoachTeam>();
                        if (objUpdateTrainerVM.PostedMobileCoachTeams != null)
                        {
                            if (objUpdateTrainerVM.PostedMobileCoachTeams.TeamsID != null)
                            {
                                for (int i = 0; i < objUpdateTrainerVM.PostedMobileCoachTeams.TeamsID.Count; i++)
                                {
                                    int teamId = Convert.ToInt32(objUpdateTrainerVM.PostedMobileCoachTeams.TeamsID[i]);
                                    if (!dataContext.TrainerMobileCoachTeams.Any(utm => utm.TrainerCredId == trainerCredentials.Id && utm.TeamId == teamId))
                                    {
                                        tblTrainerMobileCoachTeam tms = new tblTrainerMobileCoachTeam()
                                        {
                                            TeamId = teamId,
                                            TrainerCredId = trainerCredentials.Id
                                        };
                                        selectedMobileTeams.Add(tms);
                                    }
                                }
                                dataContext.TrainerMobileCoachTeams.AddRange(selectedMobileTeams);
                            }
                        }

                        // If Primary Team has been changed then remove the primary team to trainer and associated following also
                        if (objUpdateTrainerVM.PrimaryTeamId.HasValue && objUpdateTrainerVM.PrimaryTeamId > 0 && currentPrimaryTeamId != objUpdateTrainerVM.PrimaryTeamId && !isCurrentchanged)
                        {
                                                     
                            objTrainer.TeamId = primaryTeamIds;
                            dataContext.Entry(objTrainer).State = System.Data.Entity.EntityState.Modified;
                            var teamuserlist = (from user in dataContext.User
                                                join crd in dataContext.Credentials
                                                on user.UserId equals crd.UserId
                                                where crd.UserType == Message.UserTypeUser && user.TeamId == objUpdateTrainerVM.PrimaryTeamId
                                                select new
                                                {
                                                    crd.Id,
                                                    crd.UserId,
                                                    crd.UserType,
                                                    user.FirstName,
                                                    user.LastName
                                                }).ToList();
                            List<tblFollowings> listofteamuser = new List<tblFollowings>();
                            foreach (var usercrdId in teamuserlist)
                            {
                                if (!dataContext.Followings.Any(ctf => ctf.UserId == usercrdId.Id && ctf.FollowUserId == trainerCredentials.Id))
                                {
                                    tblFollowings objfollwoing = new tblFollowings();
                                    objfollwoing.UserId = usercrdId.Id;
                                    objfollwoing.FollowUserId = trainerCredentials.Id;
                                    listofteamuser.Add(objfollwoing);
                                }
                            }
                            dataContext.Followings.AddRange(listofteamuser);
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
                    }
                }
            }
        }

        /// <summary>
        /// Follow TeamUsers when trainer joined team or change team
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="teamId"></param>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        private static int FollowTeamUsersToTrainer(int trainerId, int newteamId, int modifiedBy)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                tblTrainerTeamMembers objTrainerTeamMember = null;
                List<tblFollowings> listofteamuser = null;
                try
                {
                    traceLog.AppendLine("Start:FollowTeamUsersToTrainer Follow TeamUsers To Trainer when trainer join team-trainerId-" + Convert.ToString(trainerId) + ",teamId-" + Convert.ToString(newteamId));
                    var trainerdetails = (from tr in dataContext.Trainer
                                          join crd in dataContext.Credentials
                                          on tr.TrainerId equals crd.UserId
                                          where crd.UserType == Message.UserTypeTrainer && tr.TrainerId == trainerId
                                          select new
                                          {
                                              tr.TrainerId,
                                              tr.TeamId,
                                              crd.Id
                                          }).FirstOrDefault();
                    if (trainerdetails != null && newteamId > 0 && trainerdetails.TeamId > 0 && trainerdetails.TeamId != newteamId) // Change teamId then unfollowed their team member
                    {
                        int oldteamId = trainerdetails.TeamId;
                        // Assinned new team
                        List<int> oldteamuserlist = (from user in dataContext.User
                                                     join crd in dataContext.Credentials
                                                     on user.UserId equals crd.UserId
                                                     where crd.UserType == Message.UserTypeUser && user.TeamId == oldteamId
                                                     select crd.Id).ToList();
                        List<tblFollowings> unfollowedoldtrainer = dataContext.Followings.Where(fl => !fl.IsManuallyFollow && fl.FollowUserId == trainerdetails.Id && oldteamuserlist.Contains(fl.UserId)).ToList();
                        if (unfollowedoldtrainer != null)
                            dataContext.Followings.RemoveRange(unfollowedoldtrainer);
                        dataContext.SaveChanges();
                        // Assinned new team
                        var teamuserlist = (from user in dataContext.User
                                            join crd in dataContext.Credentials
                                            on user.UserId equals crd.UserId
                                            where crd.UserType == Message.UserTypeUser && user.TeamId == newteamId
                                            select new
                                            {
                                                crd.Id,
                                                crd.UserId,
                                                crd.UserType,
                                                user.FirstName,
                                                user.LastName
                                            }).ToList();
                        listofteamuser = new List<tblFollowings>();
                        foreach (var usercrdDetail in teamuserlist)
                        {
                            if (!dataContext.Followings.Any(ctf => ctf.UserId == usercrdDetail.Id && ctf.FollowUserId == trainerdetails.Id))
                            {
                                tblFollowings objfollwoing = new tblFollowings();
                                objfollwoing.UserId = usercrdDetail.Id;
                                objfollwoing.FollowUserId = trainerdetails.Id;
                                listofteamuser.Add(objfollwoing);

                            }
                        }
                        dataContext.Followings.AddRange(listofteamuser);
                        dataContext.SaveChanges();

                        var existingtrainerteam = dataContext.TrainerTeamMembers.FirstOrDefault(tms => tms.TeamId == oldteamId && tms.UserId == trainerdetails.Id);
                        if (existingtrainerteam != null)
                        {
                            dataContext.TrainerTeamMembers.Remove(existingtrainerteam);
                        }
                        dataContext.SaveChanges();
                        if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == trainerdetails.Id && utm.TeamId == newteamId))
                        {
                            objTrainerTeamMember = new tblTrainerTeamMembers();
                            objTrainerTeamMember.TeamId = newteamId;
                            objTrainerTeamMember.UserId = trainerdetails.Id;
                            objTrainerTeamMember.CreatedBy = modifiedBy;
                            objTrainerTeamMember.CreatedDate = DateTime.Now;
                            objTrainerTeamMember.ModifiedBy = modifiedBy;
                            objTrainerTeamMember.ModifiedDate = DateTime.Now;
                            dataContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                        }
                        dataContext.SaveChanges();
                        return newteamId;
                    }
                    else if (trainerdetails != null && newteamId != 0 && trainerdetails.TeamId == 0 && trainerdetails.TeamId != newteamId) // Assinged new team
                    {
                        var teamuserlist = (from user in dataContext.User
                                            join crd in dataContext.Credentials
                                            on user.UserId equals crd.UserId
                                            where crd.UserType == Message.UserTypeUser && user.TeamId == newteamId
                                            select new
                                            {
                                                crd.Id,
                                                crd.UserId,
                                                crd.UserType,
                                                user.FirstName,
                                                user.LastName
                                            }).ToList();
                        listofteamuser = new List<tblFollowings>();
                        foreach (var usercrdDetail in teamuserlist)
                        {
                            if (!dataContext.Followings.Any(ctf => ctf.UserId == usercrdDetail.Id && ctf.FollowUserId == trainerdetails.Id))
                            {
                                tblFollowings objfollwoing = new tblFollowings();
                                objfollwoing.UserId = usercrdDetail.Id;
                                objfollwoing.FollowUserId = trainerdetails.Id;
                                listofteamuser.Add(objfollwoing);
                            }
                        }
                        dataContext.Followings.AddRange(listofteamuser);
                        dataContext.SaveChanges();
                        if (!dataContext.TrainerTeamMembers.Any(utm => utm.UserId == trainerdetails.Id && utm.TeamId == newteamId))
                        {
                            objTrainerTeamMember = new tblTrainerTeamMembers();
                            objTrainerTeamMember.TeamId = newteamId;
                            objTrainerTeamMember.UserId = trainerdetails.Id;
                            objTrainerTeamMember.CreatedBy = modifiedBy;
                            objTrainerTeamMember.CreatedDate = DateTime.Now;
                            objTrainerTeamMember.ModifiedBy = modifiedBy;
                            objTrainerTeamMember.ModifiedDate = DateTime.Now;
                            dataContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                        }
                        dataContext.SaveChanges();
                        return newteamId;
                    }
                    else if (trainerdetails != null && newteamId == 0 && trainerdetails.TeamId > 0 && trainerdetails.TeamId != newteamId) // Remove the existing team to unassined to team
                    {
                        int oldteamId = trainerdetails.TeamId;
                        // Assinned new team
                        List<int> oldteamuserlist = (from user in dataContext.User
                                                     join crd in dataContext.Credentials
                                                     on user.UserId equals crd.UserId
                                                     where crd.UserType == Message.UserTypeUser && user.TeamId == oldteamId
                                                     select crd.Id).Distinct().ToList();
                        List<tblFollowings> unfollowedoldtrainer = dataContext.Followings.Where(fl => !fl.IsManuallyFollow
                            && fl.FollowUserId == trainerdetails.Id && oldteamuserlist.Contains(fl.UserId)).ToList();
                        if (unfollowedoldtrainer != null)
                        {
                            dataContext.Followings.RemoveRange(unfollowedoldtrainer);
                            dataContext.SaveChanges();
                        }
                        var existingtrainerteam = dataContext.TrainerTeamMembers.FirstOrDefault(tms => tms.TeamId == oldteamId && tms.UserId == trainerdetails.Id);
                        // Find out Default team Information                        
                        var defaultfitcomteam = (from t in dataContext.Teams
                                                 join crd in dataContext.Credentials
                                                 on t.TeamId equals crd.UserId
                                                 where crd.UserType == Message.UserTypeTeam && t.IsDefaultTeam
                                                 select new
                                                 {
                                                     t.TeamId,
                                                     crd.Id
                                                 }).FirstOrDefault();

                        int defaultfitcomteamId = 0;
                        int defaultfitcomteamCrdId = 0;
                        if (defaultfitcomteam != null)
                        {
                            defaultfitcomteamId = defaultfitcomteam.TeamId;
                            defaultfitcomteamCrdId = defaultfitcomteam.Id;
                        }
                        // Update the existing trainer team memeber record
                        if (existingtrainerteam != null)
                        {
                            existingtrainerteam.TeamId = defaultfitcomteamId;
                            dataContext.SaveChanges();
                        }
                        List<int> defaultteamusercredIDs = (from usr in dataContext.User
                                                            join cr in dataContext.Credentials
                                                            on usr.UserId equals cr.UserId
                                                            where cr.UserType == Message.UserTypeUser && usr.TeamId == defaultfitcomteamId
                                                            select cr.Id).Distinct().ToList();
                        // Follow Defaut team user to this current user
                        listofteamuser = new List<tblFollowings>();
                        foreach (int usercredId in defaultteamusercredIDs)
                        {
                            if (!dataContext.Followings.Any(ctf => ctf.UserId == usercredId && ctf.FollowUserId == defaultfitcomteamCrdId))
                            {
                                tblFollowings objtblFollowings = new tblFollowings();
                                objtblFollowings.FollowUserId = defaultfitcomteamCrdId;
                                objtblFollowings.UserId = usercredId;
                                listofteamuser.Add(objtblFollowings);
                            }
                        }
                        if (listofteamuser != null)
                        {
                            dataContext.Followings.AddRange(listofteamuser);
                        }
                        dataContext.SaveChanges();
                        return defaultfitcomteamId;
                    }
                    return 0;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("FollowTeamUsersToTrainer  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get state
        /// </summary>
        /// <returns>List<State></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<State> GetState()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetState");
                    Mapper.CreateMap<tblState, State>();
                    List<tblState> lstObjstate = dataContext.States.OrderBy(st => st.StateName).ToList();
                    List<State> lststates = Mapper.Map<List<tblState>, List<State>>(lstObjstate);
                    return lststates;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetState  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get cities by state code
        /// </summary>
        /// <returns>List<City></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<City> GetCities(string stateCode)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetCities for retrieving cities from database ");
                    Mapper.CreateMap<tblCity, City>();
                    List<tblCity> listObjCities = dataContext.Cities.Where(c => c.StateCode == stateCode).OrderBy(cc => cc.CityName).ToList();
                    List<City> listCities = Mapper.Map<List<tblCity>, List<City>>(listObjCities);
                    return listCities;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetCities  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get all trainer Types
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static List<TrainerTypeVM> GetTrainerTypes()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerTypes for retrieving Trainer Types from database ");
                    Mapper.CreateMap<tblTrainerType, TrainerTypeVM>();
                    List<tblTrainerType> listObjCities = dataContext.TrainerType.ToList();
                    List<TrainerTypeVM> listTrainerTypes = Mapper.Map<List<tblTrainerType>, List<TrainerTypeVM>>(listObjCities);
                    return listTrainerTypes;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerTypes  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get trainer count fron the database  
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetTrainerCount()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerCount");
                    int trainerCount = dataContext.Trainer.Count();
                    return trainerCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get all trainers from database 
        /// </summary>             
        /// <returns>List<TrainerViewVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<TrainerViewVM> GetAllTrainers()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetAllTrainers for retrieving trainers from database ");
                    List<TrainerViewVM> result = (from trnr in dataContext.Trainer
                                                  orderby (trnr.FirstName + " " + trnr.LastName)
                                                  select new TrainerViewVM
                                                  {
                                                      TrainerId = trnr.TrainerId,
                                                      TrainerName = trnr.FirstName + " " + trnr.LastName
                                                  }).ToList();
                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetAllTrainers  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get trainer result for any challenge by challenge id and trainer id
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="challengeId"></param>
        /// <returns>List<UserResult></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static List<UserResult> GetTrainerResult(int trainerId, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerResult for retrieving result with related any trainer from database ");
                    List<UserResult> result = (from chlng in dataContext.UserChallenge
                                               where chlng.ChallengeId == challengeId && chlng.UserId == trainerId && ((chlng.Result != null && chlng.Result != "0") || chlng.Fraction != null)
                                               select new UserResult
                                               {
                                                   Id = chlng.Id,
                                                   Result = chlng.Result,
                                                   Fraction = chlng.Fraction
                                               }).ToList();
                    foreach (var item in result)
                    {
                        if (item.Fraction != null && item.Result != null)
                        {
                            if (!item.Fraction.Equals("0"))
                            {
                                item.Result = item.Result + ", " + item.Fraction;
                            }
                        }
                        else if (item.Fraction != null)
                        {
                            item.Result = item.Fraction;
                        }
                    }

                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerResult  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Function to get trainer id from database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetTrainerId(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerId for retrieving trainerId from database ");
                    int trainerId = dataContext.Credentials.Where(C => C.UserId == id && C.UserType == Message.UserTypeTrainer).Select(y => y.Id).FirstOrDefault();
                    return trainerId;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainers  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }      
        /// <summary>
        /// Get User Trainer Assignments List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<UserAssignmentByTrainerVM>> GetUserTrainerAssignmentsList(int userId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            List<int> listUserAssignmentID = new List<int>();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserAssignmentsList---- " + DateTime.Now.ToLongDateString());
                    int userCredID = 0;
                    userCredID = (from crd in dataContext.Credentials
                                  join usr in dataContext.User
                                  on crd.UserId equals usr.UserId
                                  where crd.UserType == Message.UserTypeUser && crd.UserId == userId
                                  select crd.Id).FirstOrDefault();
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<UserAssignmentByTrainerVM> listPendingChallenge = (from c in dataContext.Challenge
                                                                            join ctf in dataContext.UserAssignments on c.ChallengeId equals ctf.ChallengeId
                                                                            join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                            where c.IsActive && ctf.TargetId == userCredID && ctf.SubjectId == cred.Id
                                                                            orderby ctf.ChallengeDate, ctf.ChallengeCompletedDate descending
                                                                            select new UserAssignmentByTrainerVM
                                                                            {
                                                                                UserAssignmentUpdateId = ctf.UserAssignmentId,
                                                                                ChallengeId = c.ChallengeId,
                                                                                ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                                ChallengeName = c.ChallengeName,
                                                                                ChallengeType = ct.ChallengeType,
                                                                                DifficultyLevel = c.DifficultyLevel,
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
                                                                                SubjectId = ctf.SubjectId,
                                                                                TargetId = ctf.TargetId,
                                                                                DbPostedDate = ctf.ChallengeDate,
                                                                                DbCompletedDate = ctf.ChallengeCompletedDate,
                                                                                IsComplete = ctf.IsCompleted,
                                                                                ProgramImageUrl = c.ProgramImageUrl,
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
                                                                                ChallengeByUserType = dataContext.Credentials.FirstOrDefault(crd => crd.Id == ctf.SubjectId).UserType
                                                                            }).ToList();
                    if (listPendingChallenge != null)
                    {
                        listUserAssignmentID = listPendingChallenge.Select(usr => usr.UserAssignmentUpdateId).ToList();
                        listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.TargetId })
                                                                              .Select(grp => grp.FirstOrDefault())
                                                                              .ToList();
                    }
                    listPendingChallenge.ForEach(r =>
                    {                      
                        if (!string.IsNullOrEmpty(r.ProgramImageUrl))
                        {
                            string programHeight = string.Empty;
                            string programWidth = string.Empty;
                            CommonBL.GetProgramDemension(r.ProgramImageUrl,out programHeight, out programWidth);
                            r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + r.ProgramImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;
                            r.Height = programHeight;
                            r.Width = programWidth;                           
                        }
                        else
                        {
                            r.Height = string.Empty;
                            r.Width = string.Empty;
                        }
                        if (r.ExeciseVideoDetails != null)
                        {
                            if (string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseUrl))
                            {
                                r.ExeciseVideoLink = !string.IsNullOrEmpty(r.ExeciseThumbnailUrl) ? CommonUtility.VirtualFitComExercisePath +
                                    Message.ExerciseVideoDirectory + r.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                            }
                            else
                            {
                                r.ExeciseVideoLink = r.ExeciseVideoDetails.ExeciseUrl;
                            }
                            if (!string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(r.ExeciseVideoDetails.ExerciseThumnail))
                            {
                                string thumnailName = r.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                string thumnailHeight = string.Empty;
                                string thumnailWidth = string.Empty;
                                r.ExeciseThumbnailUrl = string.IsNullOrEmpty(thumnailName) ? string.Empty : CommonUtility.VirtualFitComExercisePath +
                                    Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                r.ThumbNailHeight = thumnailHeight;
                                r.ThumbNailWidth = thumnailWidth;
                            }
                            else
                            {
                                r.ThumbNailHeight = string.Empty;
                                r.ThumbNailWidth = string.Empty;
                                r.ExeciseThumbnailUrl = r.ExeciseVideoDetails.ExerciseThumnail;
                            }
                        }
                        if (!string.IsNullOrEmpty(r.ChallengeType))
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        }
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
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        r.Status = r.IsComplete ? AssignmentStatus.Completed : AssignmentStatus.Incomplete;
                    });
                    Total<List<UserAssignmentByTrainerVM>> objresult = new Total<List<UserAssignmentByTrainerVM>>();
                    objresult.TotalList = (from l in listPendingChallenge
                                           orderby l.DbCompletedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = listPendingChallenge.Count();
                    if ((objresult.TotalCount) > endIndex)
                    {
                        objresult.IsMoreAvailable = true;
                    }
                    if (listUserAssignmentID != null && listUserAssignmentID.Count > 0)
                    {
                        var listuserassignmentList = dataContext.UserAssignments.Where(uu => listUserAssignmentID.Contains(uu.UserAssignmentId)).ToList();
                        if (listuserassignmentList != null)
                        {
                            listuserassignmentList.ForEach(us =>
                                {
                                    us.IsRead = true;
                                });
                            dataContext.SaveChanges();
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
                    traceLog.AppendLine("End  GetPendingChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
        /// <summary>
        /// Get MyTrainer Assignments List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<UserAssignmentByTrainerVM>> GetMyTrainerAssignmentsList(int startIndex, int endIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserAssignmentsList---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    List<UserAssignmentByTrainerVM> listPendingChallenge = new List<UserAssignmentByTrainerVM>();
                    if (cred.UserType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        listPendingChallenge = (from c in dataContext.Challenge
                                                join ctf in dataContext.UserAssignments on c.ChallengeId equals ctf.ChallengeId
                                                join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                join ucrd in dataContext.Credentials on ctf.SubjectId equals ucrd.Id
                                                where c.IsActive
                                                && ctf.SubjectId == cred.Id
                                                && ucrd.UserType == Message.UserTypeTrainer
                                                orderby ctf.ChallengeDate, ctf.ChallengeCompletedDate descending
                                                select new UserAssignmentByTrainerVM
                                                {
                                                    ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                    ChallengeName = c.ChallengeName,
                                                    ChallengeType = ct.ChallengeType,
                                                    DifficultyLevel = c.DifficultyLevel,
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
                                                    SubjectId = ctf.SubjectId,
                                                    DbPostedDate = ctf.ChallengeDate,
                                                    DbCompletedDate = ctf.ChallengeCompletedDate,
                                                    IsComplete = ctf.IsCompleted,
                                                    ProgramImageUrl = c.ProgramImageUrl,
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
                                                    ChallengeByUserType = dataContext.Credentials.FirstOrDefault(crd => crd.Id == ctf.SubjectId).UserType
                                                }).ToList();
                        if (listPendingChallenge != null)
                        {
                            listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                                  .Select(grp => grp.FirstOrDefault())
                                                                                  .ToList();
                        }
                    }
                    else
                    {
                        listPendingChallenge = (from c in dataContext.Challenge
                                                join ctf in dataContext.UserAssignments on c.ChallengeId equals ctf.ChallengeId
                                                join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                join ucrd in dataContext.Credentials on ctf.SubjectId equals ucrd.Id
                                                where c.IsActive
                                                && ctf.TargetId == cred.Id
                                                && ucrd.UserType == Message.UserTypeTrainer
                                                orderby ctf.ChallengeDate, ctf.ChallengeCompletedDate descending
                                                select new UserAssignmentByTrainerVM
                                                {
                                                    ChallengeId = c.ChallengeId,
                                                    ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                    ChallengeName = c.ChallengeName,
                                                    ChallengeType = ct.ChallengeType,
                                                    DifficultyLevel = c.DifficultyLevel,
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
                                                    SubjectId = ctf.SubjectId,
                                                    DbPostedDate = ctf.ChallengeDate,
                                                    DbCompletedDate = ctf.ChallengeCompletedDate,
                                                    IsComplete = ctf.IsCompleted,
                                                    ProgramImageUrl = c.ProgramImageUrl,
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
                                                    ChallengeByUserType = dataContext.Credentials.FirstOrDefault(crd => crd.Id == ctf.SubjectId).UserType
                                                }).ToList();
                        if (listPendingChallenge != null)
                        {
                            listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                                  .Select(grp => grp.FirstOrDefault())
                                                                                  .ToList();
                        }
                    }
                    listPendingChallenge.ForEach(r =>
                    {
                        r.ProgramImageUrl = string.IsNullOrEmpty(r.ProgramImageUrl) ? string.Empty : CommonUtility.VirtualPath +
                                            Message.ProfilePicDirectory + r.ProgramImageUrl;
                        if (r.ExeciseVideoDetails != null)
                        {
                            if (string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseUrl))
                            {
                                r.ExeciseVideoLink = !string.IsNullOrEmpty(r.ExeciseThumbnailUrl) ? CommonUtility.VirtualFitComExercisePath +
                                                     Message.ExerciseVideoDirectory + r.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                            }
                            else
                            {
                                r.ExeciseVideoLink = r.ExeciseVideoDetails.ExeciseUrl;
                            }
                            if (!string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(r.ExeciseVideoDetails.ExerciseThumnail))
                            {
                                string thumnailName = r.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                string thumnailHeight = string.Empty;
                                string thumnailWidth = string.Empty;
                                r.ExeciseThumbnailUrl = string.IsNullOrEmpty(thumnailName) ? string.Empty : CommonUtility.VirtualFitComExercisePath +
                                                        Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                r.Height = thumnailHeight;
                                r.Width = thumnailWidth;
                            }
                            else
                            {
                                r.Height = string.Empty;
                                r.Width = string.Empty;
                                r.ExeciseThumbnailUrl = r.ExeciseVideoDetails.ExerciseThumnail;
                            }
                        }
                        if (!string.IsNullOrEmpty(r.ChallengeType))
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        }
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
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        r.Status = r.IsComplete ? AssignmentStatus.Completed : AssignmentStatus.Incomplete;
                    });

                    Total<List<UserAssignmentByTrainerVM>> objresult = new Total<List<UserAssignmentByTrainerVM>>();
                    objresult.TotalList = (from l in listPendingChallenge
                                           orderby l.DbCompletedDate descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = listPendingChallenge.Count();
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
                    traceLog.AppendLine("End  GetPendingChallengeList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                }
            }
        }
    }
}