using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class MigrationNameUnitTests
    {
        [Theory]
        [InlineData("SomeModule", "20000000000000_UpdateSomeModuleV2")]
        [InlineData("VirtoCommerce.SomeModule", "20000000000000_UpdateSomeModuleV2")]
        [InlineData("SomeCompany.SomeModule", "20000000000000_UpdateSomeCompanySomeModuleV2")]
        [InlineData("SomeCompanySomeModule", "20000000000000_UpdateSomeCompanySomeModuleV2")]
        public void GetUpdateV2MigrationName_GetNameForVirtoCommerce(string moduleName, string expectedMigrationName)
        {
            var actualMigrationName = MigrationName.GetUpdateV2MigrationName(moduleName);

            Assert.Equal(expectedMigrationName, actualMigrationName);
        }
    }
}
