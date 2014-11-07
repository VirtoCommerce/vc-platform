angular.module('catalogModule.blades.virtualCatalogMapping', [
   'catalogModule.blades.virtualCatalogMappingSource',
   'catalogModule.resources.categories',
   'catalogModule.resources.catalogs'
])
.controller('virtualCatalogMappingTargetController', ['$injector', '$rootScope', '$scope', '$filter', 'virtualCatalogSearch', 'categories', 'itemsSearch', 'items', 'bladeNavigationService', 'dialogService',
function ($injector, $rootScope, $scope, $filter, virtualCatalogSearch, categories, itemsSearch, items, bladeNavigationService, dialogService) {
    $scope.tree = undefined;
    $scope.blade.selectedMapping = null;

    var selectedNode = null;
    var state = null;
    var currentEntityId = $scope.blade.parentBlade.currentEntityId;
    $scope.filter = { searchKeyword: undefined, catalogId: undefined, categoryId: undefined };

    $scope.doEnterKeyword = function (event) {
        selectedNode = null;
        $scope.blade.selectedNode = selectedNode;
        $scope.blade.refresh();
    };

    $scope.addCategory = function (catalogId, parentCategoryId) {
        categories.newCategory({ catalogId: catalogId, parentCategoryId: parentCategoryId },
            function (category) {
                // prepare for renaming
                $scope.edit.model = category.name;
                $scope.edit.editingNode = null;
                $scope.edit.editingEntityId = category.id;

                selectedNode = category;
                state = 'renamePending';
                $scope.blade.refresh();
            });
    }

    // inline category rename
    $scope.edit = {
        reset: function () {
            $scope.edit.editingEntityId = null;
        },
        confirm: function () {
            $scope.blade.isLoading = true;

            categories.rename({ id: $scope.edit.editingEntityId, newName: $scope.edit.model }, function (newName) {
                $scope.blade.isLoading = false;
                $scope.edit.editingNode.name = newName.name;
                $scope.edit.reset();
            });
        }
    };

    $scope.renameCategory = function (categoryNode) {
        $scope.edit.model = categoryNode.name;
        $scope.edit.editingNode = categoryNode;
        $scope.edit.editingEntityId = categoryNode.id;
    };

    $scope.removeCategory = function (category) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to remove category '" + category.name + "'?",
            callback: function (remove) {
                if (remove) {
                    categories.delete({ id: category.id }, function (data, headers) {
                        $scope.selectNode(category.parent);
                        $scope.blade.refresh();
                    });
                }
            }
        }

        dialogService.showConfirmationDialog(dialog);
    }

    $scope.unmapNode = function (node) {
        $scope.blade.isLoading = true;

        //var ids = [];
        //if (node.type === 'catalog') {
        //    angular.forEach(node.children, function (child) {
        //        ids.push(child.id);
        //    });
        //} else {
        //    ids.push(node.id);
        //}
        var nodeparent = node.parent;

        categories.linkcategories({ ids: [node.id], linkId: nodeparent.id, createLink: false }, function () {
            nodeparent.children.splice(nodeparent.children.indexOf(node), 1);
            nodeparent.checked = false;
            $scope.selectNode(nodeparent);
            refreshMappings();
        });
    }

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        var searchResult = virtualCatalogSearch.query({ catalogId: currentEntityId, keyword: $scope.filter.searchKeyword }, function () {
            $scope.blade.isLoading = false;

            $scope.tree = searchResult.treeNodes;
            constructCatalogsTreeRecursive(null, $scope.tree);

            if (selectedNode != null) {
                //select category from new tree
                traverseDown($scope.tree, function (node) {
                    if (node.id == selectedNode.id) {
                        selectedNode = node;
                    };
                });
                //expand category
                traverseUp(selectedNode, function (node) {
                    node.checked = true;
                });

                if (state === 'renamePending') {
                    state = null;
                    $scope.edit.editingNode = selectedNode;
                }
            } else {
                // expand catalog categories
                $scope.tree[0].checked = true;
            }
        });
    };

    function constructCatalogsTreeRecursive(parent, nodes) {
        angular.forEach(nodes, function (node) {
            if (node != null) {
                node.parent = parent;
                constructCatalogsTreeRecursive(node, node.children);
            }
        });
    };

    function traverseDown(tree, iterator) {
        angular.forEach(tree, function (node) {
            if (angular.isDefined(node) && node != null) {
                iterator(node);
                traverseDown(node.children, iterator);
            };
        });
    };

    function traverseUp(node, iterator) {
        iterator(node);
        if (angular.isDefined(node.parent) && node.parent != null) {
            traverseUp(node.parent, iterator);
        };
    };

    $scope.selectNode = function (node) {
        if (node.id !== $scope.edit.editingEntityId) {
            node.checked = !node.checked;
            selectedNode = node;
            $scope.blade.selectedNode = selectedNode;

            refreshList();
        }
    };

    $scope.getItemsAutocomplete = function (keyword) {
        return itemsSearch.query({ categoryId: $scope.filter.categoryId, keyword: keyword, start: 0, count: 20 }).$promise.then(function (result) {
            return result.items;
        });
    };

    function refreshMappings() {
        $scope.blade.isLoading = true;
        $scope.blade.parentBlade.refreshMappings().$promise.then(function (searchResult) {
            $scope.blade.mappedNodes = searchResult.treeNodes;
            $scope.blade.isLoading = false;
        });
    }

    $scope.blade.refreshMappings = function () {
        $scope.blade.refresh();

        refreshMappings();

        refreshList();
    }

    // ITEMS LIST
    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    function refreshList() {
        if (selectedNode != null) {
            $scope.blade.isLoading = true;

            //Set filters
            if (selectedNode.type == "catalog") {
                $scope.filter.catalogId = currentEntityId;
                $scope.filter.categoryId = undefined;
            } else {
                $scope.filter.catalogId = undefined; // currentEntityId;
                $scope.filter.categoryId = selectedNode.id;
            }

            itemsSearch.query(
                {
                    catalogId: $scope.filter.catalogId,
                    categoryId: $scope.filter.categoryId,
                    keyword: $scope.filter.searchKeyword,
                    responseGroup: 'withItems',
                    start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    count: $scope.pageSettings.itemsPerPageCount
                },
            function (data, headers) {
                $scope.blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                $scope.items = data.items;

                if ($scope.selectedItem != null) {
                    var selectedItem = findItem($scope.selectedItem.id);
                    if (angular.isDefined(selectedItem)) {
                        $scope.selectedItem = selectedItem;
                    }
                }
            });
        }
    };

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        refreshList();
    });

    function findItem(id) {
        var retVal;
        angular.forEach($scope.items, function (item) {
            if (item.id == id)
                retVal = item;
        });

        return retVal;
    };
    // endof LIST

    // general
    function closeThisBlade(closeCallback) {
        if ($scope.blade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.blade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.blade.onClose = function (closeCallback) {
        closeThisBlade(closeCallback);
    };

    $scope.bladeToolbarCommands = [
        {
            name: "unmap selected", icon: 'remove',
            executeMethod: function () {
                var ids = [];
                angular.forEach($scope.items, function (item) {
                    if (item.selected)
                        ids.push(item.id);
                });

                items.linkitems({ ids: ids, linkId: selectedNode.id, createLink: false }, function () {
                    refreshList();
                });
            },
            canExecuteMethod: function () {
                var selected = false;
                angular.forEach($scope.items, function (item) {
                    if (item.selected)
                        selected = true;
                });
                return selected;
            }
        }
    ];

    // actions on load
    $scope.edit.reset();
    $scope.blade.refresh();
    refreshMappings();

    var sourceBlade = {
        id: "mappingSource",
        currentEntityId: currentEntityId,
        title: 'mapping',
        subtitle: 'choose items for mapping',
        controller: 'virtualCatalogMappingSourceController',
        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/virtual-catalog-mapping-source.tpl.html'
    };
    bladeNavigationService.showBlade(sourceBlade, $scope.blade);
}]);