namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for get data on Dashboard in admin
    /// </summary>
    public class DashBoard
    {
        public IEnumerable<FeaturedActivityQueue> FauturedActivityQueue { get; set; }

        public IEnumerable<CODQueue> ChallengeOfTheDayQueue { get; set; }

        public IEnumerable<SponsorChallengeQueue> SponsorChallengeQueue { get; set; }

        public DashBoardActivityCount DashBoardActivity { get; set; }
    }

}