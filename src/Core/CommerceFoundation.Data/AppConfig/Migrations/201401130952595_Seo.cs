namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Seo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeoUrlKeyword",
                c => new
                    {
                        SeoUrlKeywordId = c.String(nullable: false, maxLength: 64),
                        Language = c.String(nullable: false, maxLength: 5),
                        Keyword = c.String(nullable: false, maxLength: 255),
                        KeywordValue = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                        KeywordType = c.Int(nullable: false),
                        Title = c.String(maxLength: 255),
                        MetaDescription = c.String(maxLength: 1024),
                        MetaKeywords = c.String(maxLength: 255),
                        ImageAltDescription = c.String(maxLength: 255),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SeoUrlKeywordId)
                .Index(f => new {f.Keyword, f.KeywordType, f.Language, f.IsActive}, true);
        }

        public override void Down()
        {
            DropTable("dbo.SeoUrlKeyword");
        }
    }
}
