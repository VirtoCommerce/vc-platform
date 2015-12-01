var app = angular.module('storefrontApp', ['ngRoute']);

app.service('productService', ['$http', function ($http) {
    return {
		getProduct: function (productId) {
			return $http.get('product/' + productId + '/json');
		}
	}
}]);
app.service('cartService', ['$http', function ($http) {
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
}]);

app.controller('mainController', ['$scope', '$window', function ($scope, $window) {
    $scope.go = function (url) {
        $window.location.href = url;
    }
}]);

app.controller('cartController', ['$scope', 'cartService', function ($scope, cartService) {
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
}]);

app.controller('checkoutController', ['$scope', '$route', '$window', 'cartService', function ($scope, $route, $window, cartService) {
    $scope.$route = $route;
    $scope.cart = null;
    $scope.order = null;
    $scope.couponProcessing = false;
    $scope.couponApplied = false;
    $scope.couponHasEror = false;
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
        $scope.couponApplied = cart.Coupon ? cart.Coupon.length > 0 : false;

        var shippingAddresses = _.where(cart.Addresses, { Type: "Shipping" });
        if (shippingAddresses.length) {
            $scope.shippingAddress = shippingAddresses[0];
        }

        var billingAddresses = _.where(cart.Addresses, { Type: "Billing" });
        if (billingAddresses.length) {
            $scope.billingAddress = billingAddresses[0];
        }

        $scope.billingAddress = $scope.shippingAddress;

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
            $scope.couponHasError = cart.DiscountTotal.Amount == $scope.cart.DiscountTotal.Amount;
            $scope.couponApplied = !$scope.couponHasEror;
            $scope.cart = cart;
            $scope.couponProcessing = false;
        });
    }
    $scope.removeCoupon = function () {
        $scope.couponProcessing = true;
        var cartPromise = cartService.removeCoupon();
        cartPromise.then(function (response) {
            var cart = response.data;
            $scope.cart = cart;
            $scope.couponHasEror = false;
            $scope.couponApplied = false;
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
}]);

app.controller('productController', ['$scope', '$window', 'productService', function ($scope, $window, productService) {
	$scope.origProduct = {};
	$scope.product = {};
	$scope.allVariationProperties = {};

	function Initialize() {
		productService.getProduct($window.productId).then(function (response) {
			$scope.origProduct = response.data;
			$scope.allVariationProperties = getFlatternDistinctAllProductVariationProperties($scope.origProduct);	
		});
	};

	function getFlatternDistinctAllProductVariationProperties(product) {
		var retVal = getProductVariationPropertyMap(product);
		_.each(product.Variations, function (variation) {
			var propertyMap = getProductVariationPropertyMap(variation);
			//merge
			_.each(_.keys(propertyMap), function (x) {
				retVal[x] = _.uniq(_.union(retVal[x], propertyMap[x]), "Value");
			});
		});
		return retVal;
	};

	function getProductVariationPropertyMap(product) {
		var retVal = _.where(product.Properties, { Type: 'Variation' });
		retVal = _.groupBy(retVal, function (x) { return x.Name });
		return retVal;
	};

	function getSelectedPropsMap(allVariationProperties) {
		var retVal = {};
		_.each(_.keys(allVariationProperties), function (x) {
			var property = _.find(allVariationProperties[x], function (y) {
				return y.selected;
			});
			if (property) {
				retVal[x] = [property];
			}
		});
		return retVal;
	};

	function comparePropertyMaps(propMap1, propMap2) {
		return _.every(_.keys(propMap1), function (x) {
			var retVal = propMap2.hasOwnProperty(x);
			if(retVal)
			{
				retVal = propMap1[x][0].Value == propMap2[x][0].Value;
    }
			return retVal;
		});
	};

	function findSelectedVariation(product, selectedPropMap) {
		var retVal = _.find(product.Variations, function (x) {
			var productPropMap = getProductVariationPropertyMap(x);
			return comparePropertyMaps(productPropMap, selectedPropMap);
		});
		return retVal;
	};

	$scope.checkProperty = function (property) {
		var prevSelected = _.each($scope.allVariationProperties[property.Name], function (x) {
			if (x != property) {
				x.selected = false;
			}
			else {
				x.selected = !x.selected;
    }
		});
		var selectedPropsMap = getSelectedPropsMap($scope.allVariationProperties);
		//try to find best match variation 
		$scope.product = findSelectedVariation($scope.origProduct, selectedPropsMap);
		
	};

	Initialize();
}]);

app.config(['$interpolateProvider', '$routeProvider', '$locationProvider', function ($interpolateProvider, $routeProvider, $locationProvider) {
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