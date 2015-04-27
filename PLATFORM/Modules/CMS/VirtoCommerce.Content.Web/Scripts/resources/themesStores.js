angular.module('virtoCommerce.content.themeModule.resources.themesStores', [])
.factory('themesStores', ['$resource', function ($resource) {
	return $resource('api/stores', {}, {
		get: { url: 'api/stores/:id' },
		update: { method: 'PUT' }
	});
}]);