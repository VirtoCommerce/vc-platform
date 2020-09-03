angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('workspace.systemInfo', {
                url: '/systeminfo',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
                    var blade = {
                        id: 'versionInfo',
                        controller: 'platformWebApp.systemInfoController',
                        template: '$(Platform)/Scripts/app/systemInfo/blades/system-info.tpl.html',
                        isClosingDisabled: true
                    };
                    bladeNavigationService.showBlade(blade);
                }]
            });
    }]);
