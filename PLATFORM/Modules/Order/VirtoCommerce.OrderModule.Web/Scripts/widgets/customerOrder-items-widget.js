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
			title: $scope.blade.title + ' line items',
			subtitle: 'Edit line items',
			currentEntity: $scope.blade.currentEntity,
			isClosingDisabled: false,
			controller: 'virtoCommerce.orderModule.customerOrderItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
