angular.module('virtoCommerce.customerModule.widgets')
.controller('customerPropertyWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "customerPropertyDetail",
            currentEntityId: $scope.blade.currentEntityId,
            currentEntity: $scope.blade.currentEntity,
            title: $scope.blade.title,
            subtitle: 'Properties',
            controller: 'customerPropertyController',
            template: 'Modules/customer/VirtoCommerce.customerModule.Web/Scripts/blades/customer-property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);