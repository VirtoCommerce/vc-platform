angular.module('virtoCommerce.orderModule')
.controller('operationCommentWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.operation = {};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
