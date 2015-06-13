angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.menuWidgetController', ['$injector', '$rootScope', '$scope', 'virtoCommerce.contentModule.menus', 'platformWebApp.bladeNavigationService', function ($injector, $rootScope, $scope, menus, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.initialize = function () {
		$scope.meniLinkListsCount = '...';
		return menus.query({ storeId: blade.currentEntityId }, function (data) {
			$scope.meniLinkListsCount = data.length;
		}, function (error) {
		    //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
	}

	$scope.openBlade = function () {
		var newBlade = {
			id: "linkListBlade",
			storeId: blade.currentEntityId,
			parentWidget: $scope.widget,
			title: blade.title,
			subtitle: 'Link Lists',
			controller: 'virtoCommerce.contentModule.linkListsController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/link-lists.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};

	$scope.widget.initialize();
}]);

