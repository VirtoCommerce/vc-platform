angular.module('virtoCommerce.orderModule')
.controller('customerOrderTotalsWidgetController', ['$scope', 'calculateTotalsService', 'bladeNavigationService', function ($scope, calculateTotalsService, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.customerOrder = {};

	$scope.$watch('widget.blade.currentEntity', function (customerOrder) {
		if (customerOrder) {
			calculateTotalsService.recalculateTotals(customerOrder);
			$scope.customerOrder = customerOrder;
		}
	}, true);
}]);
