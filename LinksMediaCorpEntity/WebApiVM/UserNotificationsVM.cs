namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// UserNotificationsVM class is used to send notification to user.
    /// </summary>
    public class UserNotificationsVM
    {
        public long UserNotificationID { get; set; }

        public int ChallengeToFriendId { get; set; }      
           
        public int UserID { get; set; }

        public int ReceiverCredID { get; set; }

        public int SenderCredID { get; set; }    

        public string UserType { get; set; }

        public string SenderUserName { get; set; }

        public string NotificationMessage { get; set; }

        public DateTime CreatedNotificationUtcDateTime { get; set; }

        public string ImageURL { get; set; }  

        public string NotificationType { get; set; }

        public bool Status { get; set; }

        public bool IsRead { get; set; }

        public string TokenDevicesID { get; set; }

        public int TargetPostID { get; set; }

        public int TotalCount { get; set; }      

        public NotificationType UserNotificationType { get; set; }

        public bool IsRemove { get; set; }

        public UserVM UserDetails { get; set; }

        public string DevicesTokenID { get; set; }

        public bool IsActive { get; set; }

        public string SenderEmailID { get; set; }

        public bool IsOnBoarding { get; set; }

        public bool IsFriendChallenge { get; set; }

        public string TeamName { get; set; }        
        
    }
}