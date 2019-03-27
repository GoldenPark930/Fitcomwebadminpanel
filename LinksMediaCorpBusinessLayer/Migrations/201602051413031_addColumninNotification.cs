namespace LinksMediaCorp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumninNotification : DbMigration
    {
        public override void Up()
        {
          
            
            CreateTable(
                "dbo.tblUserNotification",
                c => new
                    {
                        NotificationID = c.Long(nullable: false, identity: true),
                        SenderCredlID = c.Int(nullable: false),
                        ReceiverCredID = c.Int(nullable: false),
                        NotificationType = c.String(),
                        SenderUserName = c.String(),
                        Status = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        TokenDevicesID = c.String(),
                    })
                .PrimaryKey(t => t.NotificationID);
            
         
        }
        
        public override void Down()
        {
           
            DropTable("dbo.tblUserNotification");
        }
    }
}
