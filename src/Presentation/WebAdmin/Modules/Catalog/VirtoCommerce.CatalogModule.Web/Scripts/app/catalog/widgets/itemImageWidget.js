angular.module('catalogModule.widget.itemImageWidget', [
])
.controller('itemImageWidgetController', ['$injector', '$rootScope', '$scope', 'bladeNavigationService', function ($injector, $rootScope, $scope, bladeNavigationService) {

    $scope.currentBlade = $scope.widget.blade;

    $scope.openItemImageBlade = function () {
        var blade = {
            id: "itemImage",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'item images',
            controller: 'itemImageController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-image-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
