angular.module('virtoCommerce.marketingModule')
.controller('addContentItemsController', ['$scope', 'marketing_dynamicContents_res_contentItems', 'bladeNavigationService', function ($scope, marketing_dynamicContents_res_contentItems, bladeNavigationService) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

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
						return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
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

			blade.unpackingContentItem();
		}

		blade.isLoading = false;
	}

	blade.saveChanges = function () {
		blade.createContentItem();
		if (blade.isNew) {
			marketing_dynamicContents_res_contentItems.save({}, blade.entity, function (data) {
				blade.parentBlade.updateChoosen();
				bladeNavigationService.closeBlade(blade);
			});
		}
		else {
			marketing_dynamicContents_res_contentItems.update({}, blade.entity, function (data) {
				blade.originalEntity = angular.copy(blade.entity);
			});
		}
	}

	blade.unpackingContentItem = function () {
		for (var i = 0; i < blade.entity.properties.length; i++) {
			blade.entity[blade.entity.properties[i].name] = blade.entity.properties[i].value;
		}
	}

	blade.createContentItem = function () {
		var properties = blade.properties[blade.entity.contentType];

		blade.entity.properties = [];

		for (var i = 0; i < properties.length; i++){
			blade.entity.properties.push({ name: properties[i].name, value: blade.entity[properties[i].name], valueType: properties[i].valueType });
		}
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
		CategoryWithImages: [
			{ name: 'categoryId', valueType: 'ShortText' },
			{ name: 'imageUrl', valueType: 'ShortText' },
			{ name: 'externalImageUrl', valueType: 'ShortText' },
			{ name: 'message', valueType: 'LongText' }
		],
		CategoryUrl: [
			{ name: 'categoryCode', valueType: 'ShortText' },
			{ name: 'title', valueType: 'ShortText' },
			{ name: 'sortField', valueType: 'ShortText' },
			{ name: 'itemCount', valueType: 'Integer' },
			{ name: 'newItems', valueType: 'Boolean' }
		],
		Flash: [
			{ name: 'flashFilePath', valueType: 'ShortText' },
			{ name: 'link1Url', valueType: 'ShortText' },
			{ name: 'link2Url', valueType: 'ShortText' },
			{ name: 'link3Url', valueType: 'ShortText' }
		],
		Html: [
			{ name: 'rawHtml', valueType: 'LongText' }
		],
		Razor: [
			{ name: 'razorHtml', valueType: 'LongText' }
		],
		ImageClickable: [
			{ name: 'alternativeText', valueType: 'LongText' },
			{ name: 'imageUrl', valueType: 'ShortText' },
			{ name: 'targetUrl', valueType: 'ShortText' },
			{ name: 'title', valueType: 'ShortText' }
		],
		ImageNonClickable: [
			{ name: 'alternativeText', valueType: 'LongText' },
			{ name: 'imageUrl', valueType: 'ShortText' }
		],
		ProductWithImageAndPrice: [
			{ name: 'productCode', valueType: 'ShortText' },
		]
	}

	blade.isPropertyShows = function (propertyName) {
		var properties = blade.properties[blade.entity.contentType];

		var retVal = false;

		for (var i = 0; i < properties.length; i++) {
			if (properties[i].name === propertyName)
				retVal = true;
		}

		return retVal;
	}

	blade.initialize();
}]);