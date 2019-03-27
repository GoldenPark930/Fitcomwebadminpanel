namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUnUsedAssociationTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblSponsorChallengeQueue",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(),
                        TrainerId = c.Int(nullable: false),
                        NameOfChallenge = c.String(),
                        SponsorName = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CurrentlyDisplayed = c.Boolean(),
                        ResultId = c.Int(nullable: false),
                        HypeVideoId = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QueueId);
            
            DropTable("dbo.tblCTAssociation");
            DropTable("dbo.tblCUAssociation");
            DropTable("dbo.tblTrainerChallenge");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblTrainerChallenge",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(),
                        TrainerId = c.Int(nullable: false),
                        NameOfChallenge = c.String(),
                        SponsorName = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CurrentlyDisplayed = c.Boolean(),
                        ResultId = c.Int(nullable: false),
                        HypeVideoId = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QueueId);
            
            CreateTable(
                "dbo.tblCUAssociation",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        UserResult = c.String(),
                        ChallengeStatus = c.String(),
                        VideoLink = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            CreateTable(
                "dbo.tblCTAssociation",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        UserResult = c.String(),
                        VideoLink = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            DropTable("dbo.tblSponsorChallengeQueue");
        }
    }
}
