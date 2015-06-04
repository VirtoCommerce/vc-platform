angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.properties', ['$resource', function ($resource) {

	return $resource('api/catalog/properties/:id', { id: '@id' }, {
		newCategoryProperty: { method: 'GET', url: 'api/catalog/categories/:categoryId/properties/getnew' },
		newCatalogProperty: { method: 'GET', url: 'api/catalog/:catalogId/properties/getnew' },
		get: { method: 'GET', url: 'api/catalog/properties/:propertyId' },
		update: { method: 'POST', url: 'api/catalog/properties' },
		values: { url: 'api/catalog/properties/:propertyId/values', isArray: true },
		remove: { method: 'DELETE', url: 'api/catalog/properties' }
    });

}]);