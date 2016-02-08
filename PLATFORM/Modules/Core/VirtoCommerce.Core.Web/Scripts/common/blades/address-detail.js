angular.module('virtoCommerce.coreModule.common')
.controller('virtoCommerce.coreModule.common.coreAddressDetailController', ['$scope', 'virtoCommerce.coreModule.common.countries', 'platformWebApp.dialogService', function ($scope, countries, dialogService) {
    var blade = $scope.blade;

    $scope.addressTypes = ['Billing', 'Shipping', 'BillingAndShipping'];
    function initializeBlade() {

        if (blade.currentEntity.isNew) {
            blade.currentEntity.addressType = $scope.addressTypes[1];
        }

        blade.origEntity = angular.copy(blade.currentEntity);
        blade.isLoading = false;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    }

    $scope.setForm = function (form) { $scope.formScope = form; };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.saveChanges = function () {
        if (blade.confirmChangesFn) {
            blade.confirmChangesFn(blade.currentEntity);
        };
        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    blade.onClose = function (closeCallback) {
        if (isDirty() && $scope.isValid()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "core.dialogs.address-save.title",
                message: "core.dialogs.address-save.message",
                callback: function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
                    }
                    closeCallback();
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "core.dialogs.address-delete.title",
            message: "core.dialogs.address-delete.message",
            callback: function (remove) {
                if (remove) {
                    if (blade.deleteFn) {
                        blade.deleteFn(blade.currentEntity);
                    };
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.headIcon = 'fa-user';

    blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () {
                return !blade.currentEntity.isNew;
            }
        }
    ];

    $scope.$watch('blade.currentEntity.countryCode', function (countryCode) {
        var country;
        if (countryCode && (country = _.findWhere($scope.countries, { code: countryCode }))) {
            blade.currentEntity.countryName = country.name;
        }
    });


    // on load
    $scope.countries = countries.query();
    initializeBlade();
}]);