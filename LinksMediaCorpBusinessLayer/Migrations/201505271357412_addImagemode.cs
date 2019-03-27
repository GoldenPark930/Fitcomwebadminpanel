namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImagemode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMessageStreamPic", "ImageMode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMessageStreamPic", "ImageMode");
        }
    }
}
