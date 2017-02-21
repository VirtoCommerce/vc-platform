angular.module('platformWebApp')
.factory('platformWebApp.mainMenuService', [function () {

    var menuItems = [];
    var menuItemsCache = [];

    function sortByPriority(a, b) {
        if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
            return a.priority < b.priority ? -1 : a.priority > b.priority ? 1 : 0;
        }
        return -1;
    };

    function constructList() {
        angular.forEach(menuItems, function (menuItem) {
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
        menuItemsCache.push({ path: menuItem.path, isAlwaysOnBar: menuItem.isAlwaysOnBar, isFavorite: menuItem.isFavorite, order: menuItem.order });
        constructList();
    }

    function removeMenuItem(menuItem) {
        var index = menuItems.indexOf(menuItem);
        menuItems.splice(index, 1);
        var cacheIndex = findCacheIndexByPath(menuItem.path);
        menuItemsCache.splice(cacheIndex, 1);
        constructList();
    }

    function resetUserRelatedSettings() {
        angular.forEach(menuItems, function(menuItem) {
            resetToDefaults(menuItem);
        });
    }

    function setDefaults(menuItem) {
        // set defaults
        if (!angular.isDefined(menuItem.priority)) {
            menuItem.priority = Number.NaN;
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

    function resetToDefaults(menuItem) {
        var cachedMenuItemIndex = findCacheIndexByPath(menuItem.path);
        if (cachedMenuItemIndex > -1) {
            var cachedMenuItem = menuItemsCache[cachedMenuItemIndex];
            menuItem.isAlwaysOnBar = cachedMenuItem.isAlwaysOnBar;
            menuItem.isFavorite = cachedMenuItem.isFavorite;
            menuItem.order = cachedMenuItem.order;
        } else {
            setDefaults(menuItem);
        }
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
        resetUserRelatedSettings: resetUserRelatedSettings,
        findByPath: findByPath
    };
    return retVal;
}])
.directive('vaMainMenu', [function () {

    return {
        restrict: 'E',
        replace: true,
        scope: {
            items: "=*",
            isCollapsed: "="
        },
        templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu.tpl.html',
        link: function (scope, element, attr) {
            scope.selectItem = function (menuItem) {
                scope.showList.value = false;
                if (angular.isDefined(menuItem.action)) {
                    menuItem.action(scope);
                }
            };
        }
    }
}]);
