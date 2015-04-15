angular.module('platformWebApp')
.controller('accountDetailController', ['$scope', 'bladeNavigationService', 'accounts', 'platform_res_roles', '$interval', 'uiGridConstants', 'dialogService', function ($scope, bladeNavigationService, accounts, roles, $interval, uiGridConstants, dialogService) {
    var promise = roles.get({ count: 10000 }).$promise;

    $scope.blade.refresh = function (parentRefresh) {
        accounts.get({ id: $scope.blade.data.userName }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        });
    }

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.blade.currentEntity = angular.copy(data);
            $scope.blade.origEntity = data;
            $scope.blade.isLoading = false;

            $scope.gridOptions.data = promiseData.roles;

            // $interval whilst we wait for the grid to digest the data we just gave it
            $interval(function () {
                _.each(data.roles, selectRow);
            }, 30, 1);
        });
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        accounts.update({}, $scope.blade.currentEntity, function (data) {
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
                message: "The Account has been modified. Do you want to save changes?"
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
                _.each($scope.blade.currentEntity.roles, selectRow);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Change password",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                var newBlade = {
                    id: 'accountDetailChild',
                    currentEntityId: $scope.blade.currentEntity.userName,
                    title: $scope.blade.title,
                    subtitle: "Change your password",
                    controller: 'accountChangePasswordController',
                    template: 'Scripts/common/security/blades/account-changePassword.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];


    $scope.generateNewApiAccount = function () {
        $scope.blade.isLoading = true;

        accounts.generateNewApiAccount({}, function (apiAccount) {
            $scope.blade.isLoading = false;
            if ($scope.blade.currentEntity.apiAcounts && $scope.blade.currentEntity.apiAcounts.length > 0) {
                $scope.blade.currentEntity.apiAcounts[0] = apiAccount;
            }
            else {
                $scope.blade.currentEntity.apiAcounts = [apiAccount];
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.copyCode = function () {
        var secretKey = document.getElementById('secretKey');
        secretKey.focus();
        secretKey.select();
    };

    // roles management

    function selectRow(role) {
        var row = _.findWhere($scope.gridOptions.data, { id: role.id });
        if (row) {
            $scope.gridApi.selection.selectRow(row);
        }
    }

    $scope.removeAll = function () {
        $scope.blade.currentEntity.roles.length = 0;
        $scope.gridApi.selection.clearSelectedRows();
    }

    $scope.remove = function (role) {
        var row = _.findWhere($scope.gridOptions.data, { id: role.id });
        if (row) {
            $scope.gridApi.selection.unSelectRow(row);
        } else {
            assignRole(role, false);
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
            name: 'name',
            displayName: 'Role name',
            sort: {
                direction: uiGridConstants.ASC
            }
        }
    ];

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        //set gridApi on scope
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            assignRole(row.entity, row.isSelected);
        });

        gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            _.each(rows, function (row) {
                assignRole(row.entity, row.isSelected);
            });
        });
    };

    function assignRole(role, isAdd) {
        if (isAdd) {
            if (!_.findWhere($scope.blade.currentEntity.roles, { id: role.id })) {
                $scope.blade.currentEntity.roles.push(role);
            }
        } else {
            var idx = _.findIndex($scope.blade.currentEntity.roles, function (x) { return x.id === role.id; });
            if (idx >= 0) {
                $scope.blade.currentEntity.roles.splice(idx, 1);
            }
        }
    };

    // actions on load
    $scope.blade.refresh(false);
}]);