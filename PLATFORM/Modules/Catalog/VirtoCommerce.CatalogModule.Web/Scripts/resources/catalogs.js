angular.module('virtoCommerce.catalogModule')
.factory('catalogs', ['$resource', function ($resource) {

	return $resource('api/catalog/catalogs/:id', { id: '@Id' }, {
		get: { method: 'GET', url: 'api/catalog/catalogs/:id' },
		getCatalogs: { method: 'GET', url: 'api/catalog/catalogs', isArray: true },
		newCatalog: { method: 'GET', url: 'api/catalog/catalogs/getnew' },
		newVirtualCatalog: { method: 'GET', url: 'api/catalog/catalogs/getnewvirtual' },
		update: { method: 'PUT', url: 'api/catalog/catalogs' },
		create: { method: 'POST', url: 'api/catalog/catalogs' },
		getCatalogLanguages: { method: 'GET', url: 'api/catalog/catalogs/:id/languages', isArray: true },
		updateCatalogLanguages: { method: 'POST', url: 'api/catalog/catalogs/:catalogId/languages' },
		delete: { method: 'DELETE', url: 'api/catalog/catalogs/:id' }
    });
}]);

