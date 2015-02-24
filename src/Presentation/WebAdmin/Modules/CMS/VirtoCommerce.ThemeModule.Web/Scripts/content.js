//Call this to register our module to main application
var moduleName = "virtoCommerce.content.themeModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
	'virtoCommerce.content.themeModule.widgets.themesWidget',
	'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.blades.themeAssetList',
	'virtoCommerce.content.themeModule.blades.themeList',
	'virtoCommerce.content.themeModule.blades.editAsset'
])
.run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {

	//Register widgets in store details
	widgetService.registerWidget({
		controller: 'themesWidgetController',
		template: 'Modules/CMS/VirtoCommerce.ThemeModule.Web/Scripts/widgets/themesWidget.tpl.html'
	}, 'storeDetail');

	}])
;
