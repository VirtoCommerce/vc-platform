namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformNotification",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsSuccessSend = c.Boolean(nullable: false),
                        Subject = c.String(),
                        Body = c.String(),
                        Sender = c.String(),
                        Recipient = c.String(),
                        Channel = c.String(),
                        AttemptCount = c.Int(nullable: false),
                        MaxAttemptCount = c.Int(nullable: false),
                        LastFailAttemptMessage = c.String(),
                        LastFailAttemptDate = c.DateTime(),
                        StartSendingDate = c.DateTime(),
                        SentDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlatformNotificationTemplate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ObjectId = c.String(),
                        NotificationTypeId = c.String(),
                        DisplayName = c.String(),
                        Subject = c.String(),
                        TemplateEngine = c.String(),
                        Body = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlatformNotificationTemplate");
            DropTable("dbo.PlatformNotification");
        }
    }
}
