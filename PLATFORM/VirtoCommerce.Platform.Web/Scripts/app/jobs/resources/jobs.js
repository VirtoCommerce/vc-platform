angular.module('platformWebApp')
.factory('platformWebApp.jobs', ['$resource', function ($resource) {

    return $resource('api/jobs', {}, {
        getStatus: { url: 'api/jobs/:id' }
    });
}]);
