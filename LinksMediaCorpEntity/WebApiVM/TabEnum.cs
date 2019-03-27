namespace LinksMediaCorpEntity
{
    public enum TabEnum
    {
        Home=1,
        Challenge=2,
        Team=3,
        NewsFeed=4
    }

    public enum SearchDataTypeEnum 
    {
        User = 1,
        Trainer = 2,
        Challenge = 3        
    }

    public enum DiviceTypeType
    {
        IOS = 0,
        Android = 1
    }

    public enum FitComChallengeType  
    {
        FreeFormChallenge = 0,  
        FitComChallenges = 1
    }

    public enum UserTrainingType     
    {
        PersonalTraining=0 ,
        MobileTraining=1
    }

    public enum FFChallengeSubCategory 
    {
        FreeFormWorkout = 15,
        FreeFormWellness = 16
    }

    public enum JoinStatus
    {
        JoinTeam = 0,
        MyTeam = 1,
        Follow = 2,
        Unfollow = 3,
        CurrentUser = 4
    }

    /// <summary>
    /// FeaturedType enum
    /// </summary>
    public enum FeaturedType
    {
        Workout = 0,
        FitnessTest = 1,
        Program = 2,
        Wellness = 3
    }

    public enum TrendingType
    {
        Workout = 0,
        FitnessTest = 1,
        Wellness = 2,
        Program = 3
    }

    public enum AssignmentStatus
    {
        Completed = 1,
        Incomplete = 0
    }

      /// <summary>
    /// NotificationType constant
    /// </summary>
    public enum NotificationType 
    {
        TrainerChallege = 0,
        FriendChallege = 1,
        TrainerJoinTeam = 2,
        NewsFeedBoomed=3,
        NewsFeedCommented=4,
        ResultFeedBoomed=5,
        ResultCommented=6,
        Following=7,
        TrainerPostToUser = 8,
        PostCommentedReplyMsg=9,
        AcceptedChallenge=10,
        ProfilesPostToUser = 11 ,
        PostResultReplyMsg = 12,
        SelectPrimaryTrainer = 13 ,
        ChatNotification = 14,       
        UserSentToReceiver = 15,       
        None = 16,
    }
    
    /// <summary>
    /// DeviceType constsant
    /// </summary>
    public enum DeviceType 
    {
        IOS = 0,
        Android = 1, 
        None=2
       
    }
    public enum SubscriptionPurchaseStatus:int
    {
        Buy = 0, 
        Cancellation = 1,
        Refund = 2        
    }
    /// <summary>
    /// Provide the Exteranl Login Provider
    /// </summary>
    public enum ExternalLoginProvider 
    {
        Facebook = 0,
        Google = 1,
        Twitter= 2,
        Microsoft= 3,
        Histogram=4,
        LocalProvider=5 
    }

    public enum FitnessLevel
    {
        Beginner = 0,
        Intermediate = 1,
        Advanced = 2
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }

}