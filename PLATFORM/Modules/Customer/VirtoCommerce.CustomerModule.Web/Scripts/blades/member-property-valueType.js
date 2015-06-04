angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPropertyValueTypeController', ['$scope', function ($scope) {

    $scope.selectOption = function (option) {
        $scope.blade.parentBlade.currentEntity.valueType = option;
        $scope.blade.parentBlade.currentChild = undefined;
        $scope.bladeClose();
    };

    $scope.blade.headIcon = 'fa fa-user';

    $scope.blade.isLoading = false;
}]);
