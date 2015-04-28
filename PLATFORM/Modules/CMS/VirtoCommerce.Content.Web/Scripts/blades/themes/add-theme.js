angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.addThemeController', ['$scope', 'dialogService', 'virtoCommerce.contentModule.themes', 'FileUploader', function ($scope, dialogService, themes, FileUploader) {
	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = false;
	}

	if (!$scope.uploader) {
		// Creates a uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/cms/' + blade.choosenStoreId + '/themes/file',
			autoUpload: true,
			removeAfterUpload: true
		});

		uploader.onBeforeUploadItem = function (item) {
			blade.isLoading = true;
		}

		uploader.onSuccessItem = function (fileItem, image, status, headers) {
			$scope.blade.parentBlade.refresh(true);
			$scope.bladeClose();
			blade.isLoading = false;
		};
	}

	blade.refresh();
}]);