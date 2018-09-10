angular.module('platformWebApp')
.factory('platformWebApp.accounts', ['$resource', function ($resource) {
    return $resource('api/platform/security/users/:id', { id: '@Id' }, {
        search: { method: 'POST' },
        generateNewApiAccount: { url: 'api/platform/security/apiaccounts/new' },
        generateNewApiKey: { url: 'api/platform/security/apiaccounts/newKey', method: 'PUT' },
        save: { url: 'api/platform/security/users/create', method: 'POST' },
        changepassword: { url: 'api/platform/security/users/:id/changepassword', method: 'POST' },
        resetPassword: { url: 'api/platform/security/users/:id/resetpassword', method: 'POST' },
        resetCurrentUserPassword: { url: 'api/platform/security/currentuser/resetpassword', method: 'POST' },
        validatePassword: { url: 'api/platform/security/validatepassword', method: 'POST' },
        update: { method: 'PUT' },
        locked: { url: 'api/platform/security/users/:id/locked', method: 'GET' },
        unlock: { url: 'api/platform/security/users/:id/unlock', method: 'POST' }
    });
}]);
