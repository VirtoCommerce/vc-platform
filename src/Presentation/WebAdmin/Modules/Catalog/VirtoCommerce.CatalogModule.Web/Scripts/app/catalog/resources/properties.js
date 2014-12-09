angular.module('catalogModule.resources.properties', [])
.factory('properties', ['$resource', function ($resource) {

	return $resource('api/catalog/properties/:id', { id: '@id' }, {
		newProperty: { method: 'GET', url: 'api/catalog/categories/:categoryId/properties/getnew'},
		get: { method: 'GET', url: 'api/catalog/properties/:propertyId' },
		update: { method: 'POST', url: 'api/catalog/properties' },
		values: { url: 'api/catalog/properties/:propertyId/values', isArray: true },
		remove: { method: 'DELETE', url: 'api/catalog/properties' }
    });

}]);