namespace LinksMediaCorpDataAccessLayer
{
    using LinksMediaCorpDataAccessLayer;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class LinksMediaContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        public LinksMediaContext()
            : base("name=LinksMediaDBContext")
        {
            Database.SetInitializer<LinksMediaContext>(null);
        }     
        public DbSet<tblBodyPart> BodyPart { get; set; }
        public DbSet<tblCEAssociation> CEAssociation { get; set; }
        public DbSet<tblChallenge> Challenge { get; set; }
        public DbSet<tblChallengeofTheDayQueue> ChallengeofTheDayQueue { get; set; }
        public DbSet<tblSponsorChallengeQueue> TrainerChallenge { get; set; }
        public DbSet<tblChallengeType> ChallengeType { get; set; }
        public DbSet<tblCredentials> Credentials { get; set; }
        public DbSet<tblExercise> Exercise { get; set; }
        public DbSet<tblPieceOfEquipment> PieceofEquipment { get; set; }
        public DbSet<tblSpecialization> Specialization { get; set; }
        public DbSet<tblTrainer> Trainer { get; set; }
        public DbSet<tblTrainingType> TrainingType { get; set; }
        public DbSet<tblTrainerTeamMembers> TrainerTeamMembers { get; set; }
        public DbSet<tblVideo> Videos { get; set; }
        public DbSet<tblPic> Pics { get; set; }
        public DbSet<tblFollowings> Followings { get; set; }
        public DbSet<tblUser> User { get; set; }
        public DbSet<tblUserToken> UserToken { get; set; }
        public DbSet<tblUserChallenges> UserChallenge { get; set; }
        public DbSet<tblActivity> Activity { get; set; }
        public DbSet<tblFeaturedActivityQueue> FeaturedActivityQueue { get; set; }
        public DbSet<tblUserActivities> USerActivity { get; set; }
        public DbSet<tblTrainerSpecialization> TrainerSpecialization { get; set; }
        public DbSet<tblMessageStream> MessageStraems { get; set; }
        public DbSet<tblComment> Comments { get; set; }
        public DbSet<tblBoom> Booms { get; set; }
        public DbSet<tblState> States { get; set; }
        public DbSet<tblCity> Cities { get; set; }
        public DbSet<tblHypeVideo> HypeVideos { get; set; }
        public DbSet<tblMessageStreamVideo> MessageStreamVideo { get; set; }
        public DbSet<tblMessageStreamPic> MessageStreamPic { get; set; }
        public DbSet<tblDifficulty> Difficulties { get; set; }
        public DbSet<tblEquipment> Equipments { get; set; }
        public DbSet<tblExerciseType> ExerciseTypes { get; set; }
        public DbSet<tblETCAssociation> ETCAssociations { get; set; }
        public DbSet<tblChallengeToFriend> ChallengeToFriends { get; set; }
        public DbSet<tblUserNotificationSetting> UserNotificationSetting { get; set; }
        public DbSet<tblTrainerType> TrainerType { get; set; }       
        public DbSet<tblUTCPostMessage> UTCPostMessage { get; set; }
        public DbSet<tblUserNotifications> UserNotifications { get; set; }
        public DbSet<tblResultComment> ResultComments { get; set; }
        public DbSet<tblResultBoom> ResultBooms { get; set; }
        public DbSet<tblTeam> Teams { get; set; }
        public DbSet<tblNotesExecise> NotesExecise { get; set; } 
        public DbSet<tblSessionNotesExeciseSet> SessionNotesExeciseSets { get; set; } 
        public DbSet<tblSessionPurchaseHistory> SessionPurchaseHistories { get; set; } 
        public DbSet<tblUsedSession> UsedSessions { get; set; } 
        public DbSet<tblUsedSessionNote> UsedSessionNotes { get; set; }
        public DbSet<tblUserSessionDetail> UserSessionDetails { get; set; }
        public DbSet<tblChallengeCategory> ChallengeCategory { get; set; }
        public DbSet<tblCESAssociation> CESAssociations { get; set; }
        public DbSet<tblPWAssociation> PWAssociation { get; set; }
        public DbSet<tblPWWorkoutsAssociation> PWWorkoutsAssociation { get; set; }
        public DbSet<tblUserActivePrograms> UserActivePrograms  { get; set; }
        public DbSet<tblUserAcceptedProgramWeek> UserAcceptedProgramWeeks { get; set; } 
        public DbSet<tblUserAcceptedProgramWorkouts> UserAcceptedProgramWorkouts { get; set; }
        public DbSet<tblTrainingZoneCAssociation> TrainingZoneCAssociations { get; set; }
        public DbSet<tblCEquipmentAssociation> ChallengeEquipmentAssociations { get; set; }
        public DbSet<tblUserAssignmentByTrainer> UserAssignments { get; set; }
        public DbSet<tblChatHistory> ChatHistory { get; set; }
        public DbSet<tblNoTrainerChallengeTeam> NoTrainerChallengeTeams { get; set; }
        public DbSet<tblTrendingCategory> TrendingCategory { get; set; }
        public DbSet<tblTeamTrendingAssociation> TeamTrendingAssociations { get; set; }
        public DbSet<tblChallengeCategoryAssociation> ChallengeCategoryAssociations { get; set; }
        public DbSet<tblChallengeTrendingAssociation> ChallengeTrendingAssociations { get; set; }
        public DbSet<tblAppSubscription> AppSubscriptions { get; set; } 
        public DbSet<tblTrainerMobileCoachTeam> TrainerMobileCoachTeams { get; set; }
        public DbSet<tblLevelTeam> LevelTeams { get; set; }
     
        public DbSet<tblExerciseUploadHistory> ExerciseUploadHistory { get; set; }    
        
        public DbSet<tblTeamCommissionReport> TeamCommissionReport { get; set; }

        public DbSet<tblTeamLevelCommissionReport> TeamLevelCommissionReport { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<tblComment>()
            //           .HasRequired<tblMessageStream>(c => c.MessageStream)
            //           .WithMany(m => m.Comments)
            //           .HasForeignKey(m => m.MessageStraemId);

        }
    }
}
