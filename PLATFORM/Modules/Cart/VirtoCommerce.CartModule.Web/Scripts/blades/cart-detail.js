angular.module('virtoCommerce.cartModule.blades')
.controller('cartDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'carts', function ($scope, dialogService, bladeNavigationService, carts) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        carts.getCart({ id: $scope.blade.currentEntityId }, function (results) {
            $scope.blade.currentEntity = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
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
        });
    };

    $scope.bladeHeadIco = 'fa-shopping-cart';

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The cart has been modified. Do you want to save changes?",
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
