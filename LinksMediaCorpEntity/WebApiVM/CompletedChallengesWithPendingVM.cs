using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of Completed Challenges WithPending status
    /// </summary>
    public class CompletedChallengesWithPendingVM
    {
        public List<CompletedChallengeVM> CompletedChallengesList { get; set; }

        public int TotalPendingChallengeList { get; set; }
    }
}