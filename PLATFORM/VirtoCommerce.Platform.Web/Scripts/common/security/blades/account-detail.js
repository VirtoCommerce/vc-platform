angular.module('platformWebApp')
.controller('accountDetailController', ['$scope', 'bladeNavigationService', 'accounts', 'dialogService', function ($scope, bladeNavigationService, accounts, dialogService) {
    $scope.blade.refresh = function (parentRefresh) {
        accounts.get({ id: $scope.blade.data.userName }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        });
    }

    function initializeBlade(data) {
        // $scope.blade.currentEntityId = data.id;
        $scope.blade.title = data.fullName;

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

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

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
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
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


    $scope.generateNewApiAccount = function () {
        $scope.blade.isLoading = true;

        accounts.generateNewApiAccount({}, function (apiAccount) {
            $scope.blade.isLoading = false;
            if ($scope.blade.currentEntity.apiAcounts && $scope.blade.currentEntity.apiAcounts.length > 0) {
                $scope.blade.currentEntity.apiAcounts[0] = apiAccount;
            }
            else {
                $scope.blade.currentEntity.apiAcounts = [apiAccount];
            }
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.copyCode = function () {
        var secretKey = document.getElementById('secretKey');
        secretKey.focus();
        secretKey.select();
    };

    // actions on load
    $scope.blade.refresh(false);
}]);