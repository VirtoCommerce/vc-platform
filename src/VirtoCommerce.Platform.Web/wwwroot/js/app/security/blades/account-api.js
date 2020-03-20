angular.module('platformWebApp')
.controller('platformWebApp.accountApiController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, dialogService, accounts) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';

    function refresh() {
        accounts.getUserApiKeys({ id: blade.user.id }, function (data) {
            initializeBlade(data[0]);
        },
            function (error) {
                bladeNavigationService.setError(error, blade);
            });
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
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

        accounts.saveUserApiKey(blade.currentEntity);
        $scope.bladeClose();
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "platform.dialogs.api-key-delete.title",
            message: "platform.dialogs.api-key-delete.message",
            callback: function (remove) {
                if (remove) {
                    accounts.deleteUserApiKey({ ids: blade.currentEntity.id });
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
                blade.currentEntity.apiKey = generateNewApiKey();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: deleteEntry,
            canExecuteMethod: function () {
                return !blade.isNew;
            },
            permission: 'platform:security:delete'
        }
    ];

  
    function generateNewApiKey() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }   
  
    // on load: 
    refresh();
}]);
