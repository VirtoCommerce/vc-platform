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

		var newBlade = {
			id: 'editAssetBlade',
			choosenStoreId: blade.choosenStoreId,
			choosenThemeId: blade.choosenThemeId,
			choosenAssetId: data.id,
			title: data.id,
			subtitle: 'Edit asset',
			controller: 'editAssetController',
			template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/blades/edit-asset.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	//function openBladeNew() {
	//	$scope.selectedNodeId = null;

	//	var newBlade = {
	//		id: 'storeDetails',
	//		// currentEntityId: data.id,
	//		currentEntity: {},
	//		title: 'New Store',
	//		subtitle: 'Create new Store',
	//		controller: 'newStoreWizardController',
	//		template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/wizards/newStore/new-store-wizard.tpl.html'
	//	};
	//	bladeNavigationService.showBlade(newBlade, $scope.blade);
	//}

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	//$scope.bladeToolbarCommands = [
    //    {
    //    	name: "Refresh", icon: 'fa fa-refresh',
    //    	executeMethod: function () {
    //    		$scope.blade.refresh();
    //    	},
    //    	canExecuteMethod: function () {
    //    		return true;
    //    	}
    //    },
    //    {
    //    	name: "Add", icon: 'fa fa-plus',
    //    	executeMethod: function () {
    //    		openBladeNew();
    //    	},
    //    	canExecuteMethod: function () {
    //    		return true;
    //    	}
    //    }
	//];

	blade.refresh();
}]);
