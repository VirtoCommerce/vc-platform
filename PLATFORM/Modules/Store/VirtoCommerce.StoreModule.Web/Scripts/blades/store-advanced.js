angular.module('virtoCommerce.storeModule.blades')
.controller('storeAdvancedController', ['$scope', 'bladeNavigationService', 'stores', 'fulfillments', 'countries', function ($scope, bladeNavigationService, stores, fulfillments, countries) {
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

    $scope.bladeHeadIco = 'fa fa-archive';

    $scope.blade.isLoading = false;
    $scope.blade.currentEntity = angular.copy($scope.blade.entity);
    $scope.blade.origEntity = $scope.blade.entity;
    $scope.fulfillmentCenters = fulfillments.query();
    $scope.countries = countries.query();
    $scope.timeZones = countries.getTimeZones();
}]);