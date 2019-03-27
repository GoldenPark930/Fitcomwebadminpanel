namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTblETCAssociation1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblETCAssociation",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(nullable: false),
                        ExerciseTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblETCAssociation");
        }
    }
}
