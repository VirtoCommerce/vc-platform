angular.module('platformWebApp')
.factory('platformWebApp.roles', ['$resource', function ($resource) {
    return $resource('api/security/roles/:id', { id: '@Id' }, {
        queryPermissions: { url: 'api/security/permissions', isArray: true },
        update: { method: 'PUT' }
    });
}]);