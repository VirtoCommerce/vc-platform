namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCurrencyFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currency", "CustomFormatting", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Currency", "CustomFormatting");
        }
    }
}
