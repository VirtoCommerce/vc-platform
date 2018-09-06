angular.module('platformWebApp')
.controller('platformWebApp.accountResetPasswordController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, accounts) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.currentEntity = {
            newPassword: '',
            forcePasswordChange: true,
            passwordIsValid: true,
            minPasswordLength: 0,
            passwordViolatesMinLength: false,
            passwordMustContainLowerCaseLetters: false,
            passwordMustContainUpperCaseLetters: false,
            passwordMustContainDigits: false,
            passwordMustContainSpecialCharacters: false,
        };
        
        blade.isLoading = false;
    };

    $scope.handlePasswordChange = function() {
        accounts.validatePassword(JSON.stringify(blade.currentEntity.newPassword),
            function(response) {
                blade.currentEntity.passwordIsValid = response.passwordIsValid;
                blade.currentEntity.minPasswordLength = response.minPasswordLength;
                blade.currentEntity.passwordViolatesMinLength = response.passwordViolatesMinLength;
                blade.currentEntity.passwordMustContainUpperCaseLetters = response.passwordMustHaveUpperCaseLetters;
                blade.currentEntity.passwordMustContainLowerCaseLetters = response.passwordMustHaveLowerCaseLetters;
                blade.currentEntity.passwordMustContainDigits = response.passwordMustHaveDigits;
                blade.currentEntity.passwordMustContainSpecialCharacters = response.passwordMustHaveSpecialCharacters;
            });
    }

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
