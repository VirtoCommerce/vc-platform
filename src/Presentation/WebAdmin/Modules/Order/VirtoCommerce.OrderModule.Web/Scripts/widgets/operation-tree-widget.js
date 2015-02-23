angular.module('virtoCommerce.orderModule.widgets')
.controller('operationTreeWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.currentOperation = {};
	$scope.operation = {};
	$scope.$watch('widget.blade.customerOrder', function (operation) {
		$scope.operation = operation;
		$scope.currentOperation = $scope.currentBlade.operation;
	});

	$scope.selectOperation = function (operation) {
		if ($scope.currentOperation.id != operation.id) {
			$scope.currentOperation = operation;
			var newBlade = undefined;
			if (operation.operationType.toLowerCase() == 'shipment')
			{
				newBlade = {
					id: 'customerOrderDetail',
					title: 'Shipment #' + operation.number,
					subtitle: 'Edit shipment details',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntityId: operation.id,
					isClosingDisabled: false,
					controller: 'shpmentDetailController',
					template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/shipment-detail.tpl.html'
				};
			}
			else if (operation.operationType.toLowerCase() == 'customerorder') {
				newBlade = {
					id: 'customerOrderDetail',
					title: operation.customer + '\'s Customer Order',
					subtitle: 'Edit order details and related documents',
					customerOrder: $scope.widget.blade.customerOrder,
					currentEntityId: operation.id,
					controller: 'customerOrderDetailController',
					template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-detail.tpl.html'
				};
			}

			if (newBlade) {
				bladeNavigationService.showBlade(newBlade);
			}
		}
	};

}]);
