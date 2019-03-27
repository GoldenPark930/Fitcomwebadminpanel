namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for SocketSentChat
    /// </summary>
    public class SocketSentChatVM
    {
        public long ChatHistoryId { get; set; }

        public string SenderEmailId { get; set; }

        public string ReceiverEmailId { get; set; }

        public string Message { get; set; }

        public string TrasactionDateTime { get; set; }

        public bool IsOffine { get; set; }

        public string CeritifcatePath { get; set; }
    }   
}
