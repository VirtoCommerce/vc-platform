angular.module('platformWebApp')
.factory('platformWebApp.mainMenuService', ['$filter', function ($filter) {

    var menuItems = [];
    var menuBarItems = [];

    // do not compare priority by substraction! "more" item has value as max integer
    function compare(a, b) {
        if (a < b) {
            return -1;
        }
        if (a === b) {
            return 0;
        }
        if (a > b) {
            return 1;
        }
        return null;
    }

    function sortByPriority(a, b) {
        if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
            return compare(a.priority, b.priority);
        }
        return -1;
    };

    function sortMenuItem(a, b) {
        var result;
        if (a.parent === b.parent) {
            result = compare(a.priority, b.priority);
            return result;
        }
        var maximalParentNestingDepth = getParentNestingLevel(a) - getParentNestingLevel(b);
        if (maximalParentNestingDepth === 0) {
            result = compare(a.parent.priority, b.parent.priority);
            return result;
        }
        var original = maximalParentNestingDepth > 0 ? b : a;
        var nestedParent = getParentAtLevel(maximalParentNestingDepth > 0 ? a : b, Math.abs(maximalParentNestingDepth));
        if (original === nestedParent) {
            result = maximalParentNestingDepth;
            return result;
        }
        result = compare(nestedParent.priority, original.priority) * maximalParentNestingDepth;
        return result;
    }

    function getParentNestingLevel(menuItem) {
        if (menuItem.parent == null) {
            return 0;
        }
        var parentNestingLevel = getParentNestingLevel(menuItem.parent);
        parentNestingLevel++;
        return parentNestingLevel;
    }

   function getParentAtLevel(menuItem, depth) {
       var result = menuItem;
       for (var i = 0; i < depth; i++) {
           result = result.parent;
       }
       return result;
   }

    function constructTree() {
        //console.log('------------------constructTree---------------------');
        angular.forEach(menuItems, function (menuItem) {
            //console.log(menuItem.path);
            if (menuItem.parent == null) {
                var pathParts = menuItem.path.split('/');
                var parentPath = null;
                // we want to check menuItem for null or undefined, so use '== null'
                if (pathParts.length === 1 && menuItem.dynamic == null) {
                    parentPath = "more";
                }
                if (pathParts.length > 1 && menuItem.dynamic == null) {
                    pathParts.pop();
                    parentPath = pathParts.join('/');
                }
                var parent = _.find(menuItems, function(currentMenuItem) { return currentMenuItem.path === parentPath });
                if (angular.isDefined(parent)) {
                    menuItem.parent = parent;
                    parent.children.push(menuItem);
                    parent.children.sort(sortByPriority);
                }
            }
        });
        // sort tree items
        menuItems.sort(sortMenuItem);
        // we want to check that menuItem has value defined and it is not null, so use '!= null'
        menuBarItems.length = 0;
        for (var i = 0; i < menuItems.length; i++) {
            if (menuItems[i].dynamic != null) {
                menuBarItems.push(menuItems[i]);
            }
        }
    };

    function findByPath(path) {
        return _.find(menuItems, function (menuItem) { return menuItem.path === path && menuItem.dynamic !== true; });
    };

    function addMenuItem(menuItem) {
        // place it there, because we may need access to this properties BEFORE tree will be constructed
        menuItem.parent = null;
        menuItem.children = [];
        if (!angular.isDefined(menuItem.favorite)) {
            menuItem.favorite = false;
        }
        menuItems.push(menuItem);
        constructTree();
    }

    function removeMenuItem(menuItem) {
        var index = menuItems.indexOf(menuItem);
        menuItems.splice(index, 1);
        constructTree();
    }

    var retVal = {
        menuItems: menuItems,
        menuBarItems: menuBarItems,
        addMenuItem: addMenuItem,
        removeMenuItem: removeMenuItem,
        findByPath: findByPath
    };
    return retVal;
}])
.directive('vaMainMenu', ['$compile', '$filter', '$state', 'platformWebApp.mainMenuService', function ($compile, $filter, $state, mainMenuService) {

    return {
        restrict: 'E',
        replace: true,
        templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu.tpl.html',
        link: function (scope, element, attr) {

            scope.currentMenuItem = undefined;
            // keep for backward compatibility
            scope.menuItems = mainMenuService.menuItems;
            scope.menuBarItems = mainMenuService.menuBarItems;

            scope.selectMenuItem = function (menuItem) {
                if (scope.showSubMenu && scope.currentMenuItem === menuItem) {
                    scope.showSubMenu = false;
                } else {
                    scope.currentMenuItem = menuItem;

                    // notifications should always open, even if nothing is there
                    scope.showSubMenu = menuItem.children.length > 0 || menuItem.path === 'notification';
                }

                //run action
                if (angular.isDefined(menuItem.action)) {
                    menuItem.action();
                }
            };

            scope.toggleFavorite = function(menuItem) {
                menuItem.favorite = !menuItem.favorite;
                var dynamicMenuItem;
                if (menuItem.favorite) {
                    var lastMenuBarItemPriority = mainMenuService.menuBarItems[mainMenuService.menuBarItems.length - 2].priority;
                    dynamicMenuItem = angular.extend({}, menuItem, { priority: lastMenuBarItemPriority + 1, dynamic: true });
                    mainMenuService.addMenuItem(dynamicMenuItem);
                } else {
                    dynamicMenuItem = _.find(mainMenuService.menuItems, function(currentMenuItem) { return currentMenuItem.path === menuItem.path && currentMenuItem.dynamic === true; });
                    mainMenuService.removeMenuItem(dynamicMenuItem);
                }
            };

            scope.sortableOptions = {
                stop: function (e, ui) {
                    if (mainMenuService.menuBarItems[0].dynamic === true) {
                        mainMenuService.menuBarItems[0].priority = 0;
                    }
                    for (var i = 1; i < mainMenuService.menuBarItems.length - 2; i++) {
                        if (mainMenuService.menuBarItems[i].dynamic === true) {
                            mainMenuService.menuBarItems[i].priority = mainMenuService.menuBarItems[i - 1].priority + 1;
                        }
                    }
                },
                axis: 'y',
                cursor: "move",
                items: "li:not(.static)"
            };
        }
    }
}]);
