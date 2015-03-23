angular.module('platformWebApp')
.factory('accounts', ['$resource', function ($resource) {
    return $resource('api/security/users/:id', { id: '@Id' }, {
        search: {},
        update: { method: 'PUT' },
        generateNewApiAccount: { url: 'api/security/apiaccounts/new' },
        changepassword: { url: 'api/security/users/:id/changepassword', method: 'POST' }
    });
}]);