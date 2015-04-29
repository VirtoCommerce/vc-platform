//Call this to register our module to main application
var moduleName = "virtoCommerce.contentModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, ['angularUUID2'])
.run(['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {

	var menuItem = {
		path: 'browse/content',
		icon: 'fa fa-code',
		title: 'Content',
		priority: 111,
		action: function () { $state.go('workspace.content'); },
		permission: 'content:query'
	};
	mainMenuService.addMenuItem(menuItem);

	//Register widgets in store details
	widgetService.registerWidget({
		controller: 'virtoCommerce.contentModule.themesWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themesWidget.tpl.html'
	}, 'storeDetail');

	widgetService.registerWidget({
		controller: 'virtoCommerce.contentModule.menuWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/menuWidget.tpl.html'
	}, 'storeDetail');

	widgetService.registerWidget({
		controller: 'virtoCommerce.contentModule.pagesWidgetController',
		template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/pagesWidget.tpl.html'
	}, 'storeDetail');

}])
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
		.state('workspace.content', {
			url: '/content',
			templateUrl: 'Modules/$(VirtoCommerce.Content)/Scripts/home.tpl.html',
			controller: [
				'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'marketing',
						title: 'Marketing',
						controller: 'virtoCommerce.contentModule.ContentMainController',
						template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/common/content-main.tpl.html',
						isClosingDisabled: true
					};
					bladeNavigationService.showBlade(blade);
				}
			]
		});
}]);
