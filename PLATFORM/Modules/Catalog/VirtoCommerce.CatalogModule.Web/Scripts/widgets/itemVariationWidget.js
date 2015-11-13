angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemVariationWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.openVariationListBlade = function () {
        var blade = {
            id: "itemVariationList",
            itemId: $scope.blade.item.id,
            title: $scope.blade.origItem.name,
            subtitle: 'catalog.blades.variations.subtitle',
            controller: 'virtoCommerce.catalogModule.itemVariationListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-variation-list.tpl.html',
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);
