namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Trending Challenge
    /// </summary>
    public class TrendingChallengeResponse
    {
        public int ChallengeId { get; set; }

        public string TrendingImageUrl { get; set; }

        public TrendingType TrendingType { get; set; }

        public int ChallengeSubTypeId { get; set; }
    }
}
