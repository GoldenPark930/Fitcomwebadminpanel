namespace LinksMediaCorpEntity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// Classs for get credentail details of user/trainer/team in admin
    /// </summary>
    public class Credentials
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Please enter max 50 character in username")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Please enter max 20 character in passwod")]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        public string OldPassword { get; set; }    
          
        public string ConfirmPassword { get; set; }

        public DateTime? LastLogin { get; set; }

        public string NewPassword { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }  
         
        public bool RememberMe { get; set; }  
            
        public string DeviceID { get; set; }

        public string DeviceUID { get; set; }

        public string DeviceType { get; set; }

        public ExternalLoginProvider LoginProvider { get; set; }   
    }
}