namespace LinksMediaCorpUtility
{
    using System.Collections.Generic;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public class AppSettingsReader
    {
        public AppSettingsReader()
        {
        }
        /// <summary>
        /// Read all item matching with passing parameter from <appsetting> tab inside web.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> ReadAppSettings(string key)
        {
            List<string> results = null;
            try
            {
                results = new List<string>();
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                bool count = null == appSettings;
                if (!count)
                {
                    int i = 0;
                    while (true)
                    {
                        count = i < appSettings.Count;
                        if (!count)
                        {
                            break;
                        }
                        count = null == appSettings[i];
                        if (!count)
                        {
                            count = !key.Equals(appSettings.GetKey(i));
                            if (!count)
                            {
                                results.Add(appSettings[i]);
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception exception)
            {
                // writing error information into log file
                Logging.WriteError(Logging.FormatError(string.Empty, Logging.FormatException(exception)));
            }
            List<string> strs = results;
            return strs;
        }
        /// <summary>
        /// Read first item matching with parameter key from <appsetting> tab inside web.config file 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadFirstAppSetting(string key)
        {
            string item;
            bool count;
            string result = string.Empty;
            List<string> appSettings = AppSettingsReader.ReadAppSettings(key);
            if (appSettings == null)
            {
                count = true;
            }
            else
            {
                count = appSettings.Count <= 0;
            }
            bool flag = count;
            if (!flag)
            {
                int i = 0;
                while (true)
                {
                    flag = i < appSettings.Count;
                    if (!flag)
                    {
                        item = result;
                        return item;
                    }
                    flag = null == appSettings[i];
                    if (!flag)
                    {
                        break;
                    }
                    i++;
                }
                item = appSettings[i];
                return item;
            }
            item = result;
            return item;
        }
    }
}