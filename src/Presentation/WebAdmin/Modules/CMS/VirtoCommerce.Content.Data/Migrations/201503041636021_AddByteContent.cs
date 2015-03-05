namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddByteContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentItem", "ByteContent", c => c.Binary());
            AddColumn("dbo.ContentItem", "FileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentItem", "FileUrl");
            DropColumn("dbo.ContentItem", "ByteContent");
        }
    }
}
