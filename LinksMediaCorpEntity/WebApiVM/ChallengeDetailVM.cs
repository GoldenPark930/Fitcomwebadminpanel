namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Class for Challenge Detail for workout and Wellness
    /// </summary>
    public class ChallengeDetailVM
    {
        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public string DifficultyLevel { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public string TargetZone { get; set; }

        public List<string> TempTargetZone { get; set; }

        public List<string> TempEquipments { get; set; }  

        public int Strenght { get; set; }

        public string ResultUnit { get; set; }

        public string ChallengeDescription { get; set; }

        public List<ExerciseVM> Excercises { get; set; }

        public string VariableValue { get; set; } 
         
        public int ChallengeSubTypeId { get; set; }

        public string VariableUnit { get; set; }

        public int? CreatedByTrainerId { get; set; }

        public string CreatedByTrainerName { get; set; }

        public string CreatedByProfilePic { get; set; }

        public string CreatedByUserType { get; set; }

        public string Description { get; set; }

        public bool IsShowChallengeFriendToUser { get; set; }

        public string ChallengeDuration { get; set; }

        public string ChallengeDetail { get; set; }

        public bool IsWellness { get; set; }

        public string PersonalBestResult { get; set; }

        public string LatestResult { get; set; }

        public bool IsSubscription { get; set; }

        public string ChallengeLink { get; set; } 
        
    }
}