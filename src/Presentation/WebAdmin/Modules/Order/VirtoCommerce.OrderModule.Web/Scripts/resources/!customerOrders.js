angular.module('virtoCommerce.orderModule.resources', [])
.factory('customerOrders', ['$resource', function ($resource) {
    return $resource('api/cart/:id', { id: '@Id' }, {
        search: { url: 'api/order/customerOrders' },
        get: { url: 'api/order/customerOrders/:id' },
        update: { method: 'PUT', url: 'api/order/customerOrders' }
    });
}]);