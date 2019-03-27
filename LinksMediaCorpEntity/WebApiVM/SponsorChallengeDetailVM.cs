namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// classs for Response of Sponsor Challenge Detail
    /// </summary>
    public class SponsorChallengeDetailVM
    {
        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public int PersonalBestUserId { get; set; }

        public string PersonalBestUserName { get; set; }

        public string DifficultyLevel { get; set; }

        public string ChallengeType { get; set; }

        public string Equipment { get; set; }

        public List<string> TargetZone { get; set; }

        public List<string> TempEquipments { get; set; }  

        public int Strenght { get; set; }

        public string ResultUnit { get; set; }

        public string ResultUnitSuffix { get; set; }

        public string ChallengeDescription { get; set; }

        public List<ExerciseVM> Excercises { get; set; }

        public string HypeVideo { get; set; }

        public string Result { get; set; }

        public string VariableValue { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public string VariableUnit { get; set; }

        public int CreatedByTrainerId { get; set; }

        public string CreatedByTrainerName { get; set; }

        public string CreatedByProfilePic { get; set; }

        public string CreatedByUserType { get; set; }
    }
}