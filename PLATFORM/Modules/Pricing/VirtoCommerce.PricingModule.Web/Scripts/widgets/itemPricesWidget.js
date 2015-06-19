angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.itemPricesWidgetController', ['$scope', '$filter', 'platformWebApp.bladeNavigationService', 'virtoCommerce.pricingModule.prices', function ($scope, $filter, bladeNavigationService, prices) {
    var blade = $scope.blade;

    function refresh() {
        $scope.priceRange = '';

        return prices.query({ id: blade.itemId }, function (data) {
            // find the most popular currency and min/max prices in it.
            var pricelists = _.flatten(_.pluck(data, 'productPrices'), true);
            var prices = _.flatten(_.pluck(pricelists, 'prices'), true);
            if (prices.length) {
                prices = _.groupBy(prices, 'currency');
                prices = _.max(_.values(prices), function (x) { return x.length; });
                var allPrices = _.union(_.pluck(prices, 'list'), _.pluck(prices, 'sale'));
                var minprice = _.min(allPrices);
                var maxprice = _.max(allPrices);
                var currency = prices.length ? ' ' + prices[0].currency : '';
                minprice = $filter('number')(minprice, 2);
                maxprice = $filter('number')(maxprice, 2);
                $scope.priceRange = (minprice == maxprice ? minprice : minprice + '-' + maxprice) + currency;
            } else {
                $scope.priceRange = 'N/A';
            }
        }, function (error) {
            //bladeNavigationService.setError('Error ' + error.status, blade);
        });
    }

    $scope.openBlade = function () {
        if ($scope.priceRange !== '') {
            var newBlade = {
                id: "itemPricelists",
                itemId: blade.itemId,
                parentWidgetRefresh: $scope.refresh,
                title: blade.title,
                subtitle: 'Select Price list to manage prices',
                controller: 'virtoCommerce.pricingModule.itemPricelistsListController',
                template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/item/item-pricelists-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    };

    refresh();
}]);