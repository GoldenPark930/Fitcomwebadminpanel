namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Web.Http;
    using LinksMediaCorpUtility;
    using LinksMediaCorpUtility.Resources;
    using System.Threading.Tasks;

    [TokenValidationFilter]
    public class HomeRequestController : ApiController
    {

        /// <summary>
        /// Get HomeData for version 4
        /// </summary>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/19/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult GetHomeData()
        {
            StringBuilder traceLog = null;
            ServiceResponse<HomeDataUpdated<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetHomeData");
                objResponse = new ServiceResponse<HomeDataUpdated<List<ViewPostVM>>>();
                objResponse.IsResultTrue = true;
                objResponse.jsonData = HomeRequestBL.GetHomeRequestDataUpdated(Thread.CurrentPrincipal.Identity.Name);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetHomeData Repsonse Result Ststus-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get Home Page Json Data
        /// </summary>
        /// <returns>ServiceResponse<HomeData></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 02/8/2016
        /// </devdoc>
        /// 
        [HttpGet]
        public IHttpActionResult GetHomeDataV1()
        {
            StringBuilder traceLog = null;
            ServiceResponse<HomeDataUpdated<List<ViewPostVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetHomeDataV1");
                objResponse = new ServiceResponse<HomeDataUpdated<List<ViewPostVM>>>();
                objResponse.IsResultTrue = true;
                objResponse.jsonData = HomeRequestBL.GetHomeRequestDataUpdated(Thread.CurrentPrincipal.Identity.Name);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetHomeDataV1 Repsonse Result Ststus-" + objResponse.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get Trainer Profile Data
        /// </summary>
        /// <returns>ServiceResponse<TrainerViewVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult TrainerViewData(TrainerInfo objTrainerInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<TrainerViewVM> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetTrainerViewData() Request TrainerID-" + objTrainerInfo.TrainerId);
                objResponce = new ServiceResponse<TrainerViewVM>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = HomeRequestBL.GetTrainerViewData(objTrainerInfo.TrainerId, objTrainerInfo.NotificationID);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetTrainerViewData() Reponse Data:-Result Status-" + objResponce.IsResultTrue + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get Trainer pics
        /// </summary>
        /// <returns>ServiceResponse<List<PicsVM>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult GetPics(TrainerInfo objTrainerInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<PicsVM>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetPics() Request Data:-TrainerId-" + objTrainerInfo.TrainerId);
                objResponce = new ServiceResponse<List<PicsVM>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = HomeRequestBL.GetPics(objTrainerInfo.TrainerId);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetPics() Response Result Ststus-" + objResponce.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to get Trainer videos
        /// </summary>
        /// <returns>ServiceResponse<List<VideoInfo>></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/12/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult GetVideos(TrainerInfo objTrainerInfo)
        {
            StringBuilder traceLog = null;
            ServiceResponse<List<VideoInfo>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetVideos() Request Data:-TrainerId-" + objTrainerInfo.TrainerId);
                objResponce = new ServiceResponse<List<VideoInfo>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = HomeRequestBL.GetVideos(objTrainerInfo.TrainerId);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetVideos() Response Result Ststus-" + objResponce.IsResultTrue + ",Fetched DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Get VideosAndPics
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/19/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetVideosAndPics(NumberOfRecord<UserIdAndUserType> model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<VideoAndPicVM>>> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetVideosAndPics() Request Data:-UserId-" + model.Param.UserId + ",UserType-" + model.Param.UserType + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                objResponce = new ServiceResponse<Total<List<VideoAndPicVM>>>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = HomeRequestBL.GetVideosAndPics(model.Param.UserId, model.Param.UserType, model.StartIndex, model.EndIndex);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:GetVideosAndPics() Response Data:- Result ststus-" + objResponce.IsResultTrue + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Action to Logout users
        /// </summary>
        /// <returns>ServiceResponse<MessageVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 05/19/2015
        /// </devdoc>
        [HttpGet]
        public IHttpActionResult Logout()
        {
            StringBuilder traceLog = null;
            ServiceResponse<MessageVM> response = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: Logout Action");
                response = new ServiceResponse<MessageVM>();
                bool flag = CommonWebApiBL.DeleteCurrentUserToken();
                if (flag)
                {
                    response.IsResultTrue = true;
                    response.jsonData = new MessageVM() { Message = Message.Logout };
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                response.IsResultTrue = false;
                response.ErrorMessage = Message.ServerError;
                return Ok(response);
            }
            finally
            {
                traceLog.AppendLine("End :Logout Action  Response Result Status-" + response.IsResultTrue + ",Logout DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Get MainSearch BarList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <devdoc>
        /// Developer Name - Irshad Ansari
        /// Date - 06/12/2015
        /// </devdoc>
        [HttpPost]
        public IHttpActionResult GetMainSearchBarList(LinksMediaCorpEntity.MainSearchBarParam model)
        {
            StringBuilder traceLog = null;
            SearchResponse<Dictionary<string, object>> response = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetMainSearchBarList Request Data:-SearchString-" + model.SearchString + ",SelectTab-" + model.SelectTab + ",StartIndex-" + model.StartIndex + ",EndIndex-" + model.EndIndex);
                response = new SearchResponse<Dictionary<string, object>>();
                response.IsResultTrue = true;
                object totalCount = 0;
                object isMoreFlag = false;
                response.jsonData = HomeRequestBL.GetMainSearchBarList(model);
                if (response.jsonData.TryGetValue("isMoreFlag", out isMoreFlag))
                {
                    response.IsMoreAvailable = (bool)isMoreFlag;
                    response.jsonData.Remove("isMoreFlag");
                }
                if (response.jsonData.TryGetValue("TotalCount", out totalCount))
                {
                    response.TotalCount = (int)totalCount;
                    response.jsonData.Remove("TotalCount");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                response.IsResultTrue = false;
                response.ErrorMessage = Message.ServerError;
                return Ok(response);
            }
            finally
            {
                traceLog.AppendLine("End :GetMainSearchBarList() Response Data:-Result Status-" + response.IsResultTrue + ",TotalCount-" + response.TotalCount + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Andriod App Subscription
        /// </summary>
        /// <param name="appSubscriptionInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateAppSubscriptionStatus(AppSubscriptionVM appSubscriptionInfo) 
        {
            StringBuilder traceLog = null;
            SearchResponse<bool> response = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateAppSubscriptionStatus Request Data:-IsSubscriptionStatus-"
                    + appSubscriptionInfo.SubscriptionStatus + ",DeviceId-" + appSubscriptionInfo.DeviceId + ",DeviceType-" + appSubscriptionInfo.DeviceType +
                    ",SubscriptionId-" + appSubscriptionInfo.SubscriptionId + ",SubscriptionStatus-" + Convert.ToString(appSubscriptionInfo.SubscriptionStatus)
                    + ",AutoRenewing-" + appSubscriptionInfo.AutoRenewing);
                response = new SearchResponse<bool>();
                response.IsResultTrue = true;                
                response.jsonData = HomeRequestBL.UpdateAppSubscriptionStatus(appSubscriptionInfo);           

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                response.IsResultTrue = false;
                response.ErrorMessage = Message.ServerError;
                return Ok(response);
            }
            finally
            {
                traceLog.AppendLine("End :UpdateAppSubscriptionStatus() Response Data:-Status Date-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

        /// <summary>
        /// Validate IOS Purchase Receipt
        /// </summary>
        /// <param name="receiptRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ValidatePurchaseReceipt(ReceiptRequest receiptRequest)
        {
            StringBuilder traceLog = null;
            SearchResponse<string> response = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: UpdateAppSubscriptionStatus Request Data:-DeviceId-"
                    + receiptRequest.DeviceId + ",DeviceType-" + receiptRequest.DeviceType);
                response = new SearchResponse<string>();
                response.IsResultTrue = true;
                response.jsonData = HomeRequestBL.ValidateIOSReceipt(receiptRequest);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                response.IsResultTrue = false;
                response.ErrorMessage = Message.ServerError;
                return Ok(response);
            }
            finally
            {
                traceLog.AppendLine("End :UpdateAppSubscriptionStatus() Response Data:-Status Date-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
        /// <summary>
        /// Test Primium User subsc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult TestPrimiumUser(int id)
        {
            StringBuilder traceLog = null;
            SearchResponse<bool> response = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: TestPrimiumUser");
                response = new SearchResponse<bool>();
                response.IsResultTrue = true;
                response.jsonData = HomeRequestBL.TestPrimiumSubscrition(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                response.IsResultTrue = false;
                response.ErrorMessage = Message.ServerError;
                return Ok(response);
            }
            finally
            {
                traceLog.AppendLine("End :TestPrimiumUser() Response Data:-Status Date-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}
