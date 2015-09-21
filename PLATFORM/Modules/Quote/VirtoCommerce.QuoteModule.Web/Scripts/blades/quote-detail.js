angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteDetailController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.storeModule.stores', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.accounts',
    function ($scope, $timeout, bladeNavigationService, quotes, stores, settings, dialogService, accounts) {
        var blade = $scope.blade;

        blade.refresh = function (parentRefresh) {
            quotes.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                _.once(function () {
                    $timeout(function () {
                        blade.openItemsBlade();
                    }, 0, false);
                })();
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }

        function initializeBlade(data) {
            initializeToolbar(data);
            blade.title = data.number;
            if (data.status) {
                blade.title += " - " + data.status;
                if (data.isCancelled) {
                    blade.title += ", " + data.cancelReason;
                }
            }

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;
        };

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity);
        }

        function saveChanges() {
            blade.isLoading = true;

            quotes.update({}, blade.currentEntity, function () {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }

        blade.openItemsBlade = function () {
            var newBlade = {
                id: 'quoteItems',
                title: blade.title + ' line items',
                subtitle: 'Edit line items',
                recalculateFn: blade.recalculate,
                shippingMethods: blade.shippingMethods,
                currentEntity: blade.currentEntity,
                controller: 'virtoCommerce.quoteModule.quoteItemsController',
                template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-items.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        blade.recalculate = function () {
            quotes.recalculate({}, blade.currentEntity, function (data) {
                blade.currentEntity.totals = data.totals;
                bladeNavigationService.setError(null, blade);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }

        function deleteEntry() {
            var dialog = {
                id: "confirmDelete",
                title: "Delete confirmation",
                message: "Are you sure you want to delete this Quote?",
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        quotes.remove({
                            ids: blade.currentEntityId
                        }, function () {
                            $scope.bladeClose();
                            blade.parentBlade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                        });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        $scope.setForm = function (form) {
            $scope.formScope = form;
        }

        blade.onClose = function (closeCallback) {
            closeChildrenBlades();
            if (isDirty() && !blade.origEntity.isSubmitted) {
                var dialog = {
                    id: "confirmCurrentBladeClose",
                    title: "Save changes",
                    message: "The Quote has been modified. Do you want to save changes?"
                };
                dialog.callback = function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                };
                dialogService.showConfirmationDialog(dialog);
            }
            else {
                closeCallback();
            }
        };

        function closeChildrenBlades() {
            angular.forEach(blade.childrenBlades.slice(), function (child) {
                bladeNavigationService.closeBlade(child);
            });
        }

        blade.headIcon = 'fa-file-text-o';

        function initializeToolbar(currentEntity) {
            var optionalCommands = currentEntity.isSubmitted ? [] : [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    },
                    permission: 'quote:manage'
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'quote:manage'
                }
            ];

            blade.toolbarCommands = optionalCommands.concat([
                {
                    name: "Submit proposal", icon: 'fa fa-check-square-o',
                    executeMethod: function () {
                        blade.currentEntity.isSubmitted = true;
                        // saveChanges();
                    },
                    canExecuteMethod: function () {
                        return blade.currentEntity && !blade.currentEntity.isSubmitted;
                    },
                    permission: 'quote:manage'
                },
                {
                    name: "Cancel document", icon: 'fa fa-remove',
                    executeMethod: function () {
                        var dialog = {
                            id: "confirmCancelOperation",
                            callback: function (reason) {
                                if (reason) {
                                    blade.currentEntity.cancelReason = reason;
                                    blade.currentEntity.isCancelled = true;
                                    blade.currentEntity.status = 'Canceled';
                                    saveChanges();
                                }
                            }
                        };
                        dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Quote)/Scripts/dialogs/cancelQuote-dialog.tpl.html', 'virtoCommerce.quoteModule.confirmCancelDialogController');
                    },
                    canExecuteMethod: function () {
                        return blade.currentEntity && !blade.currentEntity.isCancelled;
                    },
                    permission: 'quote:manage'
                },
                {
                    name: "Delete", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        deleteEntry();
                    },
                    canExecuteMethod: function () {
                        return !isDirty();
                    },
                    permission: 'quote:manage'
                }
            ]);
        }

        $scope.openDictionarySettingManagement = function () {
            var newBlade = {
                id: 'settingDetailChild',
                isApiSave: true,
                currentEntityId: 'Quotes.Status',
                title: 'Quote statuses',
                parentRefresh: function (data) {
                    $scope.quoteStatuses = data;
                },
                controller: 'platformWebApp.settingDictionaryController',
                template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        // datepicker
        $scope.datepickers = {}
        $scope.open = function ($event, which) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datepickers[which] = true;
        };

        blade.refresh(false);

        $scope.quoteStatuses = settings.getValues({
            id: 'Quotes.Status'
        });
        $scope.stores = stores.query();
        blade.shippingMethods = quotes.getShippingMethods({
            id: blade.currentEntityId
        });
        accounts.search({
            takeCount: 1000
        }, function (data) {
            $scope.employees = data.users; // filter??
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    }])

.controller('virtoCommerce.quoteModule.confirmCancelDialogController', ['$scope', '$modalInstance', function ($scope, $modalInstance) {

    $scope.cancelReason = undefined;
    $scope.yes = function () {
        $modalInstance.close($scope.cancelReason);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
;