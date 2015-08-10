angular.module('platformWebApp')
.factory('platformWebApp.roles', ['$resource', function ($resource) {
    return $resource('api/platform/security/roles/:id', { id: '@Id' }, {
        queryPermissions: { url: 'api/platform/security/permissions', isArray: true },
        update: { method: 'PUT' }
    });
}]);