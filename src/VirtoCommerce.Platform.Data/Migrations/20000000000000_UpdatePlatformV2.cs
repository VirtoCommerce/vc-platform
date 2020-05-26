using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Data.Migrations
{
    public partial class UpdatePlatformV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory'))
                IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'VirtoCommerce.Platform.Data.Repositories.Migrations.Configuration'))
                    BEGIN
	                    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES ('20180411091908_InitialPlatform', '2.2.3-servicing-35854')
                        ALTER TABLE [PlatformSetting] DROP COLUMN [IsSystem]
                        ALTER TABLE [PlatformSetting] DROP COLUMN [SettingValueType]
                        ALTER TABLE [PlatformSetting] DROP COLUMN [IsEnum]
                        ALTER TABLE [PlatformSetting] DROP COLUMN [IsMultiValue]
                        ALTER TABLE [PlatformSetting] DROP COLUMN [IsLocaleDependant]
                        UPDATE [PlatformOperationLog] SET [ObjectType] = REPLACE([ObjectType], 'Entity', '')

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
                        END
                    END");

            migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HangFire'))
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
            // This method defined empty
        }
    }
}
