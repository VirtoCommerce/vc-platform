angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.newStoreWizardController', ['$scope', 'bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'settings', 'dialogService', function ($scope, bladeNavigationService, stores, catalogs, settings, dialogService) {

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        stores.save({}, $scope.blade.currentEntity, function (data) {
            $scope.blade.parentBlade.refresh();
            $scope.blade.parentBlade.openBlade(data);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.catalogs = catalogs.getCatalogs();
    $scope.storeStates = settings.getValues({ id: 'Stores.States' });
    initializeBlade($scope.blade.currentEntity);
}]);