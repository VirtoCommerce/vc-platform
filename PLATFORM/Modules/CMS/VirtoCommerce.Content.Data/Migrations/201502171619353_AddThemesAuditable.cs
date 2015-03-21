namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemesAuditable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentTheme", "CreatedBy", c => c.String());
			AddColumn("dbo.ContentTheme", "CreatedDate", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
            AddColumn("dbo.ContentTheme", "ModifiedBy", c => c.String());
            AddColumn("dbo.ContentTheme", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentTheme", "ModifiedDate");
            DropColumn("dbo.ContentTheme", "ModifiedBy");
            DropColumn("dbo.ContentTheme", "CreatedDate");
            DropColumn("dbo.ContentTheme", "CreatedBy");
        }
    }
}
