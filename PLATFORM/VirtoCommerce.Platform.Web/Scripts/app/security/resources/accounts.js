angular.module('platformWebApp')
.factory('platformWebApp.accounts', ['$resource', function ($resource) {
    return $resource('api/platform/security/users/:id', { id: '@Id' }, {
        search: { method: 'POST' },
        generateNewApiAccount: { url: 'api/platform/security/apiaccounts/new' },
        save: { url: 'api/platform/security/users/create', method: 'POST' },
        changepassword: { url: 'api/platform/security/users/:id/changepassword', method: 'POST' },
        resetPassword: { url: 'api/platform/security/users/:id/resetpassword', method: 'POST' },
        update: { method: 'PUT' }
    });
}]);