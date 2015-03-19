angular.module('virtoCommerce.content.pagesModule.resources.pages', [])
.factory('pages', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/pages/', {}, {
		get: { url: 'api/cms/:storeId/pages/', method: 'GET', isArray: true },
		getPage: { url: 'api/cms/:storeId/pages/:language/:pageName', method: 'GET' },
		checkName: { url: 'api/cms/:storeId/pages/checkname', method: 'GET' },
		update: { url: 'api/cms/:storeId/pages/', method: 'POST' },
		delete: { url: 'api/cms/:storeId/pages/', method: 'DELETE' }
	});
}]);