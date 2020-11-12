angular.module('platformWebApp').factory('platformWebApp.changeLogApi', ['$resource', function ($resource) {
    return $resource('api/platform/changelog', {}, {
        search: { method: 'POST', url: 'api/platform/changelog/search', isArray: true },
        forceChanges: { method: 'POST', url: 'api/changes/force' }
    });
}]);
