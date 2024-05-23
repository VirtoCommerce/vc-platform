using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Moq;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Hangfire;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class JobSettingsWatcherUnitTests
    {
        private readonly Mock<ISettingsManager> _settingsManagerMock;
        private readonly Mock<IRecurringJobManager> _recurringJobManagerMock;

        public JobSettingsWatcherUnitTests()
        {
            _settingsManagerMock = new Mock<ISettingsManager>();
            _recurringJobManagerMock = new Mock<IRecurringJobManager>();
        }

        [Fact]
        public void WatchJobSetting_WithBuilder_AddRecurringJob()
        {
            _settingsManagerMock.Setup(x => x.GetObjectSettingAsync("enablername", null, null)).ReturnsAsync(new ObjectSettingEntry());
            _settingsManagerMock.Setup(x => x.GetObjectSettingAsync("cronname", null, null)).ReturnsAsync(new ObjectSettingEntry());
            var recurringJobService = new RecurringJobService(_recurringJobManagerMock.Object, _settingsManagerMock.Object);

            //Act
            recurringJobService.WatchJobSettingAsync(
                new SettingCronJobBuilder()
                    .SetEnablerSetting(new SettingDescriptor { DefaultValue = true, Name = "enablername" })
                    .SetCronSetting(new SettingDescriptor { DefaultValue = "* * * *", Name = "cronname" })
                    .ToJob<SomeJob>(x => x.Process())
                    .Build());

            //Assert
            _recurringJobManagerMock.Verify(x => x.AddOrUpdate(It.IsAny<string>(),
                                                               It.IsAny<Job>(),
                                                               It.IsAny<string>(),
                                                               It.IsAny<RecurringJobOptions>())
                                                , Times.Once());
        }

        [Fact]
        public void WatchJobSetting_WithoutBuilder_AddRecurringJob()
        {
            _settingsManagerMock.Setup(x => x.GetObjectSettingAsync("enablername", null, null)).ReturnsAsync(new ObjectSettingEntry());
            _settingsManagerMock.Setup(x => x.GetObjectSettingAsync("cronname", null, null)).ReturnsAsync(new ObjectSettingEntry());
            var recurringJobService = new RecurringJobService(_recurringJobManagerMock.Object, _settingsManagerMock.Object);

            //Act
            recurringJobService.WatchJobSetting<SomeJob>(
                new SettingDescriptor { DefaultValue = true, Name = "enablername" },
                new SettingDescriptor { DefaultValue = "* * * *", Name = "cronname" },
                x => x.Process(),
                nameof(SomeJob),
                TimeZoneInfo.Utc,
                EnqueuedState.DefaultQueue);

            //Assert
            _recurringJobManagerMock.Verify(x => x.AddOrUpdate(It.IsAny<string>(),
                                                               It.IsAny<Job>(),
                                                               It.IsAny<string>(),
                                                               It.IsAny<RecurringJobOptions>())
                                                , Times.Once());
        }
    }

    class SomeJob
    {
        public Task Process()
        {
            return Task.CompletedTask;
        }
    }
}
