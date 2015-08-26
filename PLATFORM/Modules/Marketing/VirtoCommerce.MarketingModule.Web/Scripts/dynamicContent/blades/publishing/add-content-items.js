angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingContentItemsStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentItems, bladeNavigationService) {
	
	var blade = $scope.blade;

	blade.choosenFolder = 'ContentItem';
	blade.currentEntity = {};

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
			blade.currentEntity.childrenFolders = data.contentFolders;
			blade.currentEntity.items = data.contentItems;
			blade.setBreadcrumbs();
			blade.isLoading = false;
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

		blade.entity.contentItems.forEach(function (el) {
		    marketing_dynamicContents_res_contentItems.get({ id: el.id }, function(data) {
		            el.path = data.path;
		        },
		        function(error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		});
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
				blade.breadcrumbs.push(
					{
						id: contentItem.id,
						name: contentItem.name,
						blade: blade,
						navigate: function (breadcrumb) {
							blade.choosenFolder = breadcrumb.id;
							marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
								blade.currentEntity.childrenFolders = data.contentFolders;
								blade.currentEntity.items = data.contentItems;
								blade.setBreadcrumbs();
							}, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
						}
					});
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
	}

	$scope.blade.headIcon = 'fa-paperclip';

	blade.setBreadcrumbs = function() {
		if (blade.breadcrumbs === undefined) {
			blade.breadcrumbs = [];
		}

		var index = _.findLastIndex(blade.breadcrumbs, { id: blade.choosenFolder });

		if (index !== -1)
			blade.breadcrumbs = blade.breadcrumbs.slice(0, index + 1);

		//catalog breadcrumb by default
		var breadCrumb = {
			id: 'ContentItem',
			name: 'Items',
			blade: blade
		};

		//prevent duplicate items
		if (!_.some(blade.breadcrumbs, function (x) { return x.id == breadCrumb.id })) {
			blade.breadcrumbs.push(breadCrumb);
		}

		breadCrumb.navigate = function (breadcrumb) {
			blade.choosenFolder = breadcrumb.id;
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.items = data.contentItems;
				blade.setBreadcrumbs();
			},
			function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		};
	}

	blade.initialize();
}]);