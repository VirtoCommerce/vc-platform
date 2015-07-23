angular.module('platformWebApp')
.controller('platformWebApp.dynamicObjectListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dynamicPropertiesApi) {
	var blade = $scope.blade;

	blade.refresh = function () {
		dynamicPropertiesApi.queryTypes(function (results) {
			results = _.map(results, function (x) { return { name: x }; });
			blade.currentEntities = results;
			blade.isLoading = false;
		}, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	};

	$scope.selectNode = function (node) {
		$scope.selectedNodeId = node.name;

		var newBlade = {
			id: 'dynamicPropertyList',
			objectType: node.name,
			controller: 'platformWebApp.dynamicPropertyListController',
			template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.headIcon = 'fa-plus-square-o';
	blade.title = 'Dynamic object types',
    blade.subtitle = 'Pick object type to manage dynamic properties',
	blade.refresh();
}]);
