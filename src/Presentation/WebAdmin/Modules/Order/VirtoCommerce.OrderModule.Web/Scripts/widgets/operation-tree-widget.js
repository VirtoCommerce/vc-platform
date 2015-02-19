angular.module('virtoCommerce.orderModule.widgets')
.controller('operationTreeWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.operation = {};
	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});

	

}]);
