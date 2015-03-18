angular.module('virtoCommerce.coreModule.settings')
.controller('settingsDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'settings', function ($scope, dialogService, bladeNavigationService, settings) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        // parse values as they all are strings
        settings.getSettings({ id: $scope.blade.moduleId }, function (results) {
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
                setting.value = setting.value.toLowerCase() === 'true';
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return value.toLowerCase() === 'true'; });
                }
            });

            selectedSettings = _.where(results, { isArray: true });
            _.forEach(selectedSettings, function (setting) {
                if (setting.arrayValues) {
                    setting.arrayValues = _.map(setting.arrayValues, function (value) { return { value: value }; });
                }
            });

            results = _.groupBy(results, 'groupName');
            $scope.blade.groupNames = _.keys(results);
            $scope.blade.objects = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    function isDirty() {
        return !angular.equals($scope.blade.objects, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        var objects = _.flatten(_.map($scope.blade.objects, _.values));

        var selectedSettings = _.where(objects, { isArray: true });
        _.forEach(selectedSettings, function (setting) {
            if (setting.arrayValues) {
                setting.arrayValues = _.pluck(setting.arrayValues, 'value');
            }
        });

        //console.log('saveChanges3: ' + angular.toJson(objects, true));
        settings.update({}, objects, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeHeadIco = 'fa fa-wrench';

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.objects);
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
