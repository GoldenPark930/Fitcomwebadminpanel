namespace LinksMediaCorpEntity
{
    /// <summary>
    /// classs for get Execise Challenge Execise Assisociation in admin
    /// </summary>
    public class ExeciseChallengeCEVM
    {
        public string ExeName { get; set; }

        public string ExeDesc { get; set; }

        public int ChallengeId { get; set; }

        public int UserId { get; set; }

        public int? Reps { get; set; }

        public int? WeightForMan { get; set; }

        public int? WeightForWoman { get; set; }

        public int ExeciseId { get; set; }
    }
}
