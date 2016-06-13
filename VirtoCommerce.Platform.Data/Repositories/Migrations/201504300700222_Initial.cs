namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformSetting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        IsSystem = c.Boolean(nullable: false),
                        SettingValueType = c.String(nullable: false, maxLength: 64),
                        IsEnum = c.Boolean(nullable: false),
                        IsMultiValue = c.Boolean(nullable: false),
                        IsLocaleDependant = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlatformSettingValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        SettingId = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformSetting", t => t.SettingId, cascadeDelete: true)
                .Index(t => t.SettingId);
            
            CreateTable(
                "dbo.PlatformAccount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(maxLength: 128),
                        MemberId = c.String(maxLength: 64),
                        UserName = c.String(nullable: false, maxLength: 128),
                        RegisterType = c.Int(nullable: false),
                        AccountState = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlatformApiAccount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        ApiAccountType = c.Int(nullable: false),
                        AccountId = c.String(nullable: false, maxLength: 64),
                        AppId = c.String(nullable: false, maxLength: 128),
                        SecretKey = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformAccount", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.AppId, unique: true);
            
            CreateTable(
                "dbo.PlatformRoleAssignment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        OrganizationId = c.String(maxLength: 64),
                        AccountId = c.String(nullable: false, maxLength: 64),
                        RoleId = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformAccount", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.PlatformRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.PlatformRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlatformRolePermission",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        RoleId = c.String(nullable: false, maxLength: 64),
                        PermissionId = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformPermission", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.PlatformRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.PlatformPermission",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlatformRoleAssignment", "RoleId", "dbo.PlatformRole");
            DropForeignKey("dbo.PlatformRolePermission", "RoleId", "dbo.PlatformRole");
            DropForeignKey("dbo.PlatformRolePermission", "PermissionId", "dbo.PlatformPermission");
            DropForeignKey("dbo.PlatformRoleAssignment", "AccountId", "dbo.PlatformAccount");
            DropForeignKey("dbo.PlatformApiAccount", "AccountId", "dbo.PlatformAccount");
            DropForeignKey("dbo.PlatformSettingValue", "SettingId", "dbo.PlatformSetting");
            DropIndex("dbo.PlatformRolePermission", new[] { "PermissionId" });
            DropIndex("dbo.PlatformRolePermission", new[] { "RoleId" });
            DropIndex("dbo.PlatformRoleAssignment", new[] { "RoleId" });
            DropIndex("dbo.PlatformRoleAssignment", new[] { "AccountId" });
            DropIndex("dbo.PlatformApiAccount", new[] { "AppId" });
            DropIndex("dbo.PlatformApiAccount", new[] { "AccountId" });
            DropIndex("dbo.PlatformSettingValue", new[] { "SettingId" });
            DropTable("dbo.PlatformPermission");
            DropTable("dbo.PlatformRolePermission");
            DropTable("dbo.PlatformRole");
            DropTable("dbo.PlatformRoleAssignment");
            DropTable("dbo.PlatformApiAccount");
            DropTable("dbo.PlatformAccount");
            DropTable("dbo.PlatformSettingValue");
            DropTable("dbo.PlatformSetting");
        }
    }
}
