angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricelistItemListController', ['$scope', '$filter', 'platformWebApp.bladeNavigationService', function ($scope, $filter, bladeNavigationService) {
    var selectedNode = null;

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.productId;

        var newBlade = {
            id: 'pricelistChildChild',
            itemId: selectedNode.productId,
            data: selectedNode,
            currency: $scope.blade.currency,
            title: 'Prices for ' + selectedNode.name,
            subtitle: 'Edit prices',
            controller: 'virtoCommerce.pricingModule.pricesListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/prices-list.tpl.html'
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
        var newBlade = {
            id: "CatalogItemsSelect",
            title: "Select items for pricing",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
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

        newBlade.options = {
            checkItemFn: function (listItem, isSelected) {
                if (listItem.type == 'category') {
                    newBlade.error = 'Categories are not supported';
                    listItem.selected = undefined;
                } else {
                    if (isSelected) {
                        if (_.all(selectedProducts, function (x) { return x.id != listItem.id; })) {
                            selectedProducts.push(listItem);
                        }
                    }
                    else {
                        selectedProducts = _.reject(selectedProducts, function (x) { return x.id == listItem.id; });
                    }
                    newBlade.error = undefined;
                }
            }
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    function addProductsToPricelist(products) {
        angular.forEach(products, function (product) {
            if (_.all($scope.blade.currentEntities, function (x) { return x.productId != product.id; })) {
                var newPricelistItem =
                {
                    productName: product.name,
                    productId: product.id,
                    prices: []
                };
                $scope.blade.currentEntities.push(newPricelistItem);
            }
        });
    }

    $scope.blade.headIcon = 'fa-usd';

    $scope.blade.toolbarCommands = [
    {
        name: "Add", icon: 'fa fa-plus',
        executeMethod: function () {
            closeChildrenBlades();
            openAddEntityWizard();
        },
        canExecuteMethod: function () {
            return true;
        },
        permission: 'pricing:manage'
    }
    ];

    $scope.getPriceRange = function (priceGroup) {
        var retVal;
        var allPrices = _.union(_.pluck(priceGroup.prices, 'list'), _.pluck(priceGroup.prices, 'sale'));
        var minprice = $filter('number')(_.min(allPrices), 2);
        var maxprice = $filter('number')(_.max(allPrices), 2);
        retVal = (minprice == maxprice ? minprice : minprice + '-' + maxprice);

        //else {
        //    retVal = 'NO PRICE';
        //}

        return retVal;
    }

    $scope.$watch('blade.parentBlade.currentEntity.productPrices', function (currentEntities) {
        // $scope.blade.data = currentEntities;
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.productPrices' gets fired
}]);