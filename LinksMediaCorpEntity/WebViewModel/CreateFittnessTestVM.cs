using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using LinksMediaCorpUtility.Resources;

namespace LinksMediaCorpEntity
{
    /// <summary>
    /// This class is used to creat Fittness Test challenge
    /// </summary>
    public class CreateFittnessTestVM
    {
        private List<BodyPart> availableTargetZones;
        private List<BodyPart> selectedTargetZones;
        private List<ExerciseType> availableExerciseTypes;
        private List<ExerciseType> selectedExerciseTypes;
        private List<Equipments> availableEquipments;
        private List<Equipments> selectedEquipments;

        private List<ChallengeCategory> availableChallengeCategory { get; set; }

        private List<ChallengeCategory> selecetdChallengeCategory { get; set; }

        public string DifficultyLevel { get; set; }

        public int ChallengeId { get; set; }

        public int SelectedChallengeTypeId { get; set; }

        public int? TrainerId { get; set; }

        [Required]
        [DisplayName("Exercise 1 Name")]
        public string ExeName1 { get; set; }

        public string ExeVideoLink1 { get; set; }

        [DisplayName("Exercise 1 Description")]
        [AllowHtml]
        public string ExeDesc1 { get; set; }

       

        public string SelectedFitcomEquipment1 { get; set; }

        public string SelectedFitcomTrainingZone1 { get; set; }

        public string SelectedFitcomExeciseType1 { get; set; }
        
        public int ExerciseId1 { get; set; }
        
        public bool IsSetFirstExecise { get; set; }
        
        public string SelectedEquipment1 { get; set; }

        public string SelectedTrainingZone1 { get; set; }

