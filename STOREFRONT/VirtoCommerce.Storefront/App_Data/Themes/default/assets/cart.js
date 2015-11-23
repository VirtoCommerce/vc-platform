angular.module('cartApp', ['ngResource'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json', null, { get: { method: 'GET', url: '/cart/json' } });
    }])

    .controller('cartController', ['$scope', '$location', 'cartApi', function ($scope, $location, cartApi) {
        cartApi.get(null, function (jsonCart) {
            $scope.cart = jsonCart;
            $scope.checkoutStep = $location.search();
        });
    }])

    .config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider) {
        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);