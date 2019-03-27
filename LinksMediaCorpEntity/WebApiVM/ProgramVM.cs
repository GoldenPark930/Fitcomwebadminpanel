using System;
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Program
    /// </summary>
    public class ProgramVM
    {
        public int ProgramId { get; set; } 

        public int? TrainerId { get; set; }

        public int? TrainerCredntialId { get; set; }     

        public string Description { get; set; }

        public string ProgramName { get; set; } 

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string DifficultyLevel { get; set; }

        public int Strenght { get; set; }

        public string ProgramType { get; set; }

        public string ProgramImageUrl { get; set; }     
          
        public string CreatedByTrainerName { get; set; }

        public string CreatedByProfilePic { get; set; }

        public string CreatedByUserType { get; set; }

        public string Height { get; set; }  

        public string Width { get; set; }

        public bool IsSubscription { get; set; }

        public List<int> ChallengeCategoryList { get; set; }

        public List<int> NoTrainerWorkoutTeamList { get; set; }          
    }
}