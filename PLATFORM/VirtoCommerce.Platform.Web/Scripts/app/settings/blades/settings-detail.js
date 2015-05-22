angular.module('platformWebApp')
.controller('platformWebApp.settingsDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.objCompareService', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($scope, dialogService, objCompareService, bladeNavigationService, settings) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        // parse values as they all are strings
        settings.getSettings({ id: $scope.blade.moduleId }, function (results) {
            var selectedSettings = _.where(results, { valueType: 'Integer' });
            _.forEach(selectedSettings, function (setting) {
                setting.value = parseInt(setting.value, 10);
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseInt(value, 10); });
                }
            });

            selectedSettings = _.where(results, { valueType: 'Decimal' });
            _.forEach(selectedSettings, function (setting) {
                setting.value = parseFloat(setting.value);
                if (setting.allowedValues) {
                    setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseFloat(value); });
                }
            });

            selectedSettings = _.where(results, { valueType: 'Boolean' });
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
            $scope.blade.currentEntities = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.editArray = function (node) {
        var newBlade = {
            id: "settingDetailChild",
            currentEntityId: node.name,
            title: $scope.blade.title,
            subtitle: 'Manage dictionary values',
            controller: 'platformWebApp.settingDictionaryController',
            template: 'Scripts/app/settings/blades/setting-dictionary.tpl.html'
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

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeHeadIco = 'fa fa-wrench';
    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
            },
            permission: 'platform:setting:manage'
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                $scope.blade.currentEntities = angular.copy($scope.blade.origEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:setting:manage'
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
