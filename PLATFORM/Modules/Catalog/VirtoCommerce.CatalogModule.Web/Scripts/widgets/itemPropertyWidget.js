angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    $scope.propertiesCount = '...';

    $scope.$watch('blade.item', function (product) {
        if (product)
            $scope.propertiesCount = _.filter(product.properties, function (x) { return x.type == 'Product' || x.type == 'Variation'; }).length;
    });

    $scope.openItemPropertyBlade = function () {
        var newBlade = {
            id: "itemProperty",
            itemId: blade.item.id,
            title: blade.origItem.name,
            subtitle: 'catalog.widgets.itemProperty.blade-subtitle',
            controller: 'virtoCommerce.catalogModule.itemPropertyListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
