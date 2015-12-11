angular.module('platformWebApp')
.controller('platformWebApp.roleListController', ['$scope', 'platformWebApp.roles', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
function ($scope, roles, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
    $scope.uiGridConstants = uiGridConstants;
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

        roles.get({
            keyword: $scope.filter.searchKeyword,
            skipCount: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            takeCount: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            blade.currentEntities = data.roles;

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
            executeMethod: function () {
                blade.refresh();
            },
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

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions);
    };
    
    $scope.$watch('pageSettings.currentPage', blade.refresh);

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);