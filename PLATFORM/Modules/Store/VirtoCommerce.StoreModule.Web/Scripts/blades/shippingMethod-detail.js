angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.shippingMethodDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, settings) {

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;
        $scope.blade.title = data.name;

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'VirtoCommerce.Core.General.TaxTypes',
            title: 'Tax types',
            parentRefresh: function (data) { $scope.taxTypes = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: 'Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.taxTypes = settings.getValues({ id: 'VirtoCommerce.Core.General.TaxTypes' });

    $scope.blade.headIcon = 'fa-archive';

    initializeBlade($scope.blade.origEntity);
}]);