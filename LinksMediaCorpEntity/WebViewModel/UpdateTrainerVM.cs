namespace LinksMediaCorpEntity
{   
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using LinksMediaCorpUtility.Resources; 
    /// <summary>
    /// Classs for Update ther Trainer in admin
    /// </summary>
    public class UpdateTrainerVM
    {
        private List<Specialization> selectedSecondarySpecializations;
        private List<Specialization> selectedTopthreeSpecializations;
        private List<Specialization> availableSpecializations;
        private List<DDTeams> availableTeams;
        private List<DDTeams> selecetdTeams;

        public int TrainerId { get; set; }

        [DisplayName("Trainer ID")]      
        public int EnteredTrainerID { get; set; } 
           
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

         [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateEmail", ErrorMessage = null)]
        public string CheckEmail { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateTeam", ErrorMessage = null)]
        public string CheckTeam { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [DateOfBirthFilter(MinAge = 0, MaxAge = 150)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public string Gender { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHeight", ErrorMessage = null)]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please enter valid Height in Inches")]
        public string Height { get; set; }

         [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please enter valid Weight in lbs")]
        public string Weight { get; set; }

        //[Required]
        [DisplayName("Team Name")]
        public int? TeamId { get; set; }

        public int? PrimaryTeamId { get; set; }

        public bool IsDefaultTeam { get; set; } 

        [Required]
        [Range(00001, 99999, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidZip", ErrorMessage = null)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        //[Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }

        [AllowHtml]
        public string AboutMe { get; set; }

        [ValidateFile]
        public HttpPostedFileBase ProfileUrl { get; set; }

        public string TrainerImageUrl { get; set; }

        [Required(ErrorMessage = "Please select atleast one primary specialization.")]
        public string PrimarySpecializationCheck { get; set; } 
              
        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]     
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateUser", ErrorMessage = null)]
        public string CheckUser { get; set; }

        public string ActivityId { get; set; }

        public PostedTeams PostedMobileCoachTeams { get; set; }

        public IList<DDTeams> SelecetdMobileCoachTeams { get; set; }

        public Nullable<bool> LoggedIn { get; set; }    
           
        public IList<Specialization> AvailableSpecializations 
        {
            get
            {
                return availableSpecializations;
            }
        }

        public void SetAvailableSpecializations(List<Specialization> avlSpecializatio)  
        {
            availableSpecializations = new List<Specialization>();
            availableSpecializations = avlSpecializatio;
        }  
             
        public IList<Specialization> SelectedTopthreeSpecializations 
        {
            get
            {
                return selectedTopthreeSpecializations;
            }
        }     
           
        public IList<Specialization> SelectedSecondarySpecializations
        {
            get
            {
                return selectedSecondarySpecializations;
            }
        }
              
        public PostedSpecializations PostedSpecializations { get; set; }     

        public IList<DDTeams> AvailableTeams
        {
            get
            {
                return availableTeams;
            }
        }         
   

        public IList<DDTeams> SelecetdTeams
        {
            get
            {
                return selecetdTeams;
            }
        }   
            
        public PostedTeams PostedTeams { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifyBy { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public bool IsChangePassword { get; set; }

        [StringLength(16, MinimumLength = 0, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidTrainerTypeLength", ErrorMessage = null)]
        [DisplayName("Trainer Type")]
        public string TrainerType { get; set; }

        public int? ImageCropWidth { get; set; }

        public int? ImageCropHeight { get; set; }

        public int? CropPointX { get; set; }

        public int? CropPointY { get; set; }

        public string CropImageRowData { get; set; }

        /// <summary>
        /// Set Selecetd Teams
        /// </summary>
        /// <param name="sltTeam"></param>
        public void SetSelecetdTeams(List<DDTeams> sltTeam)
        {
            selecetdTeams = new List<DDTeams>();
            selecetdTeams = sltTeam;
        }

        /// <summary>
        /// Set Available Teams
        /// </summary>
        /// <param name="avlTeam"></param>
        public void SetAvailableTeams(List<DDTeams> avlTeam)
        {
            availableTeams = new List<DDTeams>();
            availableTeams = avlTeam;
        }

        /// <summary>
        /// Set Selected Secondary Specializations
        /// </summary>
        /// <param name="sltSpecialization"></param>
        public void SetSelectedSecondarySpecializations(List<Specialization> sltSpecialization)
        {
            selectedSecondarySpecializations = new List<Specialization>();
            selectedSecondarySpecializations = sltSpecialization;
        }

        /// <summary>
        /// Set Selected Topthree Specializations
        /// </summary>
        /// <param name="sltSpecialization"></param>
        public void SetSelectedTopthreeSpecializations(List<Specialization> sltSpecialization)
        {
            selectedTopthreeSpecializations = new List<Specialization>();
            selectedTopthreeSpecializations = sltSpecialization;
        }
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Entered phone number is not valid.")]
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,2}[ -]?)?(\(\+?\d{2,3}\)|\+?\d{2,3})?[ -]?\d{3,4}[ -]?\d{3,4}$", ErrorMessage = "Entered phone number is not valid.")]

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }
    }
}