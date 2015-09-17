angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteItemsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.catalogModule.items', 'virtoCommerce.pricingModule.prices', function ($scope, bladeNavigationService, dialogService, quotes, items, prices) {
    var blade = $scope.blade;

    $scope.totals = {};
    //$scope.totalItems = blade.currentEntity.items.length;

    // adjust item.selectedTierPrice reference
    _.each(blade.currentEntity.items, function (item) {
        if (item.selectedTierPrice) {
            item.selectedTierPrice = _.findWhere(item.proposalPrices, { price: item.selectedTierPrice.price, quantity: item.selectedTierPrice.quantity });
        }
    });

    var selectedNode = null;
    var selectedProducts = [];
    blade.isLoading = false;

    function addProducts(products) {
        angular.forEach(products, function (product) {
            items.get({ id: product.id }, function (data) {
                prices.getProductPrices({ id: product.id }, function (prices) {
                    var price = _.find(prices, function (x) { return x.currency == blade.currentEntity.currency; });

                    var newLineItem =
					{
					    productId: data.id,
					    catalogId: data.catalogId,
					    categoryId: data.categoryId,
					    name: data.name,
					    imageUrl: data.imgSrc,
					    quantity: 1,
					    listPrice: price ? price.sale : 0,
					    salePrice: price ? price.sale : 0,
					    tax: 0,
					    discountAmount: 0,
					    currency: blade.currentEntity.currency,
					    proposalPrices:[]
					};
                    blade.currentEntity.items.push(newLineItem);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        });
    };

    //$scope.openItemDynamicProperties = function (item) {
    //    var newBlade = {
    //        id: "dynamicPropertiesList",
    //        currentEntity: item,
    //        controller: 'platformWebApp.propertyValueListController',
    //        template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
    //    };
    //    bladeNavigationService.showBlade(newBlade, blade);
    //};

    $scope.openItemDetail = function (item) {
        var newBlade = {
            id: "listItemDetail",
            itemId: item.productId,
            title: item.name,
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    function openAddEntityWizard() {
        var options = {
            checkItemFn: function (listItem, isSelected) {
                if (isSelected) {
                    if (_.all(selectedProducts, function (x) { return x.id != listItem.id; })) {
                        selectedProducts.push(listItem);
                    }
                }
                else {
                    selectedProducts = _.reject(selectedProducts, function (x) { return x.id == listItem.id; });
                }
            }
        };
        var newBlade = {
            id: "CatalogItemsSelect",
            currentEntities: blade.currentEntity,
            title: "Add item to quote",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
			  {
			      name: "Add selected", icon: 'fa fa-plus',
			      executeMethod: function (blade) {
			          addProducts(selectedProducts);
			          selectedProducts.length = 0;
			          bladeNavigationService.closeBlade(blade);

			      },
			      canExecuteMethod: function () {
			          return selectedProducts.length > 0;
			      }
			  }]
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.headIcon = 'fa-file-text';

    blade.toolbarCommands = [
        {
            name: "Add item", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'quote:manage'
        },
        {
            name: "Remove", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var lineItems = blade.currentEntity.items;
                blade.currentEntity.items = _.difference(lineItems, _.filter(lineItems, function (x) { return x.$selected }));

            },
            canExecuteMethod: function () {
                return _.any(blade.currentEntity.items, function (x) { return x.$selected; });;
            },
            permission: 'quote:manage'
        }
    ];

    $scope.selectItem = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.checkAll = function (selected) {
        angular.forEach(blade.currentEntity.items, function (item) {
            item.$selected = selected;
        });
    };

    $scope.recalculate = function () {
        quotes.recalculate({}, blade.currentEntity, function (data) {
            blade.currentEntity.totals = data.totals;
            bladeNavigationService.setError(null, blade);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };
    $scope.addProposalTier = function (tiers) {
        tiers.push({});
    };

    $scope.getMargin = function (item, proposal) {
        return 0;
    };
}]);