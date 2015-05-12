angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPhonesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            title: $scope.blade.title,
            subtitle: 'Manage customer phones',
            controller: 'virtoCommerce.customerModule.memberPhonesListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-phones-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);