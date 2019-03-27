namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnChallengeByUserNameAndResultInTblChallengeToFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallengeToFriend", "ChallengeByUserName", c => c.String());
            AddColumn("dbo.tblChallengeToFriend", "Result", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblChallengeToFriend", "Result");
            DropColumn("dbo.tblChallengeToFriend", "ChallengeByUserName");
        }
    }
}
