angular.module('platformWebApp')
.controller('platformWebApp.roleListController', ['$scope', 'platformWebApp.roles', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($scope, roles, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 4;
    $scope.pageSettings.itemsPerPageCount = 15;

    $scope.filter = { searchKeyword: undefined };
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.selectedAll = false;

        roles.get({
            keyword: $scope.filter.searchKeyword,
            skipCount: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            takeCount: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            $scope.blade.isLoading = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            $scope.blade.currentEntities = data.roles;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach($scope.blade.currentEntities, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.blade.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'listItemChild',
            data: selectedNode,
            title: selectedNode.name,
            subtitle: $scope.blade.subtitle,
            controller: 'platformWebApp.roleDetailController',
            template: 'Scripts/common/security/blades/role-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.toggleAll = function () {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = $scope.blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return $scope.blade.currentEntities && _.any($scope.blade.currentEntities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Roles?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.blade.currentEntities, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    roles.remove({ ids: itemIds }, function () {
                        $scope.blade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeHeadIco = 'fa-lock';

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
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
                    subtitle: $scope.blade.subtitle,
                    controller: 'platformWebApp.roleDetailController',
                    template: 'Scripts/common/security/wizards/new-role-wizard.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: 'platform:security:manage'
        }
    ];

    $scope.$watch('pageSettings.currentPage', function () {
        $scope.blade.refresh();
    });

    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);