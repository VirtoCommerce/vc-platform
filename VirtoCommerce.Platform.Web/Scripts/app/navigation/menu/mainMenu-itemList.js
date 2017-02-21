angular.module('platformWebApp')
.directive('vaMainMenuItemList', [function () {
return {
    restrict: 'E',
    replace: true,
    scope: {
        isOpen: "=",
        items: "=*",
        selectItem: "="
    },
    templateUrl: '$(Platform)/Scripts/app/navigation/menu/mainMenu-itemList-list.tpl.html',
    link: function (scope) {
        scope.toggleFavorite = function(menuItem) {
            menuItem.isFavorite = !menuItem.isFavorite;
            // clear order when removed from favorites
            if (!menuItem.isFavorite) {
                menuItem.order = undefined;
            }
            var favorites = _.filter(_.sortBy(scope.items, function (menuItem) { return menuItem.order; }), function (menuItem) { return menuItem.isFavorite; });
            // re-calculate order
            for (var i = 0; i < favorites.length; i++) {
                favorites[i].order = i;
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
                    $(e.target).prev(".list-fav").click();
                }
            });
        }
    }
});