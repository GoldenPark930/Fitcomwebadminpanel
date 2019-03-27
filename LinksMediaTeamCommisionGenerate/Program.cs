using LinksMediaCorpBusinessLayer;
using LinksMediaCorpUtility;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
namespace LinksMediaTeamCommissionGenerator
{

    internal class Program
    {      
        static bool isCheckedFirstdayofMonth = false;
        /// <summary>
        /// Main function to run commission report generation and save in database
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Starting LinksMediaTeamCommissionGenerator() and save report in database");
                Console.WriteLine("Start Generating service");               
                int commissionGenerateDate = Convert.ToInt32(ConfigurationManager.AppSettings["CommissionReportDate"].ToString());
                string emailTemplatePath = ConfigurationManager.AppSettings["EmailTemplatePath"].ToString();
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    TimeSpan ts = DateTime.Today.AddDays(1) - DateTime.Now;
                    if (currentDate.Day == commissionGenerateDate && !isCheckedFirstdayofMonth)
                    {
                        // get the comssion values and save in database 
                        isCheckedFirstdayofMonth = true;                      
                        // Save in database
                        string emailTempletePath = Path.Combine(Environment.CurrentDirectory, emailTemplatePath);
                        bool isSaved = TeamTalkBL.SaveTeamCommissionReport(currentDate, emailTempletePath);
                        //waits certan time and run the code                     
                        traceLog.AppendLine("Report data is saved for teamswith status - " + isSaved);
                    }
                    Thread.Sleep(DateTime.Today.AddDays(1) - DateTime.Now);
                    isCheckedFirstdayofMonth = false;               
                }
            }
            catch (Exception ex)
            {
                isCheckedFirstdayofMonth = false;
                Console.WriteLine(ex.Message);
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            finally
            {
                traceLog.AppendLine("End:Main() Response Result Status-" + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        } 

    }
}
