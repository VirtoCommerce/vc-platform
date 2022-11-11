angular.module('platformWebApp').factory('platformWebApp.webApps', ['$resource', function ($resource) {
    return $resource('/api/platform/apps', {} , {
        list: { url: '/api/platform/apps', isArray: true }
    });
}]);
