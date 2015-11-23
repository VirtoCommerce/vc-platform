angular.module('cartApp', ['ngResource'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json', null, { get: { method: 'GET', url: '/cart/json' } });
    }])

    .controller('cartController', ['$scope', '$location', 'cartApi', function ($scope, $location, cartApi) {
        cartApi.get(null, function (jsonCart) {
            $scope.cart = jsonCart;
            $scope.isCustomerInformationStep = $location.path().indexOf('/cart/customer-information') >= 0;
            $scope.isShippingMethodStep = $location.path().indexOf('/cart/shipping-method') >= 0;
            $scope.isPaymentMethodStep = $location.path().indexOf('/cart/payment-method') >= 0;
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