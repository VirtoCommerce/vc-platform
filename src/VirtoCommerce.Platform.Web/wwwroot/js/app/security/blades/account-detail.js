angular.module('platformWebApp').controller('platformWebApp.accountDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.metaFormsService', 'platformWebApp.accounts', 'platformWebApp.roles', 'platformWebApp.dialogService', 'platformWebApp.settings',
    function ($scope, bladeNavigationService, metaFormsService, accounts, roles, dialogService, settings) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:security:update';
        blade.accountTypes = [];

        blade.refresh = function (parentRefresh) {
            var entity = parentRefresh ? blade.currentEntity : blade.data;
            accounts.get({ id: entity.userName }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
                function (error) {
                    bladeNavigationService.setError(error, blade);
                });
        }

        function initializeBlade(data) {
            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            isAccountlocked(blade.currentEntity.id).then(function (result) {
                blade.accountLockedState = result.locked ? "Locked" : "Unlocked";
            });
            blade.accountTypes = settings.getValues({ id: 'VirtoCommerce.Platform.Security.AccountTypes' });
            blade.isLoading = false;
        };

        function isAccountlocked(id) {
            return accounts.locked({ id: id }).$promise;
        }


        blade.metaFields = metaFormsService.getMetaFields("accountDetails");

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        };

        function canSave() {
            return isDirty() && $scope.formScope && $scope.formScope.$valid;
        }

        blade.openAccountTypeSettingManagement = function () {
            var newBlade = {
                id: 'accountTypesDictionary',
                isApiSave: true,
                currentEntityId: 'VirtoCommerce.Platform.Security.AccountTypes',
                parentRefresh: function (data) { blade.accountTypes = data; },
                controller: 'platformWebApp.settingDictionaryController',
                template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);

        };

        $scope.setForm = function (form) {
            $scope.formScope = form;
        }

        $scope.saveChanges = function () {
            blade.isLoading = true;

            accounts.update({}, blade.currentEntity, function (result) {
                if (result.succeeded) {
                    blade.refresh(true);
                }
                else {
                    bladeNavigationService.setError(result.errors.join(), blade);
                }
            });
        };

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "platform.dialogs.account-save.title", "platform.dialogs.account-save.message");
        };

        blade.headIcon = 'fa-key';

       
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
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission
            },
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
            },
            {
                name: "platform.commands.unlock-account",
                icon: 'fa fa-unlock',
                executeMethod: function () {
                    blade.isLoading = true;
                    accounts.unlock({ id: blade.currentEntity.id }, null, function (result) {
                        if (result.succeeded) {
                            blade.accountLockedState = "Unlocked";
                        }
                        blade.isLoading = false;
                    }, function (error) {
                        bladeNavigationService.setError(error, blade);
                        blade.isLoading = false;
                    });
                },
                canExecuteMethod: function () {
                    if (blade.accountLockedState)
                        return blade.accountLockedState === "Locked";
                    return false;
                },
                permission: blade.updatePermission
            }
        ];

        // actions on load
        blade.refresh(false);
    }]);
