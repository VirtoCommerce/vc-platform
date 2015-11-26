angular.module('platformWebApp')
.controller('platformWebApp.accountApiListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        var newBlade = {
            subtitle: 'platform.blades.account-api.title',
            origEntity: node,
            deleteFn: function (entry) {
                var idx = $scope.blade.currentEntities.indexOf(entry);
                if (idx >= 0) {
                    $scope.blade.currentEntities.splice(idx, 1);
                }
            }
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "accountApiDetail",
            title: $scope.blade.title,
            controller: 'platformWebApp.accountApiController',
            template: '$(Platform)/Scripts/app/security/blades/account-api.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.headIcon = 'fa-key';

    $scope.blade.toolbarCommands = [
       {
           name: "platform.commands.add", icon: 'fa fa-plus',
           executeMethod: function () {
               $scope.blade.selectedData = undefined;
               var newBlade = {
                   subtitle: 'platform.blades.account-api.title-new',
                   isNew: true,
                   confirmChangesFn: function (entry) {
                       $scope.blade.currentEntities.push(entry);
                   },
               };
               openDetailsBlade(newBlade);
           },
           canExecuteMethod: function () {
               return true;
           },
           permission: 'platform:security:update'
       }
    ];

    $scope.$watch('blade.parentBlade.currentEntity.apiAccounts', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.apiAccounts' gets fired
}]);