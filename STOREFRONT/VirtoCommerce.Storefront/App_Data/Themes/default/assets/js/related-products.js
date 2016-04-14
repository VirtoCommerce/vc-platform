var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('relatedProductsController', ['$scope', '$window', 'catalogService', 'pricingService', function ($scope, $window, catalogService, pricingService) {
    init();

    function init() {
        $scope.relatedProducts = [];
        $scope.productPrices = [];
        $scope.relatedProductsLoaded = false;
        getRelatedProducts($window.relatedProducts);
    }

    function getRelatedProducts(products) {
        var productIds = _.map(products, function (rp) { return rp.id });
        catalogService.getProduct(productIds).then(function (response) {
            var relatedProducts = response.data;
            for (var i = 0; i < relatedProducts.length; i++) {
                $scope.relatedProducts[relatedProducts[i].id] = relatedProducts[i];
                $scope.relatedProducts[relatedProducts[i].id].url = relatedProducts[i].url.replace('~/', '');
                var relatedProductForGettingPrice = _.find(products, function (p) { return p.id === relatedProducts[i].id });
                relatedProductForGettingPrice.catalogId = relatedProducts[i].catalogId;
                relatedProductForGettingPrice.categoryId = relatedProducts[i].categoryId;
            }
            $scope.relatedProductsLoaded = true;
            getActualProductPrices(products);
        });
    }

    function getActualProductPrices(products) {
        pricingService.getActualProductPrices(products).then(function (response) {
            var prices = response.data;
            if (prices.length) {
                for (var i = 0; i < prices.length; i++) {
                    $scope.productPrices[prices[i].productId] = prices[i];
                }
            }
            var productPricesSize = $scope.getObjectSize($scope.productPrices);
            $scope.productPricesLoaded = productPricesSize > 0;
        });
    }
}]);