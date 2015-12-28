var storefrontApp = angular.module('storefrontApp');

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
        getActualProductPrices: function (products) {
            return $http.post('marketing/actualprices', { products: products });
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
        addLineItem: function (productId, quantity) {
            return $http.post('cart/additem', { id: productId, quantity: quantity });
        },
        changeLineItem: function (lineItemId, quantity) {
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
        addAddress: function (address) {
            return $http.post('cart/addaddress', { address: address });
        },
        getAvailableShippingMethods: function () {
            return $http.get('cart/shippingmethods/json?t=' + new Date().getTime());
        },
        getAvailablePaymentMethods: function () {
            return $http.get('cart/paymentmethods/json?t=' + new Date().getTime());
        },
        setShippingMethod: function (shippingMethodCode) {
            return $http.post('cart/shippingmethod', { shippingMethodCode: shippingMethodCode });
        },
        setPaymentMethod: function (paymentMethodCode, billingAddress) {
            return $http.post('cart/paymentmethod', { paymentMethodCode: paymentMethodCode, billingAddress: billingAddress });
        },
        createOrder: function (bankCardInfo) {
            return $http.post('cart/createorder', { bankCardInfo: bankCardInfo });
        }
    }
}]);