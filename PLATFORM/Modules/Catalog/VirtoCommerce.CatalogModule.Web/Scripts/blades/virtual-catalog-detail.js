angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.virtualCatalogDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogs', function ($scope, bladeNavigationService, catalogs) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';

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
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

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
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "catalog.dialogs.virtual-catalog-save.title", "catalog.dialogs.virtual-catalog-save.message");
    };
    
    function initializeToolbar() {
        if (!blade.isNew) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: canSave,
                    permission: blade.updatePermission
                },
                {
                    name: "platform.commands.reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: isDirty,
                    permission: blade.updatePermission
                }
            ];
        }
    }

    initializeToolbar();
    blade.refresh(false);
}]);
