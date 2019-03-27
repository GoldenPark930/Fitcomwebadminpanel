namespace LinksMediaCorpEntity
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using LinksMediaCorpUtility.Resources;
    using System.Collections.Generic;
    /// <summary>
    /// Classs is added for create team in admin
    /// </summary>
    public class CreateTeamVM
    {
        public int TeamId { get; set; }

        [Required]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateEmail", ErrorMessage = null)]
        public string CheckEmail { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateUser", ErrorMessage = null)]
        public string CheckUser { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "DuplicateTeam", ErrorMessage = null)]
        public string CheckTeam { get; set; }

        [Required]
        [Range(00001, 99999, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidZip", ErrorMessage = null)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [DisplayName("Team Name")]
        public string TeamName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Entered phone number is not valid.")]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?\d{1,2}[ -]?)?(\(\+?\d{2,3}\)|\+?\d{2,3})?[ -]?\d{3,4}[ -]?\d{3,4}$", ErrorMessage = "Entered phone number is not valid.")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidEmail", ErrorMessage = null)]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }

        [ValidateFile]
        public string ProfileImageUrl { get; set; }

        [ValidateFile]
        public string PremiumImageUrl { get; set; }

        public Nullable<bool> LoggedIn { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        [RegularExpression(@"^[\S]*$", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidPasswordLength", ErrorMessage = null)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public int CreatedBy { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Sign Up Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string CropProfileImageRowData { get; set; }

        public string CropPremiumImageRowData { get; set; }

        public bool IsDefaultTeam { get; set; }

        public int UniqueTeamId { get; set; }

        [Required]
        [DisplayName("Onboarding Video")]
        public string OnboardingVideo { get; set; }

        public int OnboardingExeciseVideoId { get; set; }

        public string OnboardingVideoUrl { get; set; }

        public string OnboardingVideoLink { get; set; }

        [DisplayName("Fitness Test 1 ")]
        public string FitnessTest1 { get; set; }

        public int FitcomtestChallengeId1 { get; set; }

        public string FitnessTestUrl1 { get; set; }

        public string FitnessTestLink1 { get; set; }
        
        [DisplayName("Fitness Test 2 ")]
        public string FitnessTest2 { get; set; }

        public int FitcomtestChallengeId2 { get; set; }

        public string FitnessTestUrl2 { get; set; }

        public string FitnessTestLink2 { get; set; }

        [DisplayName("Beginner Program")]
        public string BeginnerProgram { get; set; }

        public int BeginnerProgramId { get; set; }

        public string BeginnerProgramUrl { get; set; }

        public string BeginnerProgramLink { get; set; }

        [DisplayName("Adv/Int Program")]
        public string AdvIntProgram1 { get; set; }

        public int AdvIntProgramId1 { get; set; }

        public string AdvIntProgramUrl1 { get; set; }

        public string AdvIntProgramLink1 { get; set; }

        [DisplayName("Adv/Int Program")]
        public string AdvIntProgram2 { get; set; }

        public int AdvIntProgramId2 { get; set; }

        public string AdvIntProgramUrl2 { get; set; }

        public string AdvIntProgramLink2 { get; set; }

        [DisplayName("Adv/Int Program")]
        public string AdvIntProgram3 { get; set; }

        public int AdvIntProgramId3 { get; set; }

        public string AdvIntProgramUrl3 { get; set; }

        public string AdvIntProgramLink3 { get; set; }

        public IList<TrendingCategory> AvailableWorkoutTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdWorkoutTrendingCategory { get; set; }

        public PostedTrendingCategory PostedWorkoutTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableProgramTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdProgramTrendingCategory { get; set; }

        public PostedTrendingCategory PostedProgramTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableFitnessTestTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdFitnessTestTrendingCategory { get; set; }

        public PostedTrendingCategory PostedFitnessTestTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableSecondaryWorkoutTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryWorkoutTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryWorkoutTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableSecondaryProgramTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryProgramTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryProgramTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableSecondaryFitnessTestTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryFitnessTestTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryFitnessTestTrendingCategory { get; set; }

        public string CropNutrition1ImageRowData { get; set; }

        [Required]
        [DisplayName("Nutrition1 pic image")]
        public string Nutrition1ImageUrl { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Nutrition1 Hyper Link")]
        public string Nutrition1HyerLink { get; set; }

        public string CropNutrition2ImageRowData { get; set; }

        public string Nutrition2ImageUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Nutrition2 Hyper Link")]
        public string Nutrition2HyerLink { get; set; }

        [DisplayName("Show No Trainer Workouts & Program")]
        public bool IsShownNoTrainerWorkoutPrograms { get; set; }

        public bool IsShownNoTrainerFitnessTests { get; set; }


        public string Website { get; set; }

        public int LevelTeamId1 { get; set; }
        public int LevelTeamId2 { get; set; }

        public string GuidRecordId { get; set; }

       

        public decimal PrimaryCommissionRate { get; set; }

        public decimal Level1CommissionRate { get; set; }
        public decimal Level2CommissionRate { get; set; }
    }

}
