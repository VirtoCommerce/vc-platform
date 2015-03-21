angular.module('virtoCommerce.content.themeModule.blades.themeList', [
    'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.resources.themesStores',
	'virtoCommerce.content.themeModule.blades.themeAssetList',
	'virtoCommerce.content.themeModule.blades.addTheme'
])
.controller('themesListController', ['$scope', 'themes', 'themesStores', 'bladeNavigationService', function ($scope, themes, themesStores, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		themes.get({ storeId: blade.storeId }, function (data) {
			blade.currentEntities = data;
			themesStores.get({ id: blade.storeId }, function (data) {
				blade.store = data;
				if (_.where(blade.store.settings, { name: "DefaultThemeName" }).length > 0) {
					blade.currentNodeName = _.where(blade.store.settings, { name: "DefaultThemeName" }).value;
				}
				blade.isLoading = false;
			});
		});
	}

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.name;

		var newBlade = {
			id: 'themeAssetListBlade',
			choosenThemeId: data.name,
			choosenStoreId: blade.storeId,
			choosenTheme: data,
			title: 'Edit ' + data.path,
			subtitle: 'Theme asset list',
			controller: 'themeAssetListController',
			template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/theme-asset-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.selectedTheme = function (data) {
		if (blade.currentNodeName === data.name) {
			return true;
		}
	}

	function openBladeNew() {
		$scope.selectedNodeId = null;

		var newBlade = {
			id: 'addTheme',
			choosenStoreId: blade.storeId,
			currentEntity: {},
			title: 'New theme asset',
			subtitle: 'Create new theme',
			controller: 'addThemeController',
			template: 'Modules/$(VirtoCommerce.Theme)/Scripts/blades/add-theme.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	$scope.bladeHeadIco = 'fa fa-archive';

	$scope.bladeToolbarCommands = [
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
