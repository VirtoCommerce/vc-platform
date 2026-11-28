using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Jobs;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Jobs;

public class ModuleBackgroundJobHandlerTests
{
    [Fact]
    public async Task Execute_Install_ReportsStartProgressAndFinishOnTheSameNotification()
    {
        var sent = new List<string>();
        var pushNotifier = new Mock<IPushNotificationManager>();
        pushNotifier.Setup(x => x.SendAsync(It.IsAny<PushNotification>())).Callback<PushNotification>(x => sent.Add(x.Description)).Returns(Task.CompletedTask);
        pushNotifier.Setup(x => x.Send(It.IsAny<PushNotification>())).Callback<PushNotification>(x => sent.Add(x.Description));
        var modules = new Mock<IModuleManagementService>();
        var handler = CreateHandler(modules.Object, pushNotifier.Object, managementEnabled: true);

        await handler.Execute(Payload(ModuleAction.Install), Mock.Of<IJobExecutionContext>(), TestContext.Current.CancellationToken);

        sent.First().Should().Be("Starting installation...");
        sent.Last().Should().Be("Installation finished.");
        pushNotifier.Verify(x => x.SendAsync(It.Is<ModulePushNotification>(n => n.Id == "n1" && n.Finished != null)), Times.AtLeastOnce);
        modules.Verify(x => x.InstallModules(It.Is<IList<ModuleInstallRequest>>(r => r.Single().Id == "A"), It.IsAny<IProgress<ProgressMessage>>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ManagementDisabled_FinishesWithTheErrorAndInstallsNothing()
    {
        ModulePushNotification last = null;
        var pushNotifier = new Mock<IPushNotificationManager>();
        pushNotifier.Setup(x => x.SendAsync(It.IsAny<PushNotification>())).Callback<PushNotification>(x => last = (ModulePushNotification)x).Returns(Task.CompletedTask);
        var modules = new Mock<IModuleManagementService>(MockBehavior.Strict);
        var handler = CreateHandler(modules.Object, pushNotifier.Object, managementEnabled: false);

        await handler.Execute(Payload(ModuleAction.Install), Mock.Of<IJobExecutionContext>(), TestContext.Current.CancellationToken);

        last.Finished.Should().NotBeNull();
        last.Description.Should().Be("Module management is disabled.");
        last.ErrorCount.Should().Be(1);
    }

    private static ModuleBackgroundJobPayload Payload(ModuleAction action)
    {
        return new ModuleBackgroundJobPayload
        {
            Action = action,
            Modules = [new ModuleInstallRequest("A")],
            NotificationId = "n1",
            Creator = "admin",
            Title = "Install Module",
            TotalCount = 1,
        };
    }

    private static ModuleBackgroundJobHandler CreateHandler(IModuleManagementService modules, IPushNotificationManager pushNotifier, bool managementEnabled)
    {
        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetObjectSettingAsync(It.IsAny<string>(), null, null)).ReturnsAsync(new ObjectSettingEntry());

        return new ModuleBackgroundJobHandler(
            modules,
            pushNotifier,
            settings.Object,
            Options.Create(new LocalStorageModuleCatalogOptions { RefreshProbingFolderOnStart = managementEnabled }));
    }
}
