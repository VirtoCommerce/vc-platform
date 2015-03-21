using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        SettingId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        IsSystem = c.Boolean(nullable: false),
                        SettingValueType = c.String(nullable: false, maxLength: 64),
                        IsEnum = c.Boolean(nullable: false),
                        IsMultiValue = c.Boolean(nullable: false),
                        IsLocaleDependant = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SettingId);
            
            CreateTable(
                "dbo.SettingValue",
                c => new
                    {
                        SettingValueId = c.String(nullable: false, maxLength: 128),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        SettingId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.SettingValueId)
                .ForeignKey("dbo.Setting", t => t.SettingId, cascadeDelete: true)
                .Index(t => t.SettingId);
            
            CreateTable(
                "dbo.Sequence",
                c => new
                    {
                        ObjectType = c.String(nullable: false, maxLength: 256),
                        Value = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ObjectType);
            
            CreateTable(
                "dbo.SystemJob",
                c => new
                    {
                        SystemJobId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        JobClassType = c.String(nullable: false, maxLength: 1024),
                        JobAssemblyPath = c.String(maxLength: 1024),
                        LoadFromGac = c.Boolean(nullable: false, defaultValue: false),
                        IsEnabled = c.Boolean(nullable: false),
                        Schedule = c.String(maxLength: 64),
                        Priority = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        LastExecuted = c.DateTime(),
                        NextExecute = c.DateTime(),
                        AllowMultipleInstances = c.Boolean(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SystemJobId);
            
            CreateTable(
                "dbo.JobParameter",
                c => new
                    {
                        JobParameterId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Alias = c.String(maxLength: 128),
                        Value = c.String(maxLength: 128),
                        IsRequired = c.Boolean(nullable: false),
                        SystemJobId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobParameterId)
                .ForeignKey("dbo.SystemJob", t => t.SystemJobId, cascadeDelete: true)
                .Index(t => t.SystemJobId);
            
            CreateTable(
                "dbo.SystemJobLogEntry",
                c => new
                    {
                        SystemJobLogEntryId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        Message = c.String(),
                        EntryLevel = c.Int(nullable: false),
                        Instance = c.String(),
                        TaskScheduleId = c.String(maxLength: 128),
                        MultipleInstance = c.Boolean(nullable: false, defaultValue: false),
                        SystemJobId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.SystemJobLogEntryId)
                .ForeignKey("dbo.SystemJob", t => t.SystemJobId, cascadeDelete: true)
                .Index(t => t.SystemJobId);
            
            CreateTable(
                "dbo.TaskSchedule",
                c => new
                    {
                        TaskScheduleId = c.String(nullable: false, maxLength: 128),
                        SystemJobId = c.String(nullable: false, maxLength: 128, defaultValue: string.Empty),
                        Frequency = c.Int(nullable: false),
                        NextScheduledStartTime = c.DateTime(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TaskScheduleId)
                .ForeignKey("dbo.SystemJob", t => t.SystemJobId, cascadeDelete: true)
                .Index(t => t.SystemJobId);
            
            CreateTable(
                "dbo.Statistic",
                c => new
                    {
                        StatisticId = c.String(nullable: false, maxLength: 128),
                        Key = c.String(maxLength: 32),
                        Name = c.String(maxLength: 64),
                        Value = c.String(maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StatisticId);
            
            CreateTable(
                "dbo.EmailTemplate",
                c => new
                    {
                        EmailTemplateId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 32),
                        Type = c.String(maxLength: 32),
                        Body = c.String(nullable: false),
                        DefaultLanguageCode = c.String(nullable: false),
                        Subject = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmailTemplateId);
            
            CreateTable(
                "dbo.EmailTemplateLanguage",
                c => new
                    {
                        EmailTemplateLanguageId = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 32),
                        Subject = c.String(nullable: false, maxLength: 128),
                        Body = c.String(),
                        Priority = c.Int(nullable: false),
                        EmailTemplateId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmailTemplateLanguageId)
                .ForeignKey("dbo.EmailTemplate", t => t.EmailTemplateId, cascadeDelete: true)
                .Index(t => t.EmailTemplateId);
            
            CreateTable(
                "dbo.DisplayTemplateMapping",
                c => new
                    {
                        DisplayTemplateMappingId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        TargetType = c.Int(nullable: false),
                        DisplayTemplate = c.String(nullable: false, maxLength: 512),
                        Priority = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PredicateSerialized = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DisplayTemplateMappingId);
            
            CreateTable(
                "dbo.ObjectLock",
                c => new
                    {
                        ObjectType = c.String(nullable: false, maxLength: 32),
                        ObjectId = c.String(nullable: false, maxLength: 64),
                        UserId = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ObjectType, t.ObjectId });
            
            CreateTable(
                "dbo.Localization",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.String(nullable: false, maxLength: 5),
                        Category = c.String(nullable: false, maxLength: 128, defaultValue: string.Empty),
                        Value = c.String(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Name, t.LanguageCode, t.Category });
            
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
                .Index(f => new { f.Keyword, f.KeywordType, f.Language, f.IsActive }, unique:true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailTemplateLanguage", "EmailTemplateId", "dbo.EmailTemplate");
            DropForeignKey("dbo.TaskSchedule", "SystemJobId", "dbo.SystemJob");
            DropForeignKey("dbo.SystemJobLogEntry", "SystemJobId", "dbo.SystemJob");
            DropForeignKey("dbo.JobParameter", "SystemJobId", "dbo.SystemJob");
            DropForeignKey("dbo.SettingValue", "SettingId", "dbo.Setting");
            DropIndex("dbo.EmailTemplateLanguage", new[] { "EmailTemplateId" });
            DropIndex("dbo.TaskSchedule", new[] { "SystemJobId" });
            DropIndex("dbo.SystemJobLogEntry", new[] { "SystemJobId" });
            DropIndex("dbo.JobParameter", new[] { "SystemJobId" });
            DropIndex("dbo.SettingValue", new[] { "SettingId" });
            DropTable("dbo.SeoUrlKeyword");
            DropTable("dbo.Localization");
            DropTable("dbo.ObjectLock");
            DropTable("dbo.DisplayTemplateMapping");
            DropTable("dbo.EmailTemplateLanguage");
            DropTable("dbo.EmailTemplate");
            DropTable("dbo.Statistic");
            DropTable("dbo.TaskSchedule");
            DropTable("dbo.SystemJobLogEntry");
            DropTable("dbo.JobParameter");
            DropTable("dbo.SystemJob");
            DropTable("dbo.Sequence");
            DropTable("dbo.SettingValue");
            DropTable("dbo.Setting");
        }
    }
}
