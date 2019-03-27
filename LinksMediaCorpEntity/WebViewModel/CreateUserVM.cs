namespace LinksMediaCorpEntity
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using LinksMediaCorpUtility.Resources;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// classs for create user in admin
    /// </summary>
    public class CreateUserVM
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateEmail", ErrorMessage = null)]
        public string CheckEmail { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateUser", ErrorMessage = null)]
        public string CheckUser { get; set; }

        [DateOfBirthFilter(MinAge = 0, MaxAge = 150)] 
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only")]
        public string Gender { get; set; } 
             
        [DisplayName("Zip Code")]      
        public string ZipCode { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }      

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        [Compare("Password")]        
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [DisplayName("Team Name")]
        public string TeamName { get; set; }

        public string UserName { get; set; }

        public int UniqueTeamId { get; set; }

        public bool MTActive { get; set; }

        public string MTActiveStatus { get; set; }

        public int TeamId { get; set; }

        public bool IsPremiumMember { get; set; }
        public string PremiumMemberStatus { get; set; }

        public bool SubscriptionStatus { get; set; }

        public int  UserCredId { get; set; }

        public string SubscriptionStatusLebel { get; set; }
    }

}