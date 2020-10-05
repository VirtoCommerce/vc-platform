angular.module('platformWebApp').controller('platformWebApp.oAuthAppsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.oauthapps', 'platformWebApp.validators', function ($scope, bladeNavigationService, dialogService, oauthapps, validators) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';

    blade.refresh = function () {
        if (blade.isNew) {
            generateNew();
        } else {
            initialize(blade.data);
        }
    }

    function initialize(data) {
        blade.origEntity = data;
        blade.currentEntity = angular.copy(data);
        blade.isLoading = false;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

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
        }, error => {
            blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
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
            blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.copyToClipboard = function (elementId) {
        var text = document.getElementById(elementId);
        text.focus();
        text.select();
        document.execCommand('copy');
    };

    $scope.editRedirectUris = function () {
        var newBlade = {
            id: "editRedirectUris",
            updatePermission: 'platform:security:update',
            data: blade.currentEntity.redirectUris,
            validator: validators.uriWithoutQuery,
            headIcon: 'fa-plus-square-o',
            title: 'platform.blades.oauthapps-detail.blades.edit-redirectUris.title',
            subtitle: 'platform.blades.oauthapps-detail.blades.edit-redirectUris.subtitle',
            controller: 'platformWebApp.editArrayController',
            template: '$(Platform)/Scripts/common/blades/edit-array.tpl.html',
            onChangesConfirmedFn: function (values) {
                blade.currentEntity.redirectUris = angular.copy(values);
            }
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.editPostLogoutRedirectUris = function () {
        var newBlade = {
            id: "editPostLogoutRedirectUris",
            updatePermission: 'platform:security:update',
            data: blade.currentEntity.postLogoutRedirectUris,
            validator: validators.uriWithoutQuery,
            headIcon: 'fa-plus-square-o',
            title: 'platform.blades.oauthapps-detail.blades.edit-postLogoutRedirectUris.title',
            subtitle: 'platform.blades.oauthapps-detail.blades.edit-postLogoutRedirectUris.subtitle',
            controller: 'platformWebApp.editArrayController',
            template: '$(Platform)/Scripts/common/blades/edit-array.tpl.html',
            onChangesConfirmedFn: function (values) {
                blade.currentEntity.postLogoutRedirectUris = angular.copy(values);
            }
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    blade.refresh();
}]);
