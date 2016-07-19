namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSenderAndRecipient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformNotificationTemplate", "Sender", c => c.String());
            AddColumn("dbo.PlatformNotificationTemplate", "Recipient", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformNotificationTemplate", "Recipient");
            DropColumn("dbo.PlatformNotificationTemplate", "Sender");
        }
    }
}
