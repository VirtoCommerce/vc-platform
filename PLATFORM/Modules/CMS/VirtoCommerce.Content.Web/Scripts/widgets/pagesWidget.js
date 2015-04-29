angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesWidgetController', ['$injector', '$rootScope', '$scope', 'virtoCommerce.contentModule.pages', 'bladeNavigationService', function ($injector, $rootScope, $scope, pages, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.refresh = function () {
		$scope.pagesCount = '...';
		return pages.query({ storeId: blade.currentEntityId }, function (data) {
			$scope.pagesCount = data.length;
		});
	}

	$scope.openBlade = function () {
		var newBlade = {
			id: "pagesListBlade",
			storeId: blade.currentEntityId,
			parentWidget: $scope.widget,
			title: blade.title,
			subtitle: 'Pages List',
			controller: 'virtoCommerce.contentModule.pagesListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/pages-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};

	$scope.widget.refresh();
}]);