angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogItemSelectController', ['$rootScope', '$scope', '$filter', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($rootScope, $scope, $filter, categories, items, catalogs, listEntries, bladeNavigationService, dialogService) {

	$scope.blade.title = "Catalog items selection...";

	//pagination settings
	$scope.pageSettings = {};
	$scope.pageSettings.totalItems = 0;
	$scope.pageSettings.currentPage = 1;
	$scope.pageSettings.numPages = 5;
	$scope.pageSettings.itemsPerPageCount = 20;

	$scope.filter = { searchKeyword: undefined };

	$scope.options = $scope.blade.options;
	$scope.bladeToolbarCommands = $scope.blade.bladeToolbarCommands;

	$scope.selectedAll = false;
	$scope.selectedItem = null;
	
	$scope.blade.refresh = function () {
		$scope.blade.isLoading = true;

		if (!$scope.isCatalogSelectMode()) {
			listEntries.listitemssearch(
				{
					catalog: $scope.blade.catalogId,
					category: $scope.blade.categoryId,
					q: $scope.filter.searchKeyword,
					// propertyValues: ,
					respGroup: 'withCategories, withProducts',
					start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
					count: $scope.pageSettings.itemsPerPageCount
				},
			function (data, headers) {
				$scope.blade.isLoading = false;
				$scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
				$scope.items = data.listEntries;
				$scope.selectedAll = false;

				if ($scope.selectedItem != null) {
					$scope.selectedItem = _.find($scope.items, function (x) { return $scope.selectedItem.id == x.id }); 
				}

				//Set navigation breadcrumbs
				setBreadcrumps();

			}, function (error) {
				$scope.blade.isLoading = false;
				bladeNavigationService.setError('Error ' + error.status, $scope.blade);
			});
		}
		else {
			var searchResult = catalogs.getCatalogs({}, function (results) {
				$scope.blade.isLoading = false;

				$scope.items = results;
				//Set navigation breadcrumbs
				setBreadcrumps();

			}, function (error) {
				$scope.blade.isLoading = false;
				bladeNavigationService.setError('Error ' + error.status, $scope.blade);
			});
		}
	}

	//Breadcrumps
	function setBreadcrumps() {
		//Clone array (angular.copy leave a same reference)
		$scope.blade.breadcrumbs = $scope.blade.breadcrumbs.slice(0);

		//catalog breadcrump by default
		var breadCrumb = {
			id: $scope.blade.catalogId ? $scope.blade.catalogId : "All",
			name: $scope.blade.catalog ? $scope.blade.catalog.name : "All",
			blade: $scope.blade
		};

		//if category need change to category breadcrumb
		if ($scope.blade.category) {

			breadCrumb.id = $scope.blade.categoryId;
			breadCrumb.name = $scope.blade.category.name;
		}

		//prevent dublicate items
		if (!_.some($scope.blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
			$scope.blade.breadcrumbs.push(breadCrumb);
		}

		breadCrumb.navigate = function (breadcrumb) {
			bladeNavigationService.closeBlade($scope.blade,
						function () {
							if (breadcrumb.id == "All") {
								$scope.blade.catalogId = null;
							}
							bladeNavigationService.showBlade($scope.blade, $scope.blade.parentBlade);
							$scope.blade.refresh();
						});
		};
	}

	$scope.isCatalogSelectMode = function () {
		return !$scope.blade.catalogId;
	};

	$scope.$watch('pageSettings.currentPage', function (newPage) {
		$scope.blade.refresh();
	});

	$scope.blade.showItemBlade = function (id, title) {
		var newBlade = {
			id: "listItemDetail",
			itemId: id,
			title: title,
			subtitle: 'Item details',
			controller: 'virtoCommerce.catalogModule.itemDetailController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};


	function isItemsChecked() {
		return $scope.items && _.any($scope.items, function (x) { return x.selected; });
	}
	

	$scope.selectItem = function (e, listItem) {
		if ($scope.selectedItem == listItem)
			return;
		$scope.selectedItem = listItem;
		//call callback function
		if ($scope.blade.options.selectItemFn) {
			$scope.blade.options.selectItemFn(listItem);
		};

		var newBlade = {
			id: 'CatalogItemsSelect',
			breadcrumbs: $scope.blade.breadcrumbs,
			catalogId: $scope.blade.catalogId,
			catalog: $scope.blade.catalog,
			controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
			options: $scope.blade.options,
			bladeToolbarCommands: $scope.blade.bladeToolbarCommands
		};


		if ($scope.isCatalogSelectMode()) {
			newBlade.catalogId = listItem.id;
			newBlade.catalog = listItem;
			bladeNavigationService.closeBlade($scope.blade, function () {
				bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
			});
		}
		else if (listItem.type === 'category') {
			newBlade.categoryId = listItem.id;
			newBlade.category = listItem;

			bladeNavigationService.closeBlade($scope.blade, function () {
				bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
			});
		}
		else { 
			$scope.blade.showItemBlade(listItem.id, listItem.name);
		}

	};

	$scope.checkItem = function (listItem) {
		if ($scope.blade.options.checkItemFn) {
			$scope.blade.options.checkItemFn(listItem, listItem.selected);
		};
	}

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	//No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
	//$scope.blade.refresh();
}]);
