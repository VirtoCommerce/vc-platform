namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentItem", "ContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentItem", "ContentType");
        }
    }
}
