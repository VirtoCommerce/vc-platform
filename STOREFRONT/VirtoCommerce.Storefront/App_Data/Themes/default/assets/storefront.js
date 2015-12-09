var app = angular.module('storefrontApp', ['ngRoute', 'ngSanitize']);

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
			return $http.post('cart/additem', { productId: productId, quantity: quantity });
		},
		changeLineItem: function (lineItemId, quantity) {
			return $http.post('cart/changeitem', { lineItemId: lineItemId, quantity: quantity });
		},
		removeLineItem: function (lineItemId) {
			return $http.post('cart/removeitem', { lineItemId: lineItemId });
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
		createOrder: function () {
			return $http.post('cart/createorder');
		},
		processPayment: function (orderId, paymentId) {
			return $http.post('cart/processpayment', { orderId: orderId, paymentId: paymentId });
		}
	}
}]);

app.controller('mainController', ['$scope', '$location', '$window', function ($scope, $location, $window) {
    //Base store url populated in layout and can be used for construction url inside controller
    $scope.baseUrl = {};

    $scope.currentPath = $location.$$path.replace('/', '');

    //For outside app redirect (To reload the page after changing the URL, use the lower-level API)
    $scope.outerRedirect = function (absUrl) {
        window.location.href = absUrl;
    };

    //change in the current URL or change the current URL in the browser (for app route)
    $scope.innerRedirect = function (path) {
        $location.path(path);
        $scope.currentPath = $location.$$path.replace('/', '');
    };
}]);

app.controller('cartController', ['$scope', 'cartService', function ($scope, cartService) {
    $scope.cart = {};

    initialize();

    $scope.showCartModal = function () {
        $scope.isCartModalVisible = true;
    }

    $scope.hideCartModal = function () {
        $scope.isCartModalVisible = false;
    }

    $scope.addToCart = function (productId, quantity) {
        cartService.addLineItem(productId, quantity).then(function (response) {
            $scope.isCartModalVisible = true;
            refreshCart();
        });
    }

    $scope.changeLineItem = function (lineItemId, quantity) {
        if (quantity >= 1) {
            cartService.changeLineItem(lineItemId, quantity).then(function (response) {
                refreshCart();
            });
        }
    }

    $scope.removeLineItem = function (lineItemId) {
        cartService.removeLineItem(lineItemId).then(function (response) {
            refreshCart();
        });
    }

    function initialize() {
        $scope.isCartModalVisible = false;
        refreshCart();
    }

    function refreshCart() {
        cartService.getCart().then(function (response) {
            $scope.cart = response.data;
        });
    }
}]);

