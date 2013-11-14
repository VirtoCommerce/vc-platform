namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchedulerUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemJob", "JobAssemblyPath", c => c.String(maxLength: 1024));
            AddColumn("dbo.SystemJob", "LoadFromGac", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemJobLogEntry", "Instance", c => c.String());
            AddColumn("dbo.SystemJobLogEntry", "TaskScheduleId", c => c.String(maxLength: 128));
            AddColumn("dbo.SystemJobLogEntry", "MultipleInstance", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskSchedule", "SystemJobId", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.TaskSchedule", "SystemJobId", "dbo.SystemJob", "SystemJobId", cascadeDelete: true);
            CreateIndex("dbo.TaskSchedule", "SystemJobId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TaskSchedule", new[] { "SystemJobId" });
            DropForeignKey("dbo.TaskSchedule", "SystemJobId", "dbo.SystemJob");
            DropColumn("dbo.TaskSchedule", "SystemJobId");
            DropColumn("dbo.SystemJobLogEntry", "MultipleInstance");
            DropColumn("dbo.SystemJobLogEntry", "TaskScheduleId");
            DropColumn("dbo.SystemJobLogEntry", "Instance");
            DropColumn("dbo.SystemJob", "LoadFromGac");
            DropColumn("dbo.SystemJob", "JobAssemblyPath");
        }
    }
}
