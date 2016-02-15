angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.virtualCatalogDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, catalogs, dialogService) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            initializeBlade(blade.currentEntity);
        } else {
            catalogs.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    }

    function initializeBlade(data) {
        if (!blade.isNew) {
            blade.title = data.name;
        }

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
        blade.securityScopes = data.securityScopes;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntity);
        $scope.bladeClose();
    };
    $scope.saveChanges = function () {
        blade.isLoading = true;

        if (blade.isNew) {
            catalogs.save({}, blade.currentEntity, function (data) {
                blade.isNew = undefined;
                blade.currentEntityId = data.id;
                initializeBlade(data);
                initializeToolbar();
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
        else {
            catalogs.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
    };

    blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "catalog.dialogs.virtual-catalog-save.title",
                message: "catalog.dialogs.virtual-catalog-save.message"
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

    function initializeToolbar() {
        if (!blade.isNew) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'catalog:update'
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty,
                    permission: 'catalog:update'
                }
            ];
        }
    }

    initializeToolbar();
    blade.refresh(false);
}]);
