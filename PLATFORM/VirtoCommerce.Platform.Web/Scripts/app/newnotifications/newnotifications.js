var moduleName = "platformWebApp";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName)
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('workspace.newnotifications', {
			url: '/newnotifications',
			templateUrl: 'Scripts/common/templates/home.tpl.html',
			controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
				var blade = {
					id: 'newnotifications',
					title: 'Notifications',
					subtitle: 'Working with notifications system',
					controller: 'platformWebApp.notificationsMenuController',
					template: 'Scripts/app/newnotifications/blades/notifications-menu.tpl.html',
					isClosingDisabled: true
				};
				bladeNavigationService.showBlade(blade);
			}
			]
		});
  }]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
  	//Register module in main menu
  	var menuItem = {
  		path: 'browse/newnotifications',
  		icon: 'fa fa-cubes',
  		title: 'Notifications',
  		priority: 200,
  		action: function () { $state.go('workspace.newnotifications'); },
  		//permission: 'platform:module:query'
  	};
  	mainMenuService.addMenuItem(menuItem);
  }])
;