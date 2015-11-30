angular.module('storefrontApp', ['ngRoute'])
.service('cartService', ['$http', function ($http) {
    return {
        getCart: function () {
            return $http.get('cart/json');
        },
        addLineItem: function (productId, quantity) {
            return $http.post('cart/add_item', { productId: productId, quantity: quantity });
        },
        changeLineItem: function (lineItemId, quantity) {
            return $http.post('cart/change_item', { lineItemId: lineItemId, quantity: quantity });
        },
        removeLineItem: function (lineItemId) {
            return $http.post('cart/remove_item', { lineItemId: lineItemId });
        },
        addCoupon: function (couponCode) {
            return $http.post('cart/add_coupon/' + couponCode);
        },
        removeCoupon: function () {
            return $http.post('cart/remove_coupon');
        },
        addAddress: function (address) {
            return $http.post('cart/add_address', { address: address });
        },
        getAvailableShippingMethods: function (cartId) {
            return $http.get('cart/' + cartId + '/shipping_methods/json');
        },
        getAvailablePaymentMethods: function (cartId) {
            return $http.get('cart/' + cartId + '/payment_methods/json');
        },
        setShippingMethod: function (shippingMethodCode) {
            return $http.post('cart/shipping_method', { shippingMethodCode: shippingMethodCode });
        },
        setPaymentMethod: function (paymentMethodCode, billingAddress) {
            return $http.post('cart/payment_method', { paymentMethodCode: paymentMethodCode, billingAddress: billingAddress });
        },
        createOrder: function (cartId) {
            return $http.post('cart/' + cartId + '/create_order');
        },
        processPayment: function (orderId, paymentId) {
            return $http.post('cart/process_payment', { orderId: orderId, paymentId: paymentId });
        }
    }
}])

.controller('mainController', ['$scope', '$window', function ($scope, $window) {
    $scope.go = function (url) {
        $window.location.href = url;
    }
}])

.controller('cartController', ['$scope', 'cartService', function ($scope, cartService) {
    $scope.cart = null;
    $scope.isCartModalVisible = false;

    var cartPromise = cartService.getCart();
    cartPromise.then(function (response) {
        $scope.cart = response.data;
    });

    $scope.showCartModal = function () {
        $scope.isCartModalVisible = true;
    }
    $scope.hideCartModal = function () {
        $scope.isCartModalVisible = false;
    }
    $scope.addToCart = function (productId, quantity) {
        cartService.addLineItem(productId, quantity).then(function (response) {
            $scope.cart = response.data;
            $scope.isCartModalVisible = true;
        });
    }
    $scope.changeLineItem = function (lineItemId, quantity) {
        if (quantity >= 1) {
            cartService.changeLineItem(lineItemId, quantity).then(function (response) {
                $scope.cart = response.data;
            });
        }
    }
    $scope.removeLineItem = function (lineItemId) {
        cartService.removeLineItem(lineItemId).then(function (response) {
            $scope.cart = response.data;
        });
    }
}])

