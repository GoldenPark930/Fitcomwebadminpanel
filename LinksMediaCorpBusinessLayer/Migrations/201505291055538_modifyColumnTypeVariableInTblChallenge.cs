namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyColumnTypeVariableInTblChallenge : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblChallenge", "VariableValue", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblChallenge", "VariableValue", c => c.Int(nullable: false));
        }
    }
}
