
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Notification Sender detail
    /// </summary>
    public class NotificationSenderVM
    {
       public int ChallegeId {get;set;} 

       public int SenderUserID { get; set; } 

       public string SenderUserType { get; set; } 

       public int SenderCredID  {get;set;} 

       public long NotificationID { get; set; }

       public bool IsFriendChallenge { get; set; } 
    }
}