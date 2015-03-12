angular.module('virtoCommerce.customerModule.blades')
.controller('memberPropertyValueTypeController', ['$scope', function ($scope) {

    $scope.selectOption = function (option) {
        $scope.blade.parentBlade.currentEntity.valueType = option;
        $scope.blade.parentBlade.currentChild = undefined;
        $scope.bladeClose();
    };

    $scope.bladeHeadIco = 'fa fa-user';

    $scope.blade.isLoading = false;
}]);
