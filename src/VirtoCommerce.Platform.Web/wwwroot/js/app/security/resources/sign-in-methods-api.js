angular.module('platformWebApp')
    .factory('platformWebApp.signInMethodsApi', ['$resource', function ($resource) {
        return $resource('api/platform/security/sign-in-methods', {}, {
            getSignInMethods: { method: 'GET', isArray: true },
            signIn: { method: 'POST', url: 'api/platform/security/sign-in' },
            getCurrentUserSignInMethods: { method: 'GET', url: 'api/platform/security/currentuser/sign-in-methods', isArray: true },
            renameKey: { method: 'PUT', url: 'api/platform/security/currentuser/sign-in-methods/:name/keys/:id' },
            deleteKey: { method: 'DELETE', url: 'api/platform/security/currentuser/sign-in-methods/:name/keys/:id' }
        });
    }]);
