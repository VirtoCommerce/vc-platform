angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemVariationListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, dialogService, items) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        blade.isLoading = true;
        items.get({ id: blade.itemId }, function (data) {
            blade.item = data;
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

        if (angular.isUndefined(parentRefresh)) {
            parentRefresh = true;
        }
        if (parentRefresh) {
            blade.parentBlade.refresh();
        }
    }

    $scope.selectVariation = function (listItem) {
        $scope.selectedItem = listItem;

        var newBlade = {
            id: 'variationDetail',
            itemId: listItem.id,
            productType: listItem.productType,
            title: listItem.code,
            subtitle: 'catalog.blades.variation.subtitle',
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.headIcon = 'fa-dropbox';

    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                blade.refresh(false);
            },
            canExecuteMethod: function () { return true; }
        },
	     {
	         name: "platform.commands.add", icon: 'fa fa-plus',
	         executeMethod: function () {
	             items.newVariation({ itemId: blade.itemId }, function (data) {
	                 // take variation properties only
	                 data.properties = _.where(data.properties, { type: 'Variation' });
	                 data.productType = blade.item.productType;

	                 var newBlade = {
	                     id: 'variationDetail',
	                     item: data,
	                     title: "catalog.blades.new-variation.title",
	                     subtitle: 'catalog.blades.new-variation.subtitle',
	                     controller: 'virtoCommerce.catalogModule.newProductWizardController',
	                     template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newProduct/new-variation-wizard.tpl.html'
	                 };
	                 bladeNavigationService.showBlade(newBlade, blade);
	             },
                 function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
	         },
	         canExecuteMethod: function () { return true; },
	         permission: 'catalog:create'
	     },
          {
              name: "platform.commands.delete", icon: 'fa fa-trash-o',
              executeMethod: function () {
                  var dialog = {
                      id: "confirmDeleteItem",
                      title: "catalog.dialogs.variation-delete.title",
                      message: "catalog.dialogs.variation-delete.message",
                      callback: function (remove) {
                          if (remove) {
                              closeChildrenBlades();

                              var ids = [];
                              angular.forEach(blade.item.variations, function (variation) {
                                  if (variation.selected)
                                      ids.push(variation.id);
                              });

                              items.remove({ ids: ids }, blade.refresh, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                          }
                      }
                  }

                  dialogService.showConfirmationDialog(dialog);
              },
              canExecuteMethod: function () {
                  var retVal = false;
                  if (angular.isDefined(blade.item)) {
                      retVal = _.any(blade.item.variations, function (x) { return x.selected; });
                  }
                  return retVal;
              },
              permission: 'catalog:delete'
          }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach(blade.item.variations, function (variation) {
            variation.selected = selected;
        });
    };

    //// actions on load
    //$scope.$watch('blade.parentBlade.item.variations', initializeBlade);

    blade.refresh(false);
}]);
