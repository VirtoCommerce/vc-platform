const { first } = require("underscore");

angular.module('platformWebApp')
    .controller('platformWebApp.developerToolsMainController',
        ['$scope', '$sce', 'platformWebApp.developerTools', function ($scope, $sce, developerTools) {
            var blade = $scope.blade;
            blade.title = 'platform.blades.developer-tools.title';
            blade.headIcon = 'fab fa-dev';
            blade.hideToolbar = true;
            blade.isMaximized = true;
            blade.isLoading = true;

            $scope.currentUrl = null;
            $scope.currentName = null;
            $scope.items = [];

            developerTools.getAll().then(function (tools) {
                blade.isLoading = false;

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
                });

                // Show first internal tool by default
                if ($scope.items.length > 0 && !$scope.currentUrl) {
                    var firstInternalTool = $scope.items.find(function (tool) {
                        return !tool.isExternal;
                    });
                    if (firstInternalTool) {
                        $scope.currentUrl = $sce.trustAsResourceUrl(firstInternalTool.url);
                        $scope.currentName = firstInternalTool.name;
                    }
                }
            });
        }]);
