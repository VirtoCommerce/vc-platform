angular.module('virtoCommerce.customerModule.widgets')
.controller('customerEmailsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            //data: $scope.blade.currentEntity.emails,
            title: $scope.blade.title,
            subtitle: 'Manage customer emails',
            controller: 'customerEmailsListController',
            template: 'Modules/customer/VirtoCommerce.customerModule.Web/Scripts/blades/customer-emails-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);