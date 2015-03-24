angular.module('platformWebApp')
.controller('accountChangePasswordController', ['$scope', 'bladeNavigationService', 'accounts', 'dialogService', function ($scope, bladeNavigationService, accounts, dialogService) {

    function initializeBlade() {
        $scope.blade.currentEntity = {
            oldPassword: '',
            newPassword: '',
            newPassword2: ''
        };

        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.saveChanges = function () {
        if ($scope.blade.currentEntity.newPassword != $scope.blade.currentEntity.newPassword2) {
            $scope.blade.error = 'Error: new passwords doesn\'t match!';
            return;
        }

        if ($scope.blade.currentEntity.oldPassword == $scope.blade.currentEntity.newPassword) {
            $scope.blade.error = 'Error: old and new passwords are the same!';
            return;
        }
        
        $scope.blade.isLoading = true;
        $scope.blade.error = undefined;

        var postData = {
            oldPassword: $scope.blade.currentEntity.oldPassword,
            newPassword: $scope.blade.currentEntity.newPassword
        };
        
        accounts.changepassword({ id: $scope.blade.currentEntityId }, postData, function (data) {
            if (data.succeeded) {
                $scope.bladeClose();
            }
            else {
                bladeNavigationService.setError('Error: ' + data.errors[0], $scope.blade);
            }

        }, function (error) {
            bladeNavigationService.setError('Error: ' + error.status, $scope.blade);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.bladeHeadIco = 'fa-lock';

    // actions on load
    initializeBlade();
}]);