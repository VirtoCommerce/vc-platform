angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricelistItemListController', ['$scope', '$filter', 'platformWebApp.bladeNavigationService', 'filterFilter', 'uiGridConstants', 'platformWebApp.uiGridHelper', function ($scope, $filter, bladeNavigationService, filterFilter, uiGridConstants, uiGridHelper) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    function initializeBlade(data) {
        blade.currentEntities = data;
        $scope.pageSettings.totalItems = data.length;
        blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.productId;

        var newBlade = {
            id: 'pricelistChildChild',
            itemId: node.productId,
            data: node,
            currency: blade.currency,
            title: 'pricing.blades.prices-list.title',
            titleValues: { name: node.productName },
            subtitle: 'pricing.blades.prices-list.subtitle',
            controller: 'virtoCommerce.pricingModule.pricesListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/prices-list.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    function openAddEntityWizard() {
        var selectedProducts = [];
        var newBlade = {
            id: "CatalogItemsSelect",
            title: "Select items for pricing", //catalogItemSelectController hardcode set title 
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "pricing.commands.add-selected", icon: 'fa fa-plus',
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

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function addProductsToPricelist(products) {
        angular.forEach(products, function (product) {
            if (_.all(blade.currentEntities, function (x) { return x.productId != product.id; })) {
                var newPricelistItem =
                {
                    productName: product.name,
                    productId: product.id,
                    prices: []
                };
                blade.currentEntities.push(newPricelistItem);
            }
        });
    }

    blade.headIcon = 'fa-usd';

    blade.toolbarCommands = [
    {
        name: "platform.commands.add", icon: 'fa fa-plus',
        executeMethod: function () {
            openAddEntityWizard();
        },
        canExecuteMethod: function () {
            return true;
        },
        permission: 'pricing:update'
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

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions,
        function (gridApi) {
            gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
            $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
        });
    };

    $scope.singleFilter = function (renderableRows) {
        var visibleCount = 0;
        renderableRows.forEach(function (row) {
            row.visible = _.any(filterFilter([row.entity], blade.searchText));
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };

    $scope.$watch('blade.parentBlade.currentEntity.productPrices', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.productPrices' gets fired
}]);