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
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
  	//Register module in main menu
  	var menuItem = {
  	    path: 'browse/module1',
  	    icon: 'fa fa-cube',
  	    title: 'Module1',
  	    priority: 100,
  	    action: function () { $state.go('workspace.module1template') },
  		permission: 'module1Permission'
  	};
  	mainMenuService.addMenuItem(menuItem);
  }]);

