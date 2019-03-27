
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Challenge Of The Day in admin
    /// </summary>
    public class ChallengeOfTheDay
    {
        public string ImageURL { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }

        public int ChallengeId { get; set; }

        public string ChallengeName { get; set; }

        public int StrengthCount { get; set; }

        public string Featuring { get; set; }       
    }
}