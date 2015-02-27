angular.module('virtoCommerce.orderModule.blades')
.controller('operationDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'order_res_customerOrders', 'notificationService', 'order_res_fulfilmentCenters', 'order_res_stores', 'order_res_paymentGateways',
			function ($scope, dialogService, bladeNavigationService, order_res_customerOrders, notificationService, order_res_fulfilmentCenters, order_res_stores, order_res_paymentGateways) {

		$scope.blade.refresh = function (noRefresh) {
    	$scope.blade.isLoading = true;
    	$scope.fulfillmentCenters = [];
    	$scope.stores = [];
    	$scope.paymentGateways = [];

    	if (!noRefresh) {
    		order_res_customerOrders.get({ id: $scope.blade.customerOrder.id }, function (results) {
    			initialize(results);
    		},
			function (error) {
				notificationService.setError('Error ' + error.status, $scope.blade);
			});
    	}
    	else
    	{
    		initialize($scope.blade.customerOrder);
    	}
    }

    function initialize(customerOrder) {

    	var operation = angular.isDefined($scope.blade.currentEntity) ? $scope.blade.currentEntity : customerOrder;
    	var copy = angular.copy(customerOrder);

    	$scope.blade.customerOrder = copy;

    	if (operation.operationType.toLowerCase() == 'customerorder') {
    		$scope.blade.currentEntity = copy;
    		$scope.blade.origEntity = customerOrder;
    		$scope.stores = order_res_stores.query();
    	}
    	else if (operation.operationType.toLowerCase() == 'shipment') {
    		$scope.blade.currentEntity = _.find(copy.shipments, function (x) { return x.id == operation.id; });
    		$scope.blade.origEntity = _.find(customerOrder.shipments, function (x) { return x.id == operation.id; });
    		$scope.fulfillmentCenters = order_res_fulfilmentCenters.query();
    	}
    	else if (operation.operationType.toLowerCase() == 'paymentin') {
    		$scope.paymentGateways = order_res_paymentGateways.query();
    		$scope.blade.currentEntity = _.find(copy.inPayments, function (x) { return x.id == operation.id; });
    		$scope.blade.origEntity = _.find(customerOrder.inPayments, function (x) { return x.id == operation.id; });
    	}
    	$scope.blade.isLoading = false;
    };

    function isDirty() {
    	var retVal = false;
    	if ($scope.blade.origEntity) {
    		retVal = !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity) || $scope.blade.isNew;
    	}
      	return retVal;
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        order_res_customerOrders.update({}, $scope.blade.customerOrder, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "New document", icon: 'fa fa-plus',
            executeMethod: function () {
               
            	var newBlade = {
            		id: "newOperationWizard",
            		customerOrder: $scope.blade.customerOrder,
            		currentEntity: $scope.blade.currentEntity,
            		title: "New operation",
            		subtitle: 'Select operation type',
            		controller: 'newOperationWizardController',
            		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/wizards/newOperation/newOperation-wizard.tpl.html'
            	};
            	bladeNavigationService.showBlade(newBlade, $scope.blade);

            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
		   {
		   	name: "Delete", icon: 'fa fa-trash-o',
		   	executeMethod: function () {
		   		var dialog = {
		   			id: "confirmDeleteItem",
		   			title: "Delete confirmation",
		   			message: "Are you sure you want to delete current operation?",
		   			callback: function (remove) {
		   				if (remove) {

		   					if ($scope.blade.currentEntity.operationType.toLowerCase() != 'customerorder') {
		   						order_res_customerOrders.deleteOperation({ id: $scope.blade.customerOrder.id, operationId: $scope.blade.currentEntity.id },
								function () {
									$scope.blade.title = $scope.blade.customerOrder.customer + '\'s Customer Order';
									$scope.blade.subtitle = 'Edit order details and related documents';
									$scope.blade.template = 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-detail.tpl.html';
									$scope.blade.currentEntity = $scope.blade.customerOrder;
									$scope.blade.refresh();
								},
								function (error) { notificationService.setError('Error ' + error.status, $scope.blade); });
		   					}
		   					else {
		   						order_res_customerOrders.delete({ id: $scope.blade.customerOrder.id },
								function () {
									bladeNavigationService.closeBlade($scope.blade);
								},
								function (error) { notificationService.setError('Error ' +error.status, $scope.blade);
							});
		   					}

		   					
		   				}
		   			}
		   		};
		   		dialogService.showConfirmationDialog(dialog);
		   	},
		   	canExecuteMethod: function () {
		   		return true;
		   	}
		   }
    ];
  
    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The document has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeChildrenBlades();
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeChildrenBlades();
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    // actions on load
    $scope.toolbarTemplate = 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/operation-detail-toolbar.tpl.html';
    $scope.blade.refresh($scope.blade.isNew);

}]);