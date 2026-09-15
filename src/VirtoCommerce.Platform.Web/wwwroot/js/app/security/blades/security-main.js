angular.module('platformWebApp').controller('platformWebApp.securityMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', function ($scope, bladeNavigationService, authService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { name: 'platform.blades.account-list.title', entityName: 'account', subtitle: 'platform.blades.account-list.subtitle' },
            { name: 'platform.blades.role-list.title', entityName: 'role', subtitle: 'platform.blades.role-list.subtitle' },
            { name: 'platform.blades.oauthapps-list.title', entityName: 'oauthapps', subtitle: 'platform.blades.oauthapps-list.subtitle' },
            {
                name: 'platform.blades.sign-in-log-dashboard.title',
                entityName: 'sign-in-log',
                subtitle: 'platform.blades.sign-in-log-dashboard.subtitle',
                // Naming does not follow the <entityName>ListController convention, so state it explicitly.
                controller: 'platformWebApp.signInLogDashboardController',
                template: '$(Platform)/Scripts/app/security/blades/sign-in-log-dashboard.html',
                permission: 'platform:security:sign_in_log:read'
            }
        ];
        $scope.blade.currentEntities = entities.filter(function (entity) {
            return !entity.permission || authService.checkPermission(entity.permission);
        });
        $scope.blade.isLoading = false;

        $scope.blade.openBlade($scope.blade.currentEntities[0]);
    }

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.name;

        var newBlade = {
            id: 'securityDetails',
            title: data.name,
            subtitle: data.subtitle,
            controller: data.controller || ('platformWebApp.' + data.entityName + 'ListController'),
            template: data.template || ('$(Platform)/Scripts/app/security/blades/' + data.entityName + '-list.tpl.html')
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.headIcon = 'fas fa-key';

    initializeBlade();
}]);
