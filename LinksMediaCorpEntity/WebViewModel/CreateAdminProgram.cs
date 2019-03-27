using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Create Program in Admin
    /// </summary>
    public class CreateAdminProgram
    {
    
        public string DifficultyLevel { get; set; }

        public int ChallengeId { get; set; }

        public int SelectedChallengeTypeId { get; set; }

        public int? TrainerId { get; set; }

        public int? TrainerCredId { get; set; }   
             
        public int? WorkoutTrainerId1 { get; set; }  

        public int? WorkoutTraingZoneId1 { get; set; }

        public int? WorkoutDifficultyLevelId1 { get; set; }


        public int Page { get; set; }

        public string SortField { get; set; }

        public string Sortdir { get; set; }

        public string FormSubmitType { get; set; }

        [Required]
        [DisplayName("Week 1 Name")]
        public string ProgramWeekWorkout1 { get; set; }

        public string ProgramWorkoutLink1 { get; set; }

        public string ProgramWorkouts { get; set; }    
            
        [Required]
        [DisplayName("Program Name")]
        public string ProgramName { get; set; }     
           
        [Required]
        [AllowHtml]
        [DisplayName("Program Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Program Type")]
        public int ProgramType { get; set; } 

        [Required]
        [DisplayName("Program Sub-Type")]
        public int ProgramSubTypeId { get; set; } 

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }       

        public bool IsActive { get; set; }

        public bool IsPremium { get; set; }

        public bool IsSubscription { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsDraft { get; set; } 

        public string EndUserName { get; set; }    

        public string ProgramWeekWorkoutList { get; set; }

        public string SelectedAllIndex { get; set; }

        [DisplayName("Duration")]       
        public string Duration { get; set; }

        [DisplayName("Workouts")]  
        public string Workouts { get; set; }    
                 
        public int ProgramWeekHidenWorkout1 { get; set; } 

        public string CropImageRowData { get; set; }

        [ValidateFile]
        public string ProgramImageUrl { get; set; }

        public string RemovedWeekWorkouts { get; set; }

        public long IsProgramNewWeek1 { get; set; }

        public long IsProgramNewWeekWorkout1 { get; set; }

        public IList<DDTeams> AvailableTeams { get; set; }

        public IList<DDTeams> SelecetdTeams { get; set; }

        public PostedTeams PostedTeams { get; set; }     
          
        [Required(ErrorMessage = "Please select features image")]       
        public string FeaturedImageRowData { get; set; }    
           
        [ValidateFile]        
        public string FeaturedImageUrl { get; set; }

        public string SelectedFeaturedImageUrl { get; set; }

        public string SelectedTrendingCategoryCheck { get; set; } 

        public IList<TrendingCategory> AvailableTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdTrendingCategory { get; set; }

        public PostedTrendingCategory PostedTrendingCategory { get; set; }

        public IList<TrendingCategory> AvailableSecondaryTrendingCategory { get; set; }

        public IList<TrendingCategory> SelecetdSecondaryTrendingCategory { get; set; }

        public PostedTrendingCategory PostedSecondaryTrendingCategory { get; set; }


        [Required(ErrorMessage = "Please select at least one program category.")]
        public string SelectedChallengeCategoryCheck { get; set; } 

        public IList<ChallengeCategory> AvailableChallengeCategory { get; set; }

        public IList<ChallengeCategory> SelecetdChallengeCategory { get; set; }

        public PostedChallengeCategory PostedChallengeCategory { get; set; }    
        
    }

    
}