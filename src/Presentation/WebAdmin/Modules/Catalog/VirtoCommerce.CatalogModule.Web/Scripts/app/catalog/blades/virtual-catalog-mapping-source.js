angular.module('catalogModule.blades.virtualCatalogMappingSource', [
    'catalogModule.resources.categories',
    'catalogModule.resources.catalogs'])
.controller('virtualCatalogMappingSourceController', ['$injector', '$rootScope', '$scope', '$filter', 'catalogs', 'categories', 'items', 'itemsSearch', 'bladeNavigationService', 'dialogService',
function ($injector, $rootScope, $scope, $filter, catalogs, categories, items, itemsSearch, bladeNavigationService, dialogService) {
    $scope.currentBlade = $scope.blade;
    $scope.tree = undefined;

    var selectedNode = undefined;
    var parentBlade = $scope.currentBlade.parentBlade;
    $scope.filter = { searchKeyword: undefined, categoryId: undefined, catalogId: undefined };

    $scope.doEnterKeyword = function (event) {
        $scope.currentBlade.refresh();
    };

    $scope.mapCategory = function (categoryNode) {
        if (parentBlade.selectedNode != null && parentBlade.selectedNode.type === 'category') {
            $scope.currentBlade.isLoading = true;

            categories.linkcategories({ ids: [categoryNode.id], linkId: parentBlade.selectedNode.id, createLink: true }, function () {
                $scope.currentBlade.isLoading = false;
                parentBlade.refreshMappings();
            });
        } else {
            // alert select category to map to.
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "Error",
                message: "Select a target category first."
            };
            dialogService.showNotificationDialog(dialog);
        }
    }

    $scope.currentBlade.refresh = function () {
        $scope.currentBlade.isLoading = true;

        updateFilter();

        // categoryId: $scope.filter.categoryId,
        var searchResult = itemsSearch.query({ catalogId: $scope.filter.catalogId, keyword: $scope.filter.searchKeyword, responseGroup: 'withCatalogs, withCategories', count: 0 }, function () {
            $scope.currentBlade.isLoading = false;

            $scope.tree = [];
            // add real catalogs only
            angular.forEach(searchResult.treeNodes, function (node) {
                if (!node.virtual) {
                    $scope.tree.push(node);
                }
            });

            constructCatalogsTreeRecursive(null, $scope.tree);

            if (angular.isDefined(selectedNode)) {
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
            }
            else {
                // expand catalog categories
                $scope.tree[0].checked = true;
            }
        });
    };

    function updateFilter() {
        if (angular.isDefined(selectedNode)) {
            if (selectedNode.type == "category") {
                $scope.filter.catalogId = null; // selectedNode.catalogId;
                $scope.filter.categoryId = selectedNode.id;
            } else {
                $scope.filter.catalogId = selectedNode.id;
                $scope.filter.categoryId = undefined;
            }
        }
    }

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

    $scope.selectNode = function (category) {
        category.checked = !category.checked;
        selectedNode = category;

        refreshList();
    };

    $scope.getItemsAutocomplete = function (keyword) {
        return itemsSearch.query({ categoryId: $scope.filter.categoryId, keyword: keyword, start: 0, count: 20 }).$promise.then(function (result) {
            return result.items;
        });
    };

    // ITEMS LIST
    //pagination settigs
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    function refreshList() {
        $scope.currentBlade.isLoading = true;

        updateFilter();

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
		    $scope.currentBlade.isLoading = false;
		    $scope.pageSettings.totalItems = data.totalCount;
		    $scope.items = data.items;

		    if ($scope.selectedItem != null) {
		        var selectedItem = findItem($scope.selectedItem.id);
		        if (angular.isDefined(selectedItem)) {
		            $scope.selectedItem = selectedItem;
		        }
		    }
		});
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
    $scope.blade.onClose = function (closeCallback) {
        closeCallback();
        bladeNavigationService.closeBlade(parentBlade, closeCallback);
    };

    $scope.bladeToolbarCommands = [
        {
            name: "map selected", icon: 'checkmark',
            executeMethod: function () {
                $scope.currentBlade.isLoading = true;

                var ids = [];
                angular.forEach($scope.items, function (item) {
                    if (item.selected) {
                        ids.push(item.id);
                    }
                });

                items.linkitems({ ids: ids, linkId: parentBlade.selectedNode.id, createLink: true }, function () {
                    refreshList();
                });
            },
            canExecuteMethod: function () {
                var selected = false;
                angular.forEach($scope.items, function (item) {
                    if (item.selected)
                        selected = true;
                });
                return selected && parentBlade.selectedNode != null;
            }
        }
    ];

    $scope.currentBlade.refresh();
}]);