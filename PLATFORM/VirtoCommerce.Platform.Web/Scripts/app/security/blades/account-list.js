angular.module('platformWebApp')
.controller('platformWebApp.accountListController', ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants',
function ($scope, accounts, bladeNavigationService, dialogService, uiGridConstants) {
    var blade = $scope.blade;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };
    var selectedNode = null;

    blade.refresh = function () {
        blade.isLoading = true;
        blade.selectedAll = false;

        accounts.search({
            keyword: $scope.filter.searchKeyword,
            skipCount: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            takeCount: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            blade.currentEntities = data.users;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(blade.currentEntities, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.userName;

        var newBlade = {
            id: 'listItemChild',
            data: selectedNode,
            title: selectedNode.userName,
            subtitle: blade.subtitle,
            controller: 'platformWebApp.accountDetailController',
            template: '$(Platform)/Scripts/app/security/blades/account-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.toggleAll = function () {
        angular.forEach(blade.currentEntities, function (item) {
            item.selected = blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return _.any(blade.currentEntities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Accounts?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = $scope.gridApi.selection.getSelectedRows();
                    var itemIds = _.pluck(selection, 'userName');
                    accounts.remove({ names: itemIds }, function (data, headers) {
                        blade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                closeChildrenBlades();

                var newBlade = {
                    id: 'listItemChild',
                    currentEntity: { roles: [], userType: 'SiteAdministrator' },
                    title: 'New Account',
                    subtitle: blade.subtitle,
                    controller: 'platformWebApp.newAccountWizardController',
                    template: '$(Platform)/Scripts/app/security/wizards/newAccount/new-account-wizard.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:create'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                // return isItemsChecked();
            },
            permission: 'platform:security:delete'
        }
    ];

    // ui-grid
    $scope.gridOptions = {
        data: 'blade.currentEntities',
        enableRowHeaderSelection: true,
        // enableFullRowSelection: true,
        //selectionRowHeaderWidth: 35,
        //rowHeight: 35,
        rowTemplate: "<div ng-click=\"grid.appScope.blade.selectNode(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.uid\" ui-grid-one-bind-id-grid=\"rowRenderIndex + '-' + col.uid + '-cell'\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader, '__selected': row.entity.userName === grid.appScope.selectedNodeId }\" role=\"{{col.isRowHeader ? 'rowheader' : 'gridcell'}}\" ui-grid-cell></div>",
        columnDefs: [
        {
            displayName: 'Name',
            name: 'userName',
            sort: {
                direction: uiGridConstants.ASC,
                priority: 1
            }
        },
        { displayName: 'Account type', name: 'userType' },
        { displayName: 'State', name: 'userState' }
        ],
        onRegisterApi: function (gridApi) {
            //set gridApi on scope
            $scope.gridApi = gridApi;
            //gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            //    if (row.isSelected) {
            //        //blade.selectNode(row.entity);
            //    }
            //});

            //gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            //});
        }
    };


    $scope.$watch('pageSettings.currentPage', blade.refresh);

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);