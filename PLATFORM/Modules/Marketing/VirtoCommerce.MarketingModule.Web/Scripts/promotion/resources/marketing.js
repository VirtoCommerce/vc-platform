angular.module('virtoCommerce.marketingModule')
    .factory('marketing_res_search', ['$resource', function ($resource) {
    	return $resource('api/marketing/search', {}, {
    		search: {}
    	});
    }])
    //.factory('marketing_res_contentItems', ['$resource', function ($resource) {
    //	return $resource('api/marketing/contentitems/:id', {
    //		get: { method: 'GET' },
    //		create: { method: 'POST'},
    //		update: { method: 'PUT' },
    //		remove: { method: 'DELETE' },
    //	});
    //}])
    //.factory('marketing_res_contentPlaces', ['$resource', function ($resource) {
    //	return $resource('api/marketing/contentplaces:/id', {
    //		create: { method: 'POST' },
    //		update: { method: 'PUT' },
    //		remove: { method: 'DELETE' },
    //	});
    //}])
    //.factory('marketing_res_contentPublications', ['$resource', function ($resource) {
    //	return $resource('api/marketing/contentpublications/:id', {
    //		create: { method: 'POST' },
    //		update: { method: 'PUT'},
    //		remove: { method: 'DELETE'},
    //	});
    //}])
    .factory('marketing_res_promotions', ['$resource', function ($resource) {
    	return $resource('api/marketing/promotions/:id', { id: '@Id' }, {
    		getNew: { url: 'api/marketing/promotions/new' },
    		update: { method: 'PUT' }
    	});
    }]);
