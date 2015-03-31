angular.module('virtoCommerce.orderModule')
.controller('customerOrderAddressWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.operation = {};
	$scope.openAddressesBlade = function () {
		var newBlade = {
			id: 'orderOperationAddresses',
			title: 'Manage addresses',
			currentEntities: $scope.operation.addresses,
			controller: 'coreAddressListController',
			template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};
	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
