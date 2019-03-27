namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Trianrs chhallenges oin admin
    /// </summary>
    public class TrainerChallenge
    {
        public int IsVerifiedTrainer { get; set; }

        public string TrainerImageURL { get; set; }

        public int TrainerId { get; set; }

        public int ChallengeId { get; set; }

        public string AcceptChallengeBy { get; set; }

        public string ChallengeName { get; set; }

        public int StrengthCount { get; set; }

        public string TrainerName { get; set; }

        public string SponsorName { get; set; }

        public string UserType { get; set; }

        public string TrainerType { get; set; }
    }
}