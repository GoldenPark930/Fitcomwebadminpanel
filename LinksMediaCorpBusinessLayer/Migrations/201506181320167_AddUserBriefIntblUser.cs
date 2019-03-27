namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBriefIntblUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUser", "UserBrief", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUser", "UserBrief");
        }
    }
}
