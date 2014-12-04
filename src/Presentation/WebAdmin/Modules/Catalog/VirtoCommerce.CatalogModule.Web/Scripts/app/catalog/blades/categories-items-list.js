angular.module('catalogModule.blades.categoriesItemsList', [
    'catalogModule.blades.categoriesItemsAdd',
    'catalogModule.blades.advancedSearch',
    'catalogModule.resources.categories',
    'catalogModule.resources.items',
    'catalogModule.resources.itemsSearch',
    'platformWebApp.common.confirmDialog'
])
.controller('categoriesItemsListController', ['$rootScope', '$scope', '$filter', 'categories', 'items', 'itemsSearch', 'bladeNavigationService', 'dialogService', function ($rootScope, $scope, $filter, categories, items, itemsSearch, bladeNavigationService, dialogService) {
    //pagination settigs
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = { searchKeyword: undefined };

    $scope.selectedAll = false;
    $scope.selectedItem = null;
    var preventCategoryListingOnce; // prevent from unwanted additional actions after command was activated from context menu

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        itemsSearch.listitemssearch(
            {
                catalogId: $scope.blade.catalogId,
                categoryId: $scope.blade.categoryId,
                keyword: $scope.filter.searchKeyword,
                // propertyValues: ,
                responseGroup: 'withCategories, withItems',
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            },
		function (data, headers) {
		    $scope.blade.isLoading = false;
		    $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
		    $scope.items = data.listEntries;
		    $scope.selectedAll = false;

		    if ($scope.selectedItem != null) {
		        $scope.selectedItem = $scope.findItem($scope.selectedItem.id);
		    }

		    //Set navigation breadcrumbs
		    setBreadcrumps();
		});
    }

    //Breadcrumps
    function setBreadcrumps() {
        //Clone array (angular.copy leave a same reference)
        $scope.blade.breadcrumbs = $scope.blade.breadcrumbs.slice(0);

        //catalog breadcrump by default
        var breadCrumb = {
            id: $scope.blade.catalogId,
            name: $scope.blade.catalog.name,
            blade: $scope.blade
        };

        //if category need change to category breadcrumb
        if (angular.isDefined($scope.blade.category)) {

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
						    bladeNavigationService.showBlade($scope.blade, $scope.blade.parentBlade);
						    $scope.blade.refresh();
						});
        };
    }

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        $scope.blade.refresh();
    });

    $scope.edit = function (listItem) {
        if (listItem.type === 'category') {
            preventCategoryListingOnce = true;
        }
        edit(listItem);
    };

    function edit(listItem) {
        closeChildrenBlades();

        $scope.selectedItem = listItem;
        if (listItem.type === 'category') {
            $scope.blade.showCategoryBlade(listItem.id, null, listItem.name);
        }
        // else do nothing as item is opened on selecting it.
    };

    $scope.blade.showCategoryBlade = function (id, data, title) {
        var newBlade = {
            id: "listCategoryDetail",
            currentEntityId: id,
            currentEntity: data,
            title: title,
            style: 'gray',
            subtitle: 'category details',
            controller: 'categoryDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/category-detail.tpl.html',
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.showItemBlade = function (id, title) {
        var newBlade = {
            id: "listItemDetail",
            itemId: id,
            title: title,
            style: 'gray',
            subtitle: 'Item details',
            controller: 'itemDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.showNewItemWizard = function (inMemoryItem) {
        var newBlade = {
            id: "newProductWizard",
            item: inMemoryItem,
            title: "New product",
            subtitle: 'Fill all product information',
            controller: 'newProductWizardController',
            bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-actions.tpl.html',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.blade.showNewVariationWizard = function (inMemoryItem) {
        var newBlade = {
            id: "newVariationWizard",
            item: inMemoryItem,
            title: "New variation",
            subtitle: 'Fill all variation information',
            controller: 'newProductWizardController',
            bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-actions.tpl.html',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-variation-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "Message",
                message: "Nothing selected. Check some Categories or Items first."
            };
            dialogService.showNotificationDialog(dialog);
        }

        preventCategoryListingOnce = true;
    };

    function isItemsChecked() {
        if ($scope.items) {
            return $filter('filter')($scope.items, { selected: true }, true).length > 0;
        } else {
            return false;
        }
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Categories or Items?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = $filter('filter')($scope.items, { selected: true }, true);

                    var listEntryLinks = [];
                    var categoryIds = [];
                    var itemIds = [];
                    angular.forEach(selection, function (listItem) {
                        var deletingLink = false;

                        if (listItem.type === 'category') {
                            if ($scope.blade.catalog.virtual && _.some(listItem.links, function (x) { return x.categoryId === $scope.blade.categoryId; })) {
                                deletingLink = true;
                            } else {
                                categoryIds.push(listItem.id);
                            }
                        } else {
                            if ($scope.blade.catalog.virtual) {
                                deletingLink = true;
                            } else {
                                itemIds.push(listItem.id);
                            }
                        }

                        if (deletingLink)
                            listEntryLinks.push({
                                listEntryId: listItem.id,
                                listEntryType: listItem.type,
                                catalogId: $scope.blade.catalogId,
                                categoryId: $scope.blade.categoryId,
                            });
                    });

                    if (listEntryLinks.length > 0) {
                        itemsSearch.deletelinks(listEntryLinks, function (data, headers) {
                            $scope.blade.refresh();
                            if ($scope.blade.mode === 'mappingSource')
                                $scope.blade.parentBlade.refresh();
                        });
                    }
                    if (categoryIds.length > 0) {
                        categories.remove({}, categoryIds, function (data, headers) {
                            $scope.blade.refresh();
                        });
                    }
                    if (itemIds.length > 0) {
                        items.remove({ ids: itemIds }, function (data, headers) {
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function mapChecked() {
        //var dialog = {
        //    id: "confirmDeleteItem",
        //    title: "Map confirmation",
        //    message: "Are you sure you want to map selected Categories or Items?",
        //    callback: function (confirmed) {
        //        if (confirmed) {
        // $scope.blade.parentBlade.catalog.virtual....
        closeChildrenBlades();

        var selection = $filter('filter')($scope.items, { selected: true }, true);
        var listEntryLinks = [];
        angular.forEach(selection, function (listItem) {
            listEntryLinks.push({
                listEntryId: listItem.id,
                listEntryType: listItem.type,
                catalogId: $scope.blade.parentBlade.catalogId,
                categoryId: $scope.blade.parentBlade.categoryId,
            });
        });

        itemsSearch.createlinks(listEntryLinks, function () {
            $scope.blade.refresh();
            $scope.blade.parentBlade.refresh();
        });
        //}
        //    }
        //}
        //dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.setSelectedItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    $scope.selectItem = function (e, listItem) {
        $scope.blade.setSelectedItem(listItem);

        if (listItem.type === 'category') {
            if (preventCategoryListingOnce) {
                preventCategoryListingOnce = false;
            } else {
                var newBlade = {
                    id: 'itemsList' + ($scope.blade.level + (e.ctrlKey ? 1 : 0)),
                    level: $scope.blade.level + (e.ctrlKey ? 1 : 0),
                    mode: $scope.blade.mode,
                    breadcrumbs: $scope.blade.breadcrumbs,
                    title: 'Categories & Items',
                    subtitle: 'Browsing "' + listItem.name + '"',
                    catalogId: $scope.blade.catalogId,
                    catalog: $scope.blade.catalog,
                    categoryId: listItem.id,
                    category: listItem,
                    controller: 'categoriesItemsListController',
                    template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/categories-items-list.tpl.html'
                };

                if (e.ctrlKey) {
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                }
                else {
                    bladeNavigationService.closeBlade($scope.blade, function () {
                        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                    });
                }
            }
        } else {
            $scope.blade.showItemBlade(listItem.id, listItem.name);
        }

        $scope.blade.currentItemId = $scope.selectedItem.type === 'product' ? $scope.selectedItem.id : undefined;
    };


    $scope.findItem = function (id) {
        var retVal;
        angular.forEach($scope.items, function (item) {
            if (item.id == id)
                retVal = item;
        });

        return retVal;
    }

    $scope.hasLinks = function (listEntry) {
        return $scope.blade.catalog.virtual && listEntry.links && (listEntry.type === 'category' ? listEntry.links.length > 0 : listEntry.links.length > 1);
        // return listEntry.links && listEntry.links.length > 0;
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

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh", icon: 'icon-spin',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
          {
              name: "Add", icon: 'icon-plus',
              executeMethod: function () {
                  closeChildrenBlades();

                  var newBlade = {
                      id: 'listItemChild',
                      title: 'New category item',
                      subtitle: 'choose new item type',
                      controller: 'categoriesItemsAddController',
                      template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/categories-items-add.tpl.html'
                  };
                  bladeNavigationService.showBlade(newBlade, $scope.blade);
              },
              canExecuteMethod: function () {
                  return true;
              }
          },
            {
                name: "Manage", icon: 'icon-new-tab-2',
                executeMethod: function () {
                    // selected OR the first checked listItem
                    edit($scope.selectedItem || _.find($scope.items, function (x) { return x.selected; }));
                },
                canExecuteMethod: function () {
                    return $scope.selectedItem || isItemsChecked();
                }
            },
            {
                name: "Delete", icon: 'icon-remove',
                executeMethod: function () {
                    deleteChecked();
                },
                canExecuteMethod: function () {
                    return isItemsChecked();
                }
            },
            {
                name: "Advanced search", icon: 'icon-search',
                executeMethod: function () {
                    var newBlade = {
                        id: 'listItemChild',
                        title: 'Advanced search',
                        subtitle: 'Searching within...',
                        controller: 'advancedSearchController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/advanced-search.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                    $scope.bladeClose();
                },
                canExecuteMethod: function () {
                    return true;
                }
            }
    ];

    // mappingSource
    if (angular.isDefined($scope.blade.mode) && $scope.blade.mode === 'mappingSource') {
        var mapCommand = {
            name: "Map", icon: 'icon-link',
            executeMethod: function () {
                mapChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            }
        }

        $scope.bladeToolbarCommands.splice(1, 2, mapCommand);
    }

    $scope.checkAll = function (selected) {
        angular.forEach($scope.items, function (item) {
            item.selected = selected;
        });
    };

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //$scope.blade.refresh();
}]);
