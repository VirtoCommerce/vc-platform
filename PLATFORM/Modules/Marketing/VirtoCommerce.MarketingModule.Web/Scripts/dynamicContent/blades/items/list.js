angular.module('virtoCommerce.marketingModule')
.controller('itemsDynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.choosenFolder = undefined;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	function initializeBlade() {
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

	blade.closeChildrenBlades = function() {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	blade.folderClick = function (data) {
		if (angular.isUndefined(blade.currentEntity) || angular.equals(blade.currentEntity, data)) {
			blade.choosenFolder = data.id;
			blade.currentEntity = data;
		}
		else {
			blade.choosenFolder = undefined;
			blade.currentEntity = undefined;
		}
	}

	blade.checkFolder = function (data) {
		return angular.equals(data, blade.currentEntity);
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
	];

	$scope.bladeHeadIco = 'fa fa-flag';

	initializeBlade();
}]);
