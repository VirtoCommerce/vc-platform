angular.module('platformWebApp')
.controller('platformWebApp.resolveResultController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', '$sce', function ($rootScope, $scope, bladeNavigationService, $sce) {
	var blade = $scope.blade;
	blade.isLoading = false;
	$scope.to_trusted = function (html_code) {
		return $sce.trustAsHtml(html_code);
	}
}]);