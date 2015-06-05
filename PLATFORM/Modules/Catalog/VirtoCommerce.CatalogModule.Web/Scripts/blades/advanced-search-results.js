angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.advancedSearchResultsController', ['$rootScope', '$scope', '$filter', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($rootScope, $scope, $filter, categories, items, listEntries, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        var skip = ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount;
        listEntries.listitemssearch(
            {
                catalog: $scope.blade.catalogId,
                category: $scope.blade.categoryId,
                // propertyValues: .... ,
                respGroup: 'withProducts',
                start: skip,
                count: $scope.pageSettings.itemsPerPageCount
            },
		function (data, headers) {
		    $scope.blade.isLoading = false;
		    $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
		    $scope.items = data.listEntries;
		    $scope.selectedAll = false;

		    if ($scope.selectedItem != null) {
		        $scope.selectedItem = $scope.findItem($scope.selectedItem.id);
		    }
		}, function (error) {
		    $scope.blade.isLoading = false;
		    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
    }

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
    });

    $scope.edit = function (listItem) {
        $scope.selectedItem = listItem;
        $scope.blade.showItemBlade(listItem.id, listItem.name);
    };

    $scope.blade.showItemBlade = function (id, title) {
        var newBlade = {
            id: "listItemDetail",
            itemId: id,
            title: title,
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "Message",
                message: "Nothing selected. Check some Categories or Items first."
            };
            dialogService.showNotificationDialog(dialog);
        }
    };

    function isItemsChecked() {
        return $scope.items && _.any($scope.items, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Items?",
            callback: function (remove) {
                if (remove) {
                    var selection = $filter('filter')($scope.items, { selected: true }, true);
                    var itemIds = [];
                    angular.forEach(selection, function (listItem) {
                        itemIds.push(listItem.id);
                    });

                    if (itemIds.length > 0) {
                        items.remove({ ids: itemIds }, function (data, headers) {
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    $scope.findItem = function (id) {
        var retVal;
        angular.forEach($scope.items, function (item) {
            if (item.id == id)
                retVal = item;
        });

        return retVal;
    }

    $scope.blade.onClose = function (closeCallback) {
        if ($scope.blade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.blade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.blade.toolbarCommands = [
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
            name: "Manage", icon: 'fa fa-edit',
            executeMethod: function () {
                $scope.edit($scope.selectedItem);
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
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

    $scope.checkAll = function (selected) {
        angular.forEach($scope.items, function (item) {
            item.selected = selected;
        });
    };

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);