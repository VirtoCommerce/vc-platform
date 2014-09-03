namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditorialReviewLocale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EditorialReview", "Locale", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EditorialReview", "Locale");
        }
    }
}
