angular.module('virtoCommerce.orderModule.blades')
.controller('operationDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'order_res_customerOrders', 'notificationService', 'order_res_fulfilmentCenters', 'order_res_stores', 'order_res_paymentGateways',
			function ($scope, dialogService, bladeNavigationService, order_res_customerOrders, notificationService, order_res_fulfilmentCenters, order_res_stores, order_res_paymentGateways) {

    $scope.blade.refresh = function () {
    	$scope.blade.isLoading = true;
    	$scope.fulfillmentCenters = [];
    	$scope.stores = [];
    	$scope.paymentGateways = [];
    	order_res_customerOrders.get({ id: $scope.blade.customerOrder.id }, function (results) {
        	var operation = angular.isDefined($scope.blade.currentEntity) ? $scope.blade.currentEntity : results;
        	var copy = angular.copy(results);

        	$scope.blade.customerOrder = copy;

        	if (operation.operationType.toLowerCase() == 'customerorder')
        	{
        		$scope.blade.currentEntity = copy;
        		$scope.blade.origEntity = results;
        		$scope.stores = order_res_stores.query();
        	}
        	else if (operation.operationType.toLowerCase() == 'shipment')
        	{
        		$scope.blade.currentEntity = _.find(copy.shipments, function (x) { return x.id == operation.id; });
        		$scope.blade.origEntity = _.find(results.shipments, function (x) { return x.id == operation.id; });
        		$scope.fulfillmentCenters = order_res_fulfilmentCenters.query();
			}	
        	else if (operation.operationType.toLowerCase() == 'paymentin')
        	{
        		$scope.paymentGateways = order_res_paymentGateways.query();
        		$scope.blade.currentEntity = _.find(copy.inPayments, function (x) { return x.id == operation.id; });
        		$scope.blade.origEntity = _.find(results.inPayments, function (x) { return x.id == operation.id; });
		    }
        	$scope.blade.isLoading = false;

        },
        function (error) {
            $scope.blade.isLoading = false;
            notificationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {
    	var retVal = false;
    	if ($scope.blade.origEntity) {
    		retVal = !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
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
                openAddEntityWizard();
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
    $scope.blade.refresh();

}]);