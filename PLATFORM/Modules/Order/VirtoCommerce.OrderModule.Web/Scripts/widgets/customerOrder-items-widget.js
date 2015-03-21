angular.module('virtoCommerce.orderModule')
.controller('customerOrderItemsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.customerOrder = {};

	$scope.$watch('widget.blade.currentEntity', function (customerOrder) {
		$scope.customerOrder = customerOrder;
	});

	$scope.openItemsBlade = function () {
		var newBlade = {
			id: 'customerOrderItems',
			title: $scope.blade.title + ' line items',
			subtitle: 'Edit customer order line items',
			currentEntity: $scope.blade.currentEntity,
			isClosingDisabled: false,
			controller: 'customerOrderItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
