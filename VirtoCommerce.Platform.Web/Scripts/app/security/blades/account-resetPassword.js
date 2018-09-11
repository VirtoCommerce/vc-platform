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
        var promise = accounts.validatePassword(JSON.stringify(value)).$promise.then(
            function(response) {
                blade.currentEntity.errors = [];

                if (response.passwordIsValid) {
                    blade.currentEntity.passwordIsValid = true;
                    return $q.resolve();
                } else {
                    blade.currentEntity.passwordIsValid = false;
                    blade.currentEntity.minPasswordLength = response.minPasswordLength;

                    var filteredKeys = _.filter(Object.keys(response), key => !key.startsWith('$'));
                    filteredKeys.forEach(key => {
                        // Only properties with value of true can indicate violation of some password validation rule,
                        // so let's skip other properties there.
                        if (response[key] === true) {
                            var resourceName = 'platform.blades.account-resetPassword.validations.' + key;
                            blade.currentEntity.errors.push(resourceName);
                        }
                    });

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
