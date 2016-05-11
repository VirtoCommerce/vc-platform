angular.module('platformWebApp')
.controller('platformWebApp.accountApiController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, dialogService, accounts) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';
    
    $scope.apiAccountTypes = ['Hmac', 'Simple'];

    function refresh() {
        if (blade.isNew) {
            generateNewApiAccount(true);
        } else {
            initializeBlade(blade.origEntity);
        }
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        // blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        if (blade.confirmChangesFn) {
            blade.confirmChangesFn(blade.currentEntity);
        }

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "platform.dialogs.api-key-delete.title",
            message: "platform.dialogs.api-key-delete.message",
            callback: function (remove) {
                if (remove) {
                    if (blade.deleteFn) {
                        blade.deleteFn(blade.origEntity);
                    }
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
        {
            name: "platform.commands.generate", icon: 'fa fa-refresh',
            executeMethod: function () {
                generateNewApiAccount();
            },
            canExecuteMethod: function () {
                return true;
            },
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
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () {
                return !blade.isNew;
            },
            permission: blade.updatePermission
        }
    ];

    function generateNewApiAccount(isInitialRequest) {
        var parameters;
        if (isInitialRequest) {
            parameters = { "type": $scope.apiAccountTypes[0] };
        } else {
            blade.isLoading = true;
            parameters = { "type": blade.currentEntity.apiAccountType };
        }

        accounts.generateNewApiAccount(parameters, function (apiAccount) {
            if (isInitialRequest) {
                apiAccount.isActive = true;
                blade.origEntity = apiAccount;
                initializeBlade(blade.origEntity);
            } else {
                blade.currentEntity.secretKey = undefined;
                angular.extend(blade.currentEntity, apiAccount);
                blade.isLoading = false;
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

    $scope.$watch('blade.currentEntity.apiAccountType', function (oldVal, newVal) {
        if (oldVal && newVal && oldVal !== newVal) {
            generateNewApiAccount();
        }
    });

    // on load: 
    refresh();
}]);