angular.module('platformWebApp')
.controller('roleDetailController', ['$scope', 'bladeNavigationService', 'platform_res_roles', '$interval', 'uiGridConstants', 'dialogService', function ($scope, bladeNavigationService, roles, $interval, uiGridConstants, dialogService) {
    var promise = roles.queryPermissions().$promise;

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isNew) {
            initializeBlade({ permissions: [] });
        } else {
            roles.get({ id: $scope.blade.data.id }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        }
    }

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.blade.currentEntity = angular.copy(data);
            $scope.blade.origEntity = data;
            $scope.blade.isLoading = false;

            $scope.gridOptions.data = promiseData;

            // $interval whilst we wait for the grid to digest the data we just gave it
            $interval(function () {
                _.each(data.permissions, selectRow);
            }, 50, 1);
        });
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        roles.update({}, $scope.blade.currentEntity, function (data) {
            if ($scope.blade.isNew) {
                $scope.blade.isNew = undefined;
                $scope.blade.data = data;
                initializeToolbar();
            }
            $scope.blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Role has been modified. Do you want to save changes?"
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

    $scope.bladeHeadIco = 'fa-lock';

    function initializeToolbar() {
        if (!$scope.blade.isNew) {
            $scope.bladeToolbarCommands = [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    }
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        $scope.gridApi.selection.clearSelectedRows();
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                        _.each($scope.blade.currentEntity.permissions, selectRow);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    }
                }
            ];
        }
    }

    // permissions management

    function selectRow(permission) {
        var row = _.findWhere($scope.gridOptions.data, { id: permission.id });
        if (row) {
            $scope.gridApi.selection.selectRow(row);
        }
    }

    $scope.removeAll = function () {
        $scope.blade.currentEntity.permissions.length = 0;
        $scope.gridApi.selection.clearSelectedRows();
    }

    $scope.remove = function (permission) {
        var row = _.findWhere($scope.gridOptions.data, { id: permission.id });
        if (row) {
            $scope.gridApi.selection.unSelectRow(row);
        } else {
            assignPermission(permission, false);
        }
    }

    // ui-grid
    $scope.gridOptions = {
        enableRowSelection: true,
        enableSelectAll: true,
        multiSelect: true,
        selectionRowHeaderWidth: 35,
        rowHeight: 35,
        showGridFooter: true
    };

    $scope.gridOptions.columnDefs = [
        {
            name: 'moduleId',
            sort: {
                direction: uiGridConstants.ASC,
                priority: 1
            }
        },
        {
            name: 'name',
            sort: {
                direction: uiGridConstants.ASC,
                priority: 2
            }
        }
    ];

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        //set gridApi on scope
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            assignPermission(row.entity, row.isSelected);
        });

        gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            _.each(rows, function (row) {
                assignPermission(row.entity, row.isSelected);
            });
        });
    };

    function assignPermission(permission, isAdd) {
        if (isAdd) {
            if (!_.findWhere($scope.blade.currentEntity.permissions, { id: permission.id })) {
                $scope.blade.currentEntity.permissions.push(permission);
            }
        } else {
            var idx = _.findIndex($scope.blade.currentEntity.permissions, function (x) { return x.id === permission.id; });
            if (idx >= 0) {
                $scope.blade.currentEntity.permissions.splice(idx, 1);
            }
        }
    };

    // actions on load
    initializeToolbar();
    $scope.blade.refresh(false);
}]);