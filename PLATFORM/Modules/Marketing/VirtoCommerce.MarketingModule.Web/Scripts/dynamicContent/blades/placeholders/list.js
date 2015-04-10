angular.module('virtoCommerce.marketingModule')
.controller('placeholdersDynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	blade.initialize = function() {
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

	blade.folderClick = function (data) {
		if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, data.id)) {
			blade.choosenFolder = data.id;
			blade.currentEntity = data;
		}
		else {
			blade.choosenFolder = data.parentId;
			blade.currentEntity = undefined;
		}
	}

	blade.checkFolder = function (data) {
		var retVal = angular.equals(data.id, blade.choosenFolder);
		var childFolders = data.childrenFolders;
		var nextLevelChildFolders = [];
		while (childFolders.length > 0 && !retVal) {
			if (!angular.isUndefined(_.find(childFolders, function (folder) { return angular.equals(folder.id, blade.choosenFolder); }))) {
				retVal = true;
			}
			else {
				for (var i = 0; i < childFolders.length; i++) {
					if (childFolders[i].childrenFolders.length > 0) {
						nextLevelChildFolders = _.union(nextLevelChildFolders, childFolders[i].childrenFolders);
					}
				}
				childFolders = nextLevelChildFolders;
				nextLevelChildFolders = [];
			}
		}

		return retVal;
	}

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

	blade.testData = function () {
		blade.currentEntities.push(
			{
				id: 'Main',
				name: 'Main',
				description: 'Main',
				childrenFolders: [
					{
						id: 'Simple',
						name: 'Simple',
						description: 'Simple',
						childrenFolders: [
							{
								id: 'Footer',
								name: 'Footer',
								description: 'Footer',
								childrenFolders: [],
								placeholders: [],
								parentId: 'Simple',
							}
						],
						placeholders: [],
						parentId: 'Main',
					},
					{
						id: 'Tinker',
						name: 'Tinker',
						description: 'Tinker',
						childrenFolders: [
							{
								id: 'Footer1',
								name: 'Footer1',
								description: 'Footer1',
								childrenFolders: [],
								placeholders: [],
								parentId: 'Tinker',
							}
						],
						placeholders: [],
						parentId: 'Main',
					},
				],
				placeholders: [
					{ id: 'Main-Default-Slider', name: 'Main-Default-Slider', description: 'Main-Default-Slider', descriptionImageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', parentId: blade.choosenFolder }
				],
				parentId: undefined
			});
	}

	blade.testData();
	blade.initialize();
}]);
