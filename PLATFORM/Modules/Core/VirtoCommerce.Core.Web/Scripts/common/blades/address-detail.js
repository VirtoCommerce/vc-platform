﻿angular.module('virtoCommerce.coreModule.common')
.controller('virtoCommerce.coreModule.common.coreAddressDetailController', ['$scope', 'virtoCommerce.coreModule.common.countries', 'platformWebApp.dialogService', function ($scope, countries, dialogService) {
    $scope.addressTypes = ['Billing', 'Shipping', 'BillingAndShipping'];
    function initializeBlade() {

        if ($scope.blade.currentEntity.isNew) {
            $scope.blade.currentEntity.addressType = $scope.addressTypes[1];
        }

        $scope.blade.origEntity = angular.copy($scope.blade.currentEntity);
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.saveChanges = function () {
        if ($scope.blade.confirmChangesFn) {
            $scope.blade.confirmChangesFn($scope.blade.currentEntity);
        };
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "core.dialogs.address-save.title",
                message: "core.dialogs.address-save.subtitle",
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
                    if ($scope.blade.deleteFn) {
                        $scope.blade.deleteFn($scope.blade.currentEntity);
                    };
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.headIcon = 'fa-user';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !$scope.blade.currentEntity.isNew && !isDirty();
            }
        }
    ];

    $scope.$watch('blade.currentEntity.countryCode', function (countryCode) {
        var country;
        if (countryCode && (country = _.findWhere($scope.countries, { code: countryCode }))) {
            $scope.blade.currentEntity.countryName = country.name;
        }
    });


    // on load
    $scope.countries = countries.query();
    initializeBlade();
}]);