using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Main View Challege Tab Resonse
    /// </summary>
    public class ChallegeTypeDetail
    {
        public string FitcomChallengeName { get; set; }

        public string WorkoutChallengeName { get; set; }

        public string PremiumChallengeName { get; set; }

        public string ProgramChallengeName { get; set; }

        public int ProgramTypeID { get; set; }  

        public TeamsVM PremiumChallengeDetail { get; set; }

        public List<ChallengeCategory> WorksoutList { get; set; }

        public List<ChallengeCategory> ProgramList { get; set; } 

        public List<ChallengeCategory> PremimumTypeList { get; set; }
        
    }
    
}