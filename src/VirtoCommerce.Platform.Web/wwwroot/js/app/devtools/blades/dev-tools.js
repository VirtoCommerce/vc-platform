angular.module('platformWebApp')
    .controller('platformWebApp.devToolsController', ['$scope', 'platformWebApp.devToolsList', function ($scope, list) {
        var blade = $scope.blade;
        blade.title = 'DevTools';
        blade.isMaximized = true;
        blade.isLoading = false;

        $scope.currentUrl = null;
        $scope.currentName = null;
        $scope.items = [];

        list.getAll().then(function (data) {
            $scope.items = data.map(function (item) {
                return {
                    name: item.name, icon: item.icon,
                    url: document.location.origin + item.url,
                    executeMethod: function (event) {
                        $scope.currentUrl = this.url;
                        $scope.currentName = item.name;
                        event.preventDefault();
                        event.stopPropagation();
                        return false;
                    },
                };
            });
            console.log($scope.items);
        });

    }])
    .factory('platformWebApp.devToolsList',
        ['platformWebApp.modulesApi', '$q', function (modulesApi, $q) {
            var _devtools = [
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

            var _initialized = false;
            var _initPromise = null;

            function init() {
                if (_initialized) {
                    return _initPromise;
                }

                var deferred = $q.defer();


                // do it here, because it is the single thing why we should add scripts into the xapi module.
                modulesApi.get({}, function (result) {
                    var xapiModule = result.find(function (x) {
                        return x.id === 'VirtoCommerce.Xapi'
                    });
                    if (xapiModule) {
                        _devtools.push({
                            name: 'GraphQL',
                            url: '/ui/graphiql'
                        });
                    }
                    _initialized = true;
                    deferred.resolve(_devtools);
                }, function (error) {
                    deferred.reject(error);
                });

                _initPromise = deferred.promise;
                return _initPromise;
            }

            return {
                add: function (tool) {
                    _devtools.push(tool);
                },
                getAll: function () {
                    return init();
                }
            };
        }]
    );
