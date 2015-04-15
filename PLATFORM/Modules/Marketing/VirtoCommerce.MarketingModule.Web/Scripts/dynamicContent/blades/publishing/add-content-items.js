angular.module('virtoCommerce.marketingModule')
.controller('addPublishingContentItemsStepController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		blade.isLoading = false;
	}

	blade.addContentItem = function (contentItem) {
		blade.entity.contentItems.push(contentItem);
	}

	blade.checkContentItem = function (data) {
		return _.filter(blade.entity.contentItems, function (ci) { return angular.equals(ci, data); }).length == 0;
	}

	blade.folderClick = function (data) {
		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, data.id)) {
			blade.choosenFolder = data.id;
			blade.currentEntity = data;
		}
		else {
			blade.choosenFolder = data.parentId;
			blade.currentEntity = undefined;
		}
	}

	blade.checkFolder = function (data) {
		var retVal = angular.equals(data.id, blade.choosenFolder);
		var childFolders = data.childrenFolders;
		var nextLevelChildFolders = [];
		while (childFolders.length > 0 && !retVal) {
			if (!angular.isUndefined(_.find(childFolders, function (folder) { return angular.equals(folder.id, blade.choosenFolder); }))) {
				retVal = true;
			}
			else {
				for (var i = 0; i < childFolders.length; i++) {
					if (childFolders[i].childrenFolders.length > 0) {
						nextLevelChildFolders = _.union(nextLevelChildFolders, childFolders[i].childrenFolders);
					}
				}
				childFolders = nextLevelChildFolders;
				nextLevelChildFolders = [];
			}
		}

		return retVal;
	}

	blade.testData = function () {
		blade.currentEntities.push(
			{
				id: 'Main',
				name: 'Main',
				description: 'Main',
				childrenFolders: [
					{
						id: 'Simple',
						name: 'Simple',
						description: 'Simple',
						childrenFolders: [
							{
								id: 'Footer',
								name: 'Footer',
								description: 'Footer',
								childrenFolders: [],
								items: [],
								parentId: 'Simple',
							}
						],
						items: [],
						parentId: 'Main',
					},
					{
						id: 'Tinker',
						name: 'Tinker',
						description: 'Tinker',
						childrenFolders: [
							{
								id: 'Footer1',
								name: 'Footer1',
								description: 'Footer1',
								childrenFolders: [],
								items: [],
								parentId: 'Tinker',
							}
						],
						items: [],
						parentId: 'Main',
					},
				],
				items: [
					{ id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: 'Slider', description: 'Slider', contentType: 'CategoryWithImages', categoryId: 'Slider', imageUrl: 'Slider', externalImageUrl: 'Slider', message: 'Slider', categoryCode: '', title: '', sortField: '', itemCount: 1, newItems: false, flashFilePath: '', link1Url: '', link2Url: '', link3Url: '', rawHtml: '', razorHtml: '', alternativeText: '', targetUrl: '', productCode: '', parentId: 'Main' }
				],
				parentId: undefined
			});
	}

	blade.testData();
	blade.initialize();
}]);