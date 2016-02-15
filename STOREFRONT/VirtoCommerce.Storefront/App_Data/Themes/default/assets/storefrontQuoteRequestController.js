var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('quoteRequestController', ['$scope', '$window', 'quoteRequestService', 'cartService', 'customerService',
    function ($scope, $window, quoteRequestService, cartService, customerService) {

    const shippingAddressType = 'Shipping';
    const billingAddressType = 'Billing';

    initialize();

    $scope.addItemToQuoteRequest = function (product, quantity) {
        $scope.quoteRequestIsUpdating = true;
        $scope.recentQuoteItemModalVisible = true;
        $scope.quoteRequest.RecentlyAddedItem = {
            ImageUrl: product.PrimaryImage.Url,
            ListPrice: product.Price.ListPrice,
            Name: product.Name,
            SalePrice: product.Price.SalePrice,
            Quantity: quantity
        };
        quoteRequestService.addItem(product.Id, quantity).then(function (response) {
            getQuoteRequest();
        }, function (response) {
            $scope.quoteRequestIsUpdating = false;
        });
    }

    $scope.removeItemFromQuoteRequest = function (quoteItemId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (!quoteItem || $scope.quoteRequestIsUpdating) {
            return;
        }
        $scope.quoteRequestIsUpdating = true;
        var initialItems = angular.copy($scope.quoteRequest.Items);
        $scope.quoteRequest.Items = _.without($scope.quoteRequest.Items, quoteItem);
        quoteRequestService.removeItem(quoteItemId).then(function (response) {
            getQuoteRequest();
        }, function (response) {
            $scope.quoteRequest.Items = initialItems;
            $scope.quoteRequestIsUpdating = false;
        });
    }

    $scope.setQuoteRequestForm = function (formQuoteRequest) {
        $scope.formQuoteRequest = formQuoteRequest;
    }

    $scope.setRequestShippingQuote = function (isRequired) {
        $scope.quoteRequest.RequestShippingQuote = !isRequired;
        if (!$scope.quoteRequest.RequestShippingQuote) {
            $scope.quoteRequest.ShippingAddress = null;
        } else {
            $scope.quoteRequest.ShippingAddress = { Type: shippingAddressType };
            if ($scope.quoteRequest.ShippingAddressEqualsBilling) {
                setShippingAddressEqualsBilling();
            }
        }
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.quoteRequest.Countries, function (c) { return c.Name == countryName });
        if (country) {
            var countryCode = country.Code3 || country.Code2;
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingAddress.RegionId = null;
                $scope.quoteRequest.ShippingAddress.RegionName = null;
                $scope.quoteRequest.ShippingAddress.CountryCode = countryCode;
                $scope.quoteRequest.ShippingCountryRegions = [];
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingAddress.RegionId = null;
                $scope.quoteRequest.BillingAddress.RegionName = null;
                $scope.quoteRequest.BillingAddress.CountryCode = countryCode;
                $scope.quoteRequest.BillingCountryRegions = [];
            }
            if (countryCode) {
                getCountryRegions(addressType, country.Code3);
            }
        }
    }

    $scope.setCountryRegion = function (addressType, countryRegionName) {
        var countryRegion = null;
        if (addressType == shippingAddressType) {
            countryRegion = _.find($scope.quoteRequest.ShippingCountryRegions, function (r) { return r.Name == countryRegionName });
        }
        if (addressType == billingAddressType) {
            countryRegion = _.find($scope.quoteRequest.BillingCountryRegions, function (r) { return r.Name == countryRegionName });
        }
        if (countryRegion) {
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingAddress.RegionId = countryRegion.Code;
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingAddress.RegionId = countryRegion.Code;
            }
        }
    }

    $scope.setShippingAddressEqualsBilling = function () {
        if ($scope.quoteRequest.ShippingAddressEqualsBilling) {
            setShippingAddressEqualsBilling();
        }
    }

    $scope.addTierPrice = function (quoteItemId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (quoteItem) {
            quoteItem.ProposalPrices.push({
                Id: quoteItem.ProposalPrices.length + 1,
                Price: quoteItem.SalePrice,
                Quantity: 1
            });
        }
    }

    $scope.removeTierPrice = function (quoteItemId, proposalPriceId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (quoteItem) {
            var proposalPrice = _.find(quoteItem.ProposalPrices, function (pp) { return pp.Id == proposalPriceId });
            if (proposalPrice) {
                quoteItem.ProposalPrices = _.without(quoteItem.ProposalPrices, proposalPrice);
            }
        }
    }

    $scope.stringifyAddress = function (address) {
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

    $scope.submitQuoteRequest = function () {
        $scope.quoteRequest.BillingAddress.Type = billingAddressType;
        $scope.quoteRequest.BillingAddress.Email = angular.copy($scope.quoteRequest.Email);
        var shippingAddress = null;
        if ($scope.quoteRequest.RequestShippingQuote) {
            $scope.quoteRequest.ShippingAddress.Type = shippingAddressType;
            $scope.quoteRequest.ShippingAddress.Email = angular.copy($scope.quoteRequest.Email);
        }
        var quoteRequest = toStorefrontModel($scope.quoteRequest);
        quoteRequest.Status = 'Processing';
        quoteRequest.Tag = null;
        $scope.formQuoteRequest.$setSubmitted();
        if ($scope.formQuoteRequest.$invalid) {
            return;
        }
        for (var i = 0; i < $scope.quoteRequest.Items.length; i++) {
            $scope.quoteRequest.Items[i].TierPricesAreUnique = true;
            if (!tierPricesAreUnique(quoteRequest.Items[i].ProposalPrices)) {
                $scope.quoteRequest.Items[i].TierPricesAreUnique = false;
            }
        }
        var ununiqueItems = _.where($scope.quoteRequest.Items, { TierPricesAreUnique: false });
        if (ununiqueItems.length) {
            return;
        }
        quoteRequestService.update(quoteRequest).then(function (response) {
            if ($scope.customer.IsRegisteredUser) {
                $scope.outerRedirect($scope.baseUrl + 'account/quoterequests/');
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/login/');
            }
        });
    }

    function initialize() {
        $scope.actualQuoteItemsCount = 0;
        $scope.quoteRequest = {
            Countries: [],
            ShippingCountryRegions: [],
            BillingCountryRegions: [],
        };
        $scope.quotesEnabled = $window.quotesEnabled;
        $scope.recentQuoteItemModalVisible = false;
        $scope.quoteRequestIsUpdating = false;
        getCurrentCustomer();
        getCurrentQuoteRequest();
        getCustomerQuoteRequest($window.quoteRequestNumber);
    }

    function getQuoteRequest(number) {
        $scope.quoteRequestIsUpdating = true;
        if (number) {
            quoteRequestService.getCustomerQuoteRequest(number).then(function (response) {
                toViewModel(response.data);
                $scope.quoteRequestIsUpdating = false;
            }, function (response) {
                $scope.quoteRequestIsUpdating = false;
            });
        } else {
            quoteRequestService.getCurrentQuoteRequest().then(function (response) {
                toViewModel(response.data);
                $scope.actualQuoteItemsCount = response.data.ItemsCount;
                $scope.quoteRequestIsUpdating = false;
            }, function (response) {
                $scope.quoteRequestIsUpdating = false;
            });
        }
    }

    function toViewModel(quoteRequest) {
        if (quoteRequest.BillingAddress) {
            quoteRequest.Email = quoteRequest.BillingAddress.Email;
            if (quoteRequest.BillingAddress.CountryCode) {
                getCountryRegions(billingAddressType, quoteRequest.BillingAddress.CountryCode);
            }
        } else {
            quoteRequest.Email = $scope.customer.Email;
            quoteRequest.BillingAddress = {
                Type: billingAddressType,
                FirstName: $scope.customer.FirstName,
                LastName: $scope.customer.LastName
            };
        }
        if (quoteRequest.ShippingAddress && quoteRequest.ShippingAddress.CountryCode) {
            getCountryRegions(shippingAddressType, quoteRequest.ShippingAddress.CountryCode);
        }
        for (var i = 0; i < quoteRequest.Items.length; i++) {
            var quoteItem = quoteRequest.Items[i];
            for (var j = 0; j < quoteItem.ProposalPrices.length; j++) {
                var proposalPrice = quoteItem.ProposalPrices[j];
                proposalPrice.Id = j + 1;
            }
        }
        quoteRequest.ShippingAddressEqualsBilling = addressesAreEqual(quoteRequest.BillingAddress, quoteRequest.ShippingAddress);
        $scope.quoteRequest = quoteRequest;
        getCountries();
    }

    //function getCurrentQuoteRequest() {
    //    $scope.quoteRequestIsUpdating = true;
    //    quoteRequestService.getCurrentQuoteRequest().then(function (response) {
    //        var quoteRequest = response.data;
    //        if (quoteRequest.BillingAddress) {
    //            quoteRequest.Email = quoteRequest.BillingAddress.Email;
    //            if (quoteRequest.BillingAddress.CountryCode) {
    //                getCountryRegions(billingAddressType, quoteRequest.BillingAddress.CountryCode);
    //            }
    //        } else {
    //            quoteRequest.Email = $scope.customer.Email;
    //            quoteRequest.BillingAddress = {
    //                Type: billingAddressType,
    //                FirstName: $scope.customer.FirstName,
    //                LastName: $scope.customer.LastName
    //            };
    //        }
    //        if (quoteRequest.ShippingAddress && quoteRequest.ShippingAddress.CountryCode) {
    //            getCountryRegions(shippingAddressType, quoteRequest.ShippingAddress.CountryCode);
    //        }
    //        for (var i = 0; i < quoteRequest.Items.length; i++) {
    //            var quoteItem = quoteRequest.Items[i];
    //            for (var j = 0; j < quoteItem.ProposalPrices.length; j++) {
    //                var proposalPrice = quoteItem.ProposalPrices[j];
    //                proposalPrice.Id = j + 1;
    //            }
    //        }
    //        quoteRequest.ShippingAddressEqualsBilling = addressesAreEqual(quoteRequest.BillingAddress, quoteRequest.ShippingAddress);
    //        $scope.quoteRequest = quoteRequest;
    //        getCountries();
    //        $scope.quoteRequestIsUpdating = false;
    //    }, function (response) {
    //        $scope.quoteRequestIsUpdating = false;
    //    });
    //}

    function getCurrentCustomer() {
        customerService.getCurrentCustomer().then(function (response) {
            var customer = response.data;
            $scope.customer = customer;
        });
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            var countries = response.data;
            $scope.quoteRequest.Countries = countries;
        });
    }

    function getCountryRegions(addressType, countryCode) {
        var country = _.find($scope.quoteRequest.Countries, function (c) { return c.Code3 == countryCode });
        if (addressType == shippingAddressType) {
            $scope.quoteRequest.SelectedShippingCountry = country;
        }
        if (addressType == billingAddressType) {
            $scope.quoteRequest.SelectedBillingCountry = country;
        }
        cartService.getCountryRegions(countryCode).then(function (response) {
            var countryRegions = response.data;
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingCountryRegions = countryRegions || [];
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingCountryRegions = countryRegions || [];
            }
        });
    }

    function addressesAreEqual(address1, address2) {
        var isEqual = false;
        if (address1 && address2) {
            var address1Type = address1.Type; address1.Type = null;
            var address2Type = address2.Type; address2.Type = null;
            isEqual = angular.equals(address1, address2);
            address1.Type = address1Type;
            address2.Type = address2Type;
        }
        return isEqual;
    }

    function tierPricesAreUnique(tierPrices) {
        var uniqueTierPrices = _.uniq(tierPrices, 'Quantity');
        return uniqueTierPrices.length == tierPrices.length;
    }

    function setShippingAddressEqualsBilling() {
        $scope.quoteRequest.ShippingAddress = angular.copy($scope.quoteRequest.BillingAddress);
        $scope.quoteRequest.ShippingAddress.Type = shippingAddressType;
        if ($scope.quoteRequest.ShippingAddress.CountryCode) {
            getCountryRegions(shippingAddressType, $scope.quoteRequest.ShippingAddress.CountryCode);
        }
    }

    function toStorefrontModel(fullQuoteRequest) {
        var quoteRequest = {
            Id: fullQuoteRequest.Id,
            Tag: fullQuoteRequest.Tag,
            Comment: fullQuoteRequest.Comment,
            BillingAddress: fullQuoteRequest.BillingAddress,
            ShippingAddress: fullQuoteRequest.ShippingAddress,
            Items: []
        };
        for (var i = 0; i < fullQuoteRequest.Items.length; i++) {
            var quoteItem = {
                Id: fullQuoteRequest.Items[i].Id,
                Comment: fullQuoteRequest.Items[i].Comment,
                SelectedTierPrice: fullQuoteRequest.Items[i].SelectedTierPrice,
                ProposalPrices: []
            };
            for (var j = 0; j < fullQuoteRequest.Items[i].ProposalPrices.length; j++) {
                quoteItem.ProposalPrices.push({
                    Price: fullQuoteRequest.Items[i].ProposalPrices[j].Price,
                    Quantity: fullQuoteRequest.Items[i].ProposalPrices[j].Quantity
                });
            }
            quoteRequest.Items.push(quoteItem);
        }
        return quoteRequest;
    }


