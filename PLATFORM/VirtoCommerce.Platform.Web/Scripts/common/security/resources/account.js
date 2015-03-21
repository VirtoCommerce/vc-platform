angular.module('platformWebApp')
.factory('accounts', ['$resource', function ($resource) {
    return $resource('api/security/accounts/:id', { id: '@Id' }, {
        // query: { isArray: true },
        update: { method: 'PUT' }
    });
}]);