var storefrontApp = angular.module('storefrontApp');

storefrontApp.service('dialogService', ['$uibModal', function ($uibModal) {
    return {
        showDialog: function (dialogData, controller, templateUrl) {
            var modalInstance = $uibModal.open({
                controller: controller,
                templateUrl: templateUrl,
                resolve: {
                    dialogData: function () {
                        return dialogData;
                    }
                }
            });
        }
    }
}]);

storefrontApp.service('customerService', ['$http', function ($http) {
    return {
        getCurrentCustomer: function () {
            return $http.get('account/json?t=' + new Date().getTime());
        }
    }
}]);

storefrontApp.service('marketingService', ['$http', function ($http) {
    return {
        getDynamicContent: function (placeName) {
            return $http.get('marketing/dynamiccontent/' + placeName + '/json?t=' + new Date().getTime());
        },
    }
}]);

storefrontApp.service('pricingService', ['$http', function ($http) {
	return {
		getActualProductPrices: function (products) {
			return $http.post('pricing/actualprices', { products: products });
		}
	}
}]);

storefrontApp.service('catalogService', ['$http', function ($http) {
    return {
        getProduct: function (productId) {
            return $http.get('product/' + productId + '/json');
        }
    }
}]);

storefrontApp.service('cartService', ['$http', function ($http) {
    return {
        getCart: function () {
            return $http.get('cart/json?t=' + new Date().getTime());
        },
        getCartItemsCount: function () {
            return $http.get('cart/itemscount/json?t=' + new Date().getTime());
        },
        addLineItem: function (productId, quantity) {
            return $http.post('cart/additem', { id: productId, quantity: quantity });
        },
        changeLineItemQuantity: function (lineItemId, quantity) {
            return $http.post('cart/changeitem', { lineItemId: lineItemId, quantity: quantity });
        },
        removeLineItem: function (lineItemId) {
            return $http.post('cart/removeitem', { lineItemId: lineItemId });
        },
        clearCart: function () {
            return $http.post('cart/clear');
        },
        getCountries: function () {
            return $http.get('common/getcountries/json?t=' + new Date().getTime());
        },
        getCountryRegions: function (countryCode) {
            return $http.get('common/getregions/' + countryCode + '/json?t=' + new Date().getTime());
        },
        addCoupon: function (couponCode) {
            return $http.post('cart/addcoupon/' + couponCode);
        },
        removeCoupon: function () {
            return $http.post('cart/removecoupon');
        },
        addOrUpdateShipment: function (shipmentId, shippingAddress, itemIds, shippingMethodCode) {
            return $http.post('cart/addorupdateshipment', { shipmentId: shipmentId, shippingAddress: shippingAddress, itemIds: itemIds, shippingMethodCode: shippingMethodCode });
        },
        addOrUpdatePayment: function (paymentId, billingAddress, paymentMethodCode, outerId) {
            return $http.post('cart/addorupdatepayment', { paymentId: paymentId, billingAddress: billingAddress, paymentMethodCode: paymentMethodCode, outerId: outerId });
        },
        getAvailableShippingMethods: function () {
            return $http.get('cart/shippingmethods/json?t=' + new Date().getTime());
        },
        getAvailablePaymentMethods: function () {
            return $http.get('cart/paymentmethods/json?t=' + new Date().getTime());
        },
        createOrder: function (bankCardInfo) {
            return $http.post('cart/createorder', { bankCardInfo: bankCardInfo });
        }
    }
}]);

storefrontApp.service('quoteRequestService', ['$http', function ($http) {
    return {
        getQuoteRequest: function (number) {
            return $http.get('quoterequest/' + number + '/json?t=' + new Date().getTime());
        },
        getActualQuoteRequest: function () {
            return $http.get('actualquoterequest/itemscount/json?t=' + new Date().getTime());
        },
        addProductToQuoteRequest: function (productId, quantity) {
            return $http.post('actualquoterequest/addproduct', { productId: productId, quantity: quantity });
        },
        removeProductFromQuoteRequest: function (quoteItemId) {
            return $http.post('actualquoterequest/removeproduct', { quoteItemId: quoteItemId });
        },
        submitQuoteRequest: function (quoteRequest) {
            return $http.post('quoterequest/submit', { quoteRequest: quoteRequest });
        },
        rejectQuoteRequest: function (number) {
            return $http.post('quoterequest/reject', { number: number });
        },
        updateQuoteRequest: function (quoteRequest) {
            return $http.post('quoterequest/update', { quoteRequest: quoteRequest });
        },
        getTotals: function (quoteRequest) {
            return $http.post('quoterequest/totals', { quoteRequest: quoteRequest });
        },
        confirmQuoteRequest: function (quoteRequest) {
            return $http.post('quoterequest/confirm', { quoteRequest: quoteRequest });
        }
    }
}]);