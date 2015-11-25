angular.module('storefrontApp', ['ngResource', 'ngRoute'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json');
    }])

    .controller('storefrontController', ['$scope', 'cartApi', function ($scope, cartApi) {
        $scope.cartModalToggled = false;
        $scope.cart = cartApi.get();
        $scope.toggleCartModal = function (toggle) {
            $scope.cartModalToggled = !toggle;
        }
    }])

    .controller('cartController', ['$scope', '$http', 'cartApi', function ($scope, $http, cartApi) {
        $scope.cart = cartApi.get();
        $scope.addLineItem = function (productId, quantity) {
            $http.post('/cart/additem', { id: productId, quantity: quantity })
                .success(function (jsonCart) {
                    $scope.cart = jsonCart;
                });
        }
        $scope.changeLineItem = function (lineItemId, quantity) {
            if (quantity >= 1) {
                $http.post('/cart/changeitem', { id: lineItemId, quantity: quantity })
                    .success(function (jsonCart) {
                        $scope.cart = jsonCart;
                    });
            }
        }
        $scope.removeLineItem = function (lineItemId) {
            $http.post('/cart/removeitem', { id: lineItemId })
                .success(function (jsonCart) {
                    $scope.cart = jsonCart;
                });
        }
    }])

    .controller('checkoutController', ['$scope', '$route', 'cartApi', function ($scope, $route, cartApi) {
        $scope.$route = $route;
        $scope.cart = cartApi.get();
    }])

    .controller('checkoutCustomerInformationController', ['$scope', function ($scope) {
    }])

    .controller('checkoutShippingMethodController', ['$scope', function ($scope) {
    }])

    .controller('checkoutPaymentMethodController', ['$scope', function ($scope) {
    }])

    .config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider, $routeProvider, $locationProvider) {
        $routeProvider
            .when('/cart/checkout/customer-information', {
                controller: 'checkoutCustomerInformationController',
                templateUrl: 'storefront.checkout.customerInformation.tpl'
            })
            .when('/cart/checkout/shipping-method', {
                controller: 'checkoutShippingMethodController',
                templateUrl: 'storefront.checkout.shippingMethod.tpl'
            })
            .when('/cart/checkout/payment-method', {
                controller: 'checkoutPaymentMethodController',
                templateUrl: 'storefront.checkout.paymentMethod.tpl'
            });

        $locationProvider.html5Mode(true);

        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);