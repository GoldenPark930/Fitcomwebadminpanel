namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addforignkeyinTblComment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tblComment", "MessageStraemId");
            AddForeignKey("dbo.tblComment", "MessageStraemId", "dbo.tblMessageStream", "MessageStraemId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblComment", "MessageStraemId", "dbo.tblMessageStream");
            DropIndex("dbo.tblComment", new[] { "MessageStraemId" });
        }
    }
}
