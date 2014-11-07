//Call this to register our module to main application
var moduleTemplateName = "platformWebApp.module1";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [
    'platformWebApp.module1.blades.blade1'
])
.config(
  ['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
    	$stateProvider
            .state('workspace.module1template', {
            	url: '/module1Template',
            	templateUrl: 'Modules/Module1/VirtoCommerce.Module1.Web/Scripts/home/home.tpl.html',
            	controller: [
                    '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                    	var blade = {
                    		id: 'blade1',
                    		controller: 'blade1Controller',
                    		template: 'Modules/Module1/VirtoCommerce.Module1.Web/Scripts/blades/blade1.tpl.html',
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
  ['$rootScope', 'mainMenuService', 'widgetService', function ($rootScope, mainMenuService, widgetService) {
  	//Register module in main menu
  	var menuItem = {
  	    group: 'Browse',
  	    icon: 'icon-rocket',
  	    title: 'Module1',
  	    priority: 100,
  		state: 'workspace.module1template',
  		permission: 'module1Permission'
  	};
  	mainMenuService.addMenuItem(menuItem);
  }]);

