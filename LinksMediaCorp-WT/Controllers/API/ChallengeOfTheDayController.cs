namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System;
    using System.Text;
    using System.Web.Http;

    public class ChallengeOfTheDayController : ApiController
    {
        /// <summary>
        /// Action to get details of challenge of the day
        /// </summary>
        /// <returns>ServiceResponse<ChallengeOfTheDayDetailVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 03/30/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult ChallengeOfTheDayDetails(MainChallengeVM model)
        {
            StringBuilder traceLog = new StringBuilder();
            ServiceResponse<ChallengeOfTheDayDetailVM> objResponce = null;
            try
            {
                traceLog.AppendLine("Start: GetHomeRequest data:-ChallengeId-" + model.ChallengeId);
                objResponce = new ServiceResponse<ChallengeOfTheDayDetailVM>();
                objResponce.IsResultTrue = true;
                objResponce.jsonData = ChallengeOfTheDayBL.GetChallengeOfTheDayDetails(model.ChallengeId);
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest();
            }
            finally
            {
                traceLog.AppendLine("End:GetHomeRequest() Response Data:-Result Ststus-" + objResponce.IsResultTrue + ",ChallengeId-" + objResponce != null && objResponce.jsonData != null ? objResponce.jsonData.ChallengeId.ToString() : "" + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
