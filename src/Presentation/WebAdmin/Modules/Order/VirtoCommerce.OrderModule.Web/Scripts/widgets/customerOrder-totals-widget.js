angular.module('virtoCommerce.orderModule.widgets')
.controller('customerOrderTotalsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.customerOrder = {};

	$scope.$watch('widget.blade.currentEntity', function (customerOrder) {
		if (customerOrder) {
			$scope.customerOrder = customerOrder;
		}
	});
}]);
