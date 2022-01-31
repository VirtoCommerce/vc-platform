angular.module('platformWebApp').controller('platformWebApp.accountDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.metaFormsService', 'platformWebApp.accounts', 'platformWebApp.settings', 'platformWebApp.authService', 'platformWebApp.login',
    function ($scope, bladeNavigationService, metaFormsService, accounts, settings, authService, loginResources) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:security:update';
        blade.accountTypes = [];
        blade.statuses = [];
        blade.isLinkSent = false;
        blade.isPasswordLoginEnabled = false;

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

        function setToolbarCommands() {

            let filteredCommands = commands.filter((item) => item.meta !== blade.accountLockedState);

            if (!blade.isPasswordLoginEnabled) {
                filteredCommands = filteredCommands.filter((item) => item.meta !== "ChangePassword");
            }

            blade.toolbarCommands = filteredCommands;
        }

        function initializeBlade(data) {
            // Load login types
            loginResources.getLoginTypes().$promise.then(function (loginTypes) {
                loginTypes = _.filter(loginTypes, function (loginTypeFilter) {
                    return loginTypeFilter.authenticationType === "Password";
                });

                let loginType = _.first(_.sortBy(loginTypes, function (loginTypeSort) {
                    return loginTypeSort.priority;
                }));

                blade.isPasswordLoginEnabled = loginType.enabled;
            });

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            isAccountlocked(blade.currentEntity.id).then(function (result) {
                blade.accountIsLocked = result.locked;
                blade.accountLockedState = blade.accountIsLocked ? "Locked" : "Unlocked";
                setToolbarCommands();
            });

            // Load account types
            blade.accountTypes = settings.getValues({ id: 'VirtoCommerce.Platform.Security.AccountTypes' });

            // Load statuses
            blade.statuses = settings.getValues({ id: 'VirtoCommerce.Other.AccountStatuses' });

            blade.isLoading = false;
        }

        function isAccountlocked(id) {
            return accounts.locked({ id: id }).$promise;
        }

        blade.metaFields = metaFormsService.getMetaFields("accountDetails");

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        }

        blade.sendLink = function () {
            if (!blade.isLinkSent && !blade.isLoading && blade.currentEntity.email === blade.origEntity.email) {
                blade.isLoading = true;
                accounts.verifyEmail({ userId: blade.currentEntity.id }, null , () => {
                    blade.isLinkSent = true;
                    blade.isLoading = false;
                });
            }
        }

        blade.openSettingDictionaryController = function (currentEntityId) {
            var newBlade = {
                id: currentEntityId,
                isApiSave: true,
                currentEntityId: currentEntityId,
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

        blade.hasVerifyEmailPermission = () => {
            return authService.checkPermission('platform:security:verifyEmail', blade.securityScopes);
        }

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "platform.dialogs.account-save.title", "platform.dialogs.account-save.message");
        };

        blade.headIcon = 'fas fa-key';

        const commands = [
            {
                name: "platform.commands.save",
                icon: 'fas fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission,
                meta: "Save"
            },
            {
                name: "platform.commands.reset",
                icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission,
                meta: "Reset"
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
                permission: blade.updatePermission,
                meta: "ChangePassword"
            },
            {
                name: "platform.commands.lock-account",
                icon: 'fa fa-lock',
                executeMethod: function () {
                    blade.isLoading = true;
                    accounts.lock({ id: blade.currentEntity.id }, null, function (result) {
                        if (result.succeeded) {
                            blade.accountLockedState = "Locked";
                            blade.accountIsLocked = true;
                            setToolbarCommands();
                        }
                        blade.isLoading = false;
                    }, function (error) {
                        bladeNavigationService.setError(error, blade);
                        blade.isLoading = false;
                    });
                },
                canExecuteMethod: function () {
                    if (blade.accountLockedState)
                        return blade.accountLockedState === "Unlocked";
                    return false;
                },
                permission: blade.updatePermission,
                meta: "Locked"
            },
            {
                name: "platform.commands.unlock-account",
                icon: 'fa fa-unlock-alt',
                executeMethod: function () {
                    blade.isLoading = true;
                    accounts.unlock({ id: blade.currentEntity.id }, null, function (result) {
                        if (result.succeeded) {
                            blade.accountLockedState = "Unlocked";
                            blade.accountIsLocked = false;
                            setToolbarCommands();
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
                permission: blade.updatePermission,
                meta: "Unlocked"
            }
        ]

        // actions on load
        blade.refresh(false);
    }]);
