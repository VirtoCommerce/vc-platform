﻿angular.module('virtoCommerce.cartModule')
.controller('virtoCommerce.cartModule.cartDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.cartModule.carts', function ($scope, dialogService, bladeNavigationService, carts) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        carts.getCart({ id: $scope.blade.currentEntityId }, function (results) {
            $scope.blade.currentEntity = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        carts.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.blade.headIcon = 'fa-shopping-cart';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'cart:update'
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'cart:update'
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "cart.dialogs.cart-save.title",
                message: "cart.dialogs.cart-save.message",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    // actions on load
    $scope.blade.refresh();
}]);
