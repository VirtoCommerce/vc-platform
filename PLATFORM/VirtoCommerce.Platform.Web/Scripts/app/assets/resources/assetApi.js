angular.module('platformWebApp')
.factory('platformWebApp.assets.api', ['$resource', function ($resource) {
    return $resource('api/platform/assets/', {}, {
        search: {},
        move: { method: 'POST', url: 'api/platform/assets/move' },
        uploadFromUrl: { method: 'POST', params: { url: '@url', folder: '@folder' }, url: 'api/platform/assets/:folder' }
    });
}]);

