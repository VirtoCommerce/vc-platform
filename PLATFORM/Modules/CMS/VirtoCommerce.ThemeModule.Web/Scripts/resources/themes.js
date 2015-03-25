angular.module('virtoCommerce.content.themeModule.resources.themes', [])
.factory('themes', ['$resource', function ($resource) {
	return $resource('api/cms/:storeId/themes/', {}, {
		get: { url: 'api/cms/:storeId/themes/', method: 'GET', isArray: true },
		deleteTheme: { url: 'api/cms/:storeId/themes/:themeId', method: 'DELETE' },
		getAssets: { url: 'api/cms/:storeId/themes/:themeId/folders', method: 'GET', isArray: true },
		getAsset: { url: 'api/cms/:storeId/themes/:themeId/assets/:assetId', method: 'GET' },
		updateAsset: { url: 'api/cms/:storeId/themes/:themeId/assets', method: 'POST' },
		deleteAsset: { url: 'api/cms/:storeId/themes/:themeId/assets', method: 'DELETE' }
	});
}]);