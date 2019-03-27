using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// class for get response for Challenges 
    /// </summary>
    public class ChallengesData
    {
        private List<ViewChallenes> challengesViewData;

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public int CurrentPageIndex { get; set; }

        public int SelectedTrainerId { get; set; }

        public List<ViewChallenes> ChallengesViewData { get { return challengesViewData; } }

        /// <summary>
        /// Set Challenges ViewData
        /// </summary>
        /// <param name="challenegeData"></param>
        public void SetChallengesViewData(List<ViewChallenes> challenegeData)
        {
            challengesViewData = new List<ViewChallenes>();
            challengesViewData = challenegeData;
        }
    }
}
