angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemVariationWidgetController', ['$injector', '$rootScope', '$scope', 'bladeNavigationService', function ($injector, $rootScope, $scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openItemDetailBlade = function (variation, $event) {
        if ($event.stopPropagation) $event.stopPropagation();
        var blade = {
            id: 'variationDetail',
            itemId: variation.id,
            title: variation.code,
            subtitle: 'Item variation',
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

    $scope.openVariationListBlade = function () {

        var blade = {
            id: "itemVariationList",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'Item variations',
            controller: 'virtoCommerce.catalogModule.itemVariationListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-variation-list.tpl.html',
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
