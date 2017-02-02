angular.module('platformWebApp')
.factory('platformWebApp.licensingApi', ['$resource', function ($resource) {
    return $resource(null, null, { activateByCode: { method: 'POST', url: 'api/platform/licensing/activateByCode' } });
}]);