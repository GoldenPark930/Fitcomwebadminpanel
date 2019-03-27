namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class tblUserNotifications
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationID { get; set; }

        [Required]
        public int SenderCredlID { get; set; }

        [Required]
        public int ReceiverCredID { get; set; }     
         
        public string NotificationType { get; set; }

        public string SenderUserName { get; set; }  

        public bool Status { get; set; }

        public bool IsRead { get; set; }   
              
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ModifiedDate { get; set; }

        public string TokenDevicesID { get; set; }

        public int TargetID { get; set; }

        public int ChallengeToFriendId { get; set; }

        public bool IsFriendChallenge { get; set; }

        public bool IsOnBoarding { get; set; } 
    }
}