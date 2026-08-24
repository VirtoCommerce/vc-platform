using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Jobs
{
    /// <summary>
    /// Background-job handler for module install/update/uninstall. Reconstructs the
    /// <see cref="ModulePushNotification"/> from the payload (so the existing admin UI keeps working) and performs
    /// the work with progress reporting.
    /// <para>
    /// Used both by the active engine for enqueued jobs and inline by the controller's bootstrap auto-install
    /// path (which runs before any engine may be installed — see <c>ModulesController.TryToAutoInstallModules</c>).
    /// </para>
    /// </summary>
    public class ModuleBackgroundJobHandler : IBackgroundJobHandler<ModuleBackgroundJobPayload>
    {
        private const string ManagementIsDisabledMessage = "Module management is disabled.";
        private static readonly Lock _lockObject = new();

        private readonly IModuleManagementService _moduleManagementService;
        private readonly IPushNotificationManager _pushNotifier;
        private readonly ISettingsManager _settingsManager;
        private readonly LocalStorageModuleCatalogOptions _localStorageModuleCatalogOptions;

        public ModuleBackgroundJobHandler(
            IModuleManagementService moduleManagementService,
            IPushNotificationManager pushNotifier,
            ISettingsManager settingsManager,
            IOptions<LocalStorageModuleCatalogOptions> localStorageModuleCatalogOptions)
        {
            _moduleManagementService = moduleManagementService;
            _pushNotifier = pushNotifier;
            _settingsManager = settingsManager;
            _localStorageModuleCatalogOptions = localStorageModuleCatalogOptions.Value;
        }

        public Task Execute(ModuleBackgroundJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            // Reconstruct the same notification SUBTYPE the controller created under this id: the bootstrap
            // auto-install flow uses ModuleAutoInstallPushNotification (the setup wizard dispatches on that notify
            // type), everything else uses the plain ModulePushNotification.
            ModulePushNotification notification = payload.AutoInstall
                ? new ModuleAutoInstallPushNotification(payload.Creator ?? "system")
                : new ModulePushNotification(payload.Creator ?? "system");

            notification.Id = payload.NotificationId;
            notification.Title = payload.Title;
            notification.TotalCount = payload.TotalCount;

            var options = new ModuleBackgroundJobOptions
            {
                Action = payload.Action,
                Modules = payload.Modules,
            };

            var completedSuccessfully = false;

            try
            {
                notification.Started = DateTime.UtcNow;

                // The handler owns the progress log for the shared notification id. ScheduleJob only pushes the
                // notification shell (no log entry), so emitting the "Starting…" line here keeps it from being
                // overwritten when push storage replaces the notification by id.
                notification.Description = options.Action switch
                {
                    ModuleAction.Install => "Starting installation...",
                    ModuleAction.Update => "Starting update...",
                    _ => "Starting uninstall...",
                };
                notification.ProgressLog.Add(new ProgressMessage { Level = ProgressMessageLevel.Info, Message = notification.Description });
                _pushNotifier.Send(notification);

                if (_localStorageModuleCatalogOptions.RefreshProbingFolderOnStart)
                {
                    var reportProgress = new Progress<ProgressMessage>(x =>
                    {
                        lock (_lockObject)
                        {
                            notification.Description = x.Message;
                            notification.ProgressLog.Add(x);
                            _pushNotifier.Send(notification);
                        }
                    });

                    switch (options.Action)
                    {
                        case ModuleAction.Install:
                        case ModuleAction.Update:
                            _moduleManagementService.InstallModules(options.Modules, reportProgress);
                            break;
                        case ModuleAction.Uninstall:
                            _moduleManagementService.UninstallModules(options.Modules.Select(x => x.Id).ToList(), reportProgress);
                            break;
                    }

                    completedSuccessfully = true;
                }
                else
                {
                    notification.Finished = DateTime.UtcNow;
                    notification.Description = ManagementIsDisabledMessage;
                    notification.ProgressLog.Add(new ProgressMessage
                    {
                        Level = ProgressMessageLevel.Error,
                        Message = notification.Description,
                    });
                    _pushNotifier.Send(notification);
                }
            }
            catch (Exception ex)
            {
                // Record the failure as the terminal state. The finally block must NOT overwrite this with a
                // "finished." success message, so the description is set here and the success branch is gated on
                // completedSuccessfully.
                notification.Description = options.Action switch
                {
                    ModuleAction.Install => "Installation failed.",
                    ModuleAction.Update => "Update failed.",
                    _ => "Uninstall failed."
                };
                notification.ProgressLog.Add(new ProgressMessage
                {
                    Level = ProgressMessageLevel.Error,
                    Message = ex.ToString(),
                });
            }
            finally
            {
                _settingsManager.SetValue(PlatformConstants.Settings.Setup.ModulesAutoInstallState.Name, AutoInstallState.Completed);

                notification.Finished = DateTime.UtcNow;

                // Only report success when the work actually completed. On failure (catch) or when module
                // management is disabled, the description/error already recorded above is preserved.
                if (completedSuccessfully)
                {
                    notification.Description = options.Action switch
                    {
                        ModuleAction.Install => "Installation finished.",
                        ModuleAction.Update => "Updating finished.",
                        _ => "Uninstalling finished."
                    };

                    notification.ProgressLog.Add(new ProgressMessage
                    {
                        Level = ProgressMessageLevel.Info,
                        Message = notification.Description,
                    });
                }

                _pushNotifier.Send(notification);
            }

            return Task.CompletedTask;
        }
    }
}
