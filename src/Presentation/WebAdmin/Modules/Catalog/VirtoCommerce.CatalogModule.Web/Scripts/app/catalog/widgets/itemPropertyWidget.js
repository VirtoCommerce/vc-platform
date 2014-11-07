angular.module('catalogModule.widget.itemPropertyWidget', [
])
.controller('itemPropertyWidgetController', ['$injector', '$rootScope', '$scope', 'bladeNavigationService', function ($injector, $rootScope, $scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.propertyShowLimit = 5;

    $scope.openItemPropertyBlade = function () {

        var blade = {
            id: "itemProperty",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'item properties',
            controller: 'itemPropertyController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
