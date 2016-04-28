var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('productListController', ['$scope', '$window', 'pricingService', function ($scope, $window, pricingService) {
    $scope.productListPricesLoaded = false;
    $scope.productListPrices = [];

    pricingService.getActualProductPrices($window.productList).then(function (response) {
        var prices = response.data;
        if (prices.length) {
            for (var i = 0; i < prices.length; i++) {
                $scope.productListPrices[prices[i].productId] = prices[i];
            }
        }
        var productListPricesSize = $scope.getObjectSize($scope.productListPrices);
        $scope.productListPricesLoaded = productListPricesSize > 0;
    });
}]);