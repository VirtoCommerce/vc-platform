angular.module('platformWebApp').factory('platformWebApp.oauthapps', ['$resource', function ($resource) {
    return $resource('api/platform/oauthapps/:clientId', { clientId: '@clientId' }, {
        search: { url: 'api/platform/oauthapps/search', method: 'POST' },
        new: { url: 'api/platform/oauthapps/new', method: 'GET' },
        save: { url: 'api/platform/oauthapps', method: 'POST' },
        delete: { url: 'api/platform/oauthapps', method: 'DELETE' },
    });
}]);
