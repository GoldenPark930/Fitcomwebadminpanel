using System;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Socket SentChat
    /// </summary>
    public class SocketSentChatResponse
    {
        public long ChatHistoryId { get; set; }

        public string SenderEmailId { get; set; }

        public string ReceiverEmailId { get; set; }

        public string Message { get; set; }

        public DateTime TrasactionDateTime { get; set; }

        public bool IsOffine { get; set; }

        public string CeritifcatePath { get; set; }
    }
}
