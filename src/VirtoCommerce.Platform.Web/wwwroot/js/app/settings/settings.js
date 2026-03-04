angular.module("platformWebApp")
    .config(
        ['$stateProvider', '$provide', function ($stateProvider, $provide) {
            $stateProvider
                .state('workspace.modulesSettings', {
                    url: '/settings',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                        var blade = {
                            id: 'settings',
                            title: 'platform.blades.settingGroup-list.title',
                            //subtitle: 'Manage settings',
                            controller: 'platformWebApp.settingGroupListController',
                            template: '$(Platform)/Scripts/app/settings/blades/settingGroup-list.tpl.html',
                            isClosingDisabled: true
                        };
                        bladeNavigationService.showBlade(blade);
                    }
                    ]
                });

            $provide.decorator('platformWebApp.bladeNavigationService', [
                '$delegate', 'platformWebApp.localizableSettingService',
                function ($delegate, localizableSettingService) {
                    var showBlade = $delegate.showBlade;

                    $delegate.showBlade = function (blade, parentBlade) {
                        if (blade.template === "$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html" &&
                            localizableSettingService.isLocalizable(blade.currentEntityId))
                        {
                            blade.template = "$(Platform)/Scripts/app/settings/blades/localizable-setting-value-list.html";
                            blade.controller = "platformWebApp.localizableSettingValueListController";
                            blade.settingName = blade.currentEntityId;
                        }
                        showBlade(blade, parentBlade);
                    };
                    return $delegate;
                }]);
        }]
    )
    .run(['platformWebApp.mainMenuService', 'platformWebApp.breadcrumbHistoryService', 'platformWebApp.changeLogApi', 'platformWebApp.toolbarService', 'platformWebApp.dialogService', '$state', function (mainMenuService, breadcrumbHistoryService, changeLogApi, toolbarService, dialogService, $state) {
        //Register module in main menu
        var menuItem = {
            path: 'configuration/settings',
            icon: 'fa fa-gears',
            title: 'platform.menu.settings',
            priority: 1,
            action: function () { $state.go('workspace.modulesSettings'); },
            permission: 'platform:setting:access'
        };
        mainMenuService.addMenuItem(menuItem);

        // register back-button
        toolbarService.register(breadcrumbHistoryService.getBackButtonInstance(), 'platformWebApp.settingGroupListController');

        // Add 'Reset cache' command to settings blade
        var resetCacheCommand = {
            name: 'platform.commands.cache-reset.name',
            title: 'platform.commands.cache-reset.title',
            icon: 'fa fa-eraser',
            executeMethod: function (blade) {
                var confirmDialog = {
                    id: "confirmCacheResetDialog",
                    title: "platform.dialogs.cache-reset.title",
                    message: "platform.dialogs.cache-reset.confirm-reset-message",
                    callback: function (confirm) {
                        if (confirm) {
                            blade.isLoading = true;
                            changeLogApi.resetPlatformCache({}, function () {
                                blade.isLoading = false;
                                var successDialog = {
                                    id: "successCacheResetDialog",
                                    title: "platform.dialogs.cache-reset.title",
                                    message: "platform.dialogs.cache-reset.reset-successfully-message",
                                };
                                dialogService.showSuccessDialog(successDialog);
                            });
                        }
                    }
                }
                dialogService.showWarningDialog(confirmDialog);
            },
            canExecuteMethod: function (blade) { return !blade.isLoading; },
            permission: 'cache:reset',
            index: 2
        };
        toolbarService.register(resetCacheCommand, 'platformWebApp.settingGroupListController');
    }])

    .factory('platformWebApp.settings.helper', [function () {
        var retVal = {};

        retVal.getSetting = function (settings, settingName) {
            return _.findWhere(settings, { name: settingName });
        };

        retVal.fixValues = function (settings) {
            // parse values as they all are strings
            //var selectedSettings = _.where(settings, { valueType: 'Integer' });
            //_.forEach(selectedSettings, function (setting) {
            //    setting.value = parseInt(setting.value, 10);
            //    if (setting.allowedValues) {
            //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseInt(value, 10); });
            //    }
            //});

            //selectedSettings = _.where(settings, { valueType: 'Decimal' });
            //_.forEach(selectedSettings, function (setting) {
            //    setting.value = parseFloat(setting.value);
            //    if (setting.allowedValues) {
            //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseFloat(value); });
            //    }
            //});

            //selectedSettings = _.where(settings, { valueType: 'Boolean' });
            //_.forEach(selectedSettings, function (setting) {
            //    setting.value = setting.value && setting.value.toLowerCase() === 'true';
            //    if (setting.allowedValues) {
            //        setting.allowedValues = _.map(setting.allowedValues, function (value) { return value.toLowerCase() === 'true'; });
            //    }
            //});
        };

        retVal.toApiFormat = function (settings) {
            var selectedSettings = _.where(settings, { isDictionary: true });
            _.forEach(selectedSettings, function (setting) {
                if (setting.allowedValues) {
                    setting.allowedValues = _.pluck(setting.allowedValues, 'value');
                }
            });
        };

        retVal.getSettingValues = function getSettingValues(setting) {
            return setting.isDictionary ? [{ value: { id: setting.value, name: setting.value } }] : [{ id: setting.value, value: setting.value }];
        };

        function normalizeEmptyValue(value) {
            // Treat "no value" representations as equal for reset-icon display logic.
            // This prevents showing reset when current is '' but default is null (and vice versa).
            return (value === undefined || value === null || value === '') ? null : value;
        }

        retVal.resetToDefaultValue = function (groups, setting) {
            if (!setting || setting.isReadOnly) {
                return;
            }

            // Important: `va-generic-value-input` uses `ng-model="data"` (the whole object).
            // Mutating nested fields doesn't trigger ngModel $render(), so UI can stay stale.
            // Replace the setting object in-place inside `blade.currentEntities[...]` to force re-render.
            var groupKey = setting.groupName; // can be undefined -> coerces to 'undefined' key, which matches groupBy behavior
            var group = groups && groups[groupKey];
            var index = group ? _.findIndex(group, function (x) { return x && x.name === setting.name; }) : -1;

            var newSetting = angular.copy(setting);
            // Keep reference to allowedValues (ui-select relies on object identity within its choices list)
            newSetting.allowedValues = setting.allowedValues;

            // Keep the legacy `value` field in sync (used by some UI conditions) and
            // the `values` array in sync (used by editors and saveChanges()).
            newSetting.value = newSetting.defaultValue;

            // For allowed-values UI (`ui-select`) we want ng-model to reference an item
            newSetting.values = retVal.getSettingValues(newSetting);

            if (group && index >= 0) {
                group[index] = newSetting;
            } else {
                // Best-effort fallback (should be rare)
                angular.extend(setting, newSetting);
            }
        };

        retVal.isDefaultValue = function (setting) {
            if (!setting || setting.isDictionary || setting.isReadOnly) {
                return true;
            }

            // This blade edits scalar settings (non-dictionary). We use `values[0].value` as the effective current value
            // because `va-generic-value-input` and `ui-select` both bind through `values`.
            var current = setting.values && setting.values.length ? setting.values[0].value : setting.value;
            return angular.equals(normalizeEmptyValue(current), normalizeEmptyValue(setting.defaultValue));
        };

        return retVal;
    }]);

// dictionary Setting values management helper
function DictionarySettingDetailBlade(settingName) {
    this.id = 'dictionarySettingDetails';
    this.currentEntityId = settingName;
    this.isApiSave = true;
    this.controller = 'platformWebApp.settingDictionaryController';
    this.template = '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html';
}
