using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Category ,Featured  and Trending Respnse List
    /// </summary>
    public class CategoryResponse
    {
        public List<ChallengeCategory> ChallengeCategoryList { get; set; }

        public List<FeaturedResponse> FeaturedList { get; set; }

        public List<TrendingResponse> TrendingCategoryList { get; set; }
    }
}
