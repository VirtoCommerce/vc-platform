angular.module('virtoCommerce.quoteModule')
.factory('virtoCommerce.quoteModule.quotes', ['$resource', function ($resource) {
    return $resource('api/quote/requests/:id', {}, {
        search: { method: 'POST', url: 'api/quote/requests/search' },
        getShippingMethods: { url: 'api/quote/requests/:id/shipmentmethods', isArray: true },
        recalculate: { method: 'PUT', url: 'api/quote/requests/recalculate' },
        update: { method: 'PUT' }
    });
}]);