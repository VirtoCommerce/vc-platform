angular.module('platformWebApp').controller('platformWebApp.securityMainController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { name: 'platform.blades.account-list.title', entityName: 'account', subtitle: 'platform.blades.account-list.subtitle' },
            { name: 'platform.blades.role-list.title', entityName: 'role', subtitle: 'platform.blades.role-list.subtitle' },
            { name: 'platform.blades.oauthapps-list.title', entityName: 'oauthapps', subtitle: 'platform.blades.oauthapps-list.subtitle' }
        ];
        $scope.blade.currentEntities = entities;
        $scope.blade.isLoading = false;

        $scope.blade.openBlade(entities[0]);
    }

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.name;

        var newBlade = {
            id: 'securityDetails',
            title: data.name,
            subtitle: data.subtitle,
            controller: 'platformWebApp.' + data.entityName + 'ListController',
            template: '$(Platform)/Scripts/app/security/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.headIcon = 'fa-key';

    initializeBlade();
}]);
