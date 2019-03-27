namespace LinksMediaCorpUtility
{
    using System.Diagnostics;
    using System.Web;
    public class FFMPEG
    {
        Process ffmpeg;
        public void Exec(string input, string output) 
        {              
                try
                {
                    ffmpeg = new Process();
                    input = AddQuotes(input); 
                    //ffmpeg -i input.flv -ss 00:00:14.435 -vframes 1 out.png
                    //ffmpeg.StartInfo.Arguments = " -i " + input + (parametri != null ? " " + parametri : "") + " " + output;
                    ffmpeg.StartInfo.Arguments = " -i " + input + " -ss 00:00:00.100 -vframes 1" + " " + output;
                    //ffmpeg.StartInfo.FileName = "utils/ffmpeg.exe";
                    ffmpeg.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/Utility/ffmpeg.exe");
                    ffmpeg.StartInfo.UseShellExecute = false;
                    ffmpeg.StartInfo.RedirectStandardOutput = true;
                    ffmpeg.StartInfo.RedirectStandardError = true;
                    ffmpeg.StartInfo.CreateNoWindow = true;
                    ffmpeg.Start();
                    ffmpeg.WaitForExit();
                    ffmpeg.Close();                    
                }
                catch
                {                   
                    return;
                }
                finally
                {
                    ffmpeg.Dispose();
                }
                
        }
        /// <summary>
        /// Add Quotes to string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string AddQuotes(string s)
        {
            return "\"" + s + "\"";
        }
        /// <summary>
        /// Save the SaveThumbnail of Execise Video
        /// </summary>
        /// <param name="video"></param>
        /// <param name="jpg"></param>
        /// <param name="velicina"></param>
        public void SaveThumbnail(string video, string jpg)
        {
            //string pixel="640x480";
            try
            {
                //if (velicina == null) velicina = "640x480";
               // exec(video, jpg, "-s " + velicina);
                Exec(video, jpg);
            }
            catch
            {
                return;
            }
        }
    }
}
