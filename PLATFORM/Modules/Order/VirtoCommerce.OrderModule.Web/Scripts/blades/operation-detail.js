angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.order_res_customerOrders', 'virtoCommerce.orderModule.order_res_fulfilmentCenters', 'virtoCommerce.orderModule.order_res_stores', 'virtoCommerce.orderModule.order_res_paymentGateways', 'platformWebApp.objCompareService', 'platformWebApp.settings',
			function ($scope, dialogService, bladeNavigationService, order_res_customerOrders, order_res_fulfilmentCenters, order_res_stores, order_res_paymentGateways, objCompareService, settings) {

			    $scope.blade.refresh = function (noRefresh) {
			        $scope.blade.isLoading = true;
			        $scope.fulfillmentCenters = [];
			        $scope.stores = [];
			        $scope.paymentGateways = [];
			        $scope.statuses = [];

			        if (!noRefresh) {
			            order_res_customerOrders.get({ id: $scope.blade.customerOrder.id }, function (result) {
			                initialize(result);
			            },
						function (error) {
						    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
						});
			        }
			        else {
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
			            $scope.statuses = settings.getValues({ id: 'Order.Status' });
			        }
			        else if (operation.operationType.toLowerCase() == 'shipment') {
			            $scope.blade.currentEntity = _.find(copy.shipments, function (x) { return x.id == operation.id; });
			            $scope.blade.origEntity = _.find(customerOrder.shipments, function (x) { return x.id == operation.id; });
			            $scope.fulfillmentCenters = order_res_fulfilmentCenters.query();
			            $scope.statuses = settings.getValues({ id: 'Shipment.Status' });
			        }
			        else if (operation.operationType.toLowerCase() == 'paymentin') {
			            $scope.paymentGateways = order_res_paymentGateways.query();
			            $scope.blade.currentEntity = _.find(copy.inPayments, function (x) { return x.id == operation.id; });
			            $scope.blade.origEntity = _.find(customerOrder.inPayments, function (x) { return x.id == operation.id; });
			            $scope.statuses = settings.getValues({ id: 'PaymentIn.Status' });
			        }
			        $scope.blade.isLoading = false;
			    };

			    function isDirty() {
			        var retVal = false;
			        if ($scope.blade.origEntity) {
			            retVal = !objCompareService.equal($scope.blade.origEntity, $scope.blade.currentEntity) || $scope.blade.isNew;
			        }
			        return retVal;
			    };

			    function saveChanges() {
			        $scope.blade.isLoading = true;
			        order_res_customerOrders.update({}, $scope.blade.customerOrder, function (data, headers) {
			            $scope.blade.refresh();
			        });
			    };

			    $scope.openFulfillmentCentersList = function () {
			        var newBlade = {
			            id: 'fulfillmentCenterList',
			            parentWidget: $scope.blade,
			            controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentListController',
			            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-list.tpl.html'
			        };
			        bladeNavigationService.showBlade(newBlade, $scope.blade);
			    }

			    $scope.openStatusSettingManagment = function () {
			        var newBlade = {
			            id: 'moduleSettingsSection',
			            moduleId: 'VirtoCommerce.Orders',
			            // parentWidget: $scope.widget,
			            title: 'Order settings',
			            //subtitle: '',
			            controller: 'platformWebApp.settingsDetailController',
			            template: 'Modules/$(VirtoCommerce.Core)/Scripts/Settings/blades/settings-detail.tpl.html'
			        };
			        bladeNavigationService.showBlade(newBlade, $scope.blade);
			    };

			    $scope.blade.headIcon = 'fa-file-text';

			    $scope.blade.toolbarCommands = [
					{
					    name: "New document", icon: 'fa fa-plus',
					    executeMethod: function () {

					        var newBlade = {
					            id: "newOperationWizard",
					            customerOrder: $scope.blade.customerOrder,
					            currentEntity: $scope.blade.currentEntity,
					            title: "New operation",
					            subtitle: 'Select operation type',
					            controller: 'virtoCommerce.orderModule.newOperationWizardController',
					            template: 'Modules/$(VirtoCommerce.Orders)/Scripts/wizards/newOperation/newOperation-wizard.tpl.html'
					        };
					        bladeNavigationService.showBlade(newBlade, $scope.blade);

					    },
					    canExecuteMethod: function () {
					        return true;
					    },
					    permission: 'order:manage'
					},
					{
					    name: "Save", icon: 'fa fa-save',
					    executeMethod: function () {
					        saveChanges();
					    },
					    canExecuteMethod: function () {
					        return isDirty();
					    },
					    permission: 'order:manage'
					},
					{
					    name: "Reset", icon: 'fa fa-undo',
					    executeMethod: function () {
					        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
					    },
					    canExecuteMethod: function () {
					        return isDirty();
					    },
					    permission: 'order:manage'
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
                                                $scope.blade.template = 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-detail.tpl.html';
                                                $scope.blade.currentEntity = $scope.blade.customerOrder;
                                                $scope.blade.refresh();
                                            },
                                            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
					                    }
					                    else {
					                        order_res_customerOrders.delete({ id: $scope.blade.customerOrder.id },
                                            function () {
                                                bladeNavigationService.closeBlade($scope.blade);
                                            },
                                            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
					                    }


					                }
					            }
					        };
					        dialogService.showConfirmationDialog(dialog);
					    },
					    canExecuteMethod: function () {
					        return true;
					    },
					    permission: 'order:manage'
					},
					{
					    name: "Cancel document", icon: 'fa fa-remove',
					    executeMethod: function () {
					        var dialog = {
					            id: "confirmCancelOperation",
					            callback: function (reason) {
					                if (reason) {
					                    $scope.blade.currentEntity.cancelReason = reason;
					                    $scope.blade.currentEntity.isCancelled = true;
					                    $scope.blade.currentEntity.status = 'cancelled';
					                    saveChanges();
					                }
					            }
					        };
					        dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Orders)/Scripts/dialogs/cancelOperation-dialog.tpl.html', 'virtoCommerce.orderModule.confirmCancelDialogController');
					    },
					    canExecuteMethod: function () {
					        return $scope.blade.currentEntity && !$scope.blade.currentEntity.isCancelled;
					    },
					    permission: 'order:manage'
					}
			    ];

			    $scope.blade.onClose = function (closeCallback) {
			        if (isDirty()) {
			            var dialog = {
			                id: "confirmItemChange",
			                title: "Save changes",
			                message: "The operation has been modified. Do you want to save changes?",
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

			    $scope.cancelOperationResolution = function (resolution) {
			        $modalInstance.close(resolution);
			    };



			    // actions on load
			    $scope.blade.toolbarCustomTemplates = ['Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-detail-toolbar.tpl.html'];
			    $scope.blade.refresh($scope.blade.isNew);

			}])
.controller('virtoCommerce.orderModule.confirmCancelDialogController', ['$scope', '$modalInstance', function ($scope, $modalInstance, dialog) {

    $scope.cancelReason = undefined;
    $scope.yes = function () {
        $modalInstance.close($scope.cancelReason);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}]);