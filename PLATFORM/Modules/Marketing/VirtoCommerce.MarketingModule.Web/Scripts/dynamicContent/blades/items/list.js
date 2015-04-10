angular.module('virtoCommerce.marketingModule')
.controller('itemsDynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	blade.initializeBlade = function() {
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
						items: [],
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
						items: [],
						parentId: 'Main',
					},
				],
				items: [
					{ id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: 'Slider', description: 'Slider', contentType: 'CategoryWithImages', categoryId: 'Slider', imageUrl: 'Slider', externalImageUrl: 'Slider', message: 'Slider', categoryCode: '', title: '', sortField: '', itemCount: 1, newItems: false, flashFilePath: '', link1Url: '', link2Url: '', link3Url: '', rawHtml: '', razorHtml: '', alternativeText: '', targetUrl: '', productCode: '', parentId: 'Main' }
				],
				parentId: undefined
			});
	}

	blade.testData();
	blade.initializeBlade();
}]);
