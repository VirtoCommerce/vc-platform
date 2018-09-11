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

    var errorMessages = {
        passwordViolatesMinLength: "platform.blades.account-resetPassword.validations.password-too-short",
        passwordMustHaveUpperCaseLetters: "platform.blades.account-resetPassword.validations.password-must-contain-uppercase-letters",
        passwordMustHaveLowerCaseLetters: "platform.blades.account-resetPassword.validations.password-must-contain-lowercase-letters",
        passwordMustHaveDigits: "platform.blades.account-resetPassword.validations.password-must-contain-digits",
        passwordMustHaveSpecialCharacters: "platform.blades.account-resetPassword.validations.password-must-contain-special-characters"
    };

    $scope.validatePasswordAsync = function (value) {
        var promise = accounts.validatePassword(JSON.stringify(value)).$promise.then(
            function(response) {
                blade.currentEntity.errors = [];

                if (response.passwordIsValid) {
                    blade.currentEntity.passwordIsValid = true;
                    return $q.resolve();
                } else {
                    blade.currentEntity.passwordIsValid = false;
                    blade.currentEntity.minPasswordLength = response.minPasswordLength;

                    for (var propertyName in response) {
                        // If the property is not a known password validation rule result, skip it.
                        if (!errorMessages.hasOwnProperty(propertyName))
                            continue;

                        // If the password does not violate the rule, let's skip it too.
                        if (response[propertyName] !== true)
                            continue;

                        blade.currentEntity.errors.push(errorMessages[propertyName]);
                    }

                    return $q.reject();
                }
            }
        );

        return promise;
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
