angular.module('platformWebApp')
.controller('platformWebApp.resolveResultController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', function ($rootScope, $scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.isLoading = false;
}]);