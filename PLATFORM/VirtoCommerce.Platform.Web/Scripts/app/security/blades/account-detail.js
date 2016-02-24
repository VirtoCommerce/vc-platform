angular.module('platformWebApp')
.controller('platformWebApp.accountDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.roles', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, accounts, roles, dialogService, settings) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';
    blade.promise = roles.search({ count: 10000 }).$promise;
    $scope.accountTypes = [];

    blade.refresh = function (parentRefresh) {
        accounts.get({ id: blade.data.userName }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
        $scope.accountTypes = settings.getValues({ id: 'VirtoCommerce.Platform.Security.AccountTypes' });
        userStateCommand.updateName();
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    $scope.openAccountTypeSettingManagement = function () {
        var newBlade = {
            id: 'accountTypesDictionary',
            isApiSave: true,
            currentEntityId: 'VirtoCommerce.Platform.Security.AccountTypes',
            parentRefresh: function (data) { $scope.accountTypes = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);

    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        accounts.update({}, blade.currentEntity, function (data) {
            blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "platform.dialogs.account-save.title", "platform.dialogs.account-save.message");
    };

    blade.headIcon = 'fa-key';

    var userStateCommand = {
        updateName: function () {
            return this.name = (blade.currentEntity && blade.currentEntity.userState === 'Approved') ? 'platform.commands.reject-user' : 'platform.commands.approve-user';
        },
        // name: this.updateName(),
        icon: 'fa fa-dot-circle-o',
        executeMethod: function () {
            if (blade.currentEntity.userState === 'Approved') {
                blade.currentEntity.userState = 'Rejected';
            } else {
                blade.currentEntity.userState = 'Approved';
            }
            this.updateName();
        },
        canExecuteMethod: function () {
            return true;
        },
        permission: blade.updatePermission
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
                userStateCommand.updateName();
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        userStateCommand,
        {
            name: "platform.commands.change-password",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                var newBlade = {
                    id: 'accountDetailChild',
                    currentEntityId: blade.currentEntity.userName,
                    title: blade.title,
                    subtitle: "platform.blades.account-resetPassword.subtitle",
                    controller: 'platformWebApp.accountResetPasswordController',
                    template: '$(Platform)/Scripts/app/security/blades/account-resetPassword.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        }
    ];

    // actions on load
    blade.refresh(false);
}]);