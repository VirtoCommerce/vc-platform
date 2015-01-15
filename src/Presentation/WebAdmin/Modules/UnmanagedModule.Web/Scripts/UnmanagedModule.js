//Call this to register our module to main application
var moduleTemplateName = "platformWebApp.unmanagedModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [
    'unmanagedModule.blades.blade1'
])
.config(
	['$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('workspace.unmanagedModuleTemplate', {
					url: '/unmanagedModule',
					templateUrl: 'Modules/UnmanagedModule.Web/Scripts/home/home.tpl.html',
					controller: [
						'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
							var blade = {
								id: 'blade1',
								// controller name must be unique in Application. Use prefix like 'um-'.
								controller: 'um-blade1Controller',
								template: 'Modules/UnmanagedModule.Web/Scripts/blades/blade1.tpl.html',
								isClosingDisabled: true
							};
							bladeNavigationService.showBlade(blade);
						}
					]
				});
		}
	]
)
.run(
	['$rootScope', 'mainMenuService', '$state', function ($rootScope, mainMenuService, $state) {
		//Register module in main menu
		var menuItem = {
			path: 'browse/unmanaged module',
			icon: 'fa fa-cube',
			title: 'Unmanaged Module',
			priority: 110,
			action: function () { $state.go('workspace.unmanagedModuleTemplate') },
			permission: 'UnmanagedModulePermission'
		};
		mainMenuService.addMenuItem(menuItem);
	}]);