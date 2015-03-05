angular.module('virtoCommerce.content.menuModule.blades.linkLists', [
    'virtoCommerce.content.menuModule.resources.menus',
	'virtoCommerce.content.menuModule.blades.menuLinkList',
	'angularUUID2'
])
.controller('linkListsController', ['$scope', 'menus', 'bladeNavigationService', function ($scope, menus, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		menus.get({ storeId: blade.storeId }, function (data) {
			blade.isLoading = false;
			blade.currentEntities = data;
			blade.parentWidget.refresh();
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
			template: 'Modules/CMS/VirtoCommerce.MenuModule.Web/Scripts/blades/menu-link-list.tpl.html'
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
			template: 'Modules/CMS/VirtoCommerce.MenuModule.Web/Scripts/blades/menu-link-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

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
