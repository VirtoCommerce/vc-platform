angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationTreeWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.currentOperation = {};
	$scope.operation = {};
	$scope.$watch('widget.blade.customerOrder', function (operation) {
		$scope.operation = $scope.blade.customerOrder;
		$scope.currentOperation = $scope.blade.currentEntity;
	});

	$scope.selectOperation = function (operation) {
		if ($scope.currentOperation.id != operation.id) {
			$scope.currentOperation = operation;
			var newBlade = undefined;
			if (operation.operationType.toLowerCase() == 'shipment')
			{
				newBlade = {
					id: 'operationDetail',
					title: 'Shipment #' + operation.number,
					subtitle: 'Edit shipment details',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					isClosingDisabled: false,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-detail.tpl.html'
				};
			}
			else if (operation.operationType.toLowerCase() == 'customerorder') {
				newBlade = {
					id: 'operationDetail',
					title: operation.customer + '\'s Customer Order',
					subtitle: 'Edit order details and related documents',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-detail.tpl.html'
				};
			}
			else if (operation.operationType.toLowerCase() == 'paymentin') {
				newBlade = {
					id: 'operationDetail',
					title: 'Incoming payment #' + operation.number,
					subtitle: 'Edit payment details and related documents',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/payment-detail.tpl.html'
				};
			}
			if (newBlade) {
				bladeNavigationService.showBlade(newBlade);
			}
		}
	};

}]);
