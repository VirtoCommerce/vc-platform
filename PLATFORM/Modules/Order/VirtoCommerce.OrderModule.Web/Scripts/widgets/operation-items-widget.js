angular.module('virtoCommerce.orderModule')
.controller('operationItemsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
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
			controller: 'operationItemsController',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

}]);
