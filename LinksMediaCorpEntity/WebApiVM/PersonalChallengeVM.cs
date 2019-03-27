namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Classss for Response of Personal Challenge
    /// </summary>
    public class PersonalChallengeVM
    {
        public int UserChallengeId { get; set; }

        public string Result { get; set; }

        public string Fraction { get; set; }

        public string CompletedDate { get; set; }

        public DateTime CompletedDateDb { get; set; }

        public int ChallengeSubTypeid { get; set; }

        public string ResultUnit { get; set; }

        public string ResultUnitSuffix { get; set; }

        public float TempOrderIntValue { get; set; }

        public string PersonalBestResult { get; set; }

        public bool IsRecentChallengUserBest { get; set; }

        public string ChallengeType { get; set; }

        public bool IsWellness { get; set; }
    }
}