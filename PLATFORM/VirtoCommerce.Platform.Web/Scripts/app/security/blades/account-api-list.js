angular.module('platformWebApp')
.controller('platformWebApp.accountApiListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:security:update';

    function initializeBlade(data) {
        blade.currentEntities = data;
        blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        var newBlade = {
            subtitle: 'platform.blades.account-api.title',
            origEntity: node,
            deleteFn: function (entry) {
                var idx = blade.currentEntities.indexOf(entry);
                if (idx >= 0) {
                    blade.currentEntities.splice(idx, 1);
                }
            }
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "accountApiDetail",
            title: blade.title,
            controller: 'platformWebApp.accountApiController',
            template: '$(Platform)/Scripts/app/security/blades/account-api.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.headIcon = 'fa-key';

    blade.toolbarCommands = [
       {
           name: "platform.commands.add", icon: 'fa fa-plus',
           executeMethod: function () {
               blade.selectedData = undefined;
               var newBlade = {
                   subtitle: 'platform.blades.account-api.title-new',
                   isNew: true,
                   confirmChangesFn: function (entry) {
                       blade.currentEntities.push(entry);
                   },
               };
               openDetailsBlade(newBlade);
           },
           canExecuteMethod: function () {
               return true;
           },
           permission: blade.updatePermission
       }
    ];

    $scope.$watch('blade.parentBlade.currentEntity.apiAccounts', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.apiAccounts' gets fired
}]);