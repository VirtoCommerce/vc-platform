angular.module('virtoCommerce.content.themeModule.blades.editImageAsset', [
	'virtoCommerce.content.themeModule.resources.themes',
	'angularFileUpload',
]).controller('editImageAssetController', ['$scope', 'dialogService', 'themes', 'FileUploader', function ($scope, dialogService, themes, FileUploader) {
	var blade = $scope.blade;


	blade.initializeBlade = function() {
		themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
			blade.isLoading = false;
			blade.currentEntity = angular.copy(data);
			blade.origEntity = data;
		});
	}

	blade.check = function () {
		if (blade.currentEntity.contentType === 'image/png' || blade.currentEntity.contentType === 'image/jpeg' || blade.currentEntity.contentType === 'image/bmp' || blade.currentEntity.contentType === 'image/gif') {
			return true;
		}
		return false;
	}

	if (!$scope.uploader) {
		// Creates a uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/cms/Apple/themes/Simple/assets/image',
			autoUpload: true,
			removeAfterUpload: true
		});

		uploader.onSuccessItem = function () {
			blade.initializeBlade();
		};
	}

	blade.initializeBlade();

}]);

