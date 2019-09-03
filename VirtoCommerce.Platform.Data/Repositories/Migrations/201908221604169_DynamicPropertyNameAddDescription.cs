namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicPropertyNameAddDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformDynamicPropertyName", "Description", c => c.String(maxLength: 256));

            Sql(@"DECLARE @cursor CURSOR
                    SET @cursor = CURSOR FOR
                    SELECT
                    [Id],
                    [Description]
                    FROM [PlatformDynamicProperty]
                    WHERE [Description] IS NOT NULL

                    DECLARE @id NVARCHAR(64)
                    DECLARE @description NVARCHAR(256)

                    OPEN @cursor
                    FETCH NEXT FROM @cursor INTO @id, @description;
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
	                    UPDATE [PlatformDynamicPropertyName]
	                    SET [Description] = @description
	                    WHERE [PropertyId] = @id
	                    FETCH NEXT FROM @cursor INTO @id, @description;
                    END
                    CLOSE @cursor");
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformDynamicPropertyName", "Description");
        }
    }
}
