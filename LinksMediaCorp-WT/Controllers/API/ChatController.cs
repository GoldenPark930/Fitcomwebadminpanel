using LinksMediaCorp.Filters;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpEntity;
using LinksMediaCorpEntity.WebApiVM;
using LinksMediaCorpUtility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace LinksMediaCorp.Controllers.API
{
    [TokenValidationFilter]
    public class ChatController : ApiController
    {
        /// <summary>
        /// Get ChatHistory based on sebder and Receiver Id
        /// </summary>
        /// <param name="chatRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetChatHistory(NumberOfRecord<ChatHistoryRequest> chatRequest)
        {
            StringBuilder traceLog = null;
            ServiceResponse<Total<List<SocketSentChatVM>>> objResponse = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: GetChatHistory with" + chatRequest.Param.SenderCredId);
                objResponse = new ServiceResponse<Total<List<SocketSentChatVM>>>();
                objResponse.jsonData = ChatHistoryApiBL.GetChatHistory(chatRequest.Param, chatRequest.StartIndex, chatRequest.EndIndex);
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
                traceLog.AppendLine("End:GetChatHistory() Response Result Status-" + objResponse.IsResultTrue + ",Fetched DateTime" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }
}