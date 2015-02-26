angular.module('virtoCommerce.content.themeModule.blades.editImageAsset', [
	'virtoCommerce.content.themeModule.resources.themes'
]).controller('editImageAssetController', ['$scope', 'dialogService', 'themes', function ($scope, dialogService, themes) {
	var blade = $scope.blade;

	if (!$scope.uploader) {
		// Creates a uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/catalog/assets',
			autoUpload: true,
			removeAfterUpload: true
		});

		// ADDING FILTERS
		// Images only
		uploader.filters.push({
			name: 'imageFilter',
			fn: function (i /*{File|FileLikeObject}*/, options) {
				var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
				return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
			}
		});


		uploader.onSuccessItem = function (fileItem, images, status, headers) {
			angular.forEach(images, function (image) {
				image.itemId = $scope.item.id;
				//ADD uploaded image to the item
				$scope.item.images.push(image);
			});
		};
	}

}]);

