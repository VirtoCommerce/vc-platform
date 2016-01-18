namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsPrimary = c.Boolean(nullable: false),
                        ExchangeRate = c.Decimal(nullable: false, storeType: "money"),
                        Symbol = c.String(maxLength: 16),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code);
            
            DropColumn("dbo.FulfillmentCenter", "Discriminator");
            DropColumn("dbo.SeoUrlKeyword", "Discriminator");
            DropColumn("dbo.Sequence", "CreatedDate");
            DropColumn("dbo.Sequence", "CreatedBy");
            DropColumn("dbo.Sequence", "ModifiedBy");
            DropColumn("dbo.Sequence", "Id");
            DropColumn("dbo.Sequence", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sequence", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Sequence", "Id", c => c.String());
            AddColumn("dbo.Sequence", "ModifiedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.Sequence", "CreatedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.Sequence", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SeoUrlKeyword", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.FulfillmentCenter", "Discriminator", c => c.String(maxLength: 128));
            DropIndex("dbo.Currency", new[] { "Code" });
            DropTable("dbo.Currency");
        }
    }
}
