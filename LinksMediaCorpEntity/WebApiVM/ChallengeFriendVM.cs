namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Class for Challenge Friend Request for fittness,Workou and Wellness
    /// </summary>
    public class ChallengeFriendVM
    {
        public int ChallengeId { get; set; }

        public List<FriendVM> FriendList { get; set; }

        public bool IsAcceptChallenge { get; set; }

        public bool IsSelecAllTMembers { get; set; }

        public bool IsProgram { get; set; }   

        public List<FriendVM> TeamUnselectedList { get; set; }

        public string PersonalMessage { get; set; }      
    }
}