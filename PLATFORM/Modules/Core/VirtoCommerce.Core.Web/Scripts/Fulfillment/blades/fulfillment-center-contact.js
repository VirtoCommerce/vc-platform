angular.module('virtoCommerce.coreModule.fulfillment')
.controller('virtoCommerce.coreModule.fulfillment.fulfillmentCenterContactController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.common.countries', function ($scope, bladeNavigationService, countries) {
    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }
    
    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.blade.headIcon = 'fa fa-wrench';
    
    $scope.$watch('blade.currentEntity.countryCode', function (countryCode) {
        if (countryCode) {
            $scope.blade.currentEntity.countryName = _.findWhere($scope.countries, { code: countryCode }).name;
        }
    });

    $scope.blade.isLoading = false;
    $scope.blade.currentEntity = angular.copy($scope.blade.data);
    $scope.blade.origEntity = $scope.blade.data;
    $scope.countries = countries.query();
}]);