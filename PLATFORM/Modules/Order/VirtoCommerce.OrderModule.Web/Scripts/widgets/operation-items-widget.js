angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationItemsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.operation = {};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});

	$scope.openItemsBlade = function () {
		var newBlade = {
			id: 'operationItems',
			title: $scope.blade.title + ' line items',
			subtitle: 'Edit line items',
			currentEntity: $scope.blade.currentEntity,
			isClosingDisabled: false,
			controller: 'virtoCommerce.orderModule.operationItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
