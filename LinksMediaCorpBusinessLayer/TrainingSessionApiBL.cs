using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LinksMediaCorpEntity;
using LinksMediaCorpDataAccessLayer;
using System.Web;
using LinksMediaCorpUtility;
using AutoMapper;
using LinksMediaCorpUtility.Resources;
namespace LinksMediaCorpBusinessLayer
{
    public class TrainingSessionApiBL
    {
        /// <summary>
        /// Get the Trainer team user list for view  and get session from trainer
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static Total<List<SessionTeamUserVM>> GetTrainerSessionUserList(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetTrainerSessionUserList-userId-" + Convert.ToString(userId) + ",UserType-" + userType);
                    int teamId = userType.Equals(Message.UserTypeTrainer) ? (from crd in dataContext.Credentials
                                                                             join usr in dataContext.Trainer on crd.UserId equals usr.TrainerId
                                                                             where usr.TrainerId == userId && crd.UserType == Message.UserTypeTrainer
                                                                             select usr.TeamId).FirstOrDefault() : 0;
                    List<SessionTeamUserVM> objUserList = (from cred in dataContext.Credentials
                                                           join usr in dataContext.User on cred.UserId equals usr.UserId
                                                           where usr.TeamId == teamId && cred.UserType == Message.UserTypeUser
                                                           select new SessionTeamUserVM
                                                           {
                                                               CredID = cred.Id,
                                                               ID = cred.UserId,
                                                               FullName = usr.FirstName + " " + usr.LastName,
                                                               ImageUrl = usr.UserImageUrl,
                                                               UserType = cred.UserType,
                                                               UserId = cred.UserId,
                                                               RemaingNumberOfSession = dataContext.UserSessionDetails.FirstOrDefault(uu => uu.UserCredId == cred.Id).RemaingNumberOfSession ?? 0,
                                                               UsedNumberOfSession = dataContext.UserSessionDetails.FirstOrDefault(uu => uu.UserCredId == cred.Id).UsedNumberOfSession ?? 0
                                                           }).ToList();
                    // Remove the duplicate user from  team group
                    objUserList = objUserList.GroupBy(ul => ul.ID)
                                                     .Select(grp => grp.FirstOrDefault())
                                                     .ToList();
                    objUserList.ForEach(user =>
                    {
                        user.ImageUrl = (user.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + user.ImageUrl))) ?
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + user.ImageUrl : null;

                    });
                    Total<List<SessionTeamUserVM>> objresult = new Total<List<SessionTeamUserVM>>();
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
                    traceLog.AppendLine("End: GetTrainerSessionUserList: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Get User Session Details
        /// </summary>
        /// <param name="userCredId">user credetial Id</param>
        /// <returns>Return the User training session</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static SessionTeamUserVM GetUserSessionDetails(int userCredId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserSessionDetails-userCredId-" + Convert.ToString(userCredId));
                    SessionTeamUserVM objUsersessionDetails = (from cred in dataContext.Credentials
                                                               join usr in dataContext.User on cred.UserId equals usr.UserId
                                                               join us in dataContext.UserSessionDetails
                                                               on cred.Id equals us.UserCredId into usessession
                                                               from uss in usessession.DefaultIfEmpty()
                                                               where cred.Id == userCredId && cred.UserType == Message.UserTypeUser
                                                               select new SessionTeamUserVM
                                                               {
                                                                   CredID = cred.Id,
                                                                   UserId = cred.UserId,
                                                                   ID = cred.UserId,
                                                                   FullName = usr.FirstName + " " + usr.LastName,
                                                                   ImageUrl = usr.UserImageUrl,
                                                                   UserType = cred.UserType,
                                                                   RemaingNumberOfSession = uss.RemaingNumberOfSession ?? 0,
                                                                   UsedNumberOfSession = uss.UsedNumberOfSession ?? 0
                                                               }).FirstOrDefault();
                    if (objUsersessionDetails != null)
                    {
                        objUsersessionDetails.ImageUrl = (objUsersessionDetails.ImageUrl != null && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" +
                            Message.ProfilePicDirectory + objUsersessionDetails.ImageUrl))) ?
                            CommonUtility.VirtualPath + Message.ProfilePicDirectory + objUsersessionDetails.ImageUrl : null;
                    }
                    return objUsersessionDetails;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserSessionDetails: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        ///  Purchase the Traing Session for user by Trainer
        /// </summary>
        /// <param name="model">Provide the number of session and amount</param>
        /// <returns>It return Puchase session details</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static bool PurchaseSessionToUser(PurchaseTraingSessionVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            bool isSuccess = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    traceLog.AppendLine("Start: PurchaseTraingSession() Request Data:-UserCredID-" + model.UserCredId + ",NumberOfSession-" + model.NumberOfSession + ",Amount-" + model.Amount);
                    try
                    {
                        Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        Mapper.CreateMap<PurchaseTraingSessionVM, tblSessionPurchaseHistory>();
                        tblSessionPurchaseHistory objtblSessionPurchaseHistory = Mapper.Map<PurchaseTraingSessionVM, tblSessionPurchaseHistory>(model);
                        objtblSessionPurchaseHistory.TrainerId = objCredential.Id;
                        objtblSessionPurchaseHistory.PurchaseDateTime = DateTime.Now;
                        objtblSessionPurchaseHistory.CreatedDatetime = DateTime.Now;
                        objtblSessionPurchaseHistory.CreatedBy = objCredential.Id;
                        objtblSessionPurchaseHistory.ModifiedDatetime = objtblSessionPurchaseHistory.CreatedDatetime;
                        objtblSessionPurchaseHistory.ModifiedBy = objtblSessionPurchaseHistory.CreatedBy;
                        dataContext.SessionPurchaseHistories.Add(objtblSessionPurchaseHistory);
                        isSuccess = dataContext.SaveChanges() > 0;
                        if (isSuccess)
                        {
                            isSuccess = true;
                            if (dataContext.UserSessionDetails.Any(us => us.UserCredId == model.UserCredId))
                            {
                                tblUserSessionDetail objtblUserSessionDetail = dataContext.UserSessionDetails.FirstOrDefault(us => us.UserCredId == model.UserCredId);
                                objtblUserSessionDetail.RemaingNumberOfSession = objtblUserSessionDetail.RemaingNumberOfSession + model.NumberOfSession;
                                objtblUserSessionDetail.ModifiedDateTime = DateTime.Now;
                                objtblUserSessionDetail.ModifiedBy = objCredential.Id;
                            }
                            else
                            {
                                tblUserSessionDetail objtblUserSessionDetail = new tblUserSessionDetail();
                                objtblUserSessionDetail.RemaingNumberOfSession = model.NumberOfSession;
                                objtblUserSessionDetail.TrainerId = objCredential.Id;
                                objtblUserSessionDetail.UserCredId = model.UserCredId;
                                objtblUserSessionDetail.CreatedDatetime = DateTime.Now;
                                objtblUserSessionDetail.CreatedBy = objCredential.Id;
                                objtblUserSessionDetail.ModifiedDateTime = DateTime.Now;
                                objtblUserSessionDetail.ModifiedBy = objCredential.Id;
                                dataContext.UserSessionDetails.Add(objtblUserSessionDetail);
                            }
                            dataContext.SaveChanges();
                            isSuccess = true;
                        }
                        isSuccess = false;
                        dbTran.Commit();
                        return isSuccess;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: PurchaseSessionToUser: --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }

                }
            }
        }
        /// <summary>
        /// Purchase the Traing Session for user by Trainer
        /// </summary>
        /// <param name="userCrdId">User credtial Id</param>
        /// <param name="startIndex">start index number</param>
        /// <param name="endIndex">end index number</param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static TotalSession<List<PurchaseTraingSessionVM>> GetPurchaseSessionHistoryListToUser(int userCrdId, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PurchaseTraingSession() Request Data:-userCrdId-" + userCrdId + ",startIndex-" + startIndex + ",endIndex-" + endIndex);
                try
                {
                    var sessiondata = dataContext.SessionPurchaseHistories
                                        .Where(us => us.UserCredId == userCrdId)
                                        .Select(u => new PurchaseTraingSessionVM
                                        {
                                            PurchaseDateTime = u.PurchaseDateTime,
                                            Amount = u.Amount,
                                            NumberOfSession = u.NumberOfSession,
                                            PurchaseHistoryId = u.PurchaseHistoryId,
                                            CreatedDatetime = u.CreatedDatetime
                                        }).ToList();
                    // Remove the duplicate user from  team group
                    if (sessiondata != null)
                    {
                        sessiondata = sessiondata.GroupBy(ul => ul.PurchaseHistoryId)
                                                         .Select(grp => grp.FirstOrDefault())
                                                         .ToList();
                        sessiondata.ForEach(ss =>
                        {
                            ss.PurchaseDateTime = ss.PurchaseDateTime.ToUniversalTime();
                        });
                    }
                    TotalSession<List<PurchaseTraingSessionVM>> objresult = new TotalSession<List<PurchaseTraingSessionVM>>();
                    objresult.TotalList = (from l in sessiondata
                                           orderby l.CreatedDatetime descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    objresult.TotalCount = sessiondata.Count();
                    objresult.UserCredID = userCrdId;
                    if (dataContext.UserSessionDetails.Any(us => us.UserCredId == userCrdId))
                    {
                        var objtblUserSessionDetail = dataContext.UserSessionDetails.FirstOrDefault(us => us.UserCredId == userCrdId);
                        if (objtblUserSessionDetail != null)
                        {
                            objresult.RemaingNumberOfSession = objtblUserSessionDetail.RemaingNumberOfSession ?? 0;
                        }
                    }

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
                    traceLog.AppendLine("End: PurchaseSessionToUser: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get User SessionDetails with Trainer
        /// </summary>
        /// <param name="userCredId">User credtail Id</param>
        /// <returns>It return user seesion details</returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static UserSessionDetailVM GetUserSessionDetailsWithTrainer(int userCredId)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: GetUserSessionDetailsByTrainer-userCredId-" + Convert.ToString(userCredId));
                    UserSessionDetailVM objUserList = (from cred in dataContext.Credentials
                                                       join u in dataContext.User on cred.UserId equals u.UserId
                                                       join usr in dataContext.UserSessionDetails
                                                       on cred.Id equals usr.UserCredId into usessession
                                                       from uss in usessession.DefaultIfEmpty()
                                                       where cred.Id == userCredId && cred.UserType == Message.UserTypeUser
                                                       select new UserSessionDetailVM
                                                       {
                                                           UserSessionId = (long?)uss.UserSessionId ?? 0 ,
                                                           UserId = cred.UserId,
                                                           UserCredId = cred.Id,
                                                           UserType = cred.UserType,
                                                           UsedNumberOfSession = uss.UsedNumberOfSession ?? 0,
                                                           RemaingNumberOfSession = uss.RemaingNumberOfSession ?? 0,
                                                           UserName = u.FirstName + " " + u.LastName
                                                       }).FirstOrDefault();

                    return objUserList;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: GetUserSessionDetailsByTrainer: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update the Remaining session based on user session with Trainer
        /// </summary>
        /// <param name="userCredId">User credential Id</param>
        /// <param name="remaingNumberOfSession">update the remaining session</param>
        /// <returns>It returm ststus of update</returns>
        public static bool UpdateUserSessionDetailsWithTrainer(int userCredId, int remaingNumberOfSession)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: UpdateUserSessionDetailsWithTrainer-userCredId-" + Convert.ToString(userCredId) + ",remaingNumberOfSession-" + remaingNumberOfSession);
                    tblUserSessionDetail objtblUserSessionDetail = (from cred in dataContext.Credentials
                                                                    join usr in dataContext.UserSessionDetails on cred.Id equals usr.UserCredId
                                                                    where usr.UserCredId == userCredId
                                                                    select usr).FirstOrDefault();
                    if (objtblUserSessionDetail != null)
                    {
                        objtblUserSessionDetail.RemaingNumberOfSession = remaingNumberOfSession;
                    }
                    if (dataContext.SaveChanges() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: UpdateUserSessionDetailsWithTrainer: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static UserSessionDetailVM SaveUserTraingSession(UsedSessionsVM model)
        {

            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SaveUserTraingSession-userId-" + Convert.ToString(model.UserId) + ",UserType-" + model.UserType + ",IsAttended-" + model.IsAttended + ",IsTrack-" + model.IsTracNotes);
                UserSessionDetailVM userSession = new UserSessionDetailVM();
                switch (model.TrainingType)
                {
                    case UserTrainingType.PersonalTraining:
                        {
                            userSession.UserSessionId = SubmitPersonalUseSessionWithTrainer(model);
                        }
                        break;
                    case UserTrainingType.MobileTraining:
                        {
                            userSession.UserSessionId = SubmitUseMobileSessionWithTrainer(model);
                        }
                        break;
                }
                return userSession;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: SaveUserTraingSession: --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }


        /// <summary>
        /// Submit UseSession WithTrainer and retuen the userd seesion Id to submit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static long SubmitPersonalUseSessionWithTrainer(UsedSessionsVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    long usedSessionId = 0;
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SubmitUseSessionWithTrainer-userId-" + Convert.ToString(model.UserId) + ",UserType-" + model.UserType + ",IsAttended-" + model.IsAttended + ",IsTrack-" + model.IsTracNotes);
                        if (model.UserCredId > 0)
                        {
                            Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                            var useressiondeatils = dataContext.UserSessionDetails.FirstOrDefault(uss => uss.UserCredId == model.UserCredId);
                            int sunsessioncount = 0;
                            var purchasesedetails = dataContext.SessionPurchaseHistories.Where(us => us.UserCredId == model.UserCredId).ToList();
                            if (purchasesedetails != null)
                            {
                                sunsessioncount = purchasesedetails.Sum(tt => tt.NumberOfSession);
                            }
                            if (sunsessioncount > 0 && useressiondeatils.RemaingNumberOfSession > 0)
                            {
                                tblUsedSession objtblUsedSession = new tblUsedSession();
                                objtblUsedSession.UserCredId = model.UserCredId;
                                objtblUsedSession.TrainerId = objCredential.Id;
                                objtblUsedSession.IsAttended = model.IsAttended;
                                objtblUsedSession.TrainingType = Convert.ToString(model.TrainingType);
                                objtblUsedSession.UseSessionDateTime = model.UseSessionDateTime;
                                objtblUsedSession.CreatedBy = objCredential.Id;
                                objtblUsedSession.CreatedDatetime = DateTime.Now;
                                objtblUsedSession.ModifiedBy = objtblUsedSession.CreatedBy;
                                objtblUsedSession.ModifiedDatetime = objtblUsedSession.CreatedDatetime;
                                dataContext.UsedSessions.Add(objtblUsedSession);
                                if (dataContext.SaveChanges() > 0)
                                {
                                    if (useressiondeatils != null)
                                    {
                                        useressiondeatils.RemaingNumberOfSession = useressiondeatils.RemaingNumberOfSession - 1;
                                        useressiondeatils.UsedNumberOfSession = useressiondeatils.UsedNumberOfSession + 1;
                                        dataContext.SaveChanges();
                                    }
                                    usedSessionId = objtblUsedSession.UseSessionId;
                                }

                            }
                            dbTran.Commit();
                        }
                        return usedSessionId;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: UpdateUserSessionDetailsWithTrainer: --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Submit  Sunbmit Mobile UseSession WithTrainer and retuen the userd seesion Id to submit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static long SubmitUseMobileSessionWithTrainer(UsedSessionsVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SubmitUseMobileSessionWithTrainer-userId-" + Convert.ToString(model.UserId) + ",UserType-" + Convert.ToString(model.TrainingType) + ",IsAttended-" + model.IsAttended + ",IsTrack-" + model.IsTracNotes);
                    if (model.UserCredId > 0)
                    {
                        Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        tblUsedSession objtblUsedSession = new tblUsedSession();
                        objtblUsedSession.UserCredId = model.UserCredId;
                        objtblUsedSession.TrainerId = objCredential.Id;
                        objtblUsedSession.IsAttended = model.IsAttended;
                        objtblUsedSession.TrainingType = Convert.ToString(model.TrainingType);
                        objtblUsedSession.UseSessionDateTime = model.UseSessionDateTime;
                        objtblUsedSession.CreatedBy = objCredential.Id;
                        objtblUsedSession.CreatedDatetime = DateTime.Now;
                        objtblUsedSession.ModifiedBy = objtblUsedSession.CreatedBy;
                        objtblUsedSession.ModifiedDatetime = objtblUsedSession.CreatedDatetime;
                        dataContext.UsedSessions.Add(objtblUsedSession);
                        if (dataContext.SaveChanges() > 0)
                        {
                            return objtblUsedSession.UseSessionId;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    return 0;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: SubmitUseMobileSessionWithTrainer: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update the Used session IsTracNotes into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static long UpdateUseSessionWithTrainer(UsedSessionsVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: UpdateUseSessionWithTrainer-userId-" + Convert.ToString(model.UseSessionId) + ",IsTrack-" + model.IsTracNotes);
                    tblUsedSession objtblUsedSession = dataContext.UsedSessions.FirstOrDefault(us => us.UseSessionId == model.UseSessionId);
                    if (objtblUsedSession != null)
                    {
                        objtblUsedSession.IsTracNotes = model.IsTracNotes;
                        if (dataContext.SaveChanges() > 0)
                        {
                            return objtblUsedSession.UseSessionId;
                        }
                        return 0;
                    }
                    return 0;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: UpdateUseSessionWithTrainer: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Submit the Used session notes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static bool SubmitUsedSessionNotes(NotesUsedSessionVM model)
        {
            StringBuilder traceLog = null;
            bool isSavedsuccussfully = false;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: SubmitUsedSessionNotes-userId-" + Convert.ToString(model.UserId) + ",UserType-" + model.UserType);
                        Credentials objCredential = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        if (model.UseSessionId > 0)
                        {
                            tblUsedSession objtblUsedSession = dataContext.UsedSessions.FirstOrDefault(us => us.UseSessionId == model.UseSessionId);
                            objtblUsedSession.IsTracNotes = model.IsTracNotes;
                            dataContext.SaveChanges();
                            if (dataContext.UsedSessions.Any(us => us.UseSessionId == model.UseSessionId && model.IsTracNotes))
                            {
                                tblUsedSessionNote objtblNotesSession = null;
                                if (dataContext.UsedSessionNotes.Any(us => us.UseSessionId == model.UseSessionId && model.IsTracNotes))
                                {
                                    objtblNotesSession = dataContext.UsedSessionNotes.Where(us => us.UseSessionId == model.UseSessionId && model.IsTracNotes).FirstOrDefault();
                                    objtblNotesSession.Notes = model.Notes;
                                    objtblNotesSession.CreatedBy = objCredential.Id;
                                    objtblNotesSession.ModifiedBy = objtblNotesSession.CreatedBy;
                                    objtblNotesSession.ModifiedDatetime = DateTime.UtcNow;
                                }
                                else
                                {
                                    objtblNotesSession = new tblUsedSessionNote();
                                    objtblNotesSession.UseSessionId = model.UseSessionId;
                                    objtblNotesSession.Notes = model.Notes;
                                    objtblNotesSession.CreatedBy = objCredential.Id;
                                    objtblNotesSession.createdDatetime = DateTime.UtcNow;
                                    objtblNotesSession.ModifiedBy = objtblNotesSession.CreatedBy;
                                    objtblNotesSession.ModifiedDatetime = objtblNotesSession.createdDatetime;
                                    dataContext.UsedSessionNotes.Add(objtblNotesSession);
                                }
                                if (dataContext.SaveChanges() > 0)
                                {
                                    if (model.NoteExecises != null && model.NoteExecises.Count > 0)
                                    {
                                        foreach (var noteexecise in model.NoteExecises)
                                        {
                                            tblNotesExecise objtblNotesExecise = new tblNotesExecise
                                            {
                                                ExeciseName = noteexecise.ExeciseName,
                                                Notes = noteexecise.Notes,
                                                UsedSessionNoteId = objtblNotesSession.UsedSessionNoteId,
                                                CreatedBy = objCredential.Id,
                                                CreatedDatetime = DateTime.UtcNow,
                                                ModifiedBy = objCredential.Id,
                                                ModifiedDatetime = DateTime.UtcNow
                                            };
                                            dataContext.NotesExecise.Add(objtblNotesExecise);
                                            if (dataContext.SaveChanges() > 0)
                                            {
                                                List<tblSessionNotesExeciseSet> objexeciseset = new List<tblSessionNotesExeciseSet>();
                                                foreach (var execiseset in noteexecise.NoteExeciseSets)
                                                {
                                                    tblSessionNotesExeciseSet exeset = new tblSessionNotesExeciseSet()
                                                    {
                                                        Reps = execiseset.Reps,
                                                        Weight = execiseset.Weight,
                                                        NotesExeciseId = objtblNotesExecise.NotesExeciseId,
                                                        CreatedBy = objCredential.Id,
                                                        CreatedDatetime = DateTime.UtcNow,
                                                        ModifiedBy = objCredential.Id,
                                                        ModifiedDatetime = DateTime.UtcNow
                                                    };
                                                    objexeciseset.Add(exeset);
                                                }
                                                // Add associates execise sets in database for Execise Notes
                                                dataContext.SessionNotesExeciseSets.AddRange(objexeciseset);
                                                dataContext.SaveChanges();
                                            }
                                        }

                                    }
                                    isSavedsuccussfully = true;
                                }
                                dbTran.Commit();
                            }
                        }
                        return isSavedsuccussfully;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("End: SubmitUsedSessionNotes: --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }
        /// <summary>
        /// Get the Used session notes details
        /// </summary>
        /// <param name="model">provide UsedSessionNoteId,UseSessionId</param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static NotesUsedSessionVM GetUsedSessionNotes(NotesUsedSessionVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SubmitUsedSessionNotes-UsedSessionNoteId-" + Convert.ToString(model.UsedSessionNoteId));
                    if (dataContext.UsedSessions.Any(us => us.UseSessionId == model.UseSessionId && us.IsTracNotes == true))
                    {
                        var usedNotesDetails = (from uss in dataContext.UsedSessionNotes
                                                where uss.UsedSessionNoteId == model.UsedSessionNoteId
                                                select new NotesUsedSessionVM
                                                {
                                                    Notes = uss.Notes,
                                                    UsedSessionNoteId = uss.UsedSessionNoteId,
                                                    NoteExecises = (from ne in dataContext.NotesExecise
                                                                    where ne.UsedSessionNoteId == uss.UsedSessionNoteId
                                                                    select new NoteExeciseVM
                                                                    {
                                                                        ExeciseName = ne.ExeciseName,
                                                                        Notes = ne.Notes,
                                                                        NotesExeciseId = ne.NotesExeciseId,
                                                                        NoteExeciseSets = (from nes in dataContext.SessionNotesExeciseSets
                                                                                           where nes.NotesExeciseId == ne.NotesExeciseId
                                                                                           select new NoteExeciseSetVM
                                                                                           {
                                                                                               Reps = nes.Reps,
                                                                                               Weight = nes.Weight,
                                                                                               ExeciseSetId = nes.ExeciseSetId
                                                                                           }).ToList()
                                                                    }).ToList()
                                                }).FirstOrDefault();
                        return usedNotesDetails;

                    }
                    return null;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: SubmitUsedSessionNotes: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get the Used session notes details
        /// </summary>
        /// <param name="model">provide UsedSessionNoteId,UseSessionId</param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>[HttpPost]
        public static NotesUsedSessionVM GetMobileUsedSessionNotes(NotesUsedSessionVM model)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: SubmitUsedSessionNotes-UsedSessionNoteId-" + Convert.ToString(model.UsedSessionNoteId));
                    if (dataContext.UsedSessions.Any(us => us.UseSessionId == model.UseSessionId))
                    {
                        var usedNotesDetails = (from uss in dataContext.UsedSessionNotes
                                                where uss.UsedSessionNoteId == model.UsedSessionNoteId
                                                select new NotesUsedSessionVM
                                                {
                                                    Notes = uss.Notes,
                                                    UsedSessionNoteId = uss.UsedSessionNoteId,
                                                    NoteExecises = (from ne in dataContext.NotesExecise
                                                                    where ne.UsedSessionNoteId == uss.UsedSessionNoteId
                                                                    select new NoteExeciseVM
                                                                    {
                                                                        ExeciseName = ne.ExeciseName,
                                                                        Notes = ne.Notes,
                                                                        NotesExeciseId = ne.NotesExeciseId,
                                                                        NoteExeciseSets = (from nes in dataContext.SessionNotesExeciseSets
                                                                                           where nes.NotesExeciseId == ne.NotesExeciseId
                                                                                           select new NoteExeciseSetVM
                                                                                           {
                                                                                               Reps = nes.Reps,
                                                                                               Weight = nes.Weight,
                                                                                               ExeciseSetId = nes.ExeciseSetId
                                                                                           }).ToList()
                                                                    }).ToList()
                                                }).FirstOrDefault();
                        return usedNotesDetails;

                    }
                    return null;

                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End: SubmitUsedSessionNotes: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Users UseSession List
        /// </summary>
        /// <param name="userCrdId">user credetail Id</param>
        /// <param name="startIndex">startIndex count</param>
        /// <param name="endIndex">endIndex count</param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2016
        /// </devdoc>     
        public static TotalSession<List<UsedSessionsVM>> GetUsersUseSessionList(int userCrdId, UserTrainingType trainingType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: PurchaseTraingSession() Request Data:-userCrdId-" + userCrdId + ",startIndex-" + startIndex + ",endIndex-" + endIndex);
                try
                {
                    string personalTraining = Convert.ToString(trainingType);
                    var sessiondata = (from us in dataContext.UsedSessions
                                       join usn in dataContext.UsedSessionNotes
                                       on us.UseSessionId equals usn.UseSessionId into usernotes
                                       from usnn in usernotes.DefaultIfEmpty()
                                       where us.UserCredId == userCrdId && us.TrainingType.Equals(personalTraining, StringComparison.OrdinalIgnoreCase)
                                       orderby us.CreatedDatetime descending
                                       select new UsedSessionsVM
                                       {
                                           UseSessionDateTime = us.UseSessionDateTime,
                                           IsAttended = us.IsAttended,
                                           IsTracNotes = us.IsTracNotes,
                                           UserCredId = us.UserCredId,
                                           UseSessionId = us.UseSessionId,
                                           CreatedDatetime = us.CreatedDatetime,
                                           UseSessionNoteId = (long?)usnn.UsedSessionNoteId ?? 0,
                                           Notes = usnn.Notes != null ? usnn.Notes : string.Empty
                                       }).ToList();

                    // Remove the duplicate user from  team group
                    if (sessiondata != null)
                    {
                        sessiondata = sessiondata.GroupBy(ul => ul.UseSessionId)
                                                         .Select(grp => grp.FirstOrDefault())
                                                         .ToList();
                    }
                    TotalSession<List<UsedSessionsVM>> objresult = new TotalSession<List<UsedSessionsVM>>();
                    objresult.TotalList = (from l in sessiondata
                                           orderby l.CreatedDatetime descending
                                           select l).Skip(startIndex).Take(endIndex - startIndex).ToList();
                    if (sessiondata != null)
                    {
                        objresult.TotalCount = sessiondata.Count();
                    }
                    objresult.UserCredID = userCrdId;
                    var usersessiondetails = dataContext.UserSessionDetails.FirstOrDefault(usd => usd.UserCredId == userCrdId);
                    if (usersessiondetails != null)
                    {
                        objresult.RemaingNumberOfSession = usersessiondetails.RemaingNumberOfSession ?? 0;
                    }
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
                    traceLog.AppendLine("End: GetUsersUseSessionList: --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

    }
}