angular.module('virtoCommerce.storeModule.blades')
.controller('storeAdvancedController', ['$scope', 'bladeNavigationService', 'stores', 'catalogs', 'dialogService', function ($scope, bladeNavigationService, stores, catalogs, dialogService) {
    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.blade.isLoading = false;
    $scope.blade.currentEntity = angular.copy($scope.blade.entity);
    $scope.blade.origEntity = $scope.blade.entity;
    catalogs.getCatalogs({}, function (results) {
        $scope.catalogs = results;
    });
}]);