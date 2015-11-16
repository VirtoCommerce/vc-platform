angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.newOperationWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.orderModule.order_res_customerOrders', function ($scope, bladeNavigationService, dialogService, order_res_customerOrders) {

	$scope.blade.isLoading = false;
	var shipmentOperation =
		{
		    name: 'orders.blades.newOperation-wizard.menu.shipment-operation.title',
		    descr: 'orders.blades.newOperation-wizard.menu.shipment-operation.description',
			action: function () {

				order_res_customerOrders.getNewShipment({ id: $scope.blade.customerOrder.id }, function (result) {

					bladeNavigationService.closeBlade($scope.blade);

					$scope.blade.customerOrder.shipments.push(result);
					$scope.blade.customerOrder.childrenOperations.push(result);

					var newBlade = {
						id: 'operationDetail',
						title: 'orders.blades.shipment-detail.title',
						titleValues: { number: result.number },
						subtitle: 'orders.blades.shipment-detail.subtitle',
						isNew: true,
						customerOrder: $scope.blade.customerOrder,
						currentEntity: result,
						isClosingDisabled: false,
						controller: 'virtoCommerce.orderModule.operationDetailController',
						template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-detail.tpl.html'
					};
				
					bladeNavigationService.showBlade(newBlade);
				},
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
			}
		};

	var paymentOperation =
	{
	    name: 'orders.blades.newOperation-wizard.menu.payment-operation.title',
	    descr: 'orders.blades.newOperation-wizard.menu.payment-operation.description',
	    action: function () {

			order_res_customerOrders.getNewPayment({ id: $scope.blade.customerOrder.id }, function (result) {

				bladeNavigationService.closeBlade($scope.blade);

				$scope.blade.customerOrder.inPayments.push(result);
				$scope.blade.customerOrder.childrenOperations.push(result);

				var newBlade = {
					id: 'operationDetail',
					title: 'orders.blades.payment-detail.title',
					titleValues: { number: result.number },
					subtitle: 'orders.blades.payment-detail.subtitle',
					customerOrder: $scope.blade.customerOrder,
					currentEntity: result,
					isNew: true,
					controller: 'virtoCommerce.orderModule.operationDetailController',
					template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/payment-detail.tpl.html'
				};
			
				bladeNavigationService.showBlade(newBlade);
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
	};

	$scope.availOperationsMap = {
		'customerorder': [shipmentOperation, paymentOperation],
		'shipment': [],
		'payment': []
	};

	$scope.getAvailOperations = function () {
		return $scope.availOperationsMap[$scope.blade.currentEntity.operationType.toLowerCase()];
	};



}]);


