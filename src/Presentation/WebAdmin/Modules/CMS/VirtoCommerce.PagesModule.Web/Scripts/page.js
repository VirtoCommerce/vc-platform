//Call this to register our module to main application
var moduleName = "virtoCommerce.content.pagesModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, [
	'virtoCommerce.content.pagesModule.widgets.pagesWidget',
	'virtoCommerce.content.pagesModule.resources.pages',
	'virtoCommerce.content.pagesModule.blades.pagesList',
	'virtoCommerce.content.pagesModule.blades.editPage'
])
.run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {

	//Register widgets in store details
	widgetService.registerWidget({
		controller: 'pagesWidgetController',
		template: 'Modules/$(VirtoCommerce.Pages)/Scripts/widgets/pagesWidget.tpl.html'
	}, 'storeDetail');

}])
;