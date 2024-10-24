angular.module('platformWebApp')
.controller('platformWebApp.modulesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modules', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.moduleHelper', '$timeout',
function ($scope, bladeNavigationService, dialogService, modules, uiGridConstants, uiGridHelper, moduleHelper, $timeout) {
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
        if (_.any(selection)) {
            bladeNavigationService.closeChildrenBlades(blade, function () {
                blade.isLoading = true;

                // eliminate duplicating nodes, if any
                var grouped = _.groupBy(selection, 'id');
                selection = [];
                _.each(grouped, function (vals) {
                    selection.push(_.last(vals));
                });

                var modulesApiMethod = action === 'uninstall' ? modules.getDependents : modules.getDependencies;
                modulesApiMethod(selection, function (data) {
                    blade.isLoading = false;

                    var dialog = {
                        id: "confirm",
                        action: action,
                        selection: selection,
                        dependencies: data,
                        callback: function (resume) {
                            if (resume) {
                                _.each(selection, function (x) {
                                    if (!_.findWhere(data, { id: x.id })) {
                                        data.push(x);
                                    }
                                });
                                modulesApiMethod = action === 'uninstall' ? modules.uninstall : modules.install;
                                modulesApiMethod(data, onAfterConfirmed, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                            }
                        }
                    }
                    dialogService.showDialog(dialog, '$(Platform)/Scripts/app/modularity/dialogs/moduleAction-dialog.tpl.html', 'platformWebApp.confirmDialogController');
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            });
        }
    }

    function onAfterConfirmed(data) {
        var newBlade = {
            id: 'moduleInstallProgress',
            currentEntity: data,
            title: blade.title,
            controller: 'platformWebApp.moduleInstallProgressController',
            template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
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
                                gridApi.selection.selectRow(treeNode.row.entity);//this is unselect
                            } else {
                                gridApi.selection.unSelectRow(treeNode.row.entity);//this is select
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
