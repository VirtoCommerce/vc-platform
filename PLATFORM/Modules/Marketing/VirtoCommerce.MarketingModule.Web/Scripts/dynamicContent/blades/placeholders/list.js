angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.placeholdersDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = 'ContentPlace';
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentPlace', respGroup: '20' }, function (data) {
			blade.currentEntities = data.contentFolders;
		});

		blade.isLoading = false;
	};

	blade.addNew = function () {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New placeholders element',
			subtitle: 'Add new placeholders element',
			choosenFolder: blade.choosenFolder,
			controller: 'virtoCommerce.marketingModule.addPlaceholderElementController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewFolder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New placeholders folder',
			subtitle: 'Add new placeholders folder',
			entity: data,
			isNew: true,
			controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.editFolder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'Edit placeholders folder',
			subtitle: 'Edit placeholders folder',
			entity: data,
			isNew: false,
			controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewPlaceholder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New placeholders element',
			subtitle: 'Add new placeholders element',
			entity: data,
			isNew: true,
			controller: 'virtoCommerce.marketingModule.addPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.editPlaceholder = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'Edit placeholders element',
			subtitle: 'Edit placeholders element',
			entity: data,
			isNew: false,
			controller: 'virtoCommerce.marketingModule.addPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.closeChildrenBlades = function() {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	blade.folderClick = function (placeholderFolder) {
		blade.closeChildrenBlades();

		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, placeholderFolder.id)) {
			blade.choosenFolder = placeholderFolder.id;
			blade.currentEntity = placeholderFolder;
			marketing_dynamicContents_res_search.search({ folder: placeholderFolder.id, respGroup: '20' }, function (data) {
				placeholderFolder.childrenFolders = data.contentFolders;
				placeholderFolder.placeholders = data.contentPlaces;
			});
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
		if (blade.choosenFolder === 'ContentPlace') {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
				blade.currentEntities = data.contentFolders;
			});
		}
		else {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.placeholders = data.contentPlaces;
			});
		}
	};

	blade.clickDefault = function () {
		blade.choosenFolder = 'ContentPlace';
		blade.currentEntity = undefined;
	}

	blade.deleteFolder = function (data) {
		marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
			if (data.id === blade.choosenFolder) {
				blade.choosenFolder = data.parentFolderId;
				var coll = blade.currentEntities;
				var newColl = [];
				var ent = undefined;
				while (coll.length > 0) {
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

			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
				blade.currentEntity.palceholders = data.contentPlaces;
			});
		});
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

	$scope.blade.headIcon = 'fa fa-location-arrow';

	blade.initialize();
}]);
