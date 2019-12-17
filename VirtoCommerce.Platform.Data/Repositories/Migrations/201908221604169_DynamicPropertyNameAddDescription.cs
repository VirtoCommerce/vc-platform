namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicPropertyNameAddDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformDynamicPropertyName", "Description", c => c.String(maxLength: 256));

            Sql(@"DECLARE @properties TABLE(
                    [Id] NVARCHAR(64) NOT NULL,
                    [Description] NVARCHAR(256) NULL);

                    INSERT INTO @properties
                    SELECT
                    [Id],
                    [Description]
                    FROM [PlatformDynamicProperty]
                    WHERE [Description] IS NOT NULL

                    UPDATE [PlatformDynamicPropertyName]
                    SET [Description] = (SELECT [Description] FROM @properties WHERE [Id] = [PropertyId])
                    WHERE [PropertyId] = (SELECT [Id] FROM @properties WHERE [Id] = [PropertyId])");
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformDynamicPropertyName", "Description");
        }
    }
}
