//angular.module('platformWebApp')
//.controller('platformWebApp.entitySettingListController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', function ($scope, dialogService, bladeNavigationService) {
//    var blade = $scope.blade;
//    blade.updatePermission = 'platform:setting:update';
//    blade.title = 'Settings';

//    function initializeBlade(results) {
//        blade.data = results;

//        // parse values as they all are strings
//        var selectedSettings = _.where(results, { valueType: 'Integer' });
//        _.forEach(selectedSettings, function (setting) {
//            setting.value = parseInt(setting.value, 10);
//            if (setting.allowedValues) {
//                setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseInt(value, 10); });
//            }
//        });

//        selectedSettings = _.where(results, { valueType: 'Decimal' });
//        _.forEach(selectedSettings, function (setting) {
//            setting.value = parseFloat(setting.value);
//            if (setting.allowedValues) {
//                setting.allowedValues = _.map(setting.allowedValues, function (value) { return parseFloat(value); });
//            }
//        });

//        selectedSettings = _.where(results, { valueType: 'Boolean' });
//        _.forEach(selectedSettings, function (setting) {
//            setting.value = setting.value.toLowerCase() === 'true';
//            if (setting.allowedValues) {
//                setting.allowedValues = _.map(setting.allowedValues, function (value) { return value.toLowerCase() === 'true'; });
//            }
//        });

//        selectedSettings = _.where(results, { isArray: true });
//        _.forEach(selectedSettings, function (setting) {
//            if (setting.arrayValues) {
//                setting.arrayValues = _.map(setting.arrayValues, function (value) { return { value: value }; });
//            }
//        });

//        results = _.groupBy(results, 'groupName');
//        blade.groupNames = _.keys(results);
//        blade.currentEntities = angular.copy(results);
//        blade.origEntity = results;
//        blade.isLoading = false;
//    };

//    $scope.editArray = function (node) {
//        var newBlade = {
//            id: "settingDetailChild",
//            currentEntityId: node.name,
//            controller: 'platformWebApp.settingDictionaryController',
//            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
//        };
//        bladeNavigationService.showBlade(newBlade, blade);
//    }

//    function isDirty() {
//        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
//    };

//    $scope.cancelChanges = function () {
//        angular.copy(blade.origEntity, blade.currentEntities);
//        $scope.bladeClose();
//    }

//    $scope.saveChanges = function () {
//        if (!blade.hasUpdatePermission()) return;

//        blade.isLoading = true;
//        var objects = _.flatten(_.map(blade.currentEntities, _.values));

//        var selectedSettings = _.where(objects, { isArray: true });
//        _.forEach(selectedSettings, function (setting) {
//            if (setting.arrayValues) {
//                setting.arrayValues = _.pluck(setting.arrayValues, 'value');
//            }
//        });

//        //console.log('saveChanges3: ' + angular.toJson(objects, true));
//        angular.copy(objects, blade.data);
//        angular.copy(blade.currentEntities, blade.origEntity);
//        $scope.bladeClose();
//    };

//    $scope.blade.headIcon = 'fa-wrench';
//    $scope.blade.toolbarCommands = [
//        {
//            name: "platform.commands.reset", icon: 'fa fa-undo',
//            executeMethod: function () {
//                blade.currentEntities = angular.copy(blade.origEntity);
//            },
//            canExecuteMethod: isDirty,
//            permission: blade.updatePermission
//        }
//    ];

//    blade.onClose = function (closeCallback) {
//        if (isDirty()) {
//            var dialog = {
//                id: "confirmItemChange",
//                title: "platform.dialogs.settings-save.title",
//                message: "platform.dialogs.settings-save.message",
//                callback: function (needSave) {
//                    if (needSave) {
//                        $scope.saveChanges();
//                    }
//                    closeCallback();
//                }
//            };
//            dialogService.showConfirmationDialog(dialog);
//        }
//        else {
//            closeCallback();
//        }
//    };

//    // actions on load
//    $scope.$watch('blade.parentBlade.currentEntity.settings', initializeBlade);
//}]);
