angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyValueTypeController', ['$scope', function ($scope) {

	
    $scope.selectOption = function (option) {
    	$scope.blade.property.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.headIcon = 'fa-gear';

    $scope.blade.isLoading = false;
}]);
