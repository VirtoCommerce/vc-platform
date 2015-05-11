angular.module('platformWebApp')
.controller('platformWebApp.accountDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.roles', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, accounts, roles, dialogService) {
    $scope.blade.promise = roles.get({ count: 10000 }).$promise;

    $scope.blade.refresh = function (parentRefresh) {
        accounts.get({ id: $scope.blade.data.userName }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        });
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;

        userStateCommand.updateName();
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
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
                title: "Save changes",
                message: "The Account has been modified. Do you want to save changes?"
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

    $scope.bladeHeadIco = 'fa-lock';

    var userStateCommand = {
        updateName: function () {
            return this.name = ($scope.blade.currentEntity && $scope.blade.currentEntity.userState === 'Approved') ? 'Reject user' : 'Approve user';
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
        permission: 'platform:security:manage'
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:security:manage'
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                userStateCommand.updateName();
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:security:manage'
        },
        userStateCommand,
        {
            name: "Change password",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                var newBlade = {
                    id: 'accountDetailChild',
                    currentEntityId: $scope.blade.currentEntity.userName,
                    title: $scope.blade.title,
                    subtitle: "Change your password",
                    controller: 'platformWebApp.accountChangePasswordController',
                    template: 'Scripts/common/security/blades/account-changePassword.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:manage'
        }
    ];

    // actions on load
    $scope.blade.refresh(false);
}]);