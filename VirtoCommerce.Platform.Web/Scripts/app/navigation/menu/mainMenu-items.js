angular.module("platformWebApp")
.directive("vaMainMenuItems", ["$filter",
    function ($filter) {

    return {
        restrict: "E",
        replace: true,
        scope: {
            items: "=*",
            selectItem: "=",
            showTooltip: "=",
            showList: "="
        },
        templateUrl: "$(Platform)/Scripts/app/navigation/menu/mainMenu-items.tpl.html",
        link: function (scope) {
            updateFavorites(scope);
            scope.$watchCollection("items", function(newMainMenuItems, oldMainMenuItems) {
                angular.forEach(_.without(newMainMenuItems, oldMainMenuItems), function(menuItem) {
                    scope.$watch(function() { return menuItem; }, function() {
                        updateFavorites();
                    }, true);
                });
                updateFavorites();
            });
            // required by ui-sortable: we can't use filters with it
            // https://github.com/angular-ui/ui-sortable#usage
            function updateFavorites() {
                scope.favorites = $filter("orderBy")($filter("filter")(scope.items, { isFavorite: "true" }), "order");
            }

            scope.sortableOptions = {
                axis: "y",
                cursor: "move",
                // always use container with tolerance
                // because otherwise draggable item can't replace top item:
                // where is no space between top item and container top border
                containment: ".ui-sortable",
                items: ".__movable",
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
}]);