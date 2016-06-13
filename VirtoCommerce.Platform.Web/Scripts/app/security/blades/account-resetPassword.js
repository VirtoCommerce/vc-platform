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
            $scope.bladeClose();
        }, function (error) {
            bladeNavigationService.setError('Error: ' + error.data.message, $scope.blade);
        });
    };
    
    blade.headIcon = 'fa-key';

    // actions on load
    initializeBlade();
}]);