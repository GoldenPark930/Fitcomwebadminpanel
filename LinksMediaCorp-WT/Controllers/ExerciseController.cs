using Excel;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;


namespace LinksMediaCorp.Controllers
{
    /// <summary>
    /// Exercise Controller is used to update the execise and upload new exercise vidoes with excel/csv file
    /// </summary>          
    public class ExerciseController : Controller
    {
        /// <summary>
        /// Upload Exercise Videos
        /// </summary>
        /// <param name="sproutUplodaId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadExerciseVideos(string sproutUplodaId)
        {
            StringBuilder traceLog = null;
            ViewExercisesData execiseData = new ViewExercisesData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadExerciseVideos action  method in Execise Controller"+ sproutUplodaId);
                // Get Exercise Fail Upload history
                execiseData.Exercises = ExerciseBL.GetExerciseUploadHistory();
                return View(execiseData);

            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("UploadExerciseVideos end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
       
        /// <summary>
        /// Upload Exercise Video on sprout video server
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public  ActionResult UploadExerciseVideos()
        {
            StringBuilder traceLog = null;           
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadExerciseVideos() in Execise Controller");
                var httpRequest = System.Web.HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    string sPath = "";
                    sPath = Server.MapPath("~/Temp/");
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var fileDoc = httpRequest.Files[i];
                        string newVideoId = Path.GetFileName(fileDoc.FileName);
                        if (!System.IO.File.Exists(newVideoId))
                        {
                            fileDoc.SaveAs(sPath + newVideoId);
                            UploadExeciseVideoToFitcom(sPath + newVideoId);
                        }
                    }
                }
                else
                {
                    return Json(new { Message = "No files selected.", StatusCode = 100 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "File Uploaded Successfully!", StatusCode = 101 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(new { Message = "Error occurred. Error details: " + ex.Message, StatusCode = 100 }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("UploadExerciseVideos end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Upload Exercise Videos with Excel/CSV file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public  ActionResult UploadExerciseVideosAndExcel()
        {
            StringBuilder traceLog = null;           
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            DataTable dt =null;
         
            try
            {
                
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UploadExerciseVideos() in Execise Controller");
                var httpRequest = System.Web.HttpContext.Current.Request;
              
                if (httpRequest.Files.Count > 0)
                {
                   
                    Dictionary<string, string> uploadedExeciseVideos = new Dictionary<string, string>();                   
                    List<UpdateExerciseSproutLink> FailToUploadedExeciseVideos = new List<UpdateExerciseSproutLink>();
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var fileDoc = httpRequest.Files[i];
                        IExcelDataReader reader = null;
                        string sPath = "";
                        sPath = Server.MapPath("~/Temp/");
                        string newVideoId = Path.GetFileName(fileDoc.FileName);
                        string execiseName = Path.GetFileNameWithoutExtension(fileDoc.FileName);
                        if (fileDoc.FileName.EndsWith(".xls",StringComparison.OrdinalIgnoreCase))
                        {
                            using (Stream stream = fileDoc.InputStream)
                            {                              

                                reader = ExcelReaderFactory.CreateBinaryReader(stream);                             
                                reader.IsFirstRowAsColumnNames = true;
                                DataSet result = reader.AsDataSet();
                                reader.Close();
                                dt.Clear();
                                dt = result.Tables[0];
                            }
                        }
                        else if (fileDoc.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            dt = new DataTable();
                            dt.Locale = CultureInfo.InvariantCulture;
                            dt.Clear();
                            dt.Columns.Add("Name");
                            dt.Columns.Add("Index");
                            dt.Columns.Add("Team ID");
                            dt.Columns.Add("Trainer ID");
                            if (!System.IO.File.Exists(sPath + newVideoId))
                            {
                                fileDoc.SaveAs(sPath + newVideoId);
                            }                         
                            using (var fileReader = System.IO.File.OpenText(sPath + newVideoId))
                            using (var csvReader = new CsvHelper.CsvReader(fileReader))
                            {
                                csvReader.Configuration.HasHeaderRecord = true;
                                csvReader.Configuration.IgnoreHeaderWhiteSpace = false;
                                csvReader.Configuration.IsHeaderCaseSensitive = true;
                                csvReader.Configuration.RegisterClassMap<CSVExerciseMap>();                               
                                while (csvReader.Read())
                                {
                                    var record = csvReader.GetRecord<CSVExerciseVM>();
                                    if (record != null)
                                    {
                                        DataRow rowdata = dt.NewRow();
                                        rowdata["Name"] = record.Name;
                                        rowdata["Index"] = record.Index;
                                        rowdata["Team ID"] = record.TeamID;
                                        rowdata["Trainer ID"] = record.TrainerID;
                                        dt.Rows.Add(rowdata);
                                    }
                                }
                            }

                        }
                        else if (fileDoc.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                        {
                            using (Stream stream = fileDoc.InputStream)
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                reader.IsFirstRowAsColumnNames = true;
                                DataSet result = reader.AsDataSet();
                                reader.Close();
                                dt = result.Tables[0];
                            }
                        }
                        else if (fileDoc.FileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                        {
                            uploadedExeciseVideos.Add(execiseName, sPath + newVideoId);
                        }
                        else
                        {
                            var FailToUpload = new UpdateExerciseSproutLink
                            {
                                ExerciseName = execiseName,
                                Index = string.Empty,
                                TeamID = string.Empty,
                                TrainerID = string.Empty,
                                FailedVideoName = fileDoc.FileName,

                            };
                            FailToUploadedExeciseVideos.Add(FailToUpload);
                        }
                        // Save the files in local directory
                        if (!System.IO.File.Exists(sPath + newVideoId))
                        {
                            fileDoc.SaveAs(sPath + newVideoId);
                        }
                    }
                    // varify  execel execisename and upload on sprout server
                    foreach (DataRow row in dt.Rows)
                    {
                        string execiseName = string.Empty;
                        string index = string.Empty;
                        string teamID = string.Empty;
                        string trainerID = string.Empty;
                        foreach (DataColumn col in dt.Columns)
                        {
                            switch (col.ColumnName)
                            {
                                case "Name":
                                    execiseName = Convert.ToString(row[col.ColumnName]);
                                    break;
                                case "Index":
                                    index = Convert.ToString(row[col.ColumnName]);
                                    break;
                                case "Team ID":
                                    teamID = Convert.ToString(row[col.ColumnName]);
                                    break;
                                case "Trainer ID":
                                    trainerID = Convert.ToString(row[col.ColumnName]);
                                    break;
                            }
                        }
                        //  uploaded Execise Videos on sprout server
                        if (uploadedExeciseVideos.ContainsKey(execiseName))
                        {
                            // find out local stored file path from dictionary
                            string localUploadPath = string.Empty;
                            if (uploadedExeciseVideos.TryGetValue(execiseName, out localUploadPath))
                            {
                                if (!string.IsNullOrEmpty(localUploadPath))
                                {
                                    // Upload on Execise Video on sprout server, get response and add to Fitcom database;
                                    string uploadesfile = UploadExeciseVideoToFitcom(localUploadPath);
                                    if (!string.IsNullOrEmpty(uploadesfile))
                                    {
                                        ExerciseVideoResponse response = JsonConvert.DeserializeObject<ExerciseVideoResponse>(uploadesfile);
                                        if (response != null)
                                        {
                                            UpdateExerciseSproutLink objUpdateExerciseSproutLink = new UpdateExerciseSproutLink
                                            {
                                                SecuryId = response.security_token,
                                                VideoId = response.id,
                                                V1080pUrl = response.assets.videos._1080p,
                                                V240pUrl = response.assets.videos._240p,
                                                V360pUrl = response.assets.videos._360p,
                                                V480pUrl = response.assets.videos._480p,
                                                V720pUrl = response.assets.videos._720p,
                                                ThumnailUrl = response.assets.thumbnails[0],
                                                ExerciseName = execiseName,
                                                IsActive = true,
                                                Index = index,
                                                Description = string.Empty,
                                                IsUpdated = true,
                                                TeamID = teamID,
                                                TrainerID = trainerID,
                                                VideoLink = execiseName + ConstantHelper.constMP4ExesionWithdot,
                                                SourceUrl = string.Empty
                                            };
                                            // Add Execise Details
                                            ExerciseBL.AddExerciseSproutData(objUpdateExerciseSproutLink);
                                        }
                                    }
                                    else
                                    {
                                        var FailToUpload = new UpdateExerciseSproutLink
                                        {
                                            ExerciseName = execiseName,
                                            Index = index,
                                            TeamID = teamID,
                                            TrainerID = trainerID,
                                            FailedVideoName = execiseName + ConstantHelper.constMP4ExesionWithdot,
                                        };
                                        FailToUploadedExeciseVideos.Add(FailToUpload);
                                    }
                                }
                                // Delete Uploaded Execise
                                if (System.IO.File.Exists(localUploadPath))
                                {
                                    System.IO.File.Delete(localUploadPath);
                                }
                                localUploadPath = string.Empty;
                            }
                        }
                        else if (!string.IsNullOrEmpty(execiseName))
                        {
                            var FailToUpload = new UpdateExerciseSproutLink
                            {
                                ExerciseName = execiseName,
                                Index = index,
                                TeamID = teamID,
                                TrainerID = trainerID,
                                FailedVideoName = string.Empty,
                            };
                            FailToUploadedExeciseVideos.Add(FailToUpload);
                        }
                    }
                    // Save To Failed Upload data in database
                    ExerciseBL.SaveFailedSproutServer(FailToUploadedExeciseVideos);
                }
                else
                {
                    return Json(new { Message = "No files selected.", StatusCode = 100 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "File Uploaded Successfully!", StatusCode = 101 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return Json(new { Message = "Error occurred. Error details: " + ex.Message, StatusCode = 100 }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                traceLog.AppendLine("UploadExerciseVideos end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                if(dt != null)
                {
                    dt.Dispose();
                }
            }
        }
        /// <summary>
        /// Read CSV from uploed by admin
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        public static IEnumerable<CSVExerciseVM>  ReadCSV(string fileName) 
        {
            using (var fileReader = System.IO.File.OpenText(fileName))
            using (var csvReader = new CsvHelper.CsvReader(fileReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.IgnoreHeaderWhiteSpace = false;
                csvReader.Configuration.IsHeaderCaseSensitive = true;
                csvReader.Configuration.RegisterClassMap<CSVExerciseMap>();                               
                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<CSVExerciseVM>();
                    yield return record;
                }
            }
        }
        
        /// <summary>
        /// Upload Execise Video To Sprout Server
        /// </summary>
        /// <param name="sPath"></param>
        [NonAction]
        private string UploadExeciseVideoToFitcom(string sPath)
        {
            try
            {
                string uploadesfileResponse = string.Empty;
                using (WebClient httpWebClient = new WebClient())
                {
                    httpWebClient.Headers[ConstantHelper.constSproutVideoApiKey] = ConstantHelper.constSproutVideoApiKeyValue;
                    httpWebClient.Headers[HttpRequestHeader.Host] = ConstantHelper.constSproutHost;
                    Uri uploadExerciseUri = new Uri(ConstantHelper.constSproutUploadApi);
                    var uploadedExecisedata = httpWebClient.UploadFile(uploadExerciseUri, "POST", sPath);
                    if (uploadedExecisedata != null)
                    {
                        uploadesfileResponse = System.Text.Encoding.ASCII.GetString(uploadedExecisedata);

                    }

                }
                return uploadesfileResponse;
            }
            catch (WebException ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return string.Empty;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Upload Execel To Fitcom
        /// </summary>
        /// <param name="sPath"></param>
        [NonAction]
        private void UploadExecelToFitcom(string sPath)
        {
            try
            {
                using (WebClient httpWebClient = new WebClient())
                {
                    httpWebClient.Headers[ConstantHelper.constSproutVideoApiKey] = ConstantHelper.constSproutVideoApiKeyValue;
                    httpWebClient.Headers[HttpRequestHeader.Host] = ConstantHelper.constSproutHost;
                    Uri uploadExerciseUri = new Uri(ConstantHelper.constSproutUploadApi);
                    var uploadedExecisedata = httpWebClient.UploadFile(uploadExerciseUri, "POST", sPath);
                    if (uploadedExecisedata != null)
                    {
                        var uploadesfile = System.Text.Encoding.ASCII.GetString(uploadedExecisedata);
                        if (uploadesfile != null)
                        {
                            ExerciseVideoResponse response = JsonConvert.DeserializeObject<ExerciseVideoResponse>(uploadesfile);
                            if (response != null)
                            {
                                UpdateExerciseSproutLink objUpdateExerciseSproutLink = new UpdateExerciseSproutLink
                                {
                                    SecuryId = response.security_token,
                                    VideoId = response.id
                                };
                                // Get all details of
                                ExerciseBL.UpdateExerciseSproutData(objUpdateExerciseSproutLink);
                            }
                        }
                    }
                    if (System.IO.File.Exists(sPath))
                    {
                        System.IO.File.Delete(sPath);
                    }
                }
            }
            catch (WebException ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
        }
        /// <summary>
        /// Update Execise Video To Fitcom sprout server
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="exerciseId"></param>
        /// <param name="sproudId"></param>
        private void UpdateExeciseVideoToFitcom(string sPath, int exerciseId, string sproudId)
        {
            try
            {
                GetSingleExeciseVideo();
                using (WebClient httpWebClient = new WebClient())
                {
                    httpWebClient.Headers[ConstantHelper.constSproutVideoApiKey] = ConstantHelper.constSproutVideoApiKeyValue;
                    httpWebClient.Headers[HttpRequestHeader.Host] = ConstantHelper.constSproutHost;
                    string replacedExerciseWepApi = string.Format(ConstantHelper.constSproutReplacedUploadApi, sproudId);
                    Uri replacedUrl = new Uri(replacedExerciseWepApi);
                    var uploadedExecisedata = httpWebClient.UploadFile(replacedUrl, "POST", sPath);
                    if (uploadedExecisedata != null)
                    {
                        if (System.IO.File.Exists(sPath))
                        {
                            System.IO.File.Delete(sPath);
                        }
                        var uploadesfile = System.Text.Encoding.ASCII.GetString(uploadedExecisedata);
                        if (uploadesfile != null)
                        {
                            ExerciseVideoResponse response = JsonConvert.DeserializeObject<ExerciseVideoResponse>(uploadesfile);
                            if (response != null)
                            {
                                UpdateExerciseSproutLink objUpdateExerciseSproutLink = new UpdateExerciseSproutLink
                                {
                                    SecuryId = response.security_token,
                                    VideoId = response.id,
                                    V1080pUrl = response.assets.videos._1080p,
                                    V240pUrl = response.assets.videos._240p,
                                    V360pUrl = response.assets.videos._360p,
                                    V480pUrl = response.assets.videos._480p,
                                    V720pUrl = response.assets.videos._720p,
                                    ThumnailUrl = response.assets.thumbnails[0],
                                    ExerciseId = exerciseId
                                };
                                // Get all details of
                                ExerciseBL.UpdateExerciseSproutData(objUpdateExerciseSproutLink);
                            }
                        }
                    }
                    System.IO.File.Delete(sPath);
                }
            }
            catch (WebException ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
        }
       
        /// <summary>
        /// Get Single ExeciseVideo
        /// </summary>
        [NonAction]
        private void GetSingleExeciseVideo()
        {
            try
            {
                using (WebClient httpWebClient = new WebClient())
                {
                    httpWebClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    httpWebClient.Headers[ConstantHelper.constSproutVideoApiKey] = ConstantHelper.constSproutVideoApiKeyValue;
                    httpWebClient.Headers[HttpRequestHeader.Host] = ConstantHelper.constSproutHost;
                    string webapisingleExercise = string.Format(ConstantHelper.constSproutGetSingleVideoApi, "7c9adbb01d15e9c7f4");
                    httpWebClient.DownloadString(webapisingleExercise);

                }
            }
            catch (WebException ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                throw;
            }
        }

        /// <summary>
        /// Get Exercise vides
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Exercises()
        {
            StringBuilder traceLog = null;
            ViewExercisesData execiseData = new ViewExercisesData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: Exercises action  method in Execise Controller");
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    execiseData.CurrentPageIndex = 0;
                    execiseData.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = execiseData.CurrentPageIndex;
                    pagenumber = 0;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        execiseData.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    execiseData.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = "ASC";
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    execiseData.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    execiseData.SortField = ConstantHelper.constExerciseName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    execiseData.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = "ASC";
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    execiseData.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    execiseData.SortDirection = ConstantHelper.constASC;
                }
                execiseData.Exercises = ExerciseBL.GetExecisesList();
                return View(execiseData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("Exercises end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Fail Upload History
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FailUploadHistory()
        {
            StringBuilder traceLog = null;
            ViewExercisesData execiseData = new ViewExercisesData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
                || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: FailUploadHistory action  method in Execise Controller");                
                execiseData.Exercises = ExerciseBL.GetExerciseUploadHistory();
                return PartialView("_ExerciseUploalFailHistory", execiseData);
              
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return RedirectToAction(ConstantHelper.constError);
            }
            finally
            {
                traceLog.AppendLine("FailUploadHistory end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        
        /// <summary>
        /// Upload Execise To Fitcom
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public ActionResult UploadExecisedToFitcom()
        {
            return View("UploadExerciseVideos");
        }

        /// <summary>
        /// Search Exercise
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchExercise(string id)
        {
            StringBuilder traceLog = null;
            ViewExercisesData execiseData = new ViewExercisesData();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
               || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                traceLog = new StringBuilder();
                if (Request.QueryString[ConstantHelper.constpage] != null)
                {
                    int pagenumber = 0;
                    execiseData.CurrentPageIndex = 0;
                    execiseData.CurrentPageIndex = int.TryParse(Request.QueryString[ConstantHelper.constpage] as string, out pagenumber) ? (pagenumber) : pagenumber;
                    TempData[ConstantHelper.constpage] = execiseData.CurrentPageIndex;
                    pagenumber = 0;
                }
                else if (TempData[ConstantHelper.constpage] != null)
                {
                    var savedpage = TempData[ConstantHelper.constpage];
                    if (savedpage != null && savedpage.GetType() == typeof(int))
                        execiseData.CurrentPageIndex = (int)savedpage;
                }
                if (Request.QueryString[ConstantHelper.constsort] != null)
                {
                    execiseData.SortField = Request.QueryString[ConstantHelper.constsort];
                    TempData[ConstantHelper.constsort] = "ASC";
                }
                else if (TempData[ConstantHelper.constsort] != null)
                {
                    execiseData.SortField = TempData[ConstantHelper.constsort] as string;
                }
                else
                {
                    execiseData.SortField = ConstantHelper.constExerciseName;
                }
                if (Request.QueryString[ConstantHelper.constsortdir] != null)
                {
                    execiseData.SortDirection = Request.QueryString[ConstantHelper.constsortdir];
                    TempData[ConstantHelper.constsortdir] = "ASC";
                }
                else if (TempData[ConstantHelper.constsortdir] != null)
                {
                    execiseData.SortDirection = TempData[ConstantHelper.constsortdir] as string;
                }
                else
                {
                    execiseData.SortDirection = ConstantHelper.constASC;
                }
                execiseData.Exercises = ExerciseBL.GetExecisesList(id);
                HttpContext.Session[Message.PreviousUrl] = "Exercises";
                return PartialView("_Exercises", execiseData);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*transfer to error page*/
                return RedirectToAction("Error");
            }
            finally
            {
                traceLog.AppendLine("SearchChallenges end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Get Exercise by Id for Uppdate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateExercise(int id)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                if (id > 0)
                {
                    traceLog.AppendLine("Start: UpdateExercise() in Exercise controller");                   
                    ViewExerciseVM objViewExerciseVM = ExerciseBL.UpdateExercise(id);
                    return View(objViewExerciseVM);
                }
                return View();
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Update Exercise based on admin changed field
        /// </summary>
        /// <param name="updateExecise"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateExercise(ViewExerciseVM updateExecise)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {

                if (ModelState.IsValid)
                {
                    traceLog.AppendLine("Start: UpdateExercise() in Exercise controller");                  
                    string sproutId = ExerciseBL.UpdateExercise(updateExecise);
                    TempData["AlertMessage"] = Message.UpdateMessage;
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        string sPath = "";
                        sPath = Server.MapPath("~/Temp/");
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            var fileDoc = httpRequest.Files[i];
                            string newVideoId = Path.GetFileName(fileDoc.FileName);
                            if (!System.IO.File.Exists(newVideoId))
                            {
                                fileDoc.SaveAs(sPath + newVideoId);
                                if (string.IsNullOrEmpty(sproutId))
                                {
                                     UploadExecelToFitcom(sPath + newVideoId);
                                }
                                else
                                {
                                     UpdateExeciseVideoToFitcom(sPath + newVideoId, updateExecise.ExerciseId, sproutId);
                                }

                            }
                        }
                    }
                    return RedirectToAction(HttpContext.Session[Message.PreviousUrl].ToString());
                }
                else
                {
                    return View(updateExecise);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("UpdateTrainer end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        /// Change Exercise Status
        /// </summary>
        /// <param name="execiseId"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeExerciseStatus(int execiseId, int operationId)
        {
            StringBuilder traceLog = new StringBuilder();
            if (Login.IsSessionExpire(Convert.ToString(HttpContext.Session[ConstantHelper.constLoginUserName]))
            || Convert.ToString(HttpContext.Session[ConstantHelper.constUserType]) != ConstantHelper.constLoginadmin)
            {
                return RedirectToAction(ConstantHelper.constLogin, ConstantHelper.constLogin);
            }
            try
            {
                ExerciseStatusVM response = null;
                if (execiseId > 0)
                {
                    traceLog.AppendLine("Start: ChangeExerciseStatus() in Exercise controller");                  
                    int actionId = Convert.ToInt32(operationId);
                    response = ExerciseBL.ChangeExerciseStatus(execiseId,  actionId);                   
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                /*return view to error page*/
                return View();
            }
            finally
            {
                traceLog.AppendLine("ChangeExerciseStatus end() : --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        

    }
}