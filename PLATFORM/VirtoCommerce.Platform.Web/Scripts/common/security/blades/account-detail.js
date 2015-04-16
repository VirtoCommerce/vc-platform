angular.module('platformWebApp')
.controller('accountDetailController', ['$scope', 'bladeNavigationService', 'accounts', 'platform_res_roles', '$interval', 'uiGridConstants', 'dialogService', function ($scope, bladeNavigationService, accounts, roles, $interval, uiGridConstants, dialogService) {
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

    $scope.bladeToolbarCommands = [
        {
            name: "Save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Change password",
            icon: 'fa fa-refresh',
            executeMethod: function () {
                var newBlade = {
                    id: 'accountDetailChild',
                    currentEntityId: $scope.blade.currentEntity.userName,
                    title: $scope.blade.title,
                    subtitle: "Change your password",
                    controller: 'accountChangePasswordController',
                    template: 'Scripts/common/security/blades/account-changePassword.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    // actions on load
    $scope.blade.refresh(false);
}]);