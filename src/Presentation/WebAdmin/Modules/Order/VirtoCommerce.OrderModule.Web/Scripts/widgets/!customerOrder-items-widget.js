angular.module('virtoCommerce.orderModule.widgets', [
])
.controller('customerOrderItemsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.customerOrder = {};

	$scope.$watch('widget.blade.currentEntity', function (customerOrder) {
		if (customerOrder) {
			$scope.customerOrder = customerOrder;
		}
	});

	$scope.openItemsBlade = function () {
		var newBlade = {
			id: 'customerOrderItems',
			title: $scope.currentBlade.title + ' line items',
			subtitle: 'Edit customer order line items',
			currentEntity: $scope.currentBlade.currentEntity,
			isClosingDisabled: false,
			controller: 'customerOrderItemsController',
			template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.currentBlade);
	};

}]);
