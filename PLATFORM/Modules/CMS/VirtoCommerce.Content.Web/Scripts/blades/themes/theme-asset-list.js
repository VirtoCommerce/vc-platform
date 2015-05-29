angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themeAssetListController', ['$scope', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, themes, themesStores, bladeNavigationService, dialogService) {
	var blade = $scope.blade;

	blade.selectedAssetId = undefined;

	blade.initialize = function () {
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
		var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });
		if (folder.isOpen) {
			folder.isOpen = false;
		}
		else {
			folder.isOpen = true;
		}
	}

	blade.checkFolder = function (data) {
		var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });

		return folder.isOpen;
	}

	blade.checkAsset = function (data) {
		return blade.selectedAssetId === data.id;
	}

	blade.getOneItemName = function (data) {
		var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });

		return folder.oneItemName;
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

	blade.openBlade = function (asset, data) {
		blade.selectedAssetId = asset.id;
		closeChildrenBlades();

		if (asset.contentType === 'text/html' || asset.contentType === 'application/json' || asset.contentType === 'application/javascript') {
			var newBlade = {
				id: 'editAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				choosenFolder: data.folderName,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit ' + asset.name,
				controller: 'virtoCommerce.contentModule.editAssetController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
		else {
			var newBlade = {
				id: 'editImageAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				choosenFolder: data.folderName,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit ' + asset.name,
				controller: 'virtoCommerce.contentModule.editImageAssetController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-image-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
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

	blade.openBladeNew = function(data) {
		closeChildrenBlades();

		var folder = blade.getFolder(data);
		var contentType = folder.defaultContentType;
		var name = folder.defaultItemName;

		if (contentType === 'text/html') {
			var newBlade = {
				id: 'addAsset',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenFolder: data.folderName,
				newAsset: true,
				currentEntity: { id: undefined, content: undefined, contentType: contentType, assetUrl: undefined, name: name },
				title: 'New ' + folder.oneItemName,
				subtitle: 'Create new ' + folder.oneItemName,
				controller: 'virtoCommerce.contentModule.editAssetController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
		else {
			var newBlade = {
				id: 'addImageAsset',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenFolder: data.folderName,
				newAsset: true,
				currentEntity: { id: undefined, content: undefined, contentType: undefined, assetUrl: undefined, name: undefined },
				title: 'New ' + folder.oneItemName,
				subtitle: 'Create new ' + folder.oneItemName,
				controller: 'virtoCommerce.contentModule.editImageAssetController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-image-asset.tpl.html'
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

	blade.getFolder = function (data) {
		var folder = _.find(blade.folders, function (folder) { return folder.name === data.folderName });
		
		if(folder !== undefined){
			return folder;
		}

		return null;
	}

    $scope.blade.headIcon = 'fa fa-archive';

    blade.folders = [
		{ name: 'assets', oneItemName: 'asset', defaultItemName: undefined, defaultContentType: null, isOpen: false },
		{ name: 'layout', oneItemName: 'layout', defaultItemName: 'new_layout.liquid', defaultContentType: 'text/html', isOpen: false },
		{ name: 'config', oneItemName: 'config', defaultItemName: 'new_config.json', defaultContentType: 'application/json', isOpen: false },
		{ name: 'locales', oneItemName: 'locale', defaultItemName: 'new_locale.json', defaultContentType: 'application/json', isOpen: false },
		{ name: 'snippets', oneItemName: 'snippet', defaultItemName: 'new_snippet.liquid', defaultContentType: 'text/html', isOpen: false },
		{ name: 'templates', oneItemName: 'template', defaultItemName: 'new_template.liquid', defaultContentType: 'text/html', isOpen: false }
    ];

	blade.initialize();
}]);
