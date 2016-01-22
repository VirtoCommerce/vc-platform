var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('quoteRequestController', ['$scope', '$window', 'quoteRequestService', function ($scope, $window, quoteRequestService) {
    initialize();

    $scope.toggleRecentQuoteItemModal = function (isVisible) {
        $scope.recentQuoteItemModalVisible = !isVisible;
    }

    $scope.addToQuoteRequest = function (product, quantity) {
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
        });
    }

    function initialize() {
        $scope.quoteRequest = {};
        $scope.quotesEnabled = $window.quotesEnabled;
        $scope.recentQuoteItemModalVisible = false;
        if ($window.quotesEnabled) {
            refreshCurrentQuoteRequest();
        }
    }

    function refreshCurrentQuoteRequest() {
        $scope.quoteRequestIsUpdating = true;
        quoteRequestService.getCurrentQuoteRequest().then(function (response) {
            var quoteRequest = response.data;
            $scope.quoteRequest = quoteRequest;
            $scope.quoteRequestIsUpdating = false;
        });
    }
}]);