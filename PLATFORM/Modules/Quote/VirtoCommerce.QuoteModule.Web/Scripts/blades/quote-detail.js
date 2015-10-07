angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteDetailController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.storeModule.stores', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.accounts',
    function ($scope, $timeout, bladeNavigationService, quotes, stores, settings, dialogService, accounts) {
        var blade = $scope.blade;

        var openItemsListOnce = _.once(function () {
            $timeout(function () {
                blade.openItemsBlade();
            }, 0, false);
        });

        blade.refresh = function (parentRefresh) {
            quotes.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                openItemsListOnce();
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }

        function initializeBlade(data) {
            initializeToolbar(data);
            blade.title = data.number;

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            onHoldCommand.updateName();
        }

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
            bladeNavigationService.closeChildrenBlades(blade, function () {
                if (isDirty() && !isQuoteSubmitted(blade.origEntity)) {
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
            });
        };

        function isQuoteSubmitted(currentEntity) {
            return currentEntity.status === 'Proposal sent';
        }

        blade.headIcon = 'fa-file-text-o';

        var onHoldCommand = {
            updateName: function () {
                return this.name = (blade.currentEntity && blade.currentEntity.isLocked) ? 'Place On Hold' : 'Release Hold';
            },
            // name: this.updateName(),
            icon: 'fa fa-lock', // icon: 'fa fa-hand-paper-o',
            executeMethod: function () {
                blade.currentEntity.isLocked = !blade.currentEntity.isLocked;
                this.updateName();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'quote:update'
        };

        function initializeToolbar(currentEntity) {
            var optionalCommands = isQuoteSubmitted(currentEntity) ? [] : [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    },
                    permission: 'quote:update'
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                        onHoldCommand.updateName();
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'quote:update'
                }
            ];

            blade.toolbarCommands = optionalCommands.concat([
                {
                    name: "Submit proposal", icon: 'fa fa-check-square-o',
                    executeMethod: function () {
                        var dialog = {
                            id: "confirmDelete",
                            title: "Proposal confirmation",
                            message: "You can't modify Quote Request after proposal is sent. Are you sure you want to send this proposal to customer? ",
                            callback: function (ok) {
                                if (ok) {
                                    blade.currentEntity.status = 'Proposal sent';
                                    saveChanges();
                                }
                            }
                        };
                        dialogService.showConfirmationDialog(dialog);
                    },
                    canExecuteMethod: function () {
                        return blade.currentEntity && !isQuoteSubmitted(blade.origEntity);
                    },
                    permission: 'quote:update'
                },
                onHoldCommand,
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
                    permission: 'quote:update'
                },
                {
                    name: "Delete", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        deleteEntry();
                    },
                    canExecuteMethod: function () {
                        return !isDirty();
                    },
                    permission: 'quote:delete'
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
        $scope.today = new Date();
        $scope.open = function ($event, which) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datepickers[which] = true;
        };

        blade.refresh(false);

        $scope.quoteStatuses = settings.getValues({ id: 'Quotes.Status' });
        $scope.stores = stores.query();
        blade.shippingMethods = quotes.getShippingMethods({
            id: blade.currentEntityId
        });
        accounts.search({
        	takeCount: 100,
			accountTypes: [ 'Manager', 'Administrator' ]
        }, function (data) {
            $scope.employees = data.users; 
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