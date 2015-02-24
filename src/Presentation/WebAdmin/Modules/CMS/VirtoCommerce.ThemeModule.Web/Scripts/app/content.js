//Call this to register our module to main application
var moduleName = "virtoCommerce.content.themesModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
	'virtoCommerce.content.themesModule.widgets.themesWidget'
])
.run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {

	//Register widgets in store details
	widgetService.registerWidget({
		group: 'storeDetail',
		controller: 'themesWidgetController',
		template: 'Modules/Store/VirtoCommerce.ThemeModule.Web/Scripts/widgets/themesWidget.tpl.html'
	});

	}])
;
