angular.module('platformWebApp')
    .controller('platformWebApp.modulesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.moduleHelper', '$timeout', '$translate',
function ($scope, bladeNavigationService, uiGridConstants, uiGridHelper, moduleHelper, $timeout, $translate) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    // Filter state (panel toggle & outside-click handled by va-filter-panel directive)
    var filter = $scope.filter = {
        status: '',
        hasActiveFilters: function () {
            return filter.status !== '';
        },
        clearFilters: function () {
            filter.status = '';
            filter.criteriaChanged();
        },
        criteriaChanged: function () {
            if ($scope.gridApi) {
                $scope.gridApi.grid.queueGridRefresh();
            }
        }
    };

    // Pre-select the status filter based on the mode passed from modules-main
    switch (blade.mode) {
        case 'installed':
        case 'withErrors':
            filter.status = 'installed';
            break;
        case 'update':
            filter.status = 'updates';
            break;
        default:
            filter.status = '';
            break;
    }

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentBlade.refresh().then(function () {
            blade.currentEntities = blade.isGrouped ? moduleHelper.moduleBundles : moduleHelper.existingModules;
            blade.isLoading = false;
        });
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
            name: "platform.commands.grouping", icon: 'fas fa-cubes',
            executeMethod: function () {
                blade.isGrouped = !blade.isGrouped;
                if (blade.isGrouped) {
                    this.name = $translate.instant("platform.commands.ungrouping");
                } else {
                    this.name = $translate.instant("platform.commands.grouping");
                }
            },
            canExecuteMethod: function () { return true; },
            permission: 'platform:module:view'
        }
    ];

    $scope.confirmActionInDialog = function (action, selection) {
        moduleHelper.performAction(action, selection, blade, false);
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        if (blade.mode !== 'browse') {
            _.extend(gridOptions, {
                showTreeRowHeader: false
            });
        } else {
            _.extend(gridOptions, {
                enableGroupHeaderSelection: true
            });
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
        var searchText = angular.lowercase(blade.searchText || '');
        var status = filter.status;

        renderableRows.forEach(function (row) {
            // Keyword match: title, description, module id
            var keywordMatch = !searchText ||
                (row.entity.title && row.entity.title.toLowerCase().indexOf(searchText) !== -1) ||
                (row.entity.id && row.entity.id.toLowerCase().indexOf(searchText) !== -1) ||
                (row.entity.description && row.entity.description.toLowerCase().indexOf(searchText) !== -1);

            // Status match
            var statusMatch = true;
            switch (status) {
                case 'installed':
                    statusMatch = row.entity.isInstalled === true;
                    break;
                case 'notInstalled':
                    statusMatch = !row.entity.isInstalled;
                    break;
                case 'updates':
                    statusMatch = row.entity.isInstalled && !!row.entity.$alternativeVersion;
                    break;
            }

            row.visible = keywordMatch && statusMatch;
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };

    // Trigger grid refresh when search text changes (va-filter-panel uses ng-model, not ng-keyup)
    $scope.$watch('blade.searchText', function (newVal, oldVal) {
        if (newVal !== oldVal && $scope.gridApi) {
            $scope.gridApi.grid.queueGridRefresh();
        }
    });

    blade.isLoading = false;
}]);
