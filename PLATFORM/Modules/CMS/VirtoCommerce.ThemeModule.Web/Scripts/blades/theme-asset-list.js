angular.module('virtoCommerce.content.themeModule.blades.themeAssetList', [
    'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.resources.themesStores',
	'virtoCommerce.content.themeModule.blades.editAsset',
	'virtoCommerce.content.themeModule.blades.editImageAsset'
])
.controller('themeAssetListController', ['$scope', 'themes', 'themesStores', 'bladeNavigationService', 'dialogService', function ($scope, themes, themesStores, bladeNavigationService, dialogService) {
	var blade = $scope.blade;

	blade.selectedFolderId = undefined;
	blade.selectedAssetId = undefined;
	blade.selectedFolder = undefined;

	blade.refresh = function () {
		blade.isLoading = true;
		themes.getAssets({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, function (data) {
			blade.currentEntities = data;
			themesStores.get({ id: blade.choosenStoreId }, function (data) {
				blade.store = data;
				blade.isLoading = false;
			});
		});
	}

	blade.folderClick = function (data) {
		//closeChildrenBlades();

		if (blade.checkFolder(data)) {
			blade.selectedFolderId = undefined;
		}
		else {
			blade.selectedFolderId = data.folderName;

			var button = _.find($scope.bladeToolbarCommands, function (button) { return button.name.indexOf('Add') === 0; });
			if (button !== undefined) {
				blade.selectedFolder = _.find(blade.folders, function (folder) { return folder.name === blade.selectedFolderId });
				button.name = 'Add ' + blade.selectedFolder.oneItemName;
			}
		}
	}

	blade.checkFolder = function (data) {
		return blade.selectedFolderId === data.folderName;
	}

	blade.checkAsset = function (data) {
		return blade.selectedAssetId === data.id;
	}

	blade.assetClass = function (asset) {
		switch(asset.contentType)
		{
			case 'text/html':
			case 'application/json':
			case 'application/javascript':
				return 'fa-file-text';

			case 'image/png':
			case 'image/jpeg':
			case 'image/bmp':
			case 'image/gif':
				return 'fa-image';

			default:
				return 'fa-file';
		}
	}

	blade.setThemeAsActive = function () {
		blade.isLoading = true;
		if (_.where(blade.store.settings, { name: "DefaultThemeName" }).length > 0) {
			angular.forEach(blade.store.settings, function (value, key) {
				if (value.name === "DefaultThemeName") {
					value.value = blade.choosenThemeId;
				}
			});
		}
		else {
			blade.store.settings.push({ name: "DefaultThemeName", value: blade.choosenThemeId, valueType: "ShortText" })
		}

		themesStores.update({ storeId: blade.choosenStoreId }, blade.store, function (data) {
			blade.isLoading = false;
		});
	}

	blade.openBlade = function (asset) {
		blade.selectedAssetId = asset.id;
		closeChildrenBlades();

		if (asset.contentType === 'text/html' || asset.contentType === 'application/json' || asset.contentType === 'application/javascript') {
			var newBlade = {
				id: 'editAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				choosenFolder: blade.selectedFolderId,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit ' + asset.name,
				controller: 'editAssetController',
				template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/edit-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
		else {
			var newBlade = {
				id: 'editImageAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				choosenFolder: blade.selectedFolderId,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit ' + asset.name,
				controller: 'editImageAssetController',
				template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/edit-image-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
	}

	blade.deleteTheme = function () {
		var dialog = {
			id: "confirmDelete",
			title: "Delete confirmation",
			message: "Are you sure want to delete " + blade.choosenThemeId + "?",
			callback: function (remove) {
				if (remove) {
					blade.isLoading = true;
					themes.deleteTheme({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, function (data) {
						$scope.blade.parentBlade.refresh(true);
						$scope.bladeClose();
						blade.isLoading = false;
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);
	}

	blade.folderSorting = function (entity) {
	    if (entity.folderName == "layout")
	        return 0;
	    else if (entity.folderName == "templates")
	        return 1;
	    else if (entity.folderName == "snippets")
	        return 2;
	    else if (entity.folderName == "assets")
	        return 3;
	    else if (entity.folderName == "config")
	        return 4;
	    else if (entity.folderName == "locales")
	        return 5;

        return 10;
    }

	function openBladeNew() {
		closeChildrenBlades();

		var contentType = blade.getContentType();

		if (contentType === 'text/html') {
			var newBlade = {
				id: 'addAsset',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenFolder: blade.selectedFolderId,
				newAsset: true,
				currentEntity: { id: undefined, content: undefined, contentType: blade.selectedFolder.defaultContentType, assetUrl: undefined, name: undefined },
				title: 'New ' + blade.selectedFolder.oneItemName,
				subtitle: 'Create new ' + blade.selectedFolder.oneItemName,
				controller: 'editAssetController',
				template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/edit-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
		else {
			var newBlade = {
				id: 'addImageAsset',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenFolder: blade.selectedFolderId,
				newAsset: true,
				currentEntity: { id: undefined, content: undefined, contentType: undefined, assetUrl: undefined, name: undefined },
				title: 'New ' + blade.selectedFolder.oneItemName,
				subtitle: 'Create new ' + blade.selectedFolder.oneItemName,
				controller: 'editImageAssetController',
				template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/edit-image-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
	}

	blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
		$scope.selectedNodeId = null;
	}

	blade.getContentType = function () {
		var folder = _.find(blade.folders, function (folder) { return folder.name === blade.selectedFolderId });
		
		if(folder !== undefined){
			return folder.defaultContentType;
		}

		return null;
	}

    $scope.bladeHeadIco = 'fa fa-archive';

    $scope.bladeToolbarCommands = [
		{
			name: "Set Active", icon: 'fa fa-pencil-square-o',
			executeMethod: function () {
				blade.setThemeAsActive();
			},
			canExecuteMethod: function () {
				return !angular.isUndefined(blade.choosenThemeId);
			}
		},
        {
        	name: "Refresh", icon: 'fa fa-refresh',
        	executeMethod: function () {
        		blade.refresh();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
        {
        	name: "Add", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openBladeNew();
        	},
        	canExecuteMethod: function () {
        		return !angular.isUndefined(blade.selectedFolderId);
        	}
        },
		{
			name: "Delete theme", icon: 'fa fa-trash-o',
			executeMethod: function () {
				blade.deleteTheme();
			},
			canExecuteMethod: function () {
				return !angular.isUndefined(blade.choosenThemeId);
			}
		}
    ];

    blade.folders = [
		{ name: 'assets', oneItemName: 'asset', defaultContentType: null },
		{ name: 'layout', oneItemName: 'layout', defaultContentType: 'text/html' },
		{ name: 'config', oneItemName: 'config', defaultContentType: 'text/html' },
		{ name: 'locales', oneItemName: 'locale', defaultContentType: 'text/html' },
		{ name: 'snippets', oneItemName: 'snippet', defaultContentType: 'text/html' },
		{ name: 'templates', oneItemName: 'template', defaultContentType: 'text/html' }
    ];

	blade.refresh();
}]);
