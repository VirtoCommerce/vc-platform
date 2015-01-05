angular.module('virtoCommerce.coreModule.settings.blades')
.controller('settingsDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'settings', function ($scope, dialogService, bladeNavigationService, settings) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        // parse values as they all are strings
        settings.getSettings({ moduleId: $scope.blade.moduleId }, function (results) {
            var selectedSettings = _.where(results, { valueType: 'integer' });
            _.forEach(selectedSettings, function (setting) {
                setting.value = parseInt(setting.value, 10);
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseInt(value, 10); });
                }
            });

            selectedSettings = _.where(results, { valueType: 'decimal' });
            _.forEach(selectedSettings, function (setting) {
                setting.value = parseFloat(setting.value);
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseFloat(value); });
                }
            });

            selectedSettings = _.where(results, { valueType: 'boolean' });
            _.forEach(selectedSettings, function (setting) {
                setting.value = setting.value === 'true';
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return value === 'true'; });
                }
            });

            $scope.objectsGrouped = _.groupBy(results, 'groupName');
            $scope.blade.objects = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.objects, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        settings.update({}, $scope.blade.objects, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'icon-floppy',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.objects);
                $scope.objectsGrouped = _.groupBy($scope.blade.objects, 'groupName');
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The settings has been modified. Do you want to save changes?",
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

    // actions on load
    $scope.blade.refresh();
}]);
