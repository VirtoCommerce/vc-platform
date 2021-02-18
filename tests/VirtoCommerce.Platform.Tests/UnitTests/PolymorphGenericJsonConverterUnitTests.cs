using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;
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
            var extendedObjectSetting = JsonConvert.DeserializeObject<ExtendedObjectSettingEntry>(json, new PolymorphGenericJsonConverter<ObjectSettingEntry>());
            var objectSetting = JsonConvert.DeserializeObject<ObjectSettingEntry>(json, new PolymorphGenericJsonConverter<ObjectSettingEntry>());

            //Assert
            extendedObjectSetting.Should().NotBeNull();
            extendedObjectSetting.ExtField.Should().Equals("HasExtField");
            objectSetting.Should().NotBeNull();
        }
    }

    class ExtendedObjectSettingEntry : ObjectSettingEntry
    {
        public string ExtField { get; set; }
    }
}
