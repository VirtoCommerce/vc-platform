angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteItemsController', ['$scope', 'focus', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.quoteModule.quotes', 'virtoCommerce.catalogModule.items', 'virtoCommerce.pricingModule.prices', function ($scope, focus, bladeNavigationService, dialogService, quotes, items, prices) {
    var blade = $scope.blade;
    blade.updatePermission = 'quote:update';

    // set initial values to totals 
    $scope.totals = {
        isDiscountAbsolute: blade.currentEntity.manualSubTotal > 0
    };

    // adjust item.selectedTierPrice reference
    _.each(blade.currentEntity.items, function (item) {
        if (item.selectedTierPrice) {
            item.selectedTierPrice = _.findWhere(item.proposalPrices, { price: item.selectedTierPrice.price, quantity: item.selectedTierPrice.quantity });
        }
    });

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
                        product: { code: data.code },
                        catalogId: data.catalogId,
                        categoryId: data.categoryId,
                        name: data.name,
                        imageUrl: data.imgSrc,
                        taxType: data.taxType,
                        quantity: 1,
                        listPrice: (price && price.list) ? price.list : 0,
                        salePrice: (price && price.sale) ? price.sale : 0,
                        tax: 0,
                        discountAmount: 0,
                        currency: blade.currentEntity.currency
                    };
                    newLineItem.proposalPrices = [{ quantity: 1, price: undefined }];
                    newLineItem.selectedTierPrice = newLineItem.proposalPrices[0];
                    blade.currentEntity.items.push(newLineItem);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        });
    }


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
            title: "quotes.blades.catalog-items-select.title",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
			  {
			      name: "quotes.commands.add-selected", icon: 'fa fa-plus',
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

    blade.headIcon = 'fa-file-text-o';

    blade.toolbarCommands = [
        {
            name: "quotes.commands.add-item", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.remove", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var lineItems = blade.currentEntity.items;
                blade.currentEntity.items = _.difference(lineItems, _.filter(lineItems, function (x) { return x.$selected }));

            },
            canExecuteMethod: function () {
                return _.any(blade.currentEntity.items, function (x) { return x.$selected; });;
            },
            permission: blade.updatePermission
        }
    ];

    $scope.selectItem = function (node) {
        $scope.selectedNodeId = node.id;
    };

    $scope.checkAll = function (selected) {
        angular.forEach(blade.currentEntity.items, function (item) {
            item.$selected = selected;
        });
    };

    $scope.recalculate = function () {
        blade.recalculateFn();
    };

    $scope.addProposalTier = function (item, uiIndex) {
        item.proposalPrices.push({ quantity: 1, price: undefined });
        focus('focusIndex' + uiIndex);
    };

    $scope.deleteProposalTier = function (item) {
        var idx;
        if (item.selectedTierPrice && item.proposalPrices.length > 1 && (idx = item.proposalPrices.indexOf(item.selectedTierPrice)) >= 0) {
            item.proposalPrices.splice(idx, 1);
        }
    };

    $scope.getMargin = function (item, proposal) {
        if (proposal.price && (item.listPrice || item.salePrice)) {
            var itemPrice = item.salePrice ? item.salePrice : item.listPrice;
            return Math.round((proposal.price - itemPrice) / proposal.price * 100);
        } else {
            return '';
        }
    };
}]);