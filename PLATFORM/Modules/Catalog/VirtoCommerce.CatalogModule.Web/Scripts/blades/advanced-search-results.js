﻿angular.module('virtoCommerce.catalogModule')
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
		    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
    }

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
    });
    
    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "catalog.dialogs.nothing-selected.title",
                message: "catalog.dialogs.nothing-selected.message"
            };
            dialogService.showNotificationDialog(dialog);
        }
    };

    function isItemsChecked() {
        return $scope.items && _.any($scope.items, function (x) { return x.selected; });
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
          name: "platform.commands.refresh", icon: 'fa fa-refresh',
          executeMethod: function () {
              $scope.blade.refresh();
          },
          canExecuteMethod: function () {
              return true;
          }
      },
        {
            name: "platform.commands.manage", icon: 'fa fa-edit',
            executeMethod: function () {
                $scope.edit($scope.selectedItem);
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
            }
        },
      {
          name: "platform.commands.delete", icon: 'fa fa-trash-o',
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