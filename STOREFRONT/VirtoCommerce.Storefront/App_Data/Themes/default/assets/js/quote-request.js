var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('quoteRequestController', ['$rootScope', '$scope', '$location', 'quoteRequestService', 'cartService',
    function ($rootScope, $scope, $location, quoteRequestService, cartService) {
    initialize();

    $scope.setQuoteRequestForm = function (form) {
        $scope.formQuoteRequest = form;
    }

    $scope.displayForStatuses = function (statuses) {
        return _.contains(statuses, $scope.quoteRequest.Status);
    }

    $scope.addTierPrice = function (quoteItem) {
        quoteItem.ProposalPrices.push({
            Id: quoteItem.ProposalPrices.length + 1,
            Price: quoteItem.SalePrice,
            Quantity: 1
        });
    }

    $scope.changeTierPriceQuantity = function (tierPrice, quantity) {
        if (quantity < 1 || quantity.isNaN) {
            return;
        }
        tierPrice.Quantity = quantity;
    }

    $scope.removeTierPrice = function (quoteItem, tierPrice) {
        quoteItem.ProposalPrices = _.without(quoteItem.ProposalPrices, tierPrice);
    }

    $scope.removeProductFromQuoteRequest = function (quoteItem) {
        var initialQuoteItems = angular.copy($scope.quoteRequest.Items);
        $scope.quoteRequest.Items = _.without($scope.quoteRequest.Items, quoteItem);
        quoteRequestService.removeProductFromQuoteRequest(quoteItem.Id).then(function (response) {
            getQuoteRequest($scope.quoteRequest.Id);
            $rootScope.$broadcast('actualQuoteRequestItemsChanged');
        }, function (response) {
            $scope.quoteRequest.Items = initialQuoteItems;
        });
    }

    $scope.setCountry = function (addressType, countryName) {
        var country = _.find($scope.countries, function (c) { return c.Name == countryName });
        if (!country) {
            return;
        }
        if (addressType == 'Billing') {
            $scope.billingCountry = country;
            $scope.billingCountryRegions = [];
            $scope.quoteRequest.BillingAddress.CountryCode = country.Code3 || country.Code2;
            $scope.quoteRequest.BillingAddress.RegionId = null;
            $scope.quoteRequest.BillingAddress.RegionName = null;
        }
        if (addressType == 'Shipping') {
            $scope.shippingCountry = country;
            $scope.shippingCountryRegions = [];
            $scope.quoteRequest.ShippingAddress.CountryCode = country.Code3 || country.Code2;
            $scope.quoteRequest.ShippingAddress.RegionId = null;
            $scope.quoteRequest.ShippingAddress.RegionName = null;
        }
        if (country.Code3) {
            getCountryRegions(addressType, country.Code3);
        }
    }

    $scope.setCountryRegion = function (addressType) {
        if (addressType == 'Billing') {
            var countryRegion = _.find($scope.billingCountryRegions, function (r) { return r.Name == $scope.quoteRequest.BillingAddress.RegionName });
            if (!countryRegion) {
                return;
            }
            $scope.quoteRequest.BillingAddress.RegionId = countryRegion.Code;
        }
        if (addressType == 'Shipping') {
            var countryRegion = _.find($scope.shippingCountryRegions, function (r) { return r.Name == $scope.quoteRequest.ShippingAddress.RegionName });
            if (!countryRegion) {
                return;
            }
            $scope.quoteRequest.ShippingAddress.RegionId = countryRegion.Code;
        }
    }

    $scope.stringifyAddress = function (address) {
        if (!address) {
            return;
        }
        var stringifiedAddress = address.FirstName + ' ' + address.LastName + ', ';
        stringifiedAddress += address.Organization ? address.Organization + ', ' : '';
        stringifiedAddress += address.CountryName + ', ';
        stringifiedAddress += address.RegionName ? address.RegionName + ', ' : '';
        stringifiedAddress += address.City;
        stringifiedAddress += address.Line1 + ', '
        stringifiedAddress += address.Line2 ? address.Line2 : '';
        stringifiedAddress += address.PostalCode;
        return stringifiedAddress;
    }

    $scope.submitQuoteRequest = function () {
        $scope.formQuoteRequest.$setSubmitted();
        if ($scope.formQuoteRequest.$invalid) {
            return;
        }
        _.each($scope.quoteRequest.Items, function (item) {
            if (!$scope.tierPricesUnique(item)) {
                return;
            }
        });
        $scope.quoteRequest.BillingAddress.Email = $scope.quoteRequest.Email;
        if ($scope.quoteRequest.ShippingAddress) {
            $scope.quoteRequest.ShippingAddress.Email = $scope.quoteRequest.Email;
        }
        quoteRequestService.submitQuoteRequest(toFormModel($scope.quoteRequest)).then(function (response) {
            if ($scope.customer.IsRegisteredUser) {
                $scope.outerRedirect($scope.baseUrl + 'account/quoterequests');
            } else {
                $scope.outerRedirect($scope.baseUrl + 'account/login');
            }
        });
    }

    $scope.rejectQuoteRequest = function () {
        quoteRequestService.rejectQuoteRequest($scope.quoteRequest.Id).then(function (response) {
            $scope.outerRedirect($scope.baseUrl + 'quoterequest/#/' + $scope.quoteRequest.Id);
        });
    }

    $scope.selectTierPrice = function () {
        quoteRequestService.getTotals(toFormModel($scope.quoteRequest)).then(function (response) {
            $scope.quoteRequest.Totals = response.data;
        });
    }

    $scope.confirmQuoteRequest = function () {
        quoteRequestService.confirmQuoteRequest(toFormModel($scope.quoteRequest)).then(function (response) {
            $scope.outerRedirect($scope.baseUrl + 'cart/checkout/#/shipping-address');
        });
    }

    $scope.setShippingAddressEqualsBilling = function () {
        if ($scope.quoteRequest.ShippingAddressEqualsBilling) {
            $scope.quoteRequest.ShippingAddress = angular.copy($scope.quoteRequest.BillingAddress);
            $scope.quoteRequest.ShippingAddress.Type = 'Shipping';
            if ($scope.quoteRequest.ShippingAddress.CountryCode) {
                $scope.shippingCountry = $scope.billingCountry;
                getCountryRegions('Shipping', $scope.quoteRequest.ShippingAddress.CountryCode);
            }
        }
    }

    $scope.tierPricesUnique = function (quoteItem) {
        var quantities = _.map(quoteItem.ProposalPrices, function (p) { return p.Quantity });
        return _.uniq(quantities).length == quoteItem.ProposalPrices.length;
    }

    function initialize() {
        var quoteRequestNumber = $location.url().replace('/', '');
        $scope.quoteRequest = { ItemsCount: 0 };
        $scope.billingCountry = null;
        $scope.shippingCountry = null;
        getCountries();
        if (quoteRequestNumber) {
            getQuoteRequest(quoteRequestNumber);
        }
    }

    function getQuoteRequest(number) {
        quoteRequestService.getQuoteRequest(number).then(function (response) {
            var quoteRequest = response.data;
            if (!quoteRequest.BillingAddress) {
                quoteRequest.BillingAddress = {
                    FirstName: $scope.customer.FirstName,
                    LastName: $scope.customer.LastName
                };
            }
            _.each(quoteRequest.Items, function (quoteItem) {
                var i = 1;
                _.each(quoteItem.ProposalPrices, function (tierPrice) {
                    tierPrice.Id = i;
                    if (quoteItem.SelectedTierPrice.Quantity == tierPrice.Quantity) {
                        quoteItem.SelectedTierPrice.Id = i;
                    }
                    i++;
                });
            });
            quoteRequest.RequestShippingQuote = true;
            $scope.quoteRequest = quoteRequest;
        });
    }

    function getCountries() {
        cartService.getCountries().then(function (response) {
            $scope.countries = response.data;
        });
    }

    function getCountryRegions(addressType, countryCode) {
        cartService.getCountryRegions(countryCode).then(function (response) {
            var countryRegions = response.data;
            if (addressType == 'Billing') {
                $scope.billingCountryRegions = countryRegions || [];
            }
            if (addressType == 'Shipping') {
                $scope.shippingCountryRegions = countryRegions || [];
            }
        });
    }

    function toFormModel(quoteRequest) {
        var quoteRequestFormModel = {
            Id: quoteRequest.Id,
            Tag: quoteRequest.Tag,
            Status: quoteRequest.Status,
            Comment: quoteRequest.Comment,
            BillingAddress: quoteRequest.BillingAddress,
            ShippingAddress: quoteRequest.ShippingAddress,
            Items: []
        };
        _.each(quoteRequest.Items, function (quoteItem) {
            var quoteItemFormModel = {
                Id: quoteItem.Id,
                Comment: quoteItem.Comment,
                SelectedTierPrice: {
                    Price: quoteItem.SelectedTierPrice.Price.Amount,
                    Quantity: quoteItem.SelectedTierPrice.Quantity
                },
                ProposalPrices: []
            };
            _.each(quoteItem.ProposalPrices, function (tierPrice) {
                quoteItemFormModel.ProposalPrices.push({
                    Price: tierPrice.Price.Amount,
                    Quantity: tierPrice.Quantity
                });
            });
            quoteRequestFormModel.Items.push(quoteItemFormModel);
        });

        return quoteRequestFormModel;
    }
}]);

storefrontApp.controller('actualQuoteRequestBarController', ['$scope', 'quoteRequestService', function ($scope, quoteRequestService) {
    getActualQuoteRequest();

    $scope.$on('actualQuoteRequestItemsChanged', function (event, data) {
        getActualQuoteRequest();
    });

    function getActualQuoteRequest() {
        quoteRequestService.getActualQuoteRequest().then(function (response) {
            $scope.actualQuoteRequest = response.data;
        });
    }
}]);

storefrontApp.controller('recentlyAddedActualQuoteRequestItemDialogController', ['$scope', '$window', '$uibModalInstance', 'dialogData',
    function ($scope, $window, $uibModalInstance, dialogData) {
    $scope.dialogData = dialogData;

    $scope.close = function () {
        $uibModalInstance.close();
    }

    $scope.redirect = function (url) {
        $window.location = url;
    }
}]);