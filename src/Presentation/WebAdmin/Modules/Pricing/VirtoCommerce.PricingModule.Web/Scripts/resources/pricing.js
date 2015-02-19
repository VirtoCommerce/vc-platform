angular.module('virtoCommerce.pricingModule.resources.pricing', [])
.factory('prices', ['$resource', function ($resource) {
    return $resource('api/catalog/products/:id/prices', { id: '@Id' }, {
        // query: { isArray: true },
        update: { method: 'PUT', url: 'api/catalog/products/:id/prices' }
    });
}]);