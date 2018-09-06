angular.module('platformWebApp')
.controller('platformWebApp.accountResetPasswordController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($q, $scope, bladeNavigationService, accounts) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.currentEntity = {
            newPassword: '',
            forcePasswordChange: true,
            passwordIsValid: true,
            minPasswordLength: 0,
            errors: []
        };
        
        blade.isLoading = false;
    };

    $scope.validatePasswordAsync = function (value) {
        var requestPromise = accounts.validatePassword(JSON.stringify(value)).$promise;
        var validationPromise = $q(
            function (resolve, reject) {
                requestPromise.then(
                    function(response) {
                        blade.currentEntity.errors = [];

                        if (response.passwordIsValid) {
                            blade.currentEntity.passwordIsValid = true;
                            resolve();
                        } else {
                            blade.currentEntity.passwordIsValid = false;
                            blade.currentEntity.minPasswordLength = response.minPasswordLength;

                            if (response.passwordViolatesMinLength) {
                                blade.currentEntity.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-too-short");
                            }

                            if (response.passwordMustHaveUpperCaseLetters) {
                                blade.currentEntity.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-uppercase-letters");
                            }

                            if (response.passwordMustHaveLowerCaseLetters) {
                                blade.currentEntity.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-lowercase-letters");
                            }

                            if (response.passwordMustHaveDigits) {
                                blade.currentEntity.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-digits");
                            }

                            if (response.passwordMustHaveSpecialCharacters) {
                                blade.currentEntity.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-special-characters");
                            }

                            reject();
                        }
                    }
                );
            }
        );

        return validationPromise;
    }

    $scope.saveChanges = function () {
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
