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
		    return $http.get('cart/json?t=' + new Date().getTime());
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
		    return $http.get('common/getcountries/json?t=' + new Date().getTime());
		},
		getCountryRegions: function (countryCode) {
		    return $http.get('common/getregions/' + countryCode + '/json?t=' + new Date().getTime());
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
		    return $http.get('cart/' + cartId + '/shipping_methods/json?t=' + new Date().getTime());
		},
		getAvailablePaymentMethods: function (cartId) {
		    return $http.get('cart/' + cartId + '/payment_methods/json?t=' + new Date().getTime());
		},
		setShippingMethod: function (shippingMethodCode, isPreview) {
			return $http.post('cart/shipping_method', { shippingMethodCode: shippingMethodCode, isPreview: isPreview });
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

app.controller('checkoutController', ['$scope', '$location', '$sce', 'cartService', function ($scope, $location, $sce, cartService) {
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
        countryRegions: [],
        couponProcessing: false,
        couponError: null,
        customerInformationProcessing: false,
        availableShippingMethods: [],
        selectedShippingMethod: {},
        shippingMethodProcessing: false,
        availablePaymentMethods: [],
        selectedPaymentMethod: {},
        bankCardInfo: {},
        billingAddressEqualsShipping: true,
        orderProcessing: false,
        paymentFormHtml: null
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
        if (addressType == 'Shipping') {
            $scope.checkout.shippingAddress.CountryCode = country.Code3;
            $scope.checkout.shippingAddress.RegionId = null;
            $scope.checkout.shippingAddress.RegionName = null;
        }
        if (addressType == 'Billing') {
            $scope.checkout.billingAddress.CountryCode = country.Code3;
            $scope.checkout.billingAddress.RegionId = null;
            $scope.checkout.billingAddress.RegionName = null;
        }
        cartService.getCountryRegions(country.Code3).then(function (response) {
            $scope.checkout.countryRegions = response.data;
        });
    }

    $scope.setCountryRegion = function (addressType, regionName) {
        var region = _.find($scope.checkout.countryRegions, function (r) { return r.Name == regionName });
        if (!region) {
            return;
        }
        if (addressType == 'Shipping') {
            $scope.checkout.shippingAddress.RegionId = region.Code;
        }
        if (addressType == 'Billing') {
            $scope.checkout.billingAddress.RegionId = region.Code;
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

    $scope.setCustomerInformation = function (address) {
        $scope.checkout.customerInformationProcessing = true;
        cartService.addAddress(address).then(function (response) {
            updateCheckout(response.data);
            $scope.insideRedirect('shipping-method');
        });
    }

    $scope.setShippingMethod = function (shippingMethodCode, isPreview) {
        $scope.checkout.shippingMethodProcessing = true;
        cartService.setShippingMethod(shippingMethodCode, isPreview).then(function (response) {
            updateCheckout(response.data);
            if (!isPreview) {
                $scope.insideRedirect('payment-method');
            }
        });
    }

    $scope.capitalizeString = function (string) {
        $scope.checkout.bankCardInfo.CardholderName = string.toUpperCase();
    }

    $scope.setBillingAddressEqualsShipping = function () {
        setBillingAddressEqualsShipping();
    }

    $scope.completeOrder = function (paymentMethodCode) {
        $scope.checkout.orderProcessing = true;
        cartService.addAddress($scope.checkout.billingAddress).then(function (response) {
            cartService.setPaymentMethod(paymentMethodCode).then(function (response) {
                var cart = response.data;
                cartService.createOrder(cart.Id).then(function (response) {
                    var order = response.data;
                    var incomingPayment = order.InPayments.length ? order.InPayments[0] : null;
                    cartService.processPayment(order.Id, incomingPayment.Id, $scope.checkout.bankCardInfo).then(function (response) {
                        handlePaymentProcessingResult(response.data, order.Id);
                    });
                });
            });
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
        if (cart.DefaultShippingAddress.CountryCode) {
            cartService.getCountryRegions(cart.DefaultShippingAddress.CountryCode).then(function (response) {
                $scope.checkout.countryRegions = response.data;
            });
        }
        $scope.checkout.billingAddress = cart.DefaultBillingAddress;
        if ($scope.checkout.billingAddressEqualsShipping) {
            setBillingAddressEqualsShipping();
        }
        $scope.checkout.couponProcessing = false;
        $scope.checkout.couponError = _.find(cart.Errors, function (e) { return e == 'InvalidCouponCode' });
        $scope.checkout.customerInformationProcessing = false;
        $scope.checkout.shippingMethodProcessing = false;
        $scope.checkout.orderProcessing = false;
        cartService.getAvailableShippingMethods(cart.Id).then(function (response) {
            var availableShippingMethods = response.data;
            $scope.checkout.availableShippingMethods = availableShippingMethods;
            for (var i = 0; i < availableShippingMethods.length; i++) {
                var selectedShippingMethod = _.find(cart.Shipments, function (e) { return e.ShipmentMethodCode == availableShippingMethods[i].ShipmentMethodCode });
                $scope.checkout.selectedShippingMethod = selectedShippingMethod || {};
            }
        });
        cartService.getAvailablePaymentMethods(cart.Id).then(function (response) {
            $scope.checkout.availablePaymentMethods = response.data;
        });
    }

    function setBillingAddressEqualsShipping() {
        $scope.checkout.billingAddress.Name = $scope.checkout.shippingAddress.Name;
        $scope.checkout.billingAddress.Organization = $scope.checkout.shippingAddress.Organization;
        $scope.checkout.billingAddress.CountryCode = $scope.checkout.shippingAddress.CountryCode;
        $scope.checkout.billingAddress.CountryName = $scope.checkout.shippingAddress.CountryName;
        $scope.checkout.billingAddress.City = $scope.checkout.shippingAddress.City;
        $scope.checkout.billingAddress.PostalCode = $scope.checkout.shippingAddress.PostalCode;
        $scope.checkout.billingAddress.Zip = $scope.checkout.shippingAddress.Zip;
        $scope.checkout.billingAddress.Line1 = $scope.checkout.shippingAddress.Line1;
        $scope.checkout.billingAddress.Line2 = $scope.checkout.shippingAddress.Line2;
        $scope.checkout.billingAddress.RegionId = $scope.checkout.shippingAddress.RegionId;
        $scope.checkout.billingAddress.RegionName = $scope.checkout.shippingAddress.RegionName;
        $scope.checkout.billingAddress.FirstName = $scope.checkout.shippingAddress.FirstName;
        $scope.checkout.billingAddress.MiddleName = $scope.checkout.shippingAddress.MiddleName;
        $scope.checkout.billingAddress.LastName = $scope.checkout.shippingAddress.LastName;
        $scope.checkout.billingAddress.Phone = $scope.checkout.shippingAddress.Phone;
        $scope.checkout.billingAddress.Email = $scope.checkout.shippingAddress.Email;
        $scope.checkout.billingAddressEqualsShipping = true;
    }

    function getBankCardType(bankCardNumber) {
        var type = "unknown";

        return type;
    }

    function handlePaymentProcessingResult(paymentProcessingResult, orderId) {
        if (!paymentProcessingResult.isSuccess) {
            return;
        }
        if (paymentProcessingResult.paymentMethodType == 'PreparedForm' && paymentProcessingResult.htmlForm) {
            $scope.checkout.paymentFormHtml = $sce.trustAsHtml(paymentProcessingResult.htmlForm);
            $scope.insideRedirect('payment-form');
        }
        if (paymentProcessingResult.paymentMethodType == 'Standard') {
            $scope.outsideRedirect($scope.baseUrl + '/cart/checkout/thanks?id=' + orderId);
        }
        if (paymentProcessingResult.paymentMethodType == 'Unknown') {
            $scope.outsideRedirect($scope.baseUrl + '/cart/checkout/thanks?id=' + orderId);
        }
        if (paymentProcessingResult.paymentMethodType == 'Redirection' && paymentProcessingResult.redirectUrl) {
            window.location.href = paymentProcessingResult.redirectUrl;
        }
    }
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
        })
        .when('/payment-form', {
            templateUrl: 'storefront.checkout.paymentForm.tpl'
        });

    return $interpolateProvider.startSymbol('{(').endSymbol(')}');
}]);