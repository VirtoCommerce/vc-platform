angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editImageAssetController', ['$scope', 'dialogService', 'virtoCommerce.contentModule.themes', 'FileUploader', function ($scope, dialogService, themes, FileUploader) {
	var blade = $scope.blade;

	blade.initializeBlade = function () {
		if (!blade.newAsset) {
			themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
				blade.isLoading = false;

				data.content = "data:" + data.contentType + ";base64," + data.byteContent;

				blade.currentEntity = angular.copy(data);
				blade.origEntity = data;
			});

			$scope.bladeToolbarCommands = [
			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					blade.saveChanges();
				},
				canExecuteMethod: function () {
					return isDirty();
				},
				permission: 'content:manage'
			},
			{
				name: "Reset", icon: 'fa fa-undo',
				executeMethod: function () {
					angular.copy(blade.origEntity, blade.currentEntity);
				},
				canExecuteMethod: function () {
					return isDirty();
				},
				permission: 'content:manage'
			},
			{
				name: "Delete", icon: 'fa fa-trash-o',
				executeMethod: function () {
					deleteEntry();
				},
				canExecuteMethod: function () {
					return !isDirty();
				},
				permission: 'content:manage'
			}];
		}
		else {
			var data = angular.copy(blade.currentEntity);
			blade.origEntity = data;

			$scope.bladeToolbarCommands = [
			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					blade.saveChanges();
				},
				canExecuteMethod: function () {
					return isDirty() && isCanSave();
				},
				permission: 'content:manage'
			}];

			blade.isLoading = false;
		}
	}

	blade.check = function () {
	    if (!angular.isUndefined(blade.currentEntity)) {
	        if (blade.currentEntity.contentType === 'image/png' || blade.currentEntity.contentType === 'image/jpeg' || blade.currentEntity.contentType === 'image/bmp' || blade.currentEntity.contentType === 'image/gif') {
	            return true;
	        }
	    }
	    return false;
	}

	blade.getImage = function () {
		return blade.currentEntity.content;
	}

	blade.saveChanges = function() {
		blade.isLoading = true;
		blade.currentEntity.id = blade.choosenFolder + '/' + blade.currentEntity.name;

		themes.updateAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, blade.currentEntity, function () {
			blade.parentBlade.refresh(true);
			blade.choosenAssetId = blade.currentEntity.id;
			blade.title = blade.currentEntity.id;
			blade.subtitle = 'Edit asset';
			blade.newAsset = false;
			blade.initializeBlade();
		});
	};

	function isDirty() {
		return !angular.equals(blade.currentEntity, blade.origEntity);
	};

	function deleteEntry() {
		var dialog = {
			id: "confirmDelete",
			title: "Delete confirmation",
			message: "Are you sure you want to delete this asset?",
			callback: function (remove) {
				if (remove) {
					$scope.blade.isLoading = true;

					themes.deleteAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetIds: blade.choosenAssetId }, function () {
						$scope.bladeClose();
						$scope.blade.parentBlade.refresh(true);
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);
	}

	function isCanSave() {
		if (!angular.isUndefined(blade.currentEntity)) {
			if (!angular.isUndefined(blade.currentEntity.id) && !angular.isUndefined(blade.currentEntity.content)) {
				return true;
			}
			return false;
		}
		else {
			return false;
		}
	}

	if (!$scope.uploader) {
		// Creates a uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/cms/Apple/themes/' + blade.choosenStoreId + '/assets/file/assets',
			autoUpload: true,
			removeAfterUpload: true
		});

		uploader.onSuccessItem = function (fileItem, image, status, headers) {
			blade.currentEntity.content = image.content;
			blade.currentEntity.assetUrl = image.name;
			blade.currentEntity.contentType = image.contentType;

			if (blade.newAsset) {
				blade.currentEntity.name = image.name;
				blade.currentEntity.id = blade.choosenFolder + '/' + image.name;
			}
		};
	}

	blade.initializeBlade();

}]);

