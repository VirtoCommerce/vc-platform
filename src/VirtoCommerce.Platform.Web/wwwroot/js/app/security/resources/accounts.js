angular.module('platformWebApp').factory('platformWebApp.accounts', ['$resource', function ($resource) {
    return $resource('api/platform/security/users/:id', { id: '@Id' }, {
        search: { method: 'POST' },
        save: { url: 'api/platform/security/users/create', method: 'POST' },
        changepassword: { url: 'api/platform/security/users/:id/changepassword', method: 'POST' },
        resetPassword: { url: 'api/platform/security/users/:id/resetpassword', method: 'POST' },
        resetCurrentUserPassword: { url: 'api/platform/security/currentuser/resetpassword', method: 'POST' },
        validatePassword: { url: 'api/platform/security/validatepassword', method: 'POST' },
        update: { method: 'PUT' },
        locked: { url: 'api/platform/security/users/:id/locked', method: 'GET' },
        unlock: { url: 'api/platform/security/users/:id/unlock', method: 'POST' },
        lock: { url: 'api/platform/security/users/:id/lock', method: 'POST' },
        getUserApiKeys: { url: 'api/platform/security/users/:id/apikeys', method: 'GET', isArray: true},
        saveUserApiKey: { url: 'api/platform/security/users/apikeys', method: 'POST' },
        deleteUserApiKey: { url: 'api/platform/security/users/apikeys', method: 'DELETE' }
    });
}]);
