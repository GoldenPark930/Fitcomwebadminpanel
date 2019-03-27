namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTblETCAssociation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tblChallenge", "ExerciseTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblChallenge", "ExerciseTypeId", c => c.Int(nullable: false));
        }
    }
}
