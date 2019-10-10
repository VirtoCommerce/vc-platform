angular.module('platformWebApp')
.controller('platformWebApp.accountRolesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            promise: $scope.blade.promise,
            title: $scope.blade.title,
            subtitle: 'platform.widgets.accountRoles.blade-subtitle',
            controller: 'platformWebApp.accountRolesListController',
            template: '$(Platform)/Scripts/app/security/blades/account-roles-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);