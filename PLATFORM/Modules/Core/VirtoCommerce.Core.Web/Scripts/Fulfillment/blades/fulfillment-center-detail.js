angular.module('virtoCommerce.coreModule.fulfillment')
.controller('virtoCommerce.coreModule.fulfillment.fulfillmentCenterDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.fulfillment.fulfillments', function ($scope, dialogService, bladeNavigationService, fulfillments) {
    var blade = $scope.blade;
    blade.updatePermission = 'core:fulfillment:update';

    blade.refresh = function (parentRefresh) {
        if (blade.currentEntityId) {
            blade.isLoading = true;

            fulfillments.get({ _id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            initializeBlade(blade.currentEntity);
        }
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    var formScope;
    $scope.setForm = function (form) { formScope = form; };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && formScope && formScope.$valid &&
                blade.currentEntity.daytimePhoneNumber &&
                blade.currentEntity.line1 &&
                blade.currentEntity.city &&
                blade.currentEntity.stateProvince &&
                blade.currentEntity.countryCode &&
                blade.currentEntity.countryName &&
                blade.currentEntity.postalCode;;
    }

    function saveChanges() {
        blade.isLoading = true;

        if (blade.currentEntityId) {
            fulfillments.update(blade.currentEntity, function () {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error: ' + error.status, blade);
            });
        } else {
            fulfillments.update(blade.currentEntity, function (data) {
                blade.title = data.displayName;
                blade.currentEntityId = data.id;
                initializeBlade(data);
                blade.parentBlade.refresh();
            }, function (error) {
                bladeNavigationService.setError('Error: ' + error.status, blade);
            });
        }
    };

    blade.headIcon = 'fa-wrench';

    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: saveChanges,
            canExecuteMethod: canSave,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () { return true; },
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
                    blade.isLoading = true;

                    fulfillments.remove({ ids: blade.currentEntityId }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "core.dialogs.fulfillments-save.title", "core.dialogs.fulfillments-save.message");
    };

    // actions on load
    blade.refresh();
}]);
