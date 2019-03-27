namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsImageVideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMessageStream", "IsImageVideo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMessageStream", "IsImageVideo");
        }
    }
}
