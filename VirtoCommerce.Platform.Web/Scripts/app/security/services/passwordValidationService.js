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

                        var filteredKeys = _.filter(Object.keys(response),
                            function(key) {
                                return !key.startsWith('$') && response[key] === true;
                            });

                        filteredKeys.forEach(function(key) {
                            var resourceName = 'platform.blades.account-resetPassword.validations.' + key;
                            result.errors.push(resourceName);
                        });

                        return result;
                    }
                );

                return promise;
            }
        }

        return service;
    }]);
