namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Response of Friend user detail
    /// </summary>
    public class FriendVM
    {
        public int UserId { get; set; }

        public int ChallengeToFriendId { get; set; } 

        public int CredUserId { get; set; }

        public string UserName { get; set; }

        public string UserType { get; set; }

        public string ImageUrl { get; set; }

        public string CityName { get; set; }

        public string State { get; set; }

        public string Location { get; set; }

        public bool IsSelect { get; set; }

        public int OrderBy { get; set; }

        public bool IsFriendChallenge { get; set; }  
    }
}