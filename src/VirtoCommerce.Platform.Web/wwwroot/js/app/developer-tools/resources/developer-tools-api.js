angular.module('platformWebApp').factory('platformWebApp.developerToolsApi', ['$resource', function ($resource) {
    return $resource('api/platform/developer-tools', {}, {
        get: { isArray: true },
    });
}]);
