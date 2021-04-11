angular
    .module('platformWebApp')
    .factory('platformWebApp.passwordValidationService', ['$timeout', 'platformWebApp.accounts', function ($timeout, accounts) {
        var lastPassword = null;
        var lastPromise = null;

        var performPasswordValidation = function (value) {
            return accounts.validatePassword(JSON.stringify(value)).$promise.then((result) => {
                _.each(result.errors, (x) => {
                    x.descriptionKey = 'platform.blades.account-resetPassword.validations.' + x.code.charAt(0).toLowerCase() + x.code.slice(1);
                });

                return result;
            });
        };

        var performUserPasswordValidation = function (username, value) {
            return accounts.validateUserPassword({ userName: username, newPassword: value }).$promise.then((result) => {
                _.each(result.errors, (x) => {
                    x.descriptionKey = 'platform.blades.account-resetPassword.validations.' + x.code.charAt(0).toLowerCase() + x.code.slice(1);
                });

                return result;
            });
        };

        var service = {
            throttleTimeoutMilliseconds: 100,

            validatePasswordAsync: function (value) {
                lastPassword = value;

                if (lastPromise !== null) {
                    return lastPromise;
                }

                lastPromise = $timeout(
                    function () {
                        lastPromise = null;
                        return performPasswordValidation(lastPassword);
                    },
                    this.throttleTimeoutMilliseconds);

                return lastPromise;
            },
            validateUserPasswordAsync: function (username, value) {
                lastPassword = value;

                if (lastPromise !== null) {
                    return lastPromise;
                }

                lastPromise = $timeout(
                    function () {
                        lastPromise = null;
                        return performUserPasswordValidation(username, lastPassword);
                    },
                    this.throttleTimeoutMilliseconds);

                return lastPromise;
            }
        }

        return service;
    }]);
