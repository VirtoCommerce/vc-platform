angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPlaceholderController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, marketing_dynamicContents_res_contentPlaces, bladeNavigationService, FileUploader) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	var blade = $scope.blade;
	blade.originalEntity = angular.copy(blade.entity);

	blade.initialize = function () {
		if (!$scope.uploader) {
			// Creates a uploader
			var uploader = $scope.uploader = new FileUploader({
				scope: $scope,
				headers: { Accept: 'application/json' },
				url: 'api/assets/',
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
					permission: 'marketing:manage'
				},
                {
				    name: "Reset", icon: 'fa fa-undo',
				    executeMethod: function () {
				        blade.entity = angular.copy(blade.originalEntity);
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity);
				    },
				    permission: 'marketing:manage'
				},
				{
					name: "Delete", icon: 'fa fa-trash',
					executeMethod: function () {
						blade.delete();
					},
					canExecuteMethod: function () {
						return true;
					},
					permission: 'marketing:manage'
				}
			];
		}

		blade.isLoading = false;
	}

	blade.delete = function () {
		marketing_dynamicContents_res_contentPlaces.delete({ ids: [blade.entity.id] }, function () {
			blade.parentBlade.updateChoosen();
			bladeNavigationService.closeBlade(blade);
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
	}

	blade.saveChanges = function () {
		if (blade.isNew) {
			marketing_dynamicContents_res_contentPlaces.save({}, blade.entity, function (data) {
				blade.parentBlade.updateChoosen();
				bladeNavigationService.closeBlade(blade);
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
		else {
			marketing_dynamicContents_res_contentPlaces.update({}, blade.entity, function (data) {
				blade.parentBlade.updateChoosen();
				blade.originalEntity = angular.copy(blade.entity);
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
	}

	$scope.blade.headIcon = 'fa fa-location-arrow';

	blade.initialize();
}]);