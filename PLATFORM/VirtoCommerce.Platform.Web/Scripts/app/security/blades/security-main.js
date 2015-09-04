angular.module('platformWebApp')
.controller('platformWebApp.securityMainController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { name: 'Users', entityName: 'account' },
            { name: 'Roles', entityName: 'role' }
        ];
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
            controller: 'platformWebApp.' + data.entityName + 'ListController',
            template: '$(Platform)/Scripts/app/security/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.headIcon = 'fa-key';

    initializeBlade();
}]);
