angular.module('platformWebApp').factory('platformWebApp.login', ['$resource', function ($resource) {
    return $resource('', {}, {
        getLoginTypes: { url: 'api/platform/security/logintypes', method: 'GET', isArray: true }
    });
}]);
