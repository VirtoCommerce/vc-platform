angular.module('virtoCommerce.customerModule.blades')
.controller('customerPropertyValueTypeController', ['$scope', function ($scope) {

    $scope.selectOption = function (option) {
        $scope.blade.parentBlade.currentEntity.valueType = option;
        $scope.blade.parentBlade.currentChild = undefined;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
