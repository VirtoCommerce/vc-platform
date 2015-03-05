angular.module('virtoCommerce.content.themeModule.blades.themeAssetList', [
    'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.blades.editAsset'
])
.controller('themeAssetListController', ['$scope', 'themes', 'bladeNavigationService', function ($scope, themes, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		themes.getAssets({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, function (data) {
			blade.isLoading = false;
			blade.currentEntities = data;
		});
	}

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;
		closeChildrenBlades();

		var newBlade = {
			id: 'editAssetBlade',
			choosenStoreId: blade.choosenStoreId,
			choosenThemeId: blade.choosenThemeId,
			choosenAssetId: data.id,
			newAsset: false,
			title: data.id,
			subtitle: 'Edit asset',
			controller: 'editAssetController',
			template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-asset.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	function openBladeNew() {
		$scope.selectedNodeId = null;
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

    $scope.bladeHeadIco = 'fa fa-archive';

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
        		openBladeNew();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        }
	];

	blade.refresh();
}]);
