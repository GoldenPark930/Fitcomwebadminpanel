namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResultUnitSuffixInTblChallengeToFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallengeToFriend", "ResultUnitSuffix", c => c.String());
            AlterColumn("dbo.tblChallenge", "ChallengeName", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblChallenge", "ChallengeName", c => c.String(maxLength: 50));
            DropColumn("dbo.tblChallengeToFriend", "ResultUnitSuffix");
        }
    }
}
