angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dynamicPropertiesApi) {
	$scope.blade = $scope.widget.blade;
	$scope.openBlade = function () {
        var blade = {
        	id: "dynamicPropertiesList",
        	currentEntity: $scope.blade.currentEntity,
            controller: 'platformWebApp.propertyValueListController',
			template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html',
			dynamicPropertyCount: $scope.dynamicPropertyCount
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };


	$scope.$watch('widget.blade.currentEntity', function (entity) {
		if (angular.isDefined(entity)) {
			dynamicPropertiesApi.search({objectType: entity.objectType, take: 0}, function(response) {
				$scope.dynamicPropertyCount = response.totalCount;
			})
			
		}
	});

}]);