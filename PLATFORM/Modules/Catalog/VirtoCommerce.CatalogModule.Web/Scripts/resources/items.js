angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.items', ['$resource', function ($resource) {

	return $resource('api/catalog/products/:id', { id: '@id' }, {
		get: { method: 'GET', url: 'api/catalog/products/:id' },
		remove: { method: 'DELETE', url: 'api/catalog/products' },
        newItemInCatalog: { method: 'GET', url: 'api/catalog/:catalogId/products/getnew' },
        newItemInCategory: { method: 'GET', url: 'api/catalog/:catalogId/categories/:categoryId/products/getnew' },
        newVariation: { method: 'GET', url: 'api/catalog/products/:itemId/getnewvariation' },
        updateitem: { method: 'POST', url: 'api/catalog/products' }
    });

}]);

