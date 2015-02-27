angular.module('virtoCommerce.orderModule.wizards')
.controller('newOperationWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'order_res_customerOrders', function ($scope, bladeNavigationService, dialogService, order_res_customerOrders) {

	$scope.blade.isLoading = false;
	var shipmentOperation =
		{
			name: 'Shipment',
			descr: 'Add new shipment',
			action: function () {

				order_res_customerOrders.getNewShipment({ id: $scope.blade.customerOrder.id }, function (result) {

					$scope.blade.customerOrder.shipments.push(result);
					$scope.blade.customerOrder.childrenOperations.push(result);

					var newBlade = {
						id: 'operationDetail',
						title: 'Shipment #' + result.number,
						subtitle: 'Edit shipment details',
						noRefresh: true,
						customerOrder: $scope.blade.customerOrder,
						currentEntity: result,
						isClosingDisabled: false,
						controller: 'operationDetailController',
						template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/shipment-detail.tpl.html'
					};
					bladeNavigationService.closeBlade($scope.blade);
					bladeNavigationService.showBlade(newBlade);
				});
			}
		};

	var paymentOperation =
	{
		name: 'Incoming payment',
		descr: 'Add new incoming payment',

	};

	$scope.availOperationsMap = {
		'customerorder': [shipmentOperation, paymentOperation],
		'shipment': [paymentOperation],
		'payment': []
	};

	$scope.getAvailOperations = function () {
		return $scope.availOperationsMap[$scope.blade.currentEntity.operationType.toLowerCase()];
	};



}]);


