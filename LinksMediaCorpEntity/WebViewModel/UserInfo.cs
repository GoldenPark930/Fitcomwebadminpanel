namespace LinksMediaCorpEntity
{
    using LinksMediaCorpUtility.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Classs for add/get user details in admin
    /// </summary>
    public class UserInfo
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 0)]
        public string FirstName { get; set; }

        public int PrimaryTrainerId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 0)]
        public string LastName { get; set; }

       
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

       
        public string Gender { get; set; }

       
        public string ZipCode { get; set; }

        
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [StringLength(50, MinimumLength = 0)]
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        public string UserImageUrl { get; set; }

        public bool IsMailReceive { get; set; }

       
        public string DeviceID { get; set; }

        public string DeviceUID { get; set; } 

        public string DeviceType { get; set; }  
    }
}