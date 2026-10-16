using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    /// <summary>
    /// Pins how <see cref="LocalizableSettingService.SaveAsync"/> treats translations that are stored twice for the same
    /// value and language. Two concurrent saves of the same dictionary (e.g. a double-clicked Save in the Admin UI) both
    /// insert the new translation, because nothing in the database prevents it. Every later save of that dictionary used to
    /// fail with "An item with the same key has already been added"; it must succeed and remove the extra row instead.
    /// </summary>
    public class LocalizableSettingServiceTests
    {
        private const string SettingName = "Order.Status";

        [Fact]
        public async Task SaveAsync_StoredDuplicate_DeletesDuplicate()
        {
            // Arrange: the en-US translation of "Pending" is stored twice, as a double-clicked Save leaves it.
            var storedItems = new List<LocalizedItem>
            {
                CreateLocalizedItem("1", "Pending", "de-DE", "Ausstehend"),
                CreateLocalizedItem("2", "Pending", "en-US", "Awaiting payment"),
                CreateLocalizedItem("3", "Pending", "en-US", "Awaiting payment"),
            };

            var deletedIds = new List<string>();
            var savedItems = new List<LocalizedItem>();
            var service = CreateService(storedItems, deletedIds, savedItems);

            // The editor posts the whole dictionary: the edited value first, then the others unchanged.
            var items = new List<DictionaryItem>
            {
                CreateDictionaryItem("Cancelled", ("de-DE", "Storniert")),
                CreateDictionaryItem("Pending", ("de-DE", "Ausstehend"), ("en-US", "Awaiting payment")),
            };

            // Act
            await service.SaveAsync(SettingName, items);

            // Assert: the extra row is deleted, the first one is kept, and the new translation is saved.
            deletedIds.Should().Equal("3");
            savedItems.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new { Id = (string)null, Name = SettingName, Alias = "Cancelled", LanguageCode = "de-DE", Value = "Storniert" });
        }

        [Fact]
        public async Task SaveAsync_StoredDuplicate_UpdatesKeptTranslation()
        {
            // Arrange: the same duplicate, and this time the user changes the duplicated translation.
            var storedItems = new List<LocalizedItem>
            {
                CreateLocalizedItem("1", "Pending", "en-US", "Awaiting payment"),
                CreateLocalizedItem("2", "Pending", "en-US", "Awaiting payment"),
            };

            var deletedIds = new List<string>();
            var savedItems = new List<LocalizedItem>();
            var service = CreateService(storedItems, deletedIds, savedItems);

            var items = new List<DictionaryItem>
            {
                CreateDictionaryItem("Pending", ("en-US", "Payment pending")),
            };

            // Act
            await service.SaveAsync(SettingName, items);

            // Assert: one row remains and carries the new value.
            deletedIds.Should().Equal("2");
            savedItems.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new { Id = "1", Alias = "Pending", LanguageCode = "en-US", Value = "Payment pending" });
        }

        /// <summary>
        /// Builds the service over a localizable dictionary setting with the given stored translations, and captures the ids
        /// passed to <see cref="ILocalizedItemService"/> delete and the items passed to its save.
        /// </summary>
        private static LocalizableSettingService CreateService(IList<LocalizedItem> storedItems, List<string> deletedIds, List<LocalizedItem> savedItems)
        {
            var setting = new ObjectSettingEntry(new SettingDescriptor
            {
                Name = SettingName,
                ValueType = SettingValueType.ShortText,
                IsDictionary = true,
                IsLocalizable = true,
                AllowedValues = ["Cancelled", "Pending"],
            });

            var settingsManager = new Mock<ISettingsManager>();
            settingsManager
                .Setup(x => x.GetObjectSettingAsync(SettingName, null, null))
                .ReturnsAsync(setting);
            settingsManager
                .Setup(x => x.SaveObjectSettingsAsync(It.IsAny<IEnumerable<ObjectSettingEntry>>()))
                .Returns(Task.CompletedTask);

            var localizedItemSearchService = new Mock<ILocalizedItemSearchService>();
            localizedItemSearchService
                .Setup(x => x.SearchAsync(It.IsAny<LocalizedItemSearchCriteria>(), It.IsAny<bool>()))
                .ReturnsAsync(new LocalizedItemSearchResult { TotalCount = storedItems.Count, Results = storedItems });

            var localizedItemService = new Mock<ILocalizedItemService>();
            localizedItemService
                .Setup(x => x.DeleteAsync(It.IsAny<IList<string>>(), It.IsAny<bool>()))
                .Callback<IList<string>, bool>((ids, _) => deletedIds.AddRange(ids))
                .Returns(Task.CompletedTask);
            localizedItemService
                .Setup(x => x.SaveChangesAsync(It.IsAny<IList<LocalizedItem>>()))
                .Callback<IList<LocalizedItem>>(items => savedItems.AddRange(items))
                .Returns(Task.CompletedTask);

            return new LocalizableSettingService(settingsManager.Object, localizedItemSearchService.Object, localizedItemService.Object);
        }

        private static LocalizedItem CreateLocalizedItem(string id, string alias, string languageCode, string value)
        {
            return new LocalizedItem
            {
                Id = id,
                Name = SettingName,
                Alias = alias,
                LanguageCode = languageCode,
                Value = value,
            };
        }

        private static DictionaryItem CreateDictionaryItem(string alias, params (string LanguageCode, string Value)[] values)
        {
            return new DictionaryItem
            {
                Alias = alias,
                LocalizedValues = values
                    .Select(x => new LocalizedValue { LanguageCode = x.LanguageCode, Value = x.Value })
                    .ToList(),
            };
        }
    }
}
