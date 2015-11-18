angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogItemSelectController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeNavigationService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, catalogs, listEntries, bladeNavigationService, uiGridConstants, uiGridHelper) {

        $scope.blade.title = "Catalog items selection...";

        //pagination settings
        $scope.pageSettings = {};
        $scope.pageSettings.totalItems = 0;
        $scope.pageSettings.currentPage = 1;
        $scope.pageSettings.numPages = 5;
        $scope.pageSettings.itemsPerPageCount = 20;

        $scope.filter = { searchKeyword: undefined };

        $scope.options = angular.extend({
            showCheckingMultiple: true,
            allowCheckingItem: true,
            allowCheckingCategory: false,
            selectedItemIds: []
        }, $scope.blade.options);

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
                    uiGridHelper.onDataLoaded($scope.gridOptions, $scope.items);

                    //check already selected elements
                    if ($scope.options.selectedItemIds) {
                        _.each($scope.items, function (x) {
                            x.selected = _.some($scope.options.selectedItemIds, function (y) { return y == x.id; })
                        });
                    }
                    //Set navigation breadcrumbs
                    setBreadcrumbs();

                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                });
            }
            else {
                catalogs.getCatalogs({}, function (results) {
                    $scope.blade.isLoading = false;

                    $scope.items = results;
                    //Set navigation breadcrumbs
                    setBreadcrumbs();

                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                });
            }
        }

        //Breadcrumbs
        function setBreadcrumbs() {
            //Clone array (angular.copy leave a same reference)
            $scope.blade.breadcrumbs = $scope.blade.breadcrumbs.slice(0);

            //catalog breadcrumb by default
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

            //prevent duplicate items
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

        $scope.$watch('pageSettings.currentPage', function () {
            $scope.blade.refresh();
        });

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
                breadcrumbs: $scope.blade.breadcrumbs,
                catalogId: $scope.blade.catalogId,
                catalog: $scope.blade.catalog,
                controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
                options: $scope.options,
                toolbarCommands: $scope.blade.toolbarCommands
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
                newBlade = {
                    id: "listItemDetail",
                    itemId: listItem.id,
                    productType: listItem.productType,
                    title: listItem.name,
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }

        };

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            gridOptions.isRowSelectable = function (row) {
                return ($scope.options.allowCheckingItem && row.entity.type !== 'category') || ($scope.options.allowCheckingCategory && row.entity.type === 'category');
            };

            uiGridHelper.initialize($scope, gridOptions,
            function (gridApi) {
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if ($scope.options.checkItemFn) {
                        $scope.options.checkItemFn(row.entity, row.isSelected);
                    };
                    if (row.isSelected) {
                        $scope.options.selectedItemIds.push(row.entity.id);
                    }
                    else {
                        $scope.options.selectedItemIds = _.without($scope.options.selectedItemIds, row.entity.id);
                    }
                });
            });
        };

        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //$scope.blade.refresh();
    }]);
