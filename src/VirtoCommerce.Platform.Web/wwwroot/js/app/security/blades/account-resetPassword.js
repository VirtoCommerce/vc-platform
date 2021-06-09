angular.module('platformWebApp')
    .controller('platformWebApp.accountResetPasswordController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.passwordValidationService', function ($q, $scope, bladeNavigationService, accounts, passwordValidationService) {
        var blade = $scope.blade;

        function initializeBlade() {
            blade.currentEntity = {
                newPassword: '',
                forcePasswordChange: true
            };

            blade.isLoading = false;
        }

        $scope.validatePasswordAsync = function (value) {
            return passwordValidationService.validateUserPasswordAsync(blade.currentEntityId, value);
        }

        $scope.saveChanges = function () {
            blade.isLoading = true;
            blade.error = undefined;

            var postData = {
                newPassword: blade.currentEntity.newPassword,
                forcePasswordChangeOnNextSignIn: blade.currentEntity.forcePasswordChange
            };

            accounts.resetPassword({ userName: blade.currentEntityId }, postData, function (data) {
                blade.parentBlade.refresh();
                $scope.bladeClose();
            });
        };

        blade.headIcon = 'fas fa-key';

        // actions on load
        initializeBlade();
    }]);
