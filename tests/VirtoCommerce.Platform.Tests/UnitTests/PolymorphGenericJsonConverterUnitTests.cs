using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class PolymorphGenericJsonConverterUnitTests
    {
        [Fact]
        public void DeserializeObject_DeserializeExtendedSetting()
        {
            //Arrange
            AbstractTypeFactory<ObjectSettingEntry>.OverrideType<ObjectSettingEntry, ExtendedObjectSettingEntry>();

            var json = "{'extField':'HasExtField','itHasValues':false,'restartRequired':false,'moduleId':'Platform','groupName':'Platform|Security','name':'VirtoCommerce.Platform.Security.CronPruneExpiredTokensJob','isHidden':false,'valueType':'ShortText','defaultValue':'0 0 */1 * *','isDictionary':false}"
                        .Replace("'", "\"");

            //Act
            var objectSetting = JsonConvert.DeserializeObject<ObjectSettingEntry>(json, new PolymorphGenericJsonConverter<ObjectSettingEntry>());
            var extendedObjectSetting = objectSetting as ExtendedObjectSettingEntry;

            var extendedSetting_explicitly = JsonConvert.DeserializeObject<ExtendedObjectSettingEntry>(json, new PolymorphGenericJsonConverter<ObjectSettingEntry>());

            //Assert
            objectSetting.Should().NotBeNull();
            extendedObjectSetting.Should().NotBeNull();
            extendedObjectSetting.ExtField.Should().Be("HasExtField");

            extendedSetting_explicitly.Should().NotBeNull();
            extendedSetting_explicitly.ExtField.Should().Be("HasExtField");
        }

        [Fact]
        public void DeserializeObject_DeserializeBasedOnPropertyValue()
        {
            //Arrange
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScope>();
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScopeExt1>();
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScopeExt2>();

            var json0 = "{'scope':'Scope000'}".Replace("'", "\"");
            var json1 = "{'Type':'PermissionScopeExt1','scope':'Scope111'}".Replace("'", "\"");
            var json2 = "{'Type':'PermissionScopeExt2','scope':'Scope123'}".Replace("'", "\"");

            var converter = new PolymorphGenericJsonConverter<PermissionScope>(nameof(PermissionScope.Type));

            //Act
            var scope0 = JsonConvert.DeserializeObject<PermissionScope>(json0, converter);

            //Assert
            scope0.Should().NotBeNull();
            scope0.Should().BeOfType<PermissionScope>();
            scope0.Should().NotBeOfType<PermissionScopeExt1>();
            scope0.Should().NotBeOfType<PermissionScopeExt2>();

            //Act
            var scope1 = JsonConvert.DeserializeObject<PermissionScope>(json1, converter);

            //Assert
            scope1.Should().NotBeNull();
            scope1.Should().BeOfType<PermissionScopeExt1>();
            scope1.Should().NotBeOfType<PermissionScope>();
            scope1.Should().NotBeOfType<PermissionScopeExt2>();

            //Act
            var scope2 = JsonConvert.DeserializeObject<PermissionScope>(json2, converter);

            //Assert
            scope2.Should().NotBeNull();
            scope2.Should().BeOfType<PermissionScopeExt2>();
            scope2.Should().NotBeOfType<PermissionScope>();
            scope2.Should().NotBeOfType<PermissionScopeExt1>();
        }
    }

    class ExtendedObjectSettingEntry : ObjectSettingEntry
    {
        public string ExtField { get; set; }
    }

    class PermissionScopeExt1 : PermissionScope
    {
    }

    class PermissionScopeExt2 : PermissionScope
    {
    }
}
