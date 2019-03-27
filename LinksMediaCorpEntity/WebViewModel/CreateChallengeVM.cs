namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Web.Mvc;
    using LinksMediaCorpUtility.Resources;
    /// <summary>
    /// classs for create Free form challenge in admin
    /// </summary>
    public class CreateChallengeVM
    {
        public string DifficultyLevel { get; set; }

        public int ChallengeId { get; set; }

        public int SelectedChallengeTypeId { get; set; }

        [Required]
        [DisplayName("Trainer")]
        public int? TrainerId { get; set; }

        public int? TrainerCredntialId { get; set; }

        [Required]
        [DisplayName("Trainer Result")]
        public int? ResultId { get; set; }

        [Required]
        [DisplayName("User Result")]
        public int? UserResultId { get; set; }

        [Required]
        [DisplayName("Exercise 1 Name")]
        public string ExeName1 { get; set; }

        [StringLength(80000, MinimumLength = 0)]
        [AllowHtml]
        [DisplayName("Exercise 1 Description")]
        public string ExeDesc1 { get; set; }

        public string SelectedFitcomEquipment1 { get; set; }

        public string SelectedFitcomTrainingZone1 { get; set; }

        public string SelectedFitcomExeciseType1 { get; set; }

        public string SelectedEquipment1 { get; set; }

        public string SelectedTrainingZone1 { get; set; }

        public string SelectedExeciseType1 { get; set; }

        public string ExeVideoLink1 { get; set; }

        public string ExeIndexLink1 { get; set; }

        public bool IsSetFirstExecise { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        [DisplayName("Variable Value")]
        public string VariableValue { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        [DisplayName("Global Result FilterValue")]
        public string GlobalResultFilterValue { get; set; }

        public string Equipment { get; set; }

        [Required]
        [DisplayName("Challenge Name")]
        [StringLength(200, MinimumLength = 0)]
        public string ChallengeName { get; set; }

        public string TrainerResult { get; set; }

        public string TrainerMainResult { get; set; }

        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Hype Video Link")]
        [StringLength(200, MinimumLength = 0)]
        public string HypeVideoLink { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("Describe Your Challenge")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Challenge Type")]
        public int ChallengeType { get; set; }

        [Required]
        [DisplayName("Challenge Sub-Type")]
        public int ChallengeSubTypeId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ResultTime { get; set; }

        public decimal? ResultWeightorDestance { get; set; }

        [Range(0, 1000, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        public int? ResultRepsRound { get; set; }

        public string ResultFrection { get; set; }

        public bool IsActive { get; set; }

        public bool IsSetToCOD { get; set; }

        public bool IsSetToSponsorChallenge { get; set; }

        public string IsMoreThenOne { get; set; }

        public string VariableUnit { get; set; }

        public string FormSubmitType { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateTime? CODStartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateTime? CODEndDate { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateTime? TCStartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateTime? TCEndDate { get; set; }

        public string EndUserName { get; set; }

        [Required]
        [DisplayName("User")]
        public int? EndUserNameId { get; set; }

        public int EndUserCredntialId { get; set; }

        public string UserResult { get; set; }

        [Required]
        [DisplayName("Video Link")]
        [Url(ErrorMessage = "Please enter a valid url")]
        public string UserVideoLink { get; set; }

        [Required]
        [DisplayName("Video Link")]
        [Url(ErrorMessage = "Please enter a valid url")]
        public string TrainerVideoLink { get; set; }

        public string VariableLimit { get; set; }

        public string ResultUnitType { get; set; }

        [Required]
        [DisplayName("Sponsor Name")]
        public string SponsorName { get; set; }

        public string ChallengeType_Name { get; set; }

        public string ChallengeSubType_Description { get; set; }

        public string ExerciseThumnail { get; set; }

        //Free Form Challenge
        [AllowHtml]
        public string FreeFormExerciseNameDescriptionList { get; set; }

        [Required]
        [DisplayName("Exercise 1 Name")]
        public string FFExeName1 { get; set; }

        public string FFExeVideoLink1 { get; set; }

        [AllowHtml]
        public string FFExeDesc1 { get; set; }

        public string FFExeVideoUrl1 { get; set; }

        public bool IsFFAExeName1 { get; set; }

        [Required]
        [DisplayName("Exercise 1 Alternate Name")]
        public string FFAExeName1 { get; set; }

        public int ExerciseId1 { get; set; }

        public int ExerciseId { get; set; } 

        // End add FreeForm Challenges
        /*Changes for wieght for man and weight for woman*/
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeightForMan", ErrorMessage = null)]
        public int? WeightForMan1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeightForMan", ErrorMessage = null)]
        public int? WeightForWoman1 { get; set; }


        [Range(0, 24, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHours", ErrorMessage = null)]
        public string VariableHours { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMinutes", ErrorMessage = null)]
        public string VariableMinute { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidSeconds", ErrorMessage = null)]
        public string VariableSecond { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMS", ErrorMessage = null)]
        public string VariableMS { get; set; }

        [Range(0, 24, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHours", ErrorMessage = null)]
        public string ResultVariableHours { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMinutes", ErrorMessage = null)]
        public string ResultVariableMinute { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidSeconds", ErrorMessage = null)]
        public string ResultVariableSecond { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMS", ErrorMessage = null)]
        public string ResultVariableMS { get; set; }

        [Range(0, 24, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHours", ErrorMessage = null)]
        public string GlobalResultFilterHours { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMinutes", ErrorMessage = null)]
        public string GlobalResultFilterMinute { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidSeconds", ErrorMessage = null)]
        public string GlobalResultFilterSecond { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMS", ErrorMessage = null)]
        public string GlobalResultFilterMS { get; set; }

        //Added By Arvind
        //[Required(ErrorMessage = "Please select atleast one primary specialization.")]
        public string SelectedExerciseTypeCheck { get; set; }
        public IList<ExerciseType> AvailableExerciseTypes { get; set; }

        public IList<ExerciseType> SelectedExerciseTypes { get; set; }

        public PostedExerciseType PostedExerciseTypes { get; set; }

        public IList<Exercise> AvailableExerciseVideoList { get; set; }


        public string SelectedTargetZoneCheck { get; set; }

        public IList<BodyPart> AvailableTargetZones { get; set; }

        public IList<BodyPart> SelectedTargetZones { get; set; }

        public PostedTargetZone PostedTargetZones { get; set; }


        public IList<DDTeams> AvailableTeams { get; set; }

        public IList<DDTeams> SelecetdTeams { get; set; }

        public PostedTeams PostedTeams { get; set; }

        public string SelectedEquipmentCheck { get; set; }

        public IList<Equipments> AvailableEquipments { get; set; }

        public IList<Equipments> SelectedEquipments { get; set; }

        public PostedEquipment PostedEquipments { get; set; }

        public string SelectedFittnessTrendingCategoryCheck { get; set; }      

      


        public IList<TrendingCategory> AvailableSecondaryTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryTrendingCategory { get; set; } 

        public string ExeVideoUrl1 { get; set; }

        public string ExeVideoUrl2 { get; set; }

        public string ExeVideoUrl3 { get; set; }

        public string ExeVideoUrl4 { get; set; }

        [Required(ErrorMessage = "Please select features image")]
        [DisplayName("Select featured Image")]
        public string CropImageRowData { get; set; }

        public string SelectedAllIndex { get; set; }

        public bool IsPremium { get; set; }

        public bool IsSubscription { get; set; }

        public bool IsFreeFitnessTest { get; set; }

        public bool IsFeatured { get; set; }


        [DisplayName("Challenge Duration")]
        [StringLength(25, MinimumLength = 0)]
        public string FFChallengeDuration { get; set; }

        [AllowHtml]
        [DisplayName("Challenge Detail")]
        public string ChallengeDetail { get; set; }

        public List<ExeciseSetVM> ExeciseSetDaetails { get; set; }

        public int Page { get; set; }

        public string SortField { get; set; }

        public string Sortdir { get; set; }

        public int CEARocordId1 { get; set; }

        public string IsNewAddedExercise1 { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string CheckChallengeName { get; set; }

        [ValidateFile]
        public string FeaturedImageUrl { get; set; }

        public int? TrendingCategoryId { get; set; }

        public int SelectedTrendingTypeId { get; set; }

        public string SelectedTrendingCategoryCheck { get; set; }

        public IList<TrendingCategory> AvailableTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdTrendingCategory { get; set; }

        public PostedTrendingCategory PostedTrendingCategory { get; set; }

        [Required(ErrorMessage = "Please select at least one challenge category.")]
        public string SelectedChallengeCategoryCheck { get; set; }

        public IList<ChallengeCategory> AvailableChallengeCategory { get; set; }

        public IList<ChallengeCategory> SelecetdChallengeCategory { get; set; }

        public PostedChallengeCategory PostedChallengeCategory { get; set; }

    }
}