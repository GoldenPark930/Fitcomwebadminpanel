namespace LinksMediaCorpUtility
{
    using System;
    using System.IO;

    public class Logging
    {
        public Logging()
        {
            //default constructor
        }
        /// <summary>
        /// Formatting of error message date time by year, month, day, hours, minutes and second
        /// </summary>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string FormatError(string location, string message)
        {
            string result = string.Empty;
            result = string.Concat(result, string.Format("{0:yyyy-MM-dd HH:mm:ss}  Error in: {1}", DateTime.Now, location), Environment.NewLine);
            result = string.Concat(result, message, Environment.NewLine);
            string str = result;
            return str;
        }
        /// <summary>
        /// Format of exception that occur in application
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatException(Exception ex)
        {
            string result = string.Empty;
            bool flag = null == ex;
            if (!flag)
            {
                result = string.Concat(result, "Exception: ", Environment.NewLine);
                Exception e = ex;
                while (true)
                {
                    flag = null != e;
                    if (!flag)
                    {
                        break;
                    }
                    result = string.Concat(result, "==========", Environment.NewLine);
                    result = string.Concat(result, e.Message, Environment.NewLine);
                    result = string.Concat(result, e.StackTrace, Environment.NewLine);
                    e = e.InnerException;
                }
            }
            string str = result;
            return str;
        }
        /// <summary>
        /// format of message with current date time
        /// </summary>
        /// <param name="location"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string FormatMessage(string location, string message)
        {
            string result = string.Empty;
            //concatenate message with date time
            result = string.Concat(result, string.Format("{0:yyyy-MM-dd HH:mm:ss}  Message: {1}", DateTime.Now, location), Environment.NewLine);
            result = string.Concat(result, message, Environment.NewLine);
            string str = result;
            return str;
        }
        /// <summary>
        /// create static method to write error log into error log file
        /// </summary>
        /// <param name="message"></param>
        public static void WriteError(string message)
        {
            //read log file directory from web.config file
            string folder = AppSettingsReader.ReadFirstAppSetting("ErrorLogPath");
            //string directoryName = ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
            //check directory and create that
            var directoryName = System.IO.Path.GetDirectoryName(
                  System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + ConstantHelper.constDoubleBackSlash + folder;
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            bool flag = !string.IsNullOrWhiteSpace(directoryName);
            if (flag)
            {
                flag = directoryName.EndsWith(ConstantHelper.constDoubleBackSlash, StringComparison.OrdinalIgnoreCase);
                if (!flag)
                {
                    directoryName = string.Concat(directoryName, ConstantHelper.constDoubleBackSlash);
                }
                DateTime today = DateTime.Today;
                //concatenate directory name with current date
                string path = string.Concat(directoryName, today.ToString("yyyy-MM-dd"), ".txt");
                StreamWriter w = null;
                try
                {
                    //Check log file directory exist or not 
                    flag = File.Exists(path);
                    if (!flag)
                    {
                        //if file directory not exist create directory 
                        File.Create(path).Close();
                    }
                    w = File.AppendText(path);
                    //write message into log file
                    w.WriteLine(message);
                    w.Flush();
                }
                finally
                {
                    flag = null == w;
                    if (!flag)
                    {
                        try
                        {
                            w.Close();
                        }
                        catch
                        {
                            // throw;
                        }
                    }
                }
            }
        }
    }
}