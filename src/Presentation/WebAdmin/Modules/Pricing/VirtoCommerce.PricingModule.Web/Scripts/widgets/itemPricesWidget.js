angular.module('virtoCommerce.pricingModule.widget.itemPricesWidget', [
    'virtoCommerce.pricingModule.resources.pricing'
])
.controller('itemPricesWidgetController', ['$scope', 'bladeNavigationService', 'prices', function ($scope, bladeNavigationService, prices) {
    $scope.currentBlade = $scope.widget.blade;
    $scope.elementCount = '...';

    $scope.openBlade = function () {
        if ($scope.elementCount !== '...') {
            var newBlade = {
                id: "itemPrices",
                itemId: $scope.currentBlade.itemId,
                title: $scope.currentBlade.title,
                subtitle: 'Manage prices',
                controller: 'itemPricesListController',
                template: 'Modules/Pricing/VirtoCommerce.PricingModule.Web/Scripts/blades/item-prices-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.currentBlade);
        }
    };

    prices.query({ id: $scope.currentBlade.itemId }, function (data) {
        $scope.elementCount = data.length;
    });

}]);
