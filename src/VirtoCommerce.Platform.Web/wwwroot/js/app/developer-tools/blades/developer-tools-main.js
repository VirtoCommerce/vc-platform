angular.module('platformWebApp')
    .controller('platformWebApp.developerToolsMainController', ['$scope', 'platformWebApp.developerTools', function ($scope, developerTools) {
        var blade = $scope.blade;
        blade.title = 'platform.blades.developer-tools.title';
        blade.headIcon = 'fab fa-dev';
        blade.hideToolbar = true;
        blade.isMaximized = true;
        blade.isLoading = false;

        $scope.currentUrl = null;
        $scope.currentName = null;
        $scope.items = [];

        developerTools.getAll().then(function (tools) {
            $scope.items = tools.map(function (tool) {
                return {
                    name: tool.name,
                    url: document.location.origin + tool.url,
                    executeMethod: function (event) {
                        $scope.currentUrl = this.url;
                        $scope.currentName = tool.name;
                        event.preventDefault();
                        event.stopPropagation();
                        return false;
                    },
                };
            })
        });
    }]);
