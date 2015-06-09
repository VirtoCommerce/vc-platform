angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themesWidgetController', ['$injector', '$rootScope', '$scope', 'virtoCommerce.contentModule.themes', 'platformWebApp.bladeNavigationService', function ($injector, $rootScope, $scope, themes, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.initialize = function () {
		$scope.themesCount = '...';
		return themes.query({ storeId: blade.currentEntityId }, function (data) {
			$scope.themesCount = data.length;
		}, function (error) {
		    //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
	}

	$scope.openBlade = function () {
		var newBlade = {
			id: "themesListBlade",
			storeId: blade.currentEntityId,
			parentWidget: $scope.widget,
			title: blade.title,
			subtitle: 'Themes List',
			controller: 'virtoCommerce.contentModule.themesListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/themes-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};

	$scope.widget.initialize();
}]);
