angular.module('platformWebApp')
.factory('platformWebApp.jobs', ['$resource', function ($resource) {

    return $resource('api/platform/jobs', {}, {
        getStatus: { url: 'api/platform/jobs/:id' }
    });
}]);
