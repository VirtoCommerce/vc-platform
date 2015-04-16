angular.module('platformWebApp')
.controller('accountApiWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

   
    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            promise: $scope.blade.promise,
            title: $scope.blade.title,
            subtitle: 'API access',
            controller: 'accountApiController',
            template: 'Scripts/common/security/blades/account-api.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);