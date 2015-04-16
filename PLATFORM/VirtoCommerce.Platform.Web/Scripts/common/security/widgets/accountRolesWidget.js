angular.module('platformWebApp')
.controller('accountRolesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            promise: $scope.blade.promise,
            title: $scope.blade.title,
            subtitle: 'Manage roles',
            controller: 'accountRolesController',
            template: 'Scripts/common/security/blades/account-roles.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);