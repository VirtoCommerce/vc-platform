angular.module('platformWebApp')
.factory('settings', ['$resource', function ($resource) {
    return $resource('api/settings/:id', { id: '@Id' }, {
        getModules: { url: 'api/settings/modules', isArray: true },
        getSettings: { url: 'api/settings/modules/:id', isArray: true },
        getValues: { url: 'api/settings/values/:id', isArray: true },
        update: { method: 'POST', url: 'api/settings' }
    });
 
}]);