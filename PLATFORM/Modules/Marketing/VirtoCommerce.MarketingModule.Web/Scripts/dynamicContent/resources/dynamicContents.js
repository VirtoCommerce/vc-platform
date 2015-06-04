angular.module('virtoCommerce.marketingModule')
	.factory('virtoCommerce.marketingModule.dynamicContent.search', ['$resource', function ($resource) {
	    return $resource('api/marketing/search', {}, {
	        search: {}
	    });
	}])
    .factory('virtoCommerce.marketingModule.dynamicContent.contentItems', ['$resource', function ($resource) {
        return $resource('api/marketing/contentitems/:id', { id: '@Id' }, {
            get: { method: 'GET' },
            create: { method: 'POST' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' },
        });
    }])
    .factory('virtoCommerce.marketingModule.dynamicContent.contentPlaces', ['$resource', function ($resource) {
        return $resource('api/marketing/contentplaces/:id', { id: '@Id' }, {
            get: { method: 'GET' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' },
        });
    }])
    .factory('virtoCommerce.marketingModule.dynamicContent.contentPublications', ['$resource', function ($resource) {
        return $resource('api/marketing/contentpublications/:id', { id: '@Id' }, {
            getNew: { url: 'api/marketing/contentpublications/new' },
            update: { method: 'PUT' },
            remove: { method: 'DELETE' },
        });
    }])
	.factory('virtoCommerce.marketingModule.dynamicContent.folders', ['$resource', function ($resource) {
	    return $resource('api/marketing/contentfolders/:id', { id: '@Id' }, {
	        get: { method: 'GET' },
	        update: { method: 'PUT' },
	        remove: { method: 'DELETE' },
	    });
	}]);
