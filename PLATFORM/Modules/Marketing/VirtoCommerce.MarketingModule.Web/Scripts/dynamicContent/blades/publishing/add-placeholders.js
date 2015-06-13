angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingPlaceholdersStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentPlaces, bladeNavigationService) {
	var blade = $scope.blade;

	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentPlace', respGroup: '20' }, function (data) {
			blade.currentEntities = data.contentFolders;
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

		blade.entity.contentPlaces.forEach(function(el) {
		    marketing_dynamicContents_res_contentPlaces.get({ id: el.id }, function(data) {
		            el.path = data.path;
		        },
		        function(error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
			marketing_dynamicContents_res_search.search({ folder: placeholderFolder.id, respGroup: '20' }, function (data) {
				placeholderFolder.childrenFolders = data.contentFolders;
				placeholderFolder.placeholders = data.contentPlaces;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
		return _.filter(blade.entity.contentPlaces, function (ci) { return angular.equals(ci.id, data.id); }).length == 0;
	}

	blade.clickDefault = function () {
		blade.choosenFolder = 'ContentItem';
		blade.currentEntity = undefined;
	}

	$scope.blade.headIcon = 'fa fa-paperclip';

	blade.initialize();
}]);