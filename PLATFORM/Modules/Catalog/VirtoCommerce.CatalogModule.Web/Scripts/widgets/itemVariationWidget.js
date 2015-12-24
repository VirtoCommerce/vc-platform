angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemVariationWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openVariationListBlade = function () {
        var newBlade = {
            id: "itemVariationList",
            itemId: blade.item.id,
            title: blade.origItem.name,
            subtitle: 'catalog.widgets.itemVariation.blade-subtitle',
            toolbarCommandsAndEvents: blade.variationsToolbarCommandsAndEvents,
            controller: 'virtoCommerce.catalogModule.itemVariationListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-variation-list.tpl.html',
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
