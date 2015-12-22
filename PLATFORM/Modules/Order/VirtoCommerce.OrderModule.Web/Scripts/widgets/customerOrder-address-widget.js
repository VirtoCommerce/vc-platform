angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.customerOrderAddressWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.operation = {};
	$scope.openAddressesBlade = function () {
		var newBlade = {
			id: 'orderOperationAddresses',
			title: 'orders.widgets.customerOrder-address.blade-title',
			currentEntities: $scope.operation.addresses,
			controller: 'virtoCommerce.coreModule.common.coreAddressListController',
			template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};
	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
