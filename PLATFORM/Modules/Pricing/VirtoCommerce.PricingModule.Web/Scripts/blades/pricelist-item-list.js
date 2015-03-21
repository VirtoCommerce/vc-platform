angular.module('virtoCommerce.pricingModule')
.controller('pricelistItemListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var selectedNode = null;

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;

        // temporal workaround
        selectedNode = data[0];
        $scope.selectedNodeId = selectedNode.id;
        var newBlade = {
            id: 'pricelistChildChild',
            currentEntityId: selectedNode.id,
            title: 'Prices for ' + selectedNode.name,
            subtitle: 'Edit prices',
            controller: 'pricelistPricesListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-prices-list.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'pricelistChildChild',
            currentEntityId: selectedNode.id,
            title: 'Prices for ' + selectedNode.name,
            subtitle: 'Edit prices',
            controller: 'pricelistPricesListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-prices-list.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function openAddEntityWizard() {
        var selectedProducts = [];
        var options = {
            allowMultiple: true,
            checkItemFn: function (product, isSelected) {
                if (isSelected)
                    selectedProducts.push(product);
                else {
                    var toRemove = _.find(selectedProducts, function (x) { return x.id == product.id; });
                    if (toRemove) {
                        selectedProducts = _.without(selectedProducts, [toRemove]);
                    }
                }
            }
        };
        var newBlade = {
            id: "CatalogItemsSelect",
            currentEntities: $scope.blade.currentEntity,
            title: "Select items for pricing",
            controller: 'catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            bladeToolbarCommands: [
			  {
			      name: "Add selected", icon: 'fa fa-plus',
			      executeMethod: function (blade) {
			          addProductsToPricelist(selectedProducts);
			          bladeNavigationService.closeBlade(blade);
			      },
			      canExecuteMethod: function () {
			          return selectedProducts.length > 0;
			      }
			  }]
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    function addProductsToPricelist(products) {
        angular.forEach(products, function (product) {
            if (_.all($scope.blade.currentEntities, function (x) { return x.productId != product.id; })) {
                var newPricelistItem =
					{
					    productId: product.id,
					    name: product.name
					};
                $scope.blade.currentEntities.push(newPricelistItem);
            }
        });
    }

    $scope.bladeHeadIco = 'fa-usd';

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                closeChildrenBlades();
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.$watch('blade.parentBlade.currentEntity.prices', function (currentEntities) {
        // $scope.blade.data = currentEntities;
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.prices' gets fired
}]);