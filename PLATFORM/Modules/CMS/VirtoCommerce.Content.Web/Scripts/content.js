//Call this to register our module to main application
var moduleName = "virtoCommerce.content.themeModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
	'virtoCommerce.content.themeModule.widgets.themesWidget',
	'virtoCommerce.content.themeModule.resources.themes',
	'virtoCommerce.content.themeModule.resources.themesStores',
	'virtoCommerce.content.themeModule.blades.themeAssetList',
	'virtoCommerce.content.themeModule.blades.themeList',
	'virtoCommerce.content.themeModule.blades.editAsset',
	'virtoCommerce.content.themeModule.blades.editImageAsset',
	'virtoCommerce.content.menuModule.widgets.menuWidget',
	'virtoCommerce.content.menuModule.resources.menus',
	'virtoCommerce.content.menuModule.resources.menusStores',
	'virtoCommerce.content.menuModule.blades.linkLists',
	'virtoCommerce.content.menuModule.blades.menuLinkList',
	'virtoCommerce.content.pagesModule.widgets.pagesWidget',
	'virtoCommerce.content.pagesModule.resources.pages',
	'virtoCommerce.content.pagesModule.blades.pagesList',
	'virtoCommerce.content.pagesModule.blades.editPage'
])
.run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {

	//Register widgets in store details
	widgetService.registerWidget({
		controller: 'themesWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themesWidget.tpl.html'
	}, 'storeDetail');

	widgetService.registerWidget({
		controller: 'menuWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/menuWidget.tpl.html'
	}, 'storeDetail');

	widgetService.registerWidget({
		controller: 'pagesWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/pagesWidget.tpl.html'
	}, 'storeDetail');

	}])
;
