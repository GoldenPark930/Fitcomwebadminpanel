namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// class for get Home data resonse on Admin Dashboard
    /// </summary>
    public class HomeData
    {
        public string UserType { get; set; }

        public string UserName { get; set; }

        public string SponsorChallengeLebel { get; set; }

        public string ChallengeOfTheDayLabel { get; set; }

        public int UserId { get; set; }

        public string UserImageURL { get; set; }

        public TeamsVM MyTeamInfo { get; set; } 

        public List<ChallengeOfTheDay> ChallengeOfTheDayInfoList { get; set; }

        public List<TrainerChallenge> TrainerChallengeInfoList { get; set; }

        public List<FeaturedActivity> FeaturedActivityInfoList { get; set; }

        public NotificationSettingVM UserNotificationInfo { get; set; }   

        public ViewPostVM TrainerLatestActivity { get; set; }

        public int TotalPendingChallenges { get; set; }

        public Total<List<ViewPostVM>> PostList { get; set; }
               
    }
   
}