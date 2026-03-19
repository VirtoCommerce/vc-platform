angular.module('platformWebApp')
    .controller('platformWebApp.modulesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.moduleHelper', '$timeout', '$translate',
function ($scope, bladeNavigationService, uiGridConstants, uiGridHelper, moduleHelper, $timeout, $translate) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentBlade.refresh().then(function (data) {
            blade.currentEntities = blade.isGrouped ? moduleHelper.moduleBundles : data;
            blade.isLoading = false;
        })
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            id: 'moduleDetails',
            title: 'platform.blades.module-detail.title',
            currentEntity: node,
            controller: 'platformWebApp.moduleDetailController',
            template: '$(Platform)/Scripts/app/modularity/blades/module-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    function forUpdateItemsChecked() {
        return $scope.allowInstallModules && $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows()) && !_.any($scope.gridApi.selection.getSelectedRows(), function (row) { return (!row.isInstalled || !row.$alternativeVersion); });
    }

    function installedItemsChecked() {
        return $scope.allowInstallModules && $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows()) && !_.any($scope.gridApi.selection.getSelectedRows(), function (row) { return !row.isInstalled; });
    }

    function newItemsChecked() {
        return $scope.allowInstallModules && $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows()) && !_.any($scope.gridApi.selection.getSelectedRows(), function (row) { return row.isInstalled; });
    }

    // initialize blade.toolbarCommands
    switch (blade.mode) {
        case 'browse':
            blade.toolbarCommands = [
                {
                    name: "platform.commands.install", icon: 'fas fa-plus',
                    executeMethod: function () { $scope.confirmActionInDialog('install', $scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: newItemsChecked,
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.update", icon: 'fa fa-arrow-up',
                    executeMethod: function () { $scope.confirmActionInDialog('update', $scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: forUpdateItemsChecked,
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.uninstall", icon: 'fas fa-trash-alt',
                    executeMethod: function () { $scope.confirmActionInDialog('uninstall', $scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: installedItemsChecked,
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.grouping",
                    icon: 'fas fa-cubes',
                    executeMethod: function () {
                        blade.isGrouped = !blade.isGrouped;
                        if (blade.isGrouped) {
                            this.name = $translate.instant("platform.commands.ungrouping");
                        }
                        else {
                            this.name = $translate.instant("platform.commands.grouping");
                        }
                    },
                    canExecuteMethod: function () { return true; },
                    permission: 'platform:module:view'
                }
            ];
            break;
        case 'update':
        case 'installed':
            blade.toolbarCommands = [
                {
                    name: "platform.commands.update", icon: 'fa fa-arrow-up',
                    executeMethod: function () { $scope.confirmActionInDialog('update', $scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: forUpdateItemsChecked,
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.uninstall", icon: 'fas fa-trash-alt',
                    executeMethod: function () { $scope.confirmActionInDialog('uninstall', $scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: installedItemsChecked,
                    permission: 'platform:module:manage'
                }
            ];
            break;
    }

    $scope.confirmActionInDialog = function (action, selection) {
        moduleHelper.performAction(action, selection, blade, false);
    }

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        switch (blade.mode) {
            case 'update':
            case 'installed':
                _.extend(gridOptions, {
                    showTreeRowHeader: false
                });
                break;
            case 'browse':
                _.extend(gridOptions, {
                    enableGroupHeaderSelection: true
                });
                break;
        }

        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);

            if (blade.mode === 'browse') {
                $scope.$watch('blade.isGrouped', function (isGrouped) {
                    if (isGrouped) {
                        blade.currentEntities = moduleHelper.moduleBundles;
                        if (!_.any(gridApi.grouping.getGrouping().grouping)) {
                            gridApi.grouping.groupColumn('$group');
                        }
                        $timeout(gridApi.treeBase.expandAllRows);
                    } else {
                        blade.currentEntities = moduleHelper.existingModules;
                        gridApi.grouping.clearGrouping();
                    }
                });

                // toggle grouped rows selection
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.internalRow) {
                        let parentSelected = row.isSelected;
                        _.each(row.treeNode.children, function (treeNode) {
                            if (!parentSelected) {
                                gridApi.selection.selectRow(treeNode.row.entity); // this is unselect
                            } else {
                                gridApi.selection.unSelectRow(treeNode.row.entity); // this is select
                            }
                        });
                    }
                });

                $scope.toggleRow = function (row) {
                    gridApi.treeBase.toggleRowTreeState(row);
                };

                $scope.getGroupInfo = function (groupEntity) {
                    return _.values(groupEntity)[0];
                };
            }
        });
    };

    $scope.singleFilter = function (renderableRows) {
        var visibleCount = 0;
        renderableRows.forEach(function (row) {
            var searchText = angular.lowercase(blade.searchText);
            row.visible = !searchText ||
                            row.entity.title.toLowerCase().indexOf(searchText) !== -1 ||
                            row.entity.tags.toLowerCase().indexOf(searchText) !== -1 ||
                            row.entity.version.toLowerCase().indexOf(searchText) !== -1 ||
                            row.entity.description.toLowerCase().indexOf(searchText) !== -1;
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };

    blade.isLoading = false;
}]);
