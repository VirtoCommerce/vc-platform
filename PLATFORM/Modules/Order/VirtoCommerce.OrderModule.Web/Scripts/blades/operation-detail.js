angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.order_res_customerOrders', 'virtoCommerce.orderModule.order_res_fulfilmentCenters', 'virtoCommerce.orderModule.order_res_stores', 'platformWebApp.objCompareService', 'platformWebApp.settings', 'platformWebApp.authService',
			function ($scope, dialogService, bladeNavigationService, order_res_customerOrders, order_res_fulfilmentCenters, order_res_stores, objCompareService, settings, authService) {
			    var blade = $scope.blade;

			    blade.refresh = function (noRefresh) {
			        blade.isLoading = true;
			        $scope.fulfillmentCenters = [];
			        $scope.statuses = [];

			        if (!noRefresh) {
			            order_res_customerOrders.get({ id: blade.customerOrder.id }, function (result) {
			                initialize(result);
			                //necessary for scope bounded ACL checks 
			                blade.securityScopes = result.scopes;
			            },
						function (error) {
						    bladeNavigationService.setError('Error ' + error.status, blade);
						});
			        }
			        else {
			            initialize(blade.customerOrder);
			        }
			    }

			    function initialize(customerOrder) {

			        var operation = angular.isDefined(blade.currentEntity) ? blade.currentEntity : customerOrder;
			        var copy = angular.copy(customerOrder);

			        blade.customerOrder = copy;

			        if (operation.operationType.toLowerCase() == 'customerorder') {
			            blade.currentEntity = copy;
			            blade.origEntity = customerOrder;
			            blade.stores = order_res_stores.query();
			            $scope.statuses = settings.getValues({ id: 'Order.Status' });

			        }
			        else if (operation.operationType.toLowerCase() == 'shipment') {
			            blade.currentEntity = _.find(copy.shipments, function (x) { return x.id == operation.id; });
			            blade.origEntity = _.find(customerOrder.shipments, function (x) { return x.id == operation.id; });
			            $scope.fulfillmentCenters = order_res_fulfilmentCenters.query();
			            $scope.statuses = settings.getValues({ id: 'Shipment.Status' });
			        }
			        else if (operation.operationType.toLowerCase() == 'paymentin') {
			            $scope.currentStore = _.findWhere(blade.stores, { id: customerOrder.storeId });
			            blade.currentEntity = _.find(copy.inPayments, function (x) { return x.id == operation.id; });
			            blade.origEntity = _.find(customerOrder.inPayments, function (x) { return x.id == operation.id; });
			            $scope.statuses = settings.getValues({ id: 'PaymentIn.Status' });
			        }
			        blade.isLoading = false;
			    };

			    function isDirty() {
			        var retVal = false;
			        if (blade.origEntity) {
			            retVal = !objCompareService.equal(blade.origEntity, blade.currentEntity) || blade.isNew;
			        }
			        if (retVal) {
			            retVal = authService.checkPermission('order:update', blade.securityScopes);
			        }
			        return retVal;
			    };

			    function saveChanges() {
			        blade.isLoading = true;
			        order_res_customerOrders.update({}, blade.customerOrder, function (data, headers) {
			            blade.refresh();
			            blade.parentBlade.refresh();
			        },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
			    };

			    $scope.openFulfillmentCentersList = function () {
			        var newBlade = {
			            id: 'fulfillmentCenterList',
			            controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentListController',
			            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-list.tpl.html'
			        };
			        bladeNavigationService.showBlade(newBlade, blade);
			    }

			    $scope.openStatusSettingManagement = function () {
			        var newBlade = {
			            id: 'moduleSettingsSection',
			            moduleId: 'VirtoCommerce.Orders',
			            // parentWidget: $scope.widget,
			            title: 'Order settings',
			            //subtitle: '',
			            controller: 'platformWebApp.settingsDetailController',
			            template: '$(Platform)/Scripts/app/settings/blades/settings-detail.tpl.html'
			        };
			        bladeNavigationService.showBlade(newBlade, blade);
			    };

			    blade.headIcon = 'fa-file-text';

			    blade.toolbarCommands = [
					{
					    name: "orders.commands.new-document", icon: 'fa fa-plus',
					    executeMethod: function () {

					        var newBlade = {
					            id: "newOperationWizard",
					            customerOrder: blade.customerOrder,
					            currentEntity: blade.currentEntity,
					            stores: blade.stores,
					            title: "orders.blades.newOperation-wizard.title",
					            subtitle: 'orders.blades.newOperation-wizard.subtitle',
					            controller: 'virtoCommerce.orderModule.newOperationWizardController',
					            template: 'Modules/$(VirtoCommerce.Orders)/Scripts/wizards/newOperation/newOperation-wizard.tpl.html'
					        };
					        bladeNavigationService.showBlade(newBlade, blade);

					    },
					    canExecuteMethod: function () {
					        return blade.currentEntity && blade.currentEntity.operationType.toLowerCase() === 'customerorder';
					    },
					    permission: 'order:update'
					},
					{
					    name: "platform.commands.save", icon: 'fa fa-save',
					    executeMethod: function () {
					        saveChanges();
					    },
					    canExecuteMethod: function () {
					        return isDirty();
					    },
					    permission: 'order:update'
					},
					{
					    name: "platform.commands.reset", icon: 'fa fa-undo',
					    executeMethod: function () {
					        angular.copy(blade.origEntity, blade.currentEntity);
					    },
					    canExecuteMethod: function () {
					        return isDirty();
					    },
					    permission: 'order:update'
					},
					{
					    name: "platform.commands.delete", icon: 'fa fa-trash-o',
					    executeMethod: function () {
					        var dialog = {
					            id: "confirmDeleteItem",
					            title: "orders.dialogs.operation-delete.title",
					            message: "orders.dialogs.operation-delete.message",
					            callback: function (remove) {
					                if (remove) {

					                    if (blade.currentEntity.operationType.toLowerCase() != 'customerorder') {
					                        order_res_customerOrders.deleteOperation({ id: blade.customerOrder.id, operationId: blade.currentEntity.id },
                                            function () {
                                                blade.parentBlade.refresh();
                                                bladeNavigationService.closeBlade(blade);
                                            },
                                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
					                    }
					                    else {
					                        order_res_customerOrders.delete({ ids: blade.customerOrder.id },
                                            function () {
                                                blade.parentBlade.refresh();
                                                bladeNavigationService.closeBlade(blade);
                                            },
                                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
					                    }
					                }
					            }
					        };
					        dialogService.showConfirmationDialog(dialog);
					    },
					    canExecuteMethod: function () {
					        return true;
					    },
					    permission: 'order:delete'
					},
					{
					    name: "orders.commands.cancel-document", icon: 'fa fa-remove',
					    executeMethod: function () {
					        var dialog = {
					            id: "confirmCancelOperation",
					            callback: function (reason) {
					                if (reason) {
					                    blade.currentEntity.cancelReason = reason;
					                    blade.currentEntity.isCancelled = true;
					                    blade.currentEntity.status = 'Cancelled';
					                    saveChanges();
					                }
					            }
					        };
					        dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Orders)/Scripts/dialogs/cancelOperation-dialog.tpl.html', 'virtoCommerce.orderModule.confirmCancelDialogController');
					    },
					    canExecuteMethod: function () {
					        return blade.currentEntity && !blade.currentEntity.isCancelled;
					    },
					    permission: 'order:update'
					}
			    ];

			    blade.onClose = function (closeCallback) {
			        if (isDirty()) {
			            var dialog = {
			                id: "confirmItemChange",
			                title: "orders.dialogs.operation-save.title",
			                message: "orders.dialogs.operation-save.message",
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
			        angular.forEach(blade.childrenBlades.slice(), function (child) {
			            bladeNavigationService.closeBlade(child);
			        });
			    }

			    $scope.cancelOperationResolution = function (resolution) {
			        $modalInstance.close(resolution);
			    };



			    // actions on load
			    blade.refresh(blade.isNew);

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