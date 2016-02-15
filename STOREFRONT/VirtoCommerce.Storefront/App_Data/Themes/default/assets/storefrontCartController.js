var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('cartController', ['$scope', '$timeout', 'cartService', function ($scope, $timeout, cartService) {
    var timer;

    initialize();

    $scope.setCartForm = function (form) {
        $scope.formCart = form;
    }

    $scope.toggleRecentCartItemModal = function (isVisible) {
        $scope.recentCartItemModalVisible = !isVisible;
    }

    $scope.addToCart = function (product, quantity) {
        $scope.cartIsUpdating = true;
        $scope.recentCartItemModalVisible = true;
        $scope.cart.RecentlyAddedItem = {
            ImageUrl: product.PrimaryImage.Url,
            ListPrice: product.Price.ListPrice,
            Name: product.Name,
            PlacedPrice: product.Price.ActualPrice,
            Quantity: quantity
        };
        cartService.addLineItem(product.Id, quantity).then(function (response) {
            refreshCart();
        });
    }

    $scope.changeLineItemQuantity = function (lineItemId, quantity) {
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem || quantity < 1 || $scope.cartIsUpdating || $scope.formCart.$invalid) {
            return;
        }
        var initialQuantity = angular.copy(lineItem.Quantity);
        lineItem.Quantity = quantity;
        $timeout.cancel(timer);
        timer = $timeout(function () {
            $scope.cartIsUpdating = true;
            cartService.changeLineItemQuantity(lineItemId, quantity).then(function (response) {
                refreshCart();
            }, function (response) {
                lineItem.Quantity = initialQuantity;
            });
        }, 300);
    }

    $scope.removeLineItem = function (lineItemId) {
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem || $scope.cartIsUpdating) {
            return;
        }
        $scope.cartIsUpdating = true;
        var initialItems = angular.copy($scope.cart.Items);
        $scope.recentCartItemModalVisible = false;
        $scope.cart.Items = _.without($scope.cart.Items, lineItem);
        cartService.removeLineItem(lineItemId).then(function (response) {
            refreshCart();
        }, function (response) {
            $scope.cart.Items = initialItems;
            $scope.cartIsUpdating = false;
        });
    }

    $scope.submitCart = function () {
        $scope.formCart.$setSubmitted();
        if ($scope.formCart.$invalid) {
            return;
        }
        if ($scope.cart.HasPhysicalProducts) {
            $scope.outerRedirect($scope.baseUrl + 'cart/checkout/#/shipping-address');
        } else {
            $scope.outerRedirect($scope.baseUrl + 'cart/checkout/#/payment-method');
        }
    }

    function initialize() {
        $scope.cart = {};
        $scope.recentCartItemModalVisible = false;
        $scope.cartErrorOccured = false;
        refreshCart();
    }

    function refreshCart() {
        $scope.cartIsUpdating = true;
        cartService.getCart().then(function (response) {
            $scope.cart = response.data;
            $scope.cartIsUpdating = false;
        });
    }
}]);