angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogItemSelectController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeUtils', 'uiGridConstants', 'platformWebApp.uiGridHelper', '$timeout',
function ($scope, catalogs, listEntries, bladeUtils, uiGridConstants, uiGridHelper, $timeout) {
    var blade = $scope.blade;
    var bladeNavigationService = bladeUtils.bladeNavigationService;

    if (!blade.title) {
        blade.title = "Select Catalog items...";
    }

    $scope.options = angular.extend({
        showCheckingMultiple: true,
        allowCheckingItem: true,
        allowCheckingCategory: false,
        selectedItemIds: []
    }, blade.options);

    blade.refresh = function () {
        blade.isLoading = true;

        if (!$scope.isCatalogSelectMode()) {
            listEntries.listitemssearch(
                {
                    catalogId: blade.catalogId,
                    categoryId: blade.categoryId,
                    keyword: filter.keyword,
                    // propertyValues: ,
                    responseGroup: 'withCategories, withProducts',
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                },
            function (data, headers) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                $scope.items = data.listEntries;

                //Set navigation breadcrumbs
                setBreadcrumbs();

            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
        else {
            catalogs.getCatalogs({}, function (results) {
                blade.isLoading = false;

                $scope.items = results;
                //Set navigation breadcrumbs
                setBreadcrumbs();

            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
    }

    //Breadcrumbs
    function setBreadcrumbs() {
        //Clone array (angular.copy leave a same reference)
        blade.breadcrumbs = blade.breadcrumbs.slice(0);

        //catalog breadcrumb by default
        var breadCrumb = {
            id: blade.catalogId ? blade.catalogId : "All",
            name: blade.catalog ? blade.catalog.name : "All",
            blade: $scope.blade
        };

        //if category need change to category breadcrumb
        if (blade.category) {

            breadCrumb.id = blade.categoryId;
            breadCrumb.name = blade.category.name;
        }

        //prevent duplicate items
        if (!_.some(blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
            blade.breadcrumbs.push(breadCrumb);
        }

        breadCrumb.navigate = function (breadcrumb) {
            bladeNavigationService.closeBlade($scope.blade,
                        function () {
                            if (breadcrumb.id == "All") {
                                blade.catalogId = null;
                            }
                            bladeNavigationService.showBlade($scope.blade, blade.parentBlade);
                            blade.refresh();
                        });
        };
    }

    $scope.isCatalogSelectMode = function () {
        return !blade.catalogId;
    };

    $scope.$watch('pageSettings.currentPage', blade.refresh);

    $scope.selectItem = function (e, listItem) {
        if ($scope.selectedNodeId == listItem.id)
            return;

        $scope.selectedNodeId = listItem.id;
        //call callback function
        if ($scope.options.selectItemFn) {
            $scope.options.selectItemFn(listItem);
        };

        var newBlade = {
            id: 'CatalogItemsSelect',
            breadcrumbs: blade.breadcrumbs,
            catalogId: blade.catalogId,
            catalog: blade.catalog,
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: $scope.options,
            toolbarCommands: blade.toolbarCommands
        };


        if ($scope.isCatalogSelectMode()) {
            newBlade.catalogId = listItem.id;
            newBlade.catalog = listItem;
            bladeNavigationService.closeBlade(blade, function () {
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            });
        }
        else if (listItem.type === 'category') {
            newBlade.categoryId = listItem.id;
            newBlade.category = listItem;

            bladeNavigationService.closeBlade(blade, function () {
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            });
        }
        else {
            newBlade = {
                id: "listItemDetail",
                itemId: listItem.id,
                productType: listItem.productType,
                title: listItem.name,
                variationsToolbarCommandsAndEvents: { toolbarCommands: blade.toolbarCommands, externalRegisterApiCallback: externalRegisterApiCallback },
                controller: 'virtoCommerce.catalogModule.itemDetailController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }

    };

    var filter = $scope.filter = {};
    filter.criteriaChanged = function () {
        if ($scope.pageSettings.currentPage > 1) {
            $scope.pageSettings.currentPage = 1;
        } else {
            blade.refresh();
        }
    };

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        gridOptions.isRowSelectable = function (row) {
            return ($scope.options.allowCheckingItem && row.entity.type !== 'category') || ($scope.options.allowCheckingCategory && row.entity.type === 'category');
        };

        uiGridHelper.initialize($scope, gridOptions, externalRegisterApiCallback);
    };

    bladeUtils.initializePagination($scope);

    function externalRegisterApiCallback(gridApi) {
        gridApi.grid.registerDataChangeCallback(function (grid) {
            //check already selected rows
            $timeout(function () {
                _.each($scope.items, function (x) {
                    if (_.some($scope.options.selectedItemIds, function (y) { return y == x.id; })) {
                        gridApi.selection.selectRow(x);
                    }
                });
            });
        }, [uiGridConstants.dataChange.ROW]);

        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            if ($scope.options.checkItemFn) {
                $scope.options.checkItemFn(row.entity, row.isSelected);
            };
            if (row.isSelected) {
                if (!_.contains($scope.options.selectedItemIds, row.entity.id)) {
                    $scope.options.selectedItemIds.push(row.entity.id);
                }
            }
            else {
                $scope.options.selectedItemIds = _.without($scope.options.selectedItemIds, row.entity.id);
            }
        });

        uiGridHelper.bindRefreshOnSortChanged($scope);
    }

    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);
