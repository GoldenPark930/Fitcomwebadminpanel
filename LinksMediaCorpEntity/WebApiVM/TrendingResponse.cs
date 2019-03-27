namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs Response of Trending Series
    /// </summary>
    public class TrendingResponse
    {
        public int TrendingCategoryId { get; set; } 
        public string TrendingImageUrl { get; set; } 
        public TrendingType TrendingType { get; set; }
        public int ChallengeSubTypeId { get; set; }         
    } 
}
