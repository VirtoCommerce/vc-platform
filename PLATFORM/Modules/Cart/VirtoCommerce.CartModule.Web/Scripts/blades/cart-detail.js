angular.module('virtoCommerce.cartModule')
.controller('virtoCommerce.cartModule.cartDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.cartModule.carts', function ($scope, dialogService, bladeNavigationService, carts) {
    var blade = $scope.blade;
    blade.updatePermission = 'cart:update';

    blade.refresh = function () {
        blade.isLoading = true;

        carts.getCart({ id: blade.currentEntityId }, function (results) {
            blade.currentEntity = angular.copy(results);
            blade.origEntity = results;
            blade.isLoading = false;
        },
        function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function saveChanges() {
        blade.isLoading = true;
        carts.update({}, blade.currentEntity, function (data, headers) {
            blade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.headIcon = 'fa-shopping-cart';
    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: saveChanges,
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        }
    ];

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, saveChanges, closeCallback, "cart.dialogs.cart-save.title", "cart.dialogs.cart-save.message");
    };

    // actions on load
    blade.refresh();
}]);
