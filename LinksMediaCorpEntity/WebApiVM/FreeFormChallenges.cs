namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    /// <summary>
    /// Class for Request free form challege
    /// </summary>
    public class FreeFormChallenges
    {
        public int ChallengeId { get; set; }

        public int? TrainerId { get; set; }

        public int? TrainerCredntialId { get; set; }

        [Required]
        [AllowHtml]  
        public string Description { get; set; }

        [Required]
        [DisplayName("Challenge Name")]
        [StringLength(200, MinimumLength = 0)]
        public string ChallengeName { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; }

        public string Duration { get; set; }     

        public bool IsActive { get; set; }      

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string DifficultyLevel { get; set; }    
           
        public int Strenght { get; set; }

        public string ChallengeType { get; set; }

        public bool IsWelness { get; set; }

        public ExeciseVideoDetail ExeciseVideoDetails { get; set; }

        public string ThumbnailUrl { get; set; }   
             
        public string ThumbNailHeight { get; set; }

        public string ThumbNailWidth { get; set; }

        public string ExeciseVideoLink { get; set; }

        public List<int> NoTrainerWorkoutTeamList { get; set; }

        public bool IsSubscription { get; set; }

        public List<int> ChalengeCategoryList { get; set; } 
    }
}