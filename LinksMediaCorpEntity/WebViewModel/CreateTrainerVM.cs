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
    /// Classs for create Trainer in admin
    /// </summary>
    public class CreateTrainerVM
    {
        private List<Specialization> availableSpecializations;
        private List<Specialization> selectedSpecializations;
        private List<DDTeams> selecetdTeams;
        private List<DDTeams> availableTeams;
        private List<Specialization> selectedTopthreeSpecializations;
        private List<Specialization> selectedSecondarySpecializations;

        public int TrainerId { get; set; }

        [DisplayName("Trainer ID")]
        public int? EnteredTrainerID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateEmail", ErrorMessage = null)]
        public string CheckEmail { get; set; } 
           
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateTeam", ErrorMessage = null)]
        public string CheckTeam { get; set; }

        [DateOfBirthFilter(MinAge = 0, MaxAge = 150)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public string Gender { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHeight", ErrorMessage = null)]
        public string Height { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public string Weight { get; set; }

        [Required]
        [Range(00001, 99999, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidZip", ErrorMessage = null)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; } 
             
        [DisplayName("Team Name")]
        public int? TeamId { get; set; }
        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidEmail", ErrorMessage = null)]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }

        [AllowHtml]
        public string AboutMe { get; set; }

        [ValidateFile]
        public string TrainerImageUrl { get; set; }

        [Required(ErrorMessage = "Please select atleast one primary specialization.")]
        public string PrimarySpecializationCheck { get; set; }

        public IEnumerable<HttpPostedFileBase> Pics { get; set; }

        public IEnumerable<HttpPostedFileBase> Videos { get; set; }

        public string ActivityId { get; set; }

        public Nullable<bool> LoggedIn { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordWhiteSpace", ErrorMessage = null)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public PostedSpecializations PostedSpecializations { get; set; }
        public PostedTeams PostedTeams { get; set; }

        public PostedTeams PostedMobileCoachTeams { get; set; }

        public IList<DDTeams> SelecetdMobileCoachTeams { get; set; } 

        public IList<Specialization> AvailableSpecializations
        {
            get
            {
                return availableSpecializations;
            }
        }

        public IList<Specialization> SelectedSpecializations
        {
            get
            {
                return selectedSpecializations;
            }
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

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public List<State> States { get; set; }

        [ValidateFile]
        public HttpPostedFileBase ProfileUrl { get; set; }

        public Files TrainerProfileUrl { get; set; }

        public IList<Files> TrainerVideos { get; set; }

        public IList<Files> TrainerPics { get; set; }

        [StringLength(16, MinimumLength = 0, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidTrainerTypeLength", ErrorMessage = null)]
        [DisplayName("Trainer Type")]
        public string TrainerType { get; set; }

        public int? ImageCropWidth { get; set; }

        public int? ImageCropHeight { get; set; }

        public int? CropPointX { get; set; }

        public int? CropPointY { get; set; }

        public string CropImageRowData { get; set; }

        /// <summary>
        /// Set Selected Topthree Specializations
        /// </summary>
        /// <param name="sltSpecialization"></param>
        public void SetSelectedTopthreeSpecializations(List<Specialization> sltSpecialization)
        {
            selectedTopthreeSpecializations = new List<Specialization>();
            selectedTopthreeSpecializations = sltSpecialization;
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
        /// Set Selected Specializations
        /// </summary>
        /// <param name="avlSpecializatio"></param>
        public void SetSelectedSpecializations(List<Specialization> avlSpecializatio)
        {
            selectedSpecializations = new List<Specialization>();
            selectedSpecializations = avlSpecializatio;
        }

        /// <summary>
        /// Set Available Specializations
        /// </summary>
        /// <param name="avlSpecializatio"></param>
        public void SetAvailableSpecializations(List<Specialization> avlSpecializatio)
        {
            availableSpecializations = new List<Specialization>();
            availableSpecializations = avlSpecializatio;
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