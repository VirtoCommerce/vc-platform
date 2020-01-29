angular.module('platformWebApp').factory('platformWebApp.roles', ['$resource', function ($resource) {
    return $resource('api/platform/security/roles/:roleName', { roleName: '@roleName' }, {
        search: { url: 'api/platform/security/roles/search', method: 'POST' },
        queryPermissions: { url: 'api/platform/security/permissions', isArray: true },
        update: { method: 'PUT' },
        create: { method: 'PUT' },
    });
}]);
