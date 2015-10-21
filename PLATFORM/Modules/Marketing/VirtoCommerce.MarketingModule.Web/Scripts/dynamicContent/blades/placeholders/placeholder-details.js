angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPlaceholderController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces', 'platformWebApp.bladeNavigationService', 'FileUploader', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_contentPlaces, bladeNavigationService, FileUploader, dialogService) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	var blade = $scope.blade;
	blade.originalEntity = angular.copy(blade.entity);

	blade.initialize = function () {
		if (!$scope.uploader) {
			// create the uploader
			var uploader = $scope.uploader = new FileUploader({
				scope: $scope,
				headers: { Accept: 'application/json' },
				url: 'api/platform/assets?folderUrl=placeholders-images',
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
				blade.entity.imageUrl = images[0].url;
			};
		}

		if (!blade.isNew) {
			$scope.blade.toolbarCommands = [
				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						blade.saveChanges();
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
					},
					permission: 'marketing:update'
				},
                {
				    name: "Reset", icon: 'fa fa-undo',
				    executeMethod: function () {
				        blade.entity = angular.copy(blade.originalEntity);
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity);
				    },
				    permission: 'marketing:update'
				},
				{
					name: "Delete", icon: 'fa fa-trash',
					executeMethod: function () {
						var dialog = {
							id: "confirmDeleteContentPlaceholder",
							title: "Delete confirmation",
							message: "Are you sure want to delete content placeholder?",
							callback: function (remove) {
								if (remove) {
									blade.delete();
								}
							}
						};

						dialogService.showConfirmationDialog(dialog);
					},
					canExecuteMethod: function () {
						return true;
					},
					permission: 'marketing:update'
				}
			];
		}

		blade.isLoading = false;
	}

	blade.delete = function () {
		blade.isLoading = true;
		marketing_dynamicContents_res_contentPlaces.delete({ ids: [blade.entity.id] }, function () {
			blade.parentBlade.initialize();
			bladeNavigationService.closeBlade(blade);
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
	}

	blade.saveChanges = function () {
		blade.isLoading = true;

		if (blade.isNew) {
			marketing_dynamicContents_res_contentPlaces.save({}, blade.entity, function (data) {
				blade.parentBlade.initialize();
				bladeNavigationService.closeBlade(blade);
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
		}
		else {
			marketing_dynamicContents_res_contentPlaces.update({}, blade.entity, function (data) {
				blade.parentBlade.initialize();
				blade.originalEntity = angular.copy(blade.entity);
				blade.isLoading = false;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
		}
	}

	$scope.blade.headIcon = 'fa-location-arrow';

	blade.initialize();
}]);