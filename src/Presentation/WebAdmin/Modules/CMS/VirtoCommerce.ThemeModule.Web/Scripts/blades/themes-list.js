angular.module('virtoCommerce.content.themeModule.blades.themeList', [
    'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.blades.themeAssetList'
])
.controller('themesListController', ['$scope', 'themes', 'bladeNavigationService', function ($scope, themes, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		themes.get({ storeId: blade.storeId }, function (data) {
			blade.isLoading = false;
			blade.currentEntities = data;
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
			template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/theme-asset-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	function openBladeNew() {
		$scope.selectedNodeId = null;

		var newBlade = {
			id: 'storeDetails',
			// currentEntityId: data.id,
			currentEntity: {},
			title: 'New theme asset',
			subtitle: 'Create new theme',
			controller: 'newThemeWizardController',
			template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/wizards/newTheme/new-theme-wizard.tpl.html'
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

	function setThemeAsActive() {

	}

	$scope.bladeHeadIco = 'fa fa-archive';

	$scope.bladeToolbarCommands = [
        {
        	name: "Add", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openBladeNew();
        	},
        	canExecuteMethod: function () {
        		return false;
        	}
        },
		{
			name: "Set Active", icon: 'fa fa-plus',
			executeMethod: function () {
				setThemeAsActive();
			},
			canExecuteMethod: function () {
				return false;
			}
		}
	];

	blade.refresh();
}]);
