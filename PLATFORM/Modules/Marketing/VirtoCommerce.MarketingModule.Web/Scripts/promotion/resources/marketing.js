angular.module('virtoCommerce.marketingModule')
    .factory('virtoCommerce.marketingModule.promotion.search', ['$resource', function ($resource) {
    	return $resource('api/marketing/search', {}, {
    		search: {}
    	});
    }])
    .factory('virtoCommerce.marketingModule.promotions', ['$resource', function ($resource) {
    	return $resource('api/marketing/promotions/:id', { id: '@Id' }, {
    		getNew: { url: 'api/marketing/promotions/new' },
    		update: { method: 'PUT' }
    	});
    }]);
