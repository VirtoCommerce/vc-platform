angular.module('catalogModule.blades.propertyValueType', [])
.controller('propertyValueTypeController', ['$scope', function ($scope) {

    $scope.selectOption = function (option) {
        $scope.blade.parentBlade.currentEntity.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
