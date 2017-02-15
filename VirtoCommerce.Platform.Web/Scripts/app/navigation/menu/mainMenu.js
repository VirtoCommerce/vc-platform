angular.module('platformWebApp')
.factory('platformWebApp.mainMenuService', [function () {

    var menuItems = [];
    var dynamicMenuItems = [];

    function sortByPriority(a, b) {
        if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
            return a.priority - b.priority;
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

    function addMenuItem(menuItem, isDynamic) {
        patchMenuItem(menuItem);
        if (!isDynamic) {
            menuItems.push(menuItem);
            constructList();
        } else {
            dynamicMenuItems.push(menuItem);
        }
    }

    function patchMenuItem(menuItem) {
        if (!angular.isDefined(menuItem.priority)) {
            menuItem.priority = Number.NaN;
        }
        if (!angular.isDefined(menuItem.favorite)) {
            menuItem.favorite = false;
        }
    }

    function removeMenuItem(menuItem, isDynamic) {
        var list = !isDynamic ? menuItems : dynamicMenuItems;
        var index = list.indexOf(menuItem);
        list.splice(index, 1);
        if (!isDynamic) constructList();
    }

    function findByPath(path, isDynamic) {
        return _.find(!isDynamic ? menuItems : dynamicMenuItems, function (menuItem) { return menuItem.path === path });
    };

    var retVal = {
        menuItems: menuItems,
        dynamicMenuItems: dynamicMenuItems,
        addMenuItem: addMenuItem,
        removeMenuItem: removeMenuItem,
        findByPath: findByPath
    };
    return retVal;
}])
.directive('vaMainMenu', ['$compile', '$filter', '$state', '$translate', 'platformWebApp.mainMenuService', 'platformWebApp.authService', 'platformWebApp.accounts', 'platformWebApp.pushNotificationService',
    function ($compile, $filter, $state, $translate, mainMenuService, authService, accounts, pushNotificationService) {

    return {
        restrict: 'E',
        replace: true,
        templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu.tpl.html',
        link: function (scope, element, attr) {

            scope.currentMenuItem = undefined;
            scope.menuItems = mainMenuService.menuItems;
            scope.dynamicMenuItems = mainMenuService.dynamicMenuItems;
            scope.notificationMenuItem = pushNotificationService.menuItem;
            scope.search = {
                searchMenuItems: [],
                searchText : ""
            };
            scope.findByPath = mainMenuService.findByPath;

            loadDynamicMenuItems();

            scope.selectMenuItem = function (menuItem) {
                scope.currentMenuItem = menuItem;
                scope.showMenuItemList = false;
                scope.showNotificationDropdown = false;
                //run action
                if (angular.isDefined(menuItem.action)) {
                    menuItem.action();
                }
            };

            scope.toggleFavorite = function(menuItem) {
                menuItem.favorite = !menuItem.favorite;
                if (menuItem.favorite) {
                    mainMenuService.addMenuItem(menuItem, true);
                } else {
                    mainMenuService.removeMenuItem(menuItem, true);
                }
                saveDynamicMenuItems();
            };

            scope.searchMenuItem = function(menuItems) {
                scope.search.searchMenuItems.length = 0;
                scope.menuItems.forEach(function(menuItem) {
                    if (menuItem.group != null && $translate.instant(menuItem.title).search(new RegExp(scope.search.searchText, "i")) > -1) {
                        scope.search.searchMenuItems.push(menuItem);
                    }
                });
            };

            scope.sortableOptions = {
                axis: 'y',
                cursor: "move",
                // bug in ui-sortable: always use container with tolerance
                // because otherwise draggable item can't replace top item:
                // where is no space between top item and container top border
                containment: ".menu ul",
                tolerance: "pointer",
                stop: function (e, ui) {
                    saveDynamicMenuItems();
                }
            };

            scope.$on('loginStatusChanged', loadDynamicMenuItems);

            function loadDynamicMenuItems() {
                // https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/basic-functions/working-with-platform-security
                if (authService.checkPermission("platform:security:read")) {
                    accounts.get({ id: authService.userLogin },
                        function(currentUser) {
                            var userMenuItemsSettingName = "VirtoCommerce.Platform.General.MainMenu.Favorites";
                            var userMenuItemsSetting = _.find(currentUser.settings,
                                function(setting) {
                                    return setting.name === userMenuItemsSettingName;
                                });
                            Array.prototype.push.apply(mainMenuService.dynamicMenuItems,
                                _.map(_.sortBy(angular.fromJson(userMenuItemsSetting.value), 'order'),
                                    function(menuItemModel) {
                                        var menuItem = mainMenuService.findByPath(menuItemModel.path);
                                        menuItem.favorite = true;
                                        return menuItem;
                                    }));
                        });
                }
            }

            function saveDynamicMenuItems() {
                if (authService.checkPermission("platform:security:read") && authService.checkPermission("platform:security:update")) {
                    accounts.get({ id: authService.userLogin },
                        function(currentUser) {
                            var userMenuItemsSettingName = "VirtoCommerce.Platform.General.MainMenu.Favorites";
                            var userMenuItemsSetting = _.find(currentUser.settings,
                                function(setting) {
                                    return setting.name === userMenuItemsSettingName;
                                });
                            userMenuItemsSetting.value = angular.toJson(_.map(mainMenuService.dynamicMenuItems,
                                function(menuItem, index) {
                                    return { path: menuItem.path, order: index };
                                }));
                            accounts.update(currentUser);
                        });
                }
            }
        }
    }
}]).directive('vaFavorites', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            $(element).keydown(function (e) {
                if (e.shiftKey && e.keyCode == 32) { // Shift + Space
                    $(e.target).prev(".list-fav").click();
                }
            });
        }
    }
});
