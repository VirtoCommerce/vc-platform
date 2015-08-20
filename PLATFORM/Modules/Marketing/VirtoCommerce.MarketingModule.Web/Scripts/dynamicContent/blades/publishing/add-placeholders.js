angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingPlaceholdersStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentPlaces, bladeNavigationService) {
	var blade = $scope.blade;

	blade.choosenFolder = 'ContentPlace';
	blade.currentEntity = {};

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
			blade.currentEntity.childrenFolders = data.contentFolders;
			blade.currentEntity.placeholders = data.contentPlaces;
			blade.setBreadcrumbs();
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
				blade.breadcrumbs.push(
					{
						id: placeholderFolder.id,
						name: placeholderFolder.name,
						blade: blade,
						navigate: function (breadcrumb) {
							blade.choosenFolder = breadcrumb.id;
							marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
								blade.currentEntity.childrenFolders = data.contentFolders;
								blade.currentEntity.placeholders = data.contentPlaces;
								blade.setBreadcrumbs();
							}, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
						}
					});
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
		else {
			blade.choosenFolder = placeholderFolder.parentFolderId;
			blade.currentEntity = undefined;
		}
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

	blade.setBreadcrumbs = function () {
		if (blade.breadcrumbs === undefined) {
			blade.breadcrumbs = [];
		}

		var index = _.findLastIndex(blade.breadcrumbs, { id: blade.choosenFolder });

		if (index !== -1)
			blade.breadcrumbs = blade.breadcrumbs.slice(0, index + 1);

		//catalog breadcrumb by default
		var breadCrumb = {
			id: 'ContentPlace',
			name: 'Placeholders',
			blade: blade
		};

		//prevent duplicate items
		if (!_.some(blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
			blade.breadcrumbs.push(breadCrumb);
		}

		breadCrumb.navigate = function (breadcrumb) {
			blade.choosenFolder = breadcrumb.id;
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.placeholders = data.contentPlaces;
				blade.setBreadcrumbs();
			},
			function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		};
	}

	$scope.blade.headIcon = 'fa-paperclip';

	blade.initialize();
}]);