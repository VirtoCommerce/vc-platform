angular.module('platformWebApp')
.controller('platformWebApp.modulesListController', ['$scope', 'filterFilter', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.modules', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.moduleHelper',
function ($scope, filterFilter, bladeNavigationService, dialogService, modules, uiGridConstants, uiGridHelper, moduleHelper) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentRefresh().then(function (data) {
            blade.currentEntities = data;
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

    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    switch (blade.mode) {
        case 'update':
            blade.toolbarCommands = [{
                name: "platform.commands.update", icon: 'fa fa-arrow-up',
                executeMethod: function () { executeAction('update'); },
                canExecuteMethod: isItemsChecked,
                permission: 'platform:module:manage'
            }];
            break;
        case 'available':
            blade.toolbarCommands = [{
                name: "platform.commands.install", icon: 'fa fa-plus',
                executeMethod: function () { executeAction('install'); },
                canExecuteMethod: isItemsChecked,
                permission: 'platform:module:manage'
            }];
            break;
    }

    function executeAction(action) {
        var selection = _.where($scope.gridApi.selection.getSelectedRows(), { isInstalled: false });
        if (_.any(selection)) {
            bladeNavigationService.closeChildrenBlades(blade, function () {
                blade.isLoading = true;

                selection = angular.copy(selection);
                _.each(selection, function (x) {
                    x.$all = x.$latestModule = undefined;
                });
                modules.getDependencies(selection, function (data) {
                    blade.isLoading = false;
                    data = _.filter(data, function (x) { return _.all(selection, function (s) { return s.id !== x.id; }) });

                    var dialog = {
                        id: "confirm",
                        action: action,
                        selection: selection,
                        dependencies: data,
                        callback: function () {
                            modules.install(selection, onAfterConfirmed, function (error) {
                                bladeNavigationService.setError('Error ' + error.status, blade);
                            });
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
        //var versionColumn = _.findWhere(gridOptions.columnDefs, { name: 'version' });
        switch (blade.mode) {
            //case 'update':
            //    // versionColumn.displayName = 'platform.blades.modules-list.labels.new-version';
            //    break;
            //case 'available':
            //    break;
            case 'installed':
                _.extend(gridOptions, {
                    selectionRowHeaderWidth: 0,
                    enableRowSelection: false,
                    enableSelectAll: false
                });
                break;
        }

        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
        });
    };

    $scope.singleFilter = function (renderableRows) {
        var visibleCount = 0;
        renderableRows.forEach(function (row) {
            row.visible = _.any(filterFilter([row.entity], blade.searchText));
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };


    blade.isLoading = false;
}]);
