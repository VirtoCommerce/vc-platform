angular.module('virtoCommerce.orderModule')
.controller('operationItemsController', ['$scope', 'bladeNavigationService', 'dialogService', 'calculateTotalsService', 'items', 'prices', function ($scope, bladeNavigationService, dialogService, calculateTotalsService, items, prices) {
	//pagination settigs
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
						quantity: 1,
						price: price ? price.sale : 0,
						tax: 0,
						discountAmount: 0,
						currency: $scope.blade.currentEntity.currency
					};
					$scope.blade.currentEntity.items.push(newLineItem);
				});

			});

		});
	};

	function openAddEntityWizard() {
		var options = {
			allowMultiple: true,
			checkItemFn: function (product, isSelected) {
				if (isSelected)
					selectedProducts.push(product);
				else {
					var toRemove = _.find(selectedProducts, function (x) { return x.id == product.id });
					if (toRemove) {
						selectedProducts = _.without(selectedProducts, [toRemove])
					}
				}
			}
		};
		var newBlade = {
			id: "CatalogItemsSelect",
			currentEntities: $scope.blade.currentEntity,
			title: "Add item to order",
			controller: 'catalogItemSelectController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
			options: options,
			breadcrumbs: [],
			bladeToolbarCommands: [
			  {
			  	name: "Add selected", icon: 'fa fa-plus',
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

	$scope.bladeHeadIco = 'fa-file-text';

	$scope.bladeToolbarCommands = [
        {
        	name: "Add item", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openAddEntityWizard();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
        {
        	name: "Remove", icon: 'fa fa-trash-o',
        	executeMethod: function () {
        		var lineItems = $scope.blade.currentEntity.items;
        		$scope.blade.currentEntity.items = _.difference(lineItems, _.filter(lineItems, function (x) { return x.selected }));

        	},
        	canExecuteMethod: function () {
        		return _.any($scope.blade.currentEntity.items, function (x) { return x.selected; });;
        	}
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