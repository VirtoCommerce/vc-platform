angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyTypeController', ['$scope', function ($scope) {

    $scope.selectOption = function (option) {
        $scope.blade.parentBlade.currentEntity.type = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
