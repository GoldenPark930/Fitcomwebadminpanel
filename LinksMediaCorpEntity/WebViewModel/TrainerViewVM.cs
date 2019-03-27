namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for trainer detail in admin
    /// </summary>
    public class TrainerViewVM
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashTag { get; set; }
        public int IsVerifiedTrainer { get; set; }
        public string TrainerImageURL { get; set; }
        public int TeamMemberCount { get; set; }
        public int FollowingCount { get; set; }
        public int FollowerCount { get; set; }
        public Bio BioData { get; set; }
        public bool IsJoinTeam { get; set; }
        public bool IsFollowByLoginUser { get; set; }
        public bool IsMyTeam { get; set; }
        public string TrainerType { get; set; }
        public bool IsDefaultTeam { get; set; }
        public string EmailId { get; set; }
        
    }
}
    