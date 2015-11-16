angular.module('platformWebApp')
.controller('platformWebApp.accountApiWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

   
    $scope.openBlade = function () {
        var newBlade = {
            id: "accountChildBlade",
            title: $scope.blade.title,
            subtitle: 'platform.widgets.accountApi.blade-subtitle',
            controller: 'platformWebApp.accountApiListController',
            template: '$(Platform)/Scripts/app/security/blades/account-api-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);