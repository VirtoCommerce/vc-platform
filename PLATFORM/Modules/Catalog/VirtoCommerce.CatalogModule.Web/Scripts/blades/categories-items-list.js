﻿angular.module('virtoCommerce.catalogModule')
    .controller('virtoCommerce.catalogModule.categoriesItemsListController', [
        '$sessionStorage', '$scope', '$filter', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.listEntries', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.authService', function ($storage, $scope, $filter, categories, items, listEntries, bladeNavigationService, dialogService, authService) {
            //pagination settings
            $scope.pageSettings = {};
            $scope.pageSettings.totalItems = 0;
            $scope.pageSettings.currentPage = 1;
            $scope.pageSettings.numPages = 5;
            $scope.pageSettings.itemsPerPageCount = 20;

            $scope.filter = { searchKeyword: undefined };

            $scope.selectedAll = false;
            $scope.selectedItem = null;
            var preventCategoryListingOnce; // prevent from unwanted additional actions after command was activated from context menu

            var blade = $scope.blade;

            blade.refresh = function () {
                blade.isLoading = true;
                listEntries.listitemssearch(
                    {
                        catalog: blade.catalogId,
                        category: blade.categoryId,
                        q: $scope.filter.searchKeyword,
                        // propertyValues: ,
                        respGroup: 'withCategories, withProducts',
                        start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        count: $scope.pageSettings.itemsPerPageCount
                    },
                    function (data, headers) {
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
                        $scope.items = data.listEntries;
                        $scope.selectedAll = false;

                        if ($scope.selectedItem != null) {
                            $scope.selectedItem = $scope.findItem($scope.selectedItem.id);
                        }

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

            $scope.$watch('pageSettings.currentPage', function (newPage) {
                blade.refresh();
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
                    blade.showCategoryBlade(listItem);
                }
                // else do nothing as item is opened on selecting it.
            };

            blade.showCategoryBlade = function (listItem) {
                var newBlade = {
                    id: "listCategoryDetail",
                    currentEntityId: listItem.id,
                    title: listItem.name,
                    subtitle: 'catalog.blades.category-details.subtitle',
                    controller: 'virtoCommerce.catalogModule.categoryDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/category-detail.tpl.html',
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.delete = function () {
                if (isItemsChecked()) {
                    deleteChecked();
                } else {
                    var dialog = {
                        id: "notifyNoTargetCategory",
                        title: "catalog.dialogs.nothing-selected.title",
                        message: "catalog.dialogs.nothing-selected.message"
                    };
                    dialogService.showNotificationDialog(dialog);
                }

                preventCategoryListingOnce = true;
            };

            function isItemsChecked() {
                return $scope.items && _.any($scope.items, function (x) { return x.selected; });
            }

            function deleteChecked() {
                var selection = $filter('filter')($scope.items, { selected: true }, true);

                var listEntryLinks = [];
                var categoryIds = [];
                var itemIds = [];
                angular.forEach(selection, function (listItem) {
                    var deletingLink = false;

                    if (listItem.type === 'category') {
                        if (blade.catalog.virtual && _.some(listItem.links, function (x) { return x.categoryId === blade.categoryId; })) {
                            deletingLink = true;
                        } else {
                            categoryIds.push(listItem.id);
                        }
                    } else {
                        if (blade.catalog.virtual) {
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
                            closeChildrenBlades();
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
                //var dialog = {
                //    id: "confirmDeleteItem",
                //    title: "Map confirmation",
                //    message: "Are you sure you want to map selected Categories or Items?",
                //    callback: function (confirmed) {
                //        if (confirmed) {
                // blade.parentBlade.catalog.virtual....
                closeChildrenBlades();

                var selection = _.where($scope.items, { selected: true });
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
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                //}
                //    }
                //}
                //dialogService.showConfirmationDialog(dialog);
            }

            blade.setSelectedItem = function (listItem) {
                $scope.selectedItem = listItem;
            };

            $scope.selectItem = function (e, listItem) {
                blade.setSelectedItem(listItem);
                var newBlade;
                if (listItem.type === 'category') {
                    if (preventCategoryListingOnce) {
                        preventCategoryListingOnce = false;
                    } else {
                        newBlade = {
                            id: 'itemsList' + (blade.level + (e.ctrlKey ? 1 : 0)),
                            level: blade.level + (e.ctrlKey ? 1 : 0),
                            mode: blade.mode,
                            isBrowsingLinkedCategory: blade.isBrowsingLinkedCategory || $scope.hasLinks(listItem),
                            breadcrumbs: blade.breadcrumbs,
                            title: 'catalog.blades.categories.title',
                            subtitle: 'catalog.blades.categories.subtitle',
                            subtitleValues: listItem.name != null ? {name: listItem.name} : '',
                            catalogId: blade.catalogId,
                            catalog: blade.catalog,
                            categoryId: listItem.id,
                            category: listItem,
                            disableOpenAnimation: true,
                            controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
                        };

                        if (e.ctrlKey) {
                            bladeNavigationService.showBlade(newBlade, blade);
                        } else {
                            bladeNavigationService.closeBlade(blade, function () {
                                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                            });
                        }
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

                blade.currentItemId = $scope.selectedItem.type === 'product' ? $scope.selectedItem.id : undefined;
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
                return blade.catalog.virtual && listEntry.links && (listEntry.type === 'category' ? listEntry.links.length > 0 : listEntry.links.length > 1);
            }

            blade.onClose = function (closeCallback) {
                closeChildrenBlades();
                closeCallback();
            };

            function closeChildrenBlades() {
                angular.forEach(blade.childrenBlades.slice(), function (child) {
                    bladeNavigationService.closeBlade(child);
                });
            }

            $scope.blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh",
                    icon: 'fa fa-refresh',
                    executeMethod: function () {
                        blade.refresh();
                    },
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "platform.commands.manage",
                    icon: 'fa fa-edit',
                    executeMethod: function () {
                        // selected OR the first checked listItem
                        edit($scope.selectedItem || _.find($scope.items, function (x) { return x.selected; }));
                    },
                    canExecuteMethod: function () {
                        return $scope.selectedItem || isItemsChecked();
                    },
                    permission: 'catalog:read'
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        deleteChecked();
                    },
                    canExecuteMethod: isItemsChecked,
                    permission: 'catalog:delete'
                },
    			{
    			    name: "platform.commands.import",
    			    icon: 'fa fa-download',
    			    executeMethod: function () {
    			        var newBlade = {
    			            id: 'catalogImport',
    			            title: 'catalog.blades.catalog-import.title',
    			            subtitle: 'catalog.blades.catalog-import.subtitle',
    			            catalog: blade.catalog,
    			            controller: 'virtoCommerce.catalogModule.importerListController',
    			            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/importers-list.tpl.html'
    			        };
    			        bladeNavigationService.showBlade(newBlade, $scope.blade);
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
				            title: 'catalog.blades.catalog-export.title',
				            subtitle: 'catalog.blades.catalog-export.subtitle',
				            catalog: blade.catalog,
				            controller: 'virtoCommerce.catalogModule.exporterListController',
				            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/export/exporter-list.tpl.html',
				            selectedProducts: _.filter($scope.items, function (x) { return x.type == 'product' && x.selected == true; }),
				            selectedCategories: _.filter($scope.items, function (x) { return x.type == 'category' && x.selected == true; })
				        };
				        bladeNavigationService.showBlade(newBlade, $scope.blade);
				    },
				    canExecuteMethod: function () { return true; },
				    permission: 'catalog:export'
				},
                 {
                     name: "platform.commands.cut",
                     icon: 'fa fa-cut',
                     executeMethod: function () {
                         $storage.catalogClipboardContent = _.where($scope.items, { selected: true });
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
                             listEntries: $storage.catalogClipboardContent
                         }, function () {
                             delete $storage.catalogClipboardContent;
                             blade.refresh();
                         }, function (error) {
                             bladeNavigationService.setError('Error ' + error.status, blade);
                         });
                     },
                     canExecuteMethod: function () {
                         return $storage.catalogClipboardContent;
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
                $scope.blade.toolbarCommands.splice(2, 5);
            }

            if (angular.isDefined(blade.mode)) {
                // mappingSource
                if (blade.mode === 'mappingSource') {
                    var mapCommand = {
                        name: "Map",
                        icon: 'fa fa-link',
                        executeMethod: function () {
                            mapChecked();
                        },
                        canExecuteMethod: isItemsChecked
                    }
                    $scope.blade.toolbarCommands.splice(1, 6, mapCommand);
                }
            } else if (authService.checkPermission('catalog:create')) {
                $scope.blade.toolbarCommands.splice(1, 0, {
                    name: "platform.commands.add",
                    icon: 'fa fa-plus',
                    executeMethod: function () {
                        closeChildrenBlades();

                        var newBlade = {
                            id: 'listItemChild',
                            title: 'catalog.blades.new-category-type.title',
                            subtitle: 'catalog.blades.new-category-type.subtitle',
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
                    title: 'catalog.blades.mapping.title',
                    subtitle: 'catalog.blades.mapping.subtitle',
                    catalogId: selectedNode.id,
                    catalog: selectedNode,
                    disableOpenAnimation: true,
                    controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.checkAll = function (selected) {
                angular.forEach($scope.items, function (item) {
                    item.selected = selected;
                });
            };
            
            
            //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
            //blade.refresh();
        }]);
