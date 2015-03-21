namespace VirtoCommerce.Foundation.Data.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApiAcount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiAccount",
                c => new
                    {
                        ApiAccountId = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(nullable: false, maxLength: 128),
                        AppId = c.String(nullable: false, maxLength: 128),
                        SecretKey = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ApiAccountId)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.AppId, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiAccount", "AccountId", "dbo.Account");
            DropIndex("dbo.ApiAccount", new[] { "AppId" });
            DropIndex("dbo.ApiAccount", new[] { "AccountId" });
            DropTable("dbo.ApiAccount");
        }
    }
}
