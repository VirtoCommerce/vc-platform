angular.module('virtoCommerce.quoteModule')
.factory('virtoCommerce.quoteModule.quotes', ['$resource', function ($resource) {
    return $resource('api/quote/requests/:id', {}, {
        search: {},
        recalculate: { method: 'PUT', url: 'api/quote/requests/recalculate' },
        update: { method: 'PUT' }
    });
}]);