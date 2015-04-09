angular.module('virtoCommerce.marketingModule')
.controller('addContentItemsController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.originalEntity = angular.copy(blade.entity);

	blade.initialize = function () {
		if (!blade.isNew) {
			$scope.bladeToolbarCommands = [
				{
					name: "Refresh", icon: 'fa fa-refresh',
					executeMethod: function () {
						blade.entity = angular.copy(blade.originalEntity);
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity);
					}
				},
				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						blade.saveChanges();
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity);
					}
				},
				{
					name: "Delete", icon: 'fa fa-trash',
					executeMethod: function () {
						blade.delete();
					},
					canExecuteMethod: function () {
						return true;
					}
				}
			];
		}
		else {
			$scope.bladeToolbarCommands = [
				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						blade.saveChanges();
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity);
					}
				}
			];
		}

		blade.isLoading = false;
	}

	blade.saveChanges = function () {
		blade.parentBlade.currentEntity.items.push(blade.entity);
		blade.originalEntity = angular.copy(blade.entity);
		blade.isNew = false;
		blade.initialize();
	}

	blade.contentTypes = [
		'CategoryWithImages',
		'CategoryUrl',
		'Flash',
		'Html',
		'Razor',
		'ImageClickable',
		'ImageNonClickable',
		'ProductWithImageAndPrice'
	];

	blade.properties = {
		CategoryWithImages: ['categoryId', 'imageUrl', 'externalImageUrl', 'message'],
		CategoryUrl: ['categoryCode', 'title', 'sortField', 'itemCount', 'newItems'],
		Flash: ['flashFilePath', 'link1Url', 'link2Url', 'link3Url'],
		Html: ['rawHtml'],
		Razor: ['razorHtml'],
		ImageClickable: ['alternativeText', 'imageUrl', 'targetUrl', 'title'],
		ImageNonClickable: ['alternativeText', 'imageUrl'],
		ProductWithImageAndPrice: ['productCode']
	}

	blade.isPropertyShows = function (propertyName) {
		var properties = blade.properties[blade.entity.contentType];

		return properties.indexOf(propertyName) !== -1;
	}

	blade.initialize();
}]);