angular.module('virtoCommerce.content.menuModule.resources.menus', [])
.factory('menus', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/menu/', {}, {
		get: { url: 'api/cms/:storeId/menu/', method: 'GET', isArray: true },
		getList: { url: 'api/cms/:storeId/menu/:listId', method: 'GET' },
		createDefaultLinks: { url: 'api/cms/:storeId/menu/default/', method: 'GET', isArray: true },
		checkList: { url: 'api/cms/:storeId/menu/checkname', method:'GET' },
		update: { url: 'api/cms/:storeId/menu/', method: 'POST' },
		delete: { url: 'api/cms/:storeId/menu/', method: 'DELETE' }
	});
}]);