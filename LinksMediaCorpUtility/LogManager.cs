namespace LinksMediaCorpUtility
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    /// <summary>
    /// This is for trace log and error log write
    /// </summary>
    /// <ModifiedBy>Arvind Kumar</ModifiedBy>
    public class LogManager
    {
        /// <summary>
        /// its for object of LogManager
        /// </summary>
        private static readonly LogManager objLogManager = new LogManager();
        /// <summary>
        /// Create object for logEntry
        /// </summary>
        private LogEntry logEntry;
        /// <summary>
        /// Gets Logmanagerinstance
        /// </summary>
        public static LogManager LogManagerInstance
        {
            get
            {
                return objLogManager;
            }
        }
        /// <summary>
        /// Method is used to write the Tracelog
        /// </summary>
        /// <param name="message">its for message string</param>
        public void WriteTraceLog(StringBuilder message)
        {
            try
            {
                ////get the tracing flag value from the configuration file.
                string isTrace = ConfigurationManager.AppSettings["MakeTraceLog"].ToString();
                ////check the condition if is trace value is YES.
                if (string.Compare(isTrace, "Yes", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(isTrace, "True", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ////Getting the method name who called this method
                    StackFrame objStackFrame = new StackFrame(1);
                    ////create instanse of LogEntry
                    LogEntry objLogEntry = new LogEntry();
                    ////Add the category to the LogEntry object, got from the Config
                    objLogEntry.Categories.Add("Trace");
                    ////Append Method name in the Trace Message
                    objLogEntry.Message = objStackFrame.GetMethod().Name + ConstantHelper.constColonWithDash + message.ToString();
                    ////Write the trace
                    Logger.Write(objLogEntry);
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// its for write the error log
        /// </summary>
        /// <param name="exception">its for exception</param>
        public void WriteErrorLog(Exception exception)
        {
            try
            {
                ////Getting the method name who called this method
                StackFrame objStackFrame = new StackFrame(1);
                logEntry = new LogEntry();
                ////set type of severity 
                logEntry.Severity = TraceEventType.Error;
                ////set log message 
                logEntry.Message = objStackFrame.GetMethod().Name + ConstantHelper.constColonWithDash + exception.ToString();
                ////set categories 
                logEntry.Categories.Add("Exception");
                ////Set current Title
                logEntry.Title = "Error";
                //// Set Current System time.
                logEntry.TimeStamp = DateTime.Now;
                ////Write the trace
                Logger.Write(logEntry);
            }
            catch
            {
                throw;
            }
        }
    }
}
