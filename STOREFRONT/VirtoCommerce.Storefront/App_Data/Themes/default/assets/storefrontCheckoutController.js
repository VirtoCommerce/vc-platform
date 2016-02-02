var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('checkoutController', ['$rootScope', '$scope', '$window', 'customerService', 'cartService', function ($rootScope, $scope, $window, customerService, cartService) {
    const shippingAddressStepInnerUrl = 'shipping-address';
    const shippingMethodStepInnerUrl = 'shipping-method';
    const paymentMethodStepInnerUrl = 'payment-method';

    const shippingAddressType = 'Shipping';
    const billingAddressType = 'Billing';

    var newShipment = { DeliveryAddress: { Type: shippingAddressType } };
    var newPayment = { BillingAddress: { Type: billingAddressType } };

    initialize();

    $scope.goToCheckoutStep = function (innerUrl) {
        goToCheckoutStep(innerUrl);
    }

    $scope.toggleOrderSummary = function (isVisible) {
        $scope.checkout.OrderSummaryVisible = !isVisible;
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
        $scope.checkout.CouponProcessing = true;
        cartService.addCoupon($scope.checkout.Coupon.Code).then(function (response) {
            var coupon = response.data;
            $scope.checkout.CouponProcessing = false;
            $scope.checkout.Coupon = coupon;
            getCart(true);
        }, function (response) {
            $scope.checkout.CouponProcessing = false;
        });
    }

    $scope.removeCoupon = function () {
        $scope.checkout.CouponProcessing = true;
        cartService.removeCoupon().then(function (response) {
            $scope.checkout.CouponProcessing = false;
            $scope.checkout.Coupon = null;
            getCart(true);
        }, function (response) {
            $scope.checkout.CouponProcessing = false;
        });
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.checkout.Countries, function (c) { return c.Name == countryName });
        if (country) {
            var countryCode = country.Code3 || country.Code2;
            if (addressType == shippingAddressType) {
                $scope.checkout.Shipment.DeliveryAddress.RegionId = null;
                $scope.checkout.Shipment.DeliveryAddress.RegionName = null;
                $scope.checkout.Shipment.DeliveryAddress.CountryCode = countryCode;
            }
            if (addressType == billingAddressType) {
                $scope.checkout.Payment.BillingAddress.RegionId = null;
                $scope.checkout.Payment.BillingAddress.RegionName = null;
                $scope.checkout.Payment.BillingAddress.CountryCode = countryCode;
            }
            if (countryCode) {
                getCountryRegions(country.Code3);
            }
        }
    }

    $scope.setCountryRegion = function (addressType, countryRegionName) {
        var countryRegion = _.find($scope.checkout.CountryRegions, function (r) { return r.Name == countryRegionName });
        if (countryRegion) {
            if (addressType == shippingAddressType) {
                $scope.checkout.Shipment.DeliveryAddress.RegionId = countryRegion.Code;
            }
            if (addressType == billingAddressType) {
                $scope.checkout.Payment.BillingAddress.RegionId = countryRegion.Code;
            }
        }
    }

    $scope.setShippingMethod = function (shipmentId, shippingMethodCode)
    {
        setShippingMethod(shipmentId, shippingMethodCode);
    }

    $scope.setBillingAddressEqualShipping = function () {
        if ($scope.checkout.BillingAddressEqualsShipping) {
            setBillingAddressEqualShipping();
        }
    }

    $scope.validateBankCardNumber = function () {
        validateBankCardNumber();
    }

    $scope.capitalizeCardholderName = function () {
        $scope.checkout.BankCardInfo.CardholderName = $scope.checkout.BankCardInfo.CardholderName.toUpperCase();
    }

    $scope.validateBankCardExpirationDate = function () {
        var expirationDate = $scope.checkout.BankCardInfo.ExpirationDate;
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
            $scope.checkout.BankCardInfo.IsExpired = true;
        }
    }

    $scope.submitShippingAddress = function () {
        $scope.checkout.Shipment.DeliveryAddress.Email = angular.copy($scope.checkout.Email);
        $scope.formShippingAddress.$setSubmitted();
        if ($scope.formShippingAddress.$invalid) {
            return;
        }
        var shipmentId = $scope.checkout.Shipment.Id;
        var shipmentAddress = $scope.checkout.Shipment.DeliveryAddress;
        var itemIds = _.map($scope.checkout.Items, function (i) { return i.Id });
        $scope.checkout.ShippingAddressProcessing = true;
        cartService.addOrUpdateShipment(shipmentId, shipmentAddress, itemIds, null).then(function (response) {
            $scope.checkout.ShippingAddressProcessing = false;
            $scope.innerRedirect(shippingMethodStepInnerUrl);
            getCart();
        }, function (response) {
            $scope.checkout.ShippingAddressProcessing = false;
        });
    }

    $scope.submitShippingMethod = function () {
        $scope.formShippingMethod.$setSubmitted();
        if ($scope.formShippingMethod.$invalid) {
            return;
        }
        $scope.checkout.ShippingAddressProcessing = true;
        $scope.innerRedirect(paymentMethodStepInnerUrl);
        $scope.checkout.ShippingAddressProcessing = false;
    }

    $scope.submitOrder = function () {
        $scope.checkout.Payment.BillingAddress.Email = angular.copy($scope.checkout.Email);
        $scope.formPaymentMethod.$setSubmitted();
        if ($scope.formPaymentMethod.$invalid) {
            return;
        }
        var payment = $scope.checkout.Payment;
        $scope.checkout.OrderProcessing = true;
        cartService.addOrUpdatePayment(payment.Id, payment.BillingAddress, payment.PaymentGatewayCode, payment.OuterId).then(function (response) {
            createOrder();
        }, function (response) {
            $scope.checkout.OrderProcessing = false;
        });
    }

    function initialize() {
        $scope.checkout = {
            ShippingAddressIsFilled: false,
            ShippingMethodIsFilled: false,
            SelectedCustomerAddressId: 1,
            Steps: [],
            Countries: [],
            CountryRegions: [],
            CouponProcessing: false,
            ShippingAddressProcessing: false,
            ShippingMethodProcessing: false,
            OrderProcessing: false,
            BillingAddressEqualsShipping: true,
            BankCardInfo: {
                Type: 'Unknown',
                CardNumberPattern: /^\d{12,19}$/,
                CvvPattern: /^\d{3,3}$/
            }
        };
        getCart();
        getCountries();
        getAvailablePaymentMethods();
    }

    function getCurrentCustomer() {
        customerService.getCurrentCustomer().then(function (response) {
            var customer = response.data;
            $scope.customer = customer;
            getCustomerAddresses();
        });
    }

    function getCustomerAddresses() {
        if (!$scope.customer.Addresses) {
            return;
        }
        for (var i = 0; i < $scope.customer.Addresses.length; i++) {
            $scope.customer.Addresses[i].Id = i + 1;
            $scope.customer.Addresses[i].StringifiedAddress = stringifyAddress($scope.customer.Addresses[i]);
        }
        if ($scope.currentPath == shippingAddressStepInnerUrl) {
            selectAddress(shippingAddressType);
        }
        if ($scope.currentPath == paymentMethodStepInnerUrl) {
            selectAddress(billingAddressType);
        }
    }

    function selectAddress(addressType) {
        var address = _.find($scope.customer.Addresses, function (a) { return a.Id == $scope.checkout.SelectedCustomerAddressId });
        if (!address) {
            return;
        }
        if (addressType == shippingAddressType) {
            address.Type = shippingAddressType;
            $scope.checkout.Shipment.DeliveryAddress = address;
        }
        if (addressType == billingAddressType) {
            address.Type = billingAddressType;
            $scope.checkout.Payment.BillingAddress = address;
        }
        getCountryRegions(address.CountryCode);
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

    function getCart(updateOnlyTotals) {
        cartService.getCart().then(function (response) {
            var cart = response.data;
            if (updateOnlyTotals) {
                updateTotals(cart);
                return;
            }
            updateTotals(cart);
            var shipment = cart.Shipments.length ? cart.Shipments[0] : newShipment;
            if (!shipment.DeliveryAddress.FirstName) {
                shipment.DeliveryAddress.FirstName = $scope.customer.FirstName;
            }
            if (!shipment.DeliveryAddress.LastName) {
                shipment.DeliveryAddress.LastName = $scope.customer.LastName;
            }
            var payment = cart.Payments.length ? cart.Payments[0] : newPayment;
            if (!payment.BillingAddress.FirstName) {
                payment.BillingAddress.FirstName = $scope.customer.FirstName;
            }
            if (!payment.BillingAddress.LastName) {
                payment.BillingAddress.LastName = $scope.customer.LastName;
            }
            $scope.checkout.Email = shipment.DeliveryAddress.Email || $scope.customer.Email;
            $scope.checkout.Items = cart.Items;
            $scope.checkout.HasPhysicalProducts = cart.HasPhysicalProducts;
            $scope.checkout.Shipment = shipment;
            $scope.checkout.Payment = payment;
            if (shipment.DeliveryAddress.CountryCode) {
                getCountryRegions(shipment.DeliveryAddress.CountryCode);
            }
            if (payment.BillingAddress.CountryCode) {
                getCountryRegions(payment.BillingAddress.CountryCode);
            }
            getAvailableShippingMethods(shipment.Id);
            if (!payment.Id) {
                setBillingAddressEqualShipping();
            }
            $scope.checkout.BillingAddressEqualsShipping = addressesEqual(shipment.DeliveryAddress, payment.BillingAddress);
            if (cart.HasPhysicalProducts) {
                $scope.checkout.ShippingAddressIsFilled = addressIsValid($scope.checkout.Shipment.DeliveryAddress);
                $scope.checkout.ShippingMethodIsFilled = !_.isEmpty($scope.checkout.Shipment.ShipmentMethodCode);
            }
            getCheckoutSteps(cart);
            handleCheckoutRedirects(cart);
            getCurrentCustomer();
        });
    }

    function getCheckoutSteps(cart) {
        var steps = [];
        if (cart.HasPhysicalProducts) {
            steps.push({
                Code: 'shippingAddress',
                Weight: 1,
                InnerUrl: shippingAddressStepInnerUrl,
                Current: $scope.currentPath == shippingAddressStepInnerUrl,
                Enabled: true,
                Completed: $scope.checkout.ShippingAddressIsFilled
            });
            steps.push({
                Code: 'shippingMethod',
                Weight: 2,
                InnerUrl: shippingMethodStepInnerUrl,
                Current: $scope.currentPath == shippingMethodStepInnerUrl,
                Enabled: $scope.checkout.ShippingAddressIsFilled,
                Completed: $scope.checkout.ShippingAddressIsFilled && $scope.checkout.ShippingMethodIsFilled
            });
        }
        steps.push({
            Code: 'paymentMethod',
            Weight: 3,
            InnerUrl: paymentMethodStepInnerUrl,
            Current: $scope.currentPath == paymentMethodStepInnerUrl,
            Enabled: !cart.HasPhysicalProducts || $scope.checkout.ShippingAddressIsFilled && $scope.checkout.ShippingMethodIsFilled,
            Completed: false
        });
        $scope.checkout.Steps = _.sortBy(steps, 'Weight');
    }

    function handleCheckoutRedirects(cart) {
        if (cart.HasValidationErrors) {
            $scope.outerRedirect($scope.baseUrl + 'cart');
        }
        if (cart.HasPhysicalProducts) {
            if (!$scope.checkout.ShippingAddressIsFilled) {
                goToCheckoutStep(shippingAddressStepInnerUrl);
            }
            if ($scope.checkout.ShippingAddressIsFilled && !$scope.checkout.ShippingMethodIsFilled) {
                goToCheckoutStep(shippingMethodStepInnerUrl);
            }
        }
    }

    function goToCheckoutStep(innerUrl) {
        var step = _.find($scope.checkout.Steps, function (s) { return s.InnerUrl == innerUrl });
        if (step) {
            _.each($scope.checkout.Steps, function (s) { s.Current = false; });
            $scope.innerRedirect(innerUrl);
            step.Current = true;
        }
    }

    function updateTotals(cart) {
        $scope.checkout.Coupon = $scope.checkout.Coupon || cart.Coupon;
        $scope.checkout.Subtotal = cart.SubTotal;
        $scope.checkout.DiscountTotal = cart.DiscountTotal;
        $scope.checkout.ShippingTotal = cart.ShippingTotal;
        $scope.checkout.TaxTotal = cart.TaxTotal;
        $scope.checkout.Total = cart.Total;
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            var countries = response.data;
            $scope.checkout.Countries = countries;
        });
    }

    function getCountryRegions(countryCode) {
        $scope.checkout.SelectedCountry = _.find($scope.checkout.Countries, function (c) { return c.Code3 == countryCode });
        cartService.getCountryRegions(countryCode).then(function (response) {
            var countryRegions = response.data;
            $scope.checkout.CountryRegions = countryRegions || [];
        });
    }

    function getAvailableShippingMethods(shipmentId) {
        cartService.getAvailableShippingMethods().then(function (response) {
            var availableShippingMethods = response.data;
            $scope.checkout.AvailableShippingMethods = availableShippingMethods;
            if ($scope.checkout.Shipment && !$scope.checkout.Shipment.ShipmentMethodCode &&
                addressIsValid($scope.checkout.Shipment.DeliveryAddress) && availableShippingMethods.length == 1) {
                setShippingMethod(shipmentId, availableShippingMethods[0].ShipmentMethodCode);
            }
        });
    }

    function getAvailablePaymentMethods() {
        cartService.getAvailablePaymentMethods().then(function (response) {
            var availablePaymentMethods = response.data;
            $scope.checkout.AvailablePaymentMethods = availablePaymentMethods;
            if (availablePaymentMethods.length == 1) {
                $scope.checkout.Payment.PaymentGatewayCode = availablePaymentMethods[0].GatewayCode;
            }
        });
    }

    function setShippingMethod(shipmentId, shippingMethodCode) {
        $scope.checkout.ShippingMethodProcessing = true;
        cartService.addOrUpdateShipment(shipmentId, null, null, shippingMethodCode).then(function (response) {
            $scope.checkout.ShippingMethodProcessing = false;
            getCart();
        }, function (response) {
            $scope.checkout.ShippingMethodProcessing = false;
        });
    }

    function setBillingAddressEqualShipping() {
        $scope.checkout.Payment.BillingAddress = angular.copy($scope.checkout.Shipment.DeliveryAddress);
        $scope.checkout.Payment.BillingAddress.Type = billingAddressType;
    }

    function createOrder() {
        cartService.createOrder($scope.checkout.BankCardInfo).then(function (response) {
            var order = response.data.order;
            var orderProcessingResult = response.data.orderProcessingResult;
            handlePostPaymentResult(order, orderProcessingResult);
            $scope.checkout.OrderProcessing = false;
        }, function (response) {
            $scope.checkout.OrderProcessing = false;
        });
    }

    function addressesEqual(address1, address2) {
        var address1Type = address1.Type; address1.Type = null;
        var address2Type = address2.Type; address2.Type = null;
        var isEqual = angular.equals(address1, address2);
        address1.Type = address1Type;
        address2.Type = address2Type;
        return isEqual;
    }

    function validateBankCardNumber() {
        var bankCardNumber = $scope.checkout.BankCardInfo.CardNumber;
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

        $scope.checkout.BankCardInfo.Type = cardType;
        $scope.checkout.BankCardInfo.CardNumberPattern = cardNumberPattern;
        $scope.checkout.BankCardInfo.CvvPattern = cvvPattern;
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
        if (address && address.Email && address.FirstName && address.LastName && address.Line1 &&
            address.City && address.CountryName && address.PostalCode) {
            isValid = true;
        }
        return isValid;
    }
}]);