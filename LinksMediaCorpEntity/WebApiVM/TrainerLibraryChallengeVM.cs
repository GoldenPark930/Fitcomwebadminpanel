using System.Collections.Generic;
namespace LinksMediaCorpEntity
{ 
    /// <summary>
    /// classs for Response of TrainerLibrary Challenge
    /// </summary>
    public class TrainerLibraryChallengeVM
    {
        public bool IsMoreAvailable { get; set; }

        public List<ProgramVM> ChallengeList { get; set; }
    }
}
