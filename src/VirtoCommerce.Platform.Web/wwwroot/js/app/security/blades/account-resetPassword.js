angular.module('platformWebApp')
    .controller('platformWebApp.accountResetPasswordController', ['$q', '$scope', '$translate', 'platformWebApp.accounts', 'platformWebApp.passwordValidationService', function ($q, $scope, $translate, accounts, passwordValidationService) {
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
                blade.isLoading = false;

                if (!data.succeeded) {
                    blade.error = data.errors && data.errors.length ?
                        data.errors[0] :
                        $translate.instant('platform.blades.resetpassword.labels.fail');
                }
                else {
                    blade.parentBlade.refresh();
                    $scope.bladeClose();
                }
            });
        };

        blade.headIcon = 'fas fa-key';

        // actions on load
        initializeBlade();
    }]);
