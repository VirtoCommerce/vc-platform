using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Data.Migrations
{
    public partial class UpdateHangfireV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF ((EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory')) AND 
	                (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HangFire')))
                IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'VirtoCommerce.Platform.Data.Repositories.Migrations.Configuration'))
                    BEGIN
	                    ALTER TABLE [HangFire].[State] DROP CONSTRAINT [FK_HangFire_State_Job];
                        ALTER TABLE [HangFire].[JobParameter] DROP CONSTRAINT [FK_HangFire_JobParameter_Job];
                        DROP TABLE [HangFire].[Schema];
                        DROP TABLE [HangFire].[Job];
                        DROP TABLE [HangFire].[State];
                        DROP TABLE [HangFire].[JobParameter];
                        DROP TABLE [HangFire].[JobQueue];
                        DROP TABLE [HangFire].[Server];
                        DROP TABLE [HangFire].[List];
                        DROP TABLE [HangFire].[Set];
                        DROP TABLE [HangFire].[Counter];
                        DROP TABLE [HangFire].[Hash];
                        DROP TABLE [HangFire].[AggregatedCounter];
                        DROP SCHEMA [HangFire];
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
