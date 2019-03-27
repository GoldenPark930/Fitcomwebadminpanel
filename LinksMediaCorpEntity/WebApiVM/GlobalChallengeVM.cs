namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of Global Challenge
    /// </summary>
    public class GlobalChallengeVM
    {
        public int ResultId { get; set; }

        public int ChallengeId { get; set; }  
               
        public string ImageUrl { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public float TempOrderIntValue { get; set; }

        public int UserId { get; set; }

        public int UserCredId { get; set; } 

        public string UserType { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public int Rank { get; set; }

        public string SuperScript { get; set; }

        public string ResultUnit { get; set; }

        public string ResultUnitSuffix { get; set; }

        public int ResultTrainerId { get; set; }

        public string TempGlobalResultFilterIntValue { get; set; }

        public int BoomsCount { get; set; }

        public bool IsLoginUserBoom { get; set; }

        public int CommentsCount { get; set; }

        public bool IsLoginUserComment { get; set; }

        public string ChallengeType { get; set; }

        public bool IsWellness { get; set; }  
    }
}