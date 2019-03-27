
namespace LinksMediaCorpUtility
{
    using System;
    using System.Text;
    using PushSharp.Core;
    using PushSharp.Apple;
    using Newtonsoft.Json.Linq;
    using PushSharp.Google;
    using System.Collections.Generic;
    using LinksMediaCorpUtility.Resources;
    public class PushNotificationiOSAndriod
    {

        /// <summary>
        ///  Send PushNotification For iOSAndriod app
        /// </summary>
        public bool SendPushNotificationForiOS(string deviceToken, string notificationMessage, string notificationType, int totalpendingNotification, byte[] certificateData, int totalbudget, int teamjoinUserID)
        {
            StringBuilder traceLog = null;
            //------------------------------------------------
            //IMPORTANT NOTE about Push Service Registrations
            //------------------------------------------------         
            ApnsServiceBroker _pushBroker = null;

            ApnsConfiguration config = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SendPushNotificationForiOS()");
                //-------------------------
                // APPLE NOTIFICATIONS                         

                config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production, certificateData, string.Empty);
                // Create a new broker
                _pushBroker = new ApnsServiceBroker(config);
                string urlString = string.Empty;
                // Wire up events             
                _pushBroker.OnNotificationFailed += NotificationFailed;
                _pushBroker.OnNotificationSucceeded += NotificationSent;
                bool isSoundNotify = true;
                // Start the broker
                _pushBroker.Start();
                // Queue a notification to send
                _pushBroker.QueueNotification(new PushSharp.Apple.ApnsNotification
                {

                    DeviceToken = deviceToken,
                    Payload = JObject.Parse("{\"aps\":{" +
                                                         "\"alert\" : \"" + notificationMessage + "\"," +
                                                         "\"NotificationType\" : \"" + notificationType + "\"," +
                                                         "\"TotalPendingChallenges\" : \"" + totalpendingNotification + "\"," +
                                                         "\"UserID\" : \"" + teamjoinUserID + "\"," +
                                                         "\"badge\":" + totalbudget +
                                                         (isSoundNotify ? ",\"sound\":\"sound.caf\"" : "") + "}" +
                                                         ",\"url\":\"" + urlString + "\"}")


                });


                // Stop the broker, wait for it to finish   

                _pushBroker.Stop();
                System.Threading.Thread.Sleep(2000);
                return true;

            }
            catch (Exception ex)
            {
                certificateData = null;
                config = null;
                _pushBroker = null;
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                traceLog.AppendLine("End:SendPushNotificationForiOS   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                certificateData = null;
                config = null;
                _pushBroker = null;
                traceLog = null;
            }

        }
        /// <summary>
        /// Notification Sent Event Handler
        /// </summary>
        /// <param name="notification"></param>
        private void NotificationSent(ApnsNotification notification)
        {
            try
            {
                StringBuilder traceLog = new StringBuilder();
                traceLog.Append("Sent: -> " + notification);
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return;
            }
        }
        /// <summary>
        /// Notification Failed Event Handler
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="exception"></param>
        private void NotificationFailed(ApnsNotification notification, AggregateException exception)
        {
            StringBuilder traceLog = new StringBuilder();
            try
            {
                traceLog.AppendLine("Failure: " + exception.Message + " -> " + notification);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return;
            }
            finally
            {
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
            }
        }

        /// <summary>
        ///  Send PushNotification For Andriod
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="notificationMessage"></param>
        /// <param name="pendingFrientCount"></param>
        /// <param name="totalBadgeCount"></param>
        /// <returns></returns>
        public bool SendPushNotificationForAndriod(string deviceToken, string notificationMessage, string notificationType, int totalpendingNotification, int totalbudget, int teamjoinUserID)
        {
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SendPushNotificationForAndriod()");
                // Configuration
                var config = new GcmConfiguration(ConstantKey.ConsoleAPIAccessAPIKEY);
                // Create a new broker
                var gcmBroker = new GcmServiceBroker(config);
                // Wire up events
                gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {
                    aggregateEx.Handle(ex =>
                    {
                        // See what kind of exception it was to further diagnose
                        if (ex is GcmNotificationException)
                        {
                            traceLog.AppendLine("GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                        }
                        else if (ex is GcmMulticastResultException)
                        {
                            traceLog.AppendLine("GCM Notification Failed: ");
                        }
                        else if (ex is DeviceSubscriptionExpiredException)
                        {            // If this value isn't null, our subscription changed and we should update our database
                            traceLog.AppendLine("Device RegistrationId Changed To: {newId}");

                        }
                        else if (ex is RetryAfterException)
                        {
                            // var retryException = (RetryAfterException)ex;
                            // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                            traceLog.AppendLine("GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                        }
                        else
                        {
                            traceLog.AppendLine("GCM Notification Failed for some unknown reason");
                        }
                        LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                        // Mark it as handled
                        return true;
                    });
                };
                gcmBroker.OnNotificationSucceeded += (notification) =>
                {
                    traceLog.AppendLine("GCM Notification Sent! " + DateTime.Now.ToLongDateString());
                };
                // Start the broker
                gcmBroker.Start();

                // Queue a notification to send
                gcmBroker.QueueNotification(new GcmNotification
                {
                    RegistrationIds = new List<string> {
                    deviceToken
                    },
                    Data = JObject.Parse("{ \"alert\" : \"" + notificationMessage + "\"," +
                                            "\"NotificationType\" : \"" + notificationType + "\"," +
                                           "\"UserID\" : \"" + teamjoinUserID + "\"," +
                                           "\"TotlaPendingChallenegs\" : \"" + totalpendingNotification + "\"," +
                                           "\"TotalBudget\" : \"" + totalbudget + "\"}")

                });

                // Stop the broker, wait for it to finish   
                // This isn't done after every message, but after you're
                // done with the broker
                gcmBroker.Stop();
                System.Threading.Thread.Sleep(2000);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                traceLog.AppendLine("End:SendPushNotificationForAndriod   --- " + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}