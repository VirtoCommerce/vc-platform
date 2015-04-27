angular.module('virtoCommerce.content.themeModule.widgets.themesWidget', [
	'virtoCommerce.content.themeModule.resources.themes'
])
.controller('themesWidgetController', ['$injector', '$rootScope', '$scope', 'themes', 'bladeNavigationService', function ($injector, $rootScope, $scope, themes, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.refresh = function () {
		$scope.themesCount = '...';
		return themes.query({ storeId: blade.currentEntityId }, function (data) {
			$scope.themesCount = data.length;
		});
	}

	$scope.openBlade = function () {
		var newBlade = {
			id: "themesListBlade",
			storeId: blade.currentEntityId,
			parentWidget: $scope.widget,
			title: blade.title,
			subtitle: 'Themes List',
			controller: 'themesListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/themes-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};

	$scope.widget.refresh();
}]);
