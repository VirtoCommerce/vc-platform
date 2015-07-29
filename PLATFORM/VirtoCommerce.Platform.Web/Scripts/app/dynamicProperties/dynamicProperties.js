angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
		$stateProvider
			.state('workspace.dynamicProperties', {
				url: '/dynamicProperties',
				templateUrl: 'Scripts/common/templates/home.tpl.html',
				controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'dynamicPropertiesTypes',
						controller: 'platformWebApp.dynamicObjectListController',
						template: 'Scripts/app/dynamicProperties/blades/dynamicObject-list.tpl.html',
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
  	var menuItem = {
  		path: 'configuration/dynamicProperties',
  		icon: 'fa fa-pencil-square-o',
  		title: 'Dynamic properties',
  		priority: 2,
  		action: function () { $state.go('workspace.dynamicProperties'); },
  		permission: 'platform:backupAdministrator'
  	};
  	mainMenuService.addMenuItem(menuItem);
  }]);