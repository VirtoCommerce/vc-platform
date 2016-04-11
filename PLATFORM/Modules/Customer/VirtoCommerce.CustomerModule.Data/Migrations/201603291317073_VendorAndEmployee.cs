namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorAndEmployee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        FirstName = c.String(maxLength: 128),
                        MiddleName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 254),
                        TimeZone = c.String(maxLength: 32),
                        DefaultLanguage = c.String(maxLength: 32),
                        BirthDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Vendor",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        SiteUrl = c.String(maxLength: 2048),
                        LogoUrl = c.String(maxLength: 2048),
                        GroupName = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendor", "Id", "dbo.Member");
            DropForeignKey("dbo.Employee", "Id", "dbo.Member");
            DropIndex("dbo.Vendor", new[] { "Id" });
            DropIndex("dbo.Employee", new[] { "Id" });
            DropTable("dbo.Vendor");
            DropTable("dbo.Employee");
        }
    }
}
