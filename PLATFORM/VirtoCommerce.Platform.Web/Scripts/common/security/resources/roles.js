angular.module('platformWebApp')
.factory('platform_res_roles', ['$resource', function ($resource) {
    return $resource('api/security/roles/:id', { id: '@Id' }, {
        queryPermissions: { url: 'api/security/permissions', isArray: true },
        update: { method: 'PUT' }
    });
}]);