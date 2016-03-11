namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssociatedObjectNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentMenuLink", "AssociatedObjectName", c => c.String(maxLength: 254));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentMenuLink", "AssociatedObjectName");
        }
    }
}
