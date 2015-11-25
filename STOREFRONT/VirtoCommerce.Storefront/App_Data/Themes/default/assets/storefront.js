angular.module('storefrontApp', ['ngResource', 'ngRoute'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json');
    }])

    .directive('floatingLabel', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                if (element[0].tagName !== 'input'.toUpperCase()) {
                    return;
                }
                var targetElement = element[0].parentElement;
                var className = ' field--show-floating-label';
                scope.$watch(attrs.ngModel, function (value) {
                    targetElement.className = targetElement.className.replace(className, '');
                    if (value) {
                        targetElement.className += className;
                    }
                });
            }
        }
    })

    .controller('storefrontController', ['$scope', 'cartApi', function ($scope, cartApi) {
        $scope.cart = {};
        $scope.cartModalToggled = false;
        cartApi.get(null, function (data) {
            $scope.cart = data;
        });
        $scope.toggleCartModal = function (toggle) {
            $scope.cartModalToggled = !toggle;
        }
    }])

    .controller('cartController', ['$scope', '$http', 'cartApi', function ($scope, $http, cartApi) {
        $scope.cart = {};
        cartApi.get(null, function (data) {
            $scope.cart = data;
        });
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

    .controller('checkoutController', ['$scope', '$route', '$location', 'cartApi', function ($scope, $route, $location, cartApi) {
        $scope.$route = $route;
        $scope.cart = {};
        $scope.shippingAddress = {
            AddressType: 2
        };
        $scope.billingAddress = {
            AddressType: 1
        };
        cartApi.get(null, function (data) {
            $scope.cart = data;
            var shippingAddresses = _.where($scope.cart.Addresses, { AddressType: 2 });
            if (shippingAddresses.length) {
                $scope.shippingAddress = shippingAddresses[0];
            }
            var billingAddresses = _.where($scope.cart.Addresses, { AddressType: 1 });
            if (billingAddresses.length) {
                $scope.billingAddress = billingAddresses[0];
            }
        });
        $scope.setShippingAddress = function () {
            $http.post('/cart/checkout/shipping_address', { address: $scope.shippingAddress })
                .success(function (jsonCart) {
                    $scope.cart = jsonCart;
                    $scope.go('/cart/checkout/shipping-method');
                });
        }
        $scope.go = function (url) {
            $location.path(url);
        }
    }])

    .config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider, $routeProvider, $locationProvider) {
        $routeProvider
            .when('/cart/checkout/customer-information', {
                templateUrl: 'storefront.checkout.customerInformation.tpl'
            })
            .when('/cart/checkout/shipping-method', {
                templateUrl: 'storefront.checkout.shippingMethod.tpl'
            })
            .when('/cart/checkout/payment-method', {
                templateUrl: 'storefront.checkout.paymentMethod.tpl'
            });

        $locationProvider.html5Mode(true);

        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);