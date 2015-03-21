angular.module('virtoCommerce.orderModule')
.factory('order_res_customerOrders', ['$resource', function ($resource) {
    return $resource('api/cart/:id', { id: '@Id' }, {
        search: { url: 'api/order/customerOrders' },
        get: { url: 'api/order/customerOrders/:id' },
        getNewShipment: { url: 'api/order/customerOrders/:id/shipments/new' },
        getNewPayment: { url: 'api/order/customerOrders/:id/payments/new' },
        update: { method: 'PUT', url: 'api/order/customerOrders' },
        deleteOperation: { method: 'DELETE', url: 'api/order/customerOrders/:id/operations/:operationId' },
        delete: { method: 'DELETE', url: 'api/order/customerOrders' },

    });
}]);