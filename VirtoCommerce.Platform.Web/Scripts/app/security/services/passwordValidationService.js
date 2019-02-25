angular
    .module('platformWebApp')
    .factory('platformWebApp.passwordValidationService', ['$timeout', 'platformWebApp.accounts', function ($timeout, accounts) {
        var lastPassword = null;
        var lastPromise = null;

        var performPasswordValidation = function(value) {
            return accounts.validatePassword(JSON.stringify(value)).$promise.then(
                function (response) {
                    var result = {
                        passwordIsValid: response.passwordIsValid,
                        minPasswordLength: response.minPasswordLength,
                        errors: []
                    };

                    var filteredKeys = _.filter(Object.keys(response),
                        function (key) {
                            return !key.startsWith('$') && response[key] === true;
                        });

                    filteredKeys.forEach(function (key) {
                        var resourceName = 'platform.blades.account-resetPassword.validations.' + key;
                        result.errors.push(resourceName);
                    });

                    return result;
                }
            );
        };

        var service = {
            throttleTimeoutMilliseconds: 100,

            validatePasswordAsync: function(value) {
                lastPassword = value;

                if (lastPromise != null) {
                    return lastPromise;
                }

                lastPromise = $timeout(
                    function () {
                        lastPromise = null;
                        return performPasswordValidation(lastPassword);
                    },
                    this.throttleTimeoutMilliseconds);

                return lastPromise;
            }
        }

        return service;
    }]);
