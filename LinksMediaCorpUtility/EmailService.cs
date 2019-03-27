using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LinksMediaCorpUtility
{
    /// <summary>
    /// EmailService class is used for get Email teamplate and send email to monthly commission
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Create Email Body teamplate
        /// </summary>
        /// <param name="emailTempletePath"></param>
        /// <returns></returns>
        public string CreateEmailBody(string emailTempletePath)
        {
            StringBuilder traceLog = new StringBuilder();
            string emailBody = string.Empty;
            try
            {
                traceLog.AppendLine("Start CreateEmailBody() successfully ");
                using (StreamReader reader = new StreamReader(emailTempletePath))
                {
                    emailBody = reader.ReadToEnd();
                }
            }
            catch (Exception )
            {
                emailBody = string.Empty;
               // LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            finally
            {
                traceLog.AppendLine("End:CreateEmailBody() Response Fetched DateTime" + DateTime.Now.ToLongDateString());
               // LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            return emailBody;
        }
        /// <summary>
        /// SendMail
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="mailbody"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public bool SendMail(string emailTo, string mailbody, string subject)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Start SendMail() successfully ");
                string fromaddr = ConfigurationManager.AppSettings[ConstantHelper.constSenderEmailId].ToString();
                string password = ConfigurationManager.AppSettings[ConstantHelper.constSenderEmailPassword].ToString();
                string bccEmailId = ConfigurationManager.AppSettings[ConstantHelper.constSenderBccEmailId].ToString();
                using (MailMessage msg = new MailMessage())
                {
                    msg.Subject = subject;
                    msg.IsBodyHtml = true;
                    msg.From = new MailAddress(fromaddr);
                    msg.Body = mailbody;
                    msg.To.Add(new MailAddress(emailTo));
                    msg.Bcc.Add(new MailAddress(bccEmailId));
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = ConfigurationManager.AppSettings[ConstantHelper.constHost].ToString();
                        smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantHelper.constHostPort].ToString());
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        NetworkCredential nc = new NetworkCredential(fromaddr, password);
                        smtp.Credentials = nc;
                        smtp.Send(msg);
                    }
                }
               
                return true;
            }
            catch (Exception )
            {
               // LogManager.LogManagerInstance.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                traceLog.AppendLine("End:SendMail() Response Fetched DateTime" + DateTime.Now.ToLongDateString());
                //LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }
    }
}