app.controller('checkoutController', ['$scope', '$location', '$sce', 'cartService', function ($scope, $location, $sce, cartService) {
    $scope.checkout = {};

    initialize();

    $scope.toggleOrderSummary = function (orderSummaryExpanded) {
        $scope.checkout.orderSummaryExpanded = !orderSummaryExpanded;
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.checkout.countries, function (c) { return c.Name == countryName; });
        if (country) {
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
            getRegions(country.Code3);
        }
    }

    $scope.setCountryRegion = function (addressType, countryRegionName) {
        var countryRegion = _.find($scope.checkout.countryRegions, function (c) { return c.Name == countryRegionName; });
        if (countryRegion) {
            if (addressType == 'Shipping') {
                $scope.checkout.shippingAddress.RegionId = countryRegion.Code;
            }
            if (addressType == 'Billing') {
                $scope.checkout.billingAddress.RegionId = countryRegion.Code;
            }
        }
    }

    $scope.setCustomerInformation = function () {
        $scope.checkout.customerInformationProcessing = true;
        cartService.addAddress($scope.checkout.shippingAddress).then(function (response) {
            refreshCheckout();
            $scope.innerRedirect('shipping-method');
        });
    }

    $scope.addCoupon = function (event) {
        event.preventDefault();
        $scope.checkout.couponProcessing = true;
        cartService.addCoupon($scope.checkout.coupon.Code).then(function (response) {
            $scope.checkout.coupon = response.data;
            refreshCheckout();
        });
    }

    $scope.removeCoupon = function () {
        $scope.checkout.couponProcessing = true;
        cartService.removeCoupon().then(function (response) {
            $scope.checkout.coupon = null;
            refreshCheckout();
        });
    }

    $scope.validateBankCardNumber = function () {
        if ($scope.checkout.bankCardInfo.BankCardNumber) {
            validateBankCardNumber($scope.checkout.bankCardInfo.BankCardNumber);
        }
    }

    $scope.capitalizeCardholderName = function () {
        $scope.checkout.bankCardInfo.CardholderName = $scope.checkout.bankCardInfo.CardholderName.toUpperCase();
    }

    $scope.completeOrder = function () {
        $scope.checkout.orderProcessing = true;
        cartService.setPaymentMethod($scope.checkout.selectedPaymentMethod.GatewayCode).then(function (response) {
            cartService.addAddress($scope.checkout.billingAddress).then(function (response) {
                cartService.createOrder($scope.checkout.id).then(function (response) {
                    var order = response.data;
                    var payment = order.InPayments.length ? order.InPayments[0] : null;
                    if (payment) {
                        cartService.processPayment(order.Id, payment.Id, $scope.checkout.bankCardInfo).then(function (response) {
                            var processingResult = response.data;
                            handlePaymentProcessingResult(processingResult, order.Id);
                            $scope.checkout.orderProcessing = false;
                        });
                    }
                });
            });
        });
    }

    function initialize() {
        $scope.checkout.orderSummaryExpanded = false;
        $scope.checkout.shippingAddress = {};
        $scope.checkout.billingAddress = {};
        $scope.checkout.bankCardInfo = {};
        getCountries();
        getAvailableShippingMethods();
        getAvailablePaymentMethods();
        refreshCheckout();
    }

    function refreshCheckout() {
        $scope.checkout.couponProcessing = false;
        $scope.checkout.customerInformationProcessing = false;
        $scope.checkout.shippingMethodProcessing = false;
        cartService.getCart().then(function (response) {
            var cart = response.data;
            $scope.checkout.id = cart.Id;
            $scope.checkout.lineItems = cart.Items;
            $scope.checkout.coupon = $scope.checkout.coupon || cart.Coupon;
            $scope.checkout.subtotal = cart.SubTotal;
            $scope.checkout.shippingTotal = cart.ShippingTotal;
            $scope.checkout.taxTotal = cart.TaxTotal;
            $scope.checkout.discountTotal = cart.DiscountTotal;
            $scope.checkout.total = cart.Total;
            $scope.checkout.hasPhysicalProducts = cart.HasPhysicalProducts;
            if ($scope.checkout.hasPhysicalProducts) {
                getShippingAddress(cart.DefaultShippingAddress);
            }
            if (!$scope.checkout.hasPhysicalProducts) {
                $scope.innerRedirect('payment-method');
            }
            getBillingAddress(cart.DefaultBillingAddress);
            $scope.checkout.hasCustomerInformation = checkAddress($scope.checkout.shippingAddress, $scope.checkout.countryRegions && $scope.checkout.countryRegions.length);
            $scope.checkout.hasShippingMethod = cart.Shipments.length > 0;
        });
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            $scope.checkout.countries = response.data;
        });
    }

    function getRegions(countryCode3) {
        cartService.getCountryRegions(countryCode3).then(function (response) {
            $scope.checkout.countryRegions = response.data;
        });
    }

    function getAvailableShippingMethods() {
        cartService.getAvailableShippingMethods().then(function (response) {
            var availableShippingMethods = response.data;
            $scope.checkout.availableShippingMethods = availableShippingMethods;
            if (availableShippingMethods.length == 1) {
                $scope.checkout.selectedShippingMethod = availableShippingMethods[0];
                setShippingMethod();
            }
        });
    }

    function getAvailablePaymentMethods() {
        cartService.getAvailablePaymentMethods().then(function (response) {
            var availablePaymentMethods = response.data;
            $scope.checkout.availablePaymentMethods = availablePaymentMethods;
            if (availablePaymentMethods.length == 1) {
                $scope.checkout.selectedPaymentMethod = availablePaymentMethods[0];
            }
        });
    }

    function setShippingMethod() {
        $scope.checkout.shippingMethodProcessing = true;
        cartService.setShippingMethod($scope.checkout.selectedShippingMethod.ShipmentMethodCode).then(function (response) {
            refreshCheckout();
        });
    }

    function getShippingAddress(cartAddress) {
        $scope.checkout.shippingAddress.Email = $scope.checkout.shippingAddress.Email || cartAddress.Email;
        $scope.checkout.shippingAddress.FirstName = $scope.checkout.shippingAddress.FirstName || cartAddress.FirstName;
        $scope.checkout.shippingAddress.LastName = $scope.checkout.shippingAddress.LastName || cartAddress.LastName;
        $scope.checkout.shippingAddress.Organization = $scope.checkout.shippingAddress.Organization || cartAddress.Organization;
        $scope.checkout.shippingAddress.Line1 = $scope.checkout.shippingAddress.Line1 || cartAddress.Line1;
        $scope.checkout.shippingAddress.Line2 = $scope.checkout.shippingAddress.Line2 || cartAddress.Line2;
        $scope.checkout.shippingAddress.City = $scope.checkout.shippingAddress.City || cartAddress.City;
        $scope.checkout.shippingAddress.CountryCode = $scope.checkout.shippingAddress.CountryCode || cartAddress.CountryCode;
        $scope.checkout.shippingAddress.CountryName = $scope.checkout.shippingAddress.CountryName || cartAddress.CountryName;
        $scope.checkout.shippingAddress.RegionId = $scope.checkout.shippingAddress.RegionId || cartAddress.RegionId;
        $scope.checkout.shippingAddress.RegionName = $scope.checkout.shippingAddress.RegionName || cartAddress.RegionName;
        $scope.checkout.shippingAddress.PostalCode = $scope.checkout.shippingAddress.PostalCode || cartAddress.PostalCode;
        $scope.checkout.shippingAddress.Phone = $scope.checkout.shippingAddress.Phone || cartAddress.Phone;
        if ($scope.checkout.shippingAddress.CountryCode) {
            getRegions($scope.checkout.shippingAddress.CountryCode);
        }
    }

    function getBillingAddress(cartAddress) {
        $scope.checkout.billingAddress.Email = $scope.checkout.billingAddress.Email || cartAddress.Email;
        $scope.checkout.billingAddress.FirstName = $scope.checkout.billingAddress.FirstName || cartAddress.FirstName;
        $scope.checkout.billingAddress.LastName = $scope.checkout.billingAddress.LastName || cartAddress.LastName;
        $scope.checkout.billingAddress.Organization = $scope.checkout.billingAddress.Organization || cartAddress.Organization;
        $scope.checkout.billingAddress.Line1 = $scope.checkout.billingAddress.Line1 || cartAddress.Line1;
        $scope.checkout.billingAddress.Line2 = $scope.checkout.billingAddress.Line2 || cartAddress.Line2;
        $scope.checkout.billingAddress.City = $scope.checkout.billingAddress.City || cartAddress.City;
        $scope.checkout.billingAddress.CountryCode = $scope.checkout.billingAddress.CountryCode || cartAddress.CountryCode;
        $scope.checkout.billingAddress.CountryName = $scope.checkout.billingAddress.CountryName || cartAddress.CountryName;
        $scope.checkout.billingAddress.RegionId = $scope.checkout.billingAddress.RegionId || cartAddress.RegionId;
        $scope.checkout.billingAddress.RegionName = $scope.checkout.billingAddress.RegionName || cartAddress.RegionName;
        $scope.checkout.billingAddress.PostalCode = $scope.checkout.billingAddress.PostalCode || cartAddress.PostalCode;
        $scope.checkout.billingAddress.Phone = $scope.checkout.billingAddress.Phone || cartAddress.Phone;
        if ($scope.checkout.billingAddress.CountryCode) {
            getRegions($scope.checkout.billingAddress.CountryCode);
        }
    }

    function checkAddress(address, provinceRequired) {
        var isValid = false;
        if (address.Email && address.FirstName && address.LastName && address.Line1 &&
            address.City && address.CountryCode && address.CountryName && address.PostalCode) {
            if (!provinceRequired || provinceRequired && address.RegionId && address.RegionName) {
                isValid = true;
            }
        }
        return isValid;
    }

    function validateBankCardNumber(bankCardNumber) {
        var type = 'Unknown';
        var cardNumberPattern = /^\d{12,19}$/;
        var cvvPattern = /^\d{3,3}$/;

        var firstOneSymbol = bankCardNumber.substring(0, 1);
        var firstTwoSymbols = bankCardNumber.substring(0, 2);
        var firstThreeSymbols = bankCardNumber.substring(0, 3);
        var firstFourSymbols = bankCardNumber.substring(0, 4);
        var firstSixSymbols = bankCardNumber.substring(0, 6);

        if (firstTwoSymbols == '34' || firstTwoSymbols == '37') {
            type = 'AmericanExpress';
            cardNumberPattern = /^\d{15}$/;
            cvvPattern = /^\d{4,4}$/;
        }
        if (firstTwoSymbols == '62' || firstTwoSymbols == '88') {
            type = 'UnionPay';
            cardNumberPattern = /^\d{16,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstThreeSymbols >= '300' && firstThreeSymbols <= '305' || firstThreeSymbols == '309' || firstTwoSymbols == '36' || firstTwoSymbols == '38' || firstTwoSymbols == '39') {
            type = 'Diners';
            cardNumberPattern = /^\d{14,16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '6011' || firstSixSymbols >= '622126' && firstSixSymbols <= '622925' || firstThreeSymbols >= '644' && firstThreeSymbols <= '649' || firstTwoSymbols == '65') {
            type = 'Discover';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols >= '3528' && firstFourSymbols <= '3589') {
            type = 'Jcb';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '6304' || firstFourSymbols == '6706' || firstFourSymbols == '6771' || firstFourSymbols == '6709') {
            type = 'Laser';
            cardNumberPattern = /^\d{16,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '5018' || firstFourSymbols == '5020' || firstFourSymbols == '5038' || firstFourSymbols == '5612' || firstFourSymbols == '5893' || firstFourSymbols == '6304' ||
            firstFourSymbols >= '6759' && firstFourSymbols <= '6763' || firstFourSymbols == '0604' || firstFourSymbols == '6390') {
            type = 'Maestro';
            cardNumberPattern = /^\d{12,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '5019') {
            type = 'Dankort';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstTwoSymbols >= '51' && firstTwoSymbols <= '55') {
            type = 'MasterCard';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstOneSymbol == '4') {
            type = 'Visa';
            cardNumberPattern = /^\d{13,16}$/;
            cvvPattern = /^\d{3,3}$/;
        }

        $scope.checkout.bankCardInfo.Type = type;
        $scope.checkout.bankCardInfo.CardNumberPattern = cardNumberPattern;
        $scope.checkout.bankCardInfo.CvvPattern = cvvPattern;
    }

    //function validateBankCardExpirationDate(bankCardExpidationDate) {
    //    $scope.checkout.bankCardInfo.BankCardMonth = null;
    //    $scope.checkout.bankCardInfo.BankCardYear = null;
    //    var separator = ' / ';
    //    $scope.checkout.bankCardInfo.ExpirationDate = bankCardExpidationDate;
    //    if (date.length == 2) {
    //        $scope.checkout.bankCardInfo.ExpirationDate += separator;
    //    }
    //    var dateParts = date.split(separator);
    //    if (dateParts.length == 2) {
    //        var currentYear = (new Date()).getFullYear();
    //        var currentMonth = (new Date()).getMonth() + 1;
    //        var expirationMonth = parseInt(dateParts[0]);
    //        var expirationYear = parseInt(dateParts[1]) + 2000;
    //        if (expirationMonth >= 1 && expirationMonth <= 12) {
    //            if (expirationYear > currentYear || expirationYear == currentYear && expirationMonth >= currentMonth) {
    //                alert(expirationMonth + ' ' + expirationYear);
    //                $scope.checkout.bankCardInfo.BankCardMonth = expirationMonth;
    //                $scope.checkout.bankCardInfo.BankCardYear = expirationYear;
    //            }
    //        }
    //    }
    //}

    function handlePaymentProcessingResult(paymentProcessingResult, orderId) {
        if (!paymentProcessingResult.isSuccess) {
            return;
        }
        if (paymentProcessingResult.paymentMethodType == 'PreparedForm' && paymentProcessingResult.htmlForm) {
            $scope.checkout.paymentFormHtml = $sce.trustAsHtml(paymentProcessingResult.htmlForm);
            $scope.innerRedirect('payment-form');
        }
        if (paymentProcessingResult.paymentMethodType == 'Standard') {
            $scope.outerRedirect($scope.baseUrl + '/cart/thanks?orderId=' + orderId);
        }
        if (paymentProcessingResult.paymentMethodType == 'Unknown') {
            $scope.outerRedirect($scope.baseUrl + '/cart/thanks?orderId=' + orderId);
        }
        if (paymentProcessingResult.paymentMethodType == 'Redirection' && paymentProcessingResult.redirectUrl) {
            window.location.href = paymentProcessingResult.redirectUrl;
        }
    }

    //======================================================================================================


    //$scope.checkout = {
    //    hasCustomerInformation: false,
    //    hasShippingMethod: false,
    //    cart: {},
    //    orderSummaryExpanded: false,
    //    shippingAddress: {},
    //    billingAddress: {},
    //    countries: [],
    //    countryRegions: [],
    //    couponProcessing: false,
    //    couponError: null,
    //    customerInformationProcessing: false,
    //    availableShippingMethods: [],
    //    selectedShippingMethod: {},
    //    shippingMethodProcessing: false,
    //    availablePaymentMethods: [],
    //    selectedPaymentMethod: {},
    //    bankCardInfo: { Type: 'Unknown', ExpirationDate: null, BankCardMonth: null, BankCardYear: null },
    //    billingAddressEqualsShipping: true,
    //    orderProcessing: false,
    //    paymentFormHtml: null
    //}

    //$scope.toggleOrderSummary = function (orderSummaryExpanded) {
    //    $scope.checkout.orderSummaryExpanded = !orderSummaryExpanded;
    //}

    //$scope.setCountry = function (addressType, countryName)
    //{
    //    var country = _.find($scope.checkout.countries, function (c) { return c.Name == countryName; });
    //    if (!country) {
    //        return;
    //    }
    //    if (addressType == 'Shipping') {
    //        $scope.checkout.shippingAddress.CountryCode = country.Code3;
    //        $scope.checkout.shippingAddress.RegionId = null;
    //        $scope.checkout.shippingAddress.RegionName = null;
    //    }
    //    if (addressType == 'Billing') {
    //        $scope.checkout.billingAddress.CountryCode = country.Code3;
    //        $scope.checkout.billingAddress.RegionId = null;
    //        $scope.checkout.billingAddress.RegionName = null;
    //    }
    //    cartService.getCountryRegions(country.Code3).then(function (response) {
    //        $scope.checkout.countryRegions = response.data;
    //    });
    //}

    //$scope.setCountryRegion = function (addressType, regionName) {
    //    var region = _.find($scope.checkout.countryRegions, function (r) { return r.Name == regionName });
    //    if (!region) {
    //        return;
    //    }
    //    if (addressType == 'Shipping') {
    //        $scope.checkout.shippingAddress.RegionId = region.Code;
    //    }
    //    if (addressType == 'Billing') {
    //        $scope.checkout.billingAddress.RegionId = region.Code;
    //    }
    //}

    //$scope.addCoupon = function (couponCode) {
    //    if (!couponCode) {
    //        return;
    //    }
    //    $scope.checkout.couponProcessing = true;
    //    cartService.addCoupon(couponCode).then(function (response) {
    //        updateCheckout(response.data);
    //    });
    //}

    //$scope.removeCoupon = function () {
    //    $scope.checkout.couponProcessing = true;
    //    cartService.removeCoupon().then(function (response) {
    //        updateCheckout(response.data);
    //    });
    //}

    //$scope.setCustomerInformation = function (address) {
    //    $scope.checkout.customerInformationProcessing = true;
    //    cartService.addAddress(address).then(function (response) {
    //        updateCheckout(response.data);
    //        $scope.innerRedirect('shipping-method');
    //    });
    //}

    //$scope.setShippingMethod = function (shippingMethodCode, isPreview) {
    //    $scope.checkout.shippingMethodProcessing = true;
    //    cartService.setShippingMethod(shippingMethodCode, isPreview).then(function (response) {
    //        updateCheckout(response.data);
    //        if (!isPreview) {
    //            $scope.innerRedirect('payment-method');
    //        }
    //    });
    //}

    //$scope.capitalizeString = function (string) {
    //    $scope.checkout.bankCardInfo.CardholderName = string.toUpperCase();
    //}

    //$scope.setBillingAddressEqualsShipping = function () {
    //    setBillingAddressEqualsShipping();
    //}

    //$scope.validateBankCardNumber = function (bankCardNumber) {
    //    var type = 'Unknown';
    //    var cardNumberPattern = /^\d{12,19}$/;
    //    var cvvPattern = /^\d{3,3}$/;

    //    var firstOneSymbol = bankCardNumber.substring(0, 1);
    //    var firstTwoSymbols = bankCardNumber.substring(0, 2);
    //    var firstThreeSymbols = bankCardNumber.substring(0, 3);
    //    var firstFourSymbols = bankCardNumber.substring(0, 4);
    //    var firstSixSymbols = bankCardNumber.substring(0, 6);

    //    if (firstTwoSymbols == '34' || firstTwoSymbols == '37') {
    //        type = 'AmericanExpress';
    //        cardNumberPattern = /^\d{15}$/;
    //        cvvPattern = /^\d{4,4}$/;
    //    }
    //    if (firstTwoSymbols == '62' || firstTwoSymbols == '88') {
    //        type = 'UnionPay';
    //        cardNumberPattern = /^\d{16,19}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstThreeSymbols >= '300' && firstThreeSymbols <= '305' || firstThreeSymbols == '309' || firstTwoSymbols == '36' || firstTwoSymbols == '38' || firstTwoSymbols == '39') {
    //        type = 'Diners';
    //        cardNumberPattern = /^\d{14,16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstFourSymbols == '6011' || firstSixSymbols >= '622126' && firstSixSymbols <= '622925' || firstThreeSymbols >= '644' && firstThreeSymbols <= '649' || firstTwoSymbols == '65') {
    //        type = 'Discover';
    //        cardNumberPattern = /^\d{16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstFourSymbols >= '3528' && firstFourSymbols <= '3589') {
    //        type = 'Jcb';
    //        cardNumberPattern = /^\d{16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstFourSymbols == '6304' || firstFourSymbols == '6706' || firstFourSymbols == '6771' || firstFourSymbols == '6709') {
    //        type = 'Laser';
    //        cardNumberPattern = /^\d{16,19}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstFourSymbols == '5018' || firstFourSymbols == '5020' || firstFourSymbols == '5038' || firstFourSymbols == '5612' || firstFourSymbols == '5893' || firstFourSymbols == '6304' ||
    //        firstFourSymbols >= '6759' && firstFourSymbols <= '6763' || firstFourSymbols == '0604' || firstFourSymbols == '6390') {
    //        type = 'Maestro';
    //        cardNumberPattern = /^\d{12,19}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstFourSymbols == '5019') {
    //        type = 'Dankort';
    //        cardNumberPattern = /^\d{16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstTwoSymbols >= '51' && firstTwoSymbols <= '55') {
    //        type = 'MasterCard';
    //        cardNumberPattern = /^\d{16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }
    //    if (firstOneSymbol == '4') {
    //        type = 'Visa';
    //        cardNumberPattern = /^\d{13,16}$/;
    //        cvvPattern = /^\d{3,3}$/;
    //    }

    //    $scope.checkout.bankCardInfo.Type = type;
    //    $scope.checkout.bankCardInfo.CardNumberPattern = cardNumberPattern;
    //    $scope.checkout.bankCardInfo.CvvPattern = cvvPattern;
    //}

    //$scope.validateBankCardExpirationDate = function (date) {
    //    $scope.checkout.bankCardInfo.BankCardMonth = null;
    //    $scope.checkout.bankCardInfo.BankCardYear = null;
    //    var separator = ' / ';
    //    $scope.checkout.bankCardInfo.ExpirationDate = date;
    //    if (date.length == 2) {
    //        $scope.checkout.bankCardInfo.ExpirationDate += separator;
    //    }
    //    var dateParts = date.split(separator);
    //    if (dateParts.length == 2) {
    //        var currentYear = (new Date()).getFullYear();
    //        var currentMonth = (new Date()).getMonth() + 1;
    //        var expirationMonth = parseInt(dateParts[0]);
    //        var expirationYear = parseInt(dateParts[1]) + 2000;
    //        if (expirationMonth >= 1 && expirationMonth <= 12) {
    //            if (expirationYear > currentYear || expirationYear == currentYear && expirationMonth >= currentMonth) {
    //                alert(expirationMonth + ' ' + expirationYear);
    //                $scope.checkout.bankCardInfo.BankCardMonth = expirationMonth;
    //                $scope.checkout.bankCardInfo.BankCardYear = expirationYear;
    //            }
    //        }
    //    }
    //}

    //$scope.completeOrder = function (paymentMethodCode) {
    //    $scope.checkout.orderProcessing = true;
    //    cartService.addAddress($scope.checkout.billingAddress).then(function (response) {
    //        cartService.setPaymentMethod(paymentMethodCode).then(function (response) {
    //            var cart = response.data;
    //            cartService.createOrder(cart.Id).then(function (response) {
    //                var order = response.data;
    //                var incomingPayment = order.InPayments.length ? order.InPayments[0] : null;
    //                cartService.processPayment(order.Id, incomingPayment.Id, $scope.checkout.bankCardInfo).then(function (response) {
    //                    handlePaymentProcessingResult(response.data, order.Id);
    //                });
    //            });
    //        });
    //    });
    //}

    //cartService.getCountries().then(function (response) {
    //    $scope.checkout.countries = response.data;
    //});

    //cartService.getCart().then(function (response) {
    //    var cart = response.data;
    //    handleInnerRedirects(cart);
    //    updateCheckout(cart);
    //});

    //function addressIsValid(address) {
    //    var isValid = false;
    //    if (address.FirstName && address.LastName && address.Email && address.Line1 && address.City && address.CountryCode && address.CountryName && address.PostalCode) {
    //        isValid = true;
    //    }
    //    return isValid;
    //}

    //function handleInnerRedirects(cart) {
    //    if (!cart.ItemsCount) {
    //        $scope.outerRedirect($scope.baseUrl + '/cart');
    //    }
    //    var hasCustomerInformation = addressIsValid(cart.DefaultShippingAddress);
    //    var hasShippingMethod = cart.Shipments.length;
    //    if (!cart.HasPhysicalProducts) {
    //        $scope.innerRedirect('payment-method');
    //    } else {
    //        if (hasCustomerInformation && hasShippingMethod) {
    //            $scope.innerRedirect('payment-method');
    //            return;
    //        }
    //        if (hasCustomerInformation && !hasShippingMethod) {
    //            $scope.innerRedirect('shipping-method');
    //            return;
    //        }
    //        if (!hasCustomerInformation) {
    //            $scope.innerRedirect('customer-information');
    //            return;
    //        }
    //    }
    //}

    //function updateCheckout(cart) {
    //    $scope.checkout.cart = cart;
    //    $scope.checkout.shippingAddress = cart.DefaultShippingAddress;
    //    if (cart.DefaultShippingAddress.CountryCode) {
    //        cartService.getCountryRegions(cart.DefaultShippingAddress.CountryCode).then(function (response) {
    //            $scope.checkout.countryRegions = response.data;
    //        });
    //    }
    //    $scope.checkout.billingAddress = cart.DefaultBillingAddress;
    //    if ($scope.checkout.billingAddressEqualsShipping) {
    //        setBillingAddressEqualsShipping();
    //    }
    //    $scope.checkout.couponProcessing = false;
    //    $scope.checkout.couponError = _.find(cart.Errors, function (e) { return e == 'InvalidCouponCode' });
    //    $scope.checkout.customerInformationProcessing = false;
    //    $scope.checkout.shippingMethodProcessing = false;
    //    $scope.checkout.orderProcessing = false;
    //    $scope.checkout.hasCustomerInformation = addressIsValid(cart.DefaultShippingAddress);
    //    $scope.checkout.hasShippingMethod = cart.Shipments.length;
    //    cartService.getAvailableShippingMethods(cart.Id).then(function (response) {
    //        var availableShippingMethods = response.data;
    //        $scope.checkout.availableShippingMethods = availableShippingMethods;
    //        for (var i = 0; i < availableShippingMethods.length; i++) {
    //            var selectedShippingMethod = _.find(cart.Shipments, function (e) { return e.ShipmentMethodCode == availableShippingMethods[i].ShipmentMethodCode });
    //            $scope.checkout.selectedShippingMethod = selectedShippingMethod || {};
    //        }
    //    });
    //    cartService.getAvailablePaymentMethods(cart.Id).then(function (response) {
    //        $scope.checkout.availablePaymentMethods = response.data;
    //    });
    //}

    //function setBillingAddressEqualsShipping() {
    //    $scope.checkout.billingAddress.Name = $scope.checkout.shippingAddress.Name;
    //    $scope.checkout.billingAddress.Organization = $scope.checkout.shippingAddress.Organization;
    //    $scope.checkout.billingAddress.CountryCode = $scope.checkout.shippingAddress.CountryCode;
    //    $scope.checkout.billingAddress.CountryName = $scope.checkout.shippingAddress.CountryName;
    //    $scope.checkout.billingAddress.City = $scope.checkout.shippingAddress.City;
    //    $scope.checkout.billingAddress.PostalCode = $scope.checkout.shippingAddress.PostalCode;
    //    $scope.checkout.billingAddress.Zip = $scope.checkout.shippingAddress.Zip;
    //    $scope.checkout.billingAddress.Line1 = $scope.checkout.shippingAddress.Line1;
    //    $scope.checkout.billingAddress.Line2 = $scope.checkout.shippingAddress.Line2;
    //    $scope.checkout.billingAddress.RegionId = $scope.checkout.shippingAddress.RegionId;
    //    $scope.checkout.billingAddress.RegionName = $scope.checkout.shippingAddress.RegionName;
    //    $scope.checkout.billingAddress.FirstName = $scope.checkout.shippingAddress.FirstName;
    //    $scope.checkout.billingAddress.MiddleName = $scope.checkout.shippingAddress.MiddleName;
    //    $scope.checkout.billingAddress.LastName = $scope.checkout.shippingAddress.LastName;
    //    $scope.checkout.billingAddress.Phone = $scope.checkout.shippingAddress.Phone;
    //    $scope.checkout.billingAddress.Email = $scope.checkout.shippingAddress.Email;
    //    $scope.checkout.billingAddressEqualsShipping = true;
    //}

    //function handlePaymentProcessingResult(paymentProcessingResult, orderId) {
    //    if (!paymentProcessingResult.isSuccess) {
    //        return;
    //    }
    //    if (paymentProcessingResult.paymentMethodType == 'PreparedForm' && paymentProcessingResult.htmlForm) {
    //        $scope.checkout.paymentFormHtml = $sce.trustAsHtml(paymentProcessingResult.htmlForm);
    //        $scope.innerRedirect('payment-form');
    //    }
    //    if (paymentProcessingResult.paymentMethodType == 'Standard') {
    //        $scope.outerRedirect($scope.baseUrl + '/cart/checkout/thanks?id=' + orderId);
    //    }
    //    if (paymentProcessingResult.paymentMethodType == 'Unknown') {
    //        $scope.outerRedirect($scope.baseUrl + '/cart/checkout/thanks?id=' + orderId);
    //    }
    //    if (paymentProcessingResult.paymentMethodType == 'Redirection' && paymentProcessingResult.redirectUrl) {
    //        window.location.href = paymentProcessingResult.redirectUrl;
    //    }
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
        })
        .when('/payment-form', {
            templateUrl: 'storefront.checkout.paymentForm.tpl'
        });

    return $interpolateProvider.startSymbol('{(').endSymbol(')}');
}]);