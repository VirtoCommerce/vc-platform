angular.module('virtoCommerce.catalogModule')
    .controller('virtoCommerce.catalogModule.categoriesItemsListController', [
        '$sessionStorage', '$localStorage', '$timeout', '$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeUtils', 'platformWebApp.dialogService', 'platformWebApp.authService', 'platformWebApp.uiGridHelper',
        function ($sessionStorage, $localStorage, $timeout, $scope, categories, items, listEntries, bladeUtils, dialogService, authService, uiGridHelper) {
            $scope.uiGridConstants = uiGridHelper.uiGridConstants;

            var blade = $scope.blade;
            var bladeNavigationService = bladeUtils.bladeNavigationService;

            blade.refresh = function () {
                blade.isLoading = true;
                var searchCriteria = {
                    catalogId: blade.catalogId,
                    categoryId: blade.categoryId,
                    keyword: filter.keyword ? filter.keyword : undefined,
                    responseGroup: 'withCategories, withProducts',
                    sort: uiGridHelper.getSortExpression($scope),
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                };
                if (filter.current) {
                    angular.extend(searchCriteria, filter.current);
                }

                listEntries.listitemssearch(
                    searchCriteria,
                    function (data) {
                        transformByFilters(data.listEntries);

                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = data.totalCount;
                        $scope.items = data.listEntries;

                        //Set navigation breadcrumbs
                        setBreadcrumbs();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
            }

            //Breadcrumbs
            function setBreadcrumbs() {
                //Clone array (angular.copy leave a same reference)
                blade.breadcrumbs = blade.breadcrumbs.slice(0);

                //catalog breadcrumb by default
                var breadCrumb = {
                    id: blade.catalogId,
                    name: blade.catalog.name,
                    blade: blade
                };

                //if category need change to category breadcrumb
                if (angular.isDefined(blade.category)) {

                    breadCrumb.id = blade.categoryId;
                    breadCrumb.name = blade.category.name;
                }

                //prevent duplicate items
                if (!_.some(blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
                    blade.breadcrumbs.push(breadCrumb);
                }

                breadCrumb.navigate = function (breadcrumb) {
                    bladeNavigationService.closeBlade(blade,
                        function () {
                            blade.disableOpenAnimation = true;
                            bladeNavigationService.showBlade(blade, blade.parentBlade);
                            blade.refresh();
                        });
                };
            }

            $scope.edit = function (listItem) {
                if (listItem.type === 'category') {
                    blade.setSelectedItem(listItem);
                    blade.showCategoryBlade(listItem);
                } else
                    $scope.selectItem(null, listItem);
            };

            blade.showCategoryBlade = function (listItem) {
                var newBlade = {
                    id: "listCategoryDetail",
                    currentEntityId: listItem.id,
                    title: listItem.name,
                    subtitle: 'catalog.blades.category-detail.subtitle',
                    controller: 'virtoCommerce.catalogModule.categoryDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/category-detail.tpl.html',
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.cut = function (data) {
                cutList([data]);
            }

            function cutList(selection) {
                $sessionStorage.catalogClipboardContent = selection;
            }

            $scope.delete = function (data) {
                deleteList([data]);
            };

            function isItemsChecked() {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            }

            function deleteList(selection) {
                var listEntryLinks = [];
                var categoryIds = [];
                var itemIds = [];
                angular.forEach(selection, function (listItem) {
                    var deletingLink = false;

                    if (listItem.type === 'category') {
                        if (blade.catalog.isVirtual && _.some(listItem.links, function (x) { return x.categoryId === blade.categoryId; })) {
                            deletingLink = true;
                        } else {
                            categoryIds.push(listItem.id);
                        }
                    } else {
                        if (blade.catalog.isVirtual) {
                            deletingLink = true;
                        } else {
                            itemIds.push(listItem.id);
                        }
                    }

                    if (deletingLink)
                        listEntryLinks.push({
                            listEntryId: listItem.id,
                            listEntryType: listItem.type,
                            catalogId: blade.catalogId,
                            categoryId: blade.categoryId,
                        });
                });

                var listCategoryLinkCount = _.where(listEntryLinks, { listEntryType: 'category' }).length;
                var dialog = {
                    id: "confirmDeleteItem",
                    categoryCount: categoryIds.length,
                    itemCount: itemIds.length,
                    listCategoryLinkCount: listCategoryLinkCount,
                    listItemLinkCount: listEntryLinks.length - listCategoryLinkCount,
                    callback: function (remove) {
                        if (remove) {
                            bladeNavigationService.closeChildrenBlades(blade);
                            blade.isLoading = true;
                            if (listEntryLinks.length > 0) {
                                listEntries.deletelinks(listEntryLinks, function (data, headers) {
                                    blade.refresh();
                                    if (blade.mode === 'mappingSource')
                                        blade.parentBlade.refresh();
                                }, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                            }
                            if (categoryIds.length > 0) {
                                categories.remove({ ids: categoryIds }, function (data, headers) {
                                    blade.refresh();
                                }, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                            }
                            if (itemIds.length > 0) {
                                items.remove({ ids: itemIds }, function (data, headers) {
                                    blade.refresh();
                                }, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                            }
                        }
                    }
                }
                dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Catalog)/Scripts/dialogs/deleteCategoryItem-dialog.tpl.html', 'platformWebApp.confirmDialogController');
            }

            function mapChecked() {
                bladeNavigationService.closeChildrenBlades(blade);

                var selection = $scope.gridApi.selection.getSelectedRows();
                var listEntryLinks = [];
                angular.forEach(selection, function (listItem) {
                    listEntryLinks.push({
                        listEntryId: listItem.id,
                        listEntryType: listItem.type,
                        catalogId: blade.parentBlade.catalogId,
                        categoryId: blade.parentBlade.categoryId,
                    });
                });

                listEntries.createlinks(listEntryLinks, function () {
                    blade.refresh();
                    blade.parentBlade.refresh();
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }

            blade.setSelectedItem = function (listItem) {
                $scope.selectedNodeId = listItem.id;
            };

            $scope.selectItem = function (e, listItem) {
                blade.setSelectedItem(listItem);
                var newBlade;
                if (listItem.type === 'category') {
                    var openNewBlade = e.ctrlKey || filter.keyword || filter.current;
                    newBlade = {
                        id: 'itemsList' + (blade.level + (openNewBlade ? 1 : 0)),
                        level: blade.level + (openNewBlade ? 1 : 0),
                        mode: blade.mode,
                        isBrowsingLinkedCategory: blade.isBrowsingLinkedCategory || $scope.hasLinks(listItem),
                        breadcrumbs: blade.breadcrumbs,
                        title: 'catalog.blades.categories-items-list.title',
                        subtitle: 'catalog.blades.categories-items-list.subtitle',
                        subtitleValues: listItem.name != null ? { name: listItem.name } : '',
                        catalogId: blade.catalogId,
                        catalog: blade.catalog,
                        categoryId: listItem.id,
                        category: listItem,
                        disableOpenAnimation: true,
                        controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
                    };

                    if (openNewBlade) {
                        bladeNavigationService.showBlade(newBlade, blade);
                    } else {
                        bladeNavigationService.closeBlade(blade, function () {
                            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                        });
                    }
                } else {
                    newBlade = {
                        id: "listItemDetail" + blade.mode,
                        itemId: listItem.id,
                        productType: listItem.productType,
                        title: listItem.name,
                        controller: 'virtoCommerce.catalogModule.itemDetailController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                }
            };

            $scope.selectGroupByItem = function (listEntry, $id) {
                $scope.selectedNodeId = $id;
                var listItem = {
                    id: listEntry.outline.slice(-1)[0],
                    name: listEntry.path.slice(-1)[0]
                };

                newBlade = {
                    id: 'itemsList' + (blade.level + 1),
                    level: blade.level + 1,
                    mode: blade.mode,
                    // isBrowsingLinkedCategory: blade.isBrowsingLinkedCategory || $scope.hasLinks(listItem),
                    title: 'catalog.blades.categories-items-list.title',
                    // subtitle: 'catalog.blades.categories-items-list.subtitle',
                    // subtitleValues: listItem.name ? { name: listItem.name } : '',
                    catalogId: blade.catalogId,
                    catalog: blade.catalog,
                    categoryId: listItem.id,
                    category: listItem,
                    controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
                };

                newBlade.breadcrumbs = generateBreadcrumbs(newBlade, listEntry, listEntry.outline.length);
                bladeNavigationService.showBlade(newBlade, blade);
            };

            function generateBreadcrumbs(newBlade, listEntry, count) {
                var newBreadcrumbs = [{
                    id: blade.catalogId,
                    name: blade.catalog.name,
                    navigate: function (breadcrumb) {
                        bladeNavigationService.closeBlade(newBlade,
                            function () {
                                newBlade.disableOpenAnimation = true;
                                newBlade.categoryId = undefined;
                                newBlade.category = undefined;
                                newBlade.breadcrumbs = [];
                                bladeNavigationService.showBlade(newBlade, blade);
                                newBlade.refresh();
                            });
                    }
                }];

                for (var i = 0; i < count; i++) {
                    newBreadcrumbs.push({
                        id: listEntry.outline[i],
                        name: listEntry.path[i],
                        navigate: function (breadcrumb) {
                            bladeNavigationService.closeBlade(newBlade,
                                function () {
                                    newBlade.disableOpenAnimation = true;
                                    newBlade.categoryId = breadcrumb.id;
                                    newBlade.category = breadcrumb;
                                    newBlade.breadcrumbs = generateBreadcrumbs(newBlade, listEntry, i - 1);
                                    bladeNavigationService.showBlade(newBlade, blade);
                                    newBlade.refresh();
                                });
                        }
                    })
                }
                return newBreadcrumbs;
            }

            $scope.hasLinks = function (listEntry) {
                return blade.catalog.isVirtual && listEntry.links && (listEntry.type === 'category' ? listEntry.links.length > 0 : listEntry.links.length > 1);
            }

            blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh",
                    icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fa fa-trash-o',
                    executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: isItemsChecked,
                    permission: 'catalog:delete'
                },
    			{
    			    name: "platform.commands.import",
    			    icon: 'fa fa-download',
    			    executeMethod: function () {
    			        var newBlade = {
    			            id: 'catalogImport',
    			            title: 'catalog.blades.importers-list.title',
    			            subtitle: 'catalog.blades.importers-list.subtitle',
    			            catalog: blade.catalog,
    			            controller: 'virtoCommerce.catalogModule.importerListController',
    			            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/importers-list.tpl.html'
    			        };
    			        bladeNavigationService.showBlade(newBlade, blade);
    			    },
    			    canExecuteMethod: function () { return true; },
    			    permission: 'catalog:import'
    			},
				{
				    name: "platform.commands.export",
				    icon: 'fa fa-upload',
				    executeMethod: function () {
				        var newBlade = {
				            id: 'catalogExport',
				            title: 'catalog.blades.exporter-list.title',
				            subtitle: 'catalog.blades.exporter-list.subtitle',
				            catalog: blade.catalog,
				            controller: 'virtoCommerce.catalogModule.exporterListController',
				            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/exporter-list.tpl.html',
				            selectedProducts: _.filter($scope.gridApi.selection.getSelectedRows(), function (x) { return x.type == 'product' }),
				            selectedCategories: _.filter($scope.gridApi.selection.getSelectedRows(), function (x) { return x.type == 'category' })
				        };
				        bladeNavigationService.showBlade(newBlade, blade);
				    },
				    canExecuteMethod: function () { return true; },
				    permission: 'catalog:export'
				},
                 {
                     name: "platform.commands.cut",
                     icon: 'fa fa-cut',
                     executeMethod: function () {
                         cutList($scope.gridApi.selection.getSelectedRows());
                     },
                     canExecuteMethod: isItemsChecked,
                     permission: 'catalog:create'
                 },
                 {
                     name: "platform.commands.paste",
                     icon: 'fa fa-clipboard',
                     executeMethod: function () {
                         blade.isLoading = true;
                         listEntries.move({
                             catalog: blade.catalogId,
                             category: blade.categoryId,
                             listEntries: $sessionStorage.catalogClipboardContent
                         }, function () {
                             delete $sessionStorage.catalogClipboardContent;
                             blade.refresh();
                         }, function (error) {
                             bladeNavigationService.setError('Error ' + error.status, blade);
                         });
                     },
                     canExecuteMethod: function () {
                         return $sessionStorage.catalogClipboardContent;
                     },
                     permission: 'catalog:create'
                 }

                //{
                //    name: "Advanced search", icon: 'fa fa-search',
                //    executeMethod: function () {
                //        var newBlade = {
                //            id: 'listItemChild',
                //            title: 'Advanced search',
                //            subtitle: 'Searching within...',
                //            controller: 'virtoCommerce.catalogModule.advancedSearchController',
                //            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/advanced-search.tpl.html'
                //        };
                //        bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                //    },
                //    canExecuteMethod: function () {
                //        return true;
                //    }
                //}
            ];

            if (blade.isBrowsingLinkedCategory) {
                blade.toolbarCommands.splice(1, 5);
            }

            if (angular.isDefined(blade.mode)) {
                // mappingSource
                if (blade.mode === 'mappingSource') {
                    var mapCommand = {
                        name: "catalog.commands.map",
                        icon: 'fa fa-link',
                        executeMethod: function () {
                            mapChecked();
                        },
                        canExecuteMethod: isItemsChecked
                    }
                    blade.toolbarCommands.splice(1, 5, mapCommand);
                }
            } else if (authService.checkPermission('catalog:create')) {
                blade.toolbarCommands.splice(1, 0, {
                    name: "platform.commands.add",
                    icon: 'fa fa-plus',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'listItemChild',
                            title: 'catalog.blades.categories-items-add.title',
                            subtitle: 'catalog.blades.categories-items-add.subtitle',
                            controller: 'virtoCommerce.catalogModule.categoriesItemsAddController',
                            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-add.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    },
                    canExecuteMethod: function () {
                        return true;
                    }
                });
            }

            blade.onAfterCatalogSelected = function (selectedNode) {
                var newBlade = {
                    id: 'itemsList' + (blade.level + 1),
                    level: blade.level + 1,
                    mode: 'mappingSource',
                    breadcrumbs: [],
                    title: 'catalog.blades.categories-items-list.title-mapping',
                    subtitle: 'catalog.blades.categories-items-list.subtitle-mapping',
                    catalogId: selectedNode.id,
                    catalog: selectedNode,
                    disableOpenAnimation: true,
                    controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };


            // simple and advanced filtering
            //var groupingColumn;
            var filter = blade.filter = $scope.filter = {};
            $scope.$localStorage = $localStorage;
            if (!$localStorage.catalogSearchFilters) {
                $localStorage.catalogSearchFilters = [{ name: 'catalog.blades.categories-items-list.labels.new-filter' }]
            }
            if ($localStorage.catalogSearchFilterId) {
                filter.current = _.findWhere($localStorage.catalogSearchFilters, { id: $localStorage.catalogSearchFilterId });
            }

            filter.change = function () {
                $localStorage.catalogSearchFilterId = filter.current ? filter.current.id : null;
                if (filter.current && !filter.current.id) {
                    filter.current = null;
                    showFilterDetailBlade({ isNew: true });
                } else {
                    bladeNavigationService.closeBlade({ id: 'filterDetail' });
                    filter.criteriaChanged();
                }
            };

            filter.criteriaChanged = function () {
                if ($scope.pageSettings.currentPage > 1) {
                    $scope.pageSettings.currentPage = 1;
                } else {
                    blade.refresh();
                }
            };

            filter.edit = function () {
                if (filter.current) {
                    showFilterDetailBlade({ data: filter.current });
                }
            };

            function showFilterDetailBlade(bladeData) {
                var newBlade = {
                    id: 'filterDetail',
                    controller: 'virtoCommerce.catalogModule.filterDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/filter-detail.tpl.html',
                };
                angular.extend(newBlade, bladeData);
                bladeNavigationService.showBlade(newBlade, blade);
            };

            function transformByFilters(data) {
                if (_.any(data)) {
                    _.each(data, function (x) {
                        x.$path = _.any(x.path) ? x.path.join(" \\ ") : '\\';
                    });

                    if ($scope.gridApi) {
                        if (filter.keyword || filter.current) {
                            //groupingColumn.visible = true;
                            if (!_.any($scope.gridApi.grouping.getGrouping().grouping)) {
                                $scope.gridApi.grouping.groupColumn('$path');
                            }

                            $timeout($scope.gridApi.treeBase.expandAllRows);
                        } else {
                            //groupingColumn.visible = false;
                            $scope.gridApi.grouping.clearGrouping();
                        }
                    }
                } else
                    $scope.gridApi = undefined;
            }


            // ui-grid
            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    //groupingColumn = _.findWhere($scope.gridOptions.columnDefs, { name: '$path' });

                    if (filter.keyword || filter.current) {
                        $timeout(function () {
                            gridApi.grouping.groupColumn('$path');
                            $timeout(gridApi.treeBase.expandAllRows);
                        });
                    }

                    $timeout(function () {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    }, 200);
                });
                bladeUtils.initializePagination($scope);
            };

            $scope.getGroupInfo = function (groupEntity) {
                return _.values(groupEntity)[0];
            };


            //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
            //blade.refresh();
        }]);
