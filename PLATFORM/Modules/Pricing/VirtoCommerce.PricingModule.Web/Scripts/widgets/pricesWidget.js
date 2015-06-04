angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.getPriceCount = function () {
        var retVal;
        // all prices count
        if ($scope.currentBlade.currentEntity) {
            var pricelistPrices = _.flatten(_.pluck($scope.currentBlade.currentEntity.productPrices, 'prices'), true);
            retVal = pricelistPrices.length;
        } else {
            retVal = '';
        }
        return retVal;
    }

    $scope.openBlade = function () {
        var blade = {
            id: "pricelistChild",
            currency: $scope.currentBlade.currentEntity.currency,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Manage prices',
            controller: 'virtoCommerce.pricingModule.pricelistItemListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-item-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };
}]);