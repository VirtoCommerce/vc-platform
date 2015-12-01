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

    $scope.cart = null;
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