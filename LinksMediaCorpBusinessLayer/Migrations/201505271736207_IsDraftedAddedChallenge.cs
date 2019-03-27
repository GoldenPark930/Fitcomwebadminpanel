namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsDraftedAddedChallenge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallenge", "IsDrafted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblChallenge", "IsDrafted");
        }
    }
}
