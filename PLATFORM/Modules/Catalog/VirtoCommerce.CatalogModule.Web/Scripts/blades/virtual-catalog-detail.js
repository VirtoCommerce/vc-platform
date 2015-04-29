angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.virtualCatalogDetailController', ['$scope', 'bladeNavigationService', 'virtoCommerce.catalogModule.catalogs', 'dialogService', function ($scope, bladeNavigationService, catalogs, dialogService) {

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isNew) {
            initializeBlade($scope.blade.currentEntity);
        } else {
            catalogs.get({ id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        }
    }

    function initializeBlade(data) {
        if (!$scope.blade.isNew) {
            $scope.blade.title = data.name;
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
        $scope.bladeClose();
    };
    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        if ($scope.blade.isNew) {
            catalogs.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.isNew = undefined;
                $scope.blade.currentEntityId = data.id;
                initializeBlade(data);
                initializeToolbar();
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
        else {
            catalogs.update({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The virtual catalog has been modified. Do you want to save changes?"
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

    function initializeToolbar() {
        if (!$scope.blade.isNew) {
            $scope.bladeToolbarCommands = [
                {
                    name: "Save", icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'catalog:virtual_catalogs:manage'
                },
                {
                    name: "Reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'catalog:virtual_catalogs:manage'
                }
            ];
        }
    }

    initializeToolbar();
    $scope.blade.refresh(false);
}]);
