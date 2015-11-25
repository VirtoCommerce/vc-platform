angular.module('cartApp', ['ngResource', 'ngRoute'])
    .factory('cartApi', ['$resource', function ($resource) {
        return $resource('/cart/json');
    }])

    .controller('cartController', ['$scope', '$route', 'cartApi', function ($scope, $route, cartApi) {
        $scope.$route = $route;
        $scope.cartModalToggled = false;
        $scope.cart = cartApi.get();
        $scope.changeLineItem = function (lineItemId, quantity) {
            var lineItem = _.where($scope.cart.Items, { Id: lineItemId });
            if (lineItem) {
                if (quantity >= 1) {
                    lineItem[0].Quantity = quantity;
                }
            }
        }
        $scope.removeLineItem = function (lineItemId) {
            var lineItem = _.where($scope.cart.Items, { Id: lineItemId });
            if (lineItem) {
                $scope.cart.Items = _.without($scope.cart.Items, lineItem[0]);
                $scope.updateCart();
            }
        }
        $scope.updateCart = function () {
            $scope.cart.$save();
        }
    }])

    .controller('customerInformationController', function () {
    })

    .controller('shippingMethodController', function () {
    })

    .controller('paymentMethodController', function () {
    })

    .config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider, $routeProvider, $locationProvider) {
        $routeProvider
            .when('/cart/checkout/customer-information', {
                controller: 'customerInformationController',
                templateUrl: 'checkout.customerInformation.tpl'
            })
            .when('/cart/checkout/shipping-method', {
                controller: 'shippingMethodController',
                templateUrl: 'checkout.shippingMethod.tpl'
            })
            .when('/cart/checkout/payment-method', {
                controller: 'paymentMethodController',
                templateUrl: 'checkout.paymentMethod.tpl'
            });

        $locationProvider.html5Mode(true);

        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);