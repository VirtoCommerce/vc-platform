angular.module('platformWebApp')
    .controller('platformWebApp.developerToolsMainController',
        ['$scope', '$sce', 'platformWebApp.developerTools', function ($scope, $sce, developerTools) {
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
                    url: tool.url,
                    isExternal: tool.isExternal,
                    executeMethod: function (event) {
                        if (!tool.external) {
                            event.preventDefault();
                            event.stopPropagation();
                            $scope.currentUrl = $sce.trustAsResourceUrl(this.url);
                            $scope.currentName = tool.name;
                            return false;
                        }
                        return true;
                    },
                };
            })
        });
    }]);
