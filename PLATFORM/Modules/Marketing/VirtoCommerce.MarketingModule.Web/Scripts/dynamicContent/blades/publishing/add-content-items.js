angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingContentItemsStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentItems, bladeNavigationService) {
	
	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentItem', respGroup: '18' }, function (data) {
			blade.currentEntities = data.contentFolders;
		});

		blade.entity.contentItems.forEach(function (el) {
			marketing_dynamicContents_res_contentItems.get({ id: el.id }, function (data) {
				el.path = data.path;
			})
		});

		blade.isLoading = false;
	}

	blade.addContentItem = function (contentItem) {
		blade.entity.contentItems.push(contentItem);
	}

	blade.deleteAllContentItems = function () {
		blade.entity.contentItems = [];
	}

	blade.deleteContentItem = function (data) {
		blade.entity.contentItems = _.filter(blade.entity.contentItems, function (place) { return !angular.equals(data.id, place.id); });;
	}

	blade.checkContentItem = function (data) {
		return _.filter(blade.entity.contentItems, function (ci) { return angular.equals(ci.id, data.id); }).length == 0;
	}

	blade.folderClick = function (contentItem) {
		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, contentItem.id)) {
			blade.choosenFolder = contentItem.id;
			blade.currentEntity = contentItem;
			marketing_dynamicContents_res_search.search({ folder: contentItem.id, respGroup: '18' }, function (data) {
				contentItem.childrenFolders = data.contentFolders;
				contentItem.items = data.contentItems;
			});
		}
	}

	blade.clickDefault = function () {
		blade.choosenFolder = 'ContentItem';
		blade.currentEntity = undefined;
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

	$scope.bladeHeadIco = 'fa fa-paperclip';

	blade.initialize();
}]);