namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Localization",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 5),
                        Category = c.String(nullable: false, maxLength: 128, defaultValue:string.Empty),
                        Value = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Name, t.LanguageCode, t.Category });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Localization");
        }
    }
}
