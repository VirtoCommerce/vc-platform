angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.blade = $scope.widget.blade;
	$scope.openBlade = function () {
        var blade = {
        	id: "dynamicPropertiesList",
        	currentEntity: $scope.blade.currentEntity,
            controller: 'platformWebApp.propertyValueListController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };


	$scope.$watch('widget.blade.currentEntity', function (entity) {
		if (angular.isDefined(entity)) {
			var groupedByProperty = _.groupBy(entity.dynamicProperties, function (x) { return x.id; });
			$scope.dynamicPropertyCount = _.keys(groupedByProperty).length;
		}
	});

}]);