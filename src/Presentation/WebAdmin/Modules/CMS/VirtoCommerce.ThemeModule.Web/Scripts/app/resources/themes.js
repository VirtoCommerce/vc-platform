angular.module('virtoCommerce.content.themesModule.resources.themes', [])
.factory('themes', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/themes/', {}, {
		get: { url: 'api/cms/:storeId/themes/' },
	});
}]);