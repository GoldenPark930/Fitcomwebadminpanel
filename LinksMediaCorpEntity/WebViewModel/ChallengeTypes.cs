namespace LinksMediaCorpEntity
{
    /// <summary>
    /// class for get challenge type for free form type
    /// </summary>
    public class ChallengeTypes
    {
        public int ChallengeSubTypeId { get; set; }

        public string ChallengeSubType { get; set; }

        public string ChallengeType { get; set; }

        public int MaxLimit { get; set; }

        public string Unit { get; set; }

        public string IsExerciseMoreThanOne { get; set; }

        public string ResultUnit { get; set; }

        public bool IsActive { get; set; } 
    }
   
}