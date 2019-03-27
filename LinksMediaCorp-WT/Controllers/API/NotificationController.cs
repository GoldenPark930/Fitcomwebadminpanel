using LinksMediaCorp.Filters;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using LinksMediaCorpUtility;
using LinksMediaCorpUtility.Resources;
namespace LinksMediaCorp.Controllers.API
{
    /// <summary>
    /// NotificationController is used for Notification Web API services
    /// </summary>
    [TokenValidationFilter]
    public class NotificationController : ApiController
    {
        /// <summary>
        ///  Get UserFollowers list for user/trainer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/03/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UserNotificationSetting(UserNotificationSettingVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UserNotificationSetting() Request Data:-DeviceToken-" + model.DeviceID + ",DeviceType-" + model.DeviceType + ",IsNotify-" + model.IsNotify + ",NotificationType-" + model.NotificationType);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = NotificationApiBL.SaveUpdateNotification(model);
                objResponse.IsResultTrue = true;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:UserNotificationSetting() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }


        /// <summary>
        /// Get UserNotifications for Notification Tab
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/03/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetUserNotifications(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<UserNotificationsVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetUserFollowers() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponse = new ServiceResponse<Total<List<UserNotificationsVM>>>();
                objResponse.jsonData = NotificationApiBL.GetUserNotificationList(model.StartIndex, model.EndIndex);
                objResponse.IsResultTrue = true;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetUserFollowers Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Update the user Devices Token in case of devices token on in IOS Devices
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 04/03/2016
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult UpdateDeviceToken(DeviceTokenVM model)
        {
            StringBuilder traceLog = null;
            // string token = string.Empty;
            ServiceResponse<bool> totalNotifaication = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateDeviceToken Request Data:-DeviceToken-" + model.DeviceToken + ",DeviceType-" + model.DeviceType + ",AuthToken-" + model.AuthToken);
                bool isUpdated = CommonWebApiBL.UpdateDeviceTokenAndType(model);
                totalNotifaication = new ServiceResponse<bool>();
                totalNotifaication.jsonData = isUpdated;
                totalNotifaication.IsResultTrue = true;
                return Ok(totalNotifaication);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                totalNotifaication.IsResultTrue = false;
                totalNotifaication.ErrorMessage = Message.ServerError;
                return Ok(totalNotifaication);
            }
            finally
            {
                traceLog.AppendLine("End: UpdateDeviceToken() Response Result Status-" + totalNotifaication.IsResultTrue + ",Modified DatetTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;

            }
        }

        /// <summary>
        /// Get Notification Receiver Result after completed
        /// </summary>
        /// <param name="objUserPersonalTrainer"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetNotificationReceiverResult(PostIdVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<RecentResultVM> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetNotificationReceiverResult()-ResultId" + model.PostId + ",NotificationID-" + model.NotificationID);
                objResponse = new ServiceResponse<RecentResultVM>();
                objResponse.jsonData = TeamBL.GetNotificationReceiverResult(model.PostId, model.NotificationID);
                objResponse.IsResultTrue = true;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetNotificationReceiverResult() Response Result Status-Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Send DefaultTeam Trainer when user joined with No option
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendDefaultTeamNotification(NotificationRequest notifyRequest)
        {
            StringBuilder traceLog = null;
            ServiceResponse<bool> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SendDefaultTeamNotification()-notifyRequest-" + notifyRequest);
                objResponse = new ServiceResponse<bool>();
                objResponse.jsonData = TeamBL.SendDefaultTeamNotification(notifyRequest.IsJoinedDefaultTeam);
                objResponse.IsResultTrue = true;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:SendDefaultTeamNotification() Response Result Status-Fetched DatTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
