angular.module('platformWebApp').controller('platformWebApp.settingsDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.settings.helper', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', '$q', function ($scope, dialogService, settingsHelper, bladeNavigationService, settingsApi, $q) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:setting:update';

    blade.refresh = function () {
        if (blade.moduleId && !blade.data) {
            blade.isLoading = true;

            settingsApi.getSettings({ id: blade.moduleId }, initializeBlade);
        } else {
            initializeBlade(angular.copy(blade.data));
        }
    }

    function getSettingValues(setting) {
        return setting.isDictionary ? [{ value: { id: setting.value, name: setting.value } }] : [{ id: setting.value, value: setting.value }];
    }

    function initializeBlade(settings) {
        _.each(settings, function (setting) {
            // set group names to show.
            if (setting.groupName) {
                var paths = setting.groupName.split('|');
                setting.groupName = paths.pop();
            }

            // transform to va-generic-value-input suitable structure
            if (!setting.value && setting.defaultValue) {
                setting.value = setting.defaultValue;
            }

            setting.values = getSettingValues(setting);

            if (setting.allowedValues) {
                setting.allowedValues = _.map(setting.allowedValues, function (x) {
                    return { value: x };
                });
            }
        });

        settings = _.groupBy(settings, 'groupName');
        blade.groupNames = _.keys(settings);
        blade.currentEntities = angular.copy(settings);
        blade.origEntity = settings;
        blade.isLoading = false;
    }

    $scope.resetToDefaultValue = function (setting) {
        if (!setting || setting.isReadOnly) {
            return;
        }

        // Important: `va-generic-value-input` uses `ng-model="data"` (the whole object).
        // Mutating nested fields doesn't trigger ngModel $render(), so UI can stay stale.
        // Replace the setting object in-place inside `blade.currentEntities[...]` to force re-render.
        var groupKey = setting.groupName; // can be undefined -> coerces to 'undefined' key, which matches groupBy behavior
        var group = blade.currentEntities && blade.currentEntities[groupKey];
        var index = group ? _.findIndex(group, function (x) { return x && x.name === setting.name; }) : -1;

        var newSetting = angular.copy(setting);
        // Keep reference to allowedValues (ui-select relies on object identity within its choices list)
        newSetting.allowedValues = setting.allowedValues;

        // Keep the legacy `value` field in sync (used by some UI conditions) and
        // the `values` array in sync (used by editors and saveChanges()).
        newSetting.value = newSetting.defaultValue;

        // For allowed-values UI (`ui-select`) we want ng-model to reference an item
        newSetting.values = getSettingValues(newSetting);

        if (group && index >= 0) {
            group[index] = newSetting;
        } else {
            // Best-effort fallback (should be rare)
            angular.extend(setting, newSetting);
        }
    };

    function normalizeEmptyValue(value) {
        // Treat "no value" representations as equal for reset-icon display logic.
        // This prevents showing reset when current is '' but default is null (and vice versa).
        return (value === undefined || value === null || value === '') ? null : value;
    }

    $scope.isDefaultValue = function (setting) {
        if (!setting || setting.isDictionary || setting.isReadOnly) {
            return true;
        }

        // This blade edits scalar settings (non-dictionary). We use `values[0].value` as the effective current value
        // because `va-generic-value-input` and `ui-select` both bind through `values`.
        var current = setting.values && setting.values.length ? setting.values[0].value : setting.value;
        return angular.equals(normalizeEmptyValue(current), normalizeEmptyValue(setting.defaultValue));
    };

    $scope.editArray = function (node) {
        var newBlade = {
            id: "settingDetailChild",
            currentEntityId: node.name,
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    blade.saveChanges = function () {
        blade.isLoading = true;
        var deferred = $q.defer();

        var objects = _.flatten(_.map(blade.currentEntities, _.values));
        objects = _.map(objects, function (x) {
            x.value = x.isDictionary ? x.values[0].value.id : x.values[0].value;
            return x;
        });

        settingsHelper.toApiFormat(objects);

        settingsApi.update({}, objects, function () {
            if (blade.moduleId) {
                blade.data = undefined;
                blade.refresh();
            } else {
                blade.origEntity = blade.currentEntities;
                blade.parentBlade.refresh(true);
            }
            deferred.resolve();
        });

        return deferred.promise;
    };

    blade.headIcon = 'fa fa-wrench';
    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fas fa-save',
            executeMethod: blade.saveChanges,
            canExecuteMethod: canSave
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                blade.currentEntities = angular.copy(blade.origEntity);
            },
            canExecuteMethod: isDirty
        }
    ];

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback, "platform.dialogs.settings-delete.title", "platform.dialogs.settings-delete.message");
    };

    $scope.getDictionaryValues = function (setting, callback) {
        callback(setting.allowedValues);
    };

    // actions on load
    blade.refresh();
}]);
