angular.module('virtoCommerce.content.themesModule.widgets.themesWidget', [
	'virtoCommerce.content.themesModule.resources.themes'
])
.controller('themesWidgetController', ['$injector', '$rootScope', '$scope', 'themes', 'bladeNavigationService', function ($injector, $rootScope, $scope, themes, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;

	var themesList = themes.get({ id: $scope.currentBlade.currentEntityId });
}]);
