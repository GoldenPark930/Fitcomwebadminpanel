namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExerciseTypeIdIntblChallenge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblChallenge", "ExerciseTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblChallenge", "ExerciseTypeId");
        }
    }
}
