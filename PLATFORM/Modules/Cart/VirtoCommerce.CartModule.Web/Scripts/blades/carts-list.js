angular.module('virtoCommerce.cartModule')
.controller('virtoCommerce.cartModule.cartListController', ['$scope', 'virtoCommerce.cartModule.carts', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($scope, carts, bladeNavigationService, dialogService) {
    var blade = $scope.blade;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.selectedAll = false;
    var selectedNode = null;

    blade.refresh = function () {
        blade.isLoading = true;

        carts.cartsSearch({
            keyword: $scope.filter.searchKeyword,
            start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            count: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            blade.isLoading = false;
            $scope.selectedAll = false;

            $scope.pageSettings.totalItems = data.totalCount;
            $scope.objects = data.shopingCarts;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(data.shopingCarts, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.$watch('pageSettings.currentPage', blade.refresh);

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'cartDetail',
            title: 'cart.blades.cart-detail.title',
            titleValues: { name: selectedNode.customerName },
            currentEntityId: selectedNode.id,
            controller: 'virtoCommerce.cartModule.cartDetailController',
            template: 'Modules/$(VirtoCommerce.Cart)/Scripts/blades/cart-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
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
            title: "cart.dialogs.carts-delete.title",
            message: "cart.dialogs.carts-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.objects, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    carts.remove({ ids: itemIds }, function (data, headers) {
                        blade.refresh();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
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

    blade.headIcon = 'fa-shopping-cart';

    blade.toolbarCommands = [
          {
              name: "platform.commands.refresh", icon: 'fa fa-refresh',
              executeMethod: blade.refresh,
              canExecuteMethod: function () {
                  return true;
              }
          },
          //{
          //    name: "Advanced search", icon: 'fa fa-search',
          //    executeMethod: function () {
          //    },
          //    canExecuteMethod: function () {
          //        return false;
          //    }
          //},
          {
              name: "cart.commands.open-in-store", icon: 'fa fa-edit',
              executeMethod: function () {
              },
              canExecuteMethod: function () {
                  return selectedNode;
              }
          },
          {
              name: "platform.commands.delete", icon: 'fa fa-trash-o',
              executeMethod: function () {
                  deleteChecked();
              },
              canExecuteMethod: function () {
                  return isItemsChecked();
              },
              permission: 'cart:delete'
          }
    ];


    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);