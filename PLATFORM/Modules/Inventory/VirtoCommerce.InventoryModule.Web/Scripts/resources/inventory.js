angular.module('virtoCommerce.inventoryModule.resources', [])
.factory('inventories', ['$resource', function ($resource) {
    return $resource('api/inventory/products/:id', { id: '@Id' }, {
        // query: { },
        update: { method: 'PUT' }
    });
}]);