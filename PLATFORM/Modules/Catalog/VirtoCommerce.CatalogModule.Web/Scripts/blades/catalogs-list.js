angular.module('virtoCommerce.catalogModule')
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
            $scope.objects = _.filter(results, function (x) {
                return authService.checkPermission('catalog:catalogs:manage', 'catalog:' + x.name);
            });
            //init security scopes need for evaluate scope bounded ACL
            //that securityScopes will be inherited all children blades (by bladeNavigationService)
            blade.securityScopes = _.map($scope.objects, function (x) { return 'catalog:' + x.name; }).join();

            if (selectedNode != null) {
                //select the node in the new list
                angular.forEach($scope.objects, function (node) {
                    if (selectedNode.id === node.id) {
                        selectedNode = node;
                    }
                });
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

    };

    blade.onClose = function (closeCallback) {
        if (blade.childrenBlades.length > 0) {
            var callback = function () {
                if (blade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach(blade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.refreshItems = function () {
        if (preventCategoryListingOnce) {
            preventCategoryListingOnce = undefined;
        } else {
            var newBlade = {
                id: 'itemsList1',
                level: 1,
                breadcrumbs: blade.breadcrumbs,
                title: 'Categories & Items',
                subtitle: 'Browsing ' + (selectedNode != null ? '"' + selectedNode.name + '"' : ''),
                catalogId: (selectedNode != null) ? selectedNode.id : null,
                catalog: selectedNode,
                controller: 'virtoCommerce.catalogModule.categoriesItemsListController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/categories-items-list.tpl.html'
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

    function showCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            id: 'catalogEdit',
            subtitle: 'Catalog details',
            deleteFn: onAfterCatalogDeleted,
            controller: 'virtoCommerce.catalogModule.catalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }
    
    function showVirtualCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            subtitle: 'Virtual catalog details',
            deleteFn: onAfterCatalogDeleted,
            id: 'catalogEdit',
            controller: 'virtoCommerce.catalogModule.virtualCatalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function onAfterCatalogDeleted() {
        selectedNode = undefined;
        $scope.selectedNodeId = undefined;
        blade.refresh();
    }

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        $scope.refreshItems();
    };
    

    blade.toolbarCommands = [
        {
            name: "Manage", icon: 'fa fa-edit',
            executeMethod: function () {
                $scope.editCatalog(selectedNode);
            },
            canExecuteMethod: function () {
                return selectedNode;
            },
            permission: 'catalog:catalogs:manage'
        }
    ];

    if (authService.checkPermission('catalog:catalogs:manage') || authService.checkPermission('catalog:virtual_catalogs:manage')) {
        blade.toolbarCommands.splice(0, 0, {
            name: "Add",
            icon: 'fa fa-plus',
            executeMethod: function () {
                selectedNode = undefined;
                $scope.selectedNodeId = undefined;

                var newBlade = {
                    id: 'listItemChild',
                    title: 'New catalog',
                    subtitle: 'Choose new catalog type',
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