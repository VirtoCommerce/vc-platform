angular.module('platformWebApp').factory('platformWebApp.webApps', ['$resource', '$q', function ($resource, $q) {

    var resource = $resource('/api/platform/apps', {}, {
        list: { url: '/api/platform/apps', isArray: true }
    });

    resource.apps = null;

    resource.loadApps = function () {
        if (resource.apps) {
            // Return cached promise
            return $q.resolve(resource.apps);
        } else {
            // Load and cache
            return resource.list().$promise.then(function (data) {
                resource.apps = data;
                return data;
            });
        }
    };

    return resource;
}]);
