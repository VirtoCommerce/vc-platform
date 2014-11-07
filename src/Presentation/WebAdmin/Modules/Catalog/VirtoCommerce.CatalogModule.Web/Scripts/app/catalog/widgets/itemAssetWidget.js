angular.module('catalogModule.widget.itemAssetWidget', [
])
.controller('itemAssetWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "itemAsset",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'item assets',
            controller: 'itemAssetController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-asset-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
