var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('cartController', ['$scope', '$timeout', 'cartService', function ($scope, $timeout, cartService) {
    var timer;
    $scope.cart = {};

    initialize();

    $scope.showCartModal = function () {
        $scope.isCartModalVisible = true;
    }

    $scope.hideCartModal = function () {
        $scope.isCartModalVisible = false;
    }

    $scope.addToCart = function (productId, quantity) {
        var initialItems = angular.copy($scope.cart.Items);
        $scope.isCartModalVisible = true;
        $scope.isUpdating = true;
        cartService.addLineItem(productId, quantity).then(
            function (response) {
                refreshCart();
            },
            function (response) {
                $scope.cart.Items = initialItems;
                showErrorMessage(2000);
            });
    }

    $scope.changeLineItem = function (lineItemId, quantity) {
        if (quantity < 1) {
            return;
        }
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem) {
            return;
        }
        var initialQuantity = angular.copy(lineItem.Quantity);
        lineItem.Quantity = quantity;
        $timeout.cancel(timer);
        timer = $timeout(function () {
            $scope.isUpdating = true;
            cartService.changeLineItem(lineItemId, quantity).then(
                function (response) {
                    refreshCart();
                },
                function (response) {
                    lineItem.Quantity = initialQuantity;
                    showErrorMessage(2000);
                });
        }, 200);
    }

    $scope.removeLineItem = function (lineItemId) {
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem) {
            return;
        }
        var initialItems = angular.copy($scope.cart.Items);
        $scope.cart.Items = _.without($scope.cart.Items, lineItem);
        $timeout.cancel(timer);
        timer = $timeout(function () {
            $scope.isUpdating = true;
            cartService.removeLineItem(lineItemId).then(
                function (response) {
                    refreshCart();
                },
                function (response) {
                    $scope.cart.Items = initialItems;
                    showErrorMessage(2000);
                });
        }, 200);
    }

    $scope.clearCart = function () {
        var initialItems = angular.copy($scope.cart.Items);
        $timeout.cancel(timer);
        timer = $timeout(function () {
            $scope.isUpdating = true;
            cartService.clearCart().then(
                function (response) {
                    refreshCart();
                },
                function (response) {
                    $scope.cart.Items = initialItems;
                    showErrorMessage(2000);
                });
        }, 200);
    }

    function initialize() {
        $scope.isCartModalVisible = false;
        $scope.isUpdating = false;
        $scope.errorOccured = false;
        refreshCart();
    }

    function refreshCart() {
        cartService.getCart().then(function (response) {
            $scope.cart = response.data;
            $scope.isUpdating = false;
            $scope.errorOccured = false;
        });
    }

    function showErrorMessage(timeout) {
        $scope.errorOccured = true;
        $scope.isUpdating = false;
        $timeout(function () {
            $scope.errorOccured = false;
        }, timeout);
    }
}]);