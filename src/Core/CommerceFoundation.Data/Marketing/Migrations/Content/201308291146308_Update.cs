namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DynamicContentItem", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.DynamicContentPlace", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.DynamicContentPublishingGroup", "Name", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DynamicContentPublishingGroup", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.DynamicContentPlace", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.DynamicContentItem", "Name", c => c.String(maxLength: 128));
        }
    }
}
