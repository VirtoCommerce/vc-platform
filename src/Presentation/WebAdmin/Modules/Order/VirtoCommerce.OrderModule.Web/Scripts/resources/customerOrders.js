angular.module('virtoCommerce.orderModule.resources.customerOrders', [])
.factory('customerOrders', ['$resource', function ($resource) {
    return $resource('api/cart/:id', { id: '@Id' }, {
        search: { url: 'api/order/customerOrders' },
        get: { url: 'api/order/customerOrders/:id' },
        update: { method: 'POST', url: 'api/order/customerOrders' }
    });
}]);