angular.module('platformWebApp')
    .run(['$window', 'platformWebApp.signInMethodService', 'platformWebApp.externalSignInStorage',
        function ($window, signInMethodService, externalSignInStorage) {
            signInMethodService.onSignInMethodsLoaded(function (signInMethods) {
                _.each(_.where(signInMethods, { kind: 'External' }), extendSignInMethod);
            });

            function extendSignInMethod(signInMethod) {
                signInMethodService.extendSignInMethod(signInMethod.name, {
                    keyTemplateUrl: '$(Platform)/Scripts/app/userProfile/templates/external-sign-in-key.html',
                    signIn: function () {
                        externalSignInStorage.set({ providerType: signInMethod.name });
                        $window.location.href = 'externalsignin?authenticationType=' + signInMethod.name;
                    }
                });
            }
        }]);
