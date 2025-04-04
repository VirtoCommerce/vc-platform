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
        ['$http', function ($http) {
            var _tools = [];

            function isUrlAccessible(url) {
                return $http({
                    method: 'GET',
                    url: url
                }).then(function () {
                    return true;
                }).catch(function () {
                    return false;
                });
            }

            function addTool(tool) {
                var toolPromise = isUrlAccessible(tool.url).then(function (accessible) {
                    if (accessible) {
                        return tool;
                    } else {
                        console.warn('URL not accessible:', tool.url);
                        return null;
                    }
                });
                _tools.push(toolPromise);
            }

            addTool({ name: 'Swagger', url: '/docs/index.html' });
            addTool({ name: 'Hangfire', url: '/hangfire' });
            addTool({ name: 'Health', url: '/health' });

            return {
                add: addTool,
                getAll: function () {
                    return Promise.all(_tools).then(function (tools) {
                        return tools.filter(function (tool) { return tool !== null; });
                    });
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
                    permission: 'platform:developer-tools:access',
                };
                mainMenuService.addMenuItem(menuItem);
            }]);
