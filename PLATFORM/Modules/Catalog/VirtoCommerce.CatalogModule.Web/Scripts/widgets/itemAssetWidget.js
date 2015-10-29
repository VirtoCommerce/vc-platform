angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssetWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "itemAsset",
            itemId: blade.item.id,
            title: blade.origItem.name,
            subtitle: 'item assets',
            controller: 'virtoCommerce.catalogModule.itemAssetController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-asset-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

}]);
