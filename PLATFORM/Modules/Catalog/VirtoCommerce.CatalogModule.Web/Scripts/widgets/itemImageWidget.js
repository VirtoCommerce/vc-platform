angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemImageWidgetController', ['$injector', '$rootScope', '$scope', 'platformWebApp.bladeNavigationService', function ($injector, $rootScope, $scope, bladeNavigationService) {

    $scope.currentBlade = $scope.widget.blade;

    $scope.openItemImageBlade = function () {
        var blade = {
            id: "itemImage",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'item images',
            controller: 'virtoCommerce.catalogModule.itemImageController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-image-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
