angular.module('virtoCommerce.customerModule.widgets')
.controller('memberPropertyWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "customerPropertyDetail",
            currentEntityId: $scope.blade.currentEntityId,
            //currentEntities: $scope.blade.currentEntity.properties,
            currentResource: $scope.blade.currentResource,
            title: $scope.blade.title,
            subtitle: 'Properties management',
            controller: 'memberPropertyListController',
            template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/member-property-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);