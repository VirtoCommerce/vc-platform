angular.module('virtoCommerce.orderModule')
.factory('virtoCommerce.orderModule.order_res_customerOrders', ['$resource', function ($resource) {
    return $resource('api/order/customerOrders/:id', { id: '@Id' }, {
        search: { url: 'api/order/customerOrders' },
        getNewShipment: { url: 'api/order/customerOrders/:id/shipments/new' },
        getNewPayment: { url: 'api/order/customerOrders/:id/payments/new' },
        update: { method: 'PUT', url: 'api/order/customerOrders' },
        deleteOperation: { method: 'DELETE', url: 'api/order/customerOrders/:id/operations/:operationId' },
        delete: { method: 'DELETE', url: 'api/order/customerOrders' },
        getDashboardStatistics: { url: 'api/order/dashboardStatistics' }
    });
}]);