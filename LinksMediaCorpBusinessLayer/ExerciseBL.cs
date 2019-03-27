using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinksMediaCorpDataAccessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using System.Globalization;

namespace LinksMediaCorpBusinessLayer
{ 
    /// <summary>
    /// ExerciseBL class is used fro update ercesie an upload  vidoes on sprout server
    /// </summary>
    public class ExerciseBL
    {
        /// <summary>
        /// Get ExecisesList based serach cateria value
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<ViewExerciseVM> GetExecisesList(string search = null)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExecisesList---- " + DateTime.Now.ToLongDateString());
                    List<ViewExerciseVM> exerlistlist = dataContext.Exercise.Where(ex => ex.IsActive)
                                                                   .Select(exe => new ViewExerciseVM
                                                                   {
                                                                       ExerciseId = exe.ExerciseId,
                                                                       ExerciseName = exe.ExerciseName,
                                                                       Index = exe.Index,
                                                                       ThumnailUrl = exe.ThumnailUrl,
                                                                       ExerciseVideoUrl = exe.V720pUrl,
                                                                       TeamId = exe.TeamID,
                                                                       TrainerId = exe.TrainerID,
                                                                       SelectedStatus=exe.ExerciseStatus
                                                                   }).OrderBy(ch => ch.ExerciseName).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        search = search.ToLower(CultureInfo.InvariantCulture);
                        exerlistlist = exerlistlist.Where(ch => (ch.ExerciseName != null &&
                                                    ch.ExerciseName.ToLower(CultureInfo.InvariantCulture).IndexOf(search, 0, StringComparison.OrdinalIgnoreCase) > -1))
                                                   .OrderBy(ch => ch.ExerciseName).ToList<ViewExerciseVM>();
                    }
                    
                    return exerlistlist;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetExecisesList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        /// <summary>
        /// Get Exercise Upload History
        /// </summary>
        /// <returns></returns>
        public static List<ViewExerciseVM> GetExerciseUploadHistory()
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: GetExecisesList---- " + DateTime.Now.ToLongDateString());
                    List<ViewExerciseVM> exerlistlist = dataContext.ExerciseUploadHistory
                                                                   .Select(exe => new ViewExerciseVM
                                                                   {
                                                                    TeamId=exe.TeamId,
                                                                    TrainerId=exe.TrainerId,                                                                    
                                                                    ExerciseName = string.IsNullOrEmpty(exe.ExerciseName) ?exe.FailedVideoName :  exe.ExerciseName
                                                                    ,
                                                                       CreatedDate=exe.CreatedDate,
                                                                    Index = exe.Index,                                                                     
                                                                   }).OrderByDescending(ch => ch.CreatedDate).ToList();

                 

