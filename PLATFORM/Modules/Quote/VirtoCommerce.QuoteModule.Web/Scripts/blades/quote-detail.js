angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.storeModule.stores', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.accounts',
    function ($scope, bladeNavigationService, quotes, stores, settings, dialogService, accounts) {
        var blade = $scope.blade;

        blade.refresh = function (parentRefresh) {
            quotes.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }

        function initializeBlade(data) {
            blade.currentEntityId = data.id;
            blade.title = data.name;

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;
        };

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity);
        };

        $scope.saveChanges = function () {
            blade.isLoading = true;
            
            quotes.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        function deleteEntry() {
            var dialog = {
                id: "confirmDelete",
                title: "Delete confirmation",
                message: "Are you sure you want to delete this Quote?",
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        quotes.remove({ ids: blade.currentEntityId }, function () {
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
            if (isDirty()) {
                var dialog = {
                    id: "confirmCurrentBladeClose",
                    title: "Save changes",
                    message: "The Quote has been modified. Do you want to save changes?"
                };
                dialog.callback = function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
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

        blade.toolbarCommands = [
            {
                name: "Save",
                icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
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
                                $scope.blade.currentEntity.status = 'Canceled';
                                saveChanges();
                            }
                        }
                    };
                    dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Quote)/Scripts/dialogs/cancelQuote-dialog.tpl.html', 'virtoCommerce.quoteModule.confirmCancelDialogController');
                },
                canExecuteMethod: function () {
                    return $scope.blade.currentEntity && !$scope.blade.currentEntity.isCancelled;
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
        ];

        // datepicker
        $scope.datepickers = {}
        $scope.open = function ($event, which) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datepickers[which] = true;
        };

        blade.refresh(false);

        $scope.quoteStatuses = settings.getValues({ id: 'Quotes.Status' });
        //$scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
        //$scope.currencies = settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' });
        $scope.stores = stores.query();
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