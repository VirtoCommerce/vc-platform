angular.module('virtoCommerce.marketingModule')
	.factory('marketing_dynamicContents_res_search', ['$resource', function ($resource) {
		return $resource('api/marketing/search', {}, {
			search: {}
		});
	}])
    .factory('marketing_dynamicContents_res_contentItems', ['$resource', function ($resource) {
    	return $resource('api/marketing/contentitems/:id', { id: '@Id' }, {
    		get: { method: 'GET' },
    		create: { method: 'POST' },
    		update: { method: 'PUT' },
    		remove: { method: 'DELETE' },
    	});
    }])
    .factory('marketing_dynamicContents_res_contentPlaces', ['$resource', function ($resource) {
    	return $resource('api/marketing/contentplaces/:id', { id: '@Id' }, {
    		get: { method: 'GET' },
    		update: { method: 'PUT' },
    		remove: { method: 'DELETE' },
    	});
    }])
    .factory('marketing_dynamicContents_res_contentPublications', ['$resource', function ($resource) {
    	return $resource('api/marketing/contentpublications/:id', { id: '@Id' }, {
    		get: { method: 'GET' },
    		update: { method: 'PUT' },
    		remove: { method: 'DELETE' },
    	});
    }])
	.factory('marketing_dynamicContents_res_folders', ['$resource', function ($resource) {
		return $resource('api/marketing/contentfolders/:id', { id: '@Id' }, {
			get: { method: 'GET' },
			update: { method: 'PUT' },
			remove: { method: 'DELETE' },
		});
	}]);
