angular.module('platformWebApp')
.controller('platformWebApp.accountApiController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, dialogService, accounts) {
    $scope.apiAccountTypes = ['Hmac', 'Simple'];

    function refresh() {
        if ($scope.blade.isNew) {
            generateNewApiAccount(true);
        } else {
            initializeBlade($scope.blade.origEntity);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        // $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        if ($scope.blade.confirmChangesFn) {
            $scope.blade.confirmChangesFn($scope.blade.currentEntity);
        }

        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this API key?",
            callback: function (remove) {
                if (remove) {
                    if ($scope.blade.deleteFn) {
                        $scope.blade.deleteFn($scope.blade.origEntity);
                    };
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeHeadIco = 'fa-lock';
    $scope.toolbarTemplate = 'Scripts/common/security/blades/account-api-toolbar.tpl.html';

    $scope.bladeToolbarCommands = [
        {
            name: "Generate", icon: 'fa fa-refresh',
            executeMethod: function () {
                generateNewApiAccount();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'platform:security:manage'
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'platform:security:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !isDirty() && !$scope.blade.isNew;
            },
            permission: 'platform:security:manage'
        }
    ];

    function generateNewApiAccount(isInitialRequest) {
        var parameters;
        if (isInitialRequest) {
            parameters = { "type": $scope.apiAccountTypes[0] };
        } else {
            $scope.blade.isLoading = true;
            parameters = { "type": $scope.blade.currentEntity.apiAccountType };
        }
        
        accounts.generateNewApiAccount(parameters, function (apiAccount) {
            if (isInitialRequest) {
                apiAccount.isActive = true;
                $scope.blade.origEntity = apiAccount;
                initializeBlade($scope.blade.origEntity);
            } else {
                $scope.blade.currentEntity.secretKey = undefined;
                angular.extend($scope.blade.currentEntity, apiAccount);
                $scope.blade.isLoading = false;
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

    $scope.$watch('blade.currentEntity.apiAccountType', function(oldVal, newVal) {
        if (oldVal && newVal && oldVal !== newVal) {
            generateNewApiAccount();
        }
    });

    // on load: 
    refresh();
}]);