using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for response of Program Detail
    /// </summary>
    public class ProgramDetailVM
    {
        public int ProgramId { get; set; } 

        public string ProgramName { get; set; } 

        public string DifficultyLevel { get; set; }

        public string ProgramType { get; set; }      

        public int Strenght { get; set; }   
           
        public string ProgramDescription { get; set; }

        public List<ProgramWeekWorkoutVM> WeekWorkouts { get; set; }      
           
        public int? CreatedByTrainerId { get; set; }

        public string CreatedByTrainerName { get; set; }

        public string CreatedByProfilePic { get; set; }

        public string CreatedByUserType { get; set; }

        public string Description { get; set; }      
         
        public string ProgramDuration { get; set; }

        public string ProgramWorkouts { get; set; }

        public bool IsShowChallengeFriendToUser { get; set; }

        public string ProgramImageUrl { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public bool IsSubscription { get; set; }

        public string ProgramLink { get; set; }     
        
    }  
}