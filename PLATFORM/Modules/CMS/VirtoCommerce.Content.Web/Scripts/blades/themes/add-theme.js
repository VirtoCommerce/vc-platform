﻿angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.addThemeController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.themes', 'FileUploader', 'platformWebApp.bladeNavigationService', function ($scope, dialogService, themes, FileUploader, bladeNavigationService) {
	var blade = $scope.blade;
	blade.themeLoaded = false;

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	blade.initialize = function () {
		blade.isLoading = false;
	}

	if (!$scope.uploader) {
		// create the uploader
		var uploader = $scope.uploader = new FileUploader({
			scope: $scope,
			headers: { Accept: 'application/json' },
			url: 'api/platform/assets?folderUrl=tmp',
			autoUpload: true,
			removeAfterUpload: true
		});

		// ADDING FILTERS
		// Zips only
		//uploader.filters.push({
		//	name: 'zipFilter',
		//	fn: function (i /*{File|FileLikeObject}*/, options) {
		//		var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
		//		return '|zip|'.indexOf(type) !== -1;
		//	}
		//});
        
		uploader.onSuccessItem = function (fileItem, files, status, headers) {
			blade.themeFileUrl = files[0].url;
			blade.themeLoaded = true;
		};

		uploader.onAfterAddingAll = function (addedItems) {
		    bladeNavigationService.setError(null, blade);
		};

		uploader.onErrorItem = function (item, response, status, headers) {
		    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
		};
	}

	blade.save = function () {
		blade.isLoading = true;
		if (!$scope.formScope.$invalid) {
			themes.createTheme({ storeId: blade.chosenStoreId, themeName: blade.name, themeFileUrl: blade.themeFileUrl }, function (data) {
				blade.parentBlade.initialize();
				blade.parentBlade.parentBlade.refresh(blade.chosenStoreId, 'themes');
				blade.isLoading = false;
				bladeNavigationService.closeBlade(blade);
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
	}

	blade.initialize();
}]);