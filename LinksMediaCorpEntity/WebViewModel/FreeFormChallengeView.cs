namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Free Form Challenge View in Admin
    /// </summary>
    public class FreeFormChallengeView
    {
        public IEnumerable<FreeFormChallengeVM> FreeFormChallenges { get; set; }

        public bool isAdminUser { get; set; } 
    }
}