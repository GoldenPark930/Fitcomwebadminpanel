namespace LinksMediaCorpEntity
{
    using LinksMediaCorpUtility.Resources;
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Classs for change Password in admin
    /// </summary>
    public class ChangePassword
    {   
        public int Id { get; set; }

        [Required(ErrorMessage="Please enter old password.")]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage="Please enter new password")]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        public string NewPassword { get; set; }  
             
        [System.ComponentModel.DataAnnotations.Compare("NewPassword",ErrorMessage="Confirm password and New password do not match.")]
        public string ConfirmPassword { get; set; }  
         
        public int UserId { get; set; }

        public string UserType { get; set; } 
               
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Please select user.")]
        public int UserCredId { get; set; }

        public bool IsUserChangePassword { get; set; } 
    }
}