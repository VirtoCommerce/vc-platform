angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteDetailController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.storeModule.stores', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.accounts',
    function ($scope, $timeout, bladeNavigationService, quotes, stores, settings, dialogService, accounts) {
        var blade = $scope.blade;
        blade.updatePermission = 'quote:update';

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
            blade.title = data.number;

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            onHoldCommand.updateName();
        }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        }

        function canSave() {
            return isDirty() && $scope.formScope && $scope.formScope.$valid && !blade.isLocked();
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
                title: 'quotes.blades.quote-items.title',
                titleValues: { title: blade.title },
                subtitle: 'quotes.blades.quote-items.subtitle',
                recalculateFn: blade.recalculate,
                shippingMethods: blade.shippingMethods,
                currentEntity: blade.currentEntity,
                isLocked: blade.isLocked,
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
                title: "quotes.dialogs.quote-delete.title",
                message: "quotes.dialogs.quote-delete.message",
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
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "quotes.dialogs.quote-save.title", "quotes.dialogs.quote-save.message");
        };

        blade.isLocked = function () {
            return blade.currentEntity && blade.currentEntity.isLocked;
        };

        blade.headIcon = 'fa-file-text-o';

        var onHoldCommand = {
            updateName: function () {
                return this.name = (blade.currentEntity && blade.currentEntity.isLocked) ? 'quotes.commands.release-hold' : 'quotes.commands.place-on-hold';
            },
            // name: this.updateName(),
            icon: 'fa fa-lock', // icon: 'fa fa-hand-paper-o',
            executeMethod: function () {
                var dialog = {
                    id: "confirmDialog",
                    title: "quotes.dialogs.hold-confirmation.title",
                    message: (blade.currentEntity.isLocked ? 'quotes.dialogs.hold-confirmation.message-release' : 'quotes.dialogs.hold-confirmation.message-place'),
                    callback: function (ok) {
                        if (ok) {
                            blade.currentEntity.isLocked = !blade.currentEntity.isLocked;
                            saveChanges();
                        }
                    }
                };
                dialogService.showConfirmationDialog(dialog);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.save",
                icon: 'fa fa-save',
                executeMethod: saveChanges,
                canExecuteMethod: canSave,
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.reset",
                icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                    onHoldCommand.updateName();
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission
            },
            {
                name: "quotes.commands.submit-proposal", icon: 'fa fa-check-square-o',
                executeMethod: function () {
                    var dialog = {
                        id: "confirmDelete",
                        title: "quotes.dialogs.proposal-delete.title",
                        message: "quotes.dialogs.proposal-delete.message",
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
                    return blade.origEntity && blade.origEntity.status !== 'Proposal sent';
                },
                permission: blade.updatePermission
            },
            onHoldCommand,
            {
                name: "quotes.commands.cancel-document", icon: 'fa fa-remove',
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
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: deleteEntry,
                canExecuteMethod: function () { return true; },
                permission: 'quote:delete'
            }
        ];

        $scope.openDictionarySettingManagement = function () {
            var newBlade = {
                id: 'settingDetailChild',
                isApiSave: true,
                currentEntityId: 'Quotes.Status',
                parentRefresh: function (data) { $scope.quoteStatuses = data; },
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
            accountTypes: ['Manager', 'Administrator']
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