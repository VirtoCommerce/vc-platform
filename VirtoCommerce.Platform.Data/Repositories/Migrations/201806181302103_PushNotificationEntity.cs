namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushNotificationEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformPushNotification",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(maxLength: 128),
                        IsNew = c.Boolean(nullable: false),
                        Title = c.String(),
                        AssemblyQualifiedType = c.String(),
                        SourceNotificationAsJson = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlatformPushNotification");
        }
    }
}
