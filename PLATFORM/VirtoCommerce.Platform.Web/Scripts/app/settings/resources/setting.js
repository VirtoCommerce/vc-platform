angular.module('platformWebApp')
.factory('platformWebApp.settings', ['$resource', function ($resource) {
    return $resource('api/settings/:id', { id: '@Id' }, {
        getSettings: { url: 'api/settings/modules/:id', isArray: true },
        getValues: { url: 'api/settings/values/:id', isArray: true },
        update: { method: 'POST', url: 'api/settings' }
    });
 
}]);