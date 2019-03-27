using System;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Profile detail for user/trainer
    /// </summary>
    public class ProfileDetails
    {
        public string UserName { get; set; }

        public string UserType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int UserId { get; set; }

        public int TrainerId { get; set; }

        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public bool IsDefaultTeam { get; set; }

        public string ProfileImageUrl { get; set; }

        public string TeamProfileUrl { get; set; }

        public int PendingChallengeCount { get; set; }

        public int CompletedChallengeCount { get; set; }

        public int NotificationCount { get; set; }

        public bool IsJoin_team_notification { get; set; }

        public bool IsTrainer_challenge_notification { get; set; }

        public bool IsFriend_challenge_notification { get; set; }

        public int FollowingCount { get; set; }

        public int FollowerCount { get; set; }

        public string UserBrief { get; set; }

        public int TeamMemberCount { get; set; }

        public bool isJoinedTeam { get; set; }

        public string Zipcode { get; set; }

        public Bio BioData { get; set; }

        public DateTime? DateofBirth { get; set; }

        public int ProgramActiveCount { get; set; }

        public bool IsPersonalTrainer { get; set; }

        public int UserPersoanlTrainerCredId { get; set; }

        public int OffLineChatCount { get; set; }

        public string TeamPremiumpicUrl { get; set; }

    }
}
