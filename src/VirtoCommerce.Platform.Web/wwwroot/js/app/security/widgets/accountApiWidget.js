angular.module('platformWebApp')
.controller('platformWebApp.accountApiWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

   
    $scope.openBlade = function () {
        var newBlade = {
            id: "accountApiBlade",
            title: $scope.blade.title,
            user: $scope.blade.currentEntity,
            subtitle: 'platform.widgets.accountApi.blade-subtitle',
            controller: 'platformWebApp.accountApiController',
            template: '$(Platform)/Scripts/app/security/blades/account-api.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);
