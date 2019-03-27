namespace LinksMediaCorpBusinessLayer
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCityState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUser", "City", c => c.String());
            AddColumn("dbo.tblUser", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUser", "State");
            DropColumn("dbo.tblUser", "City");
        }
    }
}
