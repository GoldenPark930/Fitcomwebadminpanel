namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using LinksMediaCorpDataAccessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using AutoMapper;
    using System.Data.Entity;
    using System.Configuration;
    using LinksMediaCorpUtility.Resources;
    using System.Threading;
    using System.Web;
    using System.IO;
    using System.Drawing;
    using System.Globalization;
    using System.Text.RegularExpressions;  
    public class UseresBL
    {
        /// <summary>
        /// Function to get users
        /// </summary>
        /// <returns>List<CreateUserVM></returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/23/2015
        /// </devdoc>
        public static List<CreateUserVM> GetUsers(int teamId = 0, string search = null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                List<CreateUserVM> users = new List<CreateUserVM>();
                try
                {
                    traceLog.AppendLine("Start: GetUsers for retrieving user from database ");
                    IQueryable<CreateUserVM> listusers = (from U in _dbContext.User
                                                          join Crd in _dbContext.Credentials
                                                          on U.UserId equals Crd.UserId
                                                          join T in _dbContext.Teams
                                                          on U.TeamId equals T.TeamId into userteam
                                                          from UT in userteam.DefaultIfEmpty()
                                                          where Crd.UserType == Message.UserTypeUser
                                                          orderby U.ModifiedDate descending
                                                          select new CreateUserVM
                                                          {
                                                              FullName = U.FirstName + " " + U.LastName,
                                                              CreatedDate = U.CreatedDate,
                                                              UserId = U.UserId,
                                                              ZipCode = U.ZipCode,
                                                              DateOfBirth = U.DateOfBirth,
                                                              EmailId = U.EmailId,
                                                              Gender = U.Gender,
                                                              TeamName = UT.TeamName ?? string.Empty,
                                                              UniqueTeamId = (int?)UT.UniqueTeamId ?? 0,
                                                              MTActive = U.MTActive,
                                                              TeamId = U.TeamId,
                                                              IsPremiumMember = Crd.IOSSubscriptionStatus ? Crd.IOSSubscriptionStatus : Crd.AndriodSubscriptionStatus,
                                                              UserCredId= Crd.Id
                                                          });
                    if (teamId == 0)
                    {
                        users = listusers.OrderBy(tm=>tm.FullName).ToList();
                    }
                    else
                    {
                        users = listusers.Where(usr=>usr.TeamId == teamId).OrderBy(tm => tm.FullName).ToList();
                    }
                    if(!string.IsNullOrEmpty(search))
                    {
                        search = search.ToUpper(CultureInfo.InvariantCulture);
                        users = users.Where(usr => (usr.FullName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)  || (usr.TeamName.ToUpper(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1)).OrderBy(tm => tm.FullName).ToList();
                    }
                    users.ForEach(usr =>
                        {
                            usr.TeamName = string.IsNullOrEmpty(usr.TeamName) ? string.Empty : usr.TeamName.Substring(1);
                            usr.MTActiveStatus = usr.MTActive ? ConstantHelper.constYes : ConstantHelper.constNo;
                            usr.PremiumMemberStatus = usr.IsPremiumMember ? ConstantHelper.constYes : ConstantHelper.constNo;
                            var subscriptionStatus = _dbContext.AppSubscriptions.Where(appSub => appSub.UserCredId == usr.UserCredId).FirstOrDefault();
                            if (subscriptionStatus != null)
                            {
                                //if (subscriptionStatus.DeviceType == DeviceType.IOS.ToString())
                                //{
                                //    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                                //    var localsubscribtion = epoch.AddMilliseconds(subscriptionStatus.Expires_date_ms);
                                //    if (localsubscribtion.Date >= DateTime.Now.Date && subscriptionStatus.AutoRenewing)
                                //    {
                                //        usr.SubscriptionStatusLebel = ConstantHelper.constSubcribtionPurchase;
                                //    }
                                //    else
                                //    {
                                //        usr.SubscriptionStatusLebel = ConstantHelper.constSubcribtionCancellation;
                                //    }
                                //}
                                //else
                                //{
                                usr.SubscriptionStatusLebel = (usr.IsPremiumMember && subscriptionStatus.AutoRenewing) ? ConstantHelper.constSubcribtionPurchase : ConstantHelper.constSubcribtionCancellation;
                             
                                //}


                            }
                            else
                            {
                                usr.SubscriptionStatusLebel = "";
                            }
                             
                            
                        });
                    return users;
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

        /// <summary>
        /// Function to submit user in to database  
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/23/2015
        /// </devdoc>
        public static async Task SubmitUser(CreateUserVM objCreateUserVM)
        {
            StringBuilder traceLog = null;
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        objEncrypt = new EncryptDecrypt();
                        traceLog.AppendLine("Start: SubmitUser for creating user");
                        objCreateUserVM.FirstName = objCreateUserVM.FirstName.Trim();
                        objCreateUserVM.LastName = objCreateUserVM.LastName.Trim();
                        Mapper.CreateMap<CreateUserVM, tblUser>();
                        tblUser objUser = Mapper.Map<CreateUserVM, tblUser>(objCreateUserVM);
                        objUser.CreatedDate = DateTime.Now;
                        objUser.ModifiedDate = objUser.CreatedDate;
                        objUser.MTActive = objUser.MTActive;
                        //Getting City State (Address) of the User From ASMX Web Service
                        if (!string.IsNullOrEmpty(objUser.ZipCode) && !objUser.ZipCode.Equals(objUser.ZipCode))
                        {
                            string[] arrCityState = await GetCityAndStateOnZipcode(objUser.ZipCode);
                            if (arrCityState.Count() == 2)
                            {
                                objUser.City = arrCityState[0];
                                objUser.State = arrCityState[1];
                            }
                        }
                        /*Add user in to database*/
                        _dbContext.User.Add(objUser);
                        _dbContext.SaveChanges();
                        int userId = Convert.ToInt32(_dbContext.User.Max(x => x.UserId));
                        /*Add credential information into database*/
                        tblCredentials objUserCredential = new tblCredentials();
                        objUserCredential.Password = objEncrypt.EncryptString(objCreateUserVM.Password);
                        objUserCredential.UserId = userId;
                        objUserCredential.EmailId = objCreateUserVM.EmailId;
                        objUserCredential.UserType = LinksMediaCorpUtility.Resources.Message.UserTypeUser;
                        _dbContext.Credentials.Add(objUserCredential);
                        _dbContext.SaveChanges();
                        var defautteam = _dbContext.Teams.FirstOrDefault(t => t.IsDefaultTeam);
                        if (defautteam != null && defautteam.TeamId > 0)
                        {
                            List<int> teamtrainerList = (from tr in _dbContext.Trainer
                                                         join crd in _dbContext.Credentials
                                                         on tr.TrainerId equals crd.UserId
                                                         where crd.UserType == Message.UserTypeTrainer && tr.TeamId == defautteam.TeamId
                                                         select crd.Id).ToList();
                            List<tblFollowings> followingteamtrainers = new List<tblFollowings>();
                            foreach (int teamtrainerID in teamtrainerList)
                            {
                                if (!_dbContext.Followings.Any(f => f.UserId == objUserCredential.Id && f.FollowUserId == teamtrainerID))
                                {
                                    tblFollowings objFollowing = new tblFollowings();
                                    objFollowing.UserId = objUserCredential.Id;
                                    objFollowing.FollowUserId = teamtrainerID;
                                    followingteamtrainers.Add(objFollowing);
                                }
                                _dbContext.Followings.AddRange(followingteamtrainers);
                            }
                            if (!_dbContext.TrainerTeamMembers.Any(utm => utm.UserId == objUserCredential.Id && utm.TeamId == defautteam.TeamId))
                            {
                                tblTrainerTeamMembers objTrainerTeamMember = new tblTrainerTeamMembers();
                                objTrainerTeamMember.TeamId = defautteam.TeamId;
                                objTrainerTeamMember.UserId = objUserCredential.Id;
                                objTrainerTeamMember.CreatedBy = objCreateUserVM.CreatedBy;
                                objTrainerTeamMember.CreatedDate = DateTime.Now;
                                objTrainerTeamMember.ModifiedBy = objCreateUserVM.CreatedBy;
                                objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                _dbContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                            }
                            //Update the user team information
                            var user = _dbContext.User.FirstOrDefault(usr => usr.UserId == userId);
                            if (user != null)
                            {
                                user.TeamId = defautteam.TeamId;
                            }
                        }
                        _dbContext.SaveChanges();
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
                        traceLog.AppendLine("SubmitUser  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to update user
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/24/2015
        /// </devdoc>
        public static async Task UpdateUser(UpdateUserVM objUpdateUserVM)
        {
            StringBuilder traceLog = new StringBuilder();
            EncryptDecrypt objEncrypt = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: UpdaeUser for updating user");
                        objEncrypt = new EncryptDecrypt();
                        objUpdateUserVM.FirstName = objUpdateUserVM.FirstName.Trim();
                        objUpdateUserVM.LastName = objUpdateUserVM.LastName.Trim();
                        tblUser objUser = _dbContext.User.FirstOrDefault(usr => usr.UserId == objUpdateUserVM.UserId);
                        if (objUser != null)
                        {
                            objUser.FirstName = objUpdateUserVM.FirstName;
                            objUser.LastName = objUpdateUserVM.LastName;
                            objUser.Gender = objUpdateUserVM.Gender;
                            objUser.DateOfBirth = objUpdateUserVM.DateOfBirth;
                            objUser.EmailId = objUpdateUserVM.EmailId;
                            objUser.ZipCode = objUpdateUserVM.ZipCode;
                            objUser.MTActive = objUpdateUserVM.MTActive;
                            if (objUser != null && !string.IsNullOrEmpty(objUser.ZipCode) && !objUser.ZipCode.Equals(objUser.ZipCode))
                            {
                                //Getting City State (Address) of the User From ASMX Web Service
                                string[] arrCityState = await GetCityAndStateOnZipcode(objUser.ZipCode);
                                if (arrCityState != null && arrCityState.Count() == 2)
                                {
                                    objUser.City = arrCityState[0];
                                    objUser.State = arrCityState[1];
                                }
                            }
                            objUser.ModifiedDate = DateTime.Now;
                        }
                        /*update credentials */
                        tblCredentials userCredentials = _dbContext.Credentials.Where(ce => ce.UserId == objUser.UserId && ce.UserType == Message.UserTypeUser).FirstOrDefault();
                        if (userCredentials != null)
                        {
                            if (objUpdateUserVM.IsUserChangePassword && !string.IsNullOrEmpty(objUpdateUserVM.Password))
                            {
                                userCredentials.Password = objEncrypt.EncryptString((objUpdateUserVM.Password).Trim());
                                objUser.Password = objEncrypt.EncryptString((objUpdateUserVM.Password).Trim());
                            }
                            userCredentials.EmailId = objUpdateUserVM.EmailId;
                        }
                        _dbContext.SaveChanges();
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
                        traceLog.AppendLine("UpdaeUser  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to delete user
        /// </summary>
        /// <returns>void</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/24/2015
        /// </devdoc>
        public static void DeleteUser(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog.AppendLine("Start: DeleteUser for deleting user");
                        tblUser user = _dbContext.User.Find(Id);
                        /*Delete Related COD challenge of this user*/
                        List<tblChallengeofTheDayQueue> challengeoftheDay = _dbContext.ChallengeofTheDayQueue.Where(ce => ce.UserId == Id).ToList();
                        /*challenge of the day for this user empty then delete related cod*/
                        if (challengeoftheDay != null)
                        {
                            _dbContext.ChallengeofTheDayQueue.RemoveRange(challengeoftheDay);
                            _dbContext.SaveChanges();
                        }
                        /*Delete Related accepted challenge of this user*/
                        tblCredentials objCred = _dbContext.Credentials.FirstOrDefault(ce => ce.UserId == Id && ce.UserType == Message.UserTypeUser);
                        if (objCred != null)
                        {
                            CommonBL.DeleteUserAssociatedRecords(_dbContext, traceLog, objCred.Id);

                        }
                        _dbContext.Credentials.Remove(objCred);
                        _dbContext.User.Remove(user);
                        _dbContext.SaveChanges();
                        dbTran.Commit();
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        traceLog.AppendLine("DeleteUser  end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to get user by id
        /// </summary>
        /// <returns>UpdateUserVM</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/25/2015
        /// </devdoc>
        public static UpdateUserVM GetUserById(int Id)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetUserById for retrieving user by userid:" + Id);
                    tblUser user = _dbContext.User.Find(Id);
                    Mapper.CreateMap<tblUser, UpdateUserVM>();
                    UpdateUserVM objUser = Mapper.Map<tblUser, UpdateUserVM>(user);
                    return objUser;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUserById  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get trainer credentials by Trainer Cred Id
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 07/13/2015
        /// </devdoc>
        public static int GetActualId(int Id, string type)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start GetActualId : --- " + DateTime.Now.ToLongDateString());
                    return _dbContext.Credentials.FirstOrDefault(c => c.Id == Id && c.UserType == type).UserId;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End GetActualId : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to check Email for user 
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/25/2015
        /// </devdoc>
        public static string GetEmail(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetEmail-tearm-" + tearm);
                    string listOut = string.Empty;
                    /*if string conmtain ',' then check emial for another users not for current user else for all*/
                    if (tearm.Contains(','))
                    {
                        string email = string.Empty;
                        int id = 0;
                        int index = tearm.LastIndexOf(',');
                        email = tearm.Substring(0, index);
                        id = Convert.ToInt32(tearm.Substring(index + 1));
                        listOut = _dbContext.User.Where(ct => ct.EmailId == email && ct.UserId != id).Select(y => y.EmailId).FirstOrDefault();
                        tearm = email;
                    }
                    else
                    {
                        listOut = _dbContext.User.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }
                    if (listOut == null)
                    {
                        listOut = _dbContext.Trainer.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }
                    if (listOut == null)
                    {
                        listOut = _dbContext.Teams.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }
                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetEmail  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to check Email for trainer 
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/25/2015
        /// </devdoc>
        public static string GetTrainerEmail(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerEmail");
                    string listOut = string.Empty;
                    /*if string conmtain ',' then check emial for another users not for current user else for all*/
                    if (tearm.Contains(','))
                    {
                        string email = string.Empty;
                        int id = 0;
                        int index = tearm.LastIndexOf(',');
                        email = tearm.Substring(0, index);
                        id = Convert.ToInt32(tearm.Substring(index + 1));
                        listOut = _dbContext.Trainer.Where(ct => ct.EmailId == email && ct.TrainerId != id).Select(y => y.EmailId).FirstOrDefault();
                        tearm = email;
                    }
                    else
                    {
                        listOut = _dbContext.Trainer.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }
                    if (listOut == null)
                    {
                        listOut = _dbContext.User.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }
                    if (listOut == null)
                    {
                        listOut = _dbContext.Teams.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
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
        /// Function to get user
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/26/2015
        /// </devdoc>
        public static string GetUser(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUser");
                    string listOut = string.Empty;
                    /*if string conmtain ',' then check credentials for another users not for current user else for all*/
                    if (tearm.Contains(','))
                    {
                        string userName = string.Empty;
                        int id = 0;
                        int index = tearm.LastIndexOf(',');
                        userName = tearm.Substring(0, index);
                        id = Convert.ToInt32(tearm.Substring(index + 1));
                        int trainerCred = _dbContext.Credentials.Where(ct => (ct.UserId == id && ct.UserType == Message.UserTypeTrainer)).Select(y => y.Id).FirstOrDefault();
                        listOut = _dbContext.Credentials.Where(ct => ct.EmailId == userName && ct.Id != trainerCred).Select(y => y.EmailId).FirstOrDefault();
                    }
                    else
                    {
                        listOut = _dbContext.Credentials.Where(ct => ct.EmailId == tearm).Select(y => y.EmailId).FirstOrDefault();
                    }

                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUser  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to check team name
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 03/26/2015
        /// </devdoc>
        public static string GetTeam(string tearm)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTeam");
                    string listOut = string.Empty;
                    /*if string conmtain ',' then check teamname for another users not for current user else for all*/
                    if (tearm.Contains(','))
                    {
                        string teamName = string.Empty;
                        int id = 0;
                        int index = tearm.LastIndexOf(',');
                        teamName = tearm.Substring(0, index);
                        id = Convert.ToInt32(tearm.Substring(index + 1));
                        listOut = (from tt in _dbContext.Teams
                                   where tt.TeamId != id && tt.TeamName == ConstantHelper.constTeamNameHashTag + teamName
                                   select tt.TeamName
                                    ).FirstOrDefault();
                    }
                    else
                    {
                        listOut = listOut = (from tt in _dbContext.Teams
                                             where tt.TeamName == ConstantHelper.constTeamNameHashTag + tearm
                                             select tt.TeamName
                                   ).FirstOrDefault();
                    }

                    return listOut;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTeam  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Function to get user count fron the database  
        /// </summary>
        /// <returns>int</returns>
        /// <devdoc>
        /// Developer Name - Raghuraj Singh 
        /// Date - 04/16/2015
        /// </devdoc>
        public static int GetUserCount()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserCount");
                    int userCount = _dbContext.User.Count();
                    return userCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetUserCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get Proram Count
        /// </summary>
        /// <returns></returns>
        public static int GetProgramCount()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetProgramCount");
                    int userCount = _dbContext.Challenge.Count(ch=>ch.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType);
                    return userCount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetProgramCount  end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        #region WebAPIBL

        /// <summary>
        /// Function to register user
        /// </summary>
        /// <returns>string</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        public static async Task<Token> Register(UserInfo model)
        {
            StringBuilder traceLog = null;
            EncryptDecrypt objEncrypt = null;
            Token objToken = new Token();
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        traceLog = new StringBuilder();
                        traceLog.AppendLine("Start: Registration, Insert User in databse");
                        objEncrypt = new EncryptDecrypt();
                        model.FirstName = string.IsNullOrEmpty(model.FirstName) ? string.Empty : model.FirstName.Trim();
                        model.LastName = string.IsNullOrEmpty(model.LastName) ? string.Empty : model.LastName.Trim();
                        var user = new tblUser();
                        user.TeamId = model.PrimaryTrainerId;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.DateOfBirth = model.DateOfBirth;
                        user.Gender = model.Gender;
                        user.ZipCode = model.ZipCode;
                        user.EmailId = model.EmailId;
                        user.Password = objEncrypt.EncryptString(model.Password);
                        user.IsMailReceive = model.IsMailReceive;
                        user.CreatedBy = 1;
                        user.CreatedDate = DateTime.Now;
                        user.ModifiedBy = 1;
                        user.ModifiedDate = DateTime.Now;
                        _dbContext.User.Add(user);
                        _dbContext.SaveChanges();
                        tblCredentials objCredential = new tblCredentials();
                        objCredential.UserId = user.UserId;
                        objCredential.UserType = Message.UserTypeUser;
                        objCredential.Password = user.Password;
                        objCredential.EmailId = string.IsNullOrEmpty(user.EmailId) ? string.Empty : user.EmailId.Trim();
                        _dbContext.Credentials.Add(objCredential);
                        _dbContext.SaveChanges();
                        string andriodDevicesType = DiviceTypeType.Android.ToString();
                        if (!string.IsNullOrWhiteSpace(model.DeviceType) && model.DeviceType == andriodDevicesType)
                        {

                            List<tblUserToken> userTokenlist = _dbContext.UserToken.Where(ut => ut.UserId == objCredential.Id && ut.DeviceUID == model.DeviceUID).ToList();
                            /*delete accepted change by the user*/
                            if (userTokenlist != null)
                            {
                                _dbContext.UserToken.RemoveRange(userTokenlist);
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(model.DeviceID))
                            {
                                // Delete the previous  user token on this devices
                                List<tblUserToken> userTokenlist = _dbContext.UserToken.Where(ut => ut.TokenDevicesID == model.DeviceID).ToList();
                                /*delete accepted change by the user*/
                                if (userTokenlist != null)
                                {
                                    _dbContext.UserToken.RemoveRange(userTokenlist);
                                    _dbContext.SaveChanges();
                                }
                            }
                        }
                        tblUserToken objTokenInfo = new tblUserToken();
                        objTokenInfo.UserId = objCredential.Id;
                        objTokenInfo.Token = CommonUtility.GetToken();
                        objTokenInfo.ClientIPAddress = ConstantHelper.constClientDefaultIP;
                        objTokenInfo.TokenDevicesID = string.IsNullOrEmpty(model.DeviceID) ? string.Empty : model.DeviceID;
                        objTokenInfo.DeviceType = string.IsNullOrEmpty(model.DeviceType) ? string.Empty : model.DeviceType;
                        objTokenInfo.IsExpired = false;
                        objTokenInfo.DeviceUID = model.DeviceUID;
                        objTokenInfo.ExpiredOn = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ExpireDuration"]));
                        _dbContext.UserToken.Add(objTokenInfo);
                        _dbContext.SaveChanges();
                        objToken.TokenId = objTokenInfo.Token;
                        objToken.UserId = user.UserId;
                        objToken.UserType = Message.UserTypeUser;
                        tblTeam team = _dbContext.Teams.FirstOrDefault(t => t.IsDefaultTeam);
                        if (team != null && team.TeamId > 0)
                        {
                            List<int> teamtrainerList = (from tr in _dbContext.Trainer
                                                         join crd in _dbContext.Credentials
                                                         on tr.TrainerId equals crd.UserId
                                                         where crd.UserType == Message.UserTypeTrainer && tr.TeamId == team.TeamId
                                                         select crd.Id).ToList();
                            List<tblFollowings> followingteamtrainers = new List<tblFollowings>();
                            foreach (int teamtrainerID in teamtrainerList)
                            {
                                if (!_dbContext.Followings.Any(f => f.UserId == objCredential.Id && f.FollowUserId == teamtrainerID))
                                {
                                    tblFollowings objFollowing = new tblFollowings();
                                    objFollowing.UserId = objCredential.Id;
                                    objFollowing.FollowUserId = teamtrainerID;
                                    followingteamtrainers.Add(objFollowing);
                                }
                                _dbContext.Followings.AddRange(followingteamtrainers);
                            }
                            if (!_dbContext.TrainerTeamMembers.Any(utm => utm.UserId == objCredential.Id && utm.TeamId == team.TeamId))
                            {
                                tblTrainerTeamMembers objTrainerTeamMember = new tblTrainerTeamMembers();
                                objTrainerTeamMember.TeamId = team.TeamId;
                                objTrainerTeamMember.UserId = objCredential.Id;
                                objTrainerTeamMember.CreatedBy = user.CreatedBy;
                                objTrainerTeamMember.CreatedDate = DateTime.Now;
                                objTrainerTeamMember.ModifiedBy = user.CreatedBy;
                                objTrainerTeamMember.ModifiedDate = DateTime.Now;
                                _dbContext.TrainerTeamMembers.Add(objTrainerTeamMember);
                            }
                            //Update the user team information
                            tblUser objuser = _dbContext.User.FirstOrDefault(usr => usr.UserId == user.UserId);
                            if (objuser != null)
                            {
                                objuser.TeamId = team.TeamId;
                            }
                            _dbContext.SaveChanges();
                        }
                        dbTran.Commit();
                        return objToken;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (objEncrypt != null)
                        {
                            objEncrypt.Dispose();
                        }
                        traceLog.AppendLine("Index  Start end() : --- " + DateTime.Now.ToLongDateString());
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    }
                }
            }
        }

        /// <summary>
        /// Function to Get City State (Address) of the User From ASMX Web Service
        /// </summary>
        /// <returns>string[]</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/25/2015
        /// </devdoc>
        public static async Task<string[]> GetCityAndStateOnZipcode(string zipCode)
        {
            StringBuilder traceLog = null;
            string[] arrCityState = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetCityAndStateOnZipcode");
                using (LinksMediaCorp.ServiceReferenceZipCode.USZipSoapClient objClientZip = new LinksMediaCorp.ServiceReferenceZipCode.USZipSoapClient("USZipSoap"))
                {
                    // GetInfoByZIP
                    XmlNode xmlNode = await objClientZip.GetInfoByZIPAsync(zipCode);
                    arrCityState = new string[2];
                    if (xmlNode.ChildNodes.Count > 0)
                    {
                        arrCityState[0] = xmlNode.ChildNodes.Item(0).ChildNodes.Item(0).InnerText;  //City Field
                        arrCityState[1] = xmlNode.ChildNodes.Item(0).ChildNodes.Item(1).InnerText;  //State Field
                    }
                }
                return arrCityState;
            }
            catch
            {
                return arrCityState;
            }
            finally
            {
                traceLog.AppendLine("End:GetCityAndStateOnZipcode --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            //}
        }
        /// <summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/18/2015
        /// </devdoc>[HttpPost]
        public static async Task<Nullable<bool>> IsZipcodeExist(string zipCode)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: IsZipcodeExist");
                    bool IsExist = true;
                    // Make ZipCode optional
                    if (string.IsNullOrEmpty(zipCode))
                    {
                        return true;
                    }
                    using (LinksMediaCorp.ServiceReferenceZipCode.USZipSoapClient objClientZip = new LinksMediaCorp.ServiceReferenceZipCode.USZipSoapClient("USZipSoap"))
                    {
                        var clientzip = await objClientZip.GetInfoByZIPAsync(zipCode);
                        if (clientzip != null && clientzip.ChildNodes.Count > 0)
                        {
                            IsExist = true;
                        }
                    }
                    return IsExist;
                }
                catch
                {
                    return true;
                }
                finally
                {
                    traceLog.AppendLine("End:IsZipcodeExist --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                }
            }
        }

        /// <summary>
        /// Function to check email id is exist or not in database
        /// </summary>
        /// <returns>bool</returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 02/26/2015
        /// </devdoc>
        public static async Task<bool> IsEmailExist(string email, string userType = null, int userID = 0)
        {
            bool flag = false;
            StringBuilder traceLog = null;
            using (LinksMediaContext _dbContext = new LinksMediaContext())
            {
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: IsEmailExist, check Email in databse");
                    if (string.IsNullOrEmpty(email))
                    {
                        return flag;
                    }
                    if (!string.IsNullOrEmpty(userType) && userID > 0)
                    {

                        if (userType.Equals(Message.UserTypeUser) && await _dbContext.User.AnyAsync(u => u.EmailId.Equals(email) && u.UserId != userID))
                        {
                            flag = true;
                        }
                        else if (userType.Equals(Message.UserTypeTrainer) && await _dbContext.Trainer.AnyAsync(t => t.EmailId.Equals(email) && t.TrainerId != userID))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        if (await _dbContext.User.AnyAsync(u => u.EmailId.Equals(email)) || await _dbContext.Trainer.AnyAsync(t => t.EmailId.Equals(email)))
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
                    traceLog.AppendLine("End:IsEmailExist --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }


        /// <summary>
        ///  Get User Assignments List by Trainers
        /// </summary>
        /// <returns></returns>
        public static Total<List<UserAssignmentByTrainerVM>> GetUserAssignmentsList(int startIndex, int endIndex)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<UserAssignmentByTrainerVM> listPendingChallenge = null;
                try
                {
                    traceLog.AppendLine("Start: GetUserAssignmentsList---- " + DateTime.Now.ToLongDateString());
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    listPendingChallenge = (from c in dataContext.Challenge
                                            join ctf in dataContext.UserAssignments on c.ChallengeId equals ctf.ChallengeId
                                            join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                            where c.IsActive && ctf.TargetId == cred.Id
                                            && !ctf.IsCompleted && ctf.IsActive
                                            orderby ctf.ChallengeDate descending
                                            select new UserAssignmentByTrainerVM
                                            {
                                                ChallengeId = c.ChallengeId,
                                                ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                ChallengeName = c.ChallengeName,
                                                ChallengeType = ct.ChallengeType,
                                                DifficultyLevel = c.DifficultyLevel,
                                                ChallengeDuration = c.FFChallengeDuration,
                                                PersonalMessage = ctf.PersonalMessage,
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
                                                ChallengeByUserName = ctf.ChallengeByUserName,
                                                SubjectId = ctf.SubjectId,
                                                DbPostedDate = ctf.ChallengeDate,
                                                Description = c.Description,
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

                    listPendingChallenge = listPendingChallenge.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                          .Select(grp => grp.FirstOrDefault())
                                                                          .ToList();

                    listPendingChallenge.ForEach(r =>
                    {
                        if (r.ExeciseVideoDetails != null)
                        {

                            if (string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseUrl))
                            {
                                r.ExeciseVideoLink = !string.IsNullOrEmpty(r.ExeciseVideoDetails.ExeciseName) ? CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + r.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
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
                                r.ExeciseThumbnailUrl = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
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
                        if (r.ChallengeByUserType == Message.UserTypeUser)
                        {
                            var userdetails = (from crd in dataContext.Credentials
                                               join usr in dataContext.User
                                               on crd.UserId equals usr.UserId
                                               where crd.UserType == Message.UserTypeUser && crd.Id == r.SubjectId
                                               select usr).FirstOrDefault();
                            if (userdetails != null)
                            {
                                r.ChallengedUserImageUrl = string.IsNullOrEmpty(userdetails.UserImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + userdetails.UserImageUrl;
                                r.ChallengeByUserName = userdetails.FirstName + " " + userdetails.LastName;
                                r.ChallengeByUserId = userdetails.UserId;
                            }
                        }
                        else if (r.ChallengeByUserType == Message.UserTypeTrainer)
                        {
                            var trainerdetails = (from crd in dataContext.Credentials
                                                  join usr in dataContext.Trainer
                                                  on crd.UserId equals usr.TrainerId
                                                  where crd.UserType == Message.UserTypeTrainer && crd.Id == r.SubjectId
                                                  select usr).FirstOrDefault();
                            if (trainerdetails != null)
                            {
                                r.ChallengedUserImageUrl = string.IsNullOrEmpty(trainerdetails.TrainerImageUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ProfilePicDirectory + trainerdetails.TrainerImageUrl;
                                r.ChallengeByUserName = trainerdetails.FirstName + " " + trainerdetails.LastName;
                                r.ChallengeByUserId = trainerdetails.TrainerId;
                            }
                        }
                        if (!string.IsNullOrEmpty(r.ProgramImageUrl))
                        {
                            string filePath = HttpContext.Current.Server.MapPath("~") + "\\images\\profilepic\\" + r.ProgramImageUrl;
                            r.ProgramImageUrl = (string.IsNullOrEmpty(r.ProgramImageUrl)) ? string.Empty : File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ProfilePicDirectory + r.ProgramImageUrl)) ? CommonUtility.VirtualPath + Message.ProfilePicDirectory + r.ProgramImageUrl : string.Empty;

                            if (System.IO.File.Exists(filePath))
                            {
                                using (Bitmap objBitmap = new Bitmap(filePath))
                                {
                                    double sourceWidth = Convert.ToDouble(objBitmap.Size.Width, CultureInfo.CurrentCulture);
                                    double sourceHeight = Convert.ToDouble(objBitmap.Size.Height, CultureInfo.CurrentCulture);
                                    r.Height = (sourceWidth > 0) ? Convert.ToString(sourceHeight, CultureInfo.CurrentCulture) : string.Empty;
                                    r.Width = (sourceWidth > 0) ? Convert.ToString(sourceWidth, CultureInfo.CurrentCulture) : string.Empty;
                                }
                            }
                            else
                            {
                                r.Height = string.Empty;
                                r.Width = string.Empty;
                            }
                        }
                        else
                        {
                            r.Height = string.Empty;
                            r.Width = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(r.ChallengeType))
                        {
                            r.ChallengeType = r.ChallengeType.Split(' ')[0];
                        }
                        r.Equipment = (r.TempEquipments != null && r.TempEquipments.Count > 0) ? string.Join(", ", r.TempEquipments) : string.Empty;
                        r.TempEquipments = null;
                        r.TargetZone = (r.TempTargetZone != null && r.TempTargetZone.Count > 0) ? string.Join(", ", r.TempTargetZone) : string.Empty;
                        r.TempTargetZone = null;
                        r.IsWellness = r.ChallengeSubTypeId == ConstantHelper.constWellnessChallengeSubType;
                        if (r.ChallengeSubTypeId == ConstantHelper.constProgramChallengeSubType)
                        {
                            r.IsActiveProgram = dataContext.UserActivePrograms.Any(ap => ap.ProgramId == r.ChallengeId && ap.UserCredId == cred.Id && !ap.IsCompleted);
                        }
                    });
                    Total<List<UserAssignmentByTrainerVM>> objresult = new Total<List<UserAssignmentByTrainerVM>>();
                    objresult.TotalList = (from l in listPendingChallenge
                                           orderby l.DbPostedDate descending
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
                    listPendingChallenge = null;
                }
            }
        }
        /// <summary>
        /// Get User Activity 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static Total<List<RecentResultVM>> GetUserActivity(int userId, string userType, int startIndex, int endIndex)
        {
            StringBuilder traceLog = null;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                List<RecentResultVM> objList = null;
                List<RecentResultVM> resultQuery = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start: Trainer GetUserActivity()");
                    int userCredID = 0;
                    Credentials cred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    UserCredVM userdetails = (from crd in dataContext.Credentials
                                              join usr in dataContext.User
                                              on crd.UserId equals usr.UserId
                                              where crd.UserType == userType && crd.UserId == userId
                                              select new UserCredVM
                                              {
                                                  Id = crd.Id,
                                                  TeamId = usr.TeamId,
                                              }).FirstOrDefault();
                    if (userdetails != null)
                    {
                        userCredID = userdetails.Id;
                    }
                    Total<List<RecentResultVM>> objresult = new Total<List<RecentResultVM>>();
                    if (userCredID > 0)
                    {
                        objList = (from m in dataContext.MessageStraems
                                   join c in dataContext.Credentials on m.SubjectId equals c.Id
                                   where m.IsImageVideo && m.IsNewsFeedHide != true
                                   && (!(string.IsNullOrEmpty(m.Content)) || m.IsImageVideo)
                                   && ((m.SubjectId == userCredID && m.TargetType == Message.TeamTargetType) || (m.SubjectId == userCredID && m.TargetId  == m.SubjectId && m.TargetType == Message.UserTargetType))                              

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
                                       IsLoginUserBoom = dataContext.Booms.Where(bm => bm.MessageStraemId == m.MessageStraemId).Any(b => b.BoomedBy == cred.Id),
                                       IsLoginUserComment = dataContext.Comments.Where(cm => cm.MessageStraemId == m.MessageStraemId).Any(b => b.CommentedBy == cred.Id),
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
                                // pic.PicsUrl = CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl;
                                pic.PicsUrl = (!string.IsNullOrEmpty(pic.PicsUrl) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + Message.ResultPicDirectory + pic.PicsUrl))) ? CommonUtility.VirtualPath + Message.ResultPicDirectory + pic.PicsUrl : string.Empty;

                            });
                            string thumnailHeight, thumnailWidth;
                            //Code For Getting Posted Videos
                            item.VideoList.ForEach(vid =>
                            {
                                string thumnailFileName = vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                vid.ThumbNailUrl = string.IsNullOrEmpty(vid.VideoUrl) ? string.Empty : CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl.Split('.')[0] + Message.JpgImageExtension;
                                // vid.VideoUrl = CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl;
                                vid.VideoUrl = !string.IsNullOrEmpty(vid.VideoUrl) ? CommonUtility.VirtualPath + Message.ResultVideoDirectory + vid.VideoUrl : string.Empty;
                                thumnailHeight = string.Empty;
                                thumnailWidth = string.Empty;
                                CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                vid.ThumbNailHeight = thumnailHeight;
                                vid.ThumbNailWidth = thumnailWidth;
                            });
                        }
                        //Query to get completed challenge list
                        List<RecentResultVM> resultQueryData = (from uc in dataContext.UserChallenge
                                                                join c in dataContext.Challenge on uc.ChallengeId equals c.ChallengeId
                                                                join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                orderby uc.AcceptedDate descending
                                                                where uc.UserId == userCredID && c.IsActive
                                                                select new RecentResultVM
                                                                {
                                                                    ResultId = uc.Id,
                                                                    Description = c.Description,
                                                                    ChallengeId = uc.ChallengeId,
                                                                    ChallengeType = ct.ChallengeType,
                                                                    ChallengeName = c.ChallengeName,
                                                                    DifficultyLevel = c.DifficultyLevel,
                                                                    Duration = c.FFChallengeDuration,
                                                                    ChallengeSubTypeid = c.ChallengeSubTypeId,
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
                                                                                               ChallengeExeciseId= cexe.RocordId
                                                                                           }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault(),
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
                                                                    UserCredID = uc.UserId,
                                                                    PostType = ConstantHelper.ResultFeed,
                                                                    DbPostedDate = uc.AcceptedDate,
                                                                    BoomsCount = dataContext.ResultBooms.Count(b => b.ResultId == uc.Id),
                                                                    CommentsCount = dataContext.ResultComments.Count(cmnt => cmnt.Id == uc.Id),
                                                                    UserType = dataContext.Credentials.Where(ucrd => ucrd.Id == uc.UserId).FirstOrDefault().UserType,
                                                                    IsLoginUserBoom = dataContext.ResultBooms.Where(bm => bm.ResultId == uc.Id).Any(b => b.BoomedBy == cred.Id),
                                                                    IsLoginUserComment = dataContext.ResultComments.Where(cm => cm.Id == uc.Id).Any(b => b.CommentedBy == cred.Id),
                                                                }).ToList();

                        resultQuery = resultQueryData;
                        if (resultQuery != null)
                        {
                            resultQuery = resultQuery.GroupBy(cr => cr.ResultId).Select(usr => usr.FirstOrDefault()).ToList();
                            string thumnailHeight, thumnailWidth;
                            resultQuery.ForEach(ch =>
                            {
                                if (!string.IsNullOrEmpty(ch.Description))
                                {
                                    ch.Description = ch.Description.Replace(ConstantHelper.constBreakLine, ConstantHelper.constHTMLBreakLineTerninator);
                                    const string HTML_TAG_PATTERN = ConstantHelper.constRemoveHTMLTag;
                                    ch.Description = Regex.Replace(ch.Description, HTML_TAG_PATTERN, string.Empty).Replace(ConstantHelper.constBlankSpace, string.Empty);
                                }
                                if (ch.ExeciseVideoDetails != null)
                                {
                                    if (string.IsNullOrEmpty(ch.ExeciseVideoDetails.ExeciseUrl))
                                    {
                                        ch.ExeciseVideoLink = !string.IsNullOrEmpty(ch.ExeciseVideoDetails.ExeciseName) ? CommonUtility.VirtualFitComExercisePath + Message.ExerciseVideoDirectory + ch.ExeciseVideoDetails.ExeciseName + ConstantHelper.constExeciseVideoExtension : string.Empty;
                                    }
                                    else
                                    {
                                        ch.ExeciseVideoLink = ch.ExeciseVideoDetails.ExeciseUrl;
                                    }
                                    if (!string.IsNullOrEmpty(ch.ExeciseVideoDetails.ExeciseName) && string.IsNullOrEmpty(ch.ExeciseVideoDetails.ExerciseThumnail))
                                    {
                                        string thumnailName = ch.ExeciseVideoDetails.ExeciseName.Replace(" ", string.Empty);
                                        string thumnailFileName = thumnailName + Message.JpgImageExtension;
                                        thumnailHeight = string.Empty;
                                        thumnailWidth = string.Empty;
                                        ch.ThumbnailUrl = CommonUtility.VirtualFitComExercisePath + Message.ExerciseThumbnailDirectory + thumnailName + Message.JpgImageExtension;
                                        CommonWebApiBL.GetThumNailDemension(thumnailFileName, out thumnailHeight, out thumnailWidth);
                                        ch.ThumbNailHeight = thumnailHeight;
                                        ch.ThumbNailWidth = thumnailWidth;
                                    }
                                    else
                                    {
                                        ch.ThumbNailHeight = string.Empty;
                                        ch.ThumbNailWidth = string.Empty;
                                        ch.ThumbnailUrl = ch.ExeciseVideoDetails.ExerciseThumnail;
                                    }
                                }
                                if (ch.TempTargetZone != null && ch.TempTargetZone.Count > 0)
                                {
                                    ch.TargetZone = string.Join(", ", ch.TempTargetZone);
                                }
                                ch.TempTargetZone = null;
                            });
                        }
                        if (resultQuery != null && objList != null)
                        {
                            resultQuery = resultQuery.Union(objList).ToList();
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
                                objresult.IsMoreAvailable = true;
                            }
                            resultQuery.ForEach(r =>
                            {
                                r.isWellness = r.ChallengeSubTypeid == ConstantHelper.constWellnessChallengeSubType;
                                // Find the user details based on user credId and user Type
                                ProfileDetails userdetail = ProfileApiBL.GetProfileDetailsByCredId(r.UserCredID, r.UserType);
                                if (userdetail != null)
                                {
                                    r.UserName = userdetail.UserName;
                                    r.UserImageUrl = userdetail.ProfileImageUrl;
                                    r.userID = userdetail.UserId;
                                }
                                r.DbPostedDate = r.DbPostedDate.ToUniversalTime();
                                string resultMethod = string.Empty;
                                if (!string.IsNullOrEmpty(r.ResultUnit) && r.PostType == ConstantHelper.ResultFeed)
                                {
                                    resultMethod = r.ResultUnit.Trim();
                                    // Find all result for latest  result submit  result
                                    PersonalChallengeVM personalbestresult = TeamBL.GetGlobalPersonalBestResult(r.ChallengeId, r.UserCredID, dataContext);
                                    switch (resultMethod)
                                    {
                                        case ConstantHelper.constTime:
                                            if (!string.IsNullOrEmpty(r.Result))
                                            {
                                                r.Result = r.Result.Trim();
                                            }
                                            r.TempOrderIntValue = Convert.ToInt32(r.Result.Replace(ConstantHelper.constColon, string.Empty).Replace(ConstantHelper.constDot, string.Empty));
                                            //Code for HH:MM:SS And MM:SS format
                                            string tempResult = string.Empty;
                                            tempResult = r.Result;
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
                                                r.TempOrderIntValue = (float)(Convert.ToDecimal(r.Result) + (arrString.Count() == 2 ? ((decimal)(Convert.ToDecimal(arrString[0]) / Convert.ToDecimal(arrString[1]))) : 0));
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
                            objresult.TotalList = resultQuery;
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
                    traceLog.AppendLine("End : GetUserActivity  : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                    traceLog = null;
                    objList = null;
                    resultQuery = null;

                }
            }
        }
        /// <summary>
        /// Get User MyAssignment Count
        /// </summary>
        /// <returns></returns>
        public static int GetUserMyAssignmentCount(string userType, int userCredId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                int totalAssigmentcount = 0;
                try
                {
                    traceLog.AppendLine("Start: GetProgramDetail---- " + DateTime.Now.ToLongDateString());
                    List<MyTrainerAssignenmentRequest> myassignmentlist = new List<MyTrainerAssignenmentRequest>();
                    if (!string.IsNullOrEmpty(userType) && userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        myassignmentlist = (from c in dataContext.Challenge
                                            join usrassgn in dataContext.UserAssignments
                                            on c.ChallengeId equals usrassgn.ChallengeId
                                            join crd in dataContext.Credentials
                                            on usrassgn.SubjectId equals crd.Id
                                            where c.IsActive
                                            && !usrassgn.IsCompleted
                                            && usrassgn.IsActive
                                            && usrassgn.SubjectId == userCredId
                                            && crd.UserType == Message.UserTypeTrainer
                                            select new MyTrainerAssignenmentRequest
                                            {
                                                ChallengeId = c.ChallengeId,
                                                UserType = crd.UserType,
                                                SubjectId = usrassgn.SubjectId
                                            }).ToList();

                    }
                    else
                    {
                        myassignmentlist = (from c in dataContext.Challenge
                                            join usrassgn in dataContext.UserAssignments
                                            on c.ChallengeId equals usrassgn.ChallengeId
                                            join crd in dataContext.Credentials
                                            on usrassgn.SubjectId equals crd.Id
                                            where c.IsActive
                                            && usrassgn.IsActive
                                            && !(usrassgn.IsCompleted)
                                            && usrassgn.TargetId == userCredId
                                            && crd.UserType == Message.UserTypeTrainer
                                            select new MyTrainerAssignenmentRequest
                                            {
                                                ChallengeId = c.ChallengeId,
                                                UserType = crd.UserType,
                                                SubjectId = usrassgn.SubjectId
                                            }).ToList();
                    }
                    if (myassignmentlist != null)
                    {

                        myassignmentlist = myassignmentlist.GroupBy(pl => new { pl.ChallengeId, pl.SubjectId })
                                                                              .Select(grp => grp.FirstOrDefault())
                                                                              .ToList();
                        totalAssigmentcount = myassignmentlist.Count;
                    }
                    return totalAssigmentcount;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetProgramDetail : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Add / Update Use rOnboarding Info based on current login user
        /// </summary>
        /// <param name="onboradingUserInfo"></param>
        /// <returns></returns>
        public static bool UpdateUserOnboardingInfo(UserOnBoradingInfo onboradingUserInfo)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: onboradingUserInfo---- " + DateTime.Now.ToLongDateString());
                    if (onboradingUserInfo != null)
                    {
                        Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                        if (objCred != null)
                        {
                            tblUser userInfo = dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                            if (userInfo != null)
                            {
                                userInfo.Weight = onboradingUserInfo.Weight;
                                userInfo.Height = onboradingUserInfo.Height;
                                if (!string.IsNullOrEmpty(onboradingUserInfo.DateOfBirth))
                                {
                                    userInfo.DateOfBirth = DateTime.ParseExact(onboradingUserInfo.DateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    userInfo.DateOfBirth = null;
                                }
                                userInfo.Gender = onboradingUserInfo.Gender.ToString();
                                userInfo.FitnessLevel = string.IsNullOrEmpty(onboradingUserInfo.FitnessLevel.ToString()) ? userInfo.FitnessLevel : onboradingUserInfo.FitnessLevel.ToString();
                                dataContext.SaveChanges();
                                // Push Fitness test and Program in Pending Queue of login user; 
                                AddPendingListForUser(objCred.Id, userInfo.TeamId, userInfo.FitnessLevel);
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
                    traceLog.AppendLine("End  onboradingUserInfo : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Save user Pending based on joined team
        /// </summary>
        /// <param name="userCredId"></param>
        /// <param name="userJoinedTeamId"></param>
        /// <param name="userFittnessLevel"></param>
        public static void AddPendingListForUser(int userCredId, int userJoinedTeamId, string userFittnessLevel)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetUserOnboardingInfo---- " + DateTime.Now.ToLongDateString());
                    tblTeam teamInfo = dataContext.Teams.FirstOrDefault(usr => usr.TeamId == userJoinedTeamId);
                    if (teamInfo != null)
                    {
                        int teamprimaryTrainerCrdId = 0;
                        int trainerId = 0;
                        UserCredVM teamprimaryTrainerDetails = (from tr in dataContext.TrainerTeamMembers
                                                                join crd in dataContext.Credentials
                                                                on tr.UserId equals crd.Id
                                                                where crd.UserType == Message.UserTypeTrainer && tr.TeamId == userJoinedTeamId
                                                                orderby tr.RecordId ascending
                                                                select new UserCredVM
                                                                {
                                                                    Id = crd.Id,
                                                                    UserId = crd.UserId
                                                                }).FirstOrDefault();
                        // If Team does have Primary trainer then find on behalf of Default team trainer
                        if (teamprimaryTrainerDetails == null)
                        {
                            teamprimaryTrainerDetails = (from tr in dataContext.TrainerTeamMembers
                                                         join tm in dataContext.Teams
                                                         on tr.TeamId equals tm.TeamId
                                                         join crd in dataContext.Credentials
                                                         on tr.UserId equals crd.Id
                                                         where crd.UserType == Message.UserTypeTrainer && tm.IsDefaultTeam
                                                         orderby tr.RecordId ascending
                                                         select new UserCredVM
                                                         {
                                                             Id = crd.Id,
                                                             UserId = crd.UserId
                                                         }).FirstOrDefault();
                        }
                        if (teamprimaryTrainerDetails != null)
                        {
                            teamprimaryTrainerCrdId = teamprimaryTrainerDetails.Id;
                            trainerId = teamprimaryTrainerDetails.UserId;
                        }
                        FitnessLevel savedfitnessLevel;
                        if (!Enum.TryParse(userFittnessLevel, true, out savedfitnessLevel))
                        {
                            savedfitnessLevel = FitnessLevel.Beginner;
                        }
                        switch (savedfitnessLevel)
                        {
                            case FitnessLevel.Beginner:
                                {
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.BeginnerProgramId, true);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.FitcomtestChallengeId1, false);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.FitcomtestChallengeId2, false);

                                }
                                break;
                            case FitnessLevel.Intermediate:
                            case FitnessLevel.Advanced:
                                {
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.AdvIntProgramId1, true);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.AdvIntProgramId2, true);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.AdvIntProgramId3, true);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.FitcomtestChallengeId1, false);
                                    SavePendingAndSendNotification(teamprimaryTrainerCrdId, trainerId, userCredId, teamInfo.FitcomtestChallengeId2, false);
                                }
                                break;
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  onboradingUserInfo : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Save Pending And Send Notification to user
        /// </summary>
        /// <param name="teamprimaryTrainerCrdId"></param>
        /// <param name="userCredId"></param>
        /// <param name="challengeId"></param>
        /// <param name="isProgram"></param>
        public static void SavePendingAndSendNotification(int teamprimaryTrainerCrdId, int trainerId, int userCredId, int challengeId, bool isProgram)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: SavePendingAndSendNotification---- " + DateTime.Now.ToLongDateString());
                    if (challengeId > 0)
                    {
                        ChallengeUserDetails challengeBy = (from trainer in dataContext.Trainer
                                                            join crd in dataContext.Credentials on trainer.TrainerId equals crd.UserId
                                                            join chlng in dataContext.UserChallenge on crd.Id equals chlng.UserId into temp
                                                            from usertemp in temp.DefaultIfEmpty()
                                                            where crd.Id == teamprimaryTrainerCrdId
                                                            && crd.UserType == Message.UserTypeTrainer
                                                            && usertemp.ChallengeId == challengeId
                                                            orderby usertemp.Id descending
                                                            select new ChallengeUserDetails
                                                            {
                                                                ChallengeByUserName = trainer.FirstName + " " + trainer.LastName,
                                                                Result = usertemp == null ? string.Empty : usertemp.Result,
                                                                Fraction = usertemp == null ? string.Empty : usertemp.Fraction,
                                                                ResultUnitSuffix = usertemp == null ? string.Empty : usertemp.ResultUnit,
                                                                UserChallengeId = usertemp == null ? 0 : usertemp.Id
                                                            }).FirstOrDefault();
                        string ChallengeByUserName = string.Empty;
                        ChallengeByUserName = challengeBy != null ? challengeBy.ChallengeByUserName : string.Empty;
                        if (string.IsNullOrEmpty(ChallengeByUserName))
                        {
                            ChallengeByUserName = dataContext.Trainer.Where(trs => trs.TrainerId == trainerId).Select(tr => tr.FirstName + " " + tr.LastName).FirstOrDefault();
                        }
                        // Add user Assignment List
                        tblUserAssignmentByTrainer objtblUserAssignmentByTrainer = new tblUserAssignmentByTrainer();
                        objtblUserAssignmentByTrainer.ChallengeId = challengeId;
                        objtblUserAssignmentByTrainer.SubjectId = teamprimaryTrainerCrdId;
                        objtblUserAssignmentByTrainer.TargetId = userCredId;
                        objtblUserAssignmentByTrainer.IsCompleted = false;
                        objtblUserAssignmentByTrainer.IsRead = false;
                        objtblUserAssignmentByTrainer.Result = challengeBy == null ? string.Empty : string.IsNullOrEmpty(challengeBy.Result) ? string.Empty : challengeBy.Result.Replace(",", string.Empty);
                        objtblUserAssignmentByTrainer.Fraction = (challengeBy == null) ? string.Empty : challengeBy.Fraction;
                        objtblUserAssignmentByTrainer.ResultUnitSuffix = (challengeBy == null) ? string.Empty : challengeBy.ResultUnitSuffix;
                        objtblUserAssignmentByTrainer.ChallengeDate = DateTime.Now;
                        objtblUserAssignmentByTrainer.ChallengeByUserName = ChallengeByUserName;
                        objtblUserAssignmentByTrainer.IsProgram = isProgram;
                        objtblUserAssignmentByTrainer.UserChallengeId = (challengeBy == null) ? 0 : challengeBy.UserChallengeId;
                        objtblUserAssignmentByTrainer.IsOnBoarding = true;
                        objtblUserAssignmentByTrainer.IsActive = true;
                        dataContext.UserAssignments.Add(objtblUserAssignmentByTrainer);
                        dataContext.SaveChanges();
                        byte[] certificate = File.ReadAllBytes(CommonUtility.GetIOSCertificatePath);                        
                        int targetchallenegID = challengeId;
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = false;
                            ChallengeToFriendBL.SendChallegesNotificationToUser(userCredId, ChallengeByUserName, trainerId, Message.UserTypeTrainer, certificate, targetchallenegID, objtblUserAssignmentByTrainer.UserAssignmentId, true, false);

                        }).Start();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  SavePendingAndSendNotification : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get User On boarding Info
        /// </summary>
        /// <returns></returns>
        public static UserOnBoradingInfo GetUserOnboardingInfo()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                UserOnBoradingInfo userInfodata = new UserOnBoradingInfo();
                try
                {
                    traceLog.AppendLine("Start: GetUserOnboardingInfo---- " + DateTime.Now.ToLongDateString());
                    Credentials objCred = CommonWebApiBL.GetUserId(Thread.CurrentPrincipal.Identity.Name);
                    if (objCred != null)
                    {
                        tblUser userInfo = dataContext.User.FirstOrDefault(usr => usr.UserId == objCred.UserId);
                        if (userInfo != null)
                        {
                            userInfodata.Weight = string.IsNullOrEmpty(userInfo.Weight) ? string.Empty : userInfo.Weight;
                            userInfodata.Height = string.IsNullOrEmpty(userInfo.Height) ? string.Empty : userInfo.Height;
                            userInfodata.DateOfBirth = userInfo.DateOfBirth.HasValue ? Convert.ToString(userInfo.DateOfBirth.Value.ToString("MM/dd/yyyy"), CultureInfo.InvariantCulture) : string.Empty;
                            Gender savedGender;
                            if (!Enum.TryParse(userInfo.Gender, true, out savedGender))
                            {
                                userInfodata.Gender = Gender.Male;
                            }
                            else
                            {
                                userInfodata.Gender = savedGender;
                            }
                            FitnessLevel savedfitnessLevel;
                            if (!Enum.TryParse(userInfo.FitnessLevel, true, out savedfitnessLevel))
                            {
                                userInfodata.FitnessLevel = FitnessLevel.Beginner;
                            }
                            else
                            {
                                userInfodata.FitnessLevel = savedfitnessLevel;
                            }
                            var OnboardingExeciseVideoInfo = (from tm in dataContext.Teams
                                                              join exe in dataContext.Exercise
                                                              on tm.OnboardingExeciseVideoId equals exe.ExerciseId
                                                              where exe.IsActive && tm.TeamId == userInfo.TeamId
                                                              select new
                                                              {
                                                                  exe.V720pUrl,
                                                                  exe.ThumnailUrl
                                                              }).FirstOrDefault();
                            if (OnboardingExeciseVideoInfo != null)
                            {
                                userInfodata.OnboardingVideoThumnailUrl = string.IsNullOrEmpty(OnboardingExeciseVideoInfo.ThumnailUrl) ? string.Empty : OnboardingExeciseVideoInfo.ThumnailUrl;
                                userInfodata.OnboardingVideoUrl = string.IsNullOrEmpty(OnboardingExeciseVideoInfo.V720pUrl) ? string.Empty : OnboardingExeciseVideoInfo.V720pUrl;
                            }

                        }
                    }
                    return userInfodata;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  onboradingUserInfo : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        #endregion
    }
}