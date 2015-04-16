angular.module('virtoCommerce.marketingModule')
.controller('addPublishingContentItemsStepController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentItem', respGroup: 'WithFolders' }, function (data) {
			blade.currentEntities = data.contentFolders;
		});

		blade.isLoading = false;
	}

	blade.addContentItem = function (contentItem) {
		blade.entity.contentItems.push(contentItem);
	}

	blade.deleteAllPlaceholder = function () {
		blade.entity.contentItems = [];
	}

	blade.deletePlaceholder = function (data) {
		blade.entity.contentItems = _.filter(blade.entity.contentItems, function (place) { return !angular.equals(data.id, place.id); });;
	}

	blade.checkContentItem = function (data) {
		return _.filter(blade.entity.contentItems, function (ci) { return angular.equals(ci, data); }).length == 0;
	}

	blade.folderClick = function (contentItem) {
		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, contentItem.id)) {
			blade.choosenFolder = contentItem.id;
			blade.currentEntity = contentItem;
			marketing_dynamicContents_res_search.search({ folder: contentItem.id, respGroup: 'WithFolders' }, function (data) {
				contentItem.childrenFolders = data.contentFolders;
			});

			marketing_dynamicContents_res_search.search({ folder: contentItem.id, respGroup: 'WithContentItems' }, function (data) {
				contentItem.items = data.contentItems;
			});
		}
		else {
			blade.choosenFolder = contentItem.parentFolderId;
			blade.currentEntity = undefined;
		}
	}

	blade.checkFolder = function (data) {
		var retVal = angular.equals(data.id, blade.choosenFolder);
		if (data.childrenFolders) {
			var childFolders = data.childrenFolders;
			var nextLevelChildFolders = [];
			while (childFolders.length > 0 && !retVal) {
				if (!angular.isUndefined(_.find(childFolders, function (folder) { return angular.equals(folder.id, blade.choosenFolder); }))) {
					retVal = true;
				}
				else {
					for (var i = 0; i < childFolders.length; i++) {
						if (childFolders[i].childrenFolders) {
							if (childFolders[i].childrenFolders.length > 0) {
								nextLevelChildFolders = _.union(nextLevelChildFolders, childFolders[i].childrenFolders);
							}
						}
					}
					childFolders = nextLevelChildFolders;
					nextLevelChildFolders = [];
				}
			}
		}

		return retVal;
	}

	blade.initialize();
}]);