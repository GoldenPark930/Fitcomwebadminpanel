using System;
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for View Challenges in admin
    /// </summary>
    public class ViewChallenes
    {
        public int TrainerId { get; set; }

        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public string ChallengeDesc { get; set; }

        public string Type { get; set; }

        public string IsActive { get; set; }

        public string TrainerResult { get; set; }

        public string ResultUnit { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; } 

        public List<string> TempEquipments { get; set; }  

        public string DifficultyLevel { get; set; }

        public string VariableValue { get; set; }

        public int Strength { get; set; }

        public string HypeVideoLink { get; set; }

        public bool IsDrafted { get; set; }

        public string Category { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsWorkout { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public List<string> ChallengeCategoryName { get; set; }   
    }
   
}