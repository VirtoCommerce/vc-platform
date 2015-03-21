angular.module('virtoCommerce.customerModule.widgets')
.controller('memberEmailsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            //data: $scope.blade.currentEntity.emails,
            title: $scope.blade.title,
            subtitle: 'Manage customer emails',
            controller: 'memberEmailsListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-emails-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);