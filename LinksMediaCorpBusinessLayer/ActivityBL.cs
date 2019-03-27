namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using LinksMediaCorpDataAccessLayer;
    using AutoMapper;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    public class ActivityBL
    {
        /// <summary>
        /// Function to submit activity into database
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/01/2015
        /// </devdoc>   
        public static void SubmitActivity(ActivityVM objActivityVM)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction tran = dataContext.Database.BeginTransaction())
                {
                    traceLog = new StringBuilder();
                    try
                    {
                        traceLog.AppendLine("Start: SubmitActivity for creating activity");
                        tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == objActivityVM.TrainerId && c.UserType == Message.UserTypeTrainer);
                        int trainerCredId = objCred.Id;
                        objActivityVM.TrainerId = trainerCredId;
                        Mapper.CreateMap<ActivityVM, tblActivity>();
                        tblActivity objActivity = Mapper.Map<ActivityVM, tblActivity>(objActivityVM);
                        objActivity.CreatedDate = DateTime.Now;
                        objActivity.ModifiedDate = objActivity.CreatedDate;
                        // Add activity in to database
                        dataContext.Activity.Add(objActivity);
                        dataContext.SaveChanges();
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("SubmitActivity  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to update activity
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/03/2015
        /// </devdoc>
        public static void UpdateActivity(ActivityVM objActivityVM)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction tran = dataContext.Database.BeginTransaction())
                {
                    traceLog = new StringBuilder();
                    try
                    {
                        traceLog.AppendLine("Start: UpdateActivity for updating activity");
                        tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.UserId == objActivityVM.TrainerId && c.UserType == Message.UserTypeTrainer);
                        int trainerCredId = objCred.Id;
                        objActivityVM.TrainerId = trainerCredId;
                        objActivityVM.ModifiedDate = DateTime.Now;
                        tblActivity objActivity = dataContext.Activity.Where(ac => ac.ActivityId == objActivityVM.ActivityId).FirstOrDefault();
                        if (objActivity != null)
                        {
                            objActivity.AddressLine1 = objActivityVM.AddressLine1;
                            objActivity.TrainerId = objActivityVM.TrainerId;
                            objActivity.ModifiedDate = objActivityVM.ModifiedDate;
                            objActivity.AddressLine2 = objActivityVM.AddressLine2;
                            objActivity.DateOfEvent = objActivityVM.DateOfEvent;
                            objActivity.Description = objActivityVM.Description;
                            objActivity.NameOfActivity = objActivityVM.NameOfActivity;
                            objActivity.Pic = objActivityVM.Pic;
                            objActivity.PromotionText = objActivityVM.PromotionText;
                            objActivity.State = objActivityVM.State;
                            objActivity.Video = objActivityVM.Video;
                            objActivity.Zip = objActivityVM.Zip;
                            objActivity.LearnMore = objActivityVM.LearnMore;
                            objActivity.City = Convert.ToInt32(objActivityVM.City);
                        }
                        dataContext.SaveChanges();
                        ///set activivty to featured queue in database
                        if (objActivityVM.StartDate != null && objActivityVM.EndDate != null)
                        {
                            tblFeaturedActivityQueue featuredActivityQueue = dataContext.FeaturedActivityQueue.Where(ce => ce.ActivityId == objActivityVM.ActivityId).FirstOrDefault();
                            tblFeaturedActivityQueue featuredActivity = new tblFeaturedActivityQueue();
                            /*if featured activity is created then update else create new featured activity*/
                            if (featuredActivityQueue != null)
                            {
                                featuredActivityQueue.StartDate = objActivityVM.StartDate;
                                featuredActivityQueue.ActivityId = objActivityVM.ActivityId;
                                featuredActivityQueue.EndDate = objActivityVM.EndDate;
                            }
                            else
                            {
                                featuredActivity.StartDate = objActivityVM.StartDate;
                                featuredActivity.ActivityId = objActivityVM.ActivityId;
                                featuredActivity.EndDate = objActivityVM.EndDate;
                                dataContext.FeaturedActivityQueue.Add(featuredActivity);
                            }
                            dataContext.SaveChanges();
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("UpdateActivity  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to get activities from database for displaying 
        /// </summary>
        /// <returns>List<ViewActivitiesVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/04/2015
        /// </devdoc>
        public static List<ViewActivitiesVM> GetActivities()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: GetTrainers for retrieving challenges from database ");
                    List<ViewActivitiesVM> filterdActivities = new List<ViewActivitiesVM>();
                    DateTime today = DateTime.Now.Date;
                    List<ViewActivitiesVM> objActivity = (from A in dataContext.Activity
                                                          join C in dataContext.Credentials on A.TrainerId equals C.Id
                                                          join S in dataContext.States on A.State equals S.StateCode
                                                          join Ct in dataContext.Cities on A.City equals Ct.CityId
                                                          join T in dataContext.Trainer on C.UserId equals T.TrainerId
                                                          orderby A.ModifiedDate descending
                                                          select new ViewActivitiesVM
                                                          {
                                                              ActivityId = A.ActivityId,
                                                              NameOfActivity = A.NameOfActivity,
                                                              TrainerName = T.FirstName + " " + T.LastName,
                                                              DateofEvent = A.DateOfEvent,
                                                              Location = Ct.CityName + ", " + S.StateName
                                                          }).ToList<ViewActivitiesVM>();
                    foreach (var item in objActivity)
                    {
                        tblFeaturedActivityQueue featuredActivityQueue = dataContext.FeaturedActivityQueue.FirstOrDefault(c => c.ActivityId == item.ActivityId);
                        /*add featured activity in queue to display on dashborad*/
                        if (featuredActivityQueue != null)
                        {
                            if ((featuredActivityQueue.StartDate <= today && featuredActivityQueue.EndDate <= today)
                                || (featuredActivityQueue.StartDate >= today && featuredActivityQueue.EndDate >= today))
                            {
                                ViewActivitiesVM filterdActivity = new ViewActivitiesVM();
                                filterdActivity = item;
                                filterdActivities.Add(filterdActivity);
                            }
                        }
                        else
                        {
                            ViewActivitiesVM filterdActivity = new ViewActivitiesVM();
                            filterdActivity = item;
                            filterdActivities.Add(filterdActivity);
                        }
                    }
                    return filterdActivities;
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
        /// Function to delete activity
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/04/2015
        /// </devdoc>
        public static void DeleteActivity(int id)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    traceLog = new StringBuilder();
                    try
                    {
                        traceLog.AppendLine("Start: DeleteActivity  --- " + DateTime.Now.ToLongDateString());
                        tblActivity activity = dataContext.Activity.Find(id);
                        tblFeaturedActivityQueue featuredActivityQueue = dataContext.FeaturedActivityQueue.Where(ce => ce.ActivityId == id).FirstOrDefault();
                        /*Delete Related feateured activity for the activity*/
                        if (featuredActivityQueue != null)
                        {
                            dataContext.FeaturedActivityQueue.Remove(featuredActivityQueue);
                            dataContext.SaveChanges();
                        }
                        dataContext.Activity.Remove(activity);
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
                        traceLog.AppendLine("DeleteActivity  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Function to get activity by id
        /// </summary>
        /// <returns>ActivityVM</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/03/2015
        /// </devdoc>
        public static ActivityVM GetActivityById(int id)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    ///Get challenge detail by challenge
                    traceLog.AppendLine("Start: GetActivityById for retrieving activity by id:" + id);
                    tblActivity activity = dataContext.Activity.Find(id);
                    Mapper.CreateMap<tblActivity, ActivityVM>();
                    ActivityVM objActivity = Mapper.Map<tblActivity, ActivityVM>(activity);
                    tblFeaturedActivityQueue featuredActivityQueue = dataContext.FeaturedActivityQueue.Where(ce => ce.ActivityId == id).FirstOrDefault();
                    if (featuredActivityQueue != null)
                    {
                        objActivity.StartDate = featuredActivityQueue.StartDate;
                        objActivity.EndDate = featuredActivityQueue.EndDate;
                    }
                    tblCredentials objCred = dataContext.Credentials.FirstOrDefault(c => c.Id == objActivity.TrainerId && c.UserType == Message.UserTypeTrainer);
                    if (objCred != null)
                    {
                        objActivity.TrainerId = objCred.UserId;
                    }
                    return objActivity;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetActivityById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get activity count fron the database 
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetActivityCount()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: GetActivityCount");
                    int activityCount = dataContext.Activity.Count();
                    return activityCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetActivityCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get featured activity fron the database 
        /// </summary>
        /// <returns>IEnumerable<FeaturedActivityQueue></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static IEnumerable<FeaturedActivityQueue> GetFeaturedActivity()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: GetFeaturedActivity");
                    DateTime todayDate = DateTime.Now.Date;
                    //  IEnumerable<FeaturedActivityQueue> objActivity 
                    return (from A in dataContext.Activity
                            join C in dataContext.Credentials on A.TrainerId equals C.Id
                            join S in dataContext.States on A.State equals S.StateCode
                            join Ct in dataContext.Cities on A.City equals Ct.CityId
                            join T in dataContext.Trainer on C.UserId equals T.TrainerId
                            join FA in dataContext.FeaturedActivityQueue on A.ActivityId equals FA.ActivityId
                            where FA.StartDate <= todayDate && FA.EndDate >= todayDate
                            orderby A.ModifiedDate descending
                            select new FeaturedActivityQueue
                            {
                                QueueId = FA.Id,
                                ActivityId = A.ActivityId,
                                NameOfActivity = A.NameOfActivity,
                                TrainerName = T.FirstName + " " + T.LastName,
                                DateofEvent = A.DateOfEvent,
                                Location = Ct.CityName + ", " + S.StateName,
                                PromotionText = A.PromotionText
                            }).ToList();
                    //return objActivity;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetFeaturedActivity  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function delete featured activity
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/21/2015
        /// </devdoc>
        public static void DeleteFeaturedActivity(int id)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Start: DeleteFeaturedActivity");
                    tblFeaturedActivityQueue featuredActivityQueue = dataContext.FeaturedActivityQueue.Where(ce => ce.Id == id).FirstOrDefault();
                    if (featuredActivityQueue != null)
                    {
                        dataContext.FeaturedActivityQueue.Remove(featuredActivityQueue);
                        dataContext.SaveChanges();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("DeleteActivity  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }

        }
        /// <summary>
        /// Get DashBoard Activity Count
        /// </summary>
        /// <returns></returns>
        public static DashBoardActivityCount GetDashBoardActivityCount()
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                DashBoardActivityCount objDashBoardActivity = new DashBoardActivityCount();
                try
                {
                    traceLog.AppendLine("Start: GetDashBoardActivityCount()");
                    List<int> ChallengeSubTypeList = ChallengesBL.GetFitnessTestORWorkoutSubTypeId(ConstantHelper.constFittnessCommonSubTypeId);
                    objDashBoardActivity.FitnessTestCount = dataContext.Challenge.Count(ch => !ch.IsDraft && ChallengeSubTypeList.Contains(ch.ChallengeSubTypeId));
                    objDashBoardActivity.ActiveFitnessTestCount = dataContext.Challenge.Count(ch => !ch.IsDraft && ch.IsActive 
                    && ChallengeSubTypeList.Contains(ch.ChallengeSubTypeId));
                    objDashBoardActivity.UserCount = dataContext.User.Count();

                    //objDashBoardActivity.PremiumUserCount = (from crd in dataContext.Credentials                                                            
                    //                                         where crd.UserType != Message.UserTypeTeam
                    //                                         && (crd.IOSSubscriptionStatus || crd.AndriodSubscriptionStatus)
                    //                                         select crd.Id
                    //                                  ).Distinct().Count();

                    objDashBoardActivity.PremiumUserCount = (from usr in dataContext.User
                                                      join crd in dataContext.Credentials
                                                      on usr.UserId equals crd.UserId
                                                      where crd.UserType == Message.UserTypeUser 
                                                      && (crd.IOSSubscriptionStatus || crd.AndriodSubscriptionStatus)
                                                      select crd.Id
                                                      ).Distinct().Count();
                    objDashBoardActivity.TrainerCount = dataContext.Trainer.Count();
                    objDashBoardActivity.TeamCount = dataContext.Teams.Count();                
                    objDashBoardActivity.WorkoutCount = dataContext.Challenge.Count(workt => !workt.IsDraft 
                                                        && workt.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType);
                    objDashBoardActivity.ActiveWorkoutCount = dataContext.Challenge.Count(workt => !workt.IsDraft && workt.IsActive
                                                         && workt.ChallengeSubTypeId == ConstantHelper.constWorkoutChallengeSubType);
                    objDashBoardActivity.ProgramCount = dataContext.Challenge.Count(y => !y.IsDraft 
                                                                              && y.ChallengeSubTypeId== ConstantHelper.constProgramChallengeSubType);
                    objDashBoardActivity.ActiveProgramCount = dataContext.Challenge.Count(y => !y.IsDraft &&  y.IsActive && 
                                                        y.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType);
                    objDashBoardActivity.ActivityCount = dataContext.Activity.Count();
                    return objDashBoardActivity;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetDashBoardActivityCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
    }
}