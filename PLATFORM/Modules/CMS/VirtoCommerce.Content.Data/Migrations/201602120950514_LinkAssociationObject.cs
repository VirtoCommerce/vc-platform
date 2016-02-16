namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkAssociationObject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentMenuLink", "AssociatedObjectType", c => c.String(maxLength: 254));
            AddColumn("dbo.ContentMenuLink", "AssociatedObjectId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContentMenuLink", "Title", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.ContentMenuLink", "Url", c => c.String(nullable: false, maxLength: 2048));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContentMenuLink", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.ContentMenuLink", "Title", c => c.String(nullable: false));
            DropColumn("dbo.ContentMenuLink", "AssociatedObjectId");
            DropColumn("dbo.ContentMenuLink", "AssociatedObjectType");
        }
    }
}
