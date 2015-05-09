angular.module('platformWebApp')
.controller('platformWebApp.accountApiWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

   
    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            title: $scope.blade.title,
            subtitle: 'API access',
            controller: 'platformWebApp.accountApiController',
            template: 'Scripts/common/security/blades/account-api.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);