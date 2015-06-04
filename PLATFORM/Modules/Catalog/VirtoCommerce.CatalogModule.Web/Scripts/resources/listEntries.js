angular.module('virtoCommerce.catalogModule')
.factory('virtoCommerce.catalogModule.listEntries', ['$resource', function ($resource) {

	return $resource('api/catalog/listentries', {},
    {
    	listitemssearch: { method: 'GET', isArray: false, url: 'api/catalog/listentries' },
    	createlinks: { method: 'POST', url: 'api/catalog/listentrylinks' },
    	deletelinks: { method: 'POST', url: 'api/catalog/listentrylinks/delete' },
    	query: { method: 'GET', isArray: false, url: 'api/catalog/listentries' }
    });


}]);

