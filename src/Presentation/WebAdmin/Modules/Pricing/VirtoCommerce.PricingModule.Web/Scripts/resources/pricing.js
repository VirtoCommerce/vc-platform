angular.module('virtoCommerce.pricingModule.resources.pricing', [])
.factory('prices', ['$resource', function ($resource) {
	return $resource('api/catalog/products/:id/pricelists', { id: '@Id' }, {
		getProductPrices: { url: 'api/products/:id/prices', isArray: true },
        // query: { isArray: true },
        update: { method: 'PUT' }
    });
}]);