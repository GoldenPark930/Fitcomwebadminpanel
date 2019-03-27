namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Class for Response of MyTeamHome
    /// </summary>
    public class MyTeamHomeVM
    {       
        public bool IsMoreAvailable { get; set; }

        public TeamsVM Team { get; set; } 

        public List<RecentResultVM> TeamRecentChallenge { get; set; }

        public ViewPostVM TrainerLatestActivity { get; set; }
    }    
}