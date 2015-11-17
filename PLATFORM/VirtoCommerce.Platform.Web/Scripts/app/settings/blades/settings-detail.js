angular.module('platformWebApp')
.controller('platformWebApp.settingsDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.settings.helper', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($scope, dialogService, settingsHelper, bladeNavigationService, settings) {

    $scope.blade.refresh = function () {
        if ($scope.blade.moduleId) {
            $scope.blade.isLoading = true;

            settings.getSettings({ id: $scope.blade.moduleId }, initializeBlade,
            function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        } else {
            initializeBlade(angular.copy($scope.blade.data));
        }
    }

    function initializeBlade(results) {
        settingsHelper.fixValues(results);

        _.each(results, function (setting) {
            // set group names to show.
            if (setting.groupName) {
                var paths = setting.groupName.split('|');
                setting.groupName = paths.pop();
            }

            // transform to va-generic-value-input suitable structure
            setting.isDictionary = _.any(setting.allowedValues);
            setting.values = setting.isDictionary ? [{ value: { id: setting.value, name: setting.value } }] : [{ id: setting.value, value: setting.value }];
            if (setting.allowedValues) {
                setting.allowedValues = _.map(setting.allowedValues, function (x) {
                    return { id: x, name: x };
                });
            }
        });


        results = _.groupBy(results, 'groupName');
        $scope.blade.groupNames = _.keys(results);
        $scope.blade.currentEntities = angular.copy(results);
        $scope.blade.origEntity = results;
        $scope.blade.isLoading = false;
    }

    $scope.editArray = function (node) {
        var newBlade = {
            id: "settingDetailChild",
            currentEntityId: node.name,
            title: $scope.blade.title,
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        var objects = _.flatten(_.map($scope.blade.currentEntities, _.values));
        objects = _.map(objects, function (x) {
            x.value = x.isDictionary ? x.values[0].value.id : x.values[0].value;
            x.values = undefined;
            return x;
        });

        var selectedSettings = _.where(objects, { isArray: true });
        _.forEach(selectedSettings, function (setting) {
            if (setting.arrayValues) {
                setting.arrayValues = _.pluck(setting.arrayValues, 'value');
            }
        });

        //console.log('saveChanges3: ' + angular.toJson(objects, true));
        settings.update({}, objects, function (data, headers) {
            if ($scope.blade.moduleId) {
                $scope.blade.refresh();
            } else {
                $scope.blade.origEntity = $scope.blade.currentEntities;
                $scope.blade.parentBlade.refresh(true);
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.blade.headIcon = 'fa-wrench';
    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
            },
            permission: 'platform:setting:update'
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                $scope.blade.currentEntities = angular.copy($scope.blade.origEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:setting:update'
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "platform.dialogs.settings-delete.title",
                message: "platform.dialogs.settings-delete.message",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.getDictionaryValues = function (setting, callback) {
        callback(setting.allowedValues);
    }

    // actions on load
    $scope.blade.refresh();
}]);
