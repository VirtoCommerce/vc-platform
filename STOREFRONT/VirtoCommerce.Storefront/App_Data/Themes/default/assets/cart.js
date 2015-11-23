angular.module('cartApp', ['ngResource'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json', null, { get: { method: 'GET', url: '/cart/json' } });
    }])

    .controller('cartController', ['$scope', '$location', 'cartApi', function ($scope, $location, cartApi) {
        cartApi.get(null, function (jsonCart) {
            $scope.cart = jsonCart;
            $scope.location = $location;
        });
    }])

    .config(['$interpolateProvider', '$locationProvider', function ($interpolateProvider, $locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);