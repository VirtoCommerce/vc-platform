//Call this to register our module to main application
var moduleName = "virtoCommerce.contentModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.bladeNavigationService', 'platformWebApp.permissionScopeResolver', 'virtoCommerce.storeModule.stores',
	function ($rootScope, mainMenuService, widgetService, $state, bladeNavigationService, scopeResolver, stores) {

		var menuItem = {
			path: 'browse/content',
			icon: 'fa fa-code',
			title: 'content.main-menu-title',
			priority: 111,
			action: function () { $state.go('workspace.content'); },
			permission: 'content:access'
		};
		mainMenuService.addMenuItem(menuItem);

		
		widgetService.registerWidget({
			controller: 'virtoCommerce.contentModule.themesWidgetController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/widgets/themesWidget.tpl.html',
			permission: 'content:read'
		}, 'storeDetail');

		//Register permission scopes templates used for scope bounded definition in role management ui
		var selectedStoreScope = {
			type: 'ContentSelectedStoreScope',
			title: 'Only for selected stores',
			selectFn: function (blade, callback) {
				var newBlade = {
					id: 'store-pick',
					title: 'content.blades.scope-value-pick-from-simple-list.title',
					subtitle: 'content.blades.scope-value-pick-from-simple-list.subtitle',
					currentEntity: this,
					onChangesConfirmedFn: callback,
					dataPromise: stores.query().$promise,
					controller: 'platformWebApp.security.scopeValuePickFromSimpleListController',
					template: '$(Platform)/Scripts/app/security/blades/common/scope-value-pick-from-simple-list.tpl.html'
				};
				bladeNavigationService.showBlade(newBlade, blade);
			}
		};
		scopeResolver.register(selectedStoreScope);
	}])
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
		.state('workspace.content', {
			url: '/content?storeId',
			templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
			controller: [
				'$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'content',
						title: 'content.blades.content-main.title',
						subtitle: 'content.blades.content-main.subtitle',
						controller: 'virtoCommerce.contentModule.contentMainController',
						template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/common/content-main.tpl.html',
						isClosingDisabled: true
					};
					bladeNavigationService.showBlade(blade);
				}
			]
		});
}]);
