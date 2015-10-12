namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteRequest", "Tag", c => c.String(maxLength: 128));
            AddColumn("dbo.QuoteRequest", "IsSubmitted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteRequest", "IsSubmitted");
            DropColumn("dbo.QuoteRequest", "Tag");
        }
    }
}
