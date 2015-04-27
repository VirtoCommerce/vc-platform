angular.module('virtoCommerce.content.menuModule.widgets.menuWidget', [
	'virtoCommerce.content.menuModule.resources.menus',
	'virtoCommerce.content.menuModule.blades.linkLists'
])
.controller('menuWidgetController', ['$injector', '$rootScope', '$scope', 'menus', 'bladeNavigationService', function ($injector, $rootScope, $scope, menus, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.refresh = function () {
		$scope.meniLinkListsCount = '...';
		return menus.query({ storeId: blade.currentEntityId }, function (data) {
			$scope.meniLinkListsCount = data.length;
		});
	}

	$scope.openBlade = function () {
		var newBlade = {
			id: "linkListBlade",
			storeId: blade.currentEntityId,
			parentWidget: $scope.widget,
			title: blade.title,
			subtitle: 'Link Lists',
			controller: 'linkListsController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/link-lists.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};

	$scope.widget.refresh();
}]);

