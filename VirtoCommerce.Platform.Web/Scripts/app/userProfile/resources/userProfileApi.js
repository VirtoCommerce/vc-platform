angular.module('platformWebApp')
.factory('platformWebApp.localization', ['$resource', function ($resource) {
    return $resource('api/platform/localization/locales', {},
    {
        get: { url: 'api/platform/security/users/:id/locale' },
        update: { url: 'api/platform/security/users/:id/locale', method: 'PUT' }
    });
}]);