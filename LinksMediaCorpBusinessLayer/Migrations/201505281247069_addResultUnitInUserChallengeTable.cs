namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResultUnitInUserChallengeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUserChallenges", "ResultUnit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUserChallenges", "ResultUnit");
        }
    }
}
