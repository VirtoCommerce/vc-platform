angular.module('virtoCommerce.marketingModule')
.controller('placeholdersDynamicContentListController', ['$scope', 'marketing_dynamicContents_res_search', 'bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = 'ContentPlace';
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	blade.initialize = function () {
		marketing_dynamicContents_res_search.search({ folder: 'ContentPlace', respGroup: 'WithFolders' }, function (data) {
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
			controller: 'addPlaceholderElementController',
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
			controller: 'addFolderPlaceholderController',
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
			controller: 'addFolderPlaceholderController',
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
			controller: 'addPlaceholderController',
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
			controller: 'addPlaceholderController',
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

	blade.updateChoosen = function () {
		if (blade.choosenFolder === 'ContentPlace') {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithFolders' }, function (data) {
				blade.currentEntities = data.contentFolders;
			});
		}
		else {
			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithFolders' }, function (data) {
				blade.currentEntity.childrenFolders = data.contentFolders;
			});

			marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: 'WithContentPlaceholders' }, function (data) {
				blade.currentEntity.placeholders = data.contentPlaces;
			});
		}
	};

	blade.deleteLink = function (data) {

	}

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

	blade.initialize();
}]);
