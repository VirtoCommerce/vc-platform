angular.module('virtoCommerce.marketingModule')
.controller('addPublishingPlaceholdersStepController', ['$scope', 'marketing_dynamicContents_res_search', '', 'bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, bladeNavigationService) {
	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentPlace', respGroup: 'WithFolders' }, function (data) {
			blade.currentEntities = data.contentFolders;
		});

		blade.isLoading = false;
	}

	blade.addPlaceholder = function (placeholder) {


		blade.entity.contentPlaces.push(placeholder);
	}

	blade.folderClick = function (placeholderFolder) {
		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, placeholderFolder.id)) {
			blade.choosenFolder = placeholderFolder.id;
			blade.currentEntity = placeholderFolder;
			marketing_dynamicContents_res_search.search({ folder: placeholderFolder.id, respGroup: 'WithFolders' }, function (data) {
				placeholderFolder.childrenFolders = data.contentFolders;
			});

			marketing_dynamicContents_res_search.search({ folder: placeholderFolder.id, respGroup: 'WithContentPlaces' }, function (data) {
				placeholderFolder.placeholders = data.contentPlaces;
			});
		}
		else {
			blade.choosenFolder = placeholderFolder.parentFolderId;
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

	blade.deleteAllPlaceholder = function () {
		blade.entity.contentPlaces = [];
	}

	blade.deletePlaceholder = function (data) {
		blade.entity.contentPlaces = _.filter(blade.entity.contentPlaces, function (place) { return !angular.equals(data.id, place.id); });;
	}

	blade.checkPlaceholder = function (data) {
		return _.filter(blade.entity.contentPlaces, function (ci) { return angular.equals(ci, data); }).length == 0;
	}

	blade.initialize();
}]);