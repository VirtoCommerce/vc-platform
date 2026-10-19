using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Validators;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    /// <summary>
    /// Pins the behaviour of the <see cref="SettingValueType.Cron"/> value type: it stores like ShortText and its
    /// value is validated as a cron expression on save (5- or 6-field), so a bad cron is rejected instead of silently
    /// breaking a scheduled job.
    /// </summary>
    public class CronSettingTests
    {
        [Theory]
        [InlineData("0 7 * * *")]           // 5-field: at 07:00 every day
        [InlineData("*/5 * * * *")]         // 5-field: every 5 minutes
        [InlineData("*/30 * * * * *")]      // 6-field (seconds): every 30 seconds
        [InlineData("0 0 */1 * *")]         // step in day-of-month
        [InlineData("0,30 8-18 * * *")]     // list + range
        [InlineData("0 0-6 * * MON-FRI")]   // range + day-of-week names
        public void Validator_Accepts_Valid_Cron(string cron)
        {
            var result = new ObjectSettingEntryValidator().Validate(CronSetting(cron));

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]                 // empty
        [InlineData("   ")]              // whitespace
        [InlineData("not-a-cron")]       // garbage
        [InlineData("99 99 * * *")]      // out-of-range minute/hour
        [InlineData("0 24 * * *")]       // hour out of range (0-23)
        [InlineData("* * * *")]          // too few fields (4)
        [InlineData("* * * * * * *")]    // too many fields (7)
        public void Validator_Rejects_Invalid_Cron(string cron)
        {
            var result = new ObjectSettingEntryValidator().Validate(CronSetting(cron));

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validator_Cron_Check_Does_Not_Depend_On_Wall_Clock()
        {
            // A match timeout is measured in wall-clock time, so a GC pause or CPU starvation (typical right after
            // platform start) made a valid value time out and be rejected as invalid (VCST-6315). The cron regex must
            // be linear-time (NonBacktracking) and run without a timeout instead.
            var cronRegex = (Regex)typeof(ObjectSettingEntryValidator)
                .GetField("_cronRegex", BindingFlags.NonPublic | BindingFlags.Static)!
                .GetValue(null)!;

            cronRegex.Options.Should().HaveFlag(RegexOptions.NonBacktracking);
            cronRegex.MatchTimeout.Should().Be(Regex.InfiniteMatchTimeout);
        }

        [Fact]
        public void Validator_Rejects_Long_Pathological_Cron()
        {
            var cron = string.Join(",", Enumerable.Repeat("1-2/3", 20_000)) + " * * * * x";

            var result = new ObjectSettingEntryValidator().Validate(CronSetting(cron));

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Entity_Stores_And_Reads_Cron_Like_ShortText()
        {
            const string cron = "0 7 * * *";

            var entity = new SettingValueEntity().SetValue(SettingValueType.Cron, cron);

            entity.ShortTextValue.Should().Be(cron);
            entity.GetValue().Should().Be(cron);
            entity.ToString(SettingValueType.Cron, CultureInfo.InvariantCulture).Should().Be(cron);
        }

        private static ObjectSettingEntry CronSetting(string cron) => new()
        {
            Name = "Test.Cron",
            ValueType = SettingValueType.Cron,
            Value = cron,
        };
    }
}
