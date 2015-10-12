angular.module('platformWebApp')
.factory('platformWebApp.assets.api', ['$resource', function ($resource) {
	return $resource('', {}, {
		uploadFromUrl: { method: 'POST', params: { url: '@url', folder: '@folder' }, url: 'api/platform/assets/:folder' }		
    });

}]);

