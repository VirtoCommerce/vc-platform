angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    $scope.openBlade = function () {
        var blade = {
            id: "memberProperties",
            title: $scope.blade.title,
            subtitle: 'Properties management',
            controller: 'virtoCommerce.customerModule.memberPropertyListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.blade);
    };
}]);