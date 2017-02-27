angular.module('platformWebApp')
.factory('platformWebApp.mainMenuService', [function () {

    var menuItems = [];
    var menuItemsCache = [];

    function sortByGroupFirst(a, b) {
        return a.path.split('/').length - b.path.split('/').length;
    };

    function sortByPriority(a, b) {
        if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
            return a.priority < b.priority ? -1 : a.priority > b.priority ? 1 : 0;
        }
        return -1;
    };

    function constructList() {
        angular.forEach(menuItems.sort(sortByGroupFirst), function (menuItem) {
            if (!angular.isDefined(menuItem.group)) {
                menuItem.group = null;
            }
            if (menuItem.group == null) {
                var pathParts = menuItem.path.split('/');
                var groupPath = null;
                if (pathParts.length > 1) {
                    pathParts.pop();
                    groupPath = pathParts.join('/');
                }
                var group = _.find(menuItems, function (menuItem) { return menuItem.path === groupPath });
                if (angular.isDefined(group)) {
                    menuItem.group = group;
                }
            }
        });
        menuItems.sort(sortByPriority);
    };

    function addMenuItem(menuItem) {
        setDefaults(menuItem);
        menuItems.push(menuItem);
        menuItemsCache.push({ path: menuItem.path, isFavorite: menuItem.isFavorite, order: menuItem.order });
        constructList();
    }

    function removeMenuItem(menuItem) {
        var index = menuItems.indexOf(menuItem);
        menuItems.splice(index, 1);
        var cacheIndex = findCacheIndexByPath(menuItem.path);
        menuItemsCache.splice(cacheIndex, 1);
        constructList();
    }

    function setDefaults(menuItem) {
        // set defaults
        if (!angular.isDefined(menuItem.priority)) {
            menuItem.priority = Number.NaN;
        }
        if (!angular.isDefined(menuItem.children)) {
            menuItem.children = [];
        }
        if (!angular.isDefined(menuItem.isAlwaysOnBar)) {
            menuItem.isAlwaysOnBar = false;
        }
        if (!angular.isDefined(menuItem.isFavorite)) {
            menuItem.isFavorite = false;
        }
        if (!angular.isDefined(menuItem.order)) {
            // place at the end
            menuItem.order = 9007199254740991; // Number.MAX_SAFE_INTEGER;
        }
    }

    function resetUserRelatedSettings() {
        angular.forEach(menuItems, function (menuItem) {
            var cachedMenuItemIndex = findCacheIndexByPath(menuItem.path);
            if (cachedMenuItemIndex > -1) {
                var cachedMenuItem = menuItemsCache[cachedMenuItemIndex];
                menuItem.isFavorite = cachedMenuItem.isFavorite;
                menuItem.order = cachedMenuItem.order;
            }
        });
    }

    function findByPath(path) {
        return _.find(menuItems, function (menuItem) { return menuItem.path === path });
    };

    function findCacheIndexByPath(path) {
        return _.findIndex(menuItemsCache, function (cachedMenuItem) { return cachedMenuItem.path === path; });
    }

    var retVal = {
        menuItems: menuItems,
        addMenuItem: addMenuItem,
        removeMenuItem: removeMenuItem,
        findByPath: findByPath,
        resetUserRelatedSettings: resetUserRelatedSettings
    };
    return retVal;
}])
.directive('vaMainMenu', ["$filter",
    function ($filter) {

    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        scope: {
            items: "=*",
            isCollapsed: "="
        },
        templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu.tpl.html',
        link: function (scope) {

            scope.$on('$stateChangeStart', function () {
                scope.openedItem = null;
            });

            scope.selectItem = function (menuItem) {
                if (menuItem.path === "home") {
                    scope.selectedItem = null;
                    scope.openedItem = null;
                } else if (scope.openedItem === menuItem) {
                    scope.openedItem = null;
                } else if (menuItem.children.length > 0 || menuItem.path === "more") {
                    scope.openedItem = menuItem;
                }
                else {
                    scope.selectedItem = menuItem;
                    scope.openedItem = null;
                }
                if (angular.isDefined(menuItem.action)) {
                    menuItem.action();
                }
            };

            updateFavorites();
            scope.$watch("items", function () { updateFavorites(); }, true);
            // required by ui-sortable: we can't use filters with it
            // https://github.com/angular-ui/ui-sortable#usage
            function updateFavorites() {
                scope.favorites = $filter("orderBy")($filter("filter")(scope.items, { isFavorite: "true" }), "order");
            }
            
            scope.toggleFavorite = function (menuItem) {
                menuItem.isFavorite = !menuItem.isFavorite;
                // clear order when removed from favorites
                if (!menuItem.isFavorite) {
                    menuItem.order = undefined;
                }
                var favorites = _.filter(_.sortBy(scope.items, function (menuItem) { return menuItem.order; }), function (menuItem) { return menuItem.isFavorite && !menuItem.isAlwaysOnBar; });
                // re-calculate order
                for (var i = 0; i < favorites.length; i++) {
                    favorites[i].order = i;
                }
            };

            scope.sortableOptions = {
                axis: "y",
                cursor: "move",
                // always use container with tolerance
                // because otherwise draggable item can't replace top item:
                // where is no space between top item and container top border
                containment: ".outer-wrapper",
                items: ".__draggable",
                tolerance: "pointer",
                stop: function (e, ui) {
                    // re-calculate order for all favorites
                    // we may use scope.favorites or source model of ui-sortable
                    // I'm prefer last, to avoid dependence on directive's model
                    var favorites = ui.item.sortable.sourceModel;
                    for (var i = 0; i < favorites.length; i++) {
                        favorites[i].order = i;
                    }
                }
            };
        }
    }
}]).directive('vaFavorites', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            $(element).keydown(function (e) {
                if (e.shiftKey && e.keyCode === 32) { // Shift + Space
                    $(e.target).find(".list-fav").click();
                }
            });
        }
    }
});
