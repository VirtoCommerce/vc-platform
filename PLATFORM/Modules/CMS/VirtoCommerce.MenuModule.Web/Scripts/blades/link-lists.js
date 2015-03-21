angular.module('virtoCommerce.content.menuModule.blades.linkLists', [
    'virtoCommerce.content.menuModule.resources.menus',
	'virtoCommerce.content.menuModule.resources.menusStores',
	'virtoCommerce.content.menuModule.blades.menuLinkList',
	'angularUUID2'
])
.controller('linkListsController', ['$scope', 'menus', 'menusStores', 'bladeNavigationService', function ($scope, menus, menusStores, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		menus.get({ storeId: blade.storeId }, function (data) {
			blade.currentEntities = data;
			blade.parentWidget.refresh();
			blade.isLoading = false;
		});
	}

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;
		closeChildrenBlades();

		var newBlade = {
			id: 'editMenuLinkListBlade',
			choosenStoreId: blade.storeId,
			choosenListId: data.id,
			newList: false,
			title: 'Edit ' + data.name + ' list',
			subtitle: 'Link list edit',
			controller: 'menuLinkListController',
			template: 'Modules/$(VirtoCommerce.Menu)/Scripts/blades/menu-link-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	function openBladeNew() {
		$scope.selectedNodeId = null;
		closeChildrenBlades();

		var newBlade = {
			id: 'addMenuLinkListBlade',
			choosenStoreId: blade.storeId,
			newList: true,
			title: 'Add new list',
			subtitle: 'Create new list',
			controller: 'menuLinkListController',
			template: 'Modules/$(VirtoCommerce.Menu)/Scripts/blades/menu-link-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	blade.getFlag = function (lang) {
		switch (lang) {
			case 'ru-RU':
				return 'ru';

			case 'en-US':
				return 'us';

			case 'fr-FR':
				return 'fr';

			case 'zh-CN':
				return 'ch';

			case 'ru-RU':
				return 'ru';

			case 'ja-JP':
				return 'ja';
		}
	}

	function closeChildrenBlades() {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	$scope.bladeHeadIco = 'fa fa-archive';

	$scope.bladeToolbarCommands = [
        {
        	name: "Add list", icon: 'fa fa-plus',
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
