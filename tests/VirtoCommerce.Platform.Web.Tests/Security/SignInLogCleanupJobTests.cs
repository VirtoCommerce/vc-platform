using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Security.BackgroundJobs;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class SignInLogCleanupJobTests
{
    [Fact]
    public async Task Process_DeletesUsingTheConfiguredRetentionWindow()
    {
        DateTime? cutoff = null;
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
            .Callback<DateTime, int>((c, _) => cutoff = c)
            .ReturnsAsync(0);

        await new SignInLogCleanupJob(service.Object, CreateSettings(30)).Process();

        cutoff.Should().NotBeNull();
        cutoff!.Value.Should().BeCloseTo(DateTime.UtcNow.AddDays(-30), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task Process_KeepsDeletingUntilABatchComesBackShort()
    {
        var service = new Mock<IUserSignInLogService>();
        service.SetupSequence(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
            .ReturnsAsync(1000)
            .ReturnsAsync(1000)
            .ReturnsAsync(7);

        await new SignInLogCleanupJob(service.Object, CreateSettings(90)).Process();

        service.Verify(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Process_RetentionDaysZeroOrLess_DeletesNothing()
    {
        var service = new Mock<IUserSignInLogService>();

        await new SignInLogCleanupJob(service.Object, CreateSettings(0)).Process();

        // A misconfigured retention of 0 must not be read as "delete everything".
        service.Verify(x => x.DeleteOlderThanAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
    }

    private static ISettingsManager CreateSettings(int retentionDays)
    {
        // GetValueAsync is an extension method; GetObjectSettingAsync is the mockable seam beneath it.
        var settings = new Mock<ISettingsManager>();
        settings
            .Setup(x => x.GetObjectSettingAsync(
                PlatformConstants.Settings.Security.SignInLogRetentionDays.Name, null, null))
            .ReturnsAsync(new ObjectSettingEntry { Value = retentionDays });

        return settings.Object;
    }
}
