angular.module('platformWebApp').directive('vaHeaderPasswordWidget', ['$rootScope', 'platformWebApp.authService', 'platformWebApp.dialogService',
    function ($rootScope, authService, dialogService) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: '$(Platform)/Scripts/app/security/directives/headerPasswordWidget.tpl.html',
            link: function (scope) {
                scope.daysTillPasswordExpiry = authService.daysTillPasswordExpiry;

                $rootScope.$on('userPasswordChanged', function (event, authContext) {
                    if (authContext.isAuthenticated) {
                        scope.daysTillPasswordExpiry = authService.daysTillPasswordExpiry;
                    }
                });

                scope.changePassword = () => {
                    var dialog = {
                        id: "changePasswordDialog",
                        callback: (confirm) => {
                            if (confirm) {
                                scope.daysTillPasswordExpiry = authService.daysTillPasswordExpiry = -1;
                            }
                        }
                    };
                    dialogService.showDialog(dialog, '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html', 'platformWebApp.changePasswordDialog');
                };
            }
        }
    }]);
