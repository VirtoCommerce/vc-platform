var app = angular.module('storefrontApp', ['ngRoute']);

app.service('cartService', ['$http', function ($http) {
    return {
        getCart: function () {
            return $http.get('cart/json').then(function (response) {
                return response.data;
            });
        },
        addLineItem: function (productId, quantity) {
            return $http.post('cart/add_item', { productId: productId, quantity: quantity }).then(function (response) {
                return response.data;
            });
        },
        changeLineItem: function (lineItemId, quantity) {
            return $http.post('cart/change_item', { lineItemId: lineItemId, quantity: quantity }).then(function (response) {
                return response.data;
            });
        },
        removeLineItem: function (lineItemId) {
            return $http.post('cart/remove_item', { lineItemId: lineItemId }).then(function (response) {
                return response.data;
            });
        },
        addAddress: function (address) {
            return $http.post('cart/add_address', { address: address }).then(function (response) {
                return response.data;
            });
        },
        getShippingMethods: function (cartId) {
            return $http.get('cart/' + cartId + '/shipping_methods/json').then(function (response) {
                return response.data;
            });
        },
        getPaymentMethods: function (cartId) {
            return $http.get('cart/' + cartId + '/payment_methods/json').then(function (response) {
                return response.data;
            });
        },
        setShippingMethod: function (shippingMethodCode) {
            return $http.post('cart/shipping_method', { shippingMethodCode: shippingMethodCode }).then(function (response) {
                return response.data;
            });
        },
        setPaymentMethod: function (paymentMethodCode) {
            return $http.post('cart/payment_method', { paymentMethodCode: paymentMethodCode }).then(function (response) {
                return response.data;
            });
        },
        createOrder: function (cartId) {
            return $http.post('cart/' + cartId + '/create_order').then(function (response) {
                return response.data;
            });
        },
        processPayment: function (orderId, paymentId) {
            return $http.post('cart/process_payment', { orderId: orderId, paymentId: paymentId }).then(function (response) {
                return response.data;
            });
        }
    }
}]);

app.directive('floatingLabel', function () {
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
});

app.controller('mainController', ['$scope', '$window', 'cartService', function ($scope, $window, cartService) {
    $scope.cart = null;
    cartService.getCart().then(function (response) {
        $scope.cart = response;
    });

    $scope.isCartModalVisible = false;
    $scope.showCartModal = function () {
        $scope.isCartModalVisible = true;
    }

    $scope.go = function (url) {
        $window.location.href = url;
    }
}]);

app.controller('cartModalController', ['$scope', 'cartService', function ($scope, cartService) {
    $scope.changeLineItem = function (lineItemId, quantity) {
        if (quantity >= 1) {
            cartService.changeLineItem(lineItemId, quantity).then(function (response) {
                $scope.$parent.cart = response;
            });
        }
    }
    $scope.removeLineItem = function (lineItemId) {
        cartService.removeLineItem(lineItemId).then(function (response) {
            $scope.cart = response;
            $scope.$parent.cart = response;
        });
    }
    $scope.closeModal = function () {
        $scope.$parent.isCartModalVisible = false;
    }
}]);

app.controller('productController', ['$scope', 'cartService', function ($scope, cartService) {
    $scope.addToCart = function (productId, quantity) {
        if (quantity >= 1) {
            cartService.addLineItem(productId, quantity).then(function (response) {
                $scope.$parent.cart = response;
                $scope.$parent.isCartModalVisible = true;
            });
        }
    }
}]);

app.controller('cartController', ['$scope', 'cartService', function ($scope, cartService) {
    $scope.changeLineItem = function (lineItemId, quantity) {
        if (quantity >= 1) {
            cartService.changeLineItem(lineItemId, quantity).then(function (response) {
                $scope.$parent.cart = response;
            });
        }
    }
    $scope.removeLineItem = function (lineItemId) {
        cartService.removeLineItem(lineItemId).then(function (response) {
            $scope.cart = response;
            $scope.$parent.cart = response;
        });
    }
}]);

app.controller('checkoutController', ['$scope', '$route', '$location', 'cartService', function ($scope, $route, $location, cartService) {
    $scope.$route = $route;
    $scope.shippingAddress = {};
    $scope.availableShippingMethods = [];
    $scope.availablePaymentMethods = [];
    $scope.selectedShippingMethod = {};
    $scope.selectedPaymentMethod = {};
    $scope.order = {};

    $scope.cart = {};
    cartService.getCart().then(function (response) {
        $scope.cart = response;
        var shippingAddresses = _.where($scope.cart.Addresses, { Type: "Shipping" });
        if (shippingAddresses.length) {
            $scope.shippingAddress = shippingAddresses[0];
        }
        cartService.getShippingMethods($scope.cart.Id).then(function (response) {
            $scope.availableShippingMethods = response;
            if ($scope.cart.Shipments.length) {
                for (var i = 0; i < $scope.availableShippingMethods.length; i++) {
                    var availableShippingMethod = $scope.availableShippingMethods[i];
                    var shipments = _.where($scope.cart.Shipments, { ShipmentMethodCode: availableShippingMethod.ShipmentMethodCode });
                    if (shipments.length) {
                        $scope.selectedShippingMethod = availableShippingMethod;
                        break;
                    }
                }
            }
        });
        cartService.getPaymentMethods($scope.cart.Id).then(function (response) {
            $scope.availablePaymentMethods = response;
            if ($scope.cart.Payments.length) {
                for (var i = 0; i < cart.Payments.length; i++) {
                    var availablePaymentMethod = $scope.availablePaymentMethods[i];
                    var payments = _.where($scope.cart.Payments, { PaymentGatewayCode: availableShippingMethod.GatewayCode });
                    if (payments.length) {
                        $scope.selectedPaymentMethod = availablePaymentMethod;
                        break;
                    }
                }
            }
        });
    });

    $scope.setShippingAddress = function () {
        cartService.addAddress($scope.shippingAddress).then(function (response) {
            $scope.cart = response;
            $location.path('/cart/checkout/shipping-method');
        });
    }

    $scope.setShippingMethod = function (shippingMethodCode) {
        cartService.setShippingMethod(shippingMethodCode).then(function (response) {
            $scope.cart = response;
        });
    }

    $scope.setShippingMethods = function () {
        $location.path('/cart/checkout/payment-method');
    }

    $scope.setPaymentMethods = function (paymentMethodCode) {
        cartService.setPaymentMethod(paymentMethodCode).then(function (response) {
            $scope.cart = response;
            cartService.createOrder($scope.cart.Id).then(function (response) {
                $scope.order = response;
                if ($scope.order.InPayments.length) {
                    var payment = $scope.order.InPayments[0];
                    cartService.processPayment($scope.order.Id, payment.Id).then(function (response) {
                    });
                }
            });
        });
    }

    $scope.createOrder = function () {
    }
}]);

app.config(['$interpolateProvider', '$routeProvider', '$locationProvider', '$httpProvider', function ($interpolateProvider, $routeProvider, $locationProvider, $httpProvider) {
    $httpProvider.defaults.cache = false;
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }
    $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';

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