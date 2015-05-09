angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "customerPropertyDetail",
            currentEntityId: $scope.blade.currentEntityId,
            //currentEntities: $scope.blade.currentEntity.properties,
            currentResource: $scope.blade.currentResource,
            title: $scope.blade.title,
            subtitle: 'Properties management',
            controller: 'virtoCommerce.customerModule.memberPropertyListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);