                    return exerlistlist;


                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  GetExecisesList : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        
        /// <summary>
        /// Update Exercise index and teamId and Trainer Id
        /// </summary>
        /// <param name="execiseId"></param>
        /// <returns></returns>
        public static ViewExerciseVM UpdateExercise(int execiseId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateExercise---- " + DateTime.Now.ToLongDateString());
                    ViewExerciseVM execise = null;
                    if (execiseId > 0)
                    {
                        traceLog.AppendLine("Updating Execise Id- " + execiseId);
                        execise = dataContext.Exercise.Where(ex => ex.IsActive && ex.ExerciseId == execiseId).Select(exe => new ViewExerciseVM
                        {
                            ExerciseId = exe.ExerciseId,
                            ExerciseName = exe.ExerciseName,
                            Description=exe.Description,
                            Index = exe.Index,
                            ThumnailUrl = exe.ThumnailUrl,
                            ExerciseVideoUrl = exe.V720pUrl,
                            TeamId = exe.TeamID,
                            TrainerId = exe.TrainerID
                        }).FirstOrDefault();

                    }
                    return execise;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UpdateExercise : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update Exercise
        /// </summary>
        /// <param name="updateExecise"></param>
        /// <returns></returns>
        public static string UpdateExercise(ViewExerciseVM updateExecise)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                string sproutId = string.Empty;
                try
                {
                    traceLog.AppendLine("Start: UpdateExercise---- " + DateTime.Now.ToLongDateString());
                    if (updateExecise != null && updateExecise.ExerciseId > 0)
                    {
                        traceLog.AppendLine("Updating Execise Id- " + updateExecise.ExerciseId);
                        var execise = dataContext.Exercise.Where(ex => ex.IsActive && ex.ExerciseId == updateExecise.ExerciseId).FirstOrDefault();
                        if (execise != null)
                        {
                            execise.Index = string.IsNullOrEmpty(updateExecise.Index) ? updateExecise.Index : updateExecise.Index;
                            execise.TeamID = updateExecise.TeamId;
                            execise.TrainerID = updateExecise.TrainerId;
                            execise.Description = updateExecise.Description;
                            dataContext.SaveChanges();
                            sproutId= execise.VideoId;
                        }
                    }
                    return sproutId;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UpdateExercise : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Update Exercise SproutData
        /// </summary>
        /// <param name="updateExecise"></param>
        public static void UpdateExerciseSproutData(UpdateExerciseSproutLink updateExecise)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: UpdateExerciseSproutData---- " + DateTime.Now.ToLongDateString());
                    if (updateExecise != null && updateExecise.ExerciseId > 0)
                    {
                        traceLog.AppendLine("Updating Execise Id- " + updateExecise.ExerciseId);
                        var execise = dataContext.Exercise.Where(ex => ex.ExerciseId == updateExecise.ExerciseId).FirstOrDefault();
                        if (execise != null)
                        {
                            string vidurlformat = "https://api-files.sproutvideo.com/file/{0}/{1}/{2}.mp4";
                            string v1 = updateExecise.V1080pUrl;
                            string v5 = updateExecise.V240pUrl;
                            string v4 = updateExecise.V360pUrl;
                            string v3 = updateExecise.V480pUrl;
                            string v2 = updateExecise.V720pUrl;
                            if (string.IsNullOrEmpty(Convert.ToString(v2)))
                            {
                                if (string.IsNullOrEmpty(v3))
                                {
                                    if (string.IsNullOrEmpty(v4))
                                    {

                                        execise.V720pUrl = v5;

                                    }
                                    else
                                    {

                                        execise.V720pUrl = v4;
                                    }
                                }
                                else
                                {
                                    execise.V720pUrl = v3;
                                }

                            }
                            else
                            {
                                execise.V720pUrl = v2;
                            }
                            if (string.IsNullOrEmpty(v1))
                            {
                                execise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "1080");
                            }
                            if (string.IsNullOrEmpty(v2) && string.IsNullOrEmpty(updateExecise.V720pUrl))
                            {
                                execise.V720pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "240");
                            }
                            if (string.IsNullOrEmpty(v3))
                            {
                                execise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "480");
                            }
                            if (string.IsNullOrEmpty(v4))
                            {
                                execise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "360");
                            }
                            if (string.IsNullOrEmpty(v5))
                            {
                                execise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "240");
                            }
                            execise.ThumnailUrl = updateExecise.ThumnailUrl;
                            execise.SecuryId = updateExecise.SecuryId;
                            execise.VideoId = updateExecise.VideoId;
                            execise.IsUpdated = true;
                            execise.VideoLink = updateExecise.VideoLink;
                            execise.VideoId = updateExecise.VideoId;
                            dataContext.SaveChanges();
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  UpdateExerciseSproutData : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Add Exercise SproutData
        /// </summary>
        /// <param name="updateExecise"></param>
        public static void AddExerciseSproutData(UpdateExerciseSproutLink updateExecise)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: AddExerciseSproutData---- " + DateTime.Now.ToLongDateString());
                    if (updateExecise != null)
                    {                      
                        tblExercise uploadExecise = new tblExercise(); 
                        if (uploadExecise != null)
                        {
                            string vidurlformat = "https://api-files.sproutvideo.com/file/{0}/{1}/{2}.mp4";
                            string v1 = updateExecise.V1080pUrl;
                            string v5 = updateExecise.V240pUrl;
                            string v4 = updateExecise.V360pUrl;
                            string v3 = updateExecise.V480pUrl;
                            string v2 = updateExecise.V720pUrl;
                            if (string.IsNullOrEmpty(Convert.ToString(v2)))
                            {
                                if (string.IsNullOrEmpty(v3))
                                {
                                    if (string.IsNullOrEmpty(v4))
                                    {
                                        uploadExecise.V720pUrl = v5;
                                    }
                                    else
                                    {
                                        uploadExecise.V720pUrl = v4;
                                    }
                                }
                                else
                                {
                                    uploadExecise.V720pUrl = v3;
                                }
                            }
                            else
                            {
                                uploadExecise.V720pUrl = v2;
                            }
                            if (string.IsNullOrEmpty(v1))
                            {
                                uploadExecise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "1080");
                            }
                            if (string.IsNullOrEmpty(v2) && string.IsNullOrEmpty(updateExecise.V720pUrl))
                            {
                                uploadExecise.V720pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "240");
                            }
                            if (string.IsNullOrEmpty(v3))
                            {
                                uploadExecise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "480");
                            }
                            if (string.IsNullOrEmpty(v4))
                            {
                                uploadExecise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "360");
                            }
                            if (string.IsNullOrEmpty(v5))
                            {
                                uploadExecise.V240pUrl = string.Format(vidurlformat, updateExecise.VideoId, updateExecise.SecuryId, "240");
                            }
                            uploadExecise.ThumnailUrl = updateExecise.ThumnailUrl;
                            uploadExecise.SecuryId = updateExecise.SecuryId;
                            uploadExecise.VideoId = updateExecise.VideoId;
                            uploadExecise.IsUpdated = true;
                            uploadExecise.VideoLink = updateExecise.VideoLink;
                            uploadExecise.VideoId = updateExecise.VideoId;

                            uploadExecise.ExerciseName = updateExecise.ExerciseName;
                            uploadExecise.Index = updateExecise.Index;
                            uploadExecise.IsActive = updateExecise.IsActive;
                            uploadExecise.Description = updateExecise.Description;
                            uploadExecise.VideoLink = updateExecise.VideoLink;
                            uploadExecise.TrainerID = updateExecise.TrainerID;
                            uploadExecise.TeamID = updateExecise.TeamID;
                            uploadExecise.ExerciseStatus = 1;
                            dataContext.Exercise.Add(uploadExecise);
                            dataContext.SaveChanges();
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End  AddExerciseSproutData : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Save Failed to Sprout Server
        /// </summary>
        /// <param name="FailedExecise"></param>
        public static void SaveFailedSproutServer(List<UpdateExerciseSproutLink> FailedExecise) 
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    traceLog.AppendLine("Start: SaveFailedSproutServer---- " + DateTime.Now.ToLongDateString());
                    if (FailedExecise != null && FailedExecise.Count > 0)
                    {
                        string UploadGuid = Guid.NewGuid().ToString();
                        DateTime createdDatetime = DateTime.Now;
                        List<tblExerciseUploadHistory> failedHistoryList = new List<tblExerciseUploadHistory>();
                        FailedExecise.ForEach(failexe =>
                        {
                            tblExerciseUploadHistory tblExerciseUploadHistory = new tblExerciseUploadHistory
                            {
                                Index= failexe.Index,
                                ExerciseName=failexe.ExerciseName,
                                FailedVideoName= failexe.FailedVideoName,
                                TeamId=failexe.TeamID,
                                TrainerId=failexe.TrainerID,
                                UploadHistoryGuidId= UploadGuid,
                                CreatedDate= createdDatetime
                            };
                            failedHistoryList.Add(tblExerciseUploadHistory);
                        });
                        dataContext.ExerciseUploadHistory.AddRange(failedHistoryList);
                        dataContext.SaveChanges();
                    }
                }                   
                catch
                {
                    return;
                }
                finally
                {
                    traceLog.AppendLine("End  SaveFailedSproutServer : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }
        /// <summary>
        /// Change Exercise Status
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        public static ExerciseStatusVM ChangeExerciseStatus(int exerciseId, int operationId)
        {
            StringBuilder traceLog = new StringBuilder();
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                try
                {
                    ExerciseStatusVM objExerciseStatusVM = new ExerciseStatusVM();
                    traceLog.AppendLine("Start: ChangeExerciseStatus---- " + DateTime.Now.ToLongDateString());
                    if (exerciseId > 0)
                    {
                        tblExercise exercise = dataContext.Exercise.Where(ex => ex.ExerciseId == exerciseId).FirstOrDefault();                       
                        switch (operationId)
                        {
                            
                            case 1:
                                {
                                    exercise.ExerciseStatus = 1;                           
                                    objExerciseStatusVM.OperationId = operationId;
                                    objExerciseStatusVM.OperationResultId= 1;
                                }
                                break;
                                case 2:
                                {
                                    exercise.ExerciseStatus = 2;                                 
                                    objExerciseStatusVM.OperationId = operationId;
                                    objExerciseStatusVM.OperationResultId = 2;
                                }
                                break;
                            case 3:
                                {
                                    if (dataContext.CEAssociation.Any(ex => ex.ExerciseId == exerciseId))
                                    {
                                        exercise.ExerciseStatus = 2;                                       
                                        objExerciseStatusVM.OperationResultId = -1;
                                    }
                                    else
                                    {
                                        exercise.ExerciseStatus = 3;
                                        objExerciseStatusVM.OperationResultId = 1;
                                    }
                                    objExerciseStatusVM.OperationId = operationId;                                   

                                }
                                break;
                        }
                        dataContext.SaveChanges();
                    }
                    return objExerciseStatusVM;
                }                
                finally
                {
                    traceLog.AppendLine("End  ChangeExerciseStatus : --- " + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }
        }

        
    }
}
