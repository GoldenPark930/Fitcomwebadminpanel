using System.Collections.Generic;

namespace LinksMediaCorpEntity
{
   /// <summary>
    /// Classs for get FittnessTest Challenge in admin
   /// </summary>
    public class FittnessTestChallenge
    {
        public List<MainChallengeVM> FittnessTestChallenges { get; set; }

        public List<BodyPart> BodyPart { get; set; }

        public List<FeaturedResponse> FeaturedList { get; set; }

        public List<TrendingResponse> TrendingCategoryList { get; set; }

    }
}
