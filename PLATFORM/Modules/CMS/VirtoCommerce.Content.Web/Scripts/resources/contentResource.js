angular.module('virtoCommerce.contentModule')
.factory('virtoCommerce.contentModule.menus', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/menu/', {}, {
		get: { url: 'api/cms/:storeId/menu/', method: 'GET', isArray: true },
		getList: { url: 'api/cms/:storeId/menu/:listId', method: 'GET' },
		checkList: { url: 'api/cms/:storeId/menu/checkname', method: 'GET' },
		update: { url: 'api/cms/:storeId/menu/', method: 'POST' },
		delete: { url: 'api/cms/:storeId/menu/', method: 'DELETE' }
	});
}])
.factory('virtoCommerce.contentModule.pages', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/pages/', {}, {
		get: { url: 'api/cms/:storeId/pages/', method: 'GET', isArray: true },
		getFolders: { url: 'api/cms/:storeId/pages/folders', method: 'GET'},
		getPage: { url: 'api/cms/:storeId/pages/:language/:pageName', method: 'GET' },
		checkName: { url: 'api/cms/:storeId/pages/checkname', method: 'GET' },
		update: { url: 'api/cms/:storeId/pages/', method: 'POST' },
		delete: { url: 'api/cms/:storeId/pages/', method: 'DELETE' }
	});
}])
.factory('virtoCommerce.contentModule.themes', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/themes/', {}, {
		get: { url: 'api/cms/:storeId/themes/', method: 'GET', isArray: true },
		deleteTheme: { url: 'api/cms/:storeId/themes/:themeId', method: 'DELETE' },
		getAssets: { url: 'api/cms/:storeId/themes/:themeId/folders', method: 'GET', isArray: true },
		getAsset: { url: 'api/cms/:storeId/themes/:themeId/assets/:assetId', method: 'GET' },
		updateAsset: { url: 'api/cms/:storeId/themes/:themeId/assets', method: 'POST' },
		deleteAsset: { url: 'api/cms/:storeId/themes/:themeId/assets', method: 'DELETE' },
		createTheme: { url: 'api/cms/:storeId/themes/file', method: 'GET' }
	});
}])
.factory('virtoCommerce.contentModule.stores', ['$resource', function ($resource) {
	return $resource('api/stores', {}, {
		get: { url: 'api/stores/:id' },
		update: { method: 'PUT' }
	});
}]);