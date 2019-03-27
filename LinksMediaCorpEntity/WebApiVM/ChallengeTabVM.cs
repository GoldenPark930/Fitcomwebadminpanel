using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Challenge Tab
    /// </summary>
    public class ChallengeTabVM
    {
        public bool IsMoreAvailable { get; set; }

        public List<MainChallengeVM> ChallengeList { get; set; }   
    }
  
}