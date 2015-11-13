﻿angular.module('virtoCommerce.coreModule.fulfillment')
.controller('virtoCommerce.coreModule.fulfillment.fulfillmentCenterDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.fulfillment.fulfillments', function ($scope, dialogService, bladeNavigationService, fulfillments) {

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.currentEntityId) {
            $scope.blade.isLoading = true;

            fulfillments.get({ _id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else {
            initializeBlade($scope.blade.currentEntity);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;

        if ($scope.blade.currentEntityId) {
            fulfillments.update($scope.blade.currentEntity, function () {
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error: ' + error.status, $scope.blade);
            });
        } else {
            fulfillments.update($scope.blade.currentEntity, function (data) {
                $scope.blade.title = data.displayName;
                $scope.blade.currentEntityId = data.id;
                initializeBlade(data);
                $scope.blade.parentBlade.refresh();
            }, function (error) {
                bladeNavigationService.setError('Error: ' + error.status, $scope.blade);
            });
        }
    };

    $scope.blade.headIcon = 'fa-wrench';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && isValid();
            },
            permission: 'core:fulfillment:update'
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'core:fulfillment:update'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !isDirty();
            },
            permission: 'core:fulfillment:delete'
        }
    ];

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "core.dialogs.fulfillment-delete.title",
            message: "core.dialogs.fulfillment-delete.message",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    fulfillments.remove({ ids: $scope.blade.currentEntityId }, function () {
                        $scope.bladeClose();
                        $scope.blade.parentBlade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "core.dialogs.fulfillments-save.title",
                message: "core.dialogs.fulfillments-save.message",
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

    function isValid() {
        return formScope && formScope.$valid &&
                $scope.blade.currentEntity.daytimePhoneNumber &&
                $scope.blade.currentEntity.line1 &&
                $scope.blade.currentEntity.city &&
                $scope.blade.currentEntity.stateProvince &&
                $scope.blade.currentEntity.countryCode &&
                $scope.blade.currentEntity.countryName &&
                $scope.blade.currentEntity.postalCode;
    };

    // actions on load
    $scope.blade.refresh();
}]);
