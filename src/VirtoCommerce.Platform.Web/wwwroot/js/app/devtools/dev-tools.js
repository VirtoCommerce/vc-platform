angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.devtools', {
            url: '/devtools',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var newBlade = {
                    id: 'devTools',
                    controller: 'platformWebApp.devToolsController',
                    template: '$(Platform)/Scripts/app/devtools/blades/dev-tools.tpl.html',
                    title: 'DevTools',
                    headIcon: 'fab fa-dev',
                    hideToolbar: true
                };

                bladeNavigationService.showBlade(newBlade);
            }]
        });
    }])
    .run(
        ['$state', 'platformWebApp.mainMenuService',
            function ($state, mainMenuService,) {
                var menuItem = {
                    path: 'configuration/devTools',
                    icon: 'fab fa-dev',
                    title: 'platform.menu.dev-tools',
                    priority: 10,
                    action: function () { $state.go('workspace.devtools'); },
                };
                mainMenuService.addMenuItem(menuItem);
            }]);
