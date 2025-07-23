angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.embeddedApp', {
            url: '/embedded-app/:appId',
            templateUrl: '$(Platform)/Scripts/app/embedded-app/embedded-app.tpl.html',
            controller: [
                '$scope', '$stateParams', '$state', 'platformWebApp.mainMenuService',
                function ($scope, $stateParams, $state, mainMenuService) {
                    var appId = $stateParams.appId;
                    // Find menu item by id and appId
                    var menuItem = mainMenuService.appMenuItems.find(function(item) {
                        return item.id === appId;
                    });
                    if (menuItem && menuItem.relativeUrl) {
                        $scope.appRelativeUrl = `${menuItem.relativeUrl}?EmbeddedMode=true`;
                    } else {
                        $state.go('workspace', {}, { reload: true });
                    }
                }
            ]
        });
    }]);
