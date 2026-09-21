using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Security.BackgroundJobs;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Web.Security.SignInLog;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class SignInLogCleanupJobTests
{
    [Fact]
    public async Task Process_DeletesUsingTheConfiguredRetentionWindow()
    {
        DateTime? cutoff = null;
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.DeleteOlderThan(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Callback<DateTime, int, CancellationToken>((c, _, _) => cutoff = c)
            .ReturnsAsync(0);

        await new SignInLogCleanupJob(service.Object, CreateSettings(30)).Process(TestContext.Current.CancellationToken);

        cutoff.Should().NotBeNull();
        cutoff!.Value.Should().BeCloseTo(DateTime.UtcNow.AddDays(-30), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task Process_KeepsDeletingUntilABatchComesBackShort()
    {
        var service = new Mock<IUserSignInLogService>();
        service.SetupSequence(x => x.DeleteOlderThan(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1000)
            .ReturnsAsync(1000)
            .ReturnsAsync(7);

        await new SignInLogCleanupJob(service.Object, CreateSettings(90)).Process(TestContext.Current.CancellationToken);

        service.Verify(x => x.DeleteOlderThan(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Process_RetentionDaysZeroOrLess_KeepsEverythingForever(int retentionDays)
    {
        var service = new Mock<IUserSignInLogService>();

        await new SignInLogCleanupJob(service.Object, CreateSettings(retentionDays)).Process(TestContext.Current.CancellationToken);

        // Zero or less means keep forever, never "delete everything".
        service.Verify(x => x.DeleteOlderThan(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
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