.controller('checkoutController', ['$scope', '$route', '$window', 'cartService', function ($scope, $route, $window, cartService) {
    $scope.$route = $route;
    $scope.cart = null;
    $scope.order = null;
    $scope.couponProcessing = false;
    $scope.couponWasAdded = false;
    $scope.couponHasError = false;
    $scope.shippingAddress = {};
    $scope.billingAddress = {};
    $scope.billingAddressEqualsShipping = true;
    $scope.availableShippingMethods = null;
    $scope.selectedShippingMethod = {};
    $scope.availablePaymentMethods = null;
    $scope.selectedPaymentMethod = {};
    $scope.isOrderSummaryExpanded = false;
    $scope.orderProcessing = false;

    var cartPromise = cartService.getCart();
    cartPromise.then(function (response) {
        var cart = response.data;
        $scope.cart = cart;
        $scope.couponCode = cart.Coupon;
        $scope.couponWasAdded = cart.Coupon.length;

        var shippingAddresses = _.where(cart.Addresses, { Type: "Shipping" });
        if (shippingAddresses.length) {
            $scope.shippingAddress = shippingAddresses[0];
        }

        var billingAddresses = _.where(cart.Addresses, { Type: "Billing" });
        if (billingAddresses.length) {
            $scope.billingAddress = billingAddresses[0];
        }

        $scope.setBillingAddressEqualsShipping();

        var availableShippingMethodsPromise = cartService.getAvailableShippingMethods(cart.Id);
        availableShippingMethodsPromise.then(function (response) {
            var availableShippingMethods = response.data;
            $scope.availableShippingMethods = availableShippingMethods;
            var shippingMethod = cart.Shipments.length ? cart.Shipments[0] : null;
            if (shippingMethod) {
                var matchedShippingMethods = _.where(availableShippingMethods, { ShipmentMethodCode: shippingMethod.ShipmentMethodCode });
                $scope.selectedShippingMethod = matchedShippingMethods.length ? matchedShippingMethods[0] : {};
            }
        });

        var availablePaymentMethodsPromise = cartService.getAvailablePaymentMethods(cart.Id);
        availablePaymentMethodsPromise.then(function (response) {
            $scope.availablePaymentMethods = response.data;
        });
    });

    $scope.go = function (url) {
        $window.location.href = url;
    }
    $scope.toggleOrderSummary = function (isExpanded) {
        $scope.isOrderSummaryExpanded = !isExpanded;
    }
    $scope.addCoupon = function (couponCode) {
        $scope.couponProcessing = true;
        var cartPromise = cartService.addCoupon(couponCode);
        cartPromise.then(function (response) {
            var cart = response.data;
            $scope.couponWasAdded = true;
            $scope.couponHasError = cart.DiscountTotal.Amount == $scope.cart.DiscountTotal.Amount;
            $scope.cart = cart;
            $scope.couponProcessing = false;
        });
    }
    $scope.removeCoupon = function () {
        $scope.couponProcessing = true;
        var cartPromise = cartService.removeCoupon();
        cartPromise.then(function (response) {
            var cart = response.data;
            $scope.couponWasAdded = false;
            $scope.couponHasError = false;
            $scope.cart = cart;
            $scope.couponProcessing = false;
        });
    }
    $scope.setShippingAddress = function () {
        var cartPromise = cartService.addAddress($scope.shippingAddress);
        cartPromise.then(function (response) {
            $scope.cart = response.data;
            $scope.go('shipping-method');
        });
    }
    $scope.setShippingMethod = function () {
        var cartPromise = cartService.setShippingMethod($scope.selectedShippingMethod.ShipmentMethodCode);
        cartPromise.then(function (response) {
            $scope.cart = response.data;
        });
    }
    $scope.completeOrder = function () {
        $scope.orderProcessing = true;
        var cartPromise = cartService.setPaymentMethod($scope.selectedPaymentMethod.GatewayCode, $scope.billingAddress);
        cartPromise.then(function (response) {
            var cart = response.data;
            $scope.cart = cart;
            var orderPromise = cartService.createOrder(cart.Id);
            orderPromise.then(function (response) {
                var order = response.data;
                $scope.order = order;
                var payment = order.InPayments.length ? order.InPayments[0] : null;
                if (payment) {
                    var paymentPromise = cartService.processPayment(order.Id, payment.Id);
                    paymentPromise.then(function (response) {
                        handlePaymentResult(response.data);
                        $scope.orderProcessing = false;
                    });
                }
            });
        });
    }
    $scope.handlePaymentResult = function (paymentResult) {

    }
    $scope.setBillingAddressEqualsShipping = function () {
        $scope.billingAddressEqualsShipping = true;
        $scope.billingAddress = $scope.shippingAddress;
        $scope.billingAddress.Type = "Billing";
    }

    var handlePaymentResult = function (paymentResult) {
        if (paymentResult.isSuccess) {
            switch (paymentResult.paymentMethodType) {
                case "Unknown":
                    $scope.go('thanks?id=' + $scope.order.Id);
                    break;
            }
        }
    }
}])

.config(['$interpolateProvider', '$routeProvider', '$locationProvider', '$httpProvider', function ($interpolateProvider, $routeProvider, $locationProvider, $httpProvider) {
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