angular.module('virtoCommerce.marketingModule')
.controller('addPublishingPlaceholdersStepController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		blade.isLoading = false;
	}

	blade.addPlaceholder = function (placeholder) {
		blade.entity.contentPlaces.push(placeholder);
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

	blade.checkPlaceholder = function (data) {
		return _.filter(blade.entity.contentPlaces, function (ci) { return angular.equals(ci, data); }).length == 0;
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
								placeholders: [],
								parentId: 'Simple',
							}
						],
						placeholders: [],
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
								placeholders: [],
								parentId: 'Tinker',
							}
						],
						placeholders: [],
						parentId: 'Main',
					},
				],
				placeholders: [
					{ id: 'Main-Default-Slider', name: 'Main-Default-Slider', description: 'Main-Default-Slider', descriptionImageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', parentId: blade.choosenFolder }
				],
				parentId: undefined
			});
	}

	blade.testData();
	blade.initialize();
}]);