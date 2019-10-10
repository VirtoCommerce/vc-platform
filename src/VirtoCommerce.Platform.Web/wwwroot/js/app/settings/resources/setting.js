angular.module('platformWebApp')
.factory('platformWebApp.settings', ['$resource', function ($resource) {
    return $resource('api/platform/settings/:id', { id: '@Id' }, {
        getSettings: { url: 'api/platform/settings/modules/:id', isArray: true },
      	getValues: { url: 'api/platform/settings/values/:id', isArray: true },    	
      	update: { method: 'POST', url: 'api/platform/settings' },
        getUiCustomizationSetting: { url: 'api/platform/settings/ui/customization' }
    });
}]);