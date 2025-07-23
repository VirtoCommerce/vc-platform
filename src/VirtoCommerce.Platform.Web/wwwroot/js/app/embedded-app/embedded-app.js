angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.embeddedApp', {
            url: '/embedded-app/:appId',
            templateUrl: '$(Platform)/Scripts/app/embedded-app/embedded-app.tpl.html',
            controller: [
                '$scope', '$stateParams', '$state', 'platformWebApp.webApps',
                function ($scope, $stateParams, $state, webApps) {
                    var appId = $stateParams.appId;
                    // Find menu item by id and appId
                    var app = webApps.apps.find(function(item) {
                        return item.id === appId;
                    });
                    if (app && app.relativeUrl) {
                        $scope.appRelativeUrl = `${app.relativeUrl}#?EmbeddedMode=true`;
                    } else {
                        $state.go('workspace', {}, { reload: true });
                    }
                }
            ]
        });
    }]);
