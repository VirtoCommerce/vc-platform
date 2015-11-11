angular.module('platformWebApp')
.controller('platformWebApp.roleListController', ['$scope', 'platformWebApp.roles', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
function ($scope, roles, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
    var blade = $scope.blade;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 4;
    $scope.pageSettings.itemsPerPageCount = 15;

    $scope.filter = { searchKeyword: undefined };
    var selectedNode = null;

    blade.refresh = function () {
        blade.isLoading = true;
        blade.selectedAll = false;

        roles.get({
            keyword: $scope.filter.searchKeyword,
            skipCount: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            takeCount: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            blade.currentEntities = data.roles;
            uiGridHelper.onDataLoaded($scope.gridOptions, blade.currentEntities);

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
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'listItemChild',
            data: selectedNode,
            title: selectedNode.name,
            subtitle: blade.subtitle,
            controller: 'platformWebApp.roleDetailController',
            template: '$(Platform)/Scripts/app/security/blades/role-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    //$scope.toggleAll = function () {
    //    angular.forEach(blade.currentEntities, function (item) {
    //        item.selected = blade.selectedAll;
    //    });
    //};

    function isItemsChecked() {
        return blade.currentEntities && _.any(blade.currentEntities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Roles?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = $scope.gridApi.selection.getSelectedRows();
                    var itemIds = _.pluck(selection, 'id');
                    roles.remove({ ids: itemIds }, function () {
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
                    isNew: true,
                    title: 'New Role',
                    subtitle: blade.subtitle,
                    controller: 'platformWebApp.roleDetailController',
                    template: '$(Platform)/Scripts/app/security/wizards/new-role-wizard.tpl.html'
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
            },
            permission: 'platform:security:delete'
        }
    ];

    // ui-grid
    uiGridHelper.initialize($scope, {
        rowTemplate: "<div ng-click=\"grid.appScope.blade.selectNode(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.uid\" ui-grid-one-bind-id-grid=\"rowRenderIndex + '-' + col.uid + '-cell'\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader, '__selected': row.entity.id === grid.appScope.selectedNodeId }\" role=\"{{col.isRowHeader ? 'rowheader' : 'gridcell'}}\" ui-grid-cell style='cursor:pointer'></div>",
        rowHeight: 50,
        columnDefs: [
                    {
                        displayName: 'Role',
                        name: 'customColumn',
                        cellTemplate: 'role-list-name.cell.html'
                        // sort: { direction: uiGridConstants.ASC }
                    }
        ]
    });
    
    $scope.$watch('pageSettings.currentPage', blade.refresh);

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);