angular.module('catalogModule.widget.itemVariationWidget', [
])
.controller('itemVariationWidgetController', ['$injector', '$rootScope', '$scope', 'bladeNavigationService', function ($injector, $rootScope, $scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openItemDetailBlade = function (variation, $event) {
        if ($event.stopPropagation) $event.stopPropagation();
        var blade = {
            id: 'variationDetail',
            itemId: variation.id,
            title: variation.code,
            style: 'gray',
            subtitle: 'Item variation',
            controller: 'itemDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

    $scope.openVariationListBlade = function () {

        var blade = {
            id: "itemVariationList",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'Item variations',
            controller: 'itemVariationListController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-variation-list.tpl.html',
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
