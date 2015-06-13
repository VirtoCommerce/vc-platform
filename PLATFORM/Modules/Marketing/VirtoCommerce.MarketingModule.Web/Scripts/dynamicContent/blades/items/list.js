angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.itemsDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = 'ContentItem';
	blade.currentEntity = undefined;

	$scope.selectedNodeId = null;

	blade.initializeBlade = function () {
		marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
			blade.currentEntities = data.contentFolders;
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

		blade.isLoading = false;
	};

	blade.addNew = function () {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New content items element',
			subtitle: 'Add new content items element',
			choosenFolder: blade.choosenFolder,
			controller: 'virtoCommerce.marketingModule.addContentItemsElementController',
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
			controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
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
			controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
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
			controller: 'virtoCommerce.marketingModule.addContentItemsController',
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
			controller: 'virtoCommerce.marketingModule.addContentItemsController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.closeChildrenBlades = function () {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	blade.folderClick = function (contentItemFolder) {
		blade.closeChildrenBlades();

		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, contentItemFolder.id)) {
			blade.choosenFolder = contentItemFolder.id;
			blade.currentEntity = contentItemFolder;
			marketing_dynamicContents_res_search.search({ folder: contentItemFolder.id, respGroup: '18' }, function (data) {
				contentItemFolder.childrenFolders = data.contentFolders;
				contentItemFolder.items = data.contentItems;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
				blade.currentEntities = data.contentFolders;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
		else {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.items = data.contentItems;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		}
	};

	blade.clickDefault = function () {
		blade.choosenFolder = 'ContentItem';
		blade.currentEntity = undefined;
	}

	blade.deleteFolder = function (data) {
		marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
			if (data.id === blade.choosenFolder) {
				blade.choosenFolder = data.parentFolderId;
				var coll = blade.currentEntities;
				var newColl = [];
				var ent = undefined;
				while(coll.length > 0) {
					angular.forEach(coll, function (folder) {
						if (folder.id === blade.choosenFolder) {
							ent = folder;
						}
						angular.forEach(folder.childrenFolders, function (folder) {
							newColl.push(folder)
						})
					});
					if (ent !== undefined) {
						coll = [];
					}
					else {
						coll = newColl;
					}
				}

				blade.currentEntity = ent;
			}

			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '18' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.items = data.contentItems;
			},
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
		},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
	}

	$scope.blade.toolbarCommands = [
		{
			name: "Add", icon: 'fa fa-plus',
			executeMethod: function () {
				blade.addNew();
			},
			canExecuteMethod: function () {
				return true;
			},
			permission: 'marketing:manage'
		},
		{
			name: "Edit folder", icon: 'fa fa-pencil-square-o',
			executeMethod: function () {
				blade.editFolder(blade.currentEntity);
			},
			canExecuteMethod: function () {
				return !angular.isUndefined(blade.currentEntity);
			},
			permission: 'marketing:manage'
		}
	];

	$scope.blade.headIcon = 'fa fa-inbox';

	blade.initializeBlade();
}]);
