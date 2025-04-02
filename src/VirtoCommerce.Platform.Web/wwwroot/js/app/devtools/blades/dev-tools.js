angular.module('platformWebApp')
    .controller('platformWebApp.devToolsController', [
        '$scope', 'platformWebApp.devToolsList', function ($scope, list) {
        var blade = $scope.blade;
        blade.title = 'DevTools';
        blade.isMaximized = true;
        blade.isLoading = false;

        $scope.currentUrl = null;
        $scope.currentName = null;
        $scope.items = [];

        var data = list.getAll();
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
    }])
    .factory('platformWebApp.devToolsList',
        [function () {
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
            
            return {
                add: function (tool) {
                    _devtools.push(tool);
                },
                getAll: function () {
                    return _devtools;
                }
            };
        }]
    );
