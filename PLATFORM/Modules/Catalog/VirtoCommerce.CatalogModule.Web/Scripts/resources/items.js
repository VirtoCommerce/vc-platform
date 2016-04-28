angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.items', ['$resource', function ($resource) {
    return $resource('api/catalog/products/:id', null, {
        remove: { method: 'DELETE', url: 'api/catalog/products' },
        newItemInCatalog: { method: 'GET', url: 'api/catalog/:catalogId/products/getnew' },
        newItemInCategory: { method: 'GET', url: 'api/catalog/:catalogId/categories/:categoryId/products/getnew' },
        newVariation: { method: 'GET', url: 'api/catalog/products/:itemId/getnewvariation' },
        cloneItem: { method: 'GET', url: 'api/catalog/products/:itemId/clone' },
        update: { method: 'POST' }
    });
}]);

