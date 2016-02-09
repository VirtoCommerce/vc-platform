angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.advancedSearchResultsController', ['$rootScope', '$scope', '$filter', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($rootScope, $scope, $filter, categories, items, listEntries, bladeNavigationService, dialogService) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;
    
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        var skip = ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount;
        listEntries.listitemssearch(
            {
                catalogId: $scope.blade.catalogId,
                categoryId: $scope.blade.categoryId,
                // propertyValues: .... ,
                responseGroup: 'withProducts',
                skip: skip,
                take: $scope.pageSettings.itemsPerPageCount
            },
		function (data, headers) {
		    $scope.blade.isLoading = false;
		    $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
		    $scope.items = data.listEntries;
		}, function (error) {
		    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
    }
        
    function isItemsChecked() {
        return $scope.items && _.any($scope.items, function (x) { return x.selected; });
    }


    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };
    
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
          executeMethod: blade.refresh,
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
        }      
    ];
    
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);