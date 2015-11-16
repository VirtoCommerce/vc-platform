angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.customerOrderItemsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.operation = {};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});

	$scope.openItemsBlade = function () {
		var newBlade = {
			id: 'customerOrderItems',
			title: 'orders.widgets.customerOrder-items.blade-title',
			titleValues: { title: $scope.blade.title },
			subtitle: 'orders.widgets.customerOrder-items.blade-subtitle',
			currentEntity: $scope.blade.currentEntity,
			isClosingDisabled: false,
			controller: 'virtoCommerce.orderModule.customerOrderItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
