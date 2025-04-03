angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.developer-tools', {
            url: '/devtools',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['platformWebApp.bladeNavigationService',
                function (bladeNavigationService) {
                    var newBlade = {
                        id: 'developer-tools',
                        controller: 'platformWebApp.developerToolsController',
                        template: '$(Platform)/Scripts/app/developer-tools/blades/developer-tools.html',
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
                    path: 'configuration/developer-tools',
                    icon: 'fab fa-dev',
                    title: 'platform.menu.developer-tools',
                    priority: 10,
                    action: function () { $state.go('workspace.developer-tools'); },
                };
                mainMenuService.addMenuItem(menuItem);
            }]);
