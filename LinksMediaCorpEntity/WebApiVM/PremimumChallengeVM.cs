using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Premimum Challenge
    /// </summary>
    public class PremimumChallengeVM
    {
        public List<MainChallengeVM> PremimumChallegeList { get; set; }

        public List<ChallengeCategory> PremimumWorksoutList { get; set; }

        public List<ChallengeCategory> PremimumTypeList { get; set; }

        public List<FeaturedResponse> FeaturedList { get; set; }

        public List<TrendingResponse> TrendingList { get; set; } 
    }
}