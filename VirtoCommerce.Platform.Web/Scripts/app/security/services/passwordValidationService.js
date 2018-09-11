angular
    .module('platformWebApp')
    .factory('platformWebApp.passwordValidationService', ['$q', 'platformWebApp.accounts', function ($q, accounts) {
        var service = {
            validatePasswordAsync: function(value) {
                var promise = accounts.validatePassword(JSON.stringify(value)).$promise.then(
                    function(response) {
                        var result = {
                            passwordIsValid: response.passwordIsValid,
                            minPasswordLength: response.minPasswordLength,
                            errors: []
                        };

                        if (!response.passwordIsValid) {
                            if (response.passwordViolatesMinLength) {
                                result.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-too-short");
                            }

                            if (response.passwordMustHaveUpperCaseLetters) {
                                result.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-uppercase-letters");
                            }

                            if (response.passwordMustHaveLowerCaseLetters) {
                                result.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-lowercase-letters");
                            }

                            if (response.passwordMustHaveDigits) {
                                result.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-digits");
                            }

                            if (response.passwordMustHaveSpecialCharacters) {
                                result.errors.push(
                                    "platform.blades.account-resetPassword.validations.password-must-contain-special-characters");
                            }
                        }

                        return result;
                    }
                );

                return promise;
            }
        }

        return service;
    }]);
