angular.module('platformWebApp')
.controller('platformWebApp.roleListController', ['$scope', 'platformWebApp.roles', 'platformWebApp.bladeUtils', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper',
function ($scope, roles, bladeUtils, bladeNavigationService, dialogService, uiGridHelper) {
    $scope.uiGridConstants = uiGridHelper.uiGridConstants;
    var blade = $scope.blade;

    blade.refresh = function () {
        blade.isLoading = true;

         roles.search({
            keyword: filter.keyword,
            sort: uiGridHelper.getSortExpression($scope),
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = data.totalCount;
            blade.currentEntities = data.results;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeName = node.name;

        var newBlade = {
            id: 'listItemChild',
            data: node,
            title: node.name,
            subtitle: blade.subtitle,
            controller: 'platformWebApp.roleDetailController',
            template: '$(Platform)/Scripts/app/security/blades/role-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.delete = function (data) {
        deleteList([data]);
    };

    function deleteList(selection) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.roles-delete.title",
            message: "platform.dialogs.roles-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

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
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                closeChildrenBlades();

                var newBlade = {
                    id: 'listItemChild',
                    isNew: true,
                    title: 'platform.blades.new-role-wizard.title',
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
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: 'platform:security:delete'
        }
    ];


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
            uiGridHelper.bindRefreshOnSortChanged($scope);
        });
        bladeUtils.initializePagination($scope);
    };


    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
