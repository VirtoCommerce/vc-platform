angular.module('virtoCommerce.pricingModule')
    .factory('virtoCommerce.pricingModule.prices', ['$resource', function ($resource) {
        return $resource('api/catalog/products/:id/pricelists', { id: '@Id' }, {
            getProductPrices: { url: 'api/products/:id/prices', isArray: true },
            // query: { isArray: true },
            update: { method: 'PUT' }
        });
    }])
    .factory('virtoCommerce.pricingModule.pricelists', ['$resource', function ($resource) {
        return $resource('api/pricing/pricelists/:id', { id: '@Id' }, {
            update: { method: 'PUT' }
        });
    }])
    .factory('virtoCommerce.pricingModule.pricelistAssignments', ['$resource', function ($resource) {
        return $resource('api/pricing/assignments/:id', { id: '@Id' }, {
            update: { method: 'PUT' }
        });
    }]);