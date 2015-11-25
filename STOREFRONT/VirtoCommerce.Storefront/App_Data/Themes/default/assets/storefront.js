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

    .config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider, $routeProvider, $locationProvider) {

        $locationProvider.html5Mode(true);

        return $interpolateProvider.startSymbol('{(').endSymbol(')}');
    }]);