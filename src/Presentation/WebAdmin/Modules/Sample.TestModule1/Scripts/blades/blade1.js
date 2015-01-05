angular.module('testModule1.blades.blade1', [])
.controller('tm1-blade1Controller', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

	$scope.data = "testModule1 content";
	$scope.blade.title = "testModule1 title";
    $scope.blade.isLoading = false;
}]);
