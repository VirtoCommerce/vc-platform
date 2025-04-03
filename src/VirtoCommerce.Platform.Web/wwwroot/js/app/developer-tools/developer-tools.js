angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.developer-tools', {
            url: '/developer-tools',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['platformWebApp.bladeNavigationService',
                function (bladeNavigationService) {
                    var newBlade = {
                        id: 'developer-tools',
                        controller: 'platformWebApp.developerToolsMainController',
                        template: '$(Platform)/Scripts/app/developer-tools/blades/developer-tools-main.html',
                    };
                    bladeNavigationService.showBlade(newBlade);
                }]
        });
    }])
    .factory('platformWebApp.developerTools',
        [function () {
            var _tools = [
                {
                    name: 'Swagger',
                    url: '/docs/index.html'
                },
                {
                    name: 'Hangfire',
                    url: '/hangfire'
                },
                {
                    name: 'Health',
                    url: '/health'
                },
            ];

            return {
                add: function (tool) {
                    _tools.push(tool);
                },
                getAll: function () {
                    return _tools;
                }
            };
        }]
    )
    .run(
        ['$state', 'platformWebApp.mainMenuService',
            function ($state, mainMenuService,) {
                var menuItem = {
                    path: 'configuration/developer-tools',
                    icon: 'fab fa-dev',
                    title: 'platform.menu.developer-tools',
                    priority: Number.MAX_SAFE_INTEGER - 1,
                    action: function () { $state.go('workspace.developer-tools'); },
                };
                mainMenuService.addMenuItem(menuItem);
            }]);
