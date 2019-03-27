using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
   /// <summary>
    /// Class for Response of MainChallenge
   /// </summary>
    public class MainChallengeVM
    {
        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public string Description { get; set; }   
             
        public string DifficultyLevel { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; }  

        public string TargetZone { get; set; }

        public int Strenght { get; set; }

        public string ResultUnit { get; set; }

        public bool IsWellness { get; set; } 

        public string ProgramImageUrl { get; set; }

        public bool IsActive { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public List<ExerciseVM> Excercises { get; set; }

        public bool IsSubscription { get; set; }

        public List<int> ChallengeCategoryList { get; set; }

        public int TrainerId { get; set; }
    }   
}