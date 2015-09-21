angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.shippingMethodDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, settings) {
    var blade = $scope.blade;

    function initializeBlade(data) {
        blade.currentEntityId = data.id;
        blade.title = data.name;

        _.each(data.settings, function (setting) {
            // transform to va-generic-value-input suitable structure
            setting.isDictionary = _.any(setting.allowedValues);
            setting.values = setting.isDictionary ? [{ value: { id: setting.value, name: setting.value } }] : [{ id: setting.value, value: setting.value }];
            if (setting.allowedValues) {
                setting.allowedValues = _.map(setting.allowedValues, function (x) {
                    return { id: x, name: x };
                });
            }
        });

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        blade.currentEntity.settings = _.map(blade.currentEntity.settings, function (x) {
            x.value = x.isDictionary ? x.values[0].value.id : x.values[0].value;
            x.values = undefined;
            return x;
        });

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.getDictionaryValues = function (setting, callback) {
        callback(setting.allowedValues);
    };

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'VirtoCommerce.Core.General.TaxTypes',
            title: 'Tax types',
            parentRefresh: function (data) { $scope.taxTypes = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
    
    $scope.taxTypes = settings.getValues({ id: 'VirtoCommerce.Core.General.TaxTypes' });

    blade.headIcon = 'fa-archive';

    initializeBlade(blade.origEntity);
}]);