﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogsListController', ['$injector', '$rootScope', '$scope', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.authService',
function ($injector, $rootScope, $scope, catalogs, bladeNavigationService, dialogService, authService) {
    var blade = $scope.blade;
    var selectedNode = null;
    var preventCategoryListingOnce;

    blade.refresh = function () {
        blade.isLoading = true;

        catalogs.getCatalogs({}, function (results) {
            blade.isLoading = false;
            //filter the catalogs in which we not have access
            $scope.objects = results;

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach(results, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.refreshItems = function () {
        if (preventCategoryListingOnce) {
            preventCategoryListingOnce = undefined;
        } else {
            var newBlade = {
                id: 'itemsList1',
                level: 1,
                breadcrumbs: blade.breadcrumbs,
                title: 'catalog.blades.categories-items-list.title',
                subtitle: 'catalog.blades.categories-items-list.subtitle',
                subtitleValues: selectedNode != null ? { name: selectedNode.name } : '',
                catalogId: (selectedNode != null) ? selectedNode.id : null,
                catalog: selectedNode,
                controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html',
                securityScopes: selectedNode.securityScopes
            };

            bladeNavigationService.showBlade(newBlade, blade);
        }
    };

    $scope.editCatalog = function (catalog) {
        if (catalog.virtual) {
            showVirtualCatalogBlade(catalog.id, null, catalog.name);
        }
        else {
            showCatalogBlade(catalog.id, null, catalog.name);
        }
        preventCategoryListingOnce = true;
    };

    $scope.deleteCatalog = function (node) {
        var dialog = {
            id: "confirmDelete",
            name: node.name,
            callback: function (remove) {
                if (remove) {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        selectedNode = undefined;
                        $scope.selectedNodeId = undefined;
                        blade.isLoading = true;
                        catalogs.delete({ id: node.id },
                            blade.refresh,
                            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); }
                        );
                    });
                }
            }
        };
        dialogService.showDialog(dialog, 'Modules/$(VirtoCommerce.Catalog)/Scripts/dialogs/deleteCatalog-dialog.tpl.html', 'platformWebApp.confirmDialogController');

        preventCategoryListingOnce = true;
    };

    function showCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            id: 'catalogEdit',
            subtitle: 'catalog.blades.catalog-detail.subtitle',
            controller: 'virtoCommerce.catalogModule.catalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html',
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function showVirtualCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            subtitle: 'catalog.blades.virtual-catalog.subtitle-virtual',
            id: 'catalogEdit',
            controller: 'virtoCommerce.catalogModule.virtualCatalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html',
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        $scope.refreshItems();
    };


    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    if (authService.checkPermission('catalog:create')) {
        blade.toolbarCommands.splice(1, 0, {
            name: "platform.commands.add",
            icon: 'fa fa-plus',
            executeMethod: function () {
                selectedNode = undefined;
                $scope.selectedNodeId = undefined;

                var newBlade = {
                    id: 'listItemChild',
                    title: 'catalog.blades.catalog-add.title',
                    subtitle: 'catalog.blades.catalog-add.subtitle',
                    controller: 'virtoCommerce.catalogModule.catalogAddController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-add.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        });
    }

    // actions on load
    blade.refresh();
}]);