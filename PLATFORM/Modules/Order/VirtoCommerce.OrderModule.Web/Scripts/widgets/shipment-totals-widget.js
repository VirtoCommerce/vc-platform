angular.module('virtoCommerce.orderModule')
.controller('shipmentTotalsWidgetController', ['$scope', 'calculateTotalsService', 'bladeNavigationService', function ($scope, calculateTotalsService, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.shipment = {};

	$scope.$watch('widget.blade.currentEntity', function (shipment) {
		if (shipment) {
			calculateTotalsService.recalculateTotals(shipment);
			$scope.shipment = shipment;
		}
	}, true);
}]);
