angular.module('platformWebApp')
.controller('newAccountWizardController', ['$scope', 'bladeNavigationService', 'accounts', 'platform_res_roles', '$interval', 'uiGridConstants', function ($scope, bladeNavigationService, accounts, roles, $interval, uiGridConstants) {
    var promise = roles.get({ count: 10000 }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.blade.isLoading = false;

            $scope.gridOptions.data = promiseData.roles;
        });
    };

    $scope.saveChanges = function () {
        if ($scope.blade.currentEntity.password != $scope.blade.currentEntity.newPassword2) {
            $scope.blade.error = 'Error: passwords don\'t match!';
            return;
        }

        $scope.blade.isLoading = true;
        $scope.blade.error = undefined;
        var postData = angular.copy($scope.blade.currentEntity);
        postData.newPassword2 = undefined;

        accounts.save({}, postData, function (data) {
            $scope.blade.parentBlade.refresh();
            $scope.blade.parentBlade.selectNode(data);
        }, function (error) {
            var errText = 'Error ' + error.status;
            if (error.data && error.data.message) {
                errText = errText + ": " + error.data.message;
            }
            bladeNavigationService.setError(errText, $scope.blade);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.bladeHeadIco = 'fa-lock';

    // roles management

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
    initializeBlade($scope.blade.currentEntity);
}]);