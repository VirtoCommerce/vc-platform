using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Search.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildSetting",
                c => new
                    {
                        BuildSettingId = c.String(nullable: false, maxLength: 128),
                        DocumentType = c.String(nullable: false, maxLength: 64),
                        Scope = c.String(nullable: false, maxLength: 64),
                        LastBuildDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BuildSettingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BuildSetting");
        }
    }
}
