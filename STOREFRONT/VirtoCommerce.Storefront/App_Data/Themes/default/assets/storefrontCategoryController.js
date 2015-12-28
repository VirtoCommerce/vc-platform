﻿var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('categoryController', ['$scope', '$window', 'marketingService', function ($scope, $window, marketingService) {
    $scope.productPricesLoaded = false;
    $scope.productPrices = [];

    marketingService.getActualProductPrices($window.products).then(function (response) {
        var prices = response.data;
        if (prices.length) {
            for (var i = 0; i < prices.length; i++) {
                $scope.productPrices[prices[i].ProductId] = prices[i];
            }
        }
        var productPricesSize = $scope.getObjectSize($scope.productPrices);
        $scope.productPricesLoaded = productPricesSize > 0;
    });
}]);