angular.module('platformWebApp')
.controller('accountListController', ['$scope', 'accounts', 'bladeNavigationService', 'dialogService',
function ($scope, accounts, bladeNavigationService, dialogService) {
    var selectedNode = null;

    $scope.filter = { searchKeyword: undefined };
    $scope.totalItems = 0;

    $scope.doQuickSearch = function () {
        $scope.totalItems = 5;
    };

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.selectedAll = false;

        accounts.query({}, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'accountDetail',
            data: selectedNode,
            title: selectedNode.name,
            subtitle: $scope.blade.subtitle,
            controller: 'accountDetailController',
            template: 'Scripts/common/security/blades/account-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.checkAll = function (selected) {
        angular.forEach($scope.objects, function (item) {
            item.selected = selected;
        });
    };

    function isItemsChecked() {
        return $scope.objects && _.any($scope.objects, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Accounts?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.objects, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    accounts.remove({ ids: itemIds }, function (data, headers) {
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
                    title: 'New Account',
                    subtitle: $scope.blade.subtitle,
                    isNew: true,
                    controller: 'accountDetailController',
                    template: 'Scripts/common/security/blades/account-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            }
        }
    ];

    // actions on load
    $scope.blade.refresh();
}]);