/*
    initialize();

    $scope.trustedHtml = function (html) {
        return $sce.trustAsHtml(html);
    }

    $scope.setQuoteRequestForm = function (form) {
        $scope.formQuoteRequest = form;
    }

    $scope.toggleRecentQuoteItemModal = function (isVisible) {
        $scope.recentQuoteItemModalVisible = !isVisible;
    }

    $scope.setShippingQuoteRequired = function (isRequired) {
        $scope.quoteRequest.RequestShippingQuote = !isRequired;
        if (!$scope.quoteRequest.RequestShippingQuote) {
            $scope.quoteRequest.ShippingAddress = null;
        } else {
            $scope.quoteRequest.ShippingAddress = { Type: shippingAddressType };
            if ($scope.quoteRequest.ShippingAddressEqualsBilling) {
                setShippingAddressEqualsBilling();
            }
        }
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.quoteRequest.Countries, function (c) { return c.Name == countryName });
        if (country) {
            var countryCode = country.Code3 || country.Code2;
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingAddress.RegionId = null;
                $scope.quoteRequest.ShippingAddress.RegionName = null;
                $scope.quoteRequest.ShippingAddress.CountryCode = countryCode;
                $scope.quoteRequest.ShippingCountryRegions = [];
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingAddress.RegionId = null;
                $scope.quoteRequest.BillingAddress.RegionName = null;
                $scope.quoteRequest.BillingAddress.CountryCode = countryCode;
                $scope.quoteRequest.BillingCountryRegions = [];
            }
            if (countryCode) {
                getCountryRegions(addressType, country.Code3);
            }
        }
    }

    $scope.setCountryRegion = function (addressType, countryRegionName) {
        var countryRegion = null;
        if (addressType == shippingAddressType) {
            countryRegion = _.find($scope.quoteRequest.ShippingCountryRegions, function (r) { return r.Name == countryRegionName });
        }
        if (addressType == billingAddressType) {
            countryRegion = _.find($scope.quoteRequest.BillingCountryRegions, function (r) { return r.Name == countryRegionName });
        }
        if (countryRegion) {
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingAddress.RegionId = countryRegion.Code;
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingAddress.RegionId = countryRegion.Code;
            }
        }
    }

    $scope.addToQuoteRequest = function (product, quantity) {
        $scope.quoteRequestIsUpdating = true;
        $scope.recentQuoteItemModalVisible = true;
        $scope.quoteRequest.RecentlyAddedItem = {
            ImageUrl: product.PrimaryImage.Url,
            ListPrice: product.Price.ListPrice,
            Name: product.Name,
            SalePrice: product.Price.SalePrice,
            Quantity: quantity
        };
        quoteRequestService.addItem(product.Id, quantity).then(function (response) {
            refreshCurrentQuoteRequest();
        }, function (response) {
            $scope.quoteRequestIsUpdating = false;
        });
    }

    $scope.removeFromQuoteRequest = function (quoteItemId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (!quoteItem || $scope.quoteRequestIsUpdating) {
            return;
        }
        $scope.quoteRequestIsUpdating = true;
        var initialItems = angular.copy($scope.quoteRequest.Items);
        $scope.quoteRequest.Items = _.without($scope.quoteRequest.Items, quoteItem);
        quoteRequestService.removeItem(quoteItemId).then(function (response) {
            refreshCurrentQuoteRequest();
        }, function (response) {
            $scope.quoteRequest.Items = initialItems;
            $scope.quoteRequestIsUpdating = false;
        });
    }

    $scope.updateProposalPrice = function (quoteItemId, proposalPriceId, quantity) {
        if (quantity < 1) {
            return;
        }
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (quoteItem) {
            var proposalPrice = _.find(quoteItem.ProposalPrices, function (pp) { return pp.Id == proposalPriceId });
            if (proposalPrice) {
                proposalPrice.Quantity = quantity;
            }
        }
    }

    $scope.addProposalPrice = function (quoteItemId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (quoteItem) {
            quoteItem.ProposalPrices.push({
                Id: quoteItem.ProposalPrices.length + 1,
                Price: quoteItem.SalePrice,
                Quantity: 1
            });
        }
    }

    $scope.removeProposalPrice = function (quoteItemId, proposalPriceId) {
        var quoteItem = _.find($scope.quoteRequest.Items, function (i) { return i.Id == quoteItemId });
        if (quoteItem) {
            var proposalPrice = _.find(quoteItem.ProposalPrices, function (pp) { return pp.Id == proposalPriceId });
            if (proposalPrice) {
                quoteItem.ProposalPrices = _.without(quoteItem.ProposalPrices, proposalPrice);
            }
        }
    }

    $scope.setShippingAddressEqualsBilling = function () {
        if ($scope.quoteRequest.ShippingAddressEqualsBilling) {
            setShippingAddressEqualsBilling();
        }
    }

    $scope.getQuoteRequestTotals = function (quoteItemId, proposalPrice) {
        var selectedQuoteItem = _.find($scope.customerQuoteRequest.Items, function (i) { return i.Id == quoteItemId});
        if (selectedQuoteItem) {
            var price = {
                Price: proposalPrice.Price.Amount,
                Quantity: proposalPrice.Quantity
            };
            quoteRequestService.getQuoteRequestTotals($scope.customerQuoteRequest.Id, quoteItemId, price).then(function (response) {
                var quoteRequestTotals = response.data;
                $scope.customerQuoteRequest.Totals = quoteRequestTotals;
                selectedQuoteItem.SelectedTierPrice = proposalPrice;
            });
        }
    }

    $scope.submitQuoteRequest = function () {
        $scope.quoteRequest.BillingAddress.Type = billingAddressType;
        $scope.quoteRequest.BillingAddress.Email = angular.copy($scope.quoteRequest.Email);
        var shippingAddress = null;
        if ($scope.quoteRequest.RequestShippingQuote) {
            $scope.quoteRequest.ShippingAddress.Type = shippingAddressType;
            $scope.quoteRequest.ShippingAddress.Email = angular.copy($scope.quoteRequest.Email);
        }
        var quoteRequest = toJsonModel($scope.quoteRequest);
        quoteRequest.Tag = $scope.customer.IsRegisteredUser ? null : 'actual';
        $scope.formQuoteRequest.$setSubmitted();
        if ($scope.formQuoteRequest.$invalid) {
            return;
        }
        for (var i = 0; i < $scope.quoteRequest.Items.length; i++) {
            $scope.quoteRequest.Items[i].TierPricesAreUnique = true;
            if (!tierPricesAreUnique(quoteRequest.Items[i].ProposalPrices)) {
                $scope.quoteRequest.Items[i].TierPricesAreUnique = false;
            }
        }
        var ununiqueItems = _.where($scope.quoteRequest.Items, { TierPricesAreUnique: false });
        if (ununiqueItems.length) {
            return;
        }
        quoteRequestService.update(quoteRequest).then(function (response) {
            if ($scope.customer.IsRegisteredUser) {
            	$scope.outerRedirect($scope.baseUrl + 'quoterequest/quote-requests/');
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/login/');
            }
        });
    }

    function tierPricesAreUnique(tierPrices) {
        var uniqueTierPrices = _.uniq(tierPrices, 'Quantity');
        return uniqueTierPrices.length == tierPrices.length;
    }

    function initialize() {
        $scope.quoteRequest = {
            BillingAddress: { Type: billingAddressType },
            ShippingAddress: null,
            Countries: [],
            ShippingCountryRegions: [],
            BillingCountryRegions: [],
            RequestShippingQuote: false,
            ShippingAddressEqualsBilling: true
        };
        $scope.quotesEnabled = $window.quotesEnabled;
        $scope.recentQuoteItemModalVisible = false;
        if ($window.quotesEnabled) {
            refreshCurrentQuoteRequest();
        }
        getCurrentCustomer();
        getQuoteRequestByNumber($window.quoteRequestNumber);
    }

    function getCurrentCustomer() {
        customerService.getCurrentCustomer().then(function (response) {
            var customer = response.data;
            $scope.customer = customer;
        });
    }

    function setShippingAddressEqualsBilling() {
        $scope.quoteRequest.ShippingAddress = angular.copy($scope.quoteRequest.BillingAddress);
        $scope.quoteRequest.ShippingAddress.Type = shippingAddressType;
        if ($scope.quoteRequest.ShippingAddress.CountryCode) {
            getCountryRegions(shippingAddressType, $scope.quoteRequest.ShippingAddress.CountryCode);
        }
    }

    function getQuoteRequestByNumber(quoteRequestNumber) {
        if (!quoteRequestNumber) {
            return;
        }
        quoteRequestService.getQuoteRequestByNumber(quoteRequestNumber).then(function (response) {
            var quoteRequest = response.data;
            for (var i = 0; i < quoteRequest.Items.length; i++) {
                var quoteItem = quoteRequest.Items[i];
                for (var j = 0; j < quoteItem.ProposalPrices.length; j++) {
                    var proposalPrice = quoteItem.ProposalPrices[j];
                    proposalPrice.Id = j + 1;
                }
                var selectedTierPrice = _.find(quoteItem.ProposalPrices, function (pp) { return pp.Quantity == quoteItem.SelectedTierPrice.Quantity });
                quoteItem.SelectedTierPrice.Id = selectedTierPrice.Id;
            }
            $scope.customerQuoteRequest = quoteRequest;
        });
    }

    function refreshCurrentQuoteRequest() {
        $scope.quoteRequestIsUpdating = true;
        quoteRequestService.getCurrentQuoteRequest().then(function (response) {
            $scope.quoteRequestIsUpdating = false;
            var quoteRequest = response.data;
            if (quoteRequest.BillingAddress) {
                quoteRequest.Email = quoteRequest.BillingAddress.Email;
                if (quoteRequest.BillingAddress.CountryCode) {
                    getCountryRegions(billingAddressType, quoteRequest.BillingAddress.CountryCode);
                }
            } else {
                quoteRequest.Email = $scope.customer.Email;
                quoteRequest.BillingAddress = {
                    Type: billingAddressType,
                    FirstName: $scope.customer.FirstName,
                    LastName: $scope.customer.LastName
                };
            }
            if (quoteRequest.ShippingAddress && quoteRequest.ShippingAddress.CountryCode) {
                getCountryRegions(shippingAddressType, quoteRequest.ShippingAddress.CountryCode);
            }
            for (var i = 0; i < quoteRequest.Items.length; i++) {
                var quoteItem = quoteRequest.Items[i];
                for (var j = 0; j < quoteItem.ProposalPrices.length; j++) {
                    var proposalPrice = quoteItem.ProposalPrices[j];
                    proposalPrice.Id = j + 1;
                }
            }
            quoteRequest.ShippingAddressEqualsBilling = addressesEqual(quoteRequest.BillingAddress, quoteRequest.ShippingAddress);
            $scope.quoteRequest = quoteRequest;
            getCountries();
        });
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            var countries = response.data;
            $scope.quoteRequest.Countries = countries;
        });
    }

    function getCountryRegions(addressType, countryCode) {
        var country = _.find($scope.quoteRequest.Countries, function (c) { return c.Code3 == countryCode });
        if (addressType == shippingAddressType) {
            $scope.quoteRequest.SelectedShippingCountry = country;
        }
        if (addressType == billingAddressType) {
            $scope.quoteRequest.SelectedBillingCountry = country;
        }
        cartService.getCountryRegions(countryCode).then(function (response) {
            var countryRegions = response.data;
            if (addressType == shippingAddressType) {
                $scope.quoteRequest.ShippingCountryRegions = countryRegions || [];
            }
            if (addressType == billingAddressType) {
                $scope.quoteRequest.BillingCountryRegions = countryRegions || [];
            }
        });
    }

    function addressesEqual(address1, address2) {
        var isEqual = false;
        if (address1 && address2) {
            var address1Type = address1.Type; address1.Type = null;
            var address2Type = address2.Type; address2.Type = null;
            isEqual = angular.equals(address1, address2);
            address1.Type = address1Type;
            address2.Type = address2Type;
        }
        return isEqual;
    }

    function toJsonModel(fullQuoteRequest) {
        var quoteRequest = {
            Id: fullQuoteRequest.Id,
            Tag: fullQuoteRequest.Tag,
            Comment: fullQuoteRequest.Comment,
            BillingAddress: fullQuoteRequest.BillingAddress,
            ShippingAddress: fullQuoteRequest.ShippingAddress,
            Items: []
        };
        for (var i = 0; i < fullQuoteRequest.Items.length; i++) {
            var quoteItem = {
                Id: fullQuoteRequest.Items[i].Id,
                Comment: fullQuoteRequest.Items[i].Comment,
                SelectedTierPrice: fullQuoteRequest.Items[i].SelectedTierPrice,
                ProposalPrices: []
            };
            for (var j = 0; j < fullQuoteRequest.Items[i].ProposalPrices.length; j++) {
                quoteItem.ProposalPrices.push({
                    Price: fullQuoteRequest.Items[i].ProposalPrices[j].Price,
                    Quantity: fullQuoteRequest.Items[i].ProposalPrices[j].Quantity
                });
            }
            quoteRequest.Items.push(quoteItem);
        }
        return quoteRequest;
    }
*/
}]);