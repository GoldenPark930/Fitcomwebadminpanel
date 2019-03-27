using System;
using System.Text;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using LinksMediaCorpDataAccessLayer;
using Newtonsoft.Json;
using LinksMediaCorpUtility;
namespace SproutVideoSync
{
    class Program
    {
        static List<tblExercise> exlist = null;
        static void Main(string[] args)
        {
            StringBuilder traceLog = new StringBuilder();           
            try
            {
                traceLog.AppendLine("Starting the Sync Sprout server to Local database");
                GetAllVideos();
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);              
            }
            finally
            {
                traceLog.AppendLine("End:Main() Response Result Status-" + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);                
            }
        }
        /// <summary>
        /// update the Execise video and thumnails
        /// </summary>
        public static void GetAllVideos()
        {
            StringBuilder traceLog = new StringBuilder();      
            try
            {
                traceLog.AppendLine("Starting GetAllVideos() for Sync Sprout server to Local database");
                using (LinksMediaContext dataContext = new LinksMediaContext())
                {
                    exlist = (from exe in dataContext.Exercise
                              select exe).ToList();
                }
                string URI = SproutVideoServerConstant.SproudURL;
                bool isAvailable = true;
                int vidcount = 0;
                while (isAvailable)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = SproutVideoServerConstant.ContentType;
                        wc.Headers["SproutVideo-Api-Key"] = SproutVideoServerConstant.SproutVideoApiKey; ;
                        wc.Headers[HttpRequestHeader.Host] = SproutVideoServerConstant.SproutVideoHost; ;
                        string HtmlResult = wc.DownloadString(URI);
                        SproutResponse account = JsonConvert.DeserializeObject<SproutResponse>(HtmlResult);
                        if (account.total >= vidcount)
                        {
                            isAvailable = true;
                            URI = account.next_page;
                            UpdateExeciseVideo(account);
                            vidcount += account.videos.Count();
                            account = null;
                        }
                        else
                        {
                            isAvailable = false;
                        }
                    }
                }              
            }
            catch
            {
                throw;
            }
            finally
            {
                traceLog.AppendLine("End:GetAllVideos() with Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
        /// <summary>
        /// Update Execise Video url from sprout server
        /// </summary>
        /// <param name="account"></param>
        private static void UpdateExeciseVideo(SproutResponse account)
        {
            string vidurlformat = SproutVideoServerConstant.SproudVideoListURL;
            using (LinksMediaContext dataContext = new LinksMediaContext())
            {
                StringBuilder traceLog = new StringBuilder();
                try
                {
                    traceLog.AppendLine("Starting UpdateExeciseVideo() for Sync Sprout server to Local database");
                    account.videos.ForEach(v =>
                    {
                        string exebamelinks = string.Empty;
                        exebamelinks = v.title;
                        string exebame = exebamelinks.Trim().Split('.')[0].Trim();
                        tblExercise ex = dataContext.Exercise.FirstOrDefault(exx => string.Compare(exx.ExerciseName, exebame, true) == 0 || string.Compare(exx.VideoLink, exebamelinks, true) == 0);
                        if (ex != null)
                        {
                            string v1 = v.assets.videos._1080p;
                            string v2 = v.assets.videos._720p;
                            string v3 = v.assets.videos._480p;
                            string v4 = v.assets.videos._360p;
                            string v5 = v.assets.videos._240p;
                            ex.V1080pUrl = !string.IsNullOrEmpty(v1) ? v1 : ex.V1080pUrl;
                            ex.V720pUrl = !string.IsNullOrEmpty(v2) ? v2 : ex.V720pUrl;
                            ex.V480pUrl = !string.IsNullOrEmpty(v3) ? v3 : ex.V480pUrl;
                            ex.V360pUrl = !string.IsNullOrEmpty(v4) ? v4 : ex.V480pUrl;
                            ex.V240pUrl = !string.IsNullOrEmpty(v5) ? v5 : ex.V480pUrl;
                            if (string.IsNullOrEmpty(Convert.ToString(v2)))
                            {
                                if (string.IsNullOrEmpty(v3))
                                {
                                    if (string.IsNullOrEmpty(v4))
                                    {
                                        ex.V720pUrl = !string.IsNullOrEmpty(v5) ? v5 : ex.V720pUrl;
                                    }
                                    else
                                    {
                                        ex.V720pUrl = v4;
                                    }
                                }
                                else
                                {
                                    ex.V720pUrl = v3;
                                }
                            }
                            else
                            {
                                ex.V720pUrl = v2;
                            }
                            if (string.IsNullOrEmpty(v1))
                            {
                                ex.V240pUrl = string.Format(vidurlformat, v.id, v.security_token, SproutVideoServerConstant.SproudVideoSize1080);
                            }
                            if (string.IsNullOrEmpty(v2) && string.IsNullOrEmpty(ex.V720pUrl))
                            {
                                ex.V720pUrl = string.Format(vidurlformat, v.id, v.security_token, SproutVideoServerConstant.SproudVideoSize240);
                            }
                            if (string.IsNullOrEmpty(v3))
                            {
                                ex.V240pUrl = string.Format(vidurlformat, v.id, v.security_token, SproutVideoServerConstant.SproudVideoSize480);
                            }
                            if (string.IsNullOrEmpty(v4))
                            {
                                ex.V240pUrl = string.Format(vidurlformat, v.id, v.security_token, SproutVideoServerConstant.SproudVideoSize360);
                            }
                            if (string.IsNullOrEmpty(v5))
                            {
                                ex.V240pUrl = string.Format(vidurlformat, v.id, v.security_token, SproutVideoServerConstant.SproudVideoSize240);
                            }
                            ex.ThumnailUrl = v.assets.thumbnails.FirstOrDefault();
                            ex.SecuryId = v.security_token;
                            ex.VideoId = v.id;
                            ex.IsUpdated = true;
                            dataContext.SaveChanges();
                            exebamelinks = string.Empty;
                            exebame = string.Empty;
                            ex = null;
                        }
                    });
                    traceLog.AppendLine("Finish UpdateExeciseVideo() for Sync Sprout server to Local database");
                }
                catch
                {
                    throw;
                }
                finally
                {
                    traceLog.AppendLine("End:UpdateExeciseVideo() with Fetched DateTime" + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                }
            }

        }
    }
}
