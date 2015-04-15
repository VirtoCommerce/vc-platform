angular.module('platformWebApp')
.factory('accounts', ['$resource', function ($resource) {
    return $resource('api/security/users/:id', { id: '@Id' }, {
        search: {},
        generateNewApiAccount: { url: 'api/security/apiaccounts/new' },
        save: { url: 'api/security/users/create', method: 'POST' },
        changepassword: { url: 'api/security/users/:id/changepassword', method: 'POST' },
        update: { method: 'PUT' }
    });
}]);