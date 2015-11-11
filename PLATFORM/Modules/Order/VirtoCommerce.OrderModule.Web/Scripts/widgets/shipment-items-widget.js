angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.shipmentItemsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.operation = {};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});

	$scope.openItemsBlade = function () {
		var newBlade = {
			id: 'shipmentItems',
			title: 'orders.blades.shipment-items.title',
			title: { title: $scope.blade.title },
			subtitle: 'orders.blades.shipment-items.subtitle',
			currentEntity: $scope.blade.currentEntity,
			isClosingDisabled: false,
			controller: 'virtoCommerce.orderModule.shipmentItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
