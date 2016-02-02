var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('quoteRequestController', ['$scope', '$window', 'quoteRequestService', 'cartService', 'customerService',
    function ($scope, $window, quoteRequestService, cartService, customerService) {
    const shippingAddressType = 'Shipping';
    const billingAddressType = 'Billing';

    initialize();

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

    $scope.submitQuoteRequest = function () {
        $scope.quoteRequest.BillingAddress.Type = billingAddressType;
        $scope.quoteRequest.BillingAddress.Email = angular.copy($scope.quoteRequest.Email);
        var shippingAddress = null;
        if ($scope.quoteRequest.RequestShippingQuote) {
            $scope.quoteRequest.ShippingAddress.Type = shippingAddressType;
            $scope.quoteRequest.ShippingAddress.Email = angular.copy($scope.quoteRequest.Email);
        }
        var quoteRequest = {
            Comment: $scope.quoteRequest.Comment,
            Tag: $scope.customer.IsRegisteredUser ? null : 'actual',
            BillingAddress: $scope.quoteRequest.BillingAddress,
            ShippingAddress: $scope.quoteRequest.ShippingAddress,
            Items: []
        };
        for (var i = 0; i < $scope.quoteRequest.Items.length; i++) {
            var quoteItem = $scope.quoteRequest.Items[i];
            var proposalPrices = [];
            for (var j = 0; j < quoteItem.ProposalPrices.length; j++) {
                var proposalPrice = quoteItem.ProposalPrices[j];
                proposalPrices.push({
                    Price: quoteItem.SalePrice.Amount,
                    Quantity: proposalPrice.Quantity
                });
            }
            quoteRequest.Items.push({
                Id: quoteItem.Id,
                Comment: quoteItem.Comment,
                ProposalPrices: proposalPrices
            });
        }
        $scope.formQuoteRequest.$setSubmitted();
        if ($scope.formQuoteRequest.$invalid) {
            return;
        }
        quoteRequestService.update(quoteRequest).then(function (response) {
            if ($scope.customer.IsRegisteredUser) {
                $scope.outerRedirect($scope.baseUrl + 'account/quote-requests/');
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/login/');
            }
        });
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
}]);