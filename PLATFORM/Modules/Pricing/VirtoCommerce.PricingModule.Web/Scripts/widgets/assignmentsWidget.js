angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.assignmentsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "pricelistChild",
            // currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Manage assigned catalogs',
            controller: 'virtoCommerce.pricingModule.pricelistAssignmentListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-assignment-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };
}]);
