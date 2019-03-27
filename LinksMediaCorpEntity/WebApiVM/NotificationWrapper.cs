
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Notification Wrapper
    /// </summary>   
    public class NotificationWrapper
    {
        public int  ReceiverUserCredId { get; set; } 

        public int ChallengeId { get; set; }      
         
        public string SenderName { get; set; } 

        public int SenderUserId { get; set; }

        public string SenderUserType { get; set; } 

        public byte[] Certificate { get; set; } 

        public string DeviceID { get; set; }     
           
        public string DeviceType { get; set; }     
            
        public string NotificationType { get; set; }

        public int TotalPendingNotification { get; set; }

        public int Totalbudget { get; set; }

        public string TeamName { get; set; }

        public int ChallegeUSerID { get; set; }

        public int TargetID { get; set; } 
    }
}
