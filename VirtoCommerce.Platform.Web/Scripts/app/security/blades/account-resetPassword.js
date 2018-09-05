angular.module('platformWebApp')
.controller('platformWebApp.accountResetPasswordController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, accounts) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.currentEntity = {
            newPassword: '',
            forcePasswordChange: true
        };
        
        blade.isLoading = false;
    };
    
    $scope.saveChanges = function () {
        // TODO: invoke password strength check and set blade.error if password is too weak

        blade.isLoading = true;
        blade.error = undefined;

        var postData = {
            newPassword: blade.currentEntity.newPassword,
            forcePasswordChangeOnFirstLogin: blade.currentEntity.forcePasswordChange
        };

        accounts.resetPassword({ id: blade.currentEntityId }, postData, function () {
            $scope.bladeClose();
        }, function (response) {
            bladeNavigationService.setError(response, $scope.blade);
        });
    };
    
    blade.headIcon = 'fa-key';

    // actions on load
    initializeBlade();
}]);
