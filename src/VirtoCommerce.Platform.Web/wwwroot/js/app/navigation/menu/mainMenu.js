angular.module('platformWebApp')
    .factory('platformWebApp.mainMenuService', [function () {
        var menuItems = [];

        function sortByGroupFirst(a, b) {
            return a.path.split('/').length - b.path.split('/').length;
        }

        function sortByPriority(a, b) {
            if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
                return a.priority < b.priority ? -1 : a.priority > b.priority ? 1 : 0;
            }
            return -1;
        }

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
        }

        function addMenuItem(menuItem) {
            resetMenuItemDefaults(menuItem);
            menuItems.push(menuItem);
            constructList();
        }

        function removeMenuItem(menuItem) {
            var index = menuItems.indexOf(menuItem);
            menuItems.splice(index, 1);
            constructList();
        }

        function resetMenuItemDefaults(menuItem) {
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
            menuItem.isCollapsed = false;
            menuItem.isFavorite = false;
            // place at the end
            menuItem.order = Number.MAX_SAFE_INTEGER;
        }

        function findByPath(path) {
            return _.find(menuItems, function (menuItem) { return menuItem.path === path });
        }

        var retVal = {
            menuItems: menuItems,
            addMenuItem: addMenuItem,
            removeMenuItem: removeMenuItem,
            resetMenuItemDefaults: resetMenuItemDefaults,
            findByPath: findByPath
        };
        return retVal;
    }])
    .directive('vaMainMenu', ["$document",
        function ($document) {
            return {
                restrict: 'E',
                replace: true,
                transclude: true,
                require: 'ngModel',
                scope: {
                    onMenuChanged: "&"
                },
                templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu.tpl.html',
                link: function (scope, element, attr, ngModelController, linker) {
                    scope.menu = ngModelController.$modelValue;
                    ngModelController.$render = function () {
                        scope.menu = ngModelController.$modelValue;
                        updateFavorites();
                        scope.$watch(function () {
                            return _.filter(scope.menu.items, function (x) { return x.isFavorite && !x.isAlwaysOnBar; });
                        }, function () { updateFavorites(); }, true);
                    };

                    scope.selectItem = function (menuItem) {
                        if (scope.showSubMenu && scope.currentMenuItem === menuItem) {
                            scope.showSubMenu = false;
                        } else {
                            scope.currentMenuItem = menuItem;
                            scope.showSubMenu = menuItem.children.length > 0 || menuItem.path === "more";
                        }
                        if (angular.isDefined(menuItem.action)) {
                            menuItem.action();
                        }
                    };

                    function handleKeyUpEvent(event) {
                        if (scope.showSubMenu && event.keyCode === 27) {
                            scope.$apply(function () {
                                scope.showSubMenu = false;
                            });
                        }
                    }

                    function handleClickEvent(event) {
                        var dropdownElement = $document.find('.nav-bar .dropdown');
                        var hadDropdownElement = $document.find('.__has-dropdown');
                        if (scope.showSubMenu && !(dropdownElement.is(event.target) || dropdownElement.has(event.target).length > 0 ||
                            hadDropdownElement.is(event.target) || hadDropdownElement.has(event.target).length > 0)) {
                            scope.$apply(function () {
                                scope.showSubMenu = false;
                            });
                        }
                    }

                    $document.bind('keyup', handleKeyUpEvent);
                    $document.bind('click', handleClickEvent);

                    scope.$on('$destroy', function () {
                        $document.unbind('keyup', handleKeyUpEvent);
                        $document.unbind('click', handleClickEvent);
                    });

                    // required by ui-sortable: we can't use filters with it
                    // https://github.com/angular-ui/ui-sortable#usage
                    function updateFavorites() {
                        scope.dynamicMenuItems = _.sortBy(_.filter(scope.menu.items, function (x) { return x.isFavorite || x.path === "more"; }), function (x) { return x.order; });
                        raiseOnMenuChanged();
                    }

                    function raiseOnMenuChanged() {
                        _.throttle(scope.onMenuChanged({ menu: scope.menu }), 100);
                    }

                    scope.toggleCollapsed = function () {
                        scope.menu.isCollapsed = !scope.menu.isCollapsed;
                        raiseOnMenuChanged();
                    };

                    scope.toggleFavorite = function (menuItem) {
                        menuItem.isFavorite = !menuItem.isFavorite;
                        // clear order when removed from favorites
                        if (!menuItem.isFavorite) {
                            menuItem.order = Number.MAX_SAFE_INTEGER;
                        }
                        var favorites = _.sortBy(_.filter(scope.menu.items, function (x) { return x.isFavorite }), function (x) { return x.order; });
                        // re-calculate order
                        for (var i = 0; i < favorites.length; i++) {
                            favorites[i].order = i;
                        }
                        // Do not call the callback function to notify what favorites changed.
                        // We're already do that because we call updateFavorites (which call the calback) in ngModelController.$render
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
