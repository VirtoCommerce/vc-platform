angular.module('platformWebApp')
.factory('platformWebApp.settings', ['$resource', function ($resource) {
    return $resource('api/platform/settings/:id', { id: '@Id' }, {
        getSettings: { url: 'api/platform/settings/modules/:id', isArray: true },
      	getValues: { url: 'api/platform/settings/values/:id', isArray: true },    	
      	update: { method: 'POST', url: 'api/platform/settings' },
        // do not use 'api/platform/settings/currentuser' because in this case 'currentuser' match ':id' in 'api/platform/settings/:id'
        getCurrentUserProfile: { url: 'api/platform/profiles/currentuser', isArray: true },
        updateCurrentUserProfile: { method: 'POST', url: 'api/platform/profiles/currentuser' }
    });
}]);