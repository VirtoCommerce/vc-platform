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
            }])
    .factory('platformWebApp.developerTools',
        ['$http', '$q', function ($http, $q) {
            var _tools = [];

            var _initialized = false;
            var _initPromise = null;

            function init() {
                if (_initialized) {
                    return _initPromise;
                }

                var deferred = $q.defer();

                $http.get('/api/platform/developer-tools').then(function (result) {
                    for (var i = 0; i < result.data.length; i++) {
                        _tools.push(result.data[i]);
                    }
                    _initialized = true;
                    deferred.resolve(_tools);
                }).catch(function (error) {
                    console.log(error);
                    deferred.resolve(_tools);
                });

                _initPromise = deferred.promise;
                return _initPromise;
            }

            function addTool(tool) {
                _tools.push(tool);
            }

            return {
                add: addTool,
                getAll: function () {
                    return init();
                }
            };
        }]
    )    ;
