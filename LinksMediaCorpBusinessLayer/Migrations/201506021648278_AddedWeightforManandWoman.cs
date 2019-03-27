namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWeightforManandWoman : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblCEAssociation", "Reps", c => c.Int(nullable: false));
            AddColumn("dbo.tblCEAssociation", "WeightForMan", c => c.Int(nullable: false));
            AddColumn("dbo.tblCEAssociation", "WeightForWoman", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblCEAssociation", "WeightForWoman");
            DropColumn("dbo.tblCEAssociation", "WeightForMan");
            DropColumn("dbo.tblCEAssociation", "Reps");
        }
    }
}
