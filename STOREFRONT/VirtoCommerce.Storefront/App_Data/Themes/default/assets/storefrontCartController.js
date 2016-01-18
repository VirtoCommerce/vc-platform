var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('cartController', ['$scope', '$timeout', 'cartService', function ($scope, $timeout, cartService) {
    var timer;

    initialize();

    $scope.toggleRecentCartItemModal = function (isVisible) {
        $scope.recentCartItemModalVisible = !isVisible;
    }

    $scope.addToCart = function (productId, quantity) {
        $scope.recentCartItemModalVisible = true;
        $scope.cart.RecentlyAddedItem = null;
        cartService.addLineItem(productId, quantity).then(function (response) {
            refreshCart();
        }, function (response) {
            showErrorMessage();
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
                showErrorMessage();
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
            showErrorMessage();
        });
    }

    $scope.reapplyLineItem = function (lineItemId) {
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem) {
            return;
        }
        $scope.cartIsUpdating = true;
        var quantity = lineItem.Quantity;
        var productId = lineItem.ProductId;
        cartService.removeLineItem(lineItemId).then(function (response) {
            cartService.addLineItem(productId, quantity).then(function (response) {
                refreshCart();
            });
        });
    }

    function initialize() {
        $scope.cart = {};
        $scope.recentCartItemModalVisible = false;
        $scope.cartErrorOccured = false;
        refreshCart();
    }

    function refreshCart() {
        $scope.cartIsUpdating = true;
        promises = [];
        cartService.getCart().then(function (response) {
            $scope.cart = response.data;
            $scope.cartIsUpdating = false;
        }, function (response) {
            showErrorMessage();
        });
    }

    function showErrorMessage(message) {
        $scope.cartErrorOccured = true;
        $scope.cartIsUpdating = false;
        $scope.cartErrorMessage = message;
        $scope.cartErrorDetails = message;
    }
}]);