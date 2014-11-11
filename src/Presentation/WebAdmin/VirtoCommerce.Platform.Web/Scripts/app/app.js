var AppDependencies = [
  'ui.router',
  'ui.bootstrap',
  'ui.utils',
  'ui.sortable',
  'ngAnimate',
  'ngResource',
  'xeditable',
  'platformWebApp.dashboard',
  'platformWebApp.security',
  'platformWebApp.notification',
  'platformWebApp.mainMenu',
  'platformWebApp.bladeNavigation',
  'platformWebApp.overlay',
  'platformWebApp.contextMenu',
  'platformWebApp.Filters',
  'platformWebApp.htmlTooltip',
  'platformWebApp.widget'
];

angular.module('platformWebApp', AppDependencies)
.controller('appCtrl', ['$scope', '$state', 'mainMenuService', 'notificationService', function ($scope, $state, mainMenuService, notificationService) {
    $scope.menuItems = mainMenuService.menuItems;
    $scope.alerts = notificationService.getAllAlerts();
    $scope.closeAlert = notificationService.remove;
    $scope.curentStateName = function () {
        return $state.current.name;
    };
}])
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('workspace', {
			abstract: true,
			templateUrl: 'Scripts/app/workspace.tpl.html'
		});
  }
  ]
)
.run(
  ['$rootScope', '$state', '$stateParams', 'authService', 'mainMenuService', 'editableOptions',
    function ($rootScope, $state, $stateParams, authService, mainMenuService, editableOptions) {
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'

        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        var homeMenuItem = {
        	group: 'Home',
        	state: mainMenuService.menuItems[0].state,
        	icon: 'glyphicon glyphicon-home',
        	priority: 0
        };
        var notificationMenuItem = {
        	group: 'Notification',
        	state: mainMenuService.menuItems[0].state,
        	icon: 'glyphicon glyphicon-comment',
        	priority: 1
        };
        var journeyMenuItem = {
        	group: 'Active',
        	state: mainMenuService.menuItems[0].state,
        	icon: 'glyphicon glyphicon-tasks',
        	priority: 999
        };
       
        mainMenuService.addMenuItem(homeMenuItem);
        mainMenuService.addMenuItem(notificationMenuItem);
        mainMenuService.addMenuItem(journeyMenuItem);


        $rootScope.$on('unauthorized', function (event, rejection) {
            $state.go('loginDialog');
        });

        $rootScope.$on('loginStatusChanged', function (event, authContext) {
            if (authContext.isAuthenticated) {
                $state.go(homeMenuItem.state);
            }
            else {
                $state.go('loginDialog');
            }
        });

        authService.fillAuthData();
    }
  ]
);
