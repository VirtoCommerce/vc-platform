angular.module('platformWebApp').factory('platformWebApp.accounts', ['$resource', function ($resource) {
    return $resource('api/platform/security/users/:id', { id: '@Id' }, {
        search: { url: 'api/platform/security/users/search', method:'POST' },
        save: { url: 'api/platform/security/users/create', method: 'POST' },
        changepassword: { url: 'api/platform/security/users/:id/changepassword', method: 'POST' },
        changeCurrentUserPassword: { url: 'api/platform/security/currentuser/changepassword', method: 'POST' },
        resetPassword: { url: 'api/platform/security/users/:userName/resetpassword', method: 'POST' },
        validatePassword: { url: 'api/platform/security/validatepassword', method: 'POST' },
        validateUserPassword: { url: 'api/platform/security/validateuserpassword', method: 'POST' },
        update: { method: 'PUT' },
        locked: { url: 'api/platform/security/users/:id/locked', method: 'GET' },
        unlock: { url: 'api/platform/security/users/:id/unlock', method: 'POST' },
        lock: { url: 'api/platform/security/users/:id/lock', method: 'POST' },
        getUserApiKeys: { url: 'api/platform/security/users/:id/apikeys', method: 'GET', isArray: true},
        saveUserApiKey: { url: 'api/platform/security/users/apikeys', method: 'POST' },
        deleteUserApiKey: { url: 'api/platform/security/users/apikeys', method: 'DELETE' },
        verifyEmail: { url: 'api/platform/security/users/:userId/sendVerificationEmail', method: 'POST' },
        passwordChangeEnabled: { url: 'api/platform/security/passwordchangeenabled', method: 'GET' }
    });
}]);
