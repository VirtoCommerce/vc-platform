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

    function initializeBlade(settings) {
        _.each(settings, function (setting) {
            // set group names to show.
            if (setting.groupName) {
                var paths = setting.groupName.split('|');
                setting.groupName = paths.pop();
            }

            // transform to va-generic-value-input suitable structure
            if (setting.value == undefined && setting.defaultValue) {
                setting.value = setting.defaultValue;
            }
            setting.values = setting.isDictionary ? [{ value: { id: setting.value, name: setting.value } }] : [{ id: setting.value, value: setting.value }];
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

    blade.headIcon = 'fa-wrench';
    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
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
