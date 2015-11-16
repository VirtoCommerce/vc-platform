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
					title: 'orders.blades.shipment-detail.title',
					titleValues: { number: operation.number },
					subtitle: 'orders.blades.shipment-detail.subtitle',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					isClosingDisabled: false,
					disableOpenAnimation: true,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-detail.tpl.html'
				};
			}
			else if (operation.operationType.toLowerCase() == 'customerorder') {
				newBlade = {
					id: 'operationDetail',
					title: 'orders.blades.customerOrder-detail.title',
					titleValues: { customer: customer },
					subtitle: 'orders.blades.customerOrder-detail.subtitle',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					disableOpenAnimation: true,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-detail.tpl.html'
				};
			}
			else if (operation.operationType.toLowerCase() == 'paymentin') {
				newBlade = {
					id: 'operationDetail',
					title: 'orders.blades.payment-detail.title',
					titleValues: { number: operation.number },
					subtitle: 'orders.blades.payment-detail.subtitle',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntity: operation,
					disableOpenAnimation: true,
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
