namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTabletblExerciseType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblExerciseType",
                c => new
                    {
                        ExerciseTypeId = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                    })
                .PrimaryKey(t => t.ExerciseTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblExerciseType");
        }
    }
}
