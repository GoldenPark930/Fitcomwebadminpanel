namespace LinksMediaCorpEntity
{    
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// Classs for Update the USer Info in admin
    /// </summary>
    public class UpdateUserVM
    {
        public int UserId { get; set; }   
            
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public int PrimaryTrainerId { get; set; }

        public string UserImageUrl { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateEmail", ErrorMessage = null)]
        public string CheckEmail { get; set; }

         [DateOfBirthFilter(MinAge = 0, MaxAge = 150)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Gender { get; set; }  
           
        [DisplayName("Zip Code")]          
        public string ZipCode { get; set; }

        public int TeamId { get; set; }

        [Required]
        [DisplayName("Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }  
           
        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]    
        public string ConfirmPassword { get; set; }

        public bool IsUserChangePassword { get; set; }

        public bool MTActive { get; set; } 
        
    }
}
