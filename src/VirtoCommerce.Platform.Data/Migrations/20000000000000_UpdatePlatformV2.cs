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
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
