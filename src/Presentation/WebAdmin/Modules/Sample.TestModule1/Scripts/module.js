//Call this to register our module to main application
var moduleTemplateName = "platformWebApp.testModule1";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [
	'testModule1.blades.blade1'
])
.config(['$stateProvider',
	function ($stateProvider) {
		$stateProvider
			.state('workspace.testModule1Template', {
				url: '/testModule1',
				templateUrl: 'Modules/Sample.TestModule1/Scripts/home/home.tpl.html',
				controller: [
					'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
						var blade = {
							id: 'blade1',
							// controller name must be unique in Application. Use prefix like 'um-'.
							controller: 'tm1-blade1Controller',
							template: 'Modules/Sample.TestModule1/Scripts/blades/blade1.tpl.html',
							isClosingDisabled: true
						};
						bladeNavigationService.showBlade(blade);
					}
				]
			});
	}
])
.run(['$rootScope', 'mainMenuService', '$state', function ($rootScope, mainMenuService, $state) {
	//Register module in main menu
	var menuItem = {
		path: 'browse/testModule1',
		icon: 'glyphicon glyphicon-search',
		title: 'Test Module 1',
		priority: 110,
		action: function () { $state.go('workspace.testModule1Template'); },
		permission: 'TestModule1Permission'
	};
	mainMenuService.addMenuItem(menuItem);
}]);