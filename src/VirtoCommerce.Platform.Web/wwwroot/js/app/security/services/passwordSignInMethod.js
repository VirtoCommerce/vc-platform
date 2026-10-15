angular.module('platformWebApp')
    .run(['platformWebApp.signInMethodService', 'platformWebApp.dialogService',
        function (signInMethodService, dialogService) {
            signInMethodService.onSignInMethodsLoaded(function (signInMethods) {
                _.each(_.where(signInMethods, { kind: 'Password' }), extendSignInMethod);
            });

            function extendSignInMethod(signInMethod) {
                signInMethodService.extendSignInMethod(signInMethod.name, {
                    keyTemplateUrl: '$(Platform)/Scripts/app/userProfile/templates/password-sign-in-key.html',
                    noKeysTitle: 'platform.blades.sign-in-methods-list.labels.password-not-set',
                    createKey: function () {
                        const dialog = {
                            id: 'changePasswordDialog',
                            requireCurrentPassword: false
                        };

                        dialogService.showDialog(dialog, '$(Platform)/Scripts/app/security/dialogs/changePasswordDialog.tpl.html', 'platformWebApp.changePasswordDialog');

                        return dialog.instance.result;
                    }
                });
            }
        }]);
