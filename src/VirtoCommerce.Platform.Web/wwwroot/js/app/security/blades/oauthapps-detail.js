angular.module('platformWebApp').controller('platformWebApp.oAuthAppsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.oauthapps', function ($scope, bladeNavigationService, dialogService, oauthapps) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';

    blade.refresh = function () {
        if (blade.isNew) {
            generateNew();
        } else {
            initialize(blade.origEntity);
        }
    }

    function initialize(data) {
        blade.currentEntity = angular.copy(data);
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "platform.dialogs.oauthapps-save.title", "platform.dialogs.oauthapps-save.message");
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;
        oauthapps.save({}, blade.currentEntity, function (result) {
            blade.isLoading = false;
            angular.copy(blade.currentEntity, blade.origEntity);
            blade.parentBlade.refresh();
            $scope.bladeClose();
        });
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "platform.dialogs.oauthapps-delete.title",
            message: "platform.dialogs.oauthapps-delete.message",
            callback: function (remove) {
                blade.isLoading = true;
                if (remove) {
                    var clientIds = [blade.currentEntity.clientId];
                    oauthapps.delete({ clientIds }, result => {
                        blade.isLoading = false;
                        blade.parentBlade.refresh();
                        $scope.bladeClose();
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return !blade.isNew;
            }
        },
        {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () {
                return !blade.isNew;
            },
            permission: blade.updatePermission
        }
    ];

    function generateNew() {
        blade.isLoading = true;

        oauthapps.new(null, function (app) {
            blade.origEntity = app;
            initialize(blade.origEntity);
            blade.isLoading = false;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    blade.refresh();
}]);
