namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteMenuLinkType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContentMenuLink", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContentMenuLink", "Type", c => c.String());
        }
    }
}
