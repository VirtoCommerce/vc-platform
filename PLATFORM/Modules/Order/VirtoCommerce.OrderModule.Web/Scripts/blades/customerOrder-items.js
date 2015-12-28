﻿angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.customerOrderItemsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.orderModule.calculateTotalsService', 'virtoCommerce.catalogModule.items', 'virtoCommerce.pricingModule.prices', function ($scope, bladeNavigationService, dialogService, calculateTotalsService, items, prices) {
    //pagination settings
    $scope.pageSettings = {};
    $scope.totals = {};
    $scope.pageSettings.totalItems = $scope.blade.currentEntity.items.length;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 4;

    var selectedNode = null;
    var selectedProducts = [];

    $scope.$watch("blade.currentEntity", function (operation) {
        calculateTotalsService.recalculateTotals(operation);
    }, true);

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = false;
        $scope.blade.selectedAll = false;
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

    function addProductsToOrder(products) {
        angular.forEach(products, function (product) {
            items.get({ id: product.id }, function (data) {
                prices.getProductPrices({ id: product.id }, function (prices) {
                    var price = _.find(prices, function (x) { return x.currency == $scope.blade.currentEntity.currency });

                    var newLineItem =
					{
					    productId: data.id,
					    catalogId: data.catalogId,
					    categoryId: data.categoryId,
					    name: data.name,
					    imageUrl: data.imgSrc,
					    sku: data.code,
					    quantity: 1,
					    price: price ? (price.sale ? price.sale : price.list) : 0,
					    tax: 0,
					    discountAmount: 0,
					    currency: $scope.blade.currentEntity.currency
					};
                    $scope.blade.currentEntity.items.push(newLineItem);
                    $scope.pageSettings.totalItems = $scope.blade.currentEntity.items.length;
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        });
    };

    $scope.openItemDynamicProperties = function (item) {
        var blade = {
            id: "dynamicPropertiesList",
            currentEntity: item,
            controller: 'platformWebApp.propertyValueListController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };

    $scope.openItemDetail = function (item) {
        var newBlade = {
            id: "listItemDetail",
            itemId: item.productId,
            title: item.name,
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
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
            currentEntities: $scope.blade.currentEntity,
            title: "orders.blades.catalog-items-select.title",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
			  {
			      name: "orders.commands.add-selected", icon: 'fa fa-plus',
			      executeMethod: function (blade) {
			          addProductsToOrder(selectedProducts);
			          selectedProducts.length = 0;
			          bladeNavigationService.closeBlade(blade);

			      },
			      canExecuteMethod: function () {
			          return selectedProducts.length > 0;
			      }
			  }]
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.headIcon = 'fa-file-text';

    $scope.blade.toolbarCommands = [
        {
            name: "orders.commands.add-item", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return $scope.blade.currentEntity.operationType.toLowerCase() == 'customerorder';
            },
            permission: 'order:update'
        },
        {
            name: "platform.commands.remove", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var lineItems = $scope.blade.currentEntity.items;
                $scope.blade.currentEntity.items = _.difference(lineItems, _.filter(lineItems, function (x) { return x.selected }));
                $scope.pageSettings.totalItems = $scope.blade.currentEntity.items.length;
            },
            canExecuteMethod: function () {
                return _.any($scope.blade.currentEntity.items, function (x) { return x.selected; });;
            },
            permission: 'order:update'
        }
    ];

    //$scope.$watch('pageSettings.currentPage', function (newPage) {
    //    $scope.blade.refresh();
    //});

    $scope.selectItem = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;
    };

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntity.items, function (item) {
            item.selected = selected;
        });
    };

    $scope.blade.refresh();
}]);