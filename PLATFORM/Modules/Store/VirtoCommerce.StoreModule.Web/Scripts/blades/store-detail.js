angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, stores, catalogs, settings, dialogService) {
    $scope.blade.refresh = function (parentRefresh) {
        stores.get({ id: $scope.blade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;
        $scope.blade.title = data.name;

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        stores.update({}, $scope.blade.currentEntity, function (data) {
            $scope.blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this Store?",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    stores.remove({ ids: $scope.blade.currentEntityId }, function () {
                        $scope.bladeClose();
                        $scope.blade.parentBlade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Store has been modified. Do you want to save changes?"
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
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa fa-archive';

    $scope.blade.toolbarCommands = [
        {
            name: "Save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            },
            permission: 'store:manage'
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'store:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !isDirty();
            },
            permission: 'store:manage'
        }
    ];


    $scope.blade.refresh(false);
    $scope.catalogs = catalogs.getCatalogs();
    $scope.storeStates = settings.getValues({ id: 'Stores.States' });
}]);