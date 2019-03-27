namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolumnFractionInChallengeToFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallengeToFriend", "Fraction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblChallengeToFriend", "Fraction");
        }
    }
}
