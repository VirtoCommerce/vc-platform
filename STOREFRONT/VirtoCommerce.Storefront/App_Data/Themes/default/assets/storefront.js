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
		getCountries: function () {
		    return $http.get('common/getcountries/json', { headers: { 'Cache-Control': 'no-cache' } });
		},
		getRegions: function (countryCode) {
		    return $http.get('common/getregions/' + countryCode + '/json');
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

app.controller('mainController', ['$scope', '$location', '$window', function ($scope, $location, $window) {
    //Base store url populated in layout and can be used for construction url inside controller
    $scope.baseUrl = {};

    //For outside app redirect (To reload the page after changing the URL, use the lower-level API)
    $scope.outsideRedirect = function (absUrl) {
        window.location.href = absUrl;
    };

    //change in the current URL or change the current URL in the browser (for app route)
    $scope.insideRedirect = function (path) {
        $location.path(path);
    };
}]);

app.controller('cartController', ['$scope', 'cartService', function ($scope, cartService) {
	$scope.cart = null;
	$scope.isCartModalVisible = false;

	cartService.getCart().then(function (response) {
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

app.controller('checkoutController', ['$scope', 'cartService', function ($scope, cartService) {
    //Base store url populated in layout and can be used for construction url inside controller
    $scope.baseUrl = {};

    //For outside app redirect (To reload the page after changing the URL, use the lower-level API)
    $scope.outsideRedirect = function (absUrl) {
        window.location.href = absUrl;
    };

    //change in the current URL or change the current URL in the browser (for app route)
    $scope.insideRedirect = function (path) {
        $location.path(path);
    };

    $scope.checkout = {
        cart: {},
        orderSummaryExpanded: false,
        shippingAddress: {},
        billingAddress: {},
        countries: [],
        regions: [],
        couponProcessing: false,
        couponError: null
    }

    $scope.toggleOrderSummary = function (orderSummaryExpanded) {
        $scope.checkout.orderSummaryExpanded = !orderSummaryExpanded;
    }

    $scope.setCountry = function (addressType, countryName)
    {
        var country = _.find($scope.checkout.countries, function (c) { return c.Name == countryName; });
        if (!country) {
            return;
        }
        switch (addressType) {
            case 'Shipping':
                $scope.checkout.shippingAddress.CountryCode = country.Code3;
                break;
            case 'Billing':
                $scope.checkout.billingAddress.CountryCode = country.Code3;
                break;
        }
        cartService.getRegions(country.Code3).then(function (response) {
            $scope.checkout.regions = response.data;
        });
    }

    $scope.setCountryRegion = function (addressType, regionName) {
        var region = _.find($scope.checkout.regions, function (r) { return r.Name == regionName });
        if (region) {
            switch (addressType) {
                case 'Shipping':
                    $scope.checkout.shippingAddress.RegionId = region.Code;
                    break;
                case 'Billing':
                    $scope.checkout.billingAddress.RegionId = region.Code;
                    break;
            }
        }
    }

    $scope.addCoupon = function (couponCode) {
        if (!couponCode) {
            return;
        }
        $scope.checkout.couponProcessing = true;
        cartService.addCoupon(couponCode).then(function (response) {
            updateCheckout(response.data);
        });
    }

    $scope.removeCoupon = function () {
        $scope.checkout.couponProcessing = true;
        cartService.removeCoupon().then(function (response) {
            updateCheckout(response.data);
        });
    }

    cartService.getCountries().then(function (response) {
        $scope.checkout.countries = response.data;
    });

    cartService.getCart().then(function (response) {
        var cart = response.data;

        if (!cart.ItemsCount) {
            $scope.outsideRedirect($scope.baseUrl + '/cart');
        }

        updateCheckout(cart);
    });

    function updateCheckout(cart) {
        $scope.checkout.cart = cart;
        $scope.checkout.shippingAddress = cart.DefaultShippingAddress;
        $scope.checkout.billingAddress = cart.DefaultBillingAddress;
        $scope.checkout.couponProcessing = false;
        $scope.checkout.couponError = _.find(cart.Errors, function (e) { return e == 'InvalidCouponCode' });
    }


	//$scope.cart = null;
	//$scope.order = null;
	//$scope.couponProcessing = false;
	//$scope.couponApplied = false;
	//$scope.couponHasEror = false;
	//$scope.shippingAddress = {};
	//$scope.billingAddress = {};
	//$scope.billingAddressEqualsShipping = true;
	//$scope.availableShippingMethods = null;
	//$scope.selectedShippingMethod = {};
	//$scope.availablePaymentMethods = null;
	//$scope.selectedPaymentMethod = {};
	//$scope.isOrderSummaryExpanded = false;
	//$scope.orderProcessing = false;

	//var cartPromise = cartService.getCart();
	//cartPromise.then(function (response) {
	//	var cart = response.data;
	//	$scope.cart = cart;
	//	$scope.couponApplied = cart.Coupon ? cart.Coupon.length > 0 : false;

	//	var shippingAddresses = _.where(cart.Addresses, { Type: "Shipping" });
	//	if (shippingAddresses.length) {
	//		$scope.shippingAddress = shippingAddresses[0];
	//	}

	//	var billingAddresses = _.where(cart.Addresses, { Type: "Billing" });
	//	if (billingAddresses.length) {
	//		$scope.billingAddress = billingAddresses[0];
	//	}

	//	$scope.billingAddress = $scope.shippingAddress;

	//	var availableShippingMethodsPromise = cartService.getAvailableShippingMethods(cart.Id);
	//	availableShippingMethodsPromise.then(function (response) {
	//		var availableShippingMethods = response.data;
	//		$scope.availableShippingMethods = availableShippingMethods;
	//		var shippingMethod = cart.Shipments.length ? cart.Shipments[0] : null;
	//		if (shippingMethod) {
	//			var matchedShippingMethods = _.where(availableShippingMethods, { ShipmentMethodCode: shippingMethod.ShipmentMethodCode });
	//			$scope.selectedShippingMethod = matchedShippingMethods.length ? matchedShippingMethods[0] : {};
	//		}
	//	});

	//	var availablePaymentMethodsPromise = cartService.getAvailablePaymentMethods(cart.Id);
	//	availablePaymentMethodsPromise.then(function (response) {
	//		$scope.availablePaymentMethods = response.data;
	//	});
	//});
	
	//$scope.toggleOrderSummary = function (isExpanded) {
	//	$scope.isOrderSummaryExpanded = !isExpanded;
	//}
	//$scope.addCoupon = function (couponCode) {
	//	$scope.couponProcessing = true;
	//	var cartPromise = cartService.addCoupon(couponCode);
	//	cartPromise.then(function (response) {
	//		var cart = response.data;
	//		$scope.couponHasError = cart.DiscountTotal.Amount == $scope.cart.DiscountTotal.Amount;
	//		$scope.couponApplied = !$scope.couponHasEror;
	//		$scope.cart = cart;
	//		$scope.couponProcessing = false;
	//	});
	//}
	//$scope.removeCoupon = function () {
	//	$scope.couponProcessing = true;
	//	var cartPromise = cartService.removeCoupon();
	//	cartPromise.then(function (response) {
	//		var cart = response.data;
	//		$scope.cart = cart;
	//		$scope.couponHasEror = false;
	//		$scope.couponApplied = false;
	//		$scope.couponProcessing = false;
	//	});
	//}
	//$scope.setShippingAddress = function () {
	//	var cartPromise = cartService.addAddress($scope.shippingAddress);
	//	cartPromise.then(function (response) {
	//		$scope.cart = response.data;
	//		$scope.insideRedirect('shipping-method')
	//	});
	//}
	//$scope.setShippingMethod = function () {
	//	var cartPromise = cartService.setShippingMethod($scope.selectedShippingMethod.ShipmentMethodCode);
	//	cartPromise.then(function (response) {
	//		$scope.cart = response.data;
	//	});
	//}
	//$scope.completeOrder = function () {
	//	$scope.orderProcessing = true;
	//	var cartPromise = cartService.setPaymentMethod($scope.selectedPaymentMethod.GatewayCode, $scope.billingAddress);
	//	cartPromise.then(function (response) {
	//		var cart = response.data;
	//		$scope.cart = cart;
	//		var orderPromise = cartService.createOrder(cart.Id);
	//		orderPromise.then(function (response) {
	//			var order = response.data;
	//			$scope.order = order;
	//			var payment = order.InPayments.length ? order.InPayments[0] : null;
	//			if (payment) {
	//				var paymentPromise = cartService.processPayment(order.Id, payment.Id);
	//				paymentPromise.then(function (response) {
	//					handlePaymentResult(response.data);
	//					$scope.orderProcessing = false;
	//				});
	//			}
	//		});
	//	});
	//}
	//$scope.setBillingAddressEqualsShipping = function () {
	//	$scope.billingAddressEqualsShipping = true;
	//	$scope.billingAddress = $scope.shippingAddress;
	//	$scope.billingAddress.Type = "Billing";
	//}

	//var handlePaymentResult = function (paymentResult) {
	//	if (paymentResult.isSuccess) {
	//		switch (paymentResult.paymentMethodType) {
	//			case "Unknown":
	//				$scope.outsideRedirect($scope.baseUrl + '/cart/checkout/thanks?id=' + $scope.order.Id);
	//				break;
	//		}
	//	}
	//}
}]);

app.controller('productController', ['$scope', '$window', 'productService', function ($scope, $window, productService) {
	//TODO: prevent add to cart not selected variation
	// display validator please select property
	// display price range

	var allVarations = [];
	$scope.selectedVariation = {};
	$scope.allVariationPropsMap = {};

	function Initialize() {
		productService.getProduct($window.productId).then(function (response) {
			var product = response.data;
			//Current product its also variation (titular)
			allVarations = [ product ].concat(product.Variations);
			$scope.allVariationPropsMap = getFlatternDistinctPropertiesMap(allVarations);

			//Auto select initial product as default variation  (its possible because all our products is variations)
			var propertyMap = getVariationPropertyMap(product);
			_.each(_.keys(propertyMap), function (x) {
				$scope.checkProperty(propertyMap[x][0])
			});
			$scope.selectedVariation = product;
		});
	};

	function getFlatternDistinctPropertiesMap(variations) {
		var retVal = {};
		_.each(variations, function (variation) {
			var propertyMap = getVariationPropertyMap(variation);
			//merge
			_.each(_.keys(propertyMap), function (x) {
				retVal[x] = _.uniq(_.union(retVal[x], propertyMap[x]), "Value");
			});
		});
		return retVal;
	};

	function getVariationPropertyMap(variation) {
		retVal = _.groupBy(variation.VariationProperties, function (x) { return x.DisplayName });
		return retVal;
	};

	function getSelectedPropsMap(variationPropsMap) {
		var retVal = {};
		_.each(_.keys(variationPropsMap), function (x) {
			var property = _.find(variationPropsMap[x], function (y) {
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
			if (retVal) {
				retVal = propMap1[x][0].Value == propMap2[x][0].Value;
			}
			return retVal;
		});
	};

	function findVariationBySelectedProps(variations, selectedPropMap) {
		var retVal = _.find(variations, function (x) {
			var productPropMap = getVariationPropertyMap(x);
			return comparePropertyMaps(productPropMap, selectedPropMap);
		});
		return retVal;
	};

	//Method called from View when user click to one of properties value
	$scope.checkProperty = function (property) {
		//Select apropriate property and diselect previous selected
		var prevSelected = _.each($scope.allVariationPropsMap[property.DisplayName], function (x) {
			x.selected = x != property ? false : !x.selected;
		});

		var selectedPropsMap = getSelectedPropsMap($scope.allVariationPropsMap);
		//try to find best match variation for already selected properties
		$scope.selectedVariation = findVariationBySelectedProps(allVarations, selectedPropsMap);
	};

	Initialize();
}]);

app.config(['$interpolateProvider', '$routeProvider', function ($interpolateProvider, $routeProvider) {
    $routeProvider
        .when('/customer-information', {
            templateUrl: 'storefront.checkout.customerInformation.tpl'
        })
        .when('/shipping-method', {
            templateUrl: 'storefront.checkout.shippingMethod.tpl'
        })
        .when('/payment-method', {
            templateUrl: 'storefront.checkout.paymentMethod.tpl'
        });

    return $interpolateProvider.startSymbol('{(').endSymbol(')}');
}]);