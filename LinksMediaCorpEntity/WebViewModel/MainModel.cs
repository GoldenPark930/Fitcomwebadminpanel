namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// classs for get Main list view of challenges
    /// </summary>
    public class MainModel
    { 
        public ChallengesData ChallengesList { get; set; }
        public ChallengesData ProgramList { get; set; } 

        public IEnumerable<ViewTrainers> TrainerList { get; set; }

        public IEnumerable<CreateUserVM> UserList { get; set; }

        public IEnumerable<ViewActivitiesVM> ActivityList { get; set; }
        
    }
}