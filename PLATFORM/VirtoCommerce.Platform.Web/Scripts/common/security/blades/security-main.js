angular.module('platformWebApp')
.controller('securityMainController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { name: 'Roles', entityName: 'role' },
            { name: 'Users', entityName: 'account' }];
        $scope.blade.currentEntities = entities;
        $scope.blade.isLoading = false;

        $scope.blade.openBlade(entities[0]);
    };

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.name;

        var newBlade = {
            id: 'securityDetails',
            title: data.name,
            subtitle: 'Security service',
            controller: data.entityName + 'ListController',
            template: 'Scripts/common/security/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeHeadIco = 'fa fa-lock';

    initializeBlade();
}]);
