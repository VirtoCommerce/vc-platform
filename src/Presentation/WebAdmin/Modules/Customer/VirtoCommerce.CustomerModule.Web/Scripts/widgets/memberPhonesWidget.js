angular.module('virtoCommerce.customerModule.widgets')
.controller('memberPhonesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            title: $scope.blade.title,
            subtitle: 'Manage customer phones',
            controller: 'memberPhonesListController',
            template: 'Modules/customer/VirtoCommerce.customerModule.Web/Scripts/blades/member-phones-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);