angular.module('platformWebApp')
    .controller('platformWebApp.accountResetPasswordController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.passwordValidationService', function ($q, $scope, bladeNavigationService, accounts, passwordValidationService) {
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
        }

        $scope.validatePasswordAsync = function (value) {
            return passwordValidationService.validatePasswordAsync(value);
        }

        $scope.saveChanges = function () {
            blade.isLoading = true;
            blade.error = undefined;

            var postData = {
                newPassword: blade.currentEntity.newPassword,
                forcePasswordChangeOnNextSignIn: blade.currentEntity.forcePasswordChange
            };

            accounts.resetPassword({ id: blade.currentEntityId }, postData, function (data) {
                $scope.bladeClose();
            }, function (error) {
                bladeNavigationService.setError(error, $scope.blade);
            });
        };

        blade.headIcon = 'fa-key';

        // actions on load
        initializeBlade();
    }]);
