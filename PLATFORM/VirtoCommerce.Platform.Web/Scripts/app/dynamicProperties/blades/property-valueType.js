angular.module('platformWebApp')
.controller('platformWebApp.propertyValueTypeController', ['$scope', function ($scope) {
	
    $scope.selectOption = function (option) {
        $scope.blade.currentEntity.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
