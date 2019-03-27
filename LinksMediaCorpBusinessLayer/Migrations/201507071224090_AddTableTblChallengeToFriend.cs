namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableTblChallengeToFriend : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblChallengeToFriend",
                c => new
                    {
                        ChallengeToFriendId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        TargetId = c.Int(nullable: false),
                        ChallengeId = c.Int(nullable: false),
                        IsPending = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChallengeToFriendId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblChallengeToFriend");
        }
    }
}
