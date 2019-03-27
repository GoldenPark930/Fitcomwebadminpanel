
namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Linq;
    using System.Text;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Collections.Generic;
    using LinksMediaCorpDataAccessLayer;
    public class CommonReportingUtility
    {
        /// <summary>
        /// validate the challenge variable value and refactor by Irshad
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ValidateChallengeTimeVariableValue(CreateChallengeVM objChallenge)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ValidateVariableValueOnCreateChallenge()");
                if (objChallenge.VariableUnit != null && objChallenge.VariableUnit.Equals(Message.VariableUnitminutes, StringComparison.OrdinalIgnoreCase))
                {
                    if (objChallenge.VariableValue != null && objChallenge.VariableValue.Contains("."))
                    {
                        string[] strMinutesVariables = objChallenge.VariableValue.Split('.').ToArray();
                        if (strMinutesVariables != null && strMinutesVariables.Count() > 0 && strMinutesVariables[0].Contains(":"))
                        {
                            string[] tempMinutesVariables = strMinutesVariables[0].Split(':').ToArray();
                            if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                            {
                                objChallenge.VariableHours = (string.IsNullOrEmpty(tempMinutesVariables[0]) || tempMinutesVariables[0] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[0];
                                objChallenge.VariableMinute = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                                objChallenge.VariableSecond = (string.IsNullOrEmpty(tempMinutesVariables[2]) || tempMinutesVariables[2] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[2];
                            }
                        }
                        if (strMinutesVariables != null && strMinutesVariables.Count() > 0)
                        {
                            objChallenge.VariableMS = (string.IsNullOrEmpty(strMinutesVariables[1]) || strMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : strMinutesVariables[1];
                        }

                    }
                    else if (objChallenge.VariableValue != null && objChallenge.VariableValue.Contains(":"))
                    {


                        string[] tempMinutesVariables = objChallenge.VariableValue.Split(':').ToArray();
                        if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                        {
                            objChallenge.VariableHours = (string.IsNullOrEmpty(tempMinutesVariables[0]) || tempMinutesVariables[0] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[0];
                            objChallenge.VariableMinute = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                            objChallenge.VariableSecond = (string.IsNullOrEmpty(tempMinutesVariables[2]) || tempMinutesVariables[2] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[2];

                        }
                        if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                        {
                            objChallenge.VariableMS = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                        }

                    }
                    else
                    {
                        // Set default time format
                        objChallenge.VariableHours = ConstantHelper.constTimeVariableUnit;
                        objChallenge.VariableMinute = ConstantHelper.constTimeVariableUnit;
                        objChallenge.VariableSecond = ConstantHelper.constTimeVariableUnit;
                        objChallenge.VariableMS = ConstantHelper.constTimeVariableUnit;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: ValidateVariableValueOnCreateChallenge() --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }

        }
        /// <summary>
        /// Validate the challenge Global Variables
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ValidateChallengeGlobalVariableValue(CreateChallengeVM objChallenge)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ValidateChallengeGlobalVariableValue()");
                if (objChallenge.GlobalResultFilterValue != null && objChallenge.GlobalResultFilterValue.Contains(":"))
                {
                    if (objChallenge.GlobalResultFilterValue != null && objChallenge.GlobalResultFilterValue.Contains("."))
                    {
                        string[] globalresultvalue = objChallenge.GlobalResultFilterValue.Split('.').ToArray();
                        string[] tempMinutesVariables = globalresultvalue[0].Split(':').ToArray();
                        if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                        {
                            objChallenge.GlobalResultFilterHours = (string.IsNullOrEmpty(tempMinutesVariables[0]) || tempMinutesVariables[0] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[0];
                            objChallenge.GlobalResultFilterMinute = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                            objChallenge.GlobalResultFilterSecond = (string.IsNullOrEmpty(tempMinutesVariables[2]) || tempMinutesVariables[2] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[2];

                        }

                        if (globalresultvalue != null && globalresultvalue.Count() > 0)
                        {
                            objChallenge.GlobalResultFilterMS = (string.IsNullOrEmpty(globalresultvalue[1]) || globalresultvalue[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : globalresultvalue[1];
                        }

                    }
                    objChallenge.GlobalResultFilterValue = string.Empty;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: ValidateChallengeGlobalVariableValue() --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }

        }
        /// <summary>
        /// Validate Challenge Time Result VariableValue and refactor by Irshad
        /// </summary>
        /// <param name="objChallenge"></param>
        /// <param name="userResult"></param>
        public static void ValidateChallengeResultTimeVariableValue(CreateChallengeVM objChallenge, string userResult)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: ValidateChallengeResultTimeVariableValue()-with userResult-" + userResult);
                // objChallenge.ResultTime = userResult.Result;
                if (!string.IsNullOrEmpty(userResult) && userResult.Contains("."))
                {
                    string[] strMinutesVariables = userResult.Split('.').ToArray();
                    if (strMinutesVariables != null && strMinutesVariables.Count() > 0 && strMinutesVariables[0].Contains(":"))
                    {
                        string[] tempMinutesVariables = strMinutesVariables[0].Split(':').ToArray();
                        if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                        {
                            objChallenge.ResultVariableHours = (string.IsNullOrEmpty(tempMinutesVariables[0]) || tempMinutesVariables[0] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[0];
                            objChallenge.ResultVariableMinute = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                            objChallenge.ResultVariableSecond = (string.IsNullOrEmpty(tempMinutesVariables[2]) || tempMinutesVariables[2] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[2];
                        }
                    }
                    if (strMinutesVariables != null && strMinutesVariables.Count() > 0)
                    {
                        objChallenge.ResultVariableMS = (string.IsNullOrEmpty(strMinutesVariables[1]) || strMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : strMinutesVariables[1];
                    }

                }
                else if (!string.IsNullOrEmpty(userResult) && userResult.Contains(":"))
                {
                    string[] tempMinutesVariables = userResult.Split(':').ToArray();
                    if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                    {
                        objChallenge.ResultVariableHours = (string.IsNullOrEmpty(tempMinutesVariables[0]) || tempMinutesVariables[0] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[0];
                        objChallenge.ResultVariableMinute = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                        objChallenge.ResultVariableSecond = (string.IsNullOrEmpty(tempMinutesVariables[2]) || tempMinutesVariables[2] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[2];

                    }
                    if (tempMinutesVariables != null && tempMinutesVariables.Count() > 0)
                    {
                        objChallenge.ResultVariableMS = (string.IsNullOrEmpty(tempMinutesVariables[1]) || tempMinutesVariables[1] == ConstantHelper.constTimeVariableUnit) ? ConstantHelper.constTimeVariableUnit : tempMinutesVariables[1];
                    }
                }
                else
                {
                    objChallenge.ResultVariableHours = ConstantHelper.constTimeVariableUnit;
                    objChallenge.ResultVariableMinute = ConstantHelper.constTimeVariableUnit;
                    objChallenge.ResultVariableSecond = ConstantHelper.constTimeVariableUnit;
                    objChallenge.ResultVariableMS = ConstantHelper.constTimeVariableUnit;
                }


            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End: ValidateChallengeResultTimeVariableValue() --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }

        }
        /// <summary>
        /// Rest the Time and Result Vaiable and refactor by Irshad
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ResetChallengeTimeResultVariable(CreateChallengeVM objChallenge)
        {
            if (objChallenge != null)
            {
                // Set default time format
                objChallenge.VariableHours = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableMinute = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableSecond = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableMS = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableHours = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableMinute = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableSecond = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableMS = ConstantHelper.constTimeVariableUnit;
            }
        }
        /// <summary>
        /// Rest the Time  Vaiable and refactor by Irshad
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ResetChallengeTimeVariable(CreateChallengeVM objChallenge)
        {
            // Set default time format
            if (objChallenge != null)
            {
                objChallenge.VariableHours = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableMinute = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableSecond = ConstantHelper.constTimeVariableUnit;
                objChallenge.VariableMS = ConstantHelper.constTimeVariableUnit;
            }
        }
        /// <summary>
        /// Rest the Result Vaiable and refactor by Irshad
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ResetChallengeResultVariable(CreateChallengeVM objChallenge)
        {
            // Set default time format
            if (objChallenge != null)
            {
                objChallenge.ResultVariableHours = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableMinute = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableSecond = ConstantHelper.constTimeVariableUnit;
                objChallenge.ResultVariableMS = ConstantHelper.constTimeVariableUnit;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hrs"></param>
        /// <param name="mnts"></param>
        /// <param name="scnd"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string FormatTimeVariable(string hrs, string mnts, string scnd, string ms)
        {
            hrs = string.IsNullOrEmpty(hrs) ? ConstantHelper.constTimeVariableUnit : hrs.Trim();
            mnts = string.IsNullOrEmpty(mnts) ? ConstantHelper.constTimeVariableUnit : mnts.Trim();
            scnd = string.IsNullOrEmpty(scnd) ? ConstantHelper.constTimeVariableUnit : scnd.Trim();
            ms = string.IsNullOrEmpty(ms) ? ConstantHelper.constTimeVariableUnit : ms.Trim();
            return hrs + ":" + mnts + ":" + scnd + "." + ms;
        }
        /// <summary>
        /// Reset Challenge TimeVariable
        /// </summary>
        /// <param name="objAdminChallenge"></param>
        public static void ResetChallengeVariable(AdminChallenge objAdminChallenge)
        {
            // Set default time format
            if (objAdminChallenge != null)
            {
                objAdminChallenge.VariableHours = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.VariableMinute = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.VariableSecond = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.VariableMS = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.GlobalResultFilterHours = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.GlobalResultFilterMinute = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.GlobalResultFilterSecond = ConstantHelper.constTimeVariableUnit;
                objAdminChallenge.GlobalResultFilterMS = ConstantHelper.constTimeVariableUnit;
            }
        }
        /// <summary>
        /// Reset the Global Challenge Filter
        /// </summary>
        /// <param name="objChallenge"></param>
        public static void ResetChallengeGlobalVariable(CreateChallengeVM objChallenge)
        {
            // Set default time format
            if (objChallenge != null)
            {
                objChallenge.GlobalResultFilterHours = ConstantHelper.constTimeVariableUnit;
                objChallenge.GlobalResultFilterMinute = ConstantHelper.constTimeVariableUnit;
                objChallenge.GlobalResultFilterSecond = ConstantHelper.constTimeVariableUnit;
                objChallenge.GlobalResultFilterMS = ConstantHelper.constTimeVariableUnit;
            }
        }


        /// <summary>
        /// Get Selected TrendingCategory based on challenge/program based on challengeId and IsProgram flage
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<TrendingCategory> GetSelectedTrendingCategory(LinksMediaContext dataContext, int challengeId, bool isProgram)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetSelectedTrendingCategory challengeId:" + challengeId + ",isProgram-" + isProgram);
                List<TrendingCategory> listTrendingCategory = (from tm in dataContext.ChallengeTrendingAssociations
                                                               join tt in dataContext.TrendingCategory
                                                               on tm.TrendingCategoryId equals tt.TrendingCategoryId
                                                               where tm.ChallengeId == challengeId && tm.IsProgram == isProgram
                                                               orderby tt.TrendingName ascending
                                                               select new TrendingCategory
                                                               {
                                                                   TrendingCategoryId = tm.TrendingCategoryId,
                                                                   TrendingCategoryName = tt.TrendingName,
                                                                   challengeSubTypeId = 16
                                                               }).ToList();
                if (listTrendingCategory == null)
                {
                    listTrendingCategory = new List<TrendingCategory>();
                }
                return listTrendingCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedTrendingCategory  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected GetSelectedChallengeCategory based on challenge/program based on challengeId and IsProgram flage
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetSelectedChallengeCategory(LinksMediaContext dataContext, int challengeId, bool isProgram)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetSelectedTrendingCategory challengeId:" + challengeId + ",isProgram-" + isProgram);

                List<ChallengeCategory> listChallengeCategory = (from tm in dataContext.ChallengeCategoryAssociations
                                                                 join tt in dataContext.ChallengeCategory
                                                                 on tm.ChallengeCategoryId equals tt.ChallengeCategoryId
                                                                 where tm.ChallengeId == challengeId && tm.IsProgram == isProgram
                                                                 orderby tt.ChallengeCategoryName ascending
                                                                 select new ChallengeCategory
                                                                 {
                                                                     ChallengeCategoryId = tm.ChallengeCategoryId,
                                                                     ChallengeCategoryName = tt.ChallengeCategoryName
                                                                 }).ToList();
                if (listChallengeCategory == null)
                {
                    listChallengeCategory = new List<ChallengeCategory>();
                }
                return listChallengeCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedChallengeCategory  end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected NoTrainer Challenge Team
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="isProgram"></param>
        /// <returns></returns>
        public static List<DDTeams> GetSelectedNoTrainerChallengeTeam(LinksMediaContext dataContext, int challengeId, bool isProgram)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetSelectedNoTrainerChallengeTeam challengeId:" + challengeId + ",isProgram-" + isProgram);

                List<DDTeams> listDDTeams = (from tm in dataContext.Teams
                                             join tt in dataContext.NoTrainerChallengeTeams
                                             on tm.TeamId equals tt.TeamId
                                             where tt.ChallengeId == challengeId && tt.IsProgram == isProgram
                                             orderby tm.TeamName ascending
                                             select new DDTeams
                                             {
                                                 TeamId = tt.TeamId,
                                                 TeamName = tm.TeamName,
                                                 IsDefaultTeam = tm.IsDefaultTeam
                                             }).ToList();
                if (listDDTeams == null)
                {
                    listDDTeams = new List<DDTeams>();
                }
                return listDDTeams;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedNoTrainerChallengeTeam end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Selected NoTrainer Challenge Team
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="isProgram"></param>
        /// <returns></returns>
        public static List<ChallengeCategory> GetSelectedChallengeCategoryAssociations(LinksMediaContext dataContext, int challengeId, bool isProgram = false)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetSelectedChallengeCategoryAssociations challengeId:" + challengeId);

                List<ChallengeCategory> listChallengeCategory = (from tm in dataContext.ChallengeCategoryAssociations
                                                                 join tt in dataContext.ChallengeCategory
                                                                 on tm.ChallengeCategoryId equals tt.ChallengeCategoryId
                                                                 where tm.ChallengeId == challengeId && tm.IsProgram == isProgram
                                                                 orderby tt.ChallengeCategoryName ascending
                                                                 select new ChallengeCategory
                                                                 {
                                                                     ChallengeCategoryId = tm.ChallengeCategoryId,
                                                                     ChallengeCategoryName = tt.ChallengeCategoryName
                                                                 }).ToList();
                if (listChallengeCategory == null)
                {
                    listChallengeCategory = new List<ChallengeCategory>();
                }
                return listChallengeCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedChallengeCategoryAssociations end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Selected NoTrainer Challenge Team
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="isProgram"></param>
        /// <returns></returns>
        public static List<TrendingCategory> GetChallengeTrendingAssociationsList(LinksMediaContext dataContext, int challengeId, int challengeSubTypeId, bool IsProgram = false)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetChallengeTrendingAssociationsList challengeId:" + challengeId);

                List<TrendingCategory> listTrendingCategory = (from tm in dataContext.ChallengeTrendingAssociations
                                                               join tt in dataContext.TrendingCategory
                                                               on tm.TrendingCategoryId equals tt.TrendingCategoryId
                                                               where tm.ChallengeId == challengeId && tm.IsProgram == IsProgram
                                                               orderby tt.TrendingName ascending
                                                               select new TrendingCategory
                                                               {
                                                                   TrendingCategoryId = tm.TrendingCategoryId,
                                                                   TrendingCategoryName = tt.TrendingName,
                                                                   challengeSubTypeId = challengeSubTypeId,
                                                                   TrendingCategoryGroupId=tt.TrendingCategoryGroupId
                                                               }).ToList();
                if (listTrendingCategory == null)
                {
                    listTrendingCategory = new List<TrendingCategory>();
                }
                return listTrendingCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetChallengeTrendingAssociationsList end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected Equipmen By Challenge Id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<DDTeams> GetSelectedTeamByChallengeId(LinksMediaContext dataContext, int challengeId, bool isProgram = false)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetSelectedTeamByChallengeId challengeId:" + challengeId);

                List<DDTeams> listDDTeams = (from tm in dataContext.Teams
                                             join tt in dataContext.NoTrainerChallengeTeams
                                             on tm.TeamId equals tt.TeamId
                                             where tt.ChallengeId == challengeId && tt.IsProgram == isProgram
                                             orderby tm.TeamName ascending
                                             select new DDTeams
                                             {
                                                 TeamId = tt.TeamId,
                                                 TeamName = tm.TeamName,
                                                 IsDefaultTeam = tm.IsDefaultTeam
                                             }).ToList();
                if (listDDTeams == null)
                {
                    listDDTeams = new List<DDTeams>();
                }
                return listDDTeams;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetSelectedTeamByChallengeId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Selected CEAssociation ByChallenge
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static ExeciseVideoDetail GetSelectedCEAssociationByChallenge(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetSelectedCEAssociationByChallenge challengeId:" + challengeId);

                    ExeciseVideoDetail objExeciseVideoDetail = (from cexe in dataContext.CEAssociation
                                                                join ex in dataContext.Exercise
                                                                on cexe.ExerciseId equals ex.ExerciseId
                                                                where cexe.ChallengeId == challengeId
                                                                orderby cexe.IsShownFirstExecise descending, cexe.RocordId ascending
                                                                select new ExeciseVideoDetail
                                                                {
                                                                    ExeciseName = ex.ExerciseName,
                                                                    ExeciseUrl = ex.V720pUrl,
                                                                    ExerciseThumnail = ex.ThumnailUrl,
                                                                    ChallengeExeciseId = cexe.RocordId
                                                                }).Take(3).OrderByDescending(p => p.ChallengeExeciseId).FirstOrDefault();
                    if (objExeciseVideoDetail == null)
                    {
                        objExeciseVideoDetail = new ExeciseVideoDetail();
                    }
                    return objExeciseVideoDetail;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetSelectedCEAssociationByChallenge end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get LogInUser Primary TeamId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static int GetLogInUserPrimaryTeamId(LinksMediaContext dataContext, int userId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetLogInUserPrimaryTeamId userId:" + userId + " ,userType-" + userType);
                int teamId = 0;
                if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                {
                    teamId = (from crd in dataContext.Credentials
                              join usr in dataContext.Trainer on crd.UserId equals usr.TrainerId
                              where usr.TrainerId == userId && crd.UserType == Message.UserTypeTrainer
                              select usr.TeamId).FirstOrDefault();
                }
                else if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                {
                    teamId = (from crd in dataContext.Credentials
                              join usr in dataContext.User on crd.UserId equals usr.UserId
                              where usr.UserId == userId && crd.UserType == Message.UserTypeUser
                              select usr.TeamId).FirstOrDefault();
                }
                return teamId;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetLogInUserPrimaryTeamId end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Team MemeberCredId List based on TeamId
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static List<int> GetTeamMemeberCredIdList(LinksMediaContext dataContext, int teamId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetTeamMemeberCredIdList teamId:" + teamId);              
                List<int> credId = (from cr in dataContext.Credentials
                          join tms in dataContext.TrainerTeamMembers
                          on cr.Id equals tms.UserId
                          where tms.TeamId == teamId
                          select cr.Id).ToList();                
                return credId;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetTeamMemeberCredIdList end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Trainer TopSpecialization based on trainerId
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        public static List<string> GetTrainerTopSpecialization(int trainerId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetTrainerTopSpecialization trainerId:" + trainerId);
                   
                    List<string> listSpecializationName = (from ts in dataContext.TrainerSpecialization
                                              join s in dataContext.Specialization
                                              on ts.SpecializationId equals s.SpecializationId
                                              where ts.IsInTopThree == 1 && ts.TrainerId == trainerId
                                              select s.SpecializationName).AsEnumerable().ToList();
                    if(listSpecializationName == null)
                    {
                        listSpecializationName = new List<string>();
                    }
                    return listSpecializationName;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetTrainerTopSpecialization end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get LogIn User Following User/Trainer CredId List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static List<int> GetLogInUserFollowingCredIdList(LinksMediaContext dataContext, int userId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetLogInUserFollowingCredIdList userId:" + userId);             
                List<int> listFollowings = (from f in dataContext.Followings
                                  join c in dataContext.Credentials on f.UserId equals c.Id
                                  where c.UserType == userType && c.UserId == userId
                                  select f.FollowUserId).Distinct().ToList();
                if(listFollowings == null)
                {
                    listFollowings = new List<int>();
                }
                return listFollowings;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetLogInUserFollowingCredIdList end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Message FeedVideo List based on messageStraemId
        /// </summary>
        /// <param name="messageStraemId"></param>
        /// <returns></returns>
        public static List<VideoInfo> GetMessageFeedVideoList(int messageStraemId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetMessageFeedVideoList messageStraemId:" + messageStraemId);                 
                    List<VideoInfo> listVideoInfo = (from vl in dataContext.MessageStreamVideo
                                     where vl.MessageStraemId == messageStraemId && !string.IsNullOrEmpty(vl.VideoUrl)
                                     select new VideoInfo
                                     {
                                         RecordId = vl.RecordId,
                                         VideoUrl = vl.VideoUrl
                                     }).ToList();
                    if(listVideoInfo == null)
                    {
                        listVideoInfo = new List<VideoInfo>();
                    }
                    return listVideoInfo;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetMessageFeedVideoList end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get MessageFeed Pics List based on MessageStreamId
        /// </summary>
        /// <param name="messageStraemId"></param>
        /// <returns></returns>
        public static List<PicsInfo> GetMessageFeedPicsList(int messageStraemId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetMessageFeedPicsList messageStraemId:" + messageStraemId);

                    List<PicsInfo> listPicsInfo = (from pl in dataContext.MessageStreamPic
                                                   where pl.MessageStraemId == messageStraemId && !string.IsNullOrEmpty(pl.PicUrl)
                                                   select new PicsInfo
                                                   {
                                                       PicId = pl.RecordId,
                                                       PicsUrl = pl.PicUrl,
                                                       ImageMode = pl.ImageMode,
                                                       Height = pl.Height,
                                                       Width = pl.Width
                                                   }).ToList();
                    if (listPicsInfo == null)
                    {
                        listPicsInfo = new List<PicsInfo>();
                    }
                    return listPicsInfo;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetMessageFeedPicsList end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Save Posted ExerciseType Based on Challenge Id
        /// </summary>
        /// <param name="postedExerciseTypes"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblETCAssociation> GetPostedExerciseTypeBasedChallenge(PostedExerciseType postedExerciseTypes, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedExerciseTypeBasedChallenge challengeId:" + challengeId);
                List<tblETCAssociation> ObjETCAssociationList = new List<tblETCAssociation>();
                if (postedExerciseTypes != null)
                {
                    if (postedExerciseTypes.SelectedExerciseTypeIDs != null)
                    {
                        for (int i = 0; i < postedExerciseTypes.SelectedExerciseTypeIDs.Count; i++)
                        {
                            tblETCAssociation ObjETCAssociation = new tblETCAssociation();
                            ObjETCAssociation.ExerciseTypeId = Convert.ToInt32(postedExerciseTypes.SelectedExerciseTypeIDs[i]);
                            ObjETCAssociation.ChallengeId = challengeId;
                            ObjETCAssociationList.Add(ObjETCAssociation);
                        }
                    }
                }
                return ObjETCAssociationList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedExerciseTypeBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Save Posted TargetZones Based on ChallengeId
        /// </summary>
        /// <param name="postedTargetZones"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblTrainingZoneCAssociation> GetPostedTargetZonesBasedChallenge(PostedTargetZone postedTargetZones, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedTargetZonesBasedChallenge challengeId:" + challengeId);
                List<tblTrainingZoneCAssociation> trainingZoneCAssociationList = new List<tblTrainingZoneCAssociation>();
                if (postedTargetZones != null)
                {
                    if (postedTargetZones.SelectedTargetZoneIDs != null)
                    {
                        for (int i = 0; i < postedTargetZones.SelectedTargetZoneIDs.Count; i++)
                        {
                            tblTrainingZoneCAssociation objTrainingZoneCAssociation = new tblTrainingZoneCAssociation();
                            objTrainingZoneCAssociation.PartId = Convert.ToInt32(postedTargetZones.SelectedTargetZoneIDs[i]);
                            objTrainingZoneCAssociation.ChallengeId = challengeId;
                            trainingZoneCAssociationList.Add(objTrainingZoneCAssociation);
                        }
                    }
                }
                return trainingZoneCAssociationList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedTargetZonesBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Save PostedEquipments Based on ChallengeId
        /// </summary>
        /// <param name="postedEquipments"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblCEquipmentAssociation> GetPostedEquipmentsBasedChallenge(PostedEquipment postedEquipments, int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedEquipmentsBasedChallenge challengeId:" + challengeId);
                List<tblCEquipmentAssociation> eqipmentCAssociationList = new List<tblCEquipmentAssociation>();
                if (postedEquipments != null)
                {
                    if (postedEquipments.SelectedEquipmentIDs != null)
                    {
                        for (int i = 0; i < postedEquipments.SelectedEquipmentIDs.Count; i++)
                        {
                            tblCEquipmentAssociation objEqipmentCAssociation = new tblCEquipmentAssociation();
                            objEqipmentCAssociation.EquipmentId = Convert.ToInt32(postedEquipments.SelectedEquipmentIDs[i]);
                            objEqipmentCAssociation.ChallengeId = challengeId;
                            eqipmentCAssociationList.Add(objEqipmentCAssociation);
                        }
                    }
                }
                return eqipmentCAssociationList;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedEquipmentsBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Posted Specializations Based TrainerId
        /// </summary>
        /// <param name="postedSpecializations"></param>
        /// <param name="trainerId"></param>
        /// <returns></returns>



        /// <summary>
        /// Save PostedTeams Based on ChallengeId
        /// </summary>
        /// <param name="postedTeams"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblNoTrainerChallengeTeam> GetPostedTeamsBasedChallenge(LinksMediaContext dataContext, PostedTeams postedTeams, int challengeId, bool isProgram = false, bool isFittnessTest = false)
        {
            StringBuilder traceLog = new StringBuilder();

            try
            {
                traceLog.AppendLine("Start: GetPostedTeamsBasedChallenge challengeId:" + challengeId);
                List<tblNoTrainerChallengeTeam> selectedTeams = new List<tblNoTrainerChallengeTeam>();
                if (postedTeams != null)
                {
                    if (postedTeams.TeamsID != null)
                    {
                        for (int i = 0; i < postedTeams.TeamsID.Count; i++)
                        {
                            int teamId = Convert.ToInt32(postedTeams.TeamsID[i]);
                            if (!dataContext.NoTrainerChallengeTeams.Any(utm => utm.ChallengeId == challengeId && utm.TeamId == teamId && utm.IsProgram == isProgram))
                            {
                                tblNoTrainerChallengeTeam tms = new tblNoTrainerChallengeTeam()
                                {
                                    TeamId = teamId,
                                    ChallengeId = challengeId,
                                    IsProgram = isProgram,
                                    IsFittnessTest = isFittnessTest
                                };
                                selectedTeams.Add(tms);
                            }
                        }
                    }
                }
                return selectedTeams;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedTeamsBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Posted Teams Based on ChallengeId
        /// </summary>
        /// <param name="postedTrendingCategory"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblChallengeTrendingAssociation> GetPostedTrendingCategoryBasedChallenge(LinksMediaContext dataContext, PostedTrendingCategory postedTrendingCategory, int challengeId, bool isProgram = false)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedTeamsBasedChallenge challengeId:" + challengeId);
                List<tblChallengeTrendingAssociation> selectedTrendingCategory = new List<tblChallengeTrendingAssociation>();
                if (postedTrendingCategory != null)
                {
                    if (postedTrendingCategory.TrendingCategoryID != null)
                    {
                        for (int i = 0; i < postedTrendingCategory.TrendingCategoryID.Count; i++)
                        {
                            int trendingCategoryId = Convert.ToInt32(postedTrendingCategory.TrendingCategoryID[i]);
                            if (!dataContext.ChallengeTrendingAssociations.Any(utm => utm.ChallengeId == challengeId && utm.TrendingCategoryId == trendingCategoryId && utm.IsProgram == isProgram))
                            {
                                tblChallengeTrendingAssociation tms = new tblChallengeTrendingAssociation()
                                {
                                    TrendingCategoryId = trendingCategoryId,
                                    ChallengeId = challengeId,
                                    IsProgram = isProgram
                                };
                                selectedTrendingCategory.Add(tms);
                            }
                        }
                    }
                }
                return selectedTrendingCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedTeamsBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Posted ChallengeCategory Based on ChallengeId
        /// </summary>
        /// <param name="postedChallengeCategory"></param>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<tblChallengeCategoryAssociation> GetPostedChallengeCategoryBasedChallenge(LinksMediaContext dataContext, PostedChallengeCategory postedChallengeCategory, int challengeId, bool isProgram = false)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start: GetPostedChallengeCategoryBasedChallenge challengeId:" + challengeId);
                // Add the challenge trending associated trening categorys
                List<tblChallengeCategoryAssociation> selectedChallengeCategory = new List<tblChallengeCategoryAssociation>();
                if (postedChallengeCategory != null)
                {
                    if (postedChallengeCategory.ChallengeCategoryId != null)
                    {
                        for (int i = 0; i < postedChallengeCategory.ChallengeCategoryId.Count; i++)
                        {
                            int challengeCategoryId = Convert.ToInt32(postedChallengeCategory.ChallengeCategoryId[i]);
                            if (!dataContext.ChallengeCategoryAssociations.Any(utm => utm.ChallengeId == challengeId && utm.ChallengeCategoryId == challengeCategoryId && utm.IsProgram == isProgram))
                            {
                                tblChallengeCategoryAssociation tms = new tblChallengeCategoryAssociation()
                                {
                                    ChallengeCategoryId = challengeCategoryId,
                                    ChallengeId = challengeId,
                                    IsProgram = isProgram
                                };
                                selectedChallengeCategory.Add(tms);
                            }
                        }
                    }
                }
                return selectedChallengeCategory;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetPostedChallengeCategoryBasedChallenge end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Challenge Execise Assocated based on challengeId
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        public static List<ExerciseVM> GetCEAssociationByChallenge(int challengeId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    /*Get challenge detail by challenge*/
                    traceLog.AppendLine("Start: GetCEAssociationByChallenge challengeId:" + challengeId);

                    List<ExerciseVM> listExerciseVM = (from exer in dataContext.Exercise
                                                       join ce in dataContext.CEAssociation on exer.ExerciseId equals ce.ExerciseId
                                                       where ce.ChallengeId == challengeId
                                                       select new ExerciseVM
                                                       {
                                                           ExerciseId = exer.ExerciseId,
                                                           ExerciseName = exer.ExerciseName,
                                                           ExcersiceDescription = exer.Description,
                                                           ChallengeExcerciseDescription = ce.Description,
                                                           VedioLink = exer.VideoLink,
                                                           Index = exer.Index,
                                                           Reps = ce.Reps,
                                                           WeightForManDB = ce.WeightForMan,
                                                           WeightForWomanDB = ce.WeightForWoman,
                                                       }).ToList();
                    return listExerciseVM;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("GetCEAssociationByChallenge end() : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Get User FullName Based on User/trainer CredId And UserType
        /// </summary>
        /// <param name="userCredentialId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static string GetUserFullNameBasedUserCredIdAndUserType(LinksMediaContext dataContext, int userCredentialId, string userType)
        {
            StringBuilder traceLog = new StringBuilder();
            string userFullName = string.Empty;
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetUserFullNameBasedUserCredIdAndUserType userCredentialId:" + userCredentialId + ",userType-" + userType);

                if (!string.IsNullOrEmpty(userType) && userCredentialId > 0)
                {
                    if (userType.Equals(Message.UserTypeUser, StringComparison.OrdinalIgnoreCase))
                    {
                        userFullName = (from usr in dataContext.User
                                        join creden in dataContext.Credentials on usr.UserId equals creden.UserId
                                        where creden.Id == userCredentialId
                                        select new { UserName = usr.FirstName + " " + usr.LastName }).FirstOrDefault().UserName;
                    }
                    else if (userType.Equals(Message.UserTypeTrainer, StringComparison.OrdinalIgnoreCase))
                    {
                        userFullName = (from trnr in dataContext.Trainer
                                        join creden in dataContext.Credentials on trnr.TrainerId equals creden.UserId
                                        where creden.Id == userCredentialId
                                        select new { UserName = trnr.FirstName + " " + trnr.LastName }).FirstOrDefault().UserName;
                    }
                }
                return userFullName;
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetUserFullNameBasedUserCredIdAndUserType end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Get Friend Notification Challenge Description
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="userCredentialId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IQueryable<PendingChallengeVM> GetFriendNotificationChallengeDescription(LinksMediaContext dataContext, int userCredentialId, NotificationSenderVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            //  string userFullName = string.Empty;
            try
            {
                /*Get challenge detail by challenge*/
                traceLog.AppendLine("Start: GetUserFullNameBasedUserCredIdAndUserType userCredentialId:" + userCredentialId);
                if (model.IsFriendChallenge)
                {
                    IQueryable<PendingChallengeVM> lstPendingChallenge = (from c in dataContext.Challenge
                                                                          join ctf in dataContext.ChallengeToFriends on c.ChallengeId equals ctf.ChallengeId
                                                                          join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                          where c.IsActive && ctf.TargetId == userCredentialId && ctf.SubjectId == model.SenderCredID
                                                                          orderby ctf.ChallengeDate descending
                                                                          select new PendingChallengeVM
                                                                          {
                                                                              ChallengeId = c.ChallengeId,
                                                                              ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                              ChallengeName = c.ChallengeName,
                                                                              DifficultyLevel = c.DifficultyLevel,
                                                                              Duration = c.FFChallengeDuration == null ? string.Empty : c.FFChallengeDuration,
                                                                              ChallengeType = ct.ChallengeType,
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
                                                                              Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId).Select(y => y.UserId).Distinct().Count(),
                                                                              ResultUnit = ct.ResultUnit,
                                                                              ChallengeByUserName = ctf.ChallengeByUserName,
                                                                              Result = ctf.Result == null ? string.Empty : ctf.Result,
                                                                              Fraction = ctf.Fraction == null ? string.Empty : ctf.Fraction,
                                                                              SubjectId = ctf.SubjectId,
                                                                              ResultUnitSuffix = ctf.ResultUnitSuffix == null ? string.Empty : ctf.ResultUnitSuffix,
                                                                              ChallengeToFriendId = ctf.ChallengeToFriendId,
                                                                              DbPostedDate = ctf.ChallengeDate,
                                                                              Description = c.Description == null ? string.Empty : c.Description,
                                                                              ChallengeByUserType = model.SenderUserType,
                                                                              ChallengeByUserId = model.SenderUserID,
                                                                              ProgramImageUrl = c.ProgramImageUrl,
                                                                          });
                    return lstPendingChallenge;
                }
                else
                {

                    IQueryable<PendingChallengeVM> lstPendingChallenge = (from c in dataContext.Challenge
                                                                          join ctf in dataContext.UserAssignments on c.ChallengeId equals ctf.ChallengeId
                                                                          join ct in dataContext.ChallengeType on c.ChallengeSubTypeId equals ct.ChallengeSubTypeId
                                                                          where c.IsActive && ctf.TargetId == userCredentialId && ctf.SubjectId == model.SenderCredID
                                                                          orderby ctf.ChallengeDate descending
                                                                          select new PendingChallengeVM
                                                                          {
                                                                              ChallengeId = c.ChallengeId,
                                                                              ChallengeSubTypeId = ct.ChallengeSubTypeId,
                                                                              ChallengeName = c.ChallengeName,
                                                                              DifficultyLevel = c.DifficultyLevel,
                                                                              Duration = c.FFChallengeDuration,
                                                                              ChallengeType = ct.ChallengeType,
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
                                                                              Strenght = dataContext.UserChallenge.Where(uc => uc.ChallengeId == c.ChallengeId)
                                                                                                                   .Select(y => y.UserId).Distinct().Count(),
                                                                              ResultUnit = ct.ResultUnit,
                                                                              ChallengeByUserName = ctf.ChallengeByUserName,
                                                                              Result = ctf.Result == null ? string.Empty : ctf.Result,
                                                                              Fraction = ctf.Fraction == null ? string.Empty : ctf.Fraction,
                                                                              SubjectId = ctf.SubjectId,
                                                                              ResultUnitSuffix = ctf.ResultUnitSuffix == null ? string.Empty : ctf.ResultUnitSuffix,
                                                                              ChallengeToFriendId = ctf.UserAssignmentId,
                                                                              DbPostedDate = ctf.ChallengeDate,
                                                                              Description = c.Description,
                                                                              ChallengeByUserType = model.SenderUserType,
                                                                              ChallengeByUserId = model.SenderUserID,
                                                                              ProgramImageUrl = c.ProgramImageUrl,
                                                                          });
                    return lstPendingChallenge;
                }


            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("GetUserFullNameBasedUserCredIdAndUserType end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

    }
}