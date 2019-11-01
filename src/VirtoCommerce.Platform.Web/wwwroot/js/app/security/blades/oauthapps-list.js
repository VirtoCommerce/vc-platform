angular.module('platformWebApp').controller('platformWebApp.oauthappsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.oauthapps', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', function ($scope, bladeNavigationService, dialogService, oauthapps, bladeUtils, uiGridHelper) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';
    blade.allSelected = false;

    blade.refresh = function () {
        blade.isLoading = true;

        var criteria = {
            sort: uiGridHelper.getSortExpression($scope),
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount
        };

        oauthapps.search(criteria, function (data) {
            data.results.forEach(x => delete x.clientSecret);
            blade.isLoading = false;
            blade.currentEntities = data.results;
            $scope.pageSettings.totalItems = data.totalCount;
        }, function (error) {
            blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeId = node.clientId;
        var newBlade = {
            subtitle: 'platform.blades.oauthapps-detail.title',
            data: node
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "oauthapps-detail",
            title: blade.title,
            controller: 'platformWebApp.oAuthAppsController',
            template: '$(Platform)/Scripts/app/security/blades/oauthapps-detail.tpl.html'
        };

        angular.extend(newBlade, node);
        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.deleteList = function (selection) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.oauthapps-delete.title",
            message: "platform.dialogs.oauthapps-delete.message",
            callback: function (remove) {
                if (remove) {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var clientIds = selection.map(x => x.clientId);
                        oauthapps.delete({ clientIds }, blade.refresh);
                    });
                }
            }
        };
        dialogService.showConfirmationDialog(dialog);
    };

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                bladeNavigationService.closeChildrenBlades(blade, function () {
                    blade.selectedData = undefined;
                    var newBlade = {
                        subtitle: 'platform.blades.oauthapps-detail.title-new',
                        isNew: true
                    };
                    openDetailsBlade(newBlade);
                });
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:create'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () { $scope.deleteList($scope.gridApi.selection.getSelectedRows()); },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: 'platform:security:delete'
        }
    ];

    $scope.toggleAll = function () {
        blade.allSelected = !blade.allSelected;
        blade.currentEntities.forEach(x => x.$selected = blade.allSelected);
    };

    var filter = $scope.filter = {};
    filter.criteriaChanged = function () {
        if ($scope.pageSettings.currentPage > 1) {
            $scope.pageSettings.currentPage = 1;
        } else {
            blade.refresh();
        }
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            $scope.gridApi = gridApi;
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    // blade.refresh();
}]);
