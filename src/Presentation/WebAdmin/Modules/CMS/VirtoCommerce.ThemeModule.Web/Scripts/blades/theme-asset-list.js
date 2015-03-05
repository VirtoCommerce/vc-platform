angular.module('virtoCommerce.content.themeModule.blades.themeAssetList', [
    'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.blades.editAsset',
	'virtoCommerce.content.themeModule.blades.editImageAsset'
])
.controller('themeAssetListController', ['$scope', 'themes', 'bladeNavigationService', function ($scope, themes, bladeNavigationService) {
	var blade = $scope.blade;

	$scope.selectedFolderId = null;
	$scope.selectedAssetId = null;

	blade.refresh = function () {
		blade.isLoading = true;
		themes.getAssets({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, function (data) {
			blade.isLoading = false;
			blade.currentEntities = data;
		});
	}

	blade.folderClick = function (data) {
		//closeChildrenBlades();

		if (blade.checkFolder(data)) {
			$scope.selectedFolderId = null;
		}
		else {
			$scope.selectedFolderId = data.folderName;
		}
	}

	blade.checkFolder = function (data) {
		return $scope.selectedFolderId === data.folderName;
	}

	blade.checkAsset = function (data) {
		return $scope.selectedAssetId === data.id;
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

	blade.openBlade = function (asset) {
		$scope.selectedAssetId = asset.id;
		closeChildrenBlades();

		if (asset.contentType === 'text/html' || asset.contentType === 'application/json' || asset.contentType === 'application/javascript') {
			var newBlade = {
				id: 'editAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit text asset',
				controller: 'editAssetController',
				template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
		else {
			var newBlade = {
				id: 'editImageAssetBlade',
				choosenStoreId: blade.choosenStoreId,
				choosenThemeId: blade.choosenThemeId,
				choosenAssetId: asset.id,
				newAsset: false,
				title: asset.id,
				subtitle: 'Edit image asset',
				controller: 'editImageAssetController',
				template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-image-asset.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, blade);
		}
	}

	function openBladeNew(data) {
		$scope.selectedAssetId = data.id;
		closeChildrenBlades();

		var newBlade = {
			id: 'addAsset',
			choosenStoreId: blade.choosenStoreId,
			choosenThemeId: blade.choosenThemeId,
			newAsset: true,
			currentEntity: { id: null, content: null },
			title: 'New Asset',
			subtitle: 'Create new text asset',
			controller: 'editAssetController',
			template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-asset.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);

		//var newBlade = {
		//	id: 'addImageAsset',
		//	choosenStoreId: blade.choosenStoreId,
		//	choosenThemeId: blade.choosenThemeId,
		//	newAsset: true,
		//	currentEntity: { id: null, content: null },
		//	title: 'New Asset',
		//	subtitle: 'Create new image asset',
		//	controller: 'editImageAssetController',
		//	template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-image-asset.tpl.html'
		//};
		//bladeNavigationService.showBlade(newBlade, blade);
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
        	name: "Add text asset", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openBladeNew();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
		{
			name: "Add image asset", icon: 'fa fa-plus',
			executeMethod: function () {
				openBladeNew();
			},
			canExecuteMethod: function () {
				return false;
			}
		}
	];

	blade.refresh();
}]);
