namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetEntry",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RelativeUrl = c.String(nullable: false, maxLength: 2083),
                        TenantId = c.String(maxLength: 128),
                        TenantType = c.String(maxLength: 256),
                        Name = c.String(nullable: false, maxLength: 1024),
                        MimeType = c.String(maxLength: 128),
                        LanguageCode = c.String(maxLength: 10),
                        Size = c.Long(nullable: false),
                        Group = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.TenantId, t.TenantType }, name: "IX_AssetEntry_TenantId_TenantType");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetEntry", "IX_AssetEntry_TenantId_TenantType");
            DropTable("dbo.AssetEntry");
        }
    }
}
