angular.module('catalogModule.widget.itemAssociationsWidget', [])
.controller('itemAssociationsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "associationsList",
            currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntities: $scope.currentBlade.item.associations,
            title: $scope.currentBlade.title,
            subtitle: 'Associations',
            controller: 'itemAssociationsListController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-associations-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };
}]);
