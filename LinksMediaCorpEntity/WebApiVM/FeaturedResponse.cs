
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Featured Response DTO class
    /// </summary>
    public class FeaturedResponse
    {
        public int ChallengeId { get; set; }

        public string FeaturedImageUrl { get; set; }

        public FeaturedType FeaturedType { get; set; }

        public int ChallengeSubTypeId { get; set; }

        public string ChallengeName { get; set; }

        public bool IsActiveProgram { get; set; }
    }
    
}
