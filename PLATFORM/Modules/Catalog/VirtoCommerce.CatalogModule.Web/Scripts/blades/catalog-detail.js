angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, catalogs, dialogService) {
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
                $scope.gridsterOpts.maxRows = 3; // force re-initializing the widgets
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
        else {
            catalogs.update({}, blade.currentEntity, function () {
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
                title: "Save changes",
                message: "The catalog has been modified. Do you want to save changes?"
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
                    name: "Save", icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'catalog:catalogs:manage'
                },
                {
                    name: "Reset", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'catalog:catalogs:manage'
                },
                {
                    name: "Delete", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        var dialog = {
                            id: "confirmDelete",
                            name: blade.origEntity.name,
                            callback: function (remove) {
                                if (remove) {
                                    blade.isLoading = true;
                                    catalogs.delete({ id: blade.currentEntityId }, function () {
                                        $scope.cancelChanges();
                                        if (blade.deleteFn) {
                                            blade.deleteFn(blade.currentEntityId);
                                        } else {
                                            blade.parentBlade.refresh();
                                        }
                                    }, function (error) {
                                        bladeNavigationService.setError('Error ' + error.status, blade);
                                    });
                                }
                            }
                        };
                        dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Catalog)/Scripts/dialogs/deleteCatalog-dialog.tpl.html', 'platformWebApp.confirmDialogController');
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'catalog:catalogs:manage'
                }
            ];
        }
    }

    $scope.gridsterOpts = { width: 396 };

    initializeToolbar();
    blade.refresh(false);
}]);
