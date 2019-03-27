using System;
namespace LinksMediaCorpEntity
{   
    /// <summary>
    /// Classs for  Resonse for Chat History
    /// </summary>
    public class ChatHistoryVM
    {
        public int SenderCredId { get; set; }

        public int ReceiverCredId { get; set; }

        public string Message { get; set; }

        public Nullable<DateTime> TrasactionDateTime { get; set; }
    }
}
