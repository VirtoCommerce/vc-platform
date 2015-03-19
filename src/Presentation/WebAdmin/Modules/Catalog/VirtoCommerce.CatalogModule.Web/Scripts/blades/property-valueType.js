angular.module('virtoCommerce.catalogModule')
.controller('propertyValueTypeController', ['$scope', function ($scope) {

	
    $scope.selectOption = function (option) {
    	$scope.blade.property.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
