namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnNameAndDeleteTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblActivity", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblActivity", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblCEAssociation", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblCEAssociation", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblChallenge", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallenge", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblChallenge", "IsDraft", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblChallengeofTheDayQueue", "ChallengeId", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallengeofTheDayQueue", "ModifiedBy", c => c.Int());
            AddColumn("dbo.tblChallengeofTheDayQueue", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblChallengeType", "IsExerciseMoreThanOne", c => c.String(maxLength: 5));
            AddColumn("dbo.tblCTAssociation", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblCTAssociation", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblCUAssociation", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblCUAssociation", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblExercise", "VideoLink", c => c.String());
            AddColumn("dbo.tblFeaturedActivityQueue", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblFeaturedActivityQueue", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblHypeVideo", "ChallengeId", c => c.Int());
            AddColumn("dbo.tblHypeVideo", "ModifiedBy", c => c.Int());
            AddColumn("dbo.tblHypeVideo", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblTrainer", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblTrainer", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblTrainerChallenge", "ChallengeId", c => c.Int());
            AddColumn("dbo.tblTrainerChallenge", "ModifiedBy", c => c.Int());
            AddColumn("dbo.tblTrainerChallenge", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblTrainerTeamMembers", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblTrainerTeamMembers", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblUser", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblUser", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblUserActivities", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserActivities", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.tblUserChallenges", "ChallengeId", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserChallenges", "ModifiedBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserChallenges", "ModifiedDate", c => c.DateTime());
            AlterColumn("dbo.tblActivity", "TrainerId", c => c.Int(nullable: false));
            AlterColumn("dbo.tblActivity", "DateOfEvent", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tblChallengeofTheDayQueue", "NameOfChallenge", c => c.String());
            AlterColumn("dbo.tblFeaturedActivityQueue", "ActivityId", c => c.Int(nullable: false));
            AlterColumn("dbo.tblTrainerChallenge", "NameOfChallenge", c => c.String());
            AlterColumn("dbo.tblTrainerTeamMembers", "TrainerId", c => c.Int(nullable: false));
            DropColumn("dbo.tblActivity", "ModifyBy");
            DropColumn("dbo.tblActivity", "ModifyDate");
            DropColumn("dbo.tblCEAssociation", "CreateBy");
            DropColumn("dbo.tblCEAssociation", "CreateDate");
            DropColumn("dbo.tblChallenge", "UserId");
            DropColumn("dbo.tblChallenge", "MasterChallengeId");
            DropColumn("dbo.tblChallenge", "Description");
            DropColumn("dbo.tblChallenge", "CreateBy");
            DropColumn("dbo.tblChallenge", "CreateDate");
            DropColumn("dbo.tblChallenge", "IsDrafted");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ChallangeID");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ModifyBy");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ModifyDate");
            DropColumn("dbo.tblChallengeType", "IsExersizeMoreThanOne");
            DropColumn("dbo.tblCTAssociation", "ModifyBy");
            DropColumn("dbo.tblCTAssociation", "ModifyDate");
            DropColumn("dbo.tblCUAssociation", "ModifyBy");
            DropColumn("dbo.tblCUAssociation", "ModifyDate");
            DropColumn("dbo.tblExercise", "VedioLink");
            DropColumn("dbo.tblFeaturedActivityQueue", "ModifyBy");
            DropColumn("dbo.tblFeaturedActivityQueue", "ModifyDate");
            DropColumn("dbo.tblHypeVideo", "ChallangeID");
            DropColumn("dbo.tblHypeVideo", "ModifyBy");
            DropColumn("dbo.tblHypeVideo", "ModifyDate");
            DropColumn("dbo.tblTrainer", "ModifyBy");
            DropColumn("dbo.tblTrainer", "ModifyDate");
            DropColumn("dbo.tblTrainerChallenge", "ChallangeID");
            DropColumn("dbo.tblTrainerChallenge", "ModifyBy");
            DropColumn("dbo.tblTrainerChallenge", "ModifyDate");
            DropColumn("dbo.tblTrainerTeamMembers", "ModifyBy");
            DropColumn("dbo.tblTrainerTeamMembers", "ModifyDate");
            DropColumn("dbo.tblUser", "ModifyBy");
            DropColumn("dbo.tblUser", "ModifyDate");
            DropColumn("dbo.tblUserActivities", "ModifyBy");
            DropColumn("dbo.tblUserActivities", "ModifyDate");
            DropColumn("dbo.tblUserChallenges", "ChallangeID");
            DropColumn("dbo.tblUserChallenges", "ModifyBy");
            DropColumn("dbo.tblUserChallenges", "ModifyDate");
            DropTable("dbo.tblAdmin");
            DropTable("dbo.tblType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblType",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "dbo.tblAdmin",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(),
                        Phone = c.String(),
                        ZipCode = c.String(),
                        EmailId = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifyBy = c.Int(nullable: false),
                        ModifyDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AdminId);
            
            AddColumn("dbo.tblUserChallenges", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblUserChallenges", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserChallenges", "ChallangeID", c => c.Int(nullable: false));
            AddColumn("dbo.tblUserActivities", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblUserActivities", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblUser", "ModifyDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblUser", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblTrainerTeamMembers", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblTrainerTeamMembers", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblTrainerChallenge", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblTrainerChallenge", "ModifyBy", c => c.Int());
            AddColumn("dbo.tblTrainerChallenge", "ChallangeID", c => c.Int());
            AddColumn("dbo.tblTrainer", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblTrainer", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblHypeVideo", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblHypeVideo", "ModifyBy", c => c.Int());
            AddColumn("dbo.tblHypeVideo", "ChallangeID", c => c.Int());
            AddColumn("dbo.tblFeaturedActivityQueue", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblFeaturedActivityQueue", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblExercise", "VedioLink", c => c.String());
            AddColumn("dbo.tblCUAssociation", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblCUAssociation", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblCTAssociation", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblCTAssociation", "ModifyBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallengeType", "IsExersizeMoreThanOne", c => c.String(maxLength: 5));
            AddColumn("dbo.tblChallengeofTheDayQueue", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblChallengeofTheDayQueue", "ModifyBy", c => c.Int());
            AddColumn("dbo.tblChallengeofTheDayQueue", "ChallangeID", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallenge", "IsDrafted", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblChallenge", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblChallenge", "CreateBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallenge", "Description", c => c.String(maxLength: 200));
            AddColumn("dbo.tblChallenge", "MasterChallengeId", c => c.Int(nullable: false));
            AddColumn("dbo.tblChallenge", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.tblCEAssociation", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tblCEAssociation", "CreateBy", c => c.Int(nullable: false));
            AddColumn("dbo.tblActivity", "ModifyDate", c => c.DateTime());
            AddColumn("dbo.tblActivity", "ModifyBy", c => c.Int(nullable: false));
            AlterColumn("dbo.tblTrainerTeamMembers", "TrainerId", c => c.Int(nullable: false));
            AlterColumn("dbo.tblTrainerChallenge", "NameOfChallenge", c => c.String());
            AlterColumn("dbo.tblFeaturedActivityQueue", "ActivityId", c => c.Int(nullable: false));
            AlterColumn("dbo.tblChallengeofTheDayQueue", "NameOfChallenge", c => c.String());
            AlterColumn("dbo.tblActivity", "DateOfEvent", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tblActivity", "TrainerId", c => c.Int(nullable: false));
            DropColumn("dbo.tblUserChallenges", "ModifiedDate");
            DropColumn("dbo.tblUserChallenges", "ModifiedBy");
            DropColumn("dbo.tblUserChallenges", "ChallengeId");
            DropColumn("dbo.tblUserActivities", "ModifiedDate");
            DropColumn("dbo.tblUserActivities", "ModifiedBy");
            DropColumn("dbo.tblUser", "ModifiedDate");
            DropColumn("dbo.tblUser", "ModifiedBy");
            DropColumn("dbo.tblTrainerTeamMembers", "ModifiedDate");
            DropColumn("dbo.tblTrainerTeamMembers", "ModifiedBy");
            DropColumn("dbo.tblTrainerChallenge", "ModifiedDate");
            DropColumn("dbo.tblTrainerChallenge", "ModifiedBy");
            DropColumn("dbo.tblTrainerChallenge", "ChallengeId");
            DropColumn("dbo.tblTrainer", "ModifiedDate");
            DropColumn("dbo.tblTrainer", "ModifiedBy");
            DropColumn("dbo.tblHypeVideo", "ModifiedDate");
            DropColumn("dbo.tblHypeVideo", "ModifiedBy");
            DropColumn("dbo.tblHypeVideo", "ChallengeId");
            DropColumn("dbo.tblFeaturedActivityQueue", "ModifiedDate");
            DropColumn("dbo.tblFeaturedActivityQueue", "ModifiedBy");
            DropColumn("dbo.tblExercise", "VideoLink");
            DropColumn("dbo.tblCUAssociation", "ModifiedDate");
            DropColumn("dbo.tblCUAssociation", "ModifiedBy");
            DropColumn("dbo.tblCTAssociation", "ModifiedDate");
            DropColumn("dbo.tblCTAssociation", "ModifiedBy");
            DropColumn("dbo.tblChallengeType", "IsExerciseMoreThanOne");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ModifiedDate");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ModifiedBy");
            DropColumn("dbo.tblChallengeofTheDayQueue", "ChallengeId");
            DropColumn("dbo.tblChallenge", "IsDraft");
            DropColumn("dbo.tblChallenge", "CreatedDate");
            DropColumn("dbo.tblChallenge", "CreatedBy");
            DropColumn("dbo.tblCEAssociation", "CreatedDate");
            DropColumn("dbo.tblCEAssociation", "CreatedBy");
            DropColumn("dbo.tblActivity", "ModifiedDate");
            DropColumn("dbo.tblActivity", "ModifiedBy");
        }
    }
}
