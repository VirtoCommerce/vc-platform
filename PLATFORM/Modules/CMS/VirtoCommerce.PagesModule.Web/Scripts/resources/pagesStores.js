angular.module('virtoCommerce.content.pagesModule.resources.pagesStores', [])
.factory('pagesStores', ['$resource', function ($resource) {
	return $resource('api/stores', {}, {
		get: { url: 'api/stores/:id' },
		update: { method: 'PUT' }
	});
}]);