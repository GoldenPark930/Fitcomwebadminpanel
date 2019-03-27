
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Notification Message
    /// </summary>
    public class NotificationMessageVM
    {
        public string Alert { get; set; }

        public string Sound { get; set; } 

        public string NotificationType { get; set; }

        public int TotlaPendingChallenegs { get; set; }

        public int TotalBudget { get; set; }

        public int UserID { get; set; }    
    }
}