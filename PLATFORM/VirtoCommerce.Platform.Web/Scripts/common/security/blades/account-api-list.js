angular.module('platformWebApp')
.controller('platformWebApp.accountApiListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        var newBlade = {
            subtitle: 'API access',
            origEntity: node,
            deleteFn: function (entry) {
                var idx = $scope.blade.currentEntities.indexOf(entry);
                if (idx >= 0) {
                    $scope.blade.currentEntities.splice(idx, 1);
                }
            }
        };
        openDetailsBlade(newBlade, node);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "accountApiDetail",
            title: $scope.blade.title,
            controller: 'platformWebApp.accountApiController',
            template: 'Scripts/common/security/blades/account-api.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeHeadIco = 'fa-lock';

    $scope.bladeToolbarCommands = [
       {
           name: "Add", icon: 'fa fa-plus',
           executeMethod: function () {
               $scope.blade.selectedData = undefined;
               var newBlade = {
                   subtitle: 'New API access',
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
           permission: 'platform:security:manage'
       }
    ];

    $scope.$watch('blade.parentBlade.currentEntity.apiAcounts', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.apiAcounts' gets fired
}]);