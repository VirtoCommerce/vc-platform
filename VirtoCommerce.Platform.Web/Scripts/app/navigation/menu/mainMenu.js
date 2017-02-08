angular.module('platformWebApp')
.factory('platformWebApp.mainMenuService', ['$filter', function ($filter) {

    var menuItems = [];

    function sortByParentFirst(a, b) {
        return a.path.split('/').length - b.path.split('/').length;
    };
    function sortByPriority(a, b) {
        if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
            return a.priority - b.priority;
        }
        return -1;
    };

    function constructTree() {
        //clear arrays
        //menuTree.splice(0, menuTree.length)
        //console.log('------------------constructTree---------------------');
        angular.forEach(menuItems.sort(sortByParentFirst), function (menuItem) {
            //console.log(x.path);
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
        //sort tree items
        menuItems.sort(sortByPriority);
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
            scope.menuItems = mainMenuService.menuItems;

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
                    dynamicMenuItem = angular.extend({}, { dynamic: true }, menuItem);
                    mainMenuService.addMenuItem(dynamicMenuItem);
                } else {
                    dynamicMenuItem = _.find(mainMenuService.menuItems, function(currentMenuItem) { return currentMenuItem.path === menuItem.path && currentMenuItem.dynamic === true; });
                    mainMenuService.removeMenuItem(dynamicMenuItem);
                }
            };

            scope.sortableOptions = {
                items: "li:not(.static)"
            };
        }
    }
}]);
