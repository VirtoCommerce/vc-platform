angular.module('platformWebApp')
.controller('platformWebApp.accountApiWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

   
    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            title: $scope.blade.title,
            subtitle: 'API keys',
            controller: 'platformWebApp.accountApiListController',
            template: 'Scripts/common/security/blades/account-api-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);