var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('checkoutController', ['$scope', '$location', '$window', 'customerService', 'cartService', function ($scope, $location, $window, customerService, cartService) {
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

    $scope.selectAddress = function (addressType) {
        selectAddress(addressType);
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

    $scope.setBillingAddressEqualsShipping = function () {
        setBillingAddressEqualsShipping();
    }

    $scope.completeOrder = function () {
        $scope.checkout.orderProcessing = true;
        cartService.setPaymentMethod($scope.checkout.selectedPaymentMethod.GatewayCode).then(function (response) {
            cartService.addAddress($scope.checkout.billingAddress).then(function (response) {
                cartService.createOrder($scope.checkout.bankCardInfo).then(function (response) {
                    handlePaymentProcessingResult(response.data.orderProcessingResult, response.data.order.number);
                    $scope.checkout.orderProcessing = false;
                });
            });
        });
    }

    function initialize() {
        $scope.checkout.orderSummaryExpanded = false;
        $scope.checkout.billingAddressEqualsShipping = true;
        $scope.checkout.shippingAddress = { Type: 'Shipping' };
        $scope.checkout.billingAddress = { Type: 'Billing' };
        $scope.checkout.bankCardInfo = {};
        $scope.checkout.selectedAddressId = 1;
        getCurrentCustomer();
        getCountries();
        getAvailableShippingMethods();
        getAvailablePaymentMethods();
        refreshCheckout();
    }

    function getCurrentCustomer() {
        customerService.getCurrentCustomer().then(function (response) {
            $scope.customer = response.data;
            for (var i = 0; i < $scope.customer.Addresses.length; i++) {
                $scope.customer.Addresses[i].Id = i + 1;
                $scope.customer.Addresses[i].StringifiedAddress = stringifyAddress($scope.customer.Addresses[i]);
            }
        });
    }

    function stringifyAddress(address) {
        var stringifiedAddress = address.FirstName + ' ' + address.LastName + ', ';
        stringifiedAddress += address.Organization ? address.Organization + ', ' : '';
        stringifiedAddress += address.CountryName + ', ';
        stringifiedAddress += address.RegionName ? address.RegionName : '';
        stringifiedAddress += address.City;
        stringifiedAddress += address.Line1 + ', '
        stringifiedAddress += address.Line2 ? address.Line2 : '';
        stringifiedAddress += address.PostalCode;
        return stringifiedAddress;
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
        $scope.checkout.shippingAddress.Email = $scope.checkout.shippingAddress.Email || $scope.customer.Email || cartAddress.Email;
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
        selectAddress('Shipping');
    }

    function getBillingAddress(cartAddress) {
        $scope.checkout.billingAddress.Email = $scope.checkout.billingAddress.Email || $scope.customer.Email || cartAddress.Email;
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
        setBillingAddressEqualsShipping();
        selectAddress('Billing');
    }

    function selectAddress(addressType) {
        var address = _.find($scope.customer.Addresses, function (a) { return a.Id == $scope.checkout.selectedAddressId });
        if (address) {
            if (addressType == 'Shipping') {
                $scope.checkout.shippingAddress = address;
                $scope.checkout.shippingAddress.Type = 'Shipping';
            }
            if (addressType == 'Billing') {
                $scope.checkout.billingAddress = address;
                $scope.checkout.billingAddress.Type = 'Billing';
            }
        }
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

    function handlePaymentProcessingResult(paymentProcessingResult, orderNumber) {
        if (!paymentProcessingResult.isSuccess) {
            return;
        }
        if (paymentProcessingResult.paymentMethodType == 'PreparedForm' && paymentProcessingResult.htmlForm) {
            $scope.outerRedirect($scope.baseUrl + 'cart/checkout/paymentform?orderNumber=' + orderNumber);
        }
        if (paymentProcessingResult.paymentMethodType == 'Standard' || paymentProcessingResult.paymentMethodType == 'Unknown') {
            if ($scope.customer.UserName == 'Anonymous') {
                $scope.outerRedirect($scope.baseUrl + 'cart/thanks/' + orderNumber);
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/order/' + orderNumber);
            }
        }
        if (paymentProcessingResult.paymentMethodType == 'Redirection' && paymentProcessingResult.redirectUrl) {
            $window.location.href = paymentProcessingResult.redirectUrl;
        }
    }
}]);