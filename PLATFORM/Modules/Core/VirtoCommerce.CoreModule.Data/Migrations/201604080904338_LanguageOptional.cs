namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LanguageOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SeoUrlKeyword", "Language", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SeoUrlKeyword", "Language", c => c.String(nullable: false, maxLength: 5));
        }
    }
}
