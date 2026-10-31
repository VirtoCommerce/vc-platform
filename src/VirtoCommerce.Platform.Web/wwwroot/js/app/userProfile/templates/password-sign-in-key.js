angular.module('platformWebApp')
    .controller('platformWebApp.passwordSignInKeyController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {
        $scope.changePassword = function () {
            const dialog = {
                id: 'changePasswordDialog',
                callback: $scope.blade.refresh
            };

            dialogService.showDialog(dialog, '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html', 'platformWebApp.changePasswordDialog');
        };
    }]);
