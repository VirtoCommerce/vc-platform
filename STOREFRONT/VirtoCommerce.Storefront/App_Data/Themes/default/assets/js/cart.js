var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('cartController', ['$rootScope', '$scope', '$timeout', 'cartService', function ($rootScope, $scope, $timeout, cartService) {
    var timer;

    initialize();

    $scope.setCartForm = function (form) {
        $scope.formCart = form;
    }

    $scope.changeLineItemQuantity = function (lineItemId, quantity) {
        var lineItem = _.find($scope.cart.Items, function (i) { return i.Id == lineItemId });
        if (!lineItem || quantity < 1 || $scope.cartIsUpdating || $scope.formCart.$invalid) {
            return;
        }
        var initialQuantity = lineItem.Quantity;
        lineItem.Quantity = quantity;
        $timeout.cancel(timer);
        timer = $timeout(function () {
            $scope.cartIsUpdating = true;
            cartService.changeLineItemQuantity(lineItemId, quantity).then(function (response) {
                getCart();
                $rootScope.$broadcast('cartItemsChanged');
            }, function (response) {
                lineItem.Quantity = initialQuantity;
                $scope.cartIsUpdating = false;
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
            getCart();
            $rootScope.$broadcast('cartItemsChanged');
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
        getCart();
    }

    function getCart() {
        $scope.cartIsUpdating = true;
        cartService.getCart().then(function (response) {
            $scope.cart = response.data;
            $scope.cartIsUpdating = false;
        }, function (response) {
            $scope.cartIsUpdating = false;
        });
    }
}]);

storefrontApp.controller('cartBarController', ['$scope', 'cartService', function ($scope, cartService) {
    getCartItemsCount();

    $scope.$on('cartItemsChanged', function (event, data) {
        getCartItemsCount();
    });

    function getCartItemsCount() {
        cartService.getCartItemsCount().then(function (response) {
            $scope.cartItemsCount = response.data;
        });
    }
}]);

storefrontApp.controller('recentlyAddedCartItemDialogController', ['$scope', '$window', '$uibModalInstance', 'dialogData', function ($scope, $window, $uibModalInstance, dialogData) {
    $scope.dialogData = dialogData;

    $scope.close = function () {
        $uibModalInstance.close();
    }

    $scope.redirect = function (url) {
        $window.location = url;
    }
}]);