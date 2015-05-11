angular.module('virtoCommerce.cartModule')
.controller('virtoCommerce.cartModule.cartListController', ['$scope', 'virtoCommerce.cartModule.carts', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($scope, carts, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.selectedAll = false;
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        carts.cartsSearch({
            keyword: $scope.filter.searchKeyword,
            start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            count: $scope.pageSettings.itemsPerPageCount
        }, function (data) {
            $scope.blade.isLoading = false;
            $scope.selectedAll = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            $scope.objects = data.shopingCarts;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(data.shopingCarts, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        });
    };

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
    });

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'cartDetail',
            title: selectedNode.customerName + '\'s Shopping cart',
            currentEntityId: selectedNode.id,
            controller: 'virtoCommerce.cartModule.cartDetailController',
            template: 'Modules/$(VirtoCommerce.Cart)/Scripts/blades/cart-detail.tpl.html'
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
            message: "Are you sure you want to delete selected Carts?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.objects, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    carts.remove({ ids: itemIds }, function (data, headers) {
                        $scope.blade.refresh();
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

    $scope.bladeHeadIco = 'fa-shopping-cart';

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
          //{
          //    name: "Advanced search", icon: 'fa fa-search',
          //    executeMethod: function () {
          //    },
          //    canExecuteMethod: function () {
          //        return false;
          //    }
          //},
          {
              name: "Open in Store", icon: 'fa fa-edit',
              executeMethod: function () {
              },
              canExecuteMethod: function () {
                  return selectedNode;
              }
          },
          {
              name: "Delete", icon: 'fa fa-trash-o',
              executeMethod: function () {
                  deleteChecked();
              },
              canExecuteMethod: function () {
                  return isItemsChecked();
              },
              permission: 'cart:manage'
          }
    ];


    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);