namespace LinksMediaCorp.Controllers.API
{
    using LinksMediaCorp.Filters;
    using LinksMediaCorpBusinessLayer;
    using LinksMediaCorpEntity;
    using LinksMediaCorpUtility;
    using System;
    using System.Text;
    using System.Web.Http;

    [TokenValidationFilter]
    public class SponsorChallengeController : ApiController
    {
        /// <summary>
        /// Action to get details of sponsor challenge
        /// </summary>
        /// <returns>ServiceResponse<SponsorChallengeDetailVM></returns>
        /// <devdoc>
        /// Developer Name - Arvind Kumar
        /// Date - 04/01/2015
        /// </devdoc>
        /// 
        [HttpPost]
        public IHttpActionResult SponsorChallengeDetails(MainChallengeVM model)
        {
            StringBuilder traceLog = null;
            ServiceResponse<SponsorChallengeDetailVM> objResponce = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: SponsorChallengeDetails() Request ChallengeId-" + model.ChallengeId);
                objResponce = new ServiceResponse<SponsorChallengeDetailVM>();
                objResponce.jsonData = SponsorChallengeBL.GetSponsorChallengeDetails(model.ChallengeId);
                objResponce.IsResultTrue = true;
                return Ok(objResponce);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                traceLog.AppendLine("End:SponsorChallengeDetails() Response Result Ststus-" + objResponce.IsResultTrue + ",Challenge Name-" + objResponce.jsonData != null ? objResponce.jsonData.ChallengeName : "" + ",Fetch DateTime-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }
    }
}
