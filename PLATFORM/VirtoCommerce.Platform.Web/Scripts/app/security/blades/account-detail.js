angular.module('platformWebApp')
.controller('platformWebApp.accountDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.roles', 'platformWebApp.dialogService', 'platformWebApp.settings', function ($scope, bladeNavigationService, accounts, roles, dialogService, settings) {
    $scope.blade.promise = roles.get({ count: 10000 }).$promise;
    $scope.accountTypes = [];

    $scope.blade.refresh = function (parentRefresh) {
        accounts.get({ id: $scope.blade.data.userName }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
        $scope.accountTypes = settings.getValues({ id: 'VirtoCommerce.Platform.Security.AccountTypes' });
        userStateCommand.updateName();
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
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
    	bladeNavigationService.showBlade(newBlade, $scope.blade);
   
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;

        accounts.update({}, $scope.blade.currentEntity, function (data) {
            $scope.blade.refresh(true);
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "platform.dialogs.account-save.title",
                message: "platform.dialogs.account-save.message"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa-key';

    var userStateCommand = {
        updateName: function () {
            return this.name = ($scope.blade.currentEntity && $scope.blade.currentEntity.userState === 'Approved') ? 'platform.commands.reject-user' : 'platform.commands.approve-user';
        },
        // name: this.updateName(),
        icon: 'fa fa-dot-circle-o',
        executeMethod: function () {
            if ($scope.blade.currentEntity.userState === 'Approved') {
                $scope.blade.currentEntity.userState = 'Rejected';
            } else {
                $scope.blade.currentEntity.userState = 'Approved';
            }
            this.updateName();
        },
        canExecuteMethod: function () {
            return true;
        },
        permission: 'platform:security:update'
    };

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:security:update'
        },
        {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                userStateCommand.updateName();
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:security:update'
        },
        userStateCommand,
        {
            name: "platform.commands.change-password",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                var newBlade = {
                    id: 'accountDetailChild',
                    currentEntityId: $scope.blade.currentEntity.userName,
                    title: $scope.blade.title,
                    subtitle: "platform.blades.account-resetPassword.subtitle",
                    controller: 'platformWebApp.accountResetPasswordController',
                    template: '$(Platform)/Scripts/app/security/blades/account-resetPassword.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:update'
        }
    ];

    // actions on load
    $scope.blade.refresh(false);
}]);