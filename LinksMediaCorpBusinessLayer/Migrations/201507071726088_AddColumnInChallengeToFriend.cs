namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnInChallengeToFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallengeToFriend", "ChallengeDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblChallengeToFriend", "ChallengeDate");
        }
    }
}
