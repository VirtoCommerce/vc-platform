angular.module('platformWebApp')
    .directive('vaPasswordInput', ['$q', function($q) {
        return {
            restrict: 'E',
            templateUrl: '$(Platform)/Scripts/app/security/directives/password-input.tpl.html',
            scope: {
                runPasswordValidation: '&',
                passwordPlaceholder: '@passwordPlaceholder',
                passwordTooWeakMessage: '@',
                newPassword: '='
            },
            link: function (scope) {
                scope.doValidatePasswordAsync = function (value) {
                    return scope.runPasswordValidation({ value: value }).then(
                        function (result) {
                            scope.passwordValidationResult = result;
                            return result.succeeded ? $q.resolve() : $q.reject();
                        });
                }
            }
        }
    }]);
