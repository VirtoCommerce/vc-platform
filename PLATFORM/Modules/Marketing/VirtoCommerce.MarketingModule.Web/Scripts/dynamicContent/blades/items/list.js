angular.module('virtoCommerce.marketingModule')
.controller('itemsDynamicContentListController', ['$scope', 'marketing_dynamicContents_res_search', 'bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = 'ContentItem';
	blade.currentEntity = undefined;

	$scope.selectedNodeId = null;

	blade.initializeBlade = function () {
		marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithFolders' }, function (data) {
			blade.currentEntities = data.contentFolders;
		});

		blade.isLoading = false;
	};

	blade.addNew = function () {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New content items element',
			subtitle: 'Add new content items element',
			choosenFolder: blade.choosenFolder,
			controller: 'addContentItemsElementController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/add.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewFolder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New content items folder element',
			subtitle: 'Add new content items folder element',
			entity: data,
			isNew: true,
			controller: 'addFolderContentItemsController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.editFolder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'Edit content items folder element',
			subtitle: 'Edit content items folder element',
			entity: data,
			isNew: false,
			controller: 'addFolderPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewContentItem = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New content item element',
			subtitle: 'Add new content item element',
			entity: data,
			isNew: true,
			controller: 'addContentItemsController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.editContentItem = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'Edit content item element',
			subtitle: 'Edit content item element',
			entity: data,
			isNew: false,
			controller: 'addContentItemsController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.closeChildrenBlades = function () {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
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

	blade.updateChoosen = function () {
		if (blade.choosenFolder === 'ContentItem') {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithFolders' }, function (data) {
				blade.currentEntities = data.contentFolders;
			});
		}
		else {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithFolders' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
			});

			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithContentItems' }, function (data) {
				blade.currentEntity.items = data.contentItems;
			});
		}
	};

	$scope.bladeToolbarCommands = [
		{
			name: "Refresh", icon: 'fa fa-refresh',
			executeMethod: function () {
				$scope.blade.refresh();
			},
			canExecuteMethod: function () {
				return true;
			}
		},
		{
			name: "Add", icon: 'fa fa-plus',
			executeMethod: function () {
				blade.addNew();
			},
			canExecuteMethod: function () {
				return true;
			}
		},
		{
			name: "Edit folder", icon: 'fa fa-pencil-square-o',
			executeMethod: function () {
				blade.editFolder(blade.currentEntity);
			},
			canExecuteMethod: function () {
				return !angular.isUndefined(blade.currentEntity);
			}
		}
	];

	$scope.bladeHeadIco = 'fa fa-flag';

	blade.initializeBlade();
}]);
