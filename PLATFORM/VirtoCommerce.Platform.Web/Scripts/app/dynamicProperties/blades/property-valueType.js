angular.module('platformWebApp')
.controller('platformWebApp.propertyValueTypeController', ['$scope', function ($scope) {
	$scope.blade.title = 'Dynamic property value type';
	$scope.blade.subtitle = 'Change value type';

    $scope.selectOption = function (option) {
        $scope.blade.currentEntity.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
