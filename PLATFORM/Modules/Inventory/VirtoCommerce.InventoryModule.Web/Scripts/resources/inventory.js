angular.module('virtoCommerce.inventoryModule.resources', [])
.factory('inventories', ['$resource', function ($resource) {
    return $resource('api/catalog/products/:id/inventory', { id: '@Id' }, {
        // query: { },
        update: { method: 'PUT' }
    });
}]);