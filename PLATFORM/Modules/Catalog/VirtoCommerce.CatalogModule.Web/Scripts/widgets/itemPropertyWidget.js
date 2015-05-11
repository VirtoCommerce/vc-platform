angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;
    $scope.propertiesCount = 'calculating';

    $scope.$watch('widget.blade.item', function (product) {
    	$scope.propertiesCount = _.filter(product.properties, function (x) { return (x.type == 'Product' || x.type == 'Variation') }).length;
    });

    $scope.openItemPropertyBlade = function () {

        var blade = {
            id: "itemProperty",
            itemId: $scope.currentBlade.item.id,
            title: $scope.currentBlade.origItem.name,
            subtitle: 'item properties',
            controller: 'virtoCommerce.catalogModule.itemPropertyController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
