var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('checkoutController', ['$rootScope', '$scope', '$window', 'cartService',
    function ($rootScope, $scope, $window, cartService) {
    const shippingAddressStepInnerUrl = 'shipping-address';
    const shippingMethodStepInnerUrl = 'shipping-method';
    const paymentMethodStepInnerUrl = 'payment-method';

    const shippingAddressType = 'Shipping';
    const billingAddressType = 'Billing';

    var newShipment = { deliveryAddress: { type: shippingAddressType } };
    var newPayment = { billingAddress: { type: billingAddressType } };

    initialize();

    $scope.goToCheckoutStep = function (innerUrl) {
        goToCheckoutStep(innerUrl);
    }

    $scope.toggleOrderSummary = function (isVisible) {
        $scope.checkout.orderSummaryVisible = !isVisible;
    }

    $scope.setShippingAddressForm = function (form) {
        $scope.formShippingAddress = form;
    }

    $scope.setShippingMethodForm = function (form) {
        $scope.formShippingMethod = form;
    }

    $scope.setPaymentMethodForm = function (form) {
        $scope.formPaymentMethod = form;
    }

    $scope.selectAddress = function (addressType) {
        selectAddress(addressType);
    }

    $scope.addCoupon = function (event) {
        event.preventDefault();
        $scope.checkout.couponProcessing = true;
        cartService.addCoupon($scope.checkout.coupon.code).then(function (response) {
            var coupon = response.data;
            $scope.checkout.couponProcessing = false;
            $scope.checkout.coupon = coupon;
            getCart(true);
        }, function (response) {
            $scope.checkout.couponProcessing = false;
        });
    }

    $scope.removeCoupon = function () {
        $scope.checkout.couponProcessing = true;
        cartService.removeCoupon().then(function (response) {
            $scope.checkout.couponProcessing = false;
            $scope.checkout.coupon = null;
            getCart(true);
        }, function (response) {
            $scope.checkout.couponProcessing = false;
        });
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.checkout.countries, function (c) { return c.name == countryName });
        if (country) {
            var countryCode = country.code3 || country.code2;
            if (addressType == shippingAddressType) {
                $scope.checkout.shipment.deliveryAddress.regionId = null;
                $scope.checkout.shipment.deliveryAddress.regionName = null;
                $scope.checkout.shipment.deliveryAddress.countryCode = countryCode;
            }
            if (addressType == billingAddressType) {
                $scope.checkout.payment.billingAddress.regionId = null;
                $scope.checkout.payment.billingAddress.regionName = null;
                $scope.checkout.payment.billingAddress.countryCode = countryCode;
            }
            if (countryCode) {
                getCountryRegions(country.code3);
            }
        }
    }

    $scope.setCountryRegion = function (addressType, countryRegionName) {
        var countryRegion = _.find($scope.checkout.countryRegions, function (r) { return r.name == countryRegionName });
        if (countryRegion) {
            if (addressType == shippingAddressType) {
                $scope.checkout.shipment.deliveryAddress.regionId = countryRegion.code;
            }
            if (addressType == billingAddressType) {
                $scope.checkout.payment.billingAddress.regionId = countryRegion.code;
            }
        }
    }

    $scope.setShippingMethod = function ()
    {
        setShippingMethod($scope.checkout.shipment);
    }

    $scope.setBillingAddressEqualShipping = function () {
        if ($scope.checkout.billingAddressEqualsShipping) {
            setBillingAddressEqualShipping();
        }
    }

    $scope.validateBankCardNumber = function () {
        validateBankCardNumber();
    }

    $scope.capitalizeCardholderName = function () {
        $scope.checkout.bankCardInfo.cardholderName = $scope.checkout.bankCardInfo.cardholderName.toUpperCase();
    }

    $scope.validateBankCardExpirationDate = function () {
        var expirationDate = $scope.checkout.bankCardInfo.expirationDate;
        if (!expirationDate) {
            return;
        }

        var currentDate = new Date();
        var currentYear = currentDate.getYear() - 100;
        var currentMonth = currentDate.getMonth() + 1;
        var expirationMonth = parseInt(expirationDate.substr(0, 2));
        var expirationYear = parseInt(expirationDate.substr(expirationDate.indexOf('/') + 2, 2));
        if (!expirationMonth.isNaN && !expirationYear.isNaN &&
            (currentYear > expirationYear || currentYear <= expirationYear && currentMonth > expirationMonth)) {
            $scope.formPaymentMethod.BankCardExpirationDate.$invalid = true;
            $scope.checkout.bankCardInfo.isExpired = true;
        }
    }

    $scope.submitShippingAddress = function () {
        $scope.checkout.shipment.deliveryAddress.email = angular.copy($scope.checkout.email);
        $scope.formShippingAddress.$setSubmitted();
        if ($scope.formShippingAddress.$invalid) {
            return;
        }
        $scope.checkout.shippingAddressProcessing = true;
        cartService.addOrUpdateShipment($scope.checkout.shipment).then(function (response) {
            $scope.checkout.shippingAddressProcessing = false;
            $scope.innerRedirect(shippingMethodStepInnerUrl);
            getCart();
        }, function (response) {
            $scope.checkout.shippingAddressProcessing = false;
        });
    }

    $scope.submitShippingMethod = function () {
        $scope.formShippingMethod.$setSubmitted();
        if ($scope.formShippingMethod.$invalid) {
            return;
        }
        $scope.checkout.shippingAddressProcessing = true;
        $scope.innerRedirect(paymentMethodStepInnerUrl);
        $scope.checkout.shippingAddressProcessing = false;
    }

    $scope.submitOrder = function () {
        $scope.checkout.payment.billingAddress.email = angular.copy($scope.checkout.email);
        $scope.formPaymentMethod.$setSubmitted();
        if ($scope.formPaymentMethod.$invalid) {
            return;
        }
        $scope.checkout.orderProcessing = true;
        cartService.addOrUpdatePayment($scope.checkout.payment).then(function (response) {
            createOrder();
        }, function (response) {
            $scope.checkout.orderProcessing = false;
        });
    }

    function initialize() {
        $scope.checkout = {
            shippingAddressIsFilled: false,
            shippingMethodIsFilled: false,
            selectedCustomerAddressId: 1,
            steps: [],
            countries: [],
            countryRegions: [],
            couponProcessing: false,
            shippingAddressProcessing: false,
            shippingMethodProcessing: false,
            orderProcessing: false,
            billingAddressEqualsShipping: true,
            customerAddresses: [],
            bankCardInfo: {
                type: 'Unknown',
                cardNumberPattern: /^\d{12,19}$/,
                cvvPattern: /^\d{3,3}$/
            }
        };
        getCart();
        getCountries();
        getAvailablePaymentMethods();
    }

    function selectAddress(addressType) {
        var address = _.find($scope.checkout.customerAddresses, function (a) { return a.id == $scope.checkout.selectedCustomerAddressId });
        if (!address) {
            return;
        }
        if (addressType == shippingAddressType) {
            address.type = shippingAddressType;
            $scope.checkout.shipment.deliveryAddress = address;
        }
        if (addressType == billingAddressType) {
            address.type = billingAddressType;
            $scope.checkout.payment.billingAddress = address;
        }
        getCountryRegions(address.countryCode);
    }

    function stringifyAddress(address) {
        var stringifiedAddress = address.firstName + ' ' + address.lastName + ', ';
        stringifiedAddress += address.organization ? address.organization + ', ' : '';
        stringifiedAddress += address.countryName + ', ';
        stringifiedAddress += address.regionName ? address.regionName + ', ' : '';
        stringifiedAddress += address.city;
        stringifiedAddress += address.line1 + ', '
        stringifiedAddress += address.line2 ? address.line2 : '';
        stringifiedAddress += address.postalCode;
        return stringifiedAddress;
    }

    function getCart(updateOnlyTotals) {
        cartService.getCart().then(function (response) {
            var cart = response.data;
            if (updateOnlyTotals) {
                updateTotals(cart);
                return;
            }
            updateTotals(cart);
            var shipment = cart.shipments.length ? cart.shipments[0] : newShipment;
            shipment.deliveryAddress = addressIsValid(shipment.deliveryAddress) ? shipment.deliveryAddress : cart.defaultShippingAddress;
            var payment = cart.payments.length ? cart.payments[0] : newPayment;
            payment.billingAddress = addressIsValid(payment.billingAddress) ? payment.billingAddress : cart.defaultBillingAddress;
            $scope.checkout.email = cart.email;
            $scope.checkout.items = cart.items;
            $scope.checkout.hasPhysicalProducts = cart.hasPhysicalProducts;
            $scope.checkout.shipment = shipment;
            $scope.checkout.payment = payment;
            getCustomerAddresses(cart);
            if (shipment.deliveryAddress.countryCode) {
                getCountryRegions(shipment.deliveryAddress.countryCode);
            }
            if (payment.billingAddress.countryCode) {
                getCountryRegions(payment.billingAddress.countryCode);
            }
            getAvailableShippingMethods(shipment.id);
            if (!payment.id) {
                setBillingAddressEqualShipping();
            }
            $scope.checkout.billingAddressEqualsShipping = addressesEqual(shipment.deliveryAddress, payment.billingAddress);
            if (cart.hasPhysicalProducts) {
                $scope.checkout.shippingAddressIsFilled = addressIsValid($scope.checkout.shipment.deliveryAddress);
                $scope.checkout.shippingMethodIsFilled = !_.isEmpty($scope.checkout.shipment.shipmentMethodCode);
            }
        });
    }

    function getCustomerAddresses(cart) {
        var addressId = 1;
        if (cart.customer) {
            _.each(cart.customer.addresses, function (address) {
                address.id = addressId;
                address.stringifiedAddress = stringifyAddress(address);
                addressId++;
            });
            $scope.checkout.customerAddresses = cart.customer.addresses;
        }
    }

    function handleCheckoutRedirects(cart) {
        if (cart.hasValidationErrors) {
            $scope.outerRedirect($scope.baseUrl + 'cart');
        }
        if (cart.hasPhysicalProducts) {
            if (!$scope.checkout.shippingAddressIsFilled) {
                goToCheckoutStep(shippingAddressStepInnerUrl);
            }
            if ($scope.checkout.shippingAddressIsFilled && !$scope.checkout.shippingMethodIsFilled) {
                goToCheckoutStep(shippingMethodStepInnerUrl);
            }
        }
    }

    function goToCheckoutStep(innerUrl) {
        var step = _.find($scope.checkout.steps, function (s) { return s.innerUrl == innerUrl });
        if (step) {
            _.each($scope.checkout.steps, function (s) { s.current = false; });
            $scope.innerRedirect(innerUrl);
            step.current = true;
        }
    }

    function updateTotals(cart) {
        $scope.checkout.coupon = $scope.checkout.coupon || cart.coupon;
        $scope.checkout.subtotal = cart.subTotal;
        $scope.checkout.discountTotal = cart.discountTotal;
        $scope.checkout.shippingTotal = cart.shippingTotal;
        $scope.checkout.taxTotal = cart.taxTotal;
        $scope.checkout.total = cart.total;
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            var countries = response.data;
            $scope.checkout.countries = countries;
        });
    }

    function getCountryRegions(countryCode) {
        $scope.checkout.selectedCountry = _.find($scope.checkout.countries, function (c) { return c.code3 == countryCode });
        cartService.getCountryRegions(countryCode).then(function (response) {
            var countryRegions = response.data;
            $scope.checkout.countryRegions = countryRegions || [];
        });
    }

    function getAvailableShippingMethods(shipmentId) {
        cartService.getAvailableShippingMethods(shipmentId).then(function (response) {
            var availableShippingMethods = response.data;
            $scope.checkout.availableShippingMethods = availableShippingMethods;
            if ($scope.checkout.shipment && !$scope.checkout.shipment.shipmentMethodCode &&
                addressIsValid($scope.checkout.shipment.deliveryAddress) && availableShippingMethods.length == 1) {
                $scope.checkout.shipment.shipmentMethodCode = availableShippingMethods[0].shipmentMethodCode;
                setShippingMethod();
            }
        });
    }

    function getAvailablePaymentMethods() {
        cartService.getAvailablePaymentMethods().then(function (response) {
            var availablePaymentMethods = response.data;
            $scope.checkout.availablePaymentMethods = availablePaymentMethods;
            if (availablePaymentMethods.length == 1) {
                $scope.checkout.payment = $scope.checkout.payment || newPayment;
                $scope.checkout.payment.paymentGatewayCode = availablePaymentMethods[0].gatewayCode;
            }
        });
    }

    function setShippingMethod() {
        $scope.checkout.shippingMethodProcessing = true;
        cartService.addOrUpdateShipment($scope.checkout.shipment).then(function (response) {
            $scope.checkout.shippingMethodProcessing = false;
            getCart();
        }, function (response) {
            $scope.checkout.shippingMethodProcessing = false;
        });
    }

    function setBillingAddressEqualShipping() {
        $scope.checkout.payment.billingAddress = angular.copy($scope.checkout.shipment.deliveryAddress);
        $scope.checkout.payment.billingAddress.type = billingAddressType;
    }

    function createOrder() {
        cartService.createOrder($scope.checkout.bankCardInfo).then(function (response) {
            var order = response.data.order;
            var orderProcessingResult = response.data.orderProcessingResult;
            handlePostPaymentResult(order, orderProcessingResult);
            $scope.checkout.orderProcessing = false;
        }, function (response) {
            $scope.checkout.orderProcessing = false;
        });
    }

    function addressesEqual(address1, address2) {
        var address1Type = address1.type; address1.type = null;
        var address2Type = address2.type; address2.type = null;
        var isEqual = angular.equals(address1, address2);
        address1.type = address1Type;
        address2.type = address2Type;
        return isEqual;
    }

    function validateBankCardNumber() {
        var bankCardNumber = $scope.checkout.bankCardInfo.cardNumber;
        var cardType = 'Unknown';
        var cardNumberPattern = /^\d{12,19}$/;
        var cvvPattern = /^\d{3,3}$/;

        if (!bankCardNumber) {
            return;
        }

        var firstOneSymbol = bankCardNumber.substring(0, 1);
        var firstTwoSymbols = bankCardNumber.substring(0, 2);
        var firstThreeSymbols = bankCardNumber.substring(0, 3);
        var firstFourSymbols = bankCardNumber.substring(0, 4);
        var firstSixSymbols = bankCardNumber.substring(0, 6);

        if (firstTwoSymbols == '34' || firstTwoSymbols == '37') {
            cardType = 'AmericanExpress';
            cardNumberPattern = /^\d{15}$/;
            cvvPattern = /^\d{4,4}$/;
        }
        if (firstTwoSymbols == '62' || firstTwoSymbols == '88') {
            cardType = 'UnionPay';
            cardNumberPattern = /^\d{16,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstThreeSymbols >= '300' && firstThreeSymbols <= '305' || firstThreeSymbols == '309' || firstTwoSymbols == '36' || firstTwoSymbols == '38' || firstTwoSymbols == '39') {
            cardType = 'Diners';
            cardNumberPattern = /^\d{14,16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '6011' || firstSixSymbols >= '622126' && firstSixSymbols <= '622925' || firstThreeSymbols >= '644' && firstThreeSymbols <= '649' || firstTwoSymbols == '65') {
            cardType = 'Discover';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols >= '3528' && firstFourSymbols <= '3589') {
            cardType = 'Jcb';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '6304' || firstFourSymbols == '6706' || firstFourSymbols == '6771' || firstFourSymbols == '6709') {
            cardType = 'Laser';
            cardNumberPattern = /^\d{16,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '5018' || firstFourSymbols == '5020' || firstFourSymbols == '5038' || firstFourSymbols == '5612' || firstFourSymbols == '5893' || firstFourSymbols == '6304' ||
            firstFourSymbols >= '6759' && firstFourSymbols <= '6763' || firstFourSymbols == '0604' || firstFourSymbols == '6390') {
            cardType = 'Maestro';
            cardNumberPattern = /^\d{12,19}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstFourSymbols == '5019') {
            cardType = 'Dankort';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstTwoSymbols >= '51' && firstTwoSymbols <= '55') {
            cardType = 'MasterCard';
            cardNumberPattern = /^\d{16}$/;
            cvvPattern = /^\d{3,3}$/;
        }
        if (firstOneSymbol == '4') {
            cardType = 'Visa';
            cardNumberPattern = /^\d{13,16}$/;
            cvvPattern = /^\d{3,3}$/;
        }

        $scope.checkout.bankCardInfo.type = cardType;
        $scope.checkout.bankCardInfo.cardNumberPattern = cardNumberPattern;
        $scope.checkout.bankCardInfo.cvvPattern = cvvPattern;
    }

    function handlePostPaymentResult(order, orderProcessingResult) {
        if (!orderProcessingResult.isSuccess) {
            return;
        }
        if (orderProcessingResult.paymentMethodType == 'PreparedForm' && orderProcessingResult.htmlForm) {
            $scope.outerRedirect($scope.baseUrl + 'cart/checkout/paymentform?orderNumber=' + order.number);
        }
        if (orderProcessingResult.paymentMethodType == 'Standard' || orderProcessingResult.paymentMethodType == 'Unknown') {
            if (!$scope.customer.HasAccount) {
                $scope.outerRedirect($scope.baseUrl + 'cart/thanks/' + order.number);
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/order/' + order.number);
            }
        }
        if (orderProcessingResult.paymentMethodType == 'Redirection' && orderProcessingResult.redirectUrl) {
            $window.location.href = orderProcessingResult.redirectUrl;
        }
    }

    function addressIsValid(address) {
        var isValid = false;
        if (address && address.email && address.firstName && address.lastName && address.line1 &&
            address.city && address.countryName && address.postalCode) {
            isValid = true;
        }
        return isValid;
    }
}]);