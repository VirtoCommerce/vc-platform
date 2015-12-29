angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.newOperationWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.orderModule.order_res_customerOrders', function ($scope, bladeNavigationService, dialogService, order_res_customerOrders) {
    var blade = $scope.blade;

    var shipmentOperation =
		{
		    name: 'orders.blades.newOperation-wizard.menu.shipment-operation.title',
		    descr: 'orders.blades.newOperation-wizard.menu.shipment-operation.description',
		    action: function () {

		        order_res_customerOrders.getNewShipment({ id: blade.customerOrder.id }, function (result) {
		            bladeNavigationService.closeBlade(blade);

		            result.shippingMethod = undefined;
		            blade.customerOrder.shipments.push(result);
		            blade.customerOrder.childrenOperations.push(result);

		            var newBlade = {
		                id: 'operationDetail',
		                title: 'orders.blades.shipment-detail.title',
		                titleValues: { number: result.number },
		                subtitle: 'orders.blades.shipment-detail.subtitle',
		                stores: blade.stores,
		                isNew: true,
		                customerOrder: blade.customerOrder,
		                currentEntity: result,
		                controller: 'virtoCommerce.orderModule.operationDetailController',
		                template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-detail.tpl.html'
		            };

		            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
		        },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
		    }
		};

    var paymentOperation =
	{
	    name: 'orders.blades.newOperation-wizard.menu.payment-operation.title',
	    descr: 'orders.blades.newOperation-wizard.menu.payment-operation.description',
	    action: function () {

	        order_res_customerOrders.getNewPayment({ id: blade.customerOrder.id }, function (result) {
	            bladeNavigationService.closeBlade(blade);

	            result.paymentMethod = undefined;
	            blade.customerOrder.inPayments.push(result);
	            blade.customerOrder.childrenOperations.push(result);

	            var newBlade = {
	                id: 'operationDetail',
	                title: 'orders.blades.payment-detail.title',
	                titleValues: { number: result.number },
	                subtitle: 'orders.blades.payment-detail.subtitle',
	                customerOrder: blade.customerOrder,
	                currentEntity: result,
	                stores: blade.stores,
	                isNew: true,
	                controller: 'virtoCommerce.orderModule.operationDetailController',
	                template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/payment-detail.tpl.html'
	            };

	            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
	        },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	    }
	};

    $scope.availOperationsMap = {
        'customerorder': [shipmentOperation, paymentOperation],
        'shipment': [],
        'payment': []
    };

    $scope.availableOperations = $scope.availOperationsMap[blade.currentEntity.operationType.toLowerCase()];
    blade.isLoading = false;
}]);
