angular.module('virtoCommerce.catalogModule')
.controller('itemVariationListController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items) {

	$scope.blade.refresh = function (parentRefresh) {
		$scope.blade.isLoading = true;
        items.get({ id: $scope.blade.itemId }, function (data) {
            $scope.blade.item = data;
            $scope.blade.isLoading = false;
        });

        if (angular.isUndefined(parentRefresh)) {
            parentRefresh = true;
        }
        if (parentRefresh) {
            $scope.blade.parentBlade.refresh();
        }
    }

    $scope.selectVariation = function (listItem) {
        $scope.selectedItem = listItem;

        var blade = {
            id: 'variationDetail',
            itemId: listItem.id,
            title: listItem.code,
            subtitle: 'Variation details',
            controller: 'itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh(false);
            },
            canExecuteMethod: function () {
                return true;
            }
        },
      {
          name: "Delete", icon: 'fa fa-trash-o',
          executeMethod: function () {
              var dialog = {
                  id: "confirmDeleteItem",
                  title: "Delete confirmation",
                  message: "Are you sure you want to delete selected Variations?",
                  callback: function (remove) {
                      if (remove) {
                          closeChildrenBlades();

                          var ids = [];
                          angular.forEach($scope.blade.item.variations, function (variation) {
                              if (variation.selected)
                                  ids.push(variation.id);
                          });

                          items.remove({ ids: ids }, function () {
                              $scope.blade.refresh();
                          });
                      }
                  }
              }

              dialogService.showConfirmationDialog(dialog);
          },
          canExecuteMethod: function () {
              var retVal = false;
              if (angular.isDefined($scope.blade.item)) {
                  retVal = _.any($scope.blade.item.variations, function (x) { return x.selected; });
              }
              return retVal;
          }
      }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.item.variations, function (variation) {
            variation.selected = selected;
        });
    };


    $scope.blade.refresh(false);
}]);
