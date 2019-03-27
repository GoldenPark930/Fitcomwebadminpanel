namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for  Resonse of Fittness Completed Challenge
    /// </summary>
    public class CompletedChallengeVM
    {
        public int ChallengeId { get; set; }

        public int UserChallengeId { get; set; } 

        public int ResultId { get; set; } 

        public string ChallengeName { get; set; }

        public string ChallengeType { get; set; }

        public string DifficultyLevel { get; set; }

        public bool IsActive { get; set; }    
           
        public string PersonalBestResult { get; set; }

        public bool IsWellness { get; set; }

        public int ChallengeSubTypeId { get; set; } 
      
    }
}