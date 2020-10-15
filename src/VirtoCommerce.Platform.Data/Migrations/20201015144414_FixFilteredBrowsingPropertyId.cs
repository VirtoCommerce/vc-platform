using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Data.Migrations
{
    public partial class FixFilteredBrowsingPropertyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PlatformDynamicProperty'))
                BEGIN
                    DELETE FROM PlatformDynamicPropertyName WHERE PropertyId IN (select Id from [PlatformDynamicProperty]
                        WHERE Name = 'FilteredBrowsing' AND ObjectType IN ('VirtoCommerce.StoreModule.Core.Model.Store','VirtoCommerce.Domain.Store.Model.Store'))
                    DELETE FROM PlatformDynamicPropertyObjectValue WHERE PropertyId IN (select Id from [PlatformDynamicProperty]
                        WHERE Name = 'FilteredBrowsing' AND ObjectType IN ('VirtoCommerce.StoreModule.Core.Model.Store','VirtoCommerce.Domain.Store.Model.Store'))
                    UPDATE [PlatformDynamicProperty] SET Id = 'VirtoCommerce.Catalog_FilteredBrowsing_Property'
                        WHERE Name = 'FilteredBrowsing' AND ObjectType IN ('VirtoCommerce.StoreModule.Core.Model.Store','VirtoCommerce.Domain.Store.Model.Store') AND Id != 'VirtoCommerce.Catalog_FilteredBrowsing_Property'
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
