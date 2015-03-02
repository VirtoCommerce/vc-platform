angular.module('virtoCommerce.customerModule.widgets')
.controller('customerPropertyWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "customerPropertyDetail",
            currentEntityId: $scope.blade.currentEntityId,
            //currentEntities: $scope.blade.currentEntity.properties,
            title: $scope.blade.title,
            subtitle: 'Properties management',
            controller: 'customerPropertyListController',
            template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/customer-property-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);