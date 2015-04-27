angular.module('virtoCommerce.content.menuModule.resources.menusStores', [])
.factory('menusStores', ['$resource', function ($resource) {
	return $resource('api/stores', {}, {
		get: { url: 'api/stores/:id' },
		update: { method: 'PUT' }
	});
}]);