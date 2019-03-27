namespace LinksMediaCorpUtility
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    public class CommonUtility
    {
        public static string GetToken()
        {
            return Guid.NewGuid().ToString();
        }
        public static string VirtualPath
        {
            get
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                       HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
            }
        }
        /// <summary>
        /// Get the FitServer Path for Execise Videos
        /// </summary>
        public static string VirtualFitComExercisePath
        {
            get
            {
                //return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                //      HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                return "http://fitcomadmin.com/";
                // return Convert.ToString(ConfigurationManager.AppSettings["FitProductionServerPath"]);
            }
        }
        /// <summary>
        /// Get the IOS certifate Path
        /// </summary>
        public static string GetIOSCertificatePath
        {

            get
            {
                //distribution IOS certificate
               return HttpContext.Current.Server.MapPath("~/Resources/Dist_Certificates.p12");
                //Local IOS certificate
               //    return HttpContext.Current.Server.MapPath("~/Resources/CertificatesDev.p12");
            }
        }
        /// <summary>
        /// Remove HTML Tags from string
        /// </summary>
        /// <param name="HTMLCode"></param>
        /// <returns></returns>
        public static string RemoveHTMLTags(string HTMLCode)
        {
            return System.Text.RegularExpressions.Regex.Replace(
              HTMLCode, "<[^>]*>", "");
        }
        /// <summary>
        /// Remove Html Style from string for showing on mobile devices
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string RemoveHtmStyle(string desc)
        {
            try
            {
                if (!string.IsNullOrEmpty(desc))
                {
                    desc = Regex.Replace(desc, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim();
                    desc = Regex.Replace(desc, "(<style.+?</style>)|(<script.+?</script>)", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    desc = Regex.Replace(desc, "(<img.+?>)", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    desc = Regex.Replace(desc, "(<o:.+?</o:.+?>)", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    desc = Regex.Replace(desc, "<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    desc = Regex.Replace(desc, "class=.+?>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    desc = Regex.Replace(desc, "&quot;", "''");
                    desc = Regex.Replace(desc, "&#39;", "'");
                    desc = Regex.Replace(desc, "<.*?>|&.*?;", string.Empty);
                    desc = RemoveHTMLTags(desc);
                }
                return desc;
            }
            catch
            {
                return desc;
            }
        }
        /// <summary>
        /// check Apllication and web api runing on Production server or not
        /// </summary>
        /// <returns></returns>
        public static bool IsProduction()
        {
            return true;
        }
    }
}