        public string SelectedExeciseType1 { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        [DisplayName("Variable Value")]
        public string VariableValue { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        [DisplayName("Global Result FilterValue")]
        public string GlobalResultFilterValue { get; set; }

        [Required(ErrorMessage = "Please select at least one Equipment")]
        public string SelectedEquipmentCheck { get; set; }

        public IList<Equipments> AvailableEquipments
        {
            get
            {
                return availableEquipments;
            }
        }

        public IList<Equipments> SelectedEquipments
        {
            get
            {
                return selectedEquipments;
            }
        }
        public PostedEquipment PostedEquipments { get; set; }

        [Required(ErrorMessage = "Please select at least one Target Zone")]
        public string SelectedTargetZoneCheck { get; set; }

        public IList<BodyPart> AvailableTargetZones
        {
            get
            {
                return availableTargetZones;
            }
        }

        public IList<BodyPart> SelectedTargetZones
        {
            get
            {
                return selectedTargetZones;
            }
        }

        public PostedTargetZone PostedTargetZones { get; set; }

        [Required]
        [DisplayName("Challenge Name")]
        public string ChallengeName { get; set; }

        public string TrainerResult { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("Challenge Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Challenge Type")]
        public int ChallengeType { get; set; }

        [Required]
        [DisplayName("Challenge Sub-Type")]
        public int ChallengeSubTypeId { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ResultTime { get; set; }

        public decimal ResultWeightorDestance { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidNumeric", ErrorMessage = null)]
        public int ResultRepsRound { get; set; }

        public string ResultFrection { get; set; }

        public bool IsActive { get; set; }

        public string IsMoreThenOne { get; set; }

        public string VariableUnit { get; set; }

        public string ResultUnitType { get; set; }

        public DateTime? CODStartDate { get; set; }

        public DateTime? CODEndDate { get; set; }

        public DateTime? TCStartDate { get; set; }

        public DateTime? TCEndDate { get; set; }

        public string EndUserName { get; set; }

        public string UserResult { get; set; }

        public string UserVideoLink { get; set; }
        public string VariableLimit { get; set; }

        /*Changes for wieght for man and weight for woman*/
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidReps", ErrorMessage = null)]
        public int? Reps1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public int? WeightForMan1 { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidWeight", ErrorMessage = null)]
        public string WeightForWoman1 { get; set; }

        [Range(0, 24, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHours", ErrorMessage = null)]
        public string VariableHours { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMinutes", ErrorMessage = null)]
        public string VariableMinute { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidSeconds", ErrorMessage = null)]
        public string VariableSecond { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMS", ErrorMessage = null)]
        public string VariableMS { get; set; }

        [Range(0, 24, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidHours", ErrorMessage = null)]
        public string GlobalResultFilterHours { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMinutes", ErrorMessage = null)]
        public string GlobalResultFilterMinute { get; set; }

        [Range(0, 60, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidSeconds", ErrorMessage = null)]
        public string GlobalResultFilterSecond { get; set; }

        [Range(0, 99, ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "InvalidMS", ErrorMessage = null)]
        public string GlobalResultFilterMS { get; set; }

        //Added By Arvind       
        public string SelectedExerciseTypeCheck { get; set; }

        public IList<ExerciseType> AvailableExerciseTypes
        {
            get
            {
                return availableExerciseTypes;
            }
        }
        public IList<ExerciseType> SelectedExerciseTypes
        {
            get
            {
                return selectedExerciseTypes;
            }
        }
        public PostedExerciseType PostedExerciseTypes { get; set; }

        public IList<DDTeams> AvailableTeams { get; set; }

        public IList<DDTeams> SelecetdTeams { get; set; }
        public PostedTeams PostedTeams { get; set; }
        [AllowHtml]
        public string FreeFormExerciseNameDescriptionList { get; set; }
        public string SelectedAllIndex { get; set; }

        [AllowHtml]
        [DisplayName("Challenge Detail")]
        public string ChallengeDetail { get; set; }

        public string SelectedTrendingCategoryCheck { get; set; }

        [Required(ErrorMessage = "Please select at least one challenge category.")]
        public string SelectedChallengeCategoryCheck { get; set; }

        public IList<ChallengeCategory> AvailableChallengeCategory
        {
            get
            {
                return availableChallengeCategory;
            }
        }

        public IList<ChallengeCategory> SelecetdChallengeCategory
        {
            get
            {
                return selecetdChallengeCategory;
            }
        }
        public PostedChallengeCategory PostedChallengeCategory { get; set; }

        public string SelectedFittnessTrendingCategoryCheck { get; set; }

        public IList<TrendingCategory> AvailableFittnessTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdFittnessTrendingCategory { get; set; }
        public PostedTrendingCategory PostedFittnessTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableSecondaryFittnessTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryFittnessTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryFittnessTrendingCategory { get; set; }
        
        /// <summary>
        /// Set Available Equipments
        /// </summary>
        /// <param name="avlEquipments"></param>
        public void SetAvailableEquipments(List<Equipments> avlEquipments)
        {
            availableEquipments = new List<Equipments>();
            availableEquipments = avlEquipments;
        }

        /// <summary>
        /// Set Selected Equipments
        /// </summary>
        /// <param name="avlEquipments"></param>
        public void SetSelectedEquipments(List<Equipments> avlEquipments)
        {
            selectedEquipments = new List<Equipments>();
            selectedEquipments = avlEquipments;
        }

        /// <summary>
        /// Set Available TargetZones
        /// </summary>
        /// <param name="targetZones"></param>
        public void SetAvailableTargetZones(List<BodyPart> targetZones)
        {
            availableTargetZones = new List<BodyPart>();
            availableTargetZones = targetZones;
        }

        /// <summary>
        /// Set Selected TargetZones
        /// </summary>
        /// <param name="targetZones"></param>
        public void SetSelectedTargetZones(List<BodyPart> targetZones)
        {
            selectedTargetZones = new List<BodyPart>();
            selectedTargetZones = targetZones;
        }
        /// <summary>
        /// Set Available ExerciseTypes
        /// </summary>
        /// <param name="avlExerciseTypes"></param>
        public void SetAvailableExerciseTypes(List<ExerciseType> avlExerciseTypes)
        {
            availableExerciseTypes = new List<ExerciseType>();
            availableExerciseTypes = avlExerciseTypes;
        }
        /// <summary>
        /// Set Selected ExerciseTypes
        /// </summary>
        /// <param name="setExerciseTypes"></param>
        public void SetSelectedExerciseTypes(List<ExerciseType> setExerciseTypes)
        {
            selectedExerciseTypes = new List<ExerciseType>();
            selectedExerciseTypes = setExerciseTypes;
        }
        /// <summary>
        /// Set Available Challenge Category
        /// </summary>
        /// <param name="avlChallengeCategory"></param>
        public void SetAvailableChallengeCategory(List<ChallengeCategory> avlChallengeCategory)
        {
            availableChallengeCategory = new List<ChallengeCategory>();
            availableChallengeCategory = avlChallengeCategory;
        }
        /// <summary>
        /// Set Selecetd Challenge Category
        /// </summary>
        /// <param name="sltChallengeCategory"></param>
        public void SetSelecetdChallengeCategory(List<ChallengeCategory> sltChallengeCategory)
        {
            selecetdChallengeCategory = new List<ChallengeCategory>();
            selecetdChallengeCategory = sltChallengeCategory;
        }
    }
}
