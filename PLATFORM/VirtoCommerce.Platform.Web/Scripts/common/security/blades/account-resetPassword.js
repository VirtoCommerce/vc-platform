angular.module('platformWebApp')
.controller('platformWebApp.accountResetPasswordController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, accounts) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.currentEntity = {
            newPassword: '',
            newPassword2: ''
        };
        
        blade.isLoading = false;
    };
    
    $scope.saveChanges = function () {
        if (blade.currentEntity.newPassword != blade.currentEntity.newPassword2) {
            blade.error = 'Error: new passwords doesn\'t match!';
            return;
        }

        blade.isLoading = true;
        blade.error = undefined;

        var postData = {
            newPassword: blade.currentEntity.newPassword
        };

        accounts.resetPassword({ id: blade.currentEntityId }, postData, function (data) {
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
    
    blade.headIcon = 'fa-lock';

    // actions on load
    initializeBlade();
}]);