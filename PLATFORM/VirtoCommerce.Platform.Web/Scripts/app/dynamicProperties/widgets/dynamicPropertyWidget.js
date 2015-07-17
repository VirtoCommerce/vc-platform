angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.openBlade = function () {
        var blade = {
            id: "dynamicPropertiesList",
            title: $scope.blade.title,
            subtitle: 'Properties management',
            controller: 'platformWebApp.propertyValueListController',
            template: 'Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };

    $scope.$watch('blade.currentEntity.dynamicPropertyValues', function (values) {
        var groupedByProperty = _.groupBy(values, function (x) { return x.property.id; });
        $scope.dynamicPropertyCount = _.keys(groupedByProperty).length;
    });
